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

            //public CellInfo(Vector3Int pos, float cost, float heuristicCost, float sumCost, CellInfo parent, bool isOpen)
            //{
            //    position = pos;
            //    this.cost = cost;
            //    this.heuristicCost = heuristicCost;
            //    this.sumCost = sumCost;
            //    this.isOpen = isOpen;
            //}
        }

        GridField m_GridField;
        TestMap.Map m_Map;
        private List<CellInfo> cellInfoList = new List<CellInfo>();
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

            cellInfoList.Add(startCell);
        }

        /// <summary>
        /// 経路を探索します
        /// </summary>
        public void AStarPath()
        {
            int count = 0;
            CellInfo minCell = cellInfoList.First();
            while (true)
            {

                minCell = SearchMinCell();
                //Debug.Log(minCell.position);
                OpenCell(minCell);

                count++;

                if (minCell.position == goal) break;
            }
            StackPath(minCell);
        }


        /// <summary>
        /// 真ん中の周りのセルをオープンにします
        /// </summary>
        /// <param name="center">真ん中に設定するセル</param>
        public void OpenCell(CellInfo center)
        {
            Vector3Int centerPos = center.position;

            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 || z == 0)
                    {
                        Vector3Int searchPos = new Vector3Int(centerPos.x + x, centerPos.y, centerPos.z + z);
                        CellInfo aroundCell = new CellInfo();

                        if (searchPos.x >= m_Map.mapWidth ||
                            searchPos.z >= m_Map.mapDepth ||
                            searchPos.x < 0 || searchPos.z < 0) continue; 

                        if (m_Map.blocks[searchPos.x, searchPos.z].type == TestMap.Map.BlockType.eWall) continue;

                        if (cellInfoList.Where(x => x.isOpen == true).All(x => x.position == searchPos)) continue;

                        // 親のポジションでない、x場合は
                        if (center.parent == null || searchPos != center.parent.position && searchPos != center.position)
                        {
                            aroundCell.position = searchPos;
                            aroundCell.cost = center.cost + 1;
                            aroundCell.heuristicCost = Vector3Int.Distance(searchPos, goal);
                            aroundCell.sumCost = aroundCell.cost + aroundCell.heuristicCost;
                            aroundCell.parent = center;
                            aroundCell.isOpen = true;

                            cellInfoList.Add(aroundCell);

                            center.isOpen = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// リストの中のオープンされたセルから最も小さいセルを探します
        /// </summary>
        public CellInfo SearchMinCell()
        {
            CellInfo minCell = cellInfoList.Where(x => x.isOpen).Select(x => x).OrderBy(x => x.sumCost).FirstOrDefault();
            return minCell;
        }

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
                 //Debug.Log(pathStack.Pop().position);
                count++;


            }

        }
    }
}
