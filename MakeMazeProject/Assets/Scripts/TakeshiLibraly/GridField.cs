using System;
using UnityEngine;

namespace TakeshiLibrary
{
    /*=====�O���b�h�t�B�[���h���쐬����֐�=====*/
    // Vector3�̃N���X���Q�l�ɍ쐬���܂���
    // C:\Users\kanet\AppData\Local\Temp\MetadataAsSource\b33e6428b1fe4c03a5b0b222eb1e9f0b\DecompilationMetadataAsSourceFileProvider\4496430b4e32462b86d5e9f4984747a4\Vector3.cs
    [Serializable]
    public struct Coord
    {
        [SerializeField] private int m_X;
        [SerializeField] private int m_Z;

        public int x
        {
            get => m_X;set => m_X = value;
        }

        public int z
        {
            get => m_Z;set => m_Z = value;
        }


        private static readonly Coord s_Zero = new Coord(0, 0);

        private static readonly Coord s_One = new Coord(1, 1);

        private static readonly Coord s_Left = new Coord(-1, 0);

        private static readonly Coord s_Right = new Coord(1, 0);

        private static readonly Coord s_Forward = new Coord(0, 1);

        private static readonly Coord s_Back = new Coord(0, -1);

        public static Coord zero { get { return s_Zero; } }
        public static Coord one { get { return s_One; } }
        public static Coord left { get { return s_Left; } }
        public static Coord right { get { return s_Right; } }
        public static Coord forward { get { return s_Forward; } }
        public static Coord back { get { return s_Back; } }

        public int this[int index]
        {
            get
            {
                return index switch
                {
                    0 => m_X,
                    1 => m_Z,
                    _ => throw new IndexOutOfRangeException("Invalid Coord index!"),
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        m_X = value;
                        break;
                    case 1:
                        m_Z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Coord index!");
                }
            }
        }

        public Coord(int x,int z)
        {
            m_X = x;
            m_Z = z;
        }

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord(a.x + b.x, a.z + b.z);
        }

        public static Coord operator -(Coord a, Coord b)
        {
            return new Coord(a.x - b.x, a.z - b.z);
        }

        public static Coord operator *(Coord a, Coord b)
        {
            return new Coord(a.x * b.x, a.z * b.z);
        }

        public static Coord operator -(Coord a)
        {
            return new Coord(-a.x, -a.z);
        }

        public static Coord operator *(Coord a, int b)
        {
            return new Coord(a.x * b, a.z * b);
        }

        public static Coord operator *(int a, Coord b)
        {
            return new Coord(a * b.x, a * b.z);
        }

        public static Coord operator /(Coord a, int b)
        {
            return new Coord(a.x / b, a.z / b);
        }

        public static bool operator ==(Coord lhs, Coord rhs)
        {
            return lhs.x == rhs.x && lhs.z == rhs.z;
        }

        public static bool operator !=(Coord lhs, Coord rhs)
        {
            return !(lhs == rhs);
        }

        public float magnitude
        {
            get
            {
                return Mathf.Sqrt(x * x + z * z);
            }
        }

        public static float Distance(Coord a,Coord b)
        {
            return (a + b).magnitude;
        }

    }

    public class GridField
    {


        //======�ϐ�===========================================================================================================================

        public int gridWidth { get; }               // �O���b�h�̍L��
        public int gridDepth { get; }               //
        public int gridHeight { get; }
        public float cellWidth { get; }
        public float cellDepth { get; }
        public int y { get; }
        public eGridAnchor gridAnchor { get; }              // �O���b�h�̃A���J�[
        public Vector3[,] grid { get; } = new Vector3[100, 100];     // �O���b�h�̃Z���̔z�uVector3�̓񎟌��z��

        public enum eGridAnchor
        {
            center,
            bottomLeft
        }



        //======�ǂݎ���p�ϐ�===============================================================================================================

        /// <summary>
        /// �O���b�h�̃Z���̐���Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public int TotalCell
        {
            get
            {
                return gridWidth * gridDepth;
            }
        }

        /// <summary>
        /// �O���b�h�̕��Ɖ��s�̍ł���������Ԃ��܂��B
        /// </summary>
        public int GridMaxLength
        {
            get
            {
                return Mathf.Max(gridWidth, gridDepth);
            }
        }
        
        /// <summary>
        /// �O���b�h�̕��Ɖ��s�̍ł��Z������Ԃ��܂��B
        /// </summary>
        public int GridMinLength
        {
            get
            {
                return Mathf.Min(gridWidth, gridDepth);
            }
        }

        /// <summary>
        /// �Z���̕��Ɖ��s�̍ł���������Ԃ��܂��B
        /// </summary>
        public float CellMaxLength
        {
            get
            {
                return Mathf.Max(cellWidth, cellDepth);
            }
        }
        
        
        /// <summary>
        /// �Z���̕��Ɖ��s�̍ł���������Ԃ��܂��B
        /// </summary>
        public float CellMinLength
        {
            get
            {
                return Mathf.Min(cellWidth, cellDepth);
            }
        }



        /*==========�O���b�h�t�B�[���h�̊p�̃Z����Vector3���W==========*/
        /// <summary>
        ///�O���b�h�̍����̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 BottomLeftCell
        {
            get
            {
                return grid[0, 0];
            }
        }

        /// <summary>
        ///�O���b�h�̉E���̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 BottomRightCell
        {
            get
            {
                return grid[gridWidth - 1, 0];
            }
        }

        /// <summary>
        ///�O���b�h�̍���̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 TopLeftCell
        {
            get
            {
                return grid[0, gridDepth - 1];
            }
        }

        /// <summary>
        ///�O���b�h�̉E��̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 TopRightCell
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
        public Vector3 BottomLeft
        {
            get
            {
                return grid[0, 0] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// �O���b�h�̉E���̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 BottomRight
        {
            get
            {
                return grid[gridWidth - 1, 0] + new Vector3(cellWidth / 2, y, cellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// �O���b�h�̍���̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 TopLeft
        {
            get
            {
                return grid[0, gridDepth - 1] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2);
            }
        }

        /// <summary>
        /// �O���b�h�̉E��̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 TopRight
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
        public Vector3 Middle
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


        /*=========�O���b�h�t�B�[���h�̒��SVector3���W===========*/
        /// <summary>
        /// �O���b�h�̐^�񒆂� localPosition ��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Coord MiddleGrid
        {
            get
            {
                return new Coord(gridWidth / 2, gridDepth / 2);
            }
        }

        /*=========�����_��===========*/
        /// <summary>
        /// �O���b�h���W�̃����_���Ȉʒu��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 RandomGridPos
        {
            get
            {
                int randX = UnityEngine.Random.Range(0, gridWidth);
                int randZ = UnityEngine.Random.Range(0, gridDepth);
                return grid[randX, randZ];
            }
        }

        /// <summary>
        /// �����_���ȃO���b�h���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Coord RandomGridCoord
        {
            get
            {
                int randX = UnityEngine.Random.Range(0, gridWidth);
                int randZ = UnityEngine.Random.Range(0, gridDepth);
                return new Coord(randX, randZ);
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
        public GridField(int gridWidth = 10, int gridDepth = 10, float cellWidth = 10, float cellDepth = 10, int y = 0, eGridAnchor gridAnchor = eGridAnchor.center)
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
            Debug.DrawLine(BottomLeft, TopLeft, Color.green);

            // �Ō�̗�
            Debug.DrawLine(BottomRight, TopRight, Color.green);


            // �ŏ��̍s
            Debug.DrawLine(BottomLeft, BottomRight, Color.green);

            // �Ō�̍s
            Debug.DrawLine(TopLeft, TopRight, Color.green);
        }


        /// <summary>
        /// �����ɗ^���� Transform ���ǂ��̃O���b�h���W�ɂ���̂���Ԃ�
        /// </summary>
        /// <param name="pos">���ׂ����O���b�h�̂ǂ��̃Z���ɂ���̂����ׂ���Transform</param>
        /// <returns>Transform�̂���Z���̃O���b�h���W</returns>
        public Coord GridCoordinate(Vector3 pos)
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
                        return new Coord(x, z);                      // �Z���� Vector3��Ԃ�
                    }
                }
            }
            Debug.LogError("�^����ꂽ�|�W�V�����̓O���b�h�t�B�[���h�̏�ɂ��܂���B");
            return Coord.zero;
        }


        /// <summary>
        /// �����ɗ^���� Transform ���ǂ��� position �Ȃ̂��𒲂ׂ܂�
        /// </summary>
        /// <param name="pos">���ׂ����O���b�h�̂ǂ��̃Z���ɂ���̂����ׂ���Transform</param>
        /// <returns>Transform�̂���Z����position</returns>
        public Vector3 GridPosition(Vector3 pos)
        {
            return grid[GridCoordinate(pos).x, GridCoordinate(pos).z];
        }


        /// <summary>
        /// �����ɗ^���� Vector3position �� �O���b�h���W�ɕϊ����܂�
        /// </summary>
        /// <param name="pos">�ϊ��������|�W�V����</param>
        public void ConvertVector3ToGridCoord(ref Vector3 pos)
        {
            pos = GridPosition(pos);
        }


        /// <summary>
        /// �^����position����C�ӂ̋����̂ق���position�̃O���b�h���W�𒲂ׂ܂�
        /// </summary>
        /// <param name="pos">���ׂ��������̎n�_��Vector3���W</param>
        /// <param name="difference">�n�_����I�_�܂ł̍���</param>
        public Coord OherGridCoordinate(Vector3 pos, Coord difference)
        {
            int x = GridCoordinate(pos).x;
            int z = GridCoordinate(pos).z;

            return new Coord(x + difference.x, z + difference.z);
        }


        /// <summary>
        /// �^����position����C�ӂ̋����̂ق���position��Vector3���W�𒲂ׂ܂�
        /// </summary>
        /// <param name="pos">���ׂ��������̎n�_��Vecgtor3���W</param>
        /// <param name="difference">�n�_����I�_�܂ł̍���</param>
        public Vector3 OtherGridPosition(Vector3 pos, Coord difference)
        {
            int x = GridCoordinate(pos).x;
            int z = GridCoordinate(pos).z;

            return grid[x + difference.x, z + difference.z];
        }

        /// <summary>
        /// �����ɑΉ�����ЂƂO�̃O���b�h���W��Ԃ��܂�
        /// </summary>
        /// <param name="fourDirection">����</param>
        /// <returns>�����Ă�������̈�O�̃O���b�h���W</returns>
        public Coord GetPreviousCoordinate(FPS.eFourDirection fourDirection)
        {
            switch (fourDirection)
            {
                case FPS.eFourDirection.top:
                    return Coord.forward;

                case FPS.eFourDirection.bottom:
                    return Coord.back;

                case FPS.eFourDirection.left:
                    return Coord.left;

                case FPS.eFourDirection.right:
                    return Coord.right;
            }
            return Coord.zero;
        }

        /// <summary>
        /// �^���� posistion ���O���b�h�̏�ɂ��邩�ǂ������ׂ܂�
        /// </summary>
        /// <param name="pos">���ׂ����|�W�V����</pragma>
        public bool CheckOnGridPos(Vector3 pos)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    if (GridCoordinate(pos) == GridCoordinate(grid[x, z]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// �^�����O���b�h���W���O���b�h�̏�ɂ��邩�ǂ������ׂ܂�
        /// </summary>
        /// <param name="coord">���ׂ����|�W�V����</pragma>
        public bool CheckOnGridCoord(Vector3Int coord)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    if (coord == new Vector3Int(x,y,z))
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
            Gizmos.DrawLine(BottomLeft, TopLeft);

            // �Ō�̗�
            Gizmos.DrawLine(BottomRight, TopRight);


            // �ŏ��̍s
            Gizmos.DrawLine(BottomLeft, BottomRight);

            // �Ō�̍s
            Gizmos.DrawLine(TopLeft, TopRight);

        }
    }
}