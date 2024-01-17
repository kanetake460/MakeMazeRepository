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
    /*�Q�[���I�u�W�F�N�g�i�f�o�b�O�p�j*/
    [SerializeField] GameObject red;
    [SerializeField] GameObject blue;

    /*�u���b�N*/
    [SerializeField] Section section;           // �Z�N�V����
    [SerializeField] Elements startElements;    // �X�^�[�g�G�������g
    [SerializeField] Elements elements1;        // �G�������g1
    [SerializeField] Elements elements2;        // �G�������g2
    [SerializeField] Elements elements3;        // �G�������g3

    // �O���b�h�̃Z���̏����i�[����z��
    public Elements.eElementType[,] mapElements;

    // �C���X�^���X�̃J�E���g(�J�E���g�̒l�ɂ���Đ�������Z�N�V�������ς��܂�)
    int instanceCount1 = 0;
    int instanceCount2 = 3;
    int instanceCount3 = 6;

    protected override void Start()
    {
        base.Start();

        // mapElements �̏�����
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

        // �X�^�[�g�G�������g����
        startElements = new Elements(gridField.GetGridCoordinate(gridField.grid[50,48]),eFourDirection.top,Section.eMapSections.O_Section);
        mapElements = startElements.SetElementType(mapElements, startElements.seedElementCoord, startElements.branchElementCoord); 
        // ===�f�o�b�O==============================================================================================================================
        //for (int x = 0; x < gridWidth; x++)
        //{

        //    for (int z = 0; z < gridDepth; z++)
        //    {
        //        if (mapElements[x, z] == Elements.eElementType.Room_Element)
        //        {
        //            Instantiate(red, gridField.grid[x, z], Quaternion.identity);
        //        }
        //        else if (mapElements[x, z] == Elements.eElementType.Branch_Element)
        //        {
        //            Instantiate(blue, gridField.grid[x, z], Quaternion.identity);
        //        }
                
        //    }
        //}
    }

    /// <summary>
    /// �u���b�N���C���X�^���X���܂�
    /// </summary>
    /// <param name="playerPosition">�C���X�^���X����ꏊ</param>
    /// <param name="instanceRot">�C���X�^���X�������</param>
    public bool InstanceMapBlock(Vector3 playerPosition, Quaternion instanceRot)
    {
        // �Z�N�V����1�̐�������
        eFourDirection direction = FPS.GetFourDirection(instanceRot.eulerAngles);
        // �G�������g��ݒ�
        elements1 = new Elements(gridField.GetGridCoordinate(playerPosition), direction, section.mapSection[instanceCount1]);


        // �Z�N�V�������C���X�^���X�\�Ȃ�
        if (CheckInstanceSection(elements1.seedElementCoord, elements1.branchElementCoord))
        {
            mapElements = elements1.SetElementType(mapElements, elements1.seedElementCoord, elements1.branchElementCoord);

            int breakCount = 0;
            while (breakCount <= 50)
            {
                breakCount++;

                // �����_���Ȏ}
                int randBranch1 = UnityEngine.Random.Range(0, 3);
                // �����_���Ȏ}�̍��W
                Vector3 branchPos1 = gridField.GetGridPosition(gridField.grid[elements1.branchElementCoord[randBranch1].x, elements1.branchElementCoord[randBranch1].z]);
                // �����_���Ȍ���
                eFourDirection randDir1 = FPS.RandomFourDirection();

                elements2 = new Elements(gridField.GetGridCoordinate(branchPos1), randDir1, section.mapSection[instanceCount2]);
                if (CheckInstanceSection(elements2.seedElementCoord, elements2.branchElementCoord))
                {
                    mapElements = elements2.SetElementType(mapElements, elements2.seedElementCoord, elements2.branchElementCoord);

                    int breakCount2 = 0;
                    while (breakCount2 <= 50)
                    {
                        breakCount2++;

                        // �����_���Ȏ}
                        int randBranch2 = UnityEngine.Random.Range(0, 3);
                        // �����_���Ȏ}�̍��W
                        Vector3 branchPos2 = gridField.GetGridPosition(gridField.grid[elements1.branchElementCoord[randBranch2].x, elements1.branchElementCoord[randBranch2].z]);
                        // �����_���Ȍ���
                        eFourDirection randDir2 = FPS.RandomFourDirection();

                        elements3 = new Elements(gridField.GetGridCoordinate(branchPos2), randDir2, section.mapSection[instanceCount3]);
                        if (CheckInstanceSection(elements3.seedElementCoord, elements3.branchElementCoord))
                        {
                            mapElements = elements3.SetElementType(mapElements, elements3.seedElementCoord, elements3.branchElementCoord);
                            // �Z�N�V����1���C���X�^���X����
                            Instantiate(section.sections[(int)section.mapSection[instanceCount1]],
                                            gridField.grid[elements1.seedElementCoord.x, elements1.seedElementCoord.z],
                                            instanceRot);
                            // �Z�N�V����1�̃v���C���[�̖ڂ̑O�̕ǂ��Ȃ���
                            BreakWall(gridField.GetGridPosition(playerPosition), direction);


                            // �Z�N�V����2���C���X�^���X����
                            Instantiate(section.sections[(int)section.mapSection[instanceCount2]],
                                            gridField.grid[elements2.seedElementCoord.x, elements2.seedElementCoord.z],
                                            FPS.GetFourDirectionEulerAngles(new Vector3(0, (int)randDir1, 0)));
                            // �Z�N�V����2�̕ǂ��Ȃ���
                            BreakWall(branchPos1, randDir1);


                            // �Z�N�V����3���C���X�^���X����
                            Instantiate(section.sections[(int)section.mapSection[instanceCount3]],
                                            gridField.grid[elements3.seedElementCoord.x, elements3.seedElementCoord.z],
                                            FPS.GetFourDirectionEulerAngles(new Vector3(0, (int)randDir2, 0)));
                            // �Z�N�V����3�̕ǂ��Ȃ���
                            BreakWall(branchPos2, randDir2);

                            instanceCount1++;
                            instanceCount2++;
                            instanceCount3++;

                            // �J�E���g���������V���b�t��
                            if (instanceCount1 == section.sections.Length)
                            {
                                Algorithm.Shuffle(section.mapSection);
                                instanceCount1 = 0;
                            }
                            if (instanceCount2 == section.sections.Length)
                            {
                                instanceCount2 = 0;
                            }
                            if (instanceCount3 == section.sections.Length)
                            {
                                instanceCount3 = 0;
                            }

                            BreakRoomWall();

                            // =========�f�o�b�O====================================================================================================

                            //for (int x = 0; x < gridWidth; x++)
                            //{

                            //    for (int z = 0; z < gridDepth; z++)
                            //    {
                            //        if (mapElements[x, z] == Elements.eElementType.Room_Element ||
                            //            mapElements[x, z] == Elements.eElementType.Seed_Element)
                            //        {
                            //            Instantiate(red, gridField.grid[x, z], Quaternion.identity);
                            //        }
                            //        else if (mapElements[x, z] == Elements.eElementType.Branch_Element)
                            //        {
                            //            Instantiate(blue, gridField.grid[x, z], Quaternion.identity);
                            //        }
                            //    }
                            //}
                            return true;
                        }
                    }
                    // �����ł��Ȃ������̂ŃG�������g��None�ɖ߂�
                    mapElements = elements2.RestoreElementType(mapElements, elements2.seedElementCoord, elements2.branchElementCoord);
                }
            }
            // �����ł��Ȃ������̂ŃG�������g��None�ɖ߂�
            mapElements = elements1.RestoreElementType(mapElements, elements1.seedElementCoord, elements1.branchElementCoord);
        }
        Debug.Log("�ǂ��ɂ��u���܂���ł���");
        return false;
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

    /// <summary>
    /// �����ׂ̗̕ǂ��󂵂܂��B
    /// </summary>
    private void BreakRoomWall()
    {
        for (int x = 0; x < gridWidth; x++)
        {

            for (int z = 0; z < gridDepth; z++)
            {
                if (mapElements[x,z] == Elements.eElementType.Branch_Element ||
                    mapElements[x,z] == Elements.eElementType.Seed_Element) 
                {
                    if (mapElements[x + 1,z] == Elements.eElementType.Room_Element)
                    {
                        BreakWall(gridField.grid[x, z], eFourDirection.right);
                    }
                    if (mapElements[x - 1, z] == Elements.eElementType.Room_Element)
                    {
                        BreakWall(gridField.grid[x, z], eFourDirection.left);
                    }
                    if (mapElements[x, z + 1] == Elements.eElementType.Room_Element)
                    {
                        BreakWall(gridField.grid[x, z], eFourDirection.top);
                    }
                    if (mapElements[x, z - 1] == Elements.eElementType.Room_Element)
                    {
                        BreakWall(gridField.grid[x, z], eFourDirection.bottom);
                    }
                }
            }
        }
    }


    /// <summary>
    /// ����_�Ƀ��C�L���X�g���o���ē��������ǂ��󂵂܂�
    /// </summary>
    /// <param name="branchPos">����_</param>
    /// <param name="dir">�������</param>
    private void BreakWall(Vector3 branchPos, eFourDirection dir)
    {
        // �������I�C���[�p�ɕϊ�
        Vector3 rot = new Vector3(0, (int)dir, 0);

        // ����_���番������̃��C�쐬
        Ray breakRay = new(branchPos, FPS.GetVector3FourDirection(rot));
        // �f�o�b�O
        Debug.DrawRay(breakRay.origin, breakRay.direction * 100, UnityEngine.Color.blue, 5);

        // ���C�L���X�g�ɓ��������ǂ��A�N�e�B�u��
        RaycastHit[] hit = Physics.RaycastAll(breakRay.origin, breakRay.direction,11);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.tag == "wall")
            {
                hit[i].collider.gameObject.SetActive(false);
            }
        }

    }



    void Update()
    {
        gridField.DrowGrid();
    }
}
