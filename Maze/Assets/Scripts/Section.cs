using System;
using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using UnityEngine;
using static TakeshiClass.FPS;

public class Section : MapGridField
{
    [EnumIndex(typeof(eMapSections)), SerializeField] public GameObject[] sections;
    [SerializeField] Map map;

    /*それぞれのセクションのテーブル*/
    // 左下から設定しています

    // === T ======================================
    protected static readonly Vector3Int[] T_Top_Branch =
    {
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1)
    };

    protected static readonly Vector3Int[] T_Bottom_Branch =
    {
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1)
    };

    protected static readonly Vector3Int[] T_Left_Branch = 
    {
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1)
    };

    protected static readonly Vector3Int[] T_Right_Branch = new Vector3Int[3]
    {                    
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1)
    };


    // === I ======================================
    protected static readonly Vector3Int[] I_Top_Branch =
    {
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2),
        new Vector3Int( 0,0, 3)
    };

    protected static readonly Vector3Int[] I_Bottom_Branch = 
    {
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2),
        new Vector3Int( 0,0,-3)
    };

    protected static readonly Vector3Int[] I_Left_Branch = 
    {
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0),
        new Vector3Int(-3,0, 0)
    };

    protected static readonly Vector3Int[] I_Right_Branch = 
    {
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0),
        new Vector3Int( 3,0, 0)
    };


    // === O ======================================
    protected static readonly Vector3Int[] O_Top_Branch = 
    {
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1)
    };

    protected static readonly Vector3Int[] O_Bottom_Branch =
    {
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1)
    };

    protected static readonly Vector3Int[] O_Left_Branch = 
    {
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0)
    };

    protected static readonly Vector3Int[] O_Right_Branch = 
    {
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0)
    };


    // === L ======================================
    protected static readonly Vector3Int[] L_Top_Branch =
    {
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2),
        new Vector3Int( 1,0, 0)
    };

    protected static readonly Vector3Int[] L_Bottom_Branch =
    {
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2),
        new Vector3Int(-1,0, 0)
    };

    protected static readonly Vector3Int[] L_Left_Branch =
    {
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0),
        new Vector3Int( 0,0, 1)
    };

    protected static readonly Vector3Int[] L_Right_Branch =
    {
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0),
        new Vector3Int( 0,0,-1)
    };


    // === J ======================================
    protected static readonly Vector3Int[] J_Top_Branch =
    {
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2)
    };

    protected static readonly Vector3Int[] J_Bottom_Branch =
    {
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2)
    };

    protected static readonly Vector3Int[] J_Left_Branch =
    {
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0)
    };

    protected static readonly Vector3Int[] J_Right_Branch =
    {
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0)
    };


    // === S ======================================
    protected static readonly Vector3Int[] S_Top_Branch =
    {
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1)
    };

    protected static readonly Vector3Int[] S_Bottom_Branch =
    {
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1)
    };

    protected static readonly Vector3Int[] S_Left_Branch =
    {
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1)
    };

    protected static readonly Vector3Int[] S_Right_Branch =
    {
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1)
    };


    // === Z ======================================
    protected static readonly Vector3Int[] Z_Top_Branch =
    {
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0)
    };

    protected static readonly Vector3Int[] Z_Bottom_Branch =
    {
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0)
    };

    protected static readonly Vector3Int[] Z_Left_Branch =
    {
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1)
    };

    protected static readonly Vector3Int[] Z_Right_Branch =
    {
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

    protected override void Start()
    {
        base.Start();

        // mapSection の初期化、シャッフル
            for (int j = 0; j < sections.Length; j++)
            {
                mapSection[j] = (eMapSections)Enum.ToObject(typeof(eMapSections), j);
            }
        Algorithm.Shuffle(mapSection);
    }


    

    void Update()
    {
        
    }
}
