using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using Unity.VisualScripting;
using System.Linq;

namespace TakeshiLibrary
{
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
            public GameObject mapWallObj { get; set; }


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
                //Debug.Log(checkDir);
                return GetWallDirection(checkDir);
            }
        }

        // ブロックの二次元配列
        public Block[,] blocks { get; } = new Block[100, 100];

        public GridField gridField { get; }


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
        /// 指定した座標を壁ブロックに設定します
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        public void SetWallBlock(int x, int z)
        {
            blocks[x, z].isSpace = false;
        }
        
        /// <summary>
        /// 指定したブロックを壁ブロックに設定します
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        public void SetWallBlock(Block block)
        {
            block.isSpace = false;
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
        /// 指定したブロックの、向きを壁に設定します
        /// </summary>
        /// <param name="block">設定したいブロック</param>
        /// <param name="dir">壁を入れる向き</param>
        public void SetWall(Block block, Vector3 dir)
        {
            if (dir == Vector3.forward) block.fowardWall = true;
            else if (dir == Vector3.right) block.rightWall = true;
            else if (dir == Vector3.back) block.backWall = true;
            else if (dir == Vector3.left) block.leftWall = true;
        }


        /// <summary>
        /// 与えた座標のすべての向きの壁を設定します
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        /// <param name="foward">前壁</param>
        /// <param name="right">右壁</param>
        /// <param name="back">後壁</param>
        /// <param name="left">左壁</param>
        public void SetWalls(int x, int z, bool foward = true, bool right = true, bool back = true, bool left = true,bool isSpace = false)
        {
            blocks[x, z].fowardWall = foward;
            blocks[x, z].rightWall = right;
            blocks[x, z].backWall = back;
            blocks[x, z].leftWall = left;
            blocks[x, z].isSpace = isSpace;
        }

        /// <summary>
        /// あたえたブロックのすべての向きの壁を設定します
        /// デフォルト引数では壁があります
        /// </summary>
        /// <param name="back">ブロック</param>
        /// <param name="foward">前壁</param>
        /// <param name="right">右壁</param>
        /// <param name="back">後壁</param>
        /// <param name="left">左壁</param>
        public void SetWalls(Block block, bool foward = true, bool right = true, bool back = true, bool left = true, bool isSpace = false)
        {
            block.fowardWall = foward;
            block.rightWall = right;
            block.backWall = back;
            block.leftWall = left;
            block.isSpace = isSpace;
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
        /// 指定したブロック、向きの壁をなくします
        /// </summary>
        /// <param name="block">ブロック</param>
        /// <param name="dir">壁を入れる向き</param>
        public void BreakWall(Block block, Vector3 dir)
        {
            if (dir == Vector3.forward) block.fowardWall = false;
            else if (dir == Vector3.right) block.rightWall = false;
            else if (dir == Vector3.back) block.backWall = false;
            else if (dir == Vector3.left) block.leftWall = false;
        }


        /// <summary>
        /// マップのオブジェクトを生成します
        /// </summary>
        /// <param name="space">spaceのオブジェクト</param>
        /// <param name="wall">wallのオブジェクト</param>
        /// <param name="gf">gridField</param>
        public void InstanceMapObjects()
        {
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    plane.transform.SetPositionAndRotation(gridField.grid[blocks[x, z].coord.x, blocks[x, z].coord.z], Quaternion.identity);

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    blocks[x, z].mapWallObj = cube;
                    blocks[x, z].mapWallObj.name = new string(x + "," + z);
                    cube.transform.SetPositionAndRotation(gridField.grid[blocks[x, z].coord.x, blocks[x, z].coord.z], Quaternion.identity);
                    cube.transform.localScale = new Vector3(gridField.cellWidth, gridField.cellMaxLength, gridField.cellDepth);
                }
            }
        }


        /// <summary>
        /// マップの壁オブジェクトのアクティブを管理します
        /// </summary>
        public void ActiveMapWallObject()
        {
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    Debug.Log(blocks[x,z].mapWallObj == null);
                    blocks[x, z].mapWallObj.SetActive(!blocks[x, z].isSpace);
                }
            }
        }

        /// <summary>
        /// マップのすべてのブロックを壁に設定します
        /// </summary>
        public void SetWallAll()
        {
            // 壁を設定
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                        SetWalls(x, z);
                        SetWallBlock(x, z);
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
                        SetWalls(x, z);
                        SetWallBlock(x, z);
                    }
                }
            }
        }

        /// <summary>
        /// マップを囲むように壁を設定します
        /// </summary>
        public void SetWallSurround()
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
                        SetWalls(x, z);
                        SetWallBlock(x, z);
                    }
                }
            }
        }


        /// <summary>
        /// 与えたグリッド座標がマップないならfalseを返します
        /// </summary>
        /// <param name="coord">座標</param>
        /// <returns>グリッドの上ならtrue</returns>
        public bool CheckMap(Vector3Int coord)
        {
            return coord.x >= 0 &&
                    coord.z >= 0 &&
                    coord.x < gridField.gridWidth &&
                    coord.z < gridField.gridDepth;
        }


        /// <summary>
        /// 与えられたグリッド座標から指定の範囲でランダムな座標を取得します
        /// </summary>
        public Vector3Int GetRandomPoint(Vector3Int coord, int areaX, int areaZ)
        {
            // 選択範囲のブロックのリスト
            List<Block> lAreaBlock = new List<Block>();

            // 検索範囲のブロックをリストに追加
            for (int x = -areaX; x < areaX; x++)
            {
                for (int z = -areaZ; z < areaZ; z++)
                {
                    if (!CheckMap(new Vector3Int(coord.x + x, 0, coord.z + z))) continue;
                    Block b = blocks[coord.x + x, coord.z + z];
                    lAreaBlock.Add(b);
                }
            }
            //Debug.Log(lAreaBlock.FindAll(b => b.isSpace == true).Count);

            return lAreaBlock.FindAll(b => b.isSpace == true)[Random.Range(0, lAreaBlock.FindAll(b => b.isSpace).Count)].coord;
        }


        /// <summary>
        /// AStarの道を設定します
        /// </summary>
        /// <param name="start">探索の最初の位置</param>
        /// <param name="goal">探索のゴール地点</param>
        /// <param name="gridField">グリッドフィールド</param>
        /// <param name="pathObj">経路に配置するオブジェクト</param>
        public void SetAStar(Vector3 start, Vector3 goal, GridFieldAStar aStar)
        {
            if (aStar == null)
            {
                aStar = new GridFieldAStar();
            }

            aStar.AStarPath(this, gridField.GetGridCoordinate(start), gridField.GetGridCoordinate(goal));

            foreach(GridFieldAStar.CellInfo p in aStar.pathStack)
            {
                Debug.DrawLine(gridField.grid[p.position.x, p.position.z], gridField.grid[p.position.x, p.position.z] + Vector3.up, Color.red, 10f);

            }
        }
    }
}