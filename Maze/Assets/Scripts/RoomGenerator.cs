using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator : GameManager
{

    [SerializeField] GameObject roomPrefab;
    private int flagCount = 0;     // 生成のためのカウント

    /// <summary>
    /// フラグを生成する部屋を生成します
    /// </summary>
    public void InstanceFlagRoom()
    {
        // もし、生成できなかった場合もう一度ランダムな値を代入するためループさせます
        while (true)
        {
            // ランダムなグリッド座標
            Vector3Int randomCoord = map.gridField.randomGridCoord;

            // もし、グリッドの端じゃなければ(部屋の生成で配列外になる可能性があるため)
            if (randomCoord.x >= 3 &&
                randomCoord.z >= 3 &&
                randomCoord.x <= map.gridField.gridDepth - 4 &&
                randomCoord.z <= map.gridField.gridWidth - 4)
            {
                // ランダムな位置に部屋の中心を生成
                //Instantiate(flagPrefab, map.gridField.grid[randomCoord.x, randomCoord.z], Quaternion.identity);
                
                // 中心から±2のグリッド座標を部屋エレメントにする
                //for (int x = -1; x <= 1; x++)
                //{
                //    for (int z = -1; z <= 1; z++)
                //    {
                //        map.mapElements[randomCoord.x + x, randomCoord.z + z] = Elements.eElementType.Room_Element;
                //    }
                //}

                /*デバッグ*/
                Instantiate(roomPrefab, map.gridField.grid[55, 55], Quaternion.identity);
                for (int x = -1; x <= 1; x++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        map.mapElements[55 + x, 55 + z] = Elements.eElementType.Room_Element;
                    }
                }

                // ランダムな位置で生成可能だったのでループを抜ける
                return;
            }
        }
    }
    void Start()
    {


    }



    void Update()
    {
        // フラグのカウントがクリアに必要なフラグの数になるまでループ
        if (flagCount < clearFlagNum)
        {
            InstanceFlagRoom();
            flagCount++;
        }
    }
}
