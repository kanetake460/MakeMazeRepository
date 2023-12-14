using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TakeshiClass;

public class Map : MonoBehaviour
{
    [EnumIndex(typeof(eMapBlocks)),SerializeField] GameObject[] blocks;

    Que blockQue;
    enum eMapBlocks
    {
        T_Block = 0,
        I_Block,
        O_Block,
        L_Block,
        J_Block,
        S_Block,
        Z_Block
    }

    private eMapBlocks mapBlock;

    void Start()
    {
        blockQue = new Que();
    }

    /// <summary>
    /// ランダムでeMapBlockの値を返します
    /// 参考サイト
    /// https://marumaro7.hatenablog.com/entry/enumrandom
    /// </summary>
    /// <returns>マップブロック</returns>
    public int GetRandomBlocks()
    {
        int maxCount = Enum.GetNames(typeof(eMapBlocks)).Length;
        //eMapBlocks randBlock;
        if (blockQue.size == 0)
        {
            while (true)
            {
                int number = UnityEngine.Random.Range(0, maxCount);
                for (int i = 0; i < blockQue.size; i++)
                {
                    if (number != blockQue.data[i])
                    {
                        blockQue.Enque(number);
                    }
                }
                if (blockQue.size >= maxCount)
                {
                    break;
                }
            }
        }
        //randBlock = (eMapBlocks)Enum.ToObject(typeof(eMapBlocks), blockQue.Deque());
        return blockQue.Deque();
    }

    /// <summary>
    /// ブロックをインスタンスします
    /// </summary>
    /// <param name="instancePoint">インスタンスする場所</param>
    /// <param name="instanceRot">インスタンスする向き</param>
    public void InstanceMapBlock(Vector3 instancePoint,Quaternion instanceRot)
    {
        Instantiate(blocks[GetRandomBlocks()], instancePoint,instanceRot);
    }

    void Update()
    {
        Debug.Log(GetRandomBlocks());
    }
}
