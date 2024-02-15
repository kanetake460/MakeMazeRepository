using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using static TestMap;

// 参考サイト
// https://walkable-2020.com/unity/a-star-pathfinding/
// https://www.sejuku.net/blog/47172
// https://www.sejuku.net/blog/72918
// https://qiita.com/toRisouP/items/98cc4966d392b7f21c30
namespace TakeshiClass
{
    public class AStar
    {
        public class CellInfo
        {
            public Vector3Int position { get; set; }       // 親セルのグリッド座標
            public float cost { get; set; }                 // 実コスト
            public float heuristicCost { get; set; }        // 推定コスト
            public float sumCost { get; set; }              // 総コスト
            public CellInfo parent { get; set; }          // 親のセルの位置
            public bool isOpen { get; set; }                // 調査対象かどうか

        }

        GridField m_GridField;
        TestMap.Map m_Map;
        private List<CellInfo> openList = new List<CellInfo>();
        private List<CellInfo> closeList = new List<CellInfo>();
        private Vector3Int goal;
        private Vector3Int start;
        public Stack<CellInfo> pathStack { get; } = new Stack<CellInfo>();

        /// <summary>
        /// AStarのコンストラクタ(与えたグリッドフィールドマップでA*探索を行います)
        /// </summary>
        /// <param name="gf">グリッド座標</param>
        /// <param name="position"></param>
        /// <param name="targetPos"></param>
        public AStar(GridField gf, TestMap.Map map, Vector3Int position, Vector3Int targetPos)
        {
            m_GridField = gf;
            m_Map = map;
            start = position;
            goal = targetPos;

            CellInfo startCell = new CellInfo();
            startCell.position = start;
            startCell.cost = 0;
            startCell.heuristicCost = Vector3Int.Distance(start, goal);
            startCell.sumCost = startCell.cost + startCell.heuristicCost;
            startCell.isOpen = true;

            openList.Add(startCell);

        }

        /// <summary>
        /// 経路を探索します
        /// </summary>
        public void AStarPath()
        {
            int count = 0;
            CellInfo minCell = openList.First();
            while (count < 100000)
            {

                minCell = SearchMinCell();  // リストの中から総コストが最も低いものを格納

                OpenCell(minCell);          // 最も低いセルの周りをオープンにする

                count++;

                if (minCell.position == goal) break;    // 最も低いセルのポジションがゴールならループ終了
            }
            // ゴールにたどり着いたセルから順にたどってスタートまでの道のりをスタック
            StackPath(minCell);
        }


        /// <summary>
        /// 真ん中の周りのセルをオープンにします
        /// </summary>
        /// <param name="center">真ん中に設定するセル</param>
        public void OpenCell(CellInfo center)
        {
            Vector3Int centerPos = center.position;

            // 上下左右、真ん中は含めない
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x != 0 && z != 0) continue;

                    if (x == 0 && z == 0) continue;

                    Vector3Int searchPos = new Vector3Int(centerPos.x + x, centerPos.y, centerPos.z + z);

                    // マップの外ならオープンしない
                    if (searchPos.x >= m_Map.mapWidth ||
                        searchPos.z >= m_Map.mapDepth ||
                        searchPos.x < 0 || searchPos.z < 0) continue;

                    // マップの壁マスならオープンしない
                    if (m_Map.blocks[searchPos.x, searchPos.z].type == TestMap.Map.BlockType.eWall) continue;
                    Debug.Log("壁じゃない");

                    CellInfo aroundCell = new CellInfo();

                    // 周りのセルに情報を入れる
                    aroundCell.position = searchPos;
                    aroundCell.cost = center.cost + 1;
                    aroundCell.heuristicCost = Vector3Int.Distance(searchPos, goal);
                    aroundCell.sumCost = aroundCell.cost + aroundCell.heuristicCost;
                    aroundCell.parent = center;
                    aroundCell.isOpen = true;

                    // 両方のリストに存在しない
                    if (openList.All(x => x == null) && closeList.All(x => x == null))
                    {
                        openList.Add(aroundCell);
                        continue;
                    }

                    // オープンリストに存在して、合計コストがオープンしたいセルより大きい
                    if (openList.Where(x => x.position == searchPos).First().sumCost > aroundCell.sumCost)
                    {
                        var o = openList.Where(x => x.position == searchPos).First();
                        openList.Remove(o);
                        openList.Add(aroundCell);
                        continue;
                    }

                    // クローズリストに存在して、合計コストがオープンしたいセルより大きい
                    if(closeList.Where(x => x.position == searchPos).First().sumCost > aroundCell.sumCost)
                    {
                        var c = closeList.Where(y => y.position == searchPos).First();
                        closeList.Remove(c);
                        openList.Add(aroundCell);
                        continue;
                    }



                    //openList.Where(x => x.position == searchPos).First() == null);       // 
                    // 親のポジションでない、センターでない、親がない
                    if (center.parent == null
                        || searchPos != center.parent.position && searchPos != center.position)
                    {
                        CellInfo aroundCell = new CellInfo();

                        // 周りのセルに情報を入れる
                        aroundCell.position = searchPos;
                        aroundCell.cost = center.cost + 1;
                        aroundCell.heuristicCost = Vector3Int.Distance(searchPos, goal);
                        aroundCell.sumCost = aroundCell.cost + aroundCell.heuristicCost;
                        aroundCell.parent = center;
                        aroundCell.isOpen = true;

                        Debug.Log(aroundCell.position);
                        openList.Add(aroundCell);

                        // 真ん中のセルを閉じる
                        center.isOpen = false;
                    }
                }
            }
        }

        /// <summary>
        /// リストの中のオープンされたセルから最も小さいセルを探します
        /// </summary>
        public CellInfo SearchMinCell()
        {
            // オープンされているセルに絞って、合計コストが低い順にならべて、その先頭を代入
            return openList.Where(x => x.isOpen).Select(x => x).OrderBy(x => x.sumCost).FirstOrDefault();
        }

        public void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// 道のりをスタックします
        /// </summary>
        /// <param name="cell">スタックするセルの先頭</param>
        public void StackPath(CellInfo cell)
        {
            int count = 0;
            CellInfo preCell = cell;

            // 前回のセルがスタートの位置じゃない限りスタックにため続ける
            while (preCell.parent != null)
            {
                // ポップしないと無限ループが発生してします。
                pathStack.Push(preCell);

                preCell = preCell.parent;
                count++;

            }
        }
    }
}
