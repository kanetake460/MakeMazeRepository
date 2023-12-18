using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

namespace TakeshiClass
{
    /*=====グリッドフィールドを作成する関数=====*/
    // Vector3のクラスを参考に作成しました
    // C:\Users\kanet\AppData\Local\Temp\MetadataAsSource\b33e6428b1fe4c03a5b0b222eb1e9f0b\DecompilationMetadataAsSourceFileProvider\4496430b4e32462b86d5e9f4984747a4\Vector3.cs
    public class GridField : MonoBehaviour
    {


//======変数===========================================================================================================================
 
        public int gridWidth;               // グリッドの広さ
        public int gridDepth;               //
        public int gridHeight;
        public float cellWidth;
        public float cellDepth;
        public float y;
        public eGridAnchor gridAnchor;
        public Vector3[,] grid = new Vector3[100, 100];      // グリッドのセルの配置Vector3の二次元配列
        public Vector3Int gridCoordinate;// = Vector3Int.zero;         // グリッド座標

        public enum eGridAnchor
        {
            center,
            bottomLeft
        }



//======読み取り専用変数===============================================================================================================
        
        /*==========グリッドフィールドの角のセルのVector3座標==========*/
        /// <summary>
        /// グリッドのセルの数を返します(読み取り専用)
        /// </summary>
        public int totalCell
        {
            get
            {
                return gridWidth * gridDepth;
            }
        }

        /// <summary>
        ///グリッドの左下のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 bottomLeftCell
        {
            get
            {
                return grid[0, 0];
            }
        }

        /// <summary>
        ///グリッドの右下のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 bottomRightCell
        {
            get
            {
                return grid[gridWidth - 1,0];
            }
        }

        /// <summary>
        ///グリッドの左上のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 topLeftCell
        {
            get
            {
                return grid[0, gridDepth - 1];
            }
        }

        /// <summary>
        ///グリッドの右上のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 topRightCell
        {
            get
            {
                return grid[gridWidth - 1, gridDepth - 1];
            }
        }



        /*==========グリッドフィールドの角のVector3座標==========*/
        /// <summary>
        /// グリッドの左下の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 bottomLeft
        {
            get
            {
                return grid[0, 0] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// グリッドの右下の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 bottomRight
        {
            get
            {
                return　grid[gridWidth - 1, 0] + new Vector3(cellWidth / 2, y, cellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// グリッドの左上の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 topLeft
        {
            get
            {
                return grid[0, gridDepth - 1] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2);
            }
        }

        /// <summary>
        /// グリッドの右上の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 topRight
        {
            get
            {
                return grid[gridWidth - 1, gridDepth - 1] + new Vector3(cellWidth / 2, y, cellDepth / 2);
            }
        }



        /*=========グリッドフィールドの中心Vector3座標===========*/
        /// <summary>
        /// グリッドの真ん中の localPosition を返します(読み取り専用)
        /// </summary>
        public Vector3 middle
        {
            get
            {
                // 横幅奥行がどちらとも偶数
                if (gridWidth % 2 == 0 && gridDepth % 2 == 0)
                {
                    // グリッド座標からセルの半分の数減らした値を返す
                    return grid[gridWidth / 2, gridDepth / 2] - new Vector3(cellWidth / 2, 0, cellDepth / 2);

                }
                // 横幅が偶数
                else if (gridWidth % 2 == 0)
                {
                    // グリッド座標からからセルの半分の数を減らした値を返す(横幅のみ)
                    return grid[gridWidth / 2, gridDepth / 2] - new Vector3(cellWidth / 2, 0, 0);
                }
                // 奥行が偶数
                else if (gridDepth % 2 == 0)
                {
                    // グリッド座標からセルの半分の数を減らした値を返す(奥行のみ)
                    return grid[gridWidth / 2, gridDepth / 2] - new Vector3(0, 0, cellDepth / 2);
                }
                // どちらとも奇数
                else
                {
                    // グリッド座標を返す
                    return grid[gridWidth / 2, gridDepth / 2];
                }
            }
        }



        //======コンストラクタ=================================================================================================================

        /// <summary>
        /// GridFieldを初期化します
        /// </summary>
        /// <param name="gridWidth">グリッドの横幅</param>
        /// <param name="gridDepth">グリッドの奥行</param>
        /// <param name="cellWidth">セルの横幅</param>
        /// <param name="cellDepth">セルの奥行</param>
        /// <param name="y">グリッドのy座標</param>
        /// <param name="gridAnchor">グリッドのアンカー位置</param>
        /// <returns>GridFieldの初期化</returns>
        public GridField(int gridWidth = 10, int gridDepth = 10, float cellWidth = 10, float cellDepth = 10, float y = 0 ,eGridAnchor gridAnchor = eGridAnchor.center)
        {
            // グリッドの横幅代入
            this.gridWidth = gridWidth;

            // グリッドの奥行代入
            this.gridDepth = gridDepth;

            // セルの横幅代入
            this.cellWidth = cellWidth;

            // セルの奥行を代入
            this.cellDepth = cellDepth;

            // グリッドのアンカー位置を代入
            this.gridAnchor = gridAnchor;

            // グリッドの高さを代入
            this.y = y;

            if(gridWidth > 100 || gridDepth > 100)
            {
                Debug.LogError("安全のため広すぎるグリッドは生成できません");
                Debug.Break();
            }

            /*===二重ループでgrid配列のそれぞれにVector3の座標値を代入===*/
            for (int x = 0; x < gridWidth; x += 1)
            {
                for (int z = 0; z < gridDepth; z += 1)
                {
                    if (gridAnchor == eGridAnchor.center)
                    {
                        grid[x, z] = new Vector3(x * cellWidth, y, z * cellDepth) - new Vector3((float)(gridWidth - 1) / 2 * cellWidth, 0, (float)(gridDepth - 1) / 2 * cellDepth);    // xとzに10をかけた値を代入
                    }
                    else if (gridAnchor == eGridAnchor.bottomLeft)
                    {
                        grid[x, z] = new Vector3(x * cellWidth, y, z * cellDepth);    // xとzにセルの大きさをかけた値を代入
                    }
                }
            }
        }



//======関数===========================================================================================================================
       
        ///<summary>
        ///シーンウィンドウにグリッドを表示します
        ///</summary>
        public void DrowGrid()
        {
            // 中の行
            for (int z = 1; z < gridDepth; z++)
            {
                Vector3 gridLineStart = grid[0, z] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
                Vector3 gridLineEnd = grid[gridWidth - 1, z] + new Vector3(cellWidth / 2, y, cellDepth / 2 * -1);

                Debug.DrawLine(gridLineStart, gridLineEnd, Color.red);
            }

            // 中の列
            for (int x = 1; x < gridWidth; x++)
            {
                Vector3 gridRowStart = grid[x, 0] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2 * -1);
                Vector3 gridRowEnd = grid[x, gridDepth - 1] + new Vector3(cellWidth / 2 * -1, y, cellDepth / 2);

                Debug.DrawLine(gridRowStart, gridRowEnd, Color.red);
            }

            // 端のグリッド線表示
            // 最初の列
            Debug.DrawLine(bottomLeft, topLeft, Color.green);

            // 最後の列
            Debug.DrawLine(bottomRight, topRight, Color.green);


            // 最初の行
            Debug.DrawLine(bottomLeft, bottomRight, Color.green);

            // 最後の行
            Debug.DrawLine(topLeft, topRight, Color.green);
        }


        /// <summary>
        /// 引数に与えた Transform がどこのグリッド座標にいるのかを返す
        /// </summary>
        /// <param name="pos">調べたいグリッドのどこのセルにいるのか調べたいTransform</param>
        /// <returns>Transformのいるセルのグリッド座標</returns>
        public Vector3Int GetGridCoordinate(Vector3 pos)
        {
            /*===二重ループで現在のセルを調べる===*/
            for (gridCoordinate.x = 0; gridCoordinate.x < gridWidth; gridCoordinate.x++)
            {
                for (gridCoordinate.z = 0; gridCoordinate.z < gridDepth; gridCoordinate.z++)
                {
                    if (pos.x <= grid[gridCoordinate.x, gridCoordinate.z].x + cellWidth / 2 &&
                        pos.x >= grid[gridCoordinate.x, gridCoordinate.z].x - cellWidth / 2 &&
                        pos.z <= grid[gridCoordinate.x, gridCoordinate.z].z + cellDepth / 2 &&
                        pos.z >= grid[gridCoordinate.x, gridCoordinate.z].z - cellDepth / 2)     // もしあるセルの上にいるなら
                    {
                        return gridCoordinate;                      // セルの Vector3を返す
                    }
                }
            }
            Debug.LogError("与えられたポジションはグリッドフィールドの上にいません。");
            return Vector3Int.zero;
        }


        /// <summary>
        /// 引数に与えた Transform がどこの position なのかを調べます
        /// </summary>
        /// <param name="gridField">調べたいグリッド</param>
        /// <returns></returns>
        /// <param name="pos">調べたいグリッドのどこのセルにいるのか調べたいTransform</param>
        /// <returns>Transformのいるセルのposition</returns>
        public Vector3 GetGridPosition(Vector3 pos)
        {
            return grid[GetGridCoordinate(pos).x, GetGridCoordinate(pos).z];
        }


        /// <summary>
        /// 与えたpositionから任意の距離のほかのpositionのグリッド座標を調べます
        /// </summary>
        /// <param name="gridField">調べたいグリッド</param>
        /// <param name="pos">調べたい距離の始点のVector3座標</param>
        /// <param name="difference">始点から終点までの差分</param>
        public Vector3Int GetOtherGridCoordinate(Vector3 pos, Vector3Int difference)
        {
            int x = GetGridCoordinate(pos).x;
            int z = GetGridCoordinate(pos).z;

            return new Vector3Int(x + difference.x, 0, z + difference.z);
        }
        

        /// <summary>
        /// 与えたpositionから任意の距離のほかのpositionのVector3座標を調べます
        /// </summary>
        /// <param name="pos">調べたい距離の始点のVecgtor3座標</param>
        /// <param name="difference">始点から終点までの差分</param>
        public Vector3 GetOtherGridPosition( Vector3 pos, Vector3Int difference)
        {
            int x = GetGridCoordinate(pos).x;
            int z = GetGridCoordinate(pos).z;

            return grid[x + difference.x, z + difference.z];
        }

        /// <summary>
        /// 与えた posistion がグリッドの上にいるかどうか調べます
        /// </summary>
        /// <param name="pos">調べたいポジション</pragma>
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

        void Update()
        {
            
        }
    }
}