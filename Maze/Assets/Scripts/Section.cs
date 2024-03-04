using System;
using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;
using static TakeshiLibrary.FPS;

public class Section : MonoBehaviour
{
    [EnumIndex(typeof(eMapSections)), SerializeField] public GameObject[] sections;
    [SerializeField] MakeMap map;

    /*それぞれのセクションのテーブル*/
    // 左下から設定しています

    // === T ======================================
    public static readonly Vector3Int[] T_Top_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1)
    };

    public static readonly Vector3Int[] T_Bottom_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1)
    };

    public static readonly Vector3Int[] T_Left_Branch = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1)
    };

    public static readonly Vector3Int[] T_Right_Branch =
    {                    
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1)
    };


    // === I ======================================
    public static readonly Vector3Int[] I_Top_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2),
        new Vector3Int( 0,0, 3)
    };

    public static readonly Vector3Int[] I_Bottom_Branch = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2),
        new Vector3Int( 0,0,-3)
    };

    public static readonly Vector3Int[] I_Left_Branch = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0),
        new Vector3Int(-3,0, 0)
    };

    public static readonly Vector3Int[] I_Right_Branch = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0),
        new Vector3Int( 3,0, 0)
    };


    // === O ======================================
    public static readonly Vector3Int[] O_Top_Branch = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1)
    };

    public static readonly Vector3Int[] O_Bottom_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1)
    };

    public static readonly Vector3Int[] O_Left_Branch = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0)
    };

    public static readonly Vector3Int[] O_Right_Branch = 
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0)
    };


    // === L ======================================
    public static readonly Vector3Int[] L_Top_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2),
        new Vector3Int( 1,0, 0)
    };

    public static readonly Vector3Int[] L_Bottom_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2),
        new Vector3Int(-1,0, 0)
    };

    public static readonly Vector3Int[] L_Left_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0),
        new Vector3Int( 0,0, 1)
    };

    public static readonly Vector3Int[] L_Right_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0),
        new Vector3Int( 0,0,-1)
    };


    // === J ======================================
    public static readonly Vector3Int[] J_Top_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2)
    };

    public static readonly Vector3Int[] J_Bottom_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2)
    };

    public static readonly Vector3Int[] J_Left_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0)
    };

    public static readonly Vector3Int[] J_Right_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0)
    };


    // === S ======================================
    public static readonly Vector3Int[] S_Top_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1)
    };

    public static readonly Vector3Int[] S_Bottom_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1)
    };

    public static readonly Vector3Int[] S_Left_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1)
    };

    public static readonly Vector3Int[] S_Right_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1)
    };


    // === Z ======================================
    public static readonly Vector3Int[] Z_Top_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0)
    };

    public static readonly Vector3Int[] Z_Bottom_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0)
    };

    public static readonly Vector3Int[] Z_Left_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1)
    };

    public static readonly Vector3Int[] Z_Right_Branch =
    {
        new Vector3Int( 0,0, 0),
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1)
    };


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
