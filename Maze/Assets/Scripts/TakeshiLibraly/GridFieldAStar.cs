using System.Collections.Generic;
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
        private class CellInfo
        {
            public Coord coord { get; set; }       // �e�Z���̃O���b�h���W
            public float cost { get; set; }           // ���R�X�g
            public float heuristicCost { get; set; }  // ����R�X�g
            public float sumCost { get; set; }        // ���R�X�g
            public CellInfo parent { get; set; }      // �e�̃Z���̈ʒu
        }

        public Stack<Coord> pathStack { get; } = new Stack<Coord>();    // ���̂�̍��W�X�^�b�N
        
        private GridFieldMap _map;                                      // �}�b�v
        private List<CellInfo> _openList = new List<CellInfo>();        // �I�[�v�����X�g
        private List<CellInfo> _closeList = new List<CellInfo>();       // �N���[�Y���X�g
        private Coord _goal;                                            // �S�[�����W
        private Coord _start;                                           // �X�^�[�g���W
        private readonly int _searchLimit = 1000;                       // �T�����E�l

        /// <summary>
        /// AStar�̃R���X�g���N�^(�^�����O���b�h�t�B�[���h�}�b�v��A*�T�����s���܂�)
        /// </summary>
        /// <param name="gf">�O���b�h���W</param>
        /// <param name="position"></param>
        /// <param name="targetPos"></param>
        public GridFieldAStar(int searchLimit = 1000)
        {
            _searchLimit = searchLimit;
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
            _map = map;
            _start = position;
            _goal = targetPos;

            // �X�^�[�g�n�_�̃Z��������
            _openList.Add(GetSatrtCell());

            int count = 0;
            CellInfo minCell = new CellInfo();

            // ���~�b�g�̌��E�ɂȂ邩�A�S�[���������ł���܂ŃI�[�v����������
            while (count < _searchLimit)
            {
                count++;

                // ���X�g�̒����瑍�R�X�g���ł��Ⴂ���̂��i�[
                minCell = _openList.OrderBy(x => x.sumCost).First();

                // �S�[���ƈ�v����cell�����X�g�ɂ���Ȃ�Ȃ�I��
                if (minCell.coord == _goal) break;

                // �ł��Ⴂ�Z���̎�����I�[�v���ɂ���
                OpenCell(minCell);
            }

            // �T�����~�b�g�ɒB������X�^�[�g�ʒu���X�^�b�N���ďI��
            if (count >= _searchLimit)
            {
                pathStack.Push(GetSatrtCell().coord);
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
            startCell.coord = _start;
            startCell.cost = 0;
            startCell.heuristicCost = Vector2Int.Distance(new Vector2Int(_start.x,_start.z), new Vector2Int(_goal.x, _goal.z));
            startCell.sumCost = startCell.cost + startCell.heuristicCost;

            return startCell;
        }


        /// <summary>
        /// �^�񒆂̎���̃Z�����I�[�v���ɂ��܂�
        /// </summary>
        /// <param name="center">�^�񒆂ɐݒ肷��Z��</param>
        private void OpenCell(CellInfo center)
        {
            Coord centerPos = center.coord;

            // �^�񒆂��N���[�Y���X�g�ɒǉ�
            _openList.Remove(center);
            _closeList.Add(center);

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
                    if (searchPos.x >= _map.gridField.gridWidth ||
                        searchPos.z >= _map.gridField.gridDepth ||
                        searchPos.x < 0 || searchPos.z < 0) continue;

                    // �}�b�v�̕ǃ}�X�Ȃ�T�����Ȃ�
                    if (_map.blocks[searchPos.x, searchPos.z].isSpace == false) continue;

                    // ���̌������ǂȂ�T�����Ȃ�
                    if (_map.blocks[centerPos.x, centerPos.z].CheckWall(x,z))
                    {
                        Debug.Log("�ǂɂԂ���܂���");
                        continue;
                    }

                    CellInfo aroundCell = new CellInfo();

                    // ����̃Z���ɏ�������
                    aroundCell.coord = searchPos;
                    aroundCell.cost = center.cost + 1;
                    aroundCell.heuristicCost = Vector2Int.Distance(new Vector2Int(searchPos.x, searchPos.z), new Vector2Int(_goal.x, _goal.z));
                    aroundCell.sumCost = aroundCell.cost + aroundCell.heuristicCost;
                    aroundCell.parent = center;

                    // �I�[�v�����X�g�ɑ��݂��āA���v�R�X�g���I�[�v���������Z�����傫��
                    if (_openList.Any(c => c.coord == aroundCell.coord))
                    {
                        if (_openList.Find(x => x.coord == searchPos).sumCost > aroundCell.sumCost)
                        {
                            // ���X�g�ɂ�����̂��폜���A�V��������ǉ�
                            var o = _openList.Find(x => x.coord == searchPos);
                            _openList.Remove(o);
                            _openList.Add(aroundCell);
                            continue;
                        }
                    }

                    // �N���[�Y���X�g�ɑ��݂��āA���v�R�X�g���I�[�v���������Z�����傫��
                    if (_closeList.Any(c => c.coord == aroundCell.coord))
                    {
                        if (_closeList.Find(x => x.coord == searchPos).sumCost > aroundCell.sumCost)
                        {
                            // ���X�g�ɂ�����̂��폜���A�V��������ǉ�
                            var c = _closeList.Where(y => y.coord == searchPos).First();
                            _closeList.Remove(c);
                            _openList.Add(aroundCell);
                            continue;
                        }
                    }

                    // �����̃��X�g�ɃZ����������݂��Ȃ�
                    if (_openList.Contains(aroundCell) == false && _closeList.Contains(aroundCell) == false)
                    {
                        // �I�[�v�����X�g�ɒǉ�
                        _openList.Add(aroundCell);
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
            // ���X�g���N���A����
            _openList.Clear();
            _closeList.Clear();
            pathStack.Clear();
            
            int count = 0;
            CellInfo preCell = cell;
            
            // �S�[���ƃX�^�[�g�̈ʒu������
            if(preCell.coord == _start)
            {
                pathStack.Push(cell.coord);
            }

            // �O��̃Z�����X�^�[�g�̈ʒu����Ȃ�����X�^�b�N�ɂ��ߑ�����
            while (preCell.parent != null)
            {
                // �|�b�v���Ȃ��Ɩ������[�v���������Ă��܂��B
                pathStack.Push(preCell.coord);

                // �v�b�V�������Z���̐e�Z������
                preCell = preCell.parent;
                
                // �������[�v�΍�
                count++;
                if (count > 10000) break;
            }
        }
    }
}
