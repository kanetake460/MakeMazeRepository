using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
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
            public Coord coord { get; }
            // �u���b�N�̎��
            public bool isSpace { get; set; }

            // �ǂ����邩�ǂ���( �ǂ����� : true )
            public bool fowardWall { get; set; } = false;
            public bool rightWall { get; set; } = false;
            public bool backWall { get; set; } = false;
            public bool leftWall { get; set; } = false;
            public GameObject mapWallObj { get; set; }      // �ǃI�u�W�F�N�g
            public GameObject mapPlaneObj { get; set; }     // ���I�u�W�F�N�g


            /// <summary>
            /// �u���b�N�ɏ�������R���X�g���N�^
            /// </summary>
            /// <param name="x">x�O���b�h���W</param>
            /// <param name="z">z�O���b�h���W</param>
            /// <param name="isSpace">�ǂ��A��Ԃ�</param>
            public Block(int x, int z, bool isSpace = true)
            {
                coord = new Coord(x, z);
                this.isSpace = isSpace;
            }

            /// <summary>
            /// �^����Vector3���W�̌������ǂȂ̂��ǂ������ׂ܂�
            /// </summary>
            /// <param>Vector3�̌���</param>
            /// <returns>�ǂ��ǂ��Ȃ̂�</returns>
            public bool CheckWallDirection(Vector3 dir)
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
                return CheckWallDirection(checkDir);
            }
        }


        // �u���b�N�̓񎟌��z��
        public Block[,] blocks { get; } = new Block[100, 100];
        // �O���b�h�t�B�[���h
        public GridField gridField { get; }

        /// <summary>
        /// �}�b�v���쐬����R���X�g���N�^�ł�
        /// </summary>
        /// <param name="gridField">�O���b�h�t�B�[���h</param>
        public GridFieldMap(GridField gridField)
        {
            this.gridField = gridField;
            // �O���b�h�t�B�[���h�̃Z���̍��W�ɍ��킹�ău���b�N�쐬
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    blocks[x, z] = new Block(x, z);
                }
            }
        }


        /// <summary>
        /// �w�肵�����W��ǃu���b�N�ɐݒ肵�܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        public void CreateWallBlock(int x, int z)
        {
            blocks[x, z].isSpace = false;
        }
        
        /// <summary>
        /// �w�肵���u���b�N��ǃu���b�N�ɐݒ肵�܂�
        /// </summary>
        /// <param name="block">�ǂɂ������u���b�N</param>
        public void CreateWallBlock(Block block)
        {
            block.isSpace = false;
        }


        /// <summary>
        /// �w�肵�����W�̃u���b�N�A������ǂɐݒ肵�܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        /// <param name="dir">�ǂ��������</param>
        public void CreateWallDirection(int x, int z, Vector3 dir)
        {
            if (dir == Vector3.forward) blocks[x, z].fowardWall = true;
            else if (dir == Vector3.right) blocks[x, z].rightWall = true;
            else if (dir == Vector3.back) blocks[x, z].backWall = true;
            else if (dir == Vector3.left) blocks[x, z].leftWall = true;
        }

        /// <summary>
        /// �w�肵���u���b�N�́A������ǂɐݒ肵�܂�
        /// </summary>
        /// <param name="block">�ݒ肵�����u���b�N</param>
        /// <param name="dir">�ǂ��������</param>
        public void CreateWallDirection(Block block, Vector3 dir)
        {
            if (dir == Vector3.forward) block.fowardWall = true;
            else if (dir == Vector3.right) block.rightWall = true;
            else if (dir == Vector3.back) block.backWall = true;
            else if (dir == Vector3.left) block.leftWall = true;
        }


        /// <summary>
        /// �^�������W�̂��ׂĂ̌����̕ǂ�ݒ肵�܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        /// <param name="foward">�O��</param>
        /// <param name="right">�E��</param>
        /// <param name="back">���</param>
        /// <param name="left">����</param>
        public void SetWallsDirection(int x, int z, bool foward = true, bool right = true, bool back = true, bool left = true,bool isSpace = false)
        {
            blocks[x, z].fowardWall = foward;
            blocks[x, z].rightWall = right;
            blocks[x, z].backWall = back;
            blocks[x, z].leftWall = left;
            blocks[x, z].isSpace = isSpace;
        }

        /// <summary>
        /// ���������u���b�N�̂��ׂĂ̌����̕ǂ�ݒ肵�܂�
        /// �f�t�H���g�����ł͕ǂ�����܂�
        /// </summary>
        /// <param name="back">�u���b�N</param>
        /// <param name="foward">�O��</param>
        /// <param name="right">�E��</param>
        /// <param name="back">���</param>
        /// <param name="left">����</param>
        public void SetWallsDirection(Block block, bool foward = true, bool right = true, bool back = true, bool left = true, bool isSpace = false)
        {
            block.fowardWall = foward;
            block.rightWall = right;
            block.backWall = back;
            block.leftWall = left;
            block.isSpace = isSpace;
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
        /// �w�肵���u���b�N�A�����̕ǂ��Ȃ����܂�
        /// </summary>
        /// <param name="block">�u���b�N</param>
        /// <param name="dir">�ǂ��������</param>
        public void BreakWall(Block block, Vector3 dir)
        {
            if (dir == Vector3.forward) block.fowardWall = false;
            else if (dir == Vector3.right) block.rightWall = false;
            else if (dir == Vector3.back) block.backWall = false;
            else if (dir == Vector3.left) block.leftWall = false;
        }


        /// <summary>
        /// �}�b�v�̃I�u�W�F�N�g�𐶐����܂�
        /// </summary>
        /// <param>�ǂ̍���</param>
        public void InstanceMapObjects(float scaleY = 10)
        {
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    // ���쐬
                    GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    plane.name = new string("Plane" + x + "," + z);
                    blocks[x, z].mapPlaneObj = plane;
                    plane.transform.SetPositionAndRotation(gridField.grid[blocks[x, z].coord.x, blocks[x, z].coord.z], Quaternion.identity);

                    // �Ǎ쐬
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    blocks[x, z].mapWallObj = cube;
                    blocks[x, z].mapWallObj.name = new string("Wall" + x + "," + z);
                    cube.transform.SetPositionAndRotation(gridField.grid[blocks[x, z].coord.x, blocks[x, z].coord.z], Quaternion.identity);
                    cube.transform.localScale = new Vector3(gridField.cellWidth, scaleY, gridField.cellDepth);
                }
            }
        }


        /// <summary>
        /// �ǃI�u�W�F�N�g�̃��C���[�}�X�N��ݒ肵�܂�
        /// </summary>
        /// <param name="layerName">���C���[</param>
        public void SetLayerMapObject(string layerName)
        {
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    blocks[x,z].mapWallObj.layer = LayerMask.NameToLayer(layerName);
                }
            }
        }


        /// <summary>
        /// �v���[���I�u�W�F�N�g�̐F��ς��܂�
        /// </summary>
        /// <param name="coord">�v���[���̍��W</param>
        /// <param name="color">�F</param>
        public void ChangePlaneColor(Coord coord,Color color)
        {
            blocks[coord.x, coord.z].mapPlaneObj.GetComponent<Renderer>().material.color = color;
        }


        /// <summary>
        /// �ǃI�u�W�F�N�g�̐F��ς��܂�
        /// </summary>
        /// <param name="coord">�ǂ̍��W</param>
        /// <param name="color">�F</param>
        public void ChangeWallColor(Coord coord,Color color)
        {
            blocks[coord.x, coord.z].mapWallObj.GetComponent<Renderer>().material.color = color;
        }


        /// <summary>
        /// �ǃI�u�W�F�N�g�̃e�N�X�`����ύX���܂�
        /// </summary>
        /// <param name="coord">�ǂ̍��W</param>
        /// <param name="texrure">�e�N�X�`��</param>
        public void ChangeWallTexture(Coord coord,Texture texrure)
        {
            blocks[coord.x, coord.z].mapWallObj.GetComponent<Renderer>().material.mainTexture = texrure;
        }


        /// <summary>
        /// ���ׂĂ̕ǃI�u�W�F�N�g�̃e�N�X�`����ύX���܂�
        /// </summary>
        /// <param name="texrure">�e�N�X�`��</param>
        public void ChangeWallTextureAll(Texture texrure)
        {
            Coord coord = new Coord(0,0);
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    coord.x = x;
                    coord.z = z;
                    ChangeWallTexture(coord,texrure);
                }
            }
        }


        /// <summary>
        /// �}�b�v�̕ǃI�u�W�F�N�g�̃A�N�e�B�u���Ǘ����܂�
        /// </summary>
        public void ActiveMapWallObject()
        {
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    blocks[x, z].mapWallObj.SetActive(!blocks[x, z].isSpace);
                }
            }
        }


        /// <summary>
        /// �}�b�v�̂��ׂẴu���b�N��ǂɐݒ肵�܂�
        /// </summary>
        public void CreateWallsAll()
        {
            // �ǂ�ݒ�
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                        SetWallsDirection(x, z);
                        CreateWallBlock(x, z);
                }
            }
        }


        /// <summary>
        /// �O���b�h��ɕǂ𐶐����܂�
        /// </summary>
        public void CreateWallsGrid()
        {
            // �ǂ�ݒ�
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    if (x % 2 == 1 && z % 2 == 1)
                    {
                        SetWallsDirection(x, z);
                        CreateWallBlock(x, z);
                    }
                }
            }
        }

        /// <summary>
        /// �}�b�v���͂ނ悤�ɕǂ�ݒ肵�܂�
        /// </summary>
        public void CreateWallsSurround()
        {
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    if (x == 0 ||
                        z == 0 ||
                        x == gridField.gridWidth - 1 ||
                        z == gridField.gridDepth - 1)
                    {
                        SetWallsDirection(x, z);
                        CreateWallBlock(x, z);
                    }
                }
            }
        }


        /// <summary>
        /// �^�����O���b�h���W���}�b�v�Ȃ��Ȃ�false��Ԃ��܂�
        /// </summary>
        /// <param name="coord">���W</param>
        /// <returns>�O���b�h�̏�Ȃ�true</returns>
        public bool CheckMap(Coord coord)
        {
            return coord.x >= 0 &&
                    coord.z >= 0 &&
                    coord.x < gridField.gridWidth &&
                    coord.z < gridField.gridDepth;
        }


        /// <summary>
        /// �w�肵�����W����w��͈̔͂̂��ׂẴu���b�N��Ԃ��܂�
        /// </summary>
        /// <param name="coord">���S���W</param>
        /// <param name="areaX">X�̒���</param>
        /// <param name="areaZ">Z�̒���</param>
        public List<Block> AreaBlockList(Coord coord, int areaX, int areaZ)
        {
            // �I��͈͂̃u���b�N�̃��X�g
            List<Block> lAreaBlock = new List<Block>();

            // �����͈͂̃u���b�N�����X�g�ɒǉ�
            for (int x = -areaX; x < areaX; x++)
            {
                for (int z = -areaZ; z < areaZ; z++)
                {
                    if (!CheckMap(new Coord(coord.x + x, coord.z + z))) continue;
                    Block b = blocks[coord.x + x, coord.z + z];
                    lAreaBlock.Add(b);
                }
            }

            return lAreaBlock;
        }

        /// <summary>
        /// �w�肵�����W����w��͈̔͂�"�w�肵�����W�ȊO��"���ׂẴu���b�N��Ԃ��܂�
        /// </summary>
        /// <param name="coord">���S���W</param>
        /// <param name="exceptionCoordList">���O������W�̃��X�g</param>
        /// <param name="areaX"></param>
        /// <param name="areaZ"></param>
        public List<Block> CustomAreaBlockList(Coord coord,List<Coord> exceptionCoordList, int areaX, int areaZ)
        {
            // �I��͈͂̃u���b�N�̃��X�g
            List<Block> lAreaBlock = AreaBlockList(coord, areaX, areaZ);

            lAreaBlock.RemoveAll(b => exceptionCoordList.Contains(b.coord));

            return lAreaBlock;
        }

        /// <summary>
        /// �w�肵�����W����w��͈̔͂̃u���b�N��"�t���[����ɂ���"�u���b�N��Ԃ��܂�
        /// </summary>
        /// <param name="coord">���S���W</param>
        /// <param name="frameSize">�t���[���̃T�C�Y</param>
        /// <param name="areaX">X�̒���</param>
        /// <param name="areaZ">Z�̒���</param>
        /// <returns></returns>
        public List<Block> FrameAreaBlockList(Coord coord,int frameSize, int areaX, int areaZ)
        {
            // �I��͈͂̃u���b�N�̃��X�g
            List<Block> lAreaBlock = AreaBlockList(coord,areaX,areaZ);

            // �G���A����AframeSize�̒l�������̃G���A�̃u���b�N���폜
            for (int x = -areaX + frameSize; x < areaX - frameSize; x++)
            {
                for(int z = -areaZ + frameSize; z < areaZ - frameSize; z++)
                {
                    if (!CheckMap(new Coord(coord.x + x, coord.z + z))) continue;
                    Block removeBlock = blocks[coord.x + x, coord.z + z];
                    lAreaBlock.Remove(removeBlock);
                }
            }

            return lAreaBlock;
        }


        /// <summary>
        /// �w�肵�����W����w��͈̔͂̃u���b�N��"�t���[����ɂ���"�u���b�N��Ԃ��܂�
        /// </summary>
        /// <param name="coord">���S���W</param>
        /// <param name="frameSize">�t���[���̃T�C�Y</param>
        /// <param name="exceptionCoordList">���O������W�̃��X�g</param>
        /// <param name="areaX">X�̒���</param>
        /// <param name="areaZ">Z�̒���</param>
        public List<Block> CustomFrameAreaBlockList(Coord coord, int frameSize, List<Coord> exceptionCoordList, int areaX, int areaZ)
        {
            // �I��͈͂̃u���b�N�̃��X�g
            List<Block> lAreaBlock = FrameAreaBlockList(coord,frameSize, areaX, areaZ);

            lAreaBlock.RemoveAll(b => exceptionCoordList.Contains(b.coord));

            return lAreaBlock;
        }


        /// <summary>
        /// AStar�̓���ݒ肵�܂�
        /// </summary>
        /// <param name="start">�T���̍ŏ��̈ʒu</param>
        /// <param name="goal">�T���̃S�[���n�_</param>
        /// <param name="aStar">AStar</param>
        public void AStar(Vector3 start, Vector3 goal, GridFieldAStar aStar)
        {
            if (aStar == null)
            {
                aStar = new GridFieldAStar();
            }

            aStar.AStarPath(this, gridField.GridCoordinate(start), gridField.GridCoordinate(goal));

            foreach(Coord p in aStar.pathStack)
            {
                Debug.DrawLine(gridField.grid[p.x, p.z], gridField.grid[p.x, p.z] + Vector3.up, Color.red, 10f);

            }
        }
    }
}