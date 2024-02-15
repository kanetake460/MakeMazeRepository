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

        }

        GridField m_GridField;
        TestMap.Map m_Map;
        private List<CellInfo> openList = new List<CellInfo>();
        private List<CellInfo> closeList = new List<CellInfo>();
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

            openList.Add(startCell);

        }

        /// <summary>
        /// �o�H��T�����܂�
        /// </summary>
        public void AStarPath()
        {
            int count = 0;
            CellInfo minCell = openList.First();
            while (count < 100000)
            {

                minCell = SearchMinCell();  // ���X�g�̒����瑍�R�X�g���ł��Ⴂ���̂��i�[

                OpenCell(minCell);          // �ł��Ⴂ�Z���̎�����I�[�v���ɂ���

                count++;

                if (minCell.position == goal) break;    // �ł��Ⴂ�Z���̃|�W�V�������S�[���Ȃ烋�[�v�I��
            }
            // �S�[���ɂ��ǂ蒅�����Z�����珇�ɂ��ǂ��ăX�^�[�g�܂ł̓��̂���X�^�b�N
            StackPath(minCell);
        }


        /// <summary>
        /// �^�񒆂̎���̃Z�����I�[�v���ɂ��܂�
        /// </summary>
        /// <param name="center">�^�񒆂ɐݒ肷��Z��</param>
        public void OpenCell(CellInfo center)
        {
            Vector3Int centerPos = center.position;

            // �㉺���E�A�^�񒆂͊܂߂Ȃ�
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x != 0 && z != 0) continue;

                    if (x == 0 && z == 0) continue;

                    Vector3Int searchPos = new Vector3Int(centerPos.x + x, centerPos.y, centerPos.z + z);

                    // �}�b�v�̊O�Ȃ�I�[�v�����Ȃ�
                    if (searchPos.x >= m_Map.mapWidth ||
                        searchPos.z >= m_Map.mapDepth ||
                        searchPos.x < 0 || searchPos.z < 0) continue;

                    // �}�b�v�̕ǃ}�X�Ȃ�I�[�v�����Ȃ�
                    if (m_Map.blocks[searchPos.x, searchPos.z].type == TestMap.Map.BlockType.eWall) continue;
                    Debug.Log("�ǂ���Ȃ�");

                    CellInfo aroundCell = new CellInfo();

                    // ����̃Z���ɏ�������
                    aroundCell.position = searchPos;
                    aroundCell.cost = center.cost + 1;
                    aroundCell.heuristicCost = Vector3Int.Distance(searchPos, goal);
                    aroundCell.sumCost = aroundCell.cost + aroundCell.heuristicCost;
                    aroundCell.parent = center;
                    aroundCell.isOpen = true;

                    // �����̃��X�g�ɑ��݂��Ȃ�
                    if (openList.All(x => x == null) && closeList.All(x => x == null))
                    {
                        openList.Add(aroundCell);
                        continue;
                    }

                    // �I�[�v�����X�g�ɑ��݂��āA���v�R�X�g���I�[�v���������Z�����傫��
                    if (openList.Where(x => x.position == searchPos).First().sumCost > aroundCell.sumCost)
                    {
                        var o = openList.Where(x => x.position == searchPos).First();
                        openList.Remove(o);
                        openList.Add(aroundCell);
                        continue;
                    }

                    // �N���[�Y���X�g�ɑ��݂��āA���v�R�X�g���I�[�v���������Z�����傫��
                    if(closeList.Where(x => x.position == searchPos).First().sumCost > aroundCell.sumCost)
                    {
                        var c = closeList.Where(y => y.position == searchPos).First();
                        closeList.Remove(c);
                        openList.Add(aroundCell);
                        continue;
                    }



                    //openList.Where(x => x.position == searchPos).First() == null);       // 
                    // �e�̃|�W�V�����łȂ��A�Z���^�[�łȂ��A�e���Ȃ�
                    if (center.parent == null
                        || searchPos != center.parent.position && searchPos != center.position)
                    {
                        CellInfo aroundCell = new CellInfo();

                        // ����̃Z���ɏ�������
                        aroundCell.position = searchPos;
                        aroundCell.cost = center.cost + 1;
                        aroundCell.heuristicCost = Vector3Int.Distance(searchPos, goal);
                        aroundCell.sumCost = aroundCell.cost + aroundCell.heuristicCost;
                        aroundCell.parent = center;
                        aroundCell.isOpen = true;

                        Debug.Log(aroundCell.position);
                        openList.Add(aroundCell);

                        // �^�񒆂̃Z�������
                        center.isOpen = false;
                    }
                }
            }
        }

        /// <summary>
        /// ���X�g�̒��̃I�[�v�����ꂽ�Z������ł��������Z����T���܂�
        /// </summary>
        public CellInfo SearchMinCell()
        {
            // �I�[�v������Ă���Z���ɍi���āA���v�R�X�g���Ⴂ���ɂȂ�ׂāA���̐擪����
            return openList.Where(x => x.isOpen).Select(x => x).OrderBy(x => x.sumCost).FirstOrDefault();
        }

        public void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// ���̂���X�^�b�N���܂�
        /// </summary>
        /// <param name="cell">�X�^�b�N����Z���̐擪</param>
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
                count++;

            }
        }
    }
}
