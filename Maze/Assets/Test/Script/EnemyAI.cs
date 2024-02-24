using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    public class EnemyAI
    {
        private GridFieldAStar _aStar;          // AStar
        private GridFieldMap _map;              // �}�b�v
        
        private Vector3Int _pathTargetCoord;    // ���̂�̃^�[�Q�b�g�̍��W
        private Transform _enemyTrafo;          // �G�l�~�[�̃g�����X�t�H�[��
        
        private int _stayCount = 0;             // AStarLocomotion�̍ĒT���܂ł̃J�E���g


        public EnemyAI(Transform enemyTrafo,GridFieldMap map)
        {
            _aStar = new GridFieldAStar();
            _map = map;
            _enemyTrafo = enemyTrafo;
            _pathTargetCoord = _map.gridField.GetGridCoordinate(_enemyTrafo.position);
        }


        /// <summary>
        /// ����n�_�܂� Vector3 �̒l�܂œ������܂�
        /// </summary>
        /// <param name="trafo">���������̃g�����X�t�H�[��</param>
        /// <param name="point">�ړI�n</param>
        /// <param name="speed">�������X�s�[�h</param>
        /// <returns>�|�C���g�ɓ��B������true��Ԃ��܂�</returns>
        public bool MoveToPoint(Transform trafo, Vector3 point, float speed = 1)
        {
            Vector3 pos = trafo.position;

            pos.x += pos.x <= point.x ? speed * 0.01f : speed * -0.01f;
            pos.y += pos.y <= point.y ? speed * 0.01f : speed * -0.01f;
            pos.z += pos.z <= point.z ? speed * 0.01f : speed * -0.01f;

            if (pos.x <= point.x + speed * 0.1f && pos.x >= point.x + speed * -0.1f) pos.x = point.x;
            if (pos.y <= point.y + speed * 0.1f && pos.y >= point.y + speed * -0.1f) pos.y = point.y;
            if (pos.z <= point.z + speed * 0.1f && pos.z >= point.z + speed * -0.1f) pos.z = point.z;

            trafo.position = pos;

            return pos == point;
        }


        /// <summary>
        /// ����n�_�܂� Vector3 �̒l�܂œ������܂�
        /// </summary>
        /// <param name="trafo">���������̃g�����X�t�H�[��</param>
        /// <param name="point">�ړI�n</param>
        /// <param name="speed">�������X�s�[�h</param>
        /// <returns>�|�C���g�ɓ��B������true��Ԃ��܂�</returns>
        public bool LocomotionToCoordPoint(Vector3Int coord, float speed = 1)
        {
            Vector3 pos = _enemyTrafo.position;
            Vector3 point = _map.gridField.grid[coord.x, coord.z];

            pos.x += pos.x <= point.x ? speed * 0.01f : speed * -0.01f;
            pos.y += pos.y <= point.y ? speed * 0.01f : speed * -0.01f;
            pos.z += pos.z <= point.z ? speed * 0.01f : speed * -0.01f;

            if (pos.x <= point.x + speed * 0.1f && pos.x >= point.x + speed * -0.1f) pos.x = point.x;
            if (pos.y <= point.y + speed * 0.1f && pos.y >= point.y + speed * -0.1f) pos.y = point.y;
            if (pos.z <= point.z + speed * 0.1f && pos.z >= point.z + speed * -0.1f) pos.z = point.z;

            _enemyTrafo.position = pos;

            return pos == point;
        }


        /// <summary>
        /// �ŒZ�o�H�ŖړI�n�܂œ������܂��B��x�ړI�n�ɂ�����I�����܂��B
        /// </summary>
        /// <param name="moveSpeed">�ǂ�������X�s�[�h</param>
        /// <returns>���������� true</returns>
        public bool LocomotionToAStar( float moveSpeed = 1)
        {
            // �p�X�X�^�b�N������Ȃ�
            if (_aStar.pathStack.Count != 0)
            {
                // �p�X�^�[�Q�b�g�ɒǂ�������
                if (LocomotionToCoordPoint(_pathTargetCoord, moveSpeed)) 
                {
                    // �V�����p�X�^�[�Q�b�g���|�b�v
                    _pathTargetCoord = _aStar.pathStack.Pop().position;
                    Debug.DrawLine(_map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z], _map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z] + Vector3.up, Color.red, 0.1f);
                }
            }
            // �Ȃ��ꍇ
            else
            {
                // �G�l�~�[���Ō�̃p�X�^�[�Q�b�g�̈ʒu�ɗ��ĂȂ�������
                if(_enemyTrafo.position != _map.gridField.grid[_pathTargetCoord.x,_pathTargetCoord.z])
                {
                    return LocomotionToCoordPoint(_pathTargetCoord, moveSpeed);
                }
            }

            return false;
        }


        /// <summary>
        /// �ŒZ�o�H�ŖړI�n�܂œ����������܂��B
        /// </summary>
        /// <param name="goalPos">�ǂ������镨�̈ʒu</param>
        /// <param name="map">�}�b�v</param>
        /// <param name="aStarCount">�ĒT�����s���Ԋu</param>
        /// <param name="moveSpeed">�ǂ�������X�s�[�h</param>
        /// <returns>�ǂ�������true</returns>
        public void StayLocomotionToAStar(Vector3 goalPos,float moveSpeed = 1, int aStarCount = 360)
        {
            LocomotionToAStar(moveSpeed);

            _stayCount++;
            if(_stayCount > aStarCount)
            {
                _stayCount = 0;
                EnterLocomotionToAStar(goalPos);
            }
        }


        /// <summary>
        /// AStar�N���X����p�X��ݒ肵�܂��B
        /// </summary>
        /// <param name="goalPos"></param>
        public void EnterLocomotionToAStar(Vector3 goalPos)
        {
            var enemyCoord = _map.gridField.GetGridCoordinate(_enemyTrafo.position) ;
            var locoGoalCoord = _map.gridField.GetGridCoordinate(goalPos);

            // �p�X������āA�G�l�~�[�̂���ꏊ���ŏ��̏ꏊ�ɂ���
            _aStar.AStarPath(_map, enemyCoord, locoGoalCoord);

            /// �f�o�b�O
            ///_map.SetAStar(_enemyTrafo.position,_map.gridField.grid[_locoGoalPoint.x,_locoGoalPoint.z],_aStar);
            
            _pathTargetCoord = _aStar.pathStack.Pop().position;
        }


        /// <summary>
        /// �ړ����I�������܂�
        /// </summary>
        /// <param name="isExit"></param>
        public void ExitLocomotion(ref bool isExit)
        {
            isExit = false;

            _stayCount = 0;
            _aStar.pathStack.Clear();
        }


        /// <summary>
        /// �G�l�~�[��p�j�����܂�
        /// </summary>
        public void Wandering(float moveSpeed, int areaX = 10, int areaZ = 10)
        {
            // �p�j�|�C���g�ɂ����烉���_���Ȉʒu��p�j�|�C���g�ɂ���
            if (LocomotionToAStar(moveSpeed))
            {
                var enemyCoord = _map.gridField.GetGridCoordinate(_enemyTrafo.position);
                var locoGoalCoord = _map.GetRandomPoint(enemyCoord, areaX, areaZ);
                
                EnterLocomotionToAStar(_map.gridField.grid[locoGoalCoord.x,locoGoalCoord.z]);
            }
        }
    }
}