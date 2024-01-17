using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using UnityEngine;
using static Section;
using static System.Collections.Specialized.BitVector32;

public class SetElements : Section
{
    public Vector3Int seedElementCoord;
    public Vector3Int[] branchElementCoord = new Vector3Int[3];

    public enum eElementType
    {
        Seed_Element,       // 種エレメント
        Branch_Element,     // 枝エレメント
        None_Element,       // エレメントなし
        Room_Element,       // フラグがある部屋エレメント
        OutRange_Element,   // 範囲外
    }


    public SetElements(Vector3Int coord, FPS.eFourDirection fourDirection,eMapSections mapSection)
    {
        
        seedElementCoord = coord + GetPreviousCoordinate(fourDirection);
        
        for (int i = 0; i < 3; i++)
        {
            branchElementCoord[i] = GetBranchElement(mapSection, fourDirection, seedElementCoord, i);
        }
    }

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
    /// 向きに対応するひとつ前のグリッド座標を返します
    /// </summary>
    /// <param name="eulerAngles">向き</param>
    /// <returns>向いている方向の一つ前のグリッド座標</returns>
    public static Vector3Int GetPreviousCoordinate(FPS.eFourDirection fourDirection)
    {
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
