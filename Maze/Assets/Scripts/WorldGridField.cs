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
    public GridField gridField;                           // グリッドクラスのワールドグリッドフィールド
    void Start()
    {
        gridField = new GridField(100,100,5,5,-1);
        InstanceGridField(gridField);
    }


/// <summary>
/// グリッドフィールドに合わせてセルオブジェクトを配置する
/// </summary>
/// <param name="gridField">グリッドフィールド</param>
    public void InstanceGridField(GridField gridField)
    {
        // グリッドフィールドの中心のオブジェクトをフィールドの真ん中に親のオブジェクト
        //gridParent.transform.position = new Vector3((gridField.gridBreadth - 1) / 2 * gridField.cellWidth, 0, (gridField.gridBreadth - 1) / 2 * gridField.cellDepth);
        gridParent.transform.position = gridField.gridMiddle;
        for (int x = 0; x < gridField.gridWidth; x++)
        {
            for (int z = 0; z < gridField.gridDepth; z++)
            {
                if ((x + z) % 2 == 0)       // xとzの和が偶数なら
                {
                    Instantiate(whiteCellPrefab, gridField.grid[x, z], Quaternion.identity.normalized, gridParent.transform); // 白いセルをインスタンス
                }
                else                        // 奇数なら
                {
                    Instantiate(grayCellPrefab, gridField.grid[x, z], Quaternion.identity.normalized, gridParent.transform);  // 灰色のセルをインスタンス
                }
            }
        }

    }



    void Update()
    {

    }
}
