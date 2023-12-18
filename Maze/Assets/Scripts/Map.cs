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
    /// �u���b�N���C���X�^���X���܂�
    /// </summary>
    /// <param name="playerPosition">�C���X�^���X����ꏊ</param>
    /// <param name="instanceRot">�C���X�^���X�������</param>
    public void InstanceMapBlock(Vector3 playerPosition,Quaternion instanceRot)
    {
        Debug.Log(mapBlocks1[instanceCount]);
        Debug.Log(mapBlocks2[instanceCount]);

        if (player.gridField.CheckOnGrid(GetPreviousCoordinate(instanceRot.eulerAngles)) == true)
        {
            Instantiate(blocks[(int)mapBlocks1[instanceCount]],                             // �C���X�^���X����V���b�t�����ꂽ�u���b�N�z��u���b�N
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
        Debug.Log("�����ł͓����J���܂���");
        return false;
    }

    /// <summary>
    /// �����ɑΉ�����ЂƂO�̃O���b�h���W��Ԃ��܂�
    /// </summary>
    /// <param name="eulerAngles">����</param>
    /// <returns>�����Ă�������̈�O�̃O���b�h���W</returns>
    public Vector3Int GetPreviousCoordinate(Vector3 eulerAngles)
    {
        FPS.eFourDirection fourDirection = FPS.GetFourDirection(eulerAngles);   // �����𒲂ׂđ��
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
