using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiClass;
using Unity.VisualScripting;
using System.Linq;

public class TestMap : MonoBehaviour
{
    /*プレハブ*/
    [SerializeField] GameObject space;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject pathObj;

    /*オブジェクト*/
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;

    /*グリッド*/
    GridField gridField;

    /* A*クラス */
    AStar aStar;

    public int gridWidth = 20;
    public int gridDepth = 10;
    public float cellWidth = 10;
    public float cellDepth = 10;
    public float y = 0;

    /// <summary>
    /// マップクラス
    /// </summary>
    public class Map
    {
        /// <summary>
        /// ブロックの種類
        /// </summary>
        public enum BlockType
        {
            eSpace,     // 空間
            eWall,      // 壁
        }

        /// <summary>
        /// ブロッククラス
        /// </summary>
        public class Block
        {
            // ブロックの座標
            public Vector3Int coord { get; }
            // ブロックの種類
            public BlockType type { get; set; }

            /// <summary>
            /// ブロックに情報を入れるコンストラクタ
            /// </summary>
            /// <param name="x">xグリッド座標</param>
            /// <param name="z">zグリッド座標</param>
            /// <param name="t">ブロックの種類</param>
            public Block(int x, int z, BlockType t)
            {
                coord = new Vector3Int(x, 0, z);
                type = t;
            }
        }

        // マップの横幅
        public int mapWidth { get; }
        // マップの奥行
        public int mapDepth { get; }

        // ブロックの二次元配列
        public Block[,] blocks { get; } = new Block[100,100];

        /// <summary>
        /// マップを作成するコンストラクタです
        /// </summary>
        /// <param name="gridWidth">グリッドの横幅</param>
        /// <param name="gridDepth">グリッドの奥行</param>
        /// <param name="t">ブロックのタイプ</param>
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
        /// 指定した座標のブロックを壁に設定します
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        public void SetWall(int x, int z)
        {
            blocks[x, z].type = BlockType.eWall;
        }

        /// <summary>
        /// マップのオブジェクトを生成します
        /// </summary>
        /// <param name="space">spaceのオブジェクト</param>
        /// <param name="wall">wallのオブジェクト</param>
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

    // マップ
    Map map;

    private void Start()
    {
        // グリッド作成
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y,GridField.eGridAnchor.bottomLeft);

        // マップ作成
        map = new Map(gridWidth,gridDepth,Map.BlockType.eSpace);
        
        // 壁を設定
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
        // マップオブジェクト作成
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
        // グリッド表示
        gridField.DrowGrid();
    }
}

