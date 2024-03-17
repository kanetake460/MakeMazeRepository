using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;

// �Q�l�T�C�g
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
        /// �Z���̏��
        /// </summary>
        public class CellInfo
        {
            public Coord position { get; set; }       // �e�Z���̃O���b�h���W
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
                }                 // ���R�X�g
            public float heuristicCost { get; set; }        // ����R�X�g
            public float sumCost { get; set; }              // ���R�X�g
            public CellInfo parent { get; set; }          // �e�̃Z���̈ʒu
        }

        private GridFieldMap m_Map;
        public Stack<CellInfo> pathStack { get; } = new Stack<CellInfo>();
        private List<CellInfo> openList = new List<CellInfo>();
        private List<CellInfo> closeList = new List<CellInfo>();
        private Coord goal;
        private Coord start;
        private readonly int m_searchLimit = 1000;

        /// <summary>
        /// AStar�̃R���X�g���N�^(�^�����O���b�h�t�B�[���h�}�b�v��A*�T�����s���܂�)
        /// </summary>
        /// <param name="gf">�O���b�h���W</param>
        /// <param name="position"></param>
        /// <param name="targetPos"></param>
        public GridFieldAStar(int searchLimit = 1000)
        {
            m_searchLimit = searchLimit;
        }


        /// <summary>
        /// �o�H��T�����܂�
        /// </summary>
        /// <param name="map"></param>
        /// <param name="position"></param>
        /// <param name="targetPos"></param>
        /// <returns>�S�[���𔭌��������ǂ��� true�F�T������</returns>
        public bool AStarPath(GridFieldMap map, Coord position, Coord targetPos)
        {
            m_Map = map;
            start = position;
            goal = targetPos;

            // �X�^�[�g�n�_�̃Z��������
            openList.Add(GetSatrtCell());

            int count = 0;
            CellInfo minCell = new CellInfo();

            while (count < m_searchLimit)
            {
                count++;

                // ���X�g�̒����瑍�R�X�g���ł��Ⴂ���̂��i�[
                minCell = openList.OrderBy(x => x.sumCost).First();

                // ���R�X�g���ł��Ⴂ�Z���̃|�W�V�������S�[���ƈ�v����Ȃ�I��
                if (minCell.position == goal) break;

                // �ł��Ⴂ�Z���̎�����I�[�v���ɂ���
                OpenCell(minCell);
            }

            if (count >= m_searchLimit)
            {
                pathStack.Push(GetSatrtCell());
                Debug.Log("�S�[����������Ȃ��܂ܒT�������f����܂���");
                return false;
            }
            else
            {
                // �S�[���ɂ��ǂ蒅�����Z�����珇�ɂ��ǂ��ăX�^�[�g�܂ł̓��̂���X�^�b�N
                StackPath(minCell);
                return true;
            }
        }


        /// <summary>
        /// �T���J�n�n�_�̃Z�����I�[�v�����X�g�ɓ���܂�
        /// </summary>
        private CellInfo GetSatrtCell()
        {
            // �T���J�n�n�_�̃Z��
            CellInfo startCell = new CellInfo();
            startCell.position = start;
            startCell.cost = 0;
            startCell.heuristicCost = Coord.Distance(start, goal);
            startCell.sumCost = startCell.cost + startCell.heuristicCost;

            return startCell;
        }


        /// <summary>
        /// �^�񒆂̎���̃Z�����I�[�v���ɂ��܂�
        /// </summary>
        /// <param name="center">�^�񒆂ɐݒ肷��Z��</param>
        public void OpenCell(CellInfo center)
        {
            Coord centerPos = center.position;

            // �^�񒆂��N���[�Y���X�g�ɒǉ�
            openList.Remove(center);
            closeList.Add(center);

            // �㉺���E�A�^�񒆂͊܂߂Ȃ�
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    // �΂߂͒T�����Ȃ�
                    if (x != 0 && z != 0) continue;

                    // �^�񒆂͒T�����Ȃ�
                    if (x == 0 && z == 0) continue;

                    Coord searchPos = new Coord(centerPos.x + x, centerPos.z + z);

                    // �}�b�v�̊O�Ȃ�T�����Ȃ����Ȃ�
                    if (searchPos.x >= m_Map.gridField.gridWidth ||
                        searchPos.z >= m_Map.gridField.gridDepth ||
                        searchPos.x < 0 || searchPos.z < 0) continue;

                    // �}�b�v�̕ǃ}�X�Ȃ�T�����Ȃ�
                    if (m_Map.blocks[searchPos.x, searchPos.z].isSpace == false) continue;

                    // ���̌������ǂȂ�T�����Ȃ�
                    if (m_Map.blocks[centerPos.x, centerPos.z].CheckWall(x,z))
                    {
                        Debug.Log("�ǂɂԂ���܂���");
                        continue;
                    }

                    CellInfo aroundCell = new CellInfo();

                    // ����̃Z���ɏ�������
                    aroundCell.position = searchPos;
                    aroundCell.cost = center.cost + 1;
                    aroundCell.heuristicCost = Coord.Distance(searchPos, goal);
                    aroundCell.sumCost = aroundCell.cost + aroundCell.heuristicCost;
                    aroundCell.parent = center;

                    // �����̃��X�g�ɃZ����������݂��Ȃ�
                    if (openList.Contains(center) == false || closeList.Contains(center) == false) 
                    {
                        openList.Add(aroundCell);
                        continue;
                    }

                    // �I�[�v�����X�g�ɑ��݂��āA���v�R�X�g���I�[�v���������Z�����傫��
                    if (openList.Find(x => x.position == searchPos).sumCost > aroundCell.sumCost)
                    {
                        // ���X�g�ɂ�����̂��폜���A�V��������ǉ�
                        var o = openList.Find(x => x.position == searchPos);
                        openList.Remove(o);
                        openList.Add(aroundCell);
                        continue;
                    }

                    // �N���[�Y���X�g�ɑ��݂��āA���v�R�X�g���I�[�v���������Z�����傫��
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
        /// ���̂���X�^�b�N���܂�
        /// </summary>
        /// <param name="cell">�X�^�b�N����Z���̐擪</param>
        private void StackPath(CellInfo cell)
        {
            openList.Clear();
            closeList.Clear();
            pathStack.Clear();
            int count = 0;
            CellInfo preCell = cell;
            if(preCell.position == start)
            {
                Debug.Log("�S�[���ƃX�^�[�g�̈ʒu�������ł�");
                pathStack.Push(cell);
            }

            // �O��̃Z�����X�^�[�g�̈ʒu����Ȃ�����X�^�b�N�ɂ��ߑ�����
            while (preCell.parent != null)
            {
                // �|�b�v���Ȃ��Ɩ������[�v���������Ă��܂��B
                pathStack.Push(preCell);

                preCell = preCell.parent;
                count++;
                if (count > 10000) break;
            }
        }
    }
}
