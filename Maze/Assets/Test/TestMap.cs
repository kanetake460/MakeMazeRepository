using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiClass;
using Unity.VisualScripting;
using System.Linq;

public class TestMap : MonoBehaviour
{
    /*�v���n�u*/
    [SerializeField] GameObject space;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject pathObj;

    /*�I�u�W�F�N�g*/
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;

    /*�O���b�h*/
    GridField gridField;

    /* A*�N���X */
    AStar aStar;

    public int gridWidth = 20;
    public int gridDepth = 10;
    public float cellWidth = 10;
    public float cellDepth = 10;
    public float y = 0;

    /// <summary>
    /// �}�b�v�N���X
    /// </summary>
    public class Map
    {
        /// <summary>
        /// �u���b�N�̎��
        /// </summary>
        public enum BlockType
        {
            eSpace,     // ���
            eWall,      // ��
        }

        /// <summary>
        /// �u���b�N�N���X
        /// </summary>
        public class Block
        {
            // �u���b�N�̍��W
            public Vector3Int coord { get; }
            // �u���b�N�̎��
            public BlockType type { get; set; }

            /// <summary>
            /// �u���b�N�ɏ�������R���X�g���N�^
            /// </summary>
            /// <param name="x">x�O���b�h���W</param>
            /// <param name="z">z�O���b�h���W</param>
            /// <param name="t">�u���b�N�̎��</param>
            public Block(int x, int z, BlockType t)
            {
                coord = new Vector3Int(x, 0, z);
                type = t;
            }
        }

        // �}�b�v�̉���
        public int mapWidth { get; }
        // �}�b�v�̉��s
        public int mapDepth { get; }

        // �u���b�N�̓񎟌��z��
        public Block[,] blocks { get; } = new Block[100,100];

        /// <summary>
        /// �}�b�v���쐬����R���X�g���N�^�ł�
        /// </summary>
        /// <param name="gridWidth">�O���b�h�̉���</param>
        /// <param name="gridDepth">�O���b�h�̉��s</param>
        /// <param name="t">�u���b�N�̃^�C�v</param>
        public Map(int gridWidth, int gridDepth, BlockType t)
        {
            mapWidth = gridWidth;
            mapDepth = gridDepth;
            for (int x = 0; x < mapWidth; x++)
            {
                for (int z = 0; z < mapDepth; z++)
                {
                    blocks[x,z] = new Block(x, z, t);
                }
            }
        }

        /// <summary>
        /// �w�肵�����W�̃u���b�N��ǂɐݒ肵�܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        public void SetWall(int x, int z)
        {
            blocks[x, z].type = BlockType.eWall;
        }

        /// <summary>
        /// �}�b�v�̃I�u�W�F�N�g�𐶐����܂�
        /// </summary>
        /// <param name="space">space�̃I�u�W�F�N�g</param>
        /// <param name="wall">wall�̃I�u�W�F�N�g</param>
        /// <param name="gf">gridField</param>
        public void InstanceMapObjects(GameObject space,GameObject wall, GridField gf)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int z = 0; z < mapDepth; z++)
                {
                    if (blocks[x, z].type == BlockType.eSpace) Instantiate(space, gf.grid[blocks[x, z].coord.x, blocks[x, z].coord.z], Quaternion.identity);
                    else if (blocks[x,z].type == BlockType.eWall) Instantiate(wall, gf.grid[blocks[x,z].coord.x, blocks[x, z].coord.z] + new Vector3(0,5,0), Quaternion.identity);
                }
            }
        }
    }

    // �}�b�v
    Map map;

    private void Start()
    {
        // �O���b�h�쐬
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y,GridField.eGridAnchor.bottomLeft);

        // �}�b�v�쐬
        map = new Map(gridWidth,gridDepth,Map.BlockType.eSpace);
        
        // �ǂ�ݒ�
        for(int x = 0;x < gridWidth;x++)
        {
            for(int z = 0;z < gridDepth;z++)
            {
                if (x % 2 == 1 && z % 2 == 1) map.SetWall(x, z);
            }
        }
        //map.SetWall(0,1);
        //    map.SetWall(2, 1);
        //map.SetWall(4, 1);
        //map.SetWall(5, 1);
        // �}�b�v�I�u�W�F�N�g�쐬
        map.InstanceMapObjects(space, wall, gridField);

        aStar = new AStar(gridField,map,gridField.GetGridCoordinate(enemy.transform.position),gridField.GetGridCoordinate(player.transform.position));

        aStar.AStarPath();
        while(aStar.pathStack.Count > 0)
        {
            //Debug.Log(aStar.pathStack.Pop().position);
            Vector3Int popedInfo = aStar.pathStack.Pop().position;
            Instantiate(pathObj, gridField.grid[popedInfo.x,popedInfo.z], Quaternion.identity);
            Debug.Log(gridField.grid[popedInfo.x,popedInfo.z]);

        }
    }

    private void Update()
    {
        // �O���b�h�\��
        gridField.DrowGrid();
    }
}

