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
    /// �����_����eMapBlock�̒l��Ԃ��܂�
    /// �Q�l�T�C�g
    /// https://marumaro7.hatenablog.com/entry/enumrandom
    /// </summary>
    /// <returns>�}�b�v�u���b�N</returns>
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
    /// �u���b�N���C���X�^���X���܂�
    /// </summary>
    /// <param name="instancePoint">�C���X�^���X����ꏊ</param>
    /// <param name="instanceRot">�C���X�^���X�������</param>
    public void InstanceMapBlock(Vector3 instancePoint,Quaternion instanceRot)
    {
        Instantiate(blocks[GetRandomBlocks()], instancePoint,instanceRot);
    }

    void Update()
    {
        Debug.Log(GetRandomBlocks());
    }
}
