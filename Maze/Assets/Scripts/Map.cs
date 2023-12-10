using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    /*=====マップのブロックを引数のTransformで配置する関数=====*/
    public void InstanceMapBlock(Vector3 instancePoint,Quaternion instanceRot)
    {
        Instantiate(blocks[(int)mapBlock], instancePoint,instanceRot);
    }

    void Update()
    {
        
    }
}
