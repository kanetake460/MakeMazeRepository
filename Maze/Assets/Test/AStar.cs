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
        class CellInfo
        {
            public Vector3Int position { get; set; }       // 親セルのグリッド座標
            public Vector3Int targetPos { get; set; }       // ターゲットのグリッド座標
            public float cost { get; set; }                 // 実コスト
            public float estimatedCost { get; set; }        // 推定コスト
            public float sumCost { get; set; }              // 総コスト
            public bool isOpen { get; set; }                // 調査対象かどうか
            public bool isSearched { get; set; }            // 探索済みかどうか

            public CellInfo(GridField gridField, Vector3Int pos,Vector3Int tPos, bool isSearched)
            {  
                Vector3 vector3Pos = gridField.grid[pos.x,pos.y];
                Vector3 vector3TPos = gridField.grid[tPos.x,tPos.y];

                position = pos;
                targetPos = tPos;
                cost = 0;
                estimatedCost = Vector3.Distance(vector3TPos, vector3Pos);
                sumCost = cost + estimatedCost;
                isOpen = false;
                this.isSearched = isSearched;
            }
        }

        GridField m_GridField;
        TestMap.Map m_Map;
        List<CellInfo> cellInfoList = new List<CellInfo>();
        List<CellInfo> stack = new List<CellInfo>();
        Vector3Int goal;
        Vector3Int start;

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
            CellInfo startCell = new CellInfo(gf,start,goal,true);
            cellInfoList.Add(startCell);

            for(int x = 0; x < map.mapWidth; x++)
            {
                for(int z = 0; z < map.mapDepth; z++)
                {
                    CellInfo allCell = new CellInfo(gf, new Vector3Int(x, 0, z), targetPos,false);
                    if (map.blocks[x, z].type == TestMap.Map.BlockType.eSpace) cellInfoList.Add(allCell);
                }
            }


        }

        public void StoreCellInfo()
        {
            List<CellInfo> cellInfo = new List<CellInfo>();


        }

        public void SearchRoute()
        {
            //List<CellInfo> searchCell = new List<CellInfo>();
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    CellInfo mineCell = 
            cellInfoList.Where(x => x.position == startCell.position);
                }
            }
        }
    }
}
