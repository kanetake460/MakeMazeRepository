using System.Collections.Generic;
using UnityEngine;

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
            public Coord coord { get; }
            // ブロックの種類
            public bool isSpace { get; set; }

            // 壁があるかどうか( 壁がある : true )
            public bool fowardWall { get; set; } = false;
            public bool rightWall { get; set; } = false;
            public bool backWall { get; set; } = false;
            public bool leftWall { get; set; } = false;
            public GameObject mapWallObj { get; set; }      // 壁オブジェクト
            public GameObject mapPlaneObj { get; set; }     // 床オブジェクト


            /// <summary>
            /// ブロックに情報を入れるコンストラクタ
            /// </summary>
            /// <param name="x">xグリッド座標</param>
            /// <param name="z">zグリッド座標</param>
            /// <param name="isSpace">壁か、空間か</param>
            public Block(int x, int z, bool isSpace = true)
            {
                coord = new Coord(x, z);
                this.isSpace = isSpace;
            }

            /// <summary>
            /// 与えたVector3座標の向きが壁なのかどうか調べます
            /// </summary>
            /// <param>Vector3の向き</param>
            /// <returns>壁かどうなのか</returns>
            public bool CheckWallDirection(Vector3 dir)
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
                return CheckWallDirection(checkDir);
            }
        }


        // ブロックの二次元配列
        public Block[,] blocks { get; } = new Block[100, 100];
        // グリッドフィールド
        public GridField gridField { get; }

        /// <summary>
        /// マップを作成するコンストラクタです
        /// </summary>
        /// <param name="gridField">グリッドフィールド</param>
        public GridFieldMap(GridField gridField)
        {
            this.gridField = gridField;
            // グリッドフィールドのセルの座標に合わせてブロック作成
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
        public void CreateWallBlock(int x, int z)
        {
            blocks[x, z].isSpace = false;
        }
        
        /// <summary>
        /// 指定したブロックを壁ブロックに設定します
        /// </summary>
        /// <param name="block">壁にしたいブロック</param>
        public void CreateWallBlock(Block block)
        {
            block.isSpace = false;
        }


        /// <summary>
        /// 指定した座標のブロック、向きを壁に設定します
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        /// <param name="dir">壁を入れる向き</param>
        public void CreateWallDirection(int x, int z, Vector3 dir)
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
        public void CreateWallDirection(Block block, Vector3 dir)
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
        public void SetWallsDirection(int x, int z, bool foward = true, bool right = true, bool back = true, bool left = true,bool isSpace = false)
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
        public void SetWallsDirection(Block block, bool foward = true, bool right = true, bool back = true, bool left = true, bool isSpace = false)
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
        /// <param>壁の高さ</param>
        public void InstanceMapObjects(float scaleY = 10)
        {
            for (int x = 0; x < gridField.gridWidth; x++)
            {
                for (int z = 0; z < gridField.gridDepth; z++)
                {
                    // 床作成
                    GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    plane.name = new string("Plane" + x + "," + z);
                    blocks[x, z].mapPlaneObj = plane;
                    plane.transform.SetPositionAndRotation(gridField.grid[blocks[x, z].coord.x, blocks[x, z].coord.z], Quaternion.identity);

                    // 壁作成
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    blocks[x, z].mapWallObj = cube;
                    blocks[x, z].mapWallObj.name = new string("Wall" + x + "," + z);
                    cube.transform.SetPositionAndRotation(gridField.grid[blocks[x, z].coord.x, blocks[x, z].coord.z], Quaternion.identity);
                    cube.transform.localScale = new Vector3(gridField.cellWidth, scaleY, gridField.cellDepth);
                }
            }
        }


        /// <summary>
        /// 壁オブジェクトのレイヤーマスクを設定します
        /// </summary>
        /// <param name="layerName">レイヤー</param>
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
        /// プレーンオブジェクトの色を変えます
        /// </summary>
        /// <param name="coord">プレーンの座標</param>
        /// <param name="color">色</param>
        public void ChangePlaneColor(Coord coord,Color color)
        {
            blocks[coord.x, coord.z].mapPlaneObj.GetComponent<Renderer>().material.color = color;
        }


        /// <summary>
        /// 壁オブジェクトの色を変えます
        /// </summary>
        /// <param name="coord">壁の座標</param>
        /// <param name="color">色</param>
        public void ChangeWallColor(Coord coord,Color color)
        {
            blocks[coord.x, coord.z].mapWallObj.GetComponent<Renderer>().material.color = color;
        }


        /// <summary>
        /// 壁オブジェクトのテクスチャを変更します
        /// </summary>
        /// <param name="coord">壁の座標</param>
        /// <param name="texrure">テクスチャ</param>
        public void ChangeWallTexture(Coord coord,Texture texrure)
        {
            blocks[coord.x, coord.z].mapWallObj.GetComponent<Renderer>().material.mainTexture = texrure;
        }


        /// <summary>
        /// すべての壁オブジェクトのテクスチャを変更します
        /// </summary>
        /// <param name="texrure">テクスチャ</param>
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
        /// マップの壁オブジェクトのアクティブを管理します
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
        /// マップのすべてのブロックを壁に設定します
        /// </summary>
        public void CreateWallsAll()
        {
            // 壁を設定
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
        /// グリッド状に壁を生成します
        /// </summary>
        public void CreateWallsGrid()
        {
            // 壁を設定
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
        /// マップを囲むように壁を設定します
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
        /// 与えたグリッド座標がマップないならfalseを返します
        /// </summary>
        /// <param name="coord">座標</param>
        /// <returns>グリッドの上ならtrue</returns>
        public bool CheckMap(Coord coord)
        {
            return coord.x >= 0 &&
                    coord.z >= 0 &&
                    coord.x < gridField.gridWidth &&
                    coord.z < gridField.gridDepth;
        }


        /// <summary>
        /// 指定した座標から指定の範囲のすべてのブロックを返します
        /// </summary>
        /// <param name="coord">中心座標</param>
        /// <param name="areaX">Xの長さ</param>
        /// <param name="areaZ">Zの長さ</param>
        public List<Block> AreaBlockList(Coord coord, int areaX, int areaZ)
        {
            // 選択範囲のブロックのリスト
            List<Block> lAreaBlock = new List<Block>();

            // 検索範囲のブロックをリストに追加
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
        /// 指定した座標から指定の範囲の"指定した座標以外の"すべてのブロックを返します
        /// </summary>
        /// <param name="coord">中心座標</param>
        /// <param name="exceptionCoordList">除外する座標のリスト</param>
        /// <param name="areaX"></param>
        /// <param name="areaZ"></param>
        public List<Block> CustomAreaBlockList(Coord coord,List<Coord> exceptionCoordList, int areaX, int areaZ)
        {
            // 選択範囲のブロックのリスト
            List<Block> lAreaBlock = AreaBlockList(coord, areaX, areaZ);

            lAreaBlock.RemoveAll(b => exceptionCoordList.Contains(b.coord));

            return lAreaBlock;
        }

        /// <summary>
        /// 指定した座標から指定の範囲のブロックの"フレーム状にある"ブロックを返します
        /// </summary>
        /// <param name="coord">中心座標</param>
        /// <param name="frameSize">フレームのサイズ</param>
        /// <param name="areaX">Xの長さ</param>
        /// <param name="areaZ">Zの長さ</param>
        /// <returns></returns>
        public List<Block> FrameAreaBlockList(Coord coord,int frameSize, int areaX, int areaZ)
        {
            // 選択範囲のブロックのリスト
            List<Block> lAreaBlock = AreaBlockList(coord,areaX,areaZ);

            // エリアから、frameSizeの値分内側のエリアのブロックを削除
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
        /// 指定した座標から指定の範囲のブロックの"フレーム状にある"ブロックを返します
        /// </summary>
        /// <param name="coord">中心座標</param>
        /// <param name="frameSize">フレームのサイズ</param>
        /// <param name="exceptionCoordList">除外する座標のリスト</param>
        /// <param name="areaX">Xの長さ</param>
        /// <param name="areaZ">Zの長さ</param>
        public List<Block> CustomFrameAreaBlockList(Coord coord, int frameSize, List<Coord> exceptionCoordList, int areaX, int areaZ)
        {
            // 選択範囲のブロックのリスト
            List<Block> lAreaBlock = FrameAreaBlockList(coord,frameSize, areaX, areaZ);

            lAreaBlock.RemoveAll(b => exceptionCoordList.Contains(b.coord));

            return lAreaBlock;
        }


        /// <summary>
        /// AStarの道を設定します
        /// </summary>
        /// <param name="start">探索の最初の位置</param>
        /// <param name="goal">探索のゴール地点</param>
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