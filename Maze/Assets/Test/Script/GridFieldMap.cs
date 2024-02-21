using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using Unity.VisualScripting;
using System.Linq;

    /// <summary>
    /// �}�b�v�N���X
    /// </summary>
    public class GridFieldMap
    {
        /// <summary>
        /// �u���b�N�N���X
        /// </summary>
        public class Block
        {
            // �u���b�N�̃O���b�h���W
            public Vector3Int coord { get; }
            // �u���b�N�̎��
            public bool isSpace { get; set; }

            // �ǂ����邩�ǂ���( �ǂ����� : true )
            public bool fowardWall { get; set; } = false;
            public bool rightWall { get; set; } = false;
            public bool backWall { get; set; } = false;
            public bool leftWall { get; set; } = false;


            /// <summary>
            /// �u���b�N�ɏ�������R���X�g���N�^
            /// </summary>
            /// <param name="x">x�O���b�h���W</param>
            /// <param name="z">z�O���b�h���W</param>
            /// <param name="isSpace">�ǂ��A��Ԃ�</param>
            public Block(int x, int z, bool isSpace = true)
            {
                coord = new Vector3Int(x, 0, z);
                this.isSpace = isSpace;
            }

            /// <summary>
            /// �^����Vector3���W�̌������ǂȂ̂��ǂ������ׂ܂�
            /// </summary>
            /// <param name="x">��( -1 or 0 )</param>
            /// <param name="z">��( -1 or 0 )</param>
            /// <returns>�ǂ��ǂ��Ȃ̂�</returns>
            public bool GetWallDirection(Vector3 dir)
            {
                if (dir == Vector3.right) return rightWall;
                else if (dir == Vector3.left) return leftWall;
                else if (dir == Vector3.forward) return fowardWall;
                else if (dir == Vector3.back) return backWall;
                else return false;
            }

            /// <summary>
            /// �^�������W�̌������ǂȂ̂��ǂ������ׂ܂�
            /// </summary>
            /// <param name="x">��( -1 or 0 )</param>
            /// <param name="z">��( -1 or 0 )</param>
            /// <returns>�ǂ��ǂ��Ȃ̂�</returns>
            public bool CheckWall(int x, int z)
            {
                Vector3 checkDir = new Vector3(x, 0, z);
                //Debug.Log(checkDir);
                return GetWallDirection(checkDir);
            }
        }

        // �u���b�N�̓񎟌��z��
        public Block[,] blocks { get; } = new Block[100,100];

        public GridField gridField { get;}


    /// <summary>
    /// �}�b�v���쐬����R���X�g���N�^�ł�
    /// </summary>
    /// <param name="gridWidth">�O���b�h�̉���</param>
    /// <param name="gridDepth">�O���b�h�̉��s</param>
    /// <param name="t">�u���b�N�̃^�C�v</param>
    public GridFieldMap(GridField gridField)
    {
        this.gridField = gridField;
        for (int x = 0; x < gridField.gridWidth; x++)
        {
            for (int z = 0; z < gridField.gridDepth; z++)
            {
                blocks[x, z] = new Block(x, z);
            }
        }
    }


        /// <summary>
        /// �w�肵�����W�̃u���b�N�A������ǂɐݒ肵�܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        public void SetWallBlock(int x, int z)
        {
            blocks[x, z].isSpace = false;
        }


        /// <summary>
        /// �w�肵�����W�̃u���b�N�A������ǂɐݒ肵�܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        /// <param name="dir">�ǂ��������</param>
        public void SetWall(int x, int z, Vector3 dir)
        {
            if (dir == Vector3.forward) blocks[x, z].fowardWall = true;
            else if (dir == Vector3.right) blocks[x, z].rightWall = true;
            else if (dir == Vector3.back) blocks[x, z].backWall = true;
            else if (dir == Vector3.left) blocks[x, z].leftWall = true;
        }


        /// <summary>
        /// �w�肵�����W�̃u���b�N�A�����̕ǂ��Ȃ����܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        /// <param name="dir">�ǂ��������</param>
        public void BreakWall(int x, int z, Vector3 dir)
        {
            if (dir == Vector3.forward) blocks[x, z].fowardWall = false;
            else if (dir == Vector3.right) blocks[x, z].rightWall = false;
            else if (dir == Vector3.back) blocks[x, z].backWall = false;
            else if (dir == Vector3.left) blocks[x, z].leftWall = false;
        }


        /// <summary>
        /// �}�b�v�̃I�u�W�F�N�g�𐶐����܂�
        /// </summary>
        /// <param name="space">space�̃I�u�W�F�N�g</param>
        /// <param name="wall">wall�̃I�u�W�F�N�g</param>
        /// <param name="gf">gridField</param>
        public void InstanceMapObjects(GameObject space,GameObject wall)
        {
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    if (blocks[x, z].isSpace) MonoBehaviour.Instantiate(space, gridField.grid[blocks[x, z].coord.x, blocks[x, z].coord.z], Quaternion.identity);
                    else if (blocks[x, z].isSpace == false) MonoBehaviour.Instantiate(wall, gridField.grid[blocks[x,z].coord.x, blocks[x, z].coord.z] + new Vector3(0,5,0), Quaternion.identity);
                }
            }
        }


        /// <summary>
        /// �O���b�h��ɕǂ𐶐����܂�
        /// </summary>
        public void SetWallGrid()
        {
            // �ǂ�ݒ�
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    if (x % 2 == 1 && z % 2 == 1)
                    {
                        SetWall(x, z, Vector3.left);
                        SetWall(x, z, Vector3.right);
                        SetWall(x, z, Vector3.forward);
                        SetWall(x, z, Vector3.back);
                        SetWallBlock(x, z);
                    }
                }
            }
        }

    /// <summary>
    /// �^�����O���b�h���W���}�b�v�Ȃ��Ȃ�true��Ԃ��܂�
    /// </summary>
    /// <param name="coord">���W</param>
    /// <returns>�O���b�h�̏�Ȃ�true</returns>
    public bool CheckMap(Vector3Int coord)
    {
        return  coord.x >= 0 &&
                coord.z >= 0 && 
                coord.x < gridField.gridWidth &&
                coord.z < gridField.gridDepth;
    }


    /// <summary>
    /// �^����ꂽ�O���b�h���W����w��͈̔͂Ń����_���ȍ��W���擾���܂�
    /// </summary>
    public Vector3Int GetRandomPoint(Vector3Int coord, int areaX, int areaZ)
    {
        // �I��͈͂̃u���b�N�̃��X�g
        List<Block> lAreaBlock = new List<Block>();

        // �����͈͂̃u���b�N�����X�g�ɒǉ�
        for (int x = -areaX; x < areaX; x++)
        {
            for (int z = -areaZ; z < areaZ; z++)
            {
                if (!CheckMap(new Vector3Int(coord.x + x, 0, coord.z + z))) continue;
                Block b = blocks[coord.x + x, coord.z + z];
                lAreaBlock.Add(b);
            }
        }

        Vector3Int randCoord = coord;

        int count = 0;
        while (true)
        {
            count++;
            if(count >= areaX * areaZ)
            {
                randCoord = coord;
                Debug.Log("������܂���ł���");
                break;
            }

            // �G���A�͈͓��̃����_���Ȓl
            int randX = Random.Range(-areaX, areaX + 1);
            int randZ = Random.Range(-areaZ, areaZ + 1);

            randCoord = new Vector3Int(coord.x + randX, 0, coord.z + randZ);

            // ���X�g�̒��ɃX�y�[�X�ŁA�����_���Ȓl���W����v������̂�����΃��[�v�I��
            if (lAreaBlock.FindAll(b => b.isSpace == true).Find(b => b.coord == randCoord) != null) break;
        }
        return randCoord;
    }


        /// <summary>
        /// AStar�̓���ݒ肵�܂�
        /// </summary>
        /// <param name="start">�T���̍ŏ��̈ʒu</param>
        /// <param name="goal">�T���̃S�[���n�_</param>
        /// <param name="gridField">�O���b�h�t�B�[���h</param>
        /// <param name="pathObj">�o�H�ɔz�u����I�u�W�F�N�g</param>
        public void SetAStar(Vector3 start,Vector3 goal,GameObject pathObj,GridFieldAStar aStar)
        {
            if (aStar == null)
            {
                aStar = new GridFieldAStar(this, gridField.GetGridCoordinate(start), gridField.GetGridCoordinate(goal));
            }

            aStar.AStarPath();

            while (aStar.pathStack.Count > 0)
            {
                Vector3Int popedInfo = aStar.pathStack.Pop().position;
                MonoBehaviour.Instantiate(pathObj, gridField.grid[popedInfo.x, popedInfo.z], Quaternion.identity);
            }
        }
    }