using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    public class EnemyAI
    {
        private GridFieldAStar _aStar;        // AStar
        private GridFieldMap _map;
        private Vector3Int _locoGoalPoint;
        private Vector3Int _pathTargetCoord;      // �^�[�Q�b�g�̍��W
        private Transform _enemyTrafo;
        private Vector3Int _enemyCoord;
        private bool isStay;
        private bool isExit;


        public EnemyAI(Transform enemyTrafo,GridFieldMap map)
        {
            _aStar = new GridFieldAStar();
            _map = map;
            _enemyTrafo = enemyTrafo;
            _enemyCoord = _map.gridField.GetGridCoordinate(_enemyTrafo.position);
            _locoGoalPoint = _enemyCoord;
            _pathTargetCoord = _enemyCoord;
        }


        /// <summary>
        /// ����n�_�܂� Vector3 �̒l�܂œ������܂�
        /// </summary>
        /// <param name="trafo">���������̃g�����X�t�H�[��</param>
        /// <param name="point">�ړI�n</param>
        /// <param name="speed">�������X�s�[�h</param>
        /// <returns>�|�C���g�ɓ��B������true��Ԃ��܂�</returns>
        public bool MoveToPoint(Transform trafo, Vector3 point, float speed = 1)// ref����
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
        public bool LocomotionToCoordPoint( Vector3Int coord,GridFieldMap map, float speed = 1)// ref����
        {
            Vector3 pos = _enemyTrafo.position;
            Vector3 point = map.gridField.grid[coord.x, coord.z];

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
        /// enter,stay,exit�ł킯��
        /// </summary>



        /// <summary>
        /// �ŒZ�o�H�ŖړI�n�܂œ������܂��B��x�ړI�n�ɂ�����I�����܂��B
        /// </summary>
        /// <param name="map">�}�b�v</param>
        /// <param name="moveSpeed">�ǂ�������X�s�[�h</param>
        /// <returns>���������� true</returns>
        public bool LocomotionToAStar(GridFieldMap map, float moveSpeed = 1)
        {
            //Debug.Log(map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z]);
            //Debug.Log(_enemyTrafo.position);

            // �p�X�X�^�b�N������Ȃ�
            if (_aStar.pathStack.Count != 0)
            {
                // �p�X�^�[�Q�b�g�ɒǂ�������
                if (LocomotionToCoordPoint(_pathTargetCoord, map, moveSpeed))
                {
                    // �V�����p�X�^�[�Q�b�g���|�b�v
                    _pathTargetCoord = _aStar.pathStack.Pop().position;
                    Debug.DrawLine(map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z], map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z] + Vector3.up, Color.red, 0.1f);
                }
            }
            // �Ȃ��ꍇ
            else
            {
                // �G�l�~�[���Ō�̃p�X�^�[�Q�b�g�̈ʒu�ɗ��ĂȂ�������
                if(_enemyTrafo.position != map.gridField.grid[_pathTargetCoord.x,_pathTargetCoord.z])
                {
                    return LocomotionToCoordPoint(_pathTargetCoord, map, moveSpeed);
                }
            }

            return false;
        }


        /// <summary>
        /// �ŒZ�o�H�ŖړI�n�܂œ��������܂��B
        /// </summary>
        /// <param name="goalPos">�ǂ������镨�̈ʒu</param>
        /// <param name="map">�}�b�v</param>
        /// <param name="aStarCount">�ĒT�����s���Ԋu</param>
        /// <param name="moveSpeed">�ǂ�������X�s�[�h</param>
        /// <returns>�ǂ�������true</returns>
        public void StayLocomotionToAStar(Vector3 goalPos, GridFieldMap map,int aStarCount = 60, float moveSpeed = 1)
        {
            if (_aStar.pathStack.Count != 0)
            {
                // �p�X�^�[�Q�b�g�ɒǂ�������
                if (LocomotionToCoordPoint(_pathTargetCoord, map, moveSpeed))
                {
                    // �V�����p�X�^�[�Q�b�g���|�b�v
                    _pathTargetCoord = _aStar.pathStack.Pop().position;
                    Debug.DrawLine(map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z], map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z] + Vector3.up, Color.red, 0.1f);
                }

                // �p�X�X�^�b�N���Ȃ��Ȃ�����V�����p�X�����
                if (_aStar.pathStack.Count == 0)
                {
                    _locoGoalPoint = map.gridField.GetGridCoordinate(goalPos);
                    _enemyCoord = map.gridField.GetGridCoordinate(_enemyTrafo.position);

                    _aStar.AStarPath(map, _enemyCoord, _locoGoalPoint);
                    _pathTargetCoord = _enemyCoord;
                }
            }
        }

        /// <summary>
        /// AStar�N���X����p�X��ݒ肵�܂��B
        /// </summary>
        /// <param name="goalPos"></param>
        /// <param name="map"></param>
        public void EnterLocomotionToAStar(Vector3 goalPos, GridFieldMap map)
        {
            _locoGoalPoint = map.gridField.GetGridCoordinate(goalPos);
            _enemyCoord = map.gridField.GetGridCoordinate(_enemyTrafo.position) ;

            // �p�X������āA�G�l�~�[�̂���ꏊ���ŏ��̏ꏊ�ɂ���
            _aStar.AStarPath(map, _enemyCoord, _locoGoalPoint);
            Debug.Log(_aStar.pathStack.Count);

            map.SetAStar(_enemyTrafo.position,map.gridField.grid[_locoGoalPoint.x,_locoGoalPoint.z],_aStar);
            _pathTargetCoord = _aStar.pathStack.Pop().position;
            
            // �f�o�b�O
            //Debug.Log(_pathTargetCoord);
        }

        public void ExitLocomotion(ref bool isExit)
        {
            isExit = false;
            isStay = false;

            _locoGoalPoint = _enemyCoord;
            _aStar.pathStack.Clear();
        }

        /// <summary>
        /// �G�l�~�[��p�j�����܂�
        /// </summary>
        public void Wandering(GridFieldMap map, float moveSpeed, int areaX = 10, int areaZ = 10)
        {
            // �p�j�|�C���g�ɂ����烉���_���Ȉʒu��p�j�|�C���g�ɂ���
            if (LocomotionToAStar(map, moveSpeed))
            {
                _locoGoalPoint = map.GetRandomPoint(_enemyCoord, areaX, areaZ);
            }
        }
    }
}