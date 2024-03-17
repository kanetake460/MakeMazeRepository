using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;

// 参考サイト
// https://walkable-2020.com/unity/a-star-pathfinding/
// https://www.sejuku.net/blog/47172
// https://www.sejuku.net/blog/72918
// https://qiita.com/toRisouP/items/98cc4966d392b7f21c30
// https://yttm-work.jp/algorithm/algorithm_0015.html
namespace TakeshiLibrary
{
    public class GridFieldAStar
    {
        /// <summary>
        /// セルの情報
        /// </summary>
        public class CellInfo
        {
            public Coord position { get; set; }       // 親セルのグリッド座標
            private float _cost;
            public float cost {
                get
                {
                    return _cost;
                }
                set
                {
                    if (value < 0)
                    {
                        new InvalidOperationException("");
                    }
                    _cost = value;
                }
                }                 // 実コスト
            public float heuristicCost { get; set; }        // 推定コスト
            public float sumCost { get; set; }              // 総コスト
            public CellInfo parent { get; set; }          // 親のセルの位置
        }

        private GridFieldMap m_Map;
        public Stack<CellInfo> pathStack { get; } = new Stack<CellInfo>();
        private List<CellInfo> openList = new List<CellInfo>();
        private List<CellInfo> closeList = new List<CellInfo>();
        private Coord goal;
        private Coord start;
        private readonly int m_searchLimit = 1000;

        /// <summary>
        /// AStarのコンストラクタ(与えたグリッドフィールドマップでA*探索を行います)
        /// </summary>
        /// <param name="gf">グリッド座標</param>
        /// <param name="position"></param>
        /// <param name="targetPos"></param>
        public GridFieldAStar(int searchLimit = 1000)
        {
            m_searchLimit = searchLimit;
        }


        /// <summary>
        /// 経路を探索します
        /// </summary>
        /// <param name="map"></param>
        /// <param name="position"></param>
        /// <param name="targetPos"></param>
        /// <returns>ゴールを発見したかどうか true：探索成功</returns>
        public bool AStarPath(GridFieldMap map, Coord position, Coord targetPos)
        {
            m_Map = map;
            start = position;
            goal = targetPos;

            // スタート地点のセルを入れる
            openList.Add(GetSatrtCell());

            int count = 0;
            CellInfo minCell = new CellInfo();

            while (count < m_searchLimit)
            {
                count++;

                // リストの中から総コストが最も低いものを格納
                minCell = openList.OrderBy(x => x.sumCost).First();

                // 総コストが最も低いセルのポジションがゴールと一致するなら終了
                if (minCell.position == goal) break;

                // 最も低いセルの周りをオープンにする
                OpenCell(minCell);
            }

            if (count >= m_searchLimit)
            {
                pathStack.Push(GetSatrtCell());
                Debug.Log("ゴールが見つからないまま探索が中断されました");
                return false;
            }
            else
            {
                // ゴールにたどり着いたセルから順にたどってスタートまでの道のりをスタック
                StackPath(minCell);
                return true;
            }
        }


        /// <summary>
        /// 探索開始地点のセルをオープンリストに入れます
        /// </summary>
        private CellInfo GetSatrtCell()
        {
            // 探索開始地点のセル
            CellInfo startCell = new CellInfo();
            startCell.position = start;
            startCell.cost = 0;
            startCell.heuristicCost = Coord.Distance(start, goal);
            startCell.sumCost = startCell.cost + startCell.heuristicCost;

            return startCell;
        }


        /// <summary>
        /// 真ん中の周りのセルをオープンにします
        /// </summary>
        /// <param name="center">真ん中に設定するセル</param>
        public void OpenCell(CellInfo center)
        {
            Coord centerPos = center.position;

            // 真ん中をクローズリストに追加
            openList.Remove(center);
            closeList.Add(center);

            // 上下左右、真ん中は含めない
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    // 斜めは探索しない
                    if (x != 0 && z != 0) continue;

                    // 真ん中は探索しない
                    if (x == 0 && z == 0) continue;

                    Coord searchPos = new Coord(centerPos.x + x, centerPos.z + z);

                    // マップの外なら探索しないしない
                    if (searchPos.x >= m_Map.gridField.gridWidth ||
                        searchPos.z >= m_Map.gridField.gridDepth ||
                        searchPos.x < 0 || searchPos.z < 0) continue;

                    // マップの壁マスなら探索しない
                    if (m_Map.blocks[searchPos.x, searchPos.z].isSpace == false) continue;

                    // その向きが壁なら探索しない
                    if (m_Map.blocks[centerPos.x, centerPos.z].CheckWall(x,z))
                    {
                        Debug.Log("壁にぶつかりました");
                        continue;
                    }

                    CellInfo aroundCell = new CellInfo();

                    // 周りのセルに情報を入れる
                    aroundCell.position = searchPos;
                    aroundCell.cost = center.cost + 1;
                    aroundCell.heuristicCost = Coord.Distance(searchPos, goal);
                    aroundCell.sumCost = aroundCell.cost + aroundCell.heuristicCost;
                    aroundCell.parent = center;

                    // 両方のリストにセルが一つも存在しない
                    if (openList.Contains(center) == false || closeList.Contains(center) == false) 
                    {
                        openList.Add(aroundCell);
                        continue;
                    }

                    // オープンリストに存在して、合計コストがオープンしたいセルより大きい
                    if (openList.Find(x => x.position == searchPos).sumCost > aroundCell.sumCost)
                    {
                        // リストにあるものを削除し、新しい方を追加
                        var o = openList.Find(x => x.position == searchPos);
                        openList.Remove(o);
                        openList.Add(aroundCell);
                        continue;
                    }

                    // クローズリストに存在して、合計コストがオープンしたいセルより大きい
                    if(closeList.Find(x => x.position == searchPos).sumCost > aroundCell.sumCost)
                    {
                        var c = closeList.Where(y => y.position == searchPos).First();
                        closeList.Remove(c);
                        openList.Add(aroundCell);
                        continue;
                    }
                }
            }
        }


        /// <summary>
        /// 道のりをスタックします
        /// </summary>
        /// <param name="cell">スタックするセルの先頭</param>
        private void StackPath(CellInfo cell)
        {
            openList.Clear();
            closeList.Clear();
            pathStack.Clear();
            int count = 0;
            CellInfo preCell = cell;
            if(preCell.position == start)
            {
                Debug.Log("ゴールとスタートの位置が同じです");
                pathStack.Push(cell);
            }

            // 前回のセルがスタートの位置じゃない限りスタックにため続ける
            while (preCell.parent != null)
            {
                // ポップしないと無限ループが発生してします。
                pathStack.Push(preCell);

                preCell = preCell.parent;
                count++;
                if (count > 10000) break;
            }
        }
    }
}
