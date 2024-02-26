using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

namespace TakeshiLibrary
{
    /*=====�O���b�h�t�B�[���h���쐬����֐�=====*/
    // Vector3�̃N���X���Q�l�ɍ쐬���܂���
    // C:\Users\kanet\AppData\Local\Temp\MetadataAsSource\b33e6428b1fe4c03a5b0b222eb1e9f0b\DecompilationMetadataAsSourceFileProvider\4496430b4e32462b86d5e9f4984747a4\Vector3.cs
    public class GridField
    {


        //======�ϐ�===========================================================================================================================

        public int gridWidth { get; }               // �O���b�h�̍L��
        public int gridDepth { get; }               //
        public int gridHeight { get; }
        public float cellWidth { get; }
        public float cellDepth { get; }
        public float y { get; }
        public eGridAnchor gridAnchor { get; }              // �O���b�h�̃A���J�[
        public Vector3[,] grid { get; } = new Vector3[100, 100];     // �O���b�h�̃Z���̔z�uVector3�̓񎟌��z��

        public enum eGridAnchor
        {
            center,
            bottomLeft
        }



        //======�ǂݎ���p�ϐ�===============================================================================================================

        /*==========�O���b�h�t�B�[���h�̊p�̃Z����Vector3���W==========*/
        /// <summary>
        /// �O���b�h�̃Z���̐���Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public int totalCell
        {
            get
            {
                return gridWidth * gridDepth;
            }
        }

        /// <summary>
        ///�O���b�h�̍����̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 bottomLeftCell
        {
            get
            {
                return grid[0, 0];
            }
        }

        /// <summary>
        ///�O���b�h�̉E���̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 bottomRightCell
        {
            get
            {
                return grid[gridWidth - 1, 0];
            }
        }

        /// <summary>
        ///�O���b�h�̍���̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 topLeftCell
        {
            get
            {
                return grid[0, gridDepth - 1];
            }
        }

        /// <summary>
        ///�O���b�h�̉E��̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 topRightCell
        {
            get
            {
                return grid[gridWidth - 1, gridDepth - 1];
            }
        }



        /*==========�O���b�h�t�B�[���h�̊p��Vector3���W==========*/
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
                return grid[gridWidth - 1, 0] + new Vector3(cellWidth / 2, y, cellDepth / 2 * -1);
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



        /*=========�O���b�h�t�B�[���h�̒��SVector3���W===========*/
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
        /// �O���b�h���W�̃����_���Ȉʒu��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 randomGrid
        {
            get
            {
                int randX = Random.Range(0, gridWidth);
                int randZ = Random.Range(0, gridDepth);
                return grid[randX, randZ];
            }
        }

        /// <summary>
        /// �����_���ȃO���b�h���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3Int randomGridCoord
        {
            get
            {
                int randX = Random.Range(0, gridWidth);
                int randZ = Random.Range(0, gridDepth);
                return new Vector3Int(randX, 0, randZ);
            }
        }

        //======�R���X�g���N�^=================================================================================================================

        /// <summary>
        /// GridField�����������܂�
        /// </summary>
        /// <param name="gridWidth">�O���b�h�̉���</param>
        /// <param name="gridDepth">�O���b�h�̉��s</param>
        /// <param name="cellWidth">�Z���̉���</param>
        /// <param name="cellDepth">�Z���̉��s</param>
        /// <param name="y">�O���b�h��y���W</param>
        /// <param name="gridAnchor">�O���b�h�̃A���J�[�ʒu</param>
        /// <returns>GridField�̏�����</returns>
        public GridField(int gridWidth = 10, int gridDepth = 10, float cellWidth = 10, float cellDepth = 10, float y = 0, eGridAnchor gridAnchor = eGridAnchor.center)
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

            if (gridWidth > 100 || gridDepth > 100)
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



        //======�֐�===========================================================================================================================

        ///<summary>
        ///�V�[���E�B���h�E�ɃO���b�h��\�����܂�
        ///</summary>
        public void DrowGrid()
        {
            // ���̍s
            for (int z = 1; z < gridDepth; z++)
            {
                Vector3 gridLineStart = grid[0, z] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
                Vector3 gridLineEnd = grid[gridWidth - 1, z] + new Vector3(cellWidth / 2, y, cellDepth / 2 * -1);

                Debug.DrawLine(gridLineStart, gridLineEnd, Color.red);
            }

            // ���̗�
            for (int x = 1; x < gridWidth; x++)
            {
                Vector3 gridRowStart = grid[x, 0] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
                Vector3 gridRowEnd = grid[x, gridDepth - 1] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2);

                Debug.DrawLine(gridRowStart, gridRowEnd, Color.red);
            }

            // �[�̃O���b�h���\��
            // �ŏ��̗�
            Debug.DrawLine(bottomLeft, topLeft, Color.green);

            // �Ō�̗�
            Debug.DrawLine(bottomRight, topRight, Color.green);


            // �ŏ��̍s
            Debug.DrawLine(bottomLeft, bottomRight, Color.green);

            // �Ō�̍s
            Debug.DrawLine(topLeft, topRight, Color.green);
        }


        /// <summary>
        /// �����ɗ^���� Transform ���ǂ��̃O���b�h���W�ɂ���̂���Ԃ�
        /// </summary>
        /// <param name="pos">���ׂ����O���b�h�̂ǂ��̃Z���ɂ���̂����ׂ���Transform</param>
        /// <returns>Transform�̂���Z���̃O���b�h���W</returns>
        public Vector3Int GetGridCoordinate(Vector3 pos)
        {
            /*===��d���[�v�Ō��݂̃Z���𒲂ׂ�===*/
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    if (pos.x <= grid[x, z].x + cellWidth / 2 &&
                        pos.x >= grid[x, z].x - cellWidth / 2 &&
                        pos.z <= grid[x, z].z + cellDepth / 2 &&
                        pos.z >= grid[x, z].z - cellDepth / 2)     // ��������Z���̏�ɂ���Ȃ�
                    {
                        return new Vector3Int(x, (int)y, z);                      // �Z���� Vector3��Ԃ�
                    }
                }
            }
            Debug.LogError("�^����ꂽ�|�W�V�����̓O���b�h�t�B�[���h�̏�ɂ��܂���B");
            return Vector3Int.zero;
        }


        /// <summary>
        /// �����ɗ^���� Transform ���ǂ��� position �Ȃ̂��𒲂ׂ܂�
        /// </summary>
        /// <param name="gridField">���ׂ����O���b�h</param>
        /// <returns></returns>
        /// <param name="pos">���ׂ����O���b�h�̂ǂ��̃Z���ɂ���̂����ׂ���Transform</param>
        /// <returns>Transform�̂���Z����position</returns>
        public Vector3 GetGridPosition(Vector3 pos)
        {
            return grid[GetGridCoordinate(pos).x, GetGridCoordinate(pos).z];
        }


        /// <summary>
        /// �����ɗ^���� Vector3position �� �O���b�h���W�ɕϊ����܂�
        /// </summary>
        /// <param name="pos">�ϊ��������|�W�V����</param>
        public void ConvertVector3ToGridCoord(ref Vector3 pos)
        {
            pos = GetGridPosition(pos);
        }


        /// <summary>
        /// �����ɗ^���� �O���b�h���W ���� Vector3�|�W�V���� ��Ԃ��܂�
        /// </summary>
        /// <param name="gridCoord">�O���b�h���W</param>
        /// <returns>Vecto3�|�W�V����</returns>
        public Vector3 GetVector3Position(Vector3Int gridCoord)
        {
            return grid[gridCoord.x, gridCoord.z];
        }

        /// <summary>
        /// �^����position����C�ӂ̋����̂ق���position�̃O���b�h���W�𒲂ׂ܂�
        /// </summary>
        /// <param name="gridField">���ׂ����O���b�h</param>
        /// <param name="pos">���ׂ��������̎n�_��Vector3���W</param>
        /// <param name="difference">�n�_����I�_�܂ł̍���</param>
        public Vector3Int GetOtherGridCoordinate(Vector3 pos, Vector3Int difference)
        {
            int x = GetGridCoordinate(pos).x;
            int z = GetGridCoordinate(pos).z;

            return new Vector3Int(x + difference.x, 0, z + difference.z);
        }


        /// <summary>
        /// �^����position����C�ӂ̋����̂ق���position��Vector3���W�𒲂ׂ܂�
        /// </summary>
        /// <param name="pos">���ׂ��������̎n�_��Vecgtor3���W</param>
        /// <param name="difference">�n�_����I�_�܂ł̍���</param>
        public Vector3 GetOtherGridPosition(Vector3 pos, Vector3Int difference)
        {
            int x = GetGridCoordinate(pos).x;
            int z = GetGridCoordinate(pos).z;

            return grid[x + difference.x, z + difference.z];
        }

        /// <summary>
        /// �����ɑΉ�����ЂƂO�̃O���b�h���W��Ԃ��܂�
        /// </summary>
        /// <param name="fourDirection">����</param>
        /// <returns>�����Ă�������̈�O�̃O���b�h���W</returns>
        public Vector3Int GetPreviousCoordinate(FPS.eFourDirection fourDirection)
        {
            switch (fourDirection)
            {
                case FPS.eFourDirection.top:
                    return Vector3Int.forward;

                case FPS.eFourDirection.bottom:
                    return Vector3Int.back;

                case FPS.eFourDirection.left:
                    return Vector3Int.left;

                case FPS.eFourDirection.right:
                    return Vector3Int.right;
            }
            return Vector3Int.zero;
        }

        /// <summary>
        /// �^���� posistion ���O���b�h�̏�ɂ��邩�ǂ������ׂ܂�
        /// </summary>
        /// <param name="pos">���ׂ����|�W�V����</pragma>
        public bool CheckOnGrid(Vector3 pos)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    if (GetGridCoordinate(pos) == grid[x, z])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            // ���̍s
            for (int z = 1; z < gridDepth; z++)
            {
                Vector3 gridLineStart = grid[0, z] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
                Vector3 gridLineEnd = grid[gridWidth - 1, z] + new Vector3(cellWidth / 2, y, cellDepth / 2 * -1);

                Gizmos.DrawLine(gridLineStart, gridLineEnd);
            }

            // ���̗�
            for (int x = 1; x < gridWidth; x++)
            {
                Vector3 gridRowStart = grid[x, 0] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
                Vector3 gridRowEnd = grid[x, gridDepth - 1] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2);

                Gizmos.DrawLine(gridRowStart, gridRowEnd);
            }

            // �[�̃O���b�h���\��
            // �ŏ��̗�
            Gizmos.DrawLine(bottomLeft, topLeft);

            // �Ō�̗�
            Gizmos.DrawLine(bottomRight, topRight);


            // �ŏ��̍s
            Gizmos.DrawLine(bottomLeft, bottomRight);

            // �Ō�̍s
            Gizmos.DrawLine(topLeft, topRight);

        }
    }
}