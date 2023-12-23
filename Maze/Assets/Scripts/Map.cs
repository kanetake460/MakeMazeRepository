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
    [SerializeField] Elements elements1;
    // グリッドのセルの情報を格納する配列
    Elements.eElementType[,] mapElements;

    int instanceCount = 0;

    [SerializeField]Player player;

    protected override void Start()
    {
        base.Start();

        // mapCells の初期化
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
    }

    /// <summary>
    /// ブロックをインスタンスします
    /// </summary>
    /// <param name="playerPosition">インスタンスする場所</param>
    /// <param name="instanceRot">インスタンスする向き</param>
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



        // セクションがインスタンス可能なら
        if (CheckInstanceSection(elements1.seedElementCoord, elements1.branchElementCoord))
        {
            // プレイヤーの前の座標を種エレメントに
            mapElements[elements1.seedElementCoord.x, elements1.seedElementCoord.z] = Elements.eElementType.Seed_Element;

            // セクションのそのほかのエレメントを枝エレメントに
            for (int i = 0; i < 3; i++)
            {
                mapElements[elements1.branchElementCoord[i].x, elements1.branchElementCoord[i].z] = Elements.eElementType.Branch_Element;
            }


            // セクション1をインスタンスする
            Instantiate(section.sections[(int)section.mapSection[instanceCount]],
                            gridField.grid[elements1.seedElementCoord.x,elements1.seedElementCoord.z],
                            instanceRot);

            // セクション1をインスタンスする
            Instantiate(section.sections[(int)section.mapSection[instanceCount]],
                            gridField.grid[elements1.seedElementCoord.x, elements1.seedElementCoord.z],
                            instanceRot);

            Vector3 branchPos1 = gridField.GetOtherGridPosition(gridField.grid[elements1.branchElementCoord[0].x, elements1.branchElementCoord[0].z], Elements.GetPreviousCoordinate(GetFourDirection(UnityEngine.Random.)));

            // セクション2をインスタンスする
            Instantiate(section.sections[(int)section.mapSection[instanceCount]],
                            gridField.GetOtherGridPosition(gridField.grid[elements1.branchElementCoord[0].x, elements1.branchElementCoord[0].z], Elements.GetPreviousCoordinate(UnityEngine.Random.rotation.eulerAngles)),
                            instanceRot);

            instanceCount++;

            // カウントが回ったらシャッフル
            if (instanceCount == section.sections.Length)
            {
                Algorithm.Shuffle(section.mapSection);
                instanceCount = 0;
            }

            // =========デバッグ====================================================================================================

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




    void Update()
    {
        gridField.DrowGrid();

    }
}
