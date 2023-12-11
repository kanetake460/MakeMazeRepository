using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiClass
{
    public class GridField : MonoBehaviour
    {
        /*�p�����[�^�[*/
        public static float gridBreadth = 30;            // �O���b�h�̍L��

        /*�p�u���b�N�ϐ�*/
        public Vector3[,] m_grid = new Vector3[100, 100];      // �O���b�h�̃Z���̔z�uVector3�̓񎟌��z��
        public Vector3Int m_gridCoordinate = Vector3Int.zero;                                         // �O���b�h���W

        void Start()
        {

        }

        /*=====��d���[�v��grid�z��̂��ꂼ���Vector3�̍��W�l����=====*/
        // ����:y���W
        public static GridField AssignValue(float y)
        {
            GridField grid = new GridField();
            /*===��d���[�v��grid�z��̂��ꂼ���Vector3�̍��W�l����===*/
            for (grid.m_gridCoordinate.x = 0; grid.m_gridCoordinate.x < gridBreadth; grid.m_gridCoordinate.x++)
            {
                for (grid.m_gridCoordinate.z = 0; grid.m_gridCoordinate.z < gridBreadth; grid.m_gridCoordinate.z++)
                {
                    grid.m_grid[grid.m_gridCoordinate.x, grid.m_gridCoordinate.z] = new Vector3(grid.m_gridCoordinate.x * 10, y, grid.m_gridCoordinate.z * 10);    // x��z��10���������l����
                }
            }
            return grid;
        }

        /*=====�����ɗ^���� Transform ���ǂ��̃O���b�h���W�ɂ���̂���Ԃ�=====*/
        // ����:Transform
        // �߂�l:������transform�̂���Z���̃O���b�h���W
        public static Vector3Int GetGrid(Transform pos)
        {
            GridField grid = AssignValue(0);
            Vector3Int gridCoordinate = Vector3Int.zero;

            /*===��d���[�v�Ō��݂̃Z���𒲂ׂ�===*/
            for (gridCoordinate.x = 0; gridCoordinate.x < gridBreadth; gridCoordinate.x++)
            {
                for (gridCoordinate.z = 0; gridCoordinate.z < gridBreadth; gridCoordinate.z++)
                {
                    if (pos.position.x <= grid.m_grid[gridCoordinate.x, gridCoordinate.z].x + 5 &&
                        pos.position.x >= grid.m_grid[gridCoordinate.x, gridCoordinate.z].x - 5 &&
                        pos.position.z <= grid.m_grid[gridCoordinate.x, gridCoordinate.z].z + 5 &&
                        pos.position.z <= grid.m_grid[gridCoordinate.x, gridCoordinate.z].z + 5)     // ��������Z���̏�ɂ���Ȃ�
                    {
                        Debug.Log(gridCoordinate);
                        return gridCoordinate;                      // �Z���� Vector3��Ԃ�
                    }
                }
            }
            Debug.LogError("�^����ꂽ�|�W�V�����̓O���b�h�t�B�[���h�̏�ɂ��܂���B");
            return Vector3Int.zero;
        }

        void Update()
        {

        }
    }
}