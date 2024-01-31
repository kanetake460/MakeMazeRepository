using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// �Q�l�T�C�g
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
            public Vector3Int position { get; set; }       // �e�Z���̃O���b�h���W
            public Vector3Int targetPos { get; set; }       // �^�[�Q�b�g�̃O���b�h���W
            public float cost { get; set; }                 // ���R�X�g
            public float estimatedCost { get; set; }        // ����R�X�g
            public float sumCost { get; set; }              // ���R�X�g
            public bool isOpen { get; set; }                // �����Ώۂ��ǂ���
            public CellInfo(GridField gridField, Vector3Int pos,Vector3Int tPos)
            {  
                Vector3 vector3Pos = gridField.grid[pos.x,pos.y];
                Vector3 vector3TPos = gridField.grid[tPos.x,tPos.y];

                position = pos;
                targetPos = tPos;
                cost = 0;
                estimatedCost = Vector3.Distance(vector3TPos, vector3Pos);
                sumCost = cost + estimatedCost;
                isOpen = false;
            }
        }

        /// <summary>
        /// AStar�̃R���X�g���N�^(�^�����O���b�h�t�B�[���h�}�b�v��A*�T�����s���܂�)
        /// </summary>
        /// <param name="gridField">�O���b�h���W</param>
        /// <param name="position"></param>
        /// <param name="targetPos"></param>
        public AStar(GridField gridField, Vector3Int[] map, Vector3Int position, Vector3Int targetPos)
        {
            List<CellInfo> cellInfo = new List<CellInfo>();
            CellInfo startCell = new CellInfo(gridField,position,targetPos);
            cellInfo.Add(startCell);
            for(int x = 0; x < gridField.gridWidth; x++)
            {
                for(int z = 0; z < gridField.gridDepth; z++)
                {
                    //= new Vector3Int(map[map.Length].x, 0 , map[map.Length].z);
                     foreach(Vector3Int cellPos in map)
                    {
                        //cellInfo.Where(x => x != startCell).SelectMany(x => x.sumCost ) ;

                    }
                    //    gridField.grid[map[x].x, map[z].y];
                    //foreach()
                    

                }
            }

        }
    }
}
