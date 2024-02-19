using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using Unity.VisualScripting;
using System.Linq;

    /// <summary>
    /// マップクラス
    /// </summary>
    public class GridFieldMap
    {
        /// <summary>
        /// ブロッククラス
        /// </summary>
        public class Block
        {
            // ブロックのグリッド座標
            public Vector3Int coord { get; }
            // ブロックの種類
            public bool isSpace { get; set; }

            // 壁があるかどうか( 壁がある : true )
            public bool fowardWall { get; set; } = false;
            public bool rightWall { get; set; } = false;
            public bool backWall { get; set; } = false;
            public bool leftWall { get; set; } = false;


            /// <summary>
            /// ブロックに情報を入れるコンストラクタ
            /// </summary>
            /// <param name="x">xグリッド座標</param>
            /// <param name="z">zグリッド座標</param>
            /// <param name="isSpace">壁か、空間か</param>
            public Block(int x, int z, bool isSpace = true)
            {
                coord = new Vector3Int(x, 0, z);
                this.isSpace = isSpace;
            }

            /// <summary>
            /// 与えたVector3座標の向きが壁なのかどうか調べます
            /// </summary>
            /// <param name="x">奥( -1 or 0 )</param>
            /// <param name="z">横( -1 or 0 )</param>
            /// <returns>壁かどうなのか</returns>
            public bool GetWallDirection(Vector3 dir)
            {
                if (dir == Vector3.right) return rightWall;
                else if (dir == Vector3.left) return leftWall;
                else if (dir == Vector3.forward) return fowardWall;
                else if (dir == Vector3.back) return backWall;
                else return false;
            }

            /// <summary>
            /// 与えた座標の向きが壁なのかどうか調べます
            /// </summary>
            /// <param name="x">奥( -1 or 0 )</param>
            /// <param name="z">横( -1 or 0 )</param>
            /// <returns>壁かどうなのか</returns>
            public bool CheckWall(int x, int z)
            {
                Vector3 checkDir = new Vector3(x, 0, z);
                Debug.Log(checkDir);
                return GetWallDirection(checkDir);
            }
        }

        // ブロックの二次元配列
        public Block[,] blocks { get; } = new Block[100,100];

        public GridField gridField { get;}


    /// <summary>
    /// マップを作成するコンストラクタです
    /// </summary>
    /// <param name="gridWidth">グリッドの横幅</param>
    /// <param name="gridDepth">グリッドの奥行</param>
    /// <param name="t">ブロックのタイプ</param>
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
        /// 指定した座標のブロック、向きを壁に設定します
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        public void SetWallBlock(int x, int z)
        {
            blocks[x, z].isSpace = false;
        }


        /// <summary>
        /// 指定した座標のブロック、向きを壁に設定します
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        /// <param name="dir">壁を入れる向き</param>
        public void SetWall(int x, int z, Vector3 dir)
        {
            if (dir == Vector3.forward) blocks[x, z].fowardWall = true;
            else if (dir == Vector3.right) blocks[x, z].rightWall = true;
            else if (dir == Vector3.back) blocks[x, z].backWall = true;
            else if (dir == Vector3.left) blocks[x, z].leftWall = true;
        }


        /// <summary>
        /// 指定した座標のブロック、向きの壁をなくします
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        /// <param name="dir">壁を入れる向き</param>
        public void BreakWall(int x, int z, Vector3 dir)
        {
            if (dir == Vector3.forward) blocks[x, z].fowardWall = false;
            else if (dir == Vector3.right) blocks[x, z].rightWall = false;
            else if (dir == Vector3.back) blocks[x, z].backWall = false;
            else if (dir == Vector3.left) blocks[x, z].leftWall = false;
        }


        /// <summary>
        /// マップのオブジェクトを生成します
        /// </summary>
        /// <param name="space">spaceのオブジェクト</param>
        /// <param name="wall">wallのオブジェクト</param>
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
        /// グリッド状に壁を生成します
        /// </summary>
        public void SetWallGrid()
        {
            // 壁を設定
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
        /// AStarの道を設定します
        /// </summary>
        /// <param name="start">探索の最初の位置</param>
        /// <param name="goal">探索のゴール地点</param>
        /// <param name="gridField">グリッドフィールド</param>
        /// <param name="pathObj">経路に配置するオブジェクト</param>
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