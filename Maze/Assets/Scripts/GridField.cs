using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiClass
{
    public class GridField : MonoBehaviour
    {

        /*�p�u���b�N�ϐ�*/
        public int gridBreadth;            // �O���b�h�̍L��
        public float cellWidth;
        public float cellDepth;
        public float y;
        public Vector3[,] grid = new Vector3[100, 100];      // �O���b�h�̃Z���̔z�uVector3�̓񎟌��z��
        public Vector3Int gridCoordinate;// = Vector3Int.zero;                                         // �O���b�h���W

        /// <summary>
        /// �O���b�h�̃Z���̐���Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public int totalCell
        {
            get
            {
                return gridCoordinate.x * gridCoordinate.z;
            }
        }

        /// <summary>
        /// �O���b�h�̐^�񒆂� localPosition ��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 gridMiddle
        {
            get
            {
                return new Vector3(((float)gridBreadth - 1) / 2 * cellDepth, y, ((float)gridBreadth - 1) / 2 * cellWidth);
            }
        }

        /*=====�R���X�g���N�^=====*/
        /// <summary>
        /// GridField�����������܂�
        /// </summary>
        /// <param name="gridBreadth">�O���b�h�̍L��</param>
        /// /// <param name="cellWidth">�Z���̉���</param>
        /// /// <param name="cellDepth">�Z���̉��s</param>
        /// <param name="y">�O���b�h��y���W</param>
        /// <returns>GridField�̏�����</returns>
        public GridField(int gridBreadth = 10, float cellWidth = 10, float cellDepth = 10, float y = 0)
        {
            // �O���b�h�̍L�����
            this.gridBreadth = gridBreadth;

            // �Z���̉������
            this.cellWidth = cellWidth;

            // �Z���̉��s����
            this.cellDepth = cellDepth;

            // �O���b�h�̍�������
            this.y = y;

            /*===��d���[�v��grid�z��̂��ꂼ���Vector3�̍��W�l����===*/
            for (int x = 0; x < gridBreadth; x++)
            {
                for (int z = 0; z < gridBreadth; z++)
                {
                    grid[x, z] = new Vector3(x * cellWidth, y, z * cellDepth);    // x��z��10���������l����
                }
            }
        }

        /*=====�����ɗ^���� Transform ���ǂ��̃O���b�h���W�ɂ���̂���Ԃ�=====*/
        // ����:Transform
        // �߂�l:������transform�̂���Z���̃O���b�h���W
        /// <summary>
        /// �O���b�h���W�̂ǂ��Ȃ̂��𒲂ׂ܂�
        /// </summary>
        /// <param name="grid">���ׂ����O���b�h</param>
        /// <returns></returns>
        /// <param name="pos">���ׂ����O���b�h�̂ǂ��̃Z���ɂ���̂����ׂ���Transform</param>
        /// <returns>Transform�̂���Z���̃O���b�h���W</returns>
        public static Vector3Int GetGridCoordinate(GridField grid, Transform pos)
        {
            /*===��d���[�v�Ō��݂̃Z���𒲂ׂ�===*/
            for (grid.gridCoordinate.x = 0; grid.gridCoordinate.x < grid.gridBreadth; grid.gridCoordinate.x++)
            {
                for (grid.gridCoordinate.z = 0; grid.gridCoordinate.z < grid.gridBreadth; grid.gridCoordinate.z++)
                {
                    if (pos.position.x <= grid.grid[grid.gridCoordinate.x, grid.gridCoordinate.z].x + grid.cellWidth / 2 &&
                        pos.position.x >= grid.grid[grid.gridCoordinate.x, grid.gridCoordinate.z].x - grid.cellWidth / 2 &&
                        pos.position.z <= grid.grid[grid.gridCoordinate.x, grid.gridCoordinate.z].z + grid.cellDepth / 2 &&
                        pos.position.z >= grid.grid[grid.gridCoordinate.x, grid.gridCoordinate.z].z - grid.cellDepth / 2)     // ��������Z���̏�ɂ���Ȃ�
                    {
                        return grid.gridCoordinate;                      // �Z���� Vector3��Ԃ�
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