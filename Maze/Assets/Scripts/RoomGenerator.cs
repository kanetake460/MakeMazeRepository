using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    /*オブジェクト参照*/
    [SerializeField] MakeMap map;                                   // マップ
    [SerializeField] GameManager gameManager;                   // ゲームマネージャー
    [SerializeField] GameObject roomPrefab;                     // 部屋のプレハブ
    [SerializeField] Vector3Int roomSizeMin = new Vector3Int(); // 部屋のサイズの最小値
    [SerializeField] Vector3Int roomSizeMax = new Vector3Int(); // 部屋のサイズの最大値
    private int roomCount = 0;                                  // 生成のためのカウント
    [SerializeField] int roomNum = 10;                          // 生成する部屋の数


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

            // チェック
            if(CheckInstanceRoom(randomCoord,roomSizeMin,roomSizeMax))
            {
                // ランダムな位置に部屋の中心を生成
                Instantiate(roomPrefab, map.gridField.grid[randomCoord.x, randomCoord.z], Quaternion.identity);

                // 中心から±2のグリッド座標を部屋エレメントにする
                for (int x = roomSizeMin.x; x <= roomSizeMax.x; x++)
                {
                    for (int z = roomSizeMin.z; z <= roomSizeMax.z; z++)
                    {
                            map.mapElements[randomCoord.x + x, randomCoord.z + z] = Elements.eElementType.Room_Element;
                    }
                }

                ///<summary>
                /// ===デバッグ========================================================================================================================
                Instantiate(roomPrefab, map.gridField.grid[55, 55], Quaternion.identity);
                for (int x = -1; x <= 1; x++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        map.mapElements[55 + x, 55 + z] = Elements.eElementType.Room_Element;
                    }
                }
                ///</summary>

                // ランダムな位置で生成可能だったのでループを抜ける
                return;
            }
        }
    }

    /// <summary>
    /// 部屋を生成できるか調べます
    /// </summary>
    /// <param name="instanceCoord">インスタンスするグリッド座標</param>
    /// <param name="roomSizeMin">部屋の最小値</param>
    /// <param name="roomSizeMax">部屋の最大値</param>
    /// <returns>部屋のすべてのエレメントがNoneかどうか</returns>
    private bool CheckInstanceRoom(Vector3Int instanceCoord,Vector3Int roomSizeMin,Vector3Int roomSizeMax)
    {
        // もし、グリッドの端じゃなければ(部屋の生成で配列外になる可能性があるため)
        if (instanceCoord.x <= 3 ||
            instanceCoord.z <= 3 ||
            instanceCoord.x >= map.gridField.gridDepth - 4 ||
            instanceCoord.z >= map.gridField.gridWidth - 4)
        {
            Debug.Log("範囲外です");
            return false;
        }

            int trueCount = 0;
        for (int x = roomSizeMin.x; x <= roomSizeMax.x; x++)
        {
            for (int z = roomSizeMin.z; z <= roomSizeMax.z; z++)
            {
                if (map.mapElements[instanceCoord.x + x, instanceCoord.z + z] == Elements.eElementType.None_Element)
                {
                    trueCount++;
                }
            }
        }
        // すべての部屋がNoneかどうか
        return trueCount == (roomSizeMax.x - roomSizeMin.x + 1) * (roomSizeMax.z - roomSizeMin.z + 1);
    }

    void Update()
    {
        // フラグのカウントがクリアに必要なフラグの数になるまでループ
        if (roomCount < roomNum)
        {
            InstanceFlagRoom();
            roomCount++;
        }


    }
}
