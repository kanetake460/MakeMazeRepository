using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiClass;

public class WorldGridField : MonoBehaviour
{

    /*プレハブ*/
    [SerializeField] GameObject whiteCellPrefab;        // 白のセルのプレハブ
    [SerializeField] GameObject grayCellPrefab;         // 灰色のセルのプレハブ
    [SerializeField] GameObject gridParent;             // グリッドの中心

    /*クラス参照*/
    private GridField worldGridField;                           // グリッドクラスのワールドグリッドフィールド
    void Start()
    {
        // グリッドフィールドの中心のオブジェクトをフィールドの真ん中に親のオブジェクト
        gridParent.transform.position = new Vector3((GridField.gridBreadth - 1) / 2 * 10, 0, (GridField.gridBreadth - 1) / 2 * 10);
        for (int x = 0; x < GridField.gridBreadth; x++)
        {
            for (int z = 0; z < GridField.gridBreadth; z++)
            {
                worldGridField = GridField.AssignValue(0);
                InstanceGridField(x, z);
            }
        }

    }

    /*=====グリッドフィールドをインスタンスする=====*/
    // 引数:x,z座標インデックス
    public void InstanceGridField(int x, int z)
    {
        if ((x + z) % 2 == 0)       // xとzの和が偶数なら
        {
            Instantiate(whiteCellPrefab, worldGridField.m_grid[x, z], Quaternion.identity.normalized, gridParent.transform); // 白いセルをインスタンス
        }
        else                        // 奇数なら
        {
            Instantiate(grayCellPrefab, worldGridField.m_grid[x, z], Quaternion.identity.normalized, gridParent.transform);  // 灰色のセルをインスタンス
        }
    }



    void Update()
    {

    }
}
