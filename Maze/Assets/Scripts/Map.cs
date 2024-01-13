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

    /*ブロック*/
    [SerializeField] Section section;
    [SerializeField] Elements startElements;
    [SerializeField] Elements elements1;
    [SerializeField] Elements elements2;
    [SerializeField] Elements elements3;

    // グリッドのセルの情報を格納する配列
    public Elements.eElementType[,] mapElements;

    int instanceCount1 = 0;
    int instanceCount2 = 1;
    int instanceCount3 = 2;

    [SerializeField]Player player;

    protected override void Start()
    {
        base.Start();

        // mapElements の初期化
        mapElements = new Elements.eElementType[gridWidth, gridDepth];
        for(int x = 0; x < gridWidth; x++) 
        {

            for (int z = 0; z < gridDepth; z++)
            {
                // 端は範囲外
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

        startElements = new Elements(gridField.GetGridCoordinate(gridField.grid[50,48]),eFourDirection.top,Section.eMapSections.O_Section);
        mapElements = startElements.SetElementType(mapElements, startElements.seedElementCoord, startElements.branchElementCoord); 
        //for (int x = 0; x < gridWidth; x++)
        //{

        //    for (int z = 0; z < gridDepth; z++)
        //    {
        //        if (mapElements[x, z] == Elements.eElementType.Seed_Element)
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
    /// ブロックをインスタンスします
    /// </summary>
    /// <param name="playerPosition">インスタンスする場所</param>
    /// <param name="instanceRot">インスタンスする向き</param>
    public void InstanceMapBlock(Vector3 playerPosition, Quaternion instanceRot)
    {
        Debug.Log(section.mapSection[instanceCount1]);
        eFourDirection direction = FPS.GetFourDirection(instanceRot.eulerAngles);
        elements1 = new Elements(gridField.GetGridCoordinate(playerPosition), direction, section.mapSection[instanceCount1]);


        // セクションがインスタンス可能なら
        if (CheckInstanceSection(elements1.seedElementCoord, elements1.branchElementCoord))
        {

            mapElements = elements1.SetElementType(mapElements, elements1.seedElementCoord, elements1.branchElementCoord);

            while (true)
            {
                int randBranch1 = UnityEngine.Random.Range(0, 3);
                Vector3 branchPos1 = gridField.GetGridPosition(gridField.grid[elements1.branchElementCoord[randBranch1].x, elements1.branchElementCoord[randBranch1].z]);
                eFourDirection randDir1 = FPS.RandomFourDirection();

                Debug.Log(randBranch1);

                elements2 = new Elements(gridField.GetGridCoordinate(branchPos1), randDir1, section.mapSection[instanceCount2]);
                if (CheckInstanceSection(elements2.seedElementCoord, elements2.branchElementCoord))
                {
                    int count = 0; 
                            mapElements = elements2.SetElementType(mapElements, elements2.seedElementCoord, elements2.branchElementCoord);
                    while (true)
                    {
                        int randBranch2 = UnityEngine.Random.Range(0, 3);
                        Vector3 branchPos2 = gridField.GetGridPosition(gridField.grid[elements1.branchElementCoord[randBranch2].x, elements1.branchElementCoord[randBranch2].z]);
                        eFourDirection randDir2 = FPS.RandomFourDirection();
                        
                        count++;
                        if(count >= 100)
                        {
                            Debug.Log("そこには置けない");
                            break;
                        }

                        elements3 = new Elements(gridField.GetGridCoordinate(branchPos2), randDir2, section.mapSection[instanceCount3]);

                        if (CheckInstanceSection(elements3.seedElementCoord, elements3.branchElementCoord))
                        {
                            mapElements = elements3.SetElementType(mapElements, elements3.seedElementCoord, elements3.branchElementCoord);
                            // セクション1をインスタンスする
                            Instantiate(section.sections[(int)section.mapSection[instanceCount1]],
                                            gridField.grid[elements1.seedElementCoord.x, elements1.seedElementCoord.z],
                                            instanceRot);
                            // セクション1のプレイヤーの目の前の壁をなくす
                            breakWall(playerPosition,direction);


                            // セクション2をインスタンスする
                            Instantiate(section.sections[(int)section.mapSection[instanceCount2]],
                                            gridField.grid[elements2.seedElementCoord.x, elements2.seedElementCoord.z],
                                            FPS.GetFourDirectionEulerAngles(new Vector3(0, (int)randDir1, 0)));
                            // セクション2の壁をなくす
                            breakWall(branchPos1, randDir1);


                            // セクション3をインスタンスする
                            Instantiate(section.sections[(int)section.mapSection[instanceCount3]],
                                            gridField.grid[elements3.seedElementCoord.x, elements3.seedElementCoord.z],
                                            FPS.GetFourDirectionEulerAngles(new Vector3(0, (int)randDir2, 0)));
                            // セクション3の壁をなくす
                            breakWall(branchPos2, randDir2);

                            instanceCount1++;
                            instanceCount2++;
                            instanceCount3++;

                            // カウントが回ったらシャッフル
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

                            // =========デバッグ====================================================================================================

                            //for (int x = 0; x < gridWidth; x++)
                            //{

                            //    for (int z = 0; z < gridDepth; z++)
                            //    {
                            //        if (mapElements[x, z] == Elements.eElementType.Seed_Element)
                            //        {
                            //            Instantiate(red, gridField.grid[x, z], Quaternion.identity);
                            //        }
                            //        else if (mapElements[x, z] == Elements.eElementType.Branch_Element)
                            //        {
                            //            Instantiate(blue, gridField.grid[x, z], Quaternion.identity);
                            //        }
                            //    }
                            //}
                            break;
                        }
                    }
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 与えた種エレメントと枝エレメントの位置からインスタンス可能か調べます
    /// </summary>
    /// <param name="seed">種エレメント座標</param>
    /// <param name="branch">枝エレメント座標</param>
    /// <returns>インスタンス可能かどうか</returns>
    private bool CheckInstanceSection(Vector3Int seed,Vector3Int[] branch)
    {
        if (CheckCell(branch[0]) == true &&
            CheckCell(branch[1]) == true &&
            CheckCell(branch[2]) == true &&
            CheckCell(seed) == true )
        {
            return true;
        }
        Debug.Log("そこでは道を開けません");
        return false;
    }

    /// <summary>
    /// 与えた座標のエレメント座標が None_Element なら true を返します
    /// </summary>
    /// <param name="coord">座標</param>
    /// <returns>座標が None_Element かどうか</returns>
    private bool CheckCell(Vector3Int coord)
    {
        if (mapElements[coord.x, coord.z] == Elements.eElementType.None_Element)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 分岐点にレイキャストを出して当たった壁を壊します
    /// </summary>
    /// <param name="branchPos">分岐点</param>
    /// <param name="dir">分岐向き</param>
    private void breakWall(Vector3 branchPos, eFourDirection dir)
    {
        // 向きをオイラー角に変換
        Vector3 rot = new Vector3(0, (int)dir, 0);

        // 分岐点から分岐向きのレイ作成
        Ray breakRay = new(branchPos, FPS.GetVector3FourDirection(rot));
        // デバッグ
        Debug.DrawRay(breakRay.origin, breakRay.direction * 100, UnityEngine.Color.blue, 5);

        // レイキャストに当たった壁を非アクティブ化
        RaycastHit[] hit = Physics.RaycastAll(breakRay.origin, breakRay.direction,11);

        for (int i = 0; i < hit.Length; i++)
        {
            hit[i].collider.gameObject.SetActive(false);
        }

    }



    void Update()
    {
        //gridField.DrowGrid();
    }
}
