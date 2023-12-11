using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGurid : MonoBehaviour
{
    [SerializeField] float gridBreadth = 30;            // グリッドの広さ
    public Vector3[,] grid = new Vector3[100,100];      // グリッドのセルの配置Vector3の二次元配列
    [SerializeField] GameObject whiteCellPrefab;        // 白のセルのプレハブ
    [SerializeField] GameObject grayCellPrefab;         // 灰色のセルのプレハブ
    [SerializeField] GameObject gridParent;             // グリッドの中心
    void Start()
    {
        gridParent.transform.position = new Vector3((gridBreadth - 1) / 2 * 10, 0, (gridBreadth - 1) / 2 * 10);　// グリッドフィールドの中心のオブジェクトをフィールドの真ん中に

        /*===二重ループでgrid配列のそれぞれにVector3の座標値を代入===*/
        for (int x = 0; x < gridBreadth; x++)
        {
            for (int z = 0; z < gridBreadth; z++)
            {
                grid[x, z] = new Vector3(x * 10, 0, z * 10);    // xとzに10をかけた値を代入
                InstanceGridField(x, z);                        // グリッドフィールドをインスタンスする関数
            }
        }
    }
    
    /*=====グリッドフィールドをインスタンスする=====*/
    // 引数:x,z座標インデックス
    public void InstanceGridField(int x, int z)
    {
        if ((x + z) % 2 == 0)       // xとzの和が偶数なら
        {
            Instantiate(whiteCellPrefab, grid[x, z], Quaternion.identity.normalized, gridParent.transform); // 白いセルをインスタンス
        }
        else                        // 奇数なら
        {
            Instantiate(grayCellPrefab, grid[x, z], Quaternion.identity.normalized, gridParent.transform);  // 灰色のセルをインスタンス
        }
    }

    /*=====引数に与えた Transform がどこのセルにいるのかを返す=====*/
    // 引数:Transform
    // 戻り値:引数のtransformのいるセルの Vector3
    public Vector3 Grid(Transform pos)
    {
        /*===二重ループで現在のセルを調べる===*/
        for (int x = 0; x < gridBreadth; x++)
        {
            for (int z = 0; z < gridBreadth; z++)
            {
                if (pos.position.x <= grid[x, z].x + 5 &&
                    pos.position.x >= grid[x, z].x - 5 &&
                    pos.position.z <= grid[x, z].z + 5 &&
                    pos.position.z <= grid[x, z].z + 5)     // もしあるセルの上にいるなら
                {
                    Debug.Log(grid[x,z]) ;
                    return grid[x, z];                      // セルの Vector3を返す
                }
            }
        }
        Debug.LogError("与えられたポジションはグリッドフィールドの上にいません。");
        return Vector3.zero;
    }

    void Update()
    {

    }
}
