using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using TakeshiClass;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using static TakeshiClass.FPS;
using static System.Collections.Specialized.BitVector32;
using System;

public class Map : MapGridField
{

    [SerializeField] GameObject red;
    [SerializeField] GameObject blue;

    /*�u���b�N*/
    [SerializeField] Section section;
    [SerializeField] Elements elements1;
    // �O���b�h�̃Z���̏����i�[����z��
    Elements.eElementType[,] mapElements;

    int instanceCount = 0;

    [SerializeField]Player player;

    protected override void Start()
    {
        base.Start();

        // mapCells �̏�����
        mapElements = new Elements.eElementType[gridWidth, gridDepth];
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
                    mapElements[x, z] = Elements.eElementType.OutRange_Element;
                }
                else
                {
                    mapElements[x, z] = Elements.eElementType.None_Element;
                }
            }
        }
    }

    /// <summary>
    /// �u���b�N���C���X�^���X���܂�
    /// </summary>
    /// <param name="playerPosition">�C���X�^���X����ꏊ</param>
    /// <param name="instanceRot">�C���X�^���X�������</param>
    public void InstanceMapBlock(Vector3 playerPosition, Quaternion instanceRot)
    {
        eFourDirection direction = FPS.GetFourDirection(instanceRot.eulerAngles);
        elements1 = new Elements(gridField.GetGridCoordinate(playerPosition), direction, section.mapSection[instanceCount]);


        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < section.sections.Length; j++)
            {
            }
        }
        Debug.Log(section.mapSection[0]);



        // �Z�N�V�������C���X�^���X�\�Ȃ�
        if (CheckInstanceSection(elements1.seedElementCoord, elements1.branchElementCoord))
        {
            // �v���C���[�̑O�̍��W����G�������g��
            mapElements[elements1.seedElementCoord.x, elements1.seedElementCoord.z] = Elements.eElementType.Seed_Element;

            // �Z�N�V�����̂��̂ق��̃G�������g���}�G�������g��
            for (int i = 0; i < 3; i++)
            {
                mapElements[elements1.branchElementCoord[i].x, elements1.branchElementCoord[i].z] = Elements.eElementType.Branch_Element;
            }


            // �Z�N�V����1���C���X�^���X����
            Instantiate(section.sections[(int)section.mapSection[instanceCount]],
                            gridField.grid[elements1.seedElementCoord.x,elements1.seedElementCoord.z],
                            instanceRot);

            // �Z�N�V����1���C���X�^���X����
            Instantiate(section.sections[(int)section.mapSection[instanceCount]],
                            gridField.grid[elements1.seedElementCoord.x, elements1.seedElementCoord.z],
                            instanceRot);

            Vector3 branchPos1 = gridField.GetOtherGridPosition(gridField.grid[elements1.branchElementCoord[0].x, elements1.branchElementCoord[0].z], Elements.GetPreviousCoordinate(GetFourDirection(UnityEngine.Random.)));

            // �Z�N�V����2���C���X�^���X����
            Instantiate(section.sections[(int)section.mapSection[instanceCount]],
                            gridField.GetOtherGridPosition(gridField.grid[elements1.branchElementCoord[0].x, elements1.branchElementCoord[0].z], Elements.GetPreviousCoordinate(UnityEngine.Random.rotation.eulerAngles)),
                            instanceRot);

            instanceCount++;

            // �J�E���g���������V���b�t��
            if (instanceCount == section.sections.Length)
            {
                Algorithm.Shuffle(section.mapSection);
                instanceCount = 0;
            }

            // =========�f�o�b�O====================================================================================================

            for (int x = 0; x < gridWidth; x++)
            {

                for (int z = 0; z < gridDepth; z++)
                {
                    if (mapElements[x, z] == Elements.eElementType.Seed_Element)
                    {
                        Instantiate(red, gridField.grid[x, z], Quaternion.identity);
                    }
                    else if (mapElements[x, z] == Elements.eElementType.Branch_Element)
                    {
                        Instantiate(blue, gridField.grid[x, z], Quaternion.identity);
                    }
                }
            }
        }
    }

    /// <summary>
    /// �^������G�������g�Ǝ}�G�������g�̈ʒu����C���X�^���X�\�����ׂ܂�
    /// </summary>
    /// <param name="seed">��G�������g���W</param>
    /// <param name="branch">�}�G�������g���W</param>
    /// <returns>�C���X�^���X�\���ǂ���</returns>
    private bool CheckInstanceSection(Vector3Int seed,Vector3Int[] branch)
    {
        if (CheckCell(branch[0]) == true &&
            CheckCell(branch[1]) == true &&
            CheckCell(branch[2]) == true &&
            CheckCell(seed) == true )
        {
            return true;
        }
        Debug.Log("�����ł͓����J���܂���");
        return false;
    }

    /// <summary>
    /// �^�������W�̃G�������g���W�� None_Element �Ȃ� true ��Ԃ��܂�
    /// </summary>
    /// <param name="coord">���W</param>
    /// <returns>���W�� None_Element ���ǂ���</returns>
    private bool CheckCell(Vector3Int coord)
    {
        if (mapElements[coord.x, coord.z] == Elements.eElementType.None_Element)
        {
            return true;
        }
        return false;
    }




    void Update()
    {
        gridField.DrowGrid();

    }
}
