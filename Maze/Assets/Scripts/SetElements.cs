using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;
using static Section;
using static System.Collections.Specialized.BitVector32;

public class Elements : Section
{
    public Vector3Int seedElementCoord;                         // 種エレメントのグリッド座標
    public Vector3Int[] branchElementCoord = new Vector3Int[3]; // 枝エレメントのグリッド座標の配列

    public enum eElementType
    {
        Seed_Element,       // 種エレメント
        Branch_Element,     // 枝エレメント
        None_Element,       // エレメントなし
        Room_Element,       // フラグがある部屋エレメント
        OutRange_Element,   // 範囲外
    }

    /// <summary>
    /// エレメントのコンストラクタ
    /// </summary>
    /// <param name="coord">グリッド座標</param>
    /// <param name="fourDirection">向き</param>
    /// <param name="mapSection">セクションの種類</param>
    public Elements(Vector3Int coord, FPS.eFourDirection fourDirection,eMapSections mapSection)
    {
        // 種エレメントの座標にグリッド座標＋向きの方向に１
        seedElementCoord = coord + GetPreviousCoordinate(fourDirection);
        
        for (int i = 0; i < 3; i++)
        {
            // 枝エレメントを入れる
            branchElementCoord[i] = GetBranchElement(mapSection, fourDirection, seedElementCoord, i);
        }
    }

    /// <summary>
    /// 座標にエレメントを確約します
    /// </summary>
    /// <param name="elements">エレメント</param>
    /// <param name="seed">種のグリッド座標</param>
    /// <param name="branch">枝のグリッド座標</param>
    /// <returns></returns>
    public eElementType[,] SetElementType(eElementType[,] elements, Vector3Int seed, Vector3Int[] branch)
    {
        // プレイヤーの前の座標を種エレメントに
        elements[seed.x, seed.z] = eElementType.Seed_Element;

        // セクションのそのほかのエレメントを枝エレメントに
        for (int i = 0; i < 3; i++)
        {
            elements[branch[i].x, branch[i].z] = eElementType.Branch_Element;
        }
        return elements;
    }

    /// <summary>
    /// エレメントをNoneにします
    /// </summary>
    /// <param name="elements">エレメント</param>
    /// <param name="seed">種</param>
    /// <param name="branch">枝</param>
    /// <returns></returns>
    public eElementType[,] RestoreElementType(eElementType[,] elements, Vector3Int seed, Vector3Int[] branch)
    {
        // プレイヤーの前の座標をなしエレメントに
        elements[seed.x, seed.z] = eElementType.None_Element;

        // セクションのそのほかのエレメントをなしエレメントに
        for (int i = 0; i < 3; i++)
        {
            elements[branch[i].x, branch[i].z] = eElementType.None_Element;
        }
        return elements;
    }


    /// <summary>
    /// 向きに対応するひとつ前のグリッド座標を返します
    /// </summary>
    /// <param name="fourDirection">向き</param>
    /// <returns>向いている方向の一つ前のグリッド座標</returns>
    public static Vector3Int GetPreviousCoordinate(FPS.eFourDirection fourDirection)
    {
        switch (fourDirection)
        {
            case FPS.eFourDirection.top:
                return Vector3Int.forward;

            case FPS.eFourDirection.bottom:
                return Vector3Int.back;

            case FPS.eFourDirection.left:
                return Vector3Int.left;

            case FPS.eFourDirection.right:
                return Vector3Int.right;
        }
        return Vector3Int.zero;
    }

    /// <summary>
    /// それぞれのセクションのそれぞれの向きの枝エレメントを返します
    /// </summary>
    /// <param name="section">セクション</param>
    /// <param name="direction">向き</param>
    /// <param name="seedCoord">セクションのシードのグリッド座標</param>
    /// <param name="elementIndx">要素のインデックス</param>
    /// <returns>インデックスに対応するエレメントのグリッド座標</returns>
    public Vector3Int GetBranchElement(eMapSections section, FPS.eFourDirection direction, Vector3Int seedCoord, int elementIndx)
    {
        switch (section)
        {
            // === T ======================================
            case eMapSections.T_Section:
                switch (direction)
                {
                    case FPS.eFourDirection.top:
                        return seedCoord + T_Top_Branch[elementIndx];

                    case FPS.eFourDirection.bottom:
                        return seedCoord + T_Bottom_Branch[elementIndx];

                    case FPS.eFourDirection.left:
                        return seedCoord + T_Left_Branch[elementIndx];

                    case FPS.eFourDirection.right:
                        return seedCoord + T_Right_Branch[elementIndx];
                }
                Debug.LogError("ラベルを出ました");
                return Vector3Int.zero;


            // === I ======================================
            case eMapSections.I_Section:
                switch (direction)
                {
                    case FPS.eFourDirection.top:
                        return seedCoord + I_Top_Branch[elementIndx];

                    case FPS.eFourDirection.bottom:
                        return seedCoord + I_Bottom_Branch[elementIndx];

                    case FPS.eFourDirection.left:
                        return seedCoord + I_Left_Branch[elementIndx];

                    case FPS.eFourDirection.right:
                        return seedCoord + I_Right_Branch[elementIndx];
                }
                Debug.LogError("ラベルを出ました");
                return Vector3Int.zero;


            // === O ======================================
            case eMapSections.O_Section:
                switch (direction)
                {
                    case FPS.eFourDirection.top:
                        return seedCoord + O_Top_Branch[elementIndx];

                    case FPS.eFourDirection.bottom:
                        return seedCoord + O_Bottom_Branch[elementIndx];

                    case FPS.eFourDirection.left:
                        return seedCoord + O_Left_Branch[elementIndx];

                    case FPS.eFourDirection.right:
                        return seedCoord + O_Right_Branch[elementIndx];
                }
                Debug.LogError("ラベルを出ました");
                return Vector3Int.zero;


            // === L ======================================
            case eMapSections.L_Section:
                switch (direction)
                {
                    case FPS.eFourDirection.top:
                        return seedCoord + L_Top_Branch[elementIndx];

                    case FPS.eFourDirection.bottom:
                        return seedCoord + L_Bottom_Branch[elementIndx];

                    case FPS.eFourDirection.left:
                        return seedCoord + L_Left_Branch[elementIndx];

                    case FPS.eFourDirection.right:
                        return seedCoord + L_Right_Branch[elementIndx];
                }
                Debug.LogError("ラベルを出ました");
                return Vector3Int.zero;


            // === J ======================================
            case eMapSections.J_Section:
                switch (direction)
                {
                    case FPS.eFourDirection.top:
                        return seedCoord + J_Top_Branch[elementIndx];

                    case FPS.eFourDirection.bottom:
                        return seedCoord + J_Bottom_Branch[elementIndx];

                    case FPS.eFourDirection.left:
                        return seedCoord + J_Left_Branch[elementIndx];

                    case FPS.eFourDirection.right:
                        return seedCoord + J_Right_Branch[elementIndx];
                }
                Debug.LogError("ラベルを出ました");
                return Vector3Int.zero;


            // === S ======================================
            case eMapSections.S_Section:
                switch (direction)
                {
                    case FPS.eFourDirection.top:
                        return seedCoord + S_Top_Branch[elementIndx];

                    case FPS.eFourDirection.bottom:
                        return seedCoord + S_Bottom_Branch[elementIndx];

                    case FPS.eFourDirection.left:
                        return seedCoord + S_Left_Branch[elementIndx];

                    case FPS.eFourDirection.right:
                        return seedCoord + S_Right_Branch[elementIndx];
                }
                Debug.LogError("ラベルを出ました");
                return Vector3Int.zero;


            // === Z ======================================
            case eMapSections.Z_Section:
                switch (direction)
                {
                    case FPS.eFourDirection.top:
                        return seedCoord + Z_Top_Branch[elementIndx];

                    case FPS.eFourDirection.bottom:
                        return seedCoord + Z_Bottom_Branch[elementIndx];

                    case FPS.eFourDirection.left:
                        return seedCoord + Z_Left_Branch[elementIndx];

                    case FPS.eFourDirection.right:
                        return seedCoord + Z_Right_Branch[elementIndx];
                }
                Debug.LogError("ラベルを出ました");
                return Vector3Int.zero;
        }
        Debug.LogError("ラベルを出ました");
        return Vector3Int.zero;
    }
}
