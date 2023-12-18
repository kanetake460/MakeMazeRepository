using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TakeshiClass;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;

public class Map : MapGridField
{

    [SerializeField] GameObject red;
    [SerializeField] GameObject blue;

    /*�u���b�N*/
    [SerializeField] Section section;

    int instanceCount = 0;

    enum eElementType
    {
        Seed_Element,       // ��G�������g
        Branch_Element,     // �}�G�������g
        None_Element,       // �G�������g�Ȃ�
        OutRange_Element,   // �͈͊O
    }

    // �O���b�h�̃Z���̏����i�[����z��
    eElementType[,] mapElements;



    [SerializeField]Player player;

    protected override void Start()
    {
        base.Start();

        // mapCells �̏�����
        mapElements = new eElementType[gridWidth, gridDepth];
        for(int x = 0; x < gridWidth; x++) 
        {

            for (int z = 0; z < gridDepth; z++)
            {
                // �[�͔͈͊O
                if (x == 0  ||
                    z == 0  ||
                    x == gridWidth - 1 ||
                    z == gridDepth - 1)
                {
                    mapElements[x, z] = eElementType.OutRange_Element;
                }
                else
                {
                    mapElements[x, z] = eElementType.None_Element;
                }
            }
        }
    }

    /// <summary>
    /// �u���b�N���C���X�^���X���܂�
    /// </summary>
    /// <param name="playerPosition">�C���X�^���X����ꏊ</param>
    /// <param name="instanceRot">�C���X�^���X�������</param>
    public void InstanceMapBlock(Vector3 playerPosition,Quaternion instanceRot)
    {

        if (CheckCell(gridField.GetOtherGridCoordinate(playerPosition,
            GetPreviousCoordinate(instanceRot.eulerAngles))) == true)    // �����A�v���C���[�̑O�̃Z�����u����Z���Ȃ�
        {
        Debug.Log(section.mapSection1[instanceCount]);
        Debug.Log(section.mapSection2[instanceCount]);

            // �������Ƃ��A�v���C���[�̖ڂ̑O�̃Z���͎�G�������g
            Vector3Int seedElementCoord = gridField.GetOtherGridCoordinate(playerPosition, GetPreviousCoordinate(instanceRot.eulerAngles));
            mapElements[seedElementCoord.x, seedElementCoord.z] = eElementType.Seed_Element;

            // �������u���b�N�̎�ȊO�̕����͎}�G�������g
            FPS.eFourDirection fourDirection = FPS.GetFourDirection(instanceRot.eulerAngles);
                Vector3Int[] branchElementCoord = new Vector3Int[3];
            for (int i = 0; i < 3; i++)
            {
                branchElementCoord[i] = section.GetBranchElement(section.mapSection1[instanceCount],fourDirection, seedElementCoord, i);
                mapElements[branchElementCoord[i].x,branchElementCoord[i].z] = eElementType.Branch_Element;
            }

            // �Z�N�V����1���C���X�^���X����
            Instantiate(section.sections[(int)section.mapSection1[instanceCount]],
                        gridField.GetOtherGridPosition(playerPosition, GetPreviousCoordinate(instanceRot.eulerAngles)),
                        instanceRot);

            instanceCount++;

            // �J�E���g���������V���b�t��
            if (instanceCount == section.sections.Length)
            {
                Algorithm.Shuffle(section.mapSection1);
                Algorithm.Shuffle(section.mapSection2);
                instanceCount = 0;
            }

// =========�f�o�b�O====================================================================================================

            for (int x = 0; x < gridWidth; x++)
            {

                for (int z = 0; z < gridDepth; z++)
                {
                    if (mapElements[x, z] == eElementType.Seed_Element)
                    {
                        Instantiate(red, gridField.grid[x, z], Quaternion.identity);
                    }
                    else if (mapElements[x,z] == eElementType.Branch_Element)
                    {
                        Instantiate(blue, gridField.grid[x, z], Quaternion.identity);
                    }
                }
            }
        }
    }


    private bool CheckCell(Vector3Int coord)
    {
        if (mapElements[coord.x, coord.z] == eElementType.None_Element)
        {
            return true;
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
        gridField.DrowGrid();

    }
}
