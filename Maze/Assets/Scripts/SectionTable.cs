using System;
using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;
using static TakeshiLibrary.FPS;

public class SectionTable
{
    [EnumIndex(typeof(eMapSections)), SerializeField] public GameObject[] sections;
    
    public class Sections
    {
        public Vector3Int[] Top;
        public Vector3Int[] Left;
        public Vector3Int[] Right;
        public Vector3Int[] Bottom;

        public Sections(Vector3Int[] top, Vector3Int[] left, Vector3Int[] right, Vector3Int[] bottom)
        {
            Top = top;
            Left = left;
            Right = right;
            Bottom = bottom;
        }
    }


    /*それぞれのセクションのテーブル*/
    // 左下から設定しています

    // === T ======================================
    private static readonly Vector3Int[] T_Top =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1)
    };

    private static readonly Vector3Int[] T_Bottom =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1)
    };

    private static readonly Vector3Int[] T_Left = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1)
    };

    private static readonly Vector3Int[] T_Right =
    {                    
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1)
    };


    // === I ======================================
    private static readonly Vector3Int[] I_Top =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2),
        new Vector3Int( 0,0, 3)
    };

    private static readonly Vector3Int[] I_Bottom = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2),
        new Vector3Int( 0,0,-3)
    };

    private static readonly Vector3Int[] I_Left = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0),
        new Vector3Int(-3,0, 0)
    };

    private static readonly Vector3Int[] I_Right = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0),
        new Vector3Int( 3,0, 0)
    };


    // === O ======================================
    private static readonly Vector3Int[] O_Top = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1)
    };

    private static readonly Vector3Int[] O_Bottom =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1)
    };

    private static readonly Vector3Int[] O_Left = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0)
    };

    private static readonly     Vector3Int[] O_Right = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0)
    };


    // === L ======================================
    private static readonly Vector3Int[] L_Top =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2),
        new Vector3Int( 1,0, 0)
    };

    private static readonly Vector3Int[] L_Bottom =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2),
        new Vector3Int(-1,0, 0)
    };

    private static readonly Vector3Int[] L_Left =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0),
        new Vector3Int( 0,0, 1)
    };

    private static readonly Vector3Int[] L_Right =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0),
        new Vector3Int( 0,0,-1)
    };


    // === J ======================================
    private static readonly Vector3Int[] J_Top =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2)
    };

    private static readonly Vector3Int[] J_Bottom =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2)
    };

    private static readonly Vector3Int[] J_Left =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0)
    };

    private static readonly Vector3Int[] J_Right =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0)
    };


    // === S ======================================
    private static readonly Vector3Int[] S_Top =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1)
    };

    private static readonly Vector3Int[] S_Bottom =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1)
    };

    private static readonly Vector3Int[] S_Left =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1)
    };

    private static readonly Vector3Int[] S_Right =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1)
    };


    // === Z ======================================
    private static readonly Vector3Int[] Z_Top =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0)
    };

    private static readonly Vector3Int[] Z_Bottom =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0)
    };

    private static readonly Vector3Int[] Z_Left =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1)
    };

    private static readonly Vector3Int[] Z_Right =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1)
    };

    public static readonly Sections T = new Sections(T_Top, T_Left, T_Right, T_Bottom);
    public static readonly Sections I = new Sections(I_Top, I_Left, I_Right, I_Bottom);
    public static readonly Sections O = new Sections(O_Top, O_Left, O_Right, O_Bottom);
    public static readonly Sections L = new Sections(L_Top, L_Left, L_Right, L_Bottom);
    public static readonly Sections J = new Sections(J_Top, J_Left, J_Right, J_Bottom);
    public static readonly Sections S = new Sections(S_Top, S_Left, S_Right, S_Bottom);
    public static readonly Sections Z = new Sections(Z_Top, Z_Left, Z_Right, Z_Bottom);


    /*セクションの列挙*/
    public enum eMapSections
    {
        T_Section = 0,
        I_Section = 1,
        O_Section = 2,
        L_Section = 3,
        J_Section = 4,
        S_Section = 5,
        Z_Section = 6
    }

    public eMapSections[] mapSection = new eMapSections[7];    // セクション1

    void Start()
    {

        // mapSection の初期化、シャッフル
            for (int j = 0; j < sections.Length; j++)
            {
            mapSection[j] = (eMapSections)Enum.ToObject(typeof(eMapSections), j);
            }
        Algorithm.Shuffle(mapSection);
    }
}
