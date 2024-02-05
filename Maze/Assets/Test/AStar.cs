using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using static TestMap;

// �Q�l�T�C�g
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
            public Vector3Int position { get; set; }       // �e�Z���̃O���b�h���W
            public float cost { get; set; }                 // ���R�X�g
            public float heuristicCost { get; set; }        // ����R�X�g
            public float sumCost { get; set; }              // ���R�X�g
            public CellInfo parent { get; set; }          // �e�̃Z���̈ʒu
            public bool isOpen { get; set; }                // �����Ώۂ��ǂ���

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
        /// AStar�̃R���X�g���N�^(�^�����O���b�h�t�B�[���h�}�b�v��A*�T�����s���܂�)
        /// </summary>
        /// <param name="gf">�O���b�h���W</param>
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
        /// �o�H��T�����܂�
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
        /// �^�񒆂̎���̃Z�����I�[�v���ɂ��܂�
        /// </summary>
        /// <param name="center">�^�񒆂ɐݒ肷��Z��</param>
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

                        // �e�̃|�W�V�����łȂ��Ax�ꍇ��
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
        /// ���X�g�̒��̃I�[�v�����ꂽ�Z������ł��������Z����T���܂�
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
            // �O��̃Z�����X�^�[�g�̈ʒu����Ȃ�����X�^�b�N�ɂ��ߑ�����
            while (preCell.parent != null)
            {
                // �|�b�v���Ȃ��Ɩ������[�v���������Ă��܂��B
                pathStack.Push(preCell);

                    preCell = preCell.parent;
                 //Debug.Log(pathStack.Pop().position);
                count++;


            }

        }
    }
}
