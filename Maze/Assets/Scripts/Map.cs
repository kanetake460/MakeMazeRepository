using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TakeshiClass;

public class Map : MonoBehaviour
{
    [EnumIndex(typeof(eMapBlocks)),SerializeField] GameObject[] blocks;
    int a = 0;

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

    private eMapBlocks[] mapBlock = new eMapBlocks[7];

    void Start()
    {

        for (int i = 0; i < blocks.Length; i++)
        {
            mapBlock[i] = (eMapBlocks)Enum.ToObject(typeof(eMapBlocks), i); 
        }
        Algorithm.Shuffle(mapBlock);

    }

    /// <summary>
    /// ランダムでeMapBlockの値を返します
    /// 参考サイト
    /// 
    /// </summary>
    /// <returns>マップブロック</returns>
    public int GetRandomBlocks()
    {

       
        return 0;
    }

    /// <summary>
    /// ブロックをインスタンスします
    /// </summary>
    /// <param name="instancePoint">インスタンスする場所</param>
    /// <param name="instanceRot">インスタンスする向き</param>
    public void InstanceMapBlock(Vector3 instancePoint,Quaternion instanceRot)
    {
        Debug.Log((int)mapBlock[a]);
        Instantiate(blocks[(int)mapBlock[a]], instancePoint,instanceRot);
        a++;
        if(a == blocks.Length)
        {
            Algorithm.Shuffle(mapBlock);
            a = 0;
        }
    }

    void Update()
    {
        
        //Debug.Log(GetRandomBlocks());
    }
}
