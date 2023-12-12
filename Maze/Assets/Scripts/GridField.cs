using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiClass
{
    public class GridField : MonoBehaviour
    {

        /*パブリック変数*/
        public int gridWidth;               // グリッドの広さ
        public int gridDepth;               //
        public int gridHeight;
        public float cellWidth;
        public float cellDepth;
        public float y;
        public Vector3[,] grid = new Vector3[100, 100];      // グリッドのセルの配置Vector3の二次元配列
        public Vector3Int gridCoordinate;// = Vector3Int.zero;                                         // グリッド座標

        /// <summary>
        /// グリッドのセルの数を返します(読み取り専用)
        /// </summary>
        public int totalCell
        {
            get
            {
                return gridCoordinate.x * gridCoordinate.z;
            }
        }

        /// <summary>
        /// グリッドの真ん中の localPosition を返します(読み取り専用)
        /// </summary>
        public Vector3 gridMiddle
        {
            get
            {
                return new Vector3(((float)gridWidth - 1) / 2 * cellWidth, y, ((float)gridDepth - 1) / 2 * cellDepth);
            }
        }

        /*=====コンストラクタ=====*/
        /// <summary>
        /// GridFieldを初期化します
        /// </summary>
        /// <param name="gridBreadth">グリッドの広さ</param>
        /// /// <param name="cellWidth">セルの横幅</param>
        /// /// <param name="cellDepth">セルの奥行</param>
        /// <param name="y">グリッドのy座標</param>
        /// <returns>GridFieldの初期化</returns>
        public GridField(int gridWidth = 10, int gridDepth = 10, float cellWidth = 10, float cellDepth = 10, float y = 0)
        {

            // グリッドの横幅代入
            this.gridWidth = gridWidth;

            // グリッドの奥行代入
            this.gridDepth = gridDepth;

            // セルの横幅代入
            this.cellWidth = cellWidth;

            // セルの奥行を代入
            this.cellDepth = cellDepth;

            // グリッドの高さを代入
            this.y = y;
            if(gridWidth > 100 || gridDepth > 100)
            {
                Debug.LogError("安全のため広すぎるグリッドは生成できません");
                Debug.Break();
            }
            /*===二重ループでgrid配列のそれぞれにVector3の座標値を代入===*/
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    grid[x, z] = new Vector3(x * cellWidth, y, z * cellDepth);    // xとzに10をかけた値を代入
                }
            }
        }

        /*=====引数に与えた Transform がどこのグリッド座標にいるのかを返す=====*/
        /// <summary>
        /// グリッド座標のどこなのかを調べます
        /// </summary>
        /// <param name="grid">調べたいグリッド</param>
        /// <returns></returns>
        /// <param name="pos">調べたいグリッドのどこのセルにいるのか調べたいTransform</param>
        /// <returns>Transformのいるセルのグリッド座標</returns>
        public static Vector3Int GetGridCoordinate(GridField grid, Transform pos)
        {
            /*===二重ループで現在のセルを調べる===*/
            for (grid.gridCoordinate.x = 0; grid.gridCoordinate.x < grid.gridWidth; grid.gridCoordinate.x++)
            {
                for (grid.gridCoordinate.z = 0; grid.gridCoordinate.z < grid.gridDepth; grid.gridCoordinate.z++)
                {
                    if (pos.position.x <= grid.grid[grid.gridCoordinate.x, grid.gridCoordinate.z].x + grid.cellWidth / 2 &&
                        pos.position.x >= grid.grid[grid.gridCoordinate.x, grid.gridCoordinate.z].x - grid.cellWidth / 2 &&
                        pos.position.z <= grid.grid[grid.gridCoordinate.x, grid.gridCoordinate.z].z + grid.cellDepth / 2 &&
                        pos.position.z >= grid.grid[grid.gridCoordinate.x, grid.gridCoordinate.z].z - grid.cellDepth / 2)     // もしあるセルの上にいるなら
                    {
                        return grid.gridCoordinate;                      // セルの Vector3を返す
                    }
                }
            }
            Debug.LogError("与えられたポジションはグリッドフィールドの上にいません。");
            return Vector3Int.zero;
        }

        void Update()
        {
            Debug.DrawLine(Vector3.zero,new Vector3(100,0,0),Color.red);
            //Debug.DrawRay();
        }
    }
}