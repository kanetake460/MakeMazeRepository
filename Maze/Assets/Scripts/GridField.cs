using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

namespace TakeshiClass
{
    public class GridField : MonoBehaviour
    {

        /*�p�u���b�N�ϐ�*/
        public int gridWidth;               // �O���b�h�̍L��
        public int gridDepth;               //
        public int gridHeight;
        public float cellWidth;
        public float cellDepth;
        public float y;
        public eGridAnchor gridAnchor;
        public Vector3[,] grid = new Vector3[100, 100];      // �O���b�h�̃Z���̔z�uVector3�̓񎟌��z��
        public Vector3Int gridCoordinate;// = Vector3Int.zero;                                         // �O���b�h���W


        public enum eGridAnchor
        {
            center,
            bottomLeft
        }


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
        public Vector3 middle
        {
            get
            {
                // �������s���ǂ���Ƃ�����
                if (gridWidth % 2 == 0 && gridDepth % 2 == 0)
                {
                    // �O���b�h���W����Z���̔����̐����炵���l��Ԃ�
                    return grid[gridWidth / 2, gridDepth / 2] - new Vector3(cellWidth / 2, 0, cellDepth / 2);

                }
                // ����������
                else if (gridWidth % 2 == 0)
                {
                    // �O���b�h���W���炩��Z���̔����̐������炵���l��Ԃ�(�����̂�)
                    return grid[gridWidth / 2, gridDepth / 2] - new Vector3(cellWidth / 2, 0, 0);
                }
                // ���s������
                else if (gridDepth % 2 == 0)
                {
                    // �O���b�h���W����Z���̔����̐������炵���l��Ԃ�(���s�̂�)
                    return grid[gridWidth / 2, gridDepth / 2] - new Vector3(0, 0, cellDepth / 2);
                }
                // �ǂ���Ƃ��
                else
                {
                    // �O���b�h���W��Ԃ�
                    return grid[gridWidth / 2, gridDepth / 2];
                }
            }
        }


        /// <summary>
        /// �O���b�h�̍����̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 bottomLeft
        {
            get
            {
                return grid[0, 0] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// �O���b�h�̉E���̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 bottomRight
        {
            get
            {
                return�@grid[gridWidth - 1, 0] + new Vector3(cellWidth / 2, y, cellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// �O���b�h�̍���̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 topLeft
        {
            get
            {
                return grid[0, gridDepth - 1] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2);
            }
        }

        /// <summary>
        /// �O���b�h�̉E��̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 topRight
        {
            get
            {
                return grid[gridWidth - 1, gridDepth - 1] + new Vector3(cellWidth / 2, y, cellDepth / 2);
            }
        }



        /*=====�R���X�g���N�^=====*/
        /// <summary>
        /// GridField�����������܂�
        /// </summary>
        /// <param name="gridBreadth">�O���b�h�̍L��</param>
        /// <param name="cellWidth">�Z���̉���</param>
        /// <param name="cellDepth">�Z���̉��s</param>
        /// <param name="y">�O���b�h��y���W</param>
        /// <param name="gridAnchor">�O���b�h�̃A���J�[�ʒu</param>
        /// <returns>GridField�̏�����</returns>
        public GridField(int gridWidth = 10, int gridDepth = 10, float cellWidth = 10, float cellDepth = 10, float y = 0 ,eGridAnchor gridAnchor = eGridAnchor.center)
        {
            // �O���b�h�̉������
            this.gridWidth = gridWidth;

            // �O���b�h�̉��s���
            this.gridDepth = gridDepth;

            // �Z���̉������
            this.cellWidth = cellWidth;

            // �Z���̉��s����
            this.cellDepth = cellDepth;

            // �O���b�h�̃A���J�[�ʒu����
            this.gridAnchor = gridAnchor;

            // �O���b�h�̍�������
            this.y = y;

            if(gridWidth > 100 || gridDepth > 100)
            {
                Debug.LogError("���S�̂��ߍL������O���b�h�͐����ł��܂���");
                Debug.Break();
            }

            /*===��d���[�v��grid�z��̂��ꂼ���Vector3�̍��W�l����===*/
            for (int x = 0; x < gridWidth; x += 1)
            {
                for (int z = 0; z < gridDepth; z += 1)
                {
                    if (gridAnchor == eGridAnchor.center)
                    {
                        grid[x, z] = new Vector3(x * cellWidth, y, z * cellDepth) - new Vector3((float)(gridWidth - 1) / 2 * cellWidth, 0, (float)(gridDepth - 1) / 2 * cellDepth);    // x��z��10���������l����
                    }
                    else if (gridAnchor == eGridAnchor.bottomLeft)
                    {
                        grid[x, z] = new Vector3(x * cellWidth, y, z * cellDepth);    // x��z�ɃZ���̑傫�����������l����
                    }
                }
            }
        }

        ///<summary>
        ///�V�[���E�B���h�E�ɃO���b�h��\�����܂�
        ///</summary>
        ///<param name="gridField">�\���������O���b�h</param>
        public static void DrowGrid(GridField gridField)
        {
            // ���̍s
            for (int z = 1; z < gridField.gridDepth; z++)
            {
                Vector3 gridLineStart = gridField.grid[0, z] + new Vector3(gridField.cellWidth / 2 * -1, gridField.y, gridField.cellDepth / 2 * -1);
                Vector3 gridLineEnd = gridField.grid[gridField.gridWidth - 1, z] + new Vector3(gridField.cellWidth / 2, gridField.y, gridField.cellDepth / 2 * -1);

                Debug.DrawLine(gridLineStart, gridLineEnd, Color.red);
            }

            // ���̗�
            for (int x = 1; x < gridField.gridWidth; x++)
            {
                Vector3 gridRowStart = gridField.grid[x, 0] + new Vector3(gridField.cellWidth / 2 * -1, gridField.y, gridField.cellDepth / 2 * -1);
                Vector3 gridRowEnd = gridField.grid[x, gridField.gridDepth - 1] + new Vector3(gridField.cellWidth / 2 * -1, gridField.y, gridField.cellDepth / 2);

                Debug.DrawLine(gridRowStart, gridRowEnd, Color.red);
            }

            // �[�̃O���b�h���\��
            // �ŏ��̗�
            Debug.DrawLine(gridField.bottomLeft, gridField.topLeft, Color.green);

            // �Ō�̗�
            Debug.DrawLine(gridField.bottomRight, gridField.topRight, Color.green);


            // �ŏ��̍s
            Debug.DrawLine(gridField.bottomLeft, gridField.bottomRight, Color.green);

            // �Ō�̍s
            Debug.DrawLine(gridField.topLeft, gridField.topRight, Color.green);
        }

        /*=====�����ɗ^���� Transform ���ǂ��̃O���b�h���W�ɂ���̂���Ԃ�=====*/
        /// <summary>
        /// �O���b�h���W�̂ǂ��Ȃ̂��𒲂ׂ܂�
        /// </summary>
        /// <param name="gridField">���ׂ����O���b�h</param>
        /// <returns></returns>
        /// <param name="pos">���ׂ����O���b�h�̂ǂ��̃Z���ɂ���̂����ׂ���Transform</param>
        /// <returns>Transform�̂���Z���̃O���b�h���W</returns>
        public static Vector3Int GetGridCoordinate(GridField gridField, Transform pos)
        {
            /*===��d���[�v�Ō��݂̃Z���𒲂ׂ�===*/
            for (gridField.gridCoordinate.x = 0; gridField.gridCoordinate.x < gridField.gridWidth; gridField.gridCoordinate.x++)
            {
                for (gridField.gridCoordinate.z = 0; gridField.gridCoordinate.z < gridField.gridDepth; gridField.gridCoordinate.z++)
                {
                    if (pos.position.x <= gridField.grid[gridField.gridCoordinate.x, gridField.gridCoordinate.z].x + gridField.cellWidth / 2 &&
                        pos.position.x >= gridField.grid[gridField.gridCoordinate.x, gridField.gridCoordinate.z].x - gridField.cellWidth / 2 &&
                        pos.position.z <= gridField.grid[gridField.gridCoordinate.x, gridField.gridCoordinate.z].z + gridField.cellDepth / 2 &&
                        pos.position.z >= gridField.grid[gridField.gridCoordinate.x, gridField.gridCoordinate.z].z - gridField.cellDepth / 2)     // ��������Z���̏�ɂ���Ȃ�
                    {
                        return gridField.gridCoordinate;                      // �Z���� Vector3��Ԃ�
                    }
                }
            }
            Debug.LogError("�^����ꂽ�|�W�V�����̓O���b�h�t�B�[���h�̏�ɂ��܂���B");
            return Vector3Int.zero;
        }


        /// <summary>
        /// �O���b�h�̂ǂ���position�Ȃ̂��𒲂ׂ܂�
        /// </summary>
        /// <param name="gridField">���ׂ����O���b�h</param>
        /// <returns></returns>
        /// <param name="pos">���ׂ����O���b�h�̂ǂ��̃Z���ɂ���̂����ׂ���Transform</param>
        /// <returns>Transform�̂���Z����position</returns>
        public static Vector3 GetGridPosition(GridField gridField, Transform pos)
        {
            return gridField.grid[GetGridCoordinate(gridField,pos).x, GetGridCoordinate(gridField, pos).z];
        }

        public static Vector3Int MeasureToOtherCell

        void Update()
        {
            
        }
    }
}