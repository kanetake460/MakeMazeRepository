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

    /*ブロック*/
    [SerializeField] Section section;

    int instanceCount = 0;

    enum eElementType
    {
        Seed_Element,       // 種エレメント
        Branch_Element,     // 枝エレメント
        None_Element,       // エレメントなし
        OutRange_Element,   // 範囲外
    }

    // グリッドのセルの情報を格納する配列
    eElementType[,] mapElements;



    [SerializeField]Player player;

    protected override void Start()
    {
        base.Start();

        // mapCells の初期化
        mapElements = new eElementType[gridWidth, gridDepth];
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
    /// ブロックをインスタンスします
    /// </summary>
    /// <param name="playerPosition">インスタンスする場所</param>
    /// <param name="instanceRot">インスタンスする向き</param>
    public void InstanceMapBlock(Vector3 playerPosition,Quaternion instanceRot)
    {

        if (CheckCell(gridField.GetOtherGridCoordinate(playerPosition,
            GetPreviousCoordinate(instanceRot.eulerAngles))) == true)    // もし、プレイヤーの前のセルが置けるセルなら
        {
        Debug.Log(section.mapSection1[instanceCount]);
        Debug.Log(section.mapSection2[instanceCount]);

            // おいたとき、プレイヤーの目の前のセルは種エレメント
            Vector3Int seedElementCoord = gridField.GetOtherGridCoordinate(playerPosition, GetPreviousCoordinate(instanceRot.eulerAngles));
            mapElements[seedElementCoord.x, seedElementCoord.z] = eElementType.Seed_Element;

            // おいたブロックの種以外の部分は枝エレメント
            FPS.eFourDirection fourDirection = FPS.GetFourDirection(instanceRot.eulerAngles);
                Vector3Int[] branchElementCoord = new Vector3Int[3];
            for (int i = 0; i < 3; i++)
            {
                branchElementCoord[i] = section.GetBranchElement(section.mapSection1[instanceCount],fourDirection, seedElementCoord, i);
                mapElements[branchElementCoord[i].x,branchElementCoord[i].z] = eElementType.Branch_Element;
            }

            // セクション1をインスタンスする
            Instantiate(section.sections[(int)section.mapSection1[instanceCount]],
                        gridField.GetOtherGridPosition(playerPosition, GetPreviousCoordinate(instanceRot.eulerAngles)),
                        instanceRot);

            instanceCount++;

            // カウントが回ったらシャッフル
            if (instanceCount == section.sections.Length)
            {
                Algorithm.Shuffle(section.mapSection1);
                Algorithm.Shuffle(section.mapSection2);
                instanceCount = 0;
            }

// =========デバッグ====================================================================================================

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
        Debug.Log("そこでは道を開けません");
        return false;
    }

    /// <summary>
    /// 向きに対応するひとつ前のグリッド座標を返します
    /// </summary>
    /// <param name="eulerAngles">向き</param>
    /// <returns>向いている方向の一つ前のグリッド座標</returns>
    public Vector3Int GetPreviousCoordinate(Vector3 eulerAngles)
    {
        FPS.eFourDirection fourDirection = FPS.GetFourDirection(eulerAngles);   // 向きを調べて代入
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
