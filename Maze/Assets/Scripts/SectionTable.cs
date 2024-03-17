using System;
using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;
using static TakeshiLibrary.FPS;

public class SectionTable
{
    [EnumIndex(typeof(eMapSections)), SerializeField] public GameObject[] sections;
    
    public class Section
    {
        public Coord[] Top { get; }
        public Coord[] Left { get; }
        public Coord[] Right { get; }
        public Coord[] Bottom { get; }

        /// <summary>
        /// 与えられたVector3Intの向きからセクションの向きを返します。
        /// </summary>
        /// <param name="dir">向き</param>
        /// <returns>引数に対応する向きのセクション</returns>
        public Coord[] GetDirectionSection(Coord dir)
        {
            if (dir == Coord.forward)
                return Top;
            else if (dir == Coord.left)
                return Left;
            else if (dir == Coord.right)
                return Right;
            else
                return Bottom;
        }

        public Section(Coord[] top, Coord[] left, Coord[] right, Coord[] bottom)
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
    private static readonly Coord[] T_Top =
    {
        new Coord( 0, 0),
        new Coord(-1, 1),
        new Coord( 0, 1),
        new Coord( 1, 1)
    };

    private static readonly Coord[] T_Bottom =
    {
        new Coord( 0, 0),
        new Coord( 1,-1),
        new Coord( 0,-1),
        new Coord(-1,-1)
    };

    private static readonly Coord[] T_Left = 
    {
        new Coord( 0, 0),
        new Coord(-1,-1),
        new Coord(-1, 0),
        new Coord(-1, 1)
    };

    private static readonly Coord[] T_Right =
    {                    
        new Coord( 0, 0),
        new Coord( 1, 1),
        new Coord( 1, 0),
        new Coord( 1,-1)
    };


    // === I ======================================
    private static readonly Coord[] I_Top =
    {
        new Coord( 0, 0),
        new Coord( 0, 1),
        new Coord( 0, 2),
        new Coord( 0, 3)
    };

    private static readonly Coord[] I_Bottom = 
    {
        new Coord( 0, 0),
        new Coord( 0,-1),
        new Coord( 0,-2),
        new Coord( 0,-3)
    };

    private static readonly Coord[] I_Left = 
    {
        new Coord( 0, 0),
        new Coord(-1, 0),
        new Coord(-2, 0),
        new Coord(-3, 0)
    };

    private static readonly Coord[] I_Right = 
    {
        new Coord( 0, 0),
        new Coord( 1, 0),
        new Coord( 2, 0),
        new Coord( 3, 0)
    };


    // === O ======================================
    private static readonly Coord[] O_Top = 
    {
        new Coord( 0, 0),
        new Coord(-1, 0),
        new Coord(-1, 1),
        new Coord( 0, 1)
    };

    private static readonly Coord[] O_Bottom =
    {
        new Coord( 0, 0),
        new Coord( 1, 0),
        new Coord( 1,-1),
        new Coord( 0,-1)
    };

    private static readonly Coord[] O_Left = 
    {
        new Coord( 0, 0),
        new Coord( 0,-1),
        new Coord(-1,-1),
        new Coord(-1, 0)
    };

    private static readonly Coord[] O_Right = 
    {
        new Coord( 0, 0),
        new Coord( 0, 1),
        new Coord( 1, 1),
        new Coord( 1, 0)
    };


    // === L ======================================
    private static readonly Coord[] L_Top =
    {
        new Coord( 0, 0),
        new Coord( 0, 1),
        new Coord( 0, 2),
        new Coord( 1, 0)
    };

    private static readonly Coord[] L_Bottom =
    {
        new Coord( 0, 0),
        new Coord( 0,-1),
        new Coord( 0,-2),
        new Coord(-1, 0)
    };

    private static readonly Coord[] L_Left =
    {
        new Coord( 0, 0),
        new Coord(-1, 0),
        new Coord(-2, 0),
        new Coord( 0, 1)
    };

    private static readonly Coord[] L_Right =
    {
        new Coord( 0, 0),
        new Coord( 1, 0),
        new Coord( 2, 0),
        new Coord( 0,-1)
    };


    // === J ======================================
    private static readonly Coord[] J_Top =
    {
        new Coord( 0, 0),
        new Coord(-1, 0),
        new Coord( 0, 1),
        new Coord( 0, 2)
    };

    private static readonly Coord[] J_Bottom =
    {
        new Coord( 0, 0),
        new Coord( 1, 0),
        new Coord( 0,-1),
        new Coord( 0,-2)
    };

    private static readonly Coord[] J_Left =
    {
        new Coord( 0, 0),
        new Coord( 0,-1),
        new Coord(-1, 0),
        new Coord(-2, 0)
    };

    private static readonly Coord[] J_Right =
    {
        new Coord( 0, 0),
        new Coord( 0, 1),
        new Coord( 1, 0),
        new Coord( 2, 0)
    };


    // === S ======================================
    private static readonly Coord[] S_Top =
    {
        new Coord( 0,0),
        new Coord(-1,0),
        new Coord( 0,1),
        new Coord( 1,1)
    };

    private static readonly Coord[] S_Bottom =
    {
        new Coord( 0, 0),
        new Coord( 1, 0),
        new Coord( 0,-1),
        new Coord(-1,-1)
    };

    private static readonly Coord[] S_Left =
    {
        new Coord( 0, 0),
        new Coord( 0,-1),
        new Coord(-1, 0),
        new Coord(-1, 1)
    };

    private static readonly Coord[] S_Right =
    {
        new Coord( 0, 0),
        new Coord( 0, 1),
        new Coord( 1, 0),
        new Coord( 1,-1)
    };


    // === Z ======================================
    private static readonly Coord[] Z_Top =
    {
        new Coord( 0, 0),
        new Coord(-1, 1),
        new Coord( 0, 1),
        new Coord( 1, 0)
    };

    private static readonly Coord[] Z_Bottom =
    {
        new Coord( 0, 0),
        new Coord( 1,-1),
        new Coord( 0,-1),
        new Coord(-1, 0)
    };

    private static readonly Coord[] Z_Left =
    {
        new Coord( 0, 0),
        new Coord(-1,-1),
        new Coord(-1, 0),
        new Coord( 0, 1)
    };

    private static readonly Coord[] Z_Right =
    {
        new Coord( 0, 0),
        new Coord( 1, 1),
        new Coord( 1, 0),
        new Coord( 0,-1)
    };

    public static readonly Section T = new Section(T_Top, T_Left, T_Right, T_Bottom);
    public static readonly Section I = new Section(I_Top, I_Left, I_Right, I_Bottom);
    public static readonly Section O = new Section(O_Top, O_Left, O_Right, O_Bottom);
    public static readonly Section L = new Section(L_Top, L_Left, L_Right, L_Bottom);
    public static readonly Section J = new Section(J_Top, J_Left, J_Right, J_Bottom);
    public static readonly Section S = new Section(S_Top, S_Left, S_Right, S_Bottom);
    public static readonly Section Z = new Section(Z_Top, Z_Left, Z_Right, Z_Bottom);


    /// <summary>
    /// ランダムにセクションを返します
    /// </summary>
    public static Section randSection 
    {
        get
        {
            int rand = UnityEngine.Random.Range(0,7);
            switch (rand)
            {
                case 0:
                    return T;

                case 1:
                    return I;

                case 2:
                    return O;

                case 3:
                    return L;

                case 4:
                    return J;

                case 5:
                    return S;

                default:
                    return Z;
            }
        }
    }

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
