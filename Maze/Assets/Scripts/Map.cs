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
    /// �����_����eMapBlock�̒l��Ԃ��܂�
    /// �Q�l�T�C�g
    /// 
    /// </summary>
    /// <returns>�}�b�v�u���b�N</returns>
    public int GetRandomBlocks()
    {

       
        return 0;
    }

    /// <summary>
    /// �u���b�N���C���X�^���X���܂�
    /// </summary>
    /// <param name="instancePoint">�C���X�^���X����ꏊ</param>
    /// <param name="instanceRot">�C���X�^���X�������</param>
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
