using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Map : MonoBehaviour
{
    [EnumIndex(typeof(eMapBlocks)),SerializeField] GameObject[] blocks;

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
        mapBlock = 0;
    }

    /// <summary>
    /// �����_����eMapBlock�̒l��Ԃ��܂�
    /// �Q�l�T�C�g
    /// https://marumaro7.hatenablog.com/entry/enumrandom
    /// </summary>
    /// <returns>�}�b�v�u���b�N</returns>
    public eMapBlocks RandomBlocks()
    {
        int maxCount = Enum.GetNames(typeof(eMapBlocks)).Length;
        int number = UnityEngine.Random.Range(0,maxCount);
        mapBlock = (eMapBlocks)Enum.ToObject(typeof(eMapBlocks),number);
        return 
    }

    /// <summary>
    /// �u���b�N���C���X�^���X���܂�
    /// </summary>
    /// <param name="instancePoint">�C���X�^���X����ꏊ</param>
    /// <param name="instanceRot">�C���X�^���X�������</param>
    public void InstanceMapBlock(Vector3 instancePoint,Quaternion instanceRot)
    {
        Instantiate(blocks[(int)mapBlock], instancePoint,instanceRot);
    }

    void Update()
    {
        
    }
}
