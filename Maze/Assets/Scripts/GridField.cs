using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiClass
{
    public class GridField : MonoBehaviour
    {
        /*パラメーター*/
        public static float gridBreadth = 30;            // グリッドの広さ

        /*パブリック変数*/
        public Vector3[,] m_grid = new Vector3[100, 100];      // グリッドのセルの配置Vector3の二次元配列
        public Vector3Int m_gridCoordinate = Vector3Int.zero;                                         // グリッド座標

        void Start()
        {

        }

        /*=====二重ループでgrid配列のそれぞれにVector3の座標値を代入=====*/
        // 引数:y座標
        public static GridField AssignValue(float y)
        {
            GridField grid = new GridField();
            /*===二重ループでgrid配列のそれぞれにVector3の座標値を代入===*/
            for (grid.m_gridCoordinate.x = 0; grid.m_gridCoordinate.x < gridBreadth; grid.m_gridCoordinate.x++)
            {
                for (grid.m_gridCoordinate.z = 0; grid.m_gridCoordinate.z < gridBreadth; grid.m_gridCoordinate.z++)
                {
                    grid.m_grid[grid.m_gridCoordinate.x, grid.m_gridCoordinate.z] = new Vector3(grid.m_gridCoordinate.x * 10, y, grid.m_gridCoordinate.z * 10);    // xとzに10をかけた値を代入
                }
            }
            return grid;
        }

        /*=====引数に与えた Transform がどこのグリッド座標にいるのかを返す=====*/
        // 引数:Transform
        // 戻り値:引数のtransformのいるセルのグリッド座標
        public static Vector3Int GetGrid(Transform pos)
        {
            GridField grid = AssignValue(0);
            Vector3Int gridCoordinate = Vector3Int.zero;

            /*===二重ループで現在のセルを調べる===*/
            for (gridCoordinate.x = 0; gridCoordinate.x < gridBreadth; gridCoordinate.x++)
            {
                for (gridCoordinate.z = 0; gridCoordinate.z < gridBreadth; gridCoordinate.z++)
                {
                    if (pos.position.x <= grid.m_grid[gridCoordinate.x, gridCoordinate.z].x + 5 &&
                        pos.position.x >= grid.m_grid[gridCoordinate.x, gridCoordinate.z].x - 5 &&
                        pos.position.z <= grid.m_grid[gridCoordinate.x, gridCoordinate.z].z + 5 &&
                        pos.position.z <= grid.m_grid[gridCoordinate.x, gridCoordinate.z].z + 5)     // もしあるセルの上にいるなら
                    {
                        Debug.Log(gridCoordinate);
                        return gridCoordinate;                      // セルの Vector3を返す
                    }
                }
            }
            Debug.LogError("与えられたポジションはグリッドフィールドの上にいません。");
            return Vector3Int.zero;
        }

        void Update()
        {

        }
    }
}