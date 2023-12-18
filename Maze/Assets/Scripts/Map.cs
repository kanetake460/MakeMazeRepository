using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TakeshiClass;

public class Map : MonoBehaviour
{
    [EnumIndex(typeof(eMapBlocks)),SerializeField] GameObject[] blocks;
    int instanceCount = 0;

    enum eCellType
    {
        Seed_Cell,
        Branch_Cell,
        Empty_Cell
        
    }

    enum eMapBlocks
    {
        T_Block = 0,
        I_Block = 1,
        O_Block = 2,
        L_Block = 3,
        J_Block = 4,
        S_Block = 5,
        Z_Block = 6
    }

    private eMapBlocks[] mapBlocks1 = new eMapBlocks[7];
    private eMapBlocks[] mapBlocks2 = new eMapBlocks[7];

    [SerializeField]Player player;

    void Start()
    {

        for (int i = 0; i < blocks.Length; i++)
        {
            mapBlocks1[i] = (eMapBlocks)Enum.ToObject(typeof(eMapBlocks), i);
            mapBlocks2[i] = (eMapBlocks)Enum.ToObject(typeof(eMapBlocks), i);
        }
        Algorithm.Shuffle(mapBlocks1);
        Algorithm.Shuffle(mapBlocks2);

    }

    /// <summary>
    /// ブロックをインスタンスします
    /// </summary>
    /// <param name="playerPosition">インスタンスする場所</param>
    /// <param name="instanceRot">インスタンスする向き</param>
    public void InstanceMapBlock(Vector3 playerPosition,Quaternion instanceRot)
    {
        Debug.Log(mapBlocks1[instanceCount]);
        Debug.Log(mapBlocks2[instanceCount]);

        if (player.gridField.CheckOnGrid(GetPreviousCoordinate(instanceRot.eulerAngles)) == true)
        {
            Instantiate(blocks[(int)mapBlocks1[instanceCount]],                             // インスタンスするシャッフルされたブロック配列ブロック
                        player.gridField.GetOtherGridPosition(playerPosition, GetPreviousCoordinate(instanceRot.eulerAngles)),
                        instanceRot);

            instanceCount++;

            if (instanceCount == blocks.Length)
            {
                Algorithm.Shuffle(mapBlocks1);
                Algorithm.Shuffle(mapBlocks2);
                instanceCount = 0;
            }
        }
    }

    public bool CheckOnGrid(Vector3 pos)
    {
        for (int x = 0; x < player.gridField.gridWidth; x++)
        {
            for (int z = 0; z < player.gridField.gridDepth; z++)
            {
                if (player.gridField.GetGridCoordinate(pos) == player.gridField.GetGridCoordinate(player.gridField.grid[x, z]))
                {
                    return true;
                }
            }
        }
        Debug.Log("そこでは道を開けません");
        return false;
    }

    /// <summary>
    /// 向きに対応するひとつ前のグリッド座標を返します
    /// </summary>
    /// <param name="eulerAngles">向き</param>
    /// <returns>向いている方向の一つ前のグリッド座標</returns>
    public Vector3Int GetPreviousCoordinate(Vector3 eulerAngles)
    {
        FPS.eFourDirection fourDirection = FPS.GetFourDirection(eulerAngles);   // 向きを調べて代入
        switch (fourDirection)
        {
            case FPS.eFourDirection.top:
                return new Vector3Int(0, 0, 1);

            case FPS.eFourDirection.bottom:
                return new Vector3Int(0, 0, -1);

            case FPS.eFourDirection.left:
                return new Vector3Int(-1, 0, 0);

            case FPS.eFourDirection.right:
                return new Vector3Int(1, 0, 0);
        }
        return new Vector3Int(0, 0, 0);
    }


    void Update()
    {
        
        //Debug.Log(GetRandomBlocks());
    }
}
