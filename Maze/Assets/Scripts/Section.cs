using System;
using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using UnityEngine;

public class Section : MapGridField
{
    [EnumIndex(typeof(eMapSections)), SerializeField] public GameObject[] sections;
    [SerializeField] Map map;

    /*左下から*/
    Vector3Int[] T_Branch = new Vector3Int[3]
                            {new Vector3Int(-1,0,1),
                             new Vector3Int( 0,0,1),
                             new Vector3Int( 1,0,1)};

    Vector3Int[] I_Branch = new Vector3Int[3]
                            {new Vector3Int( 0,0,1),
                             new Vector3Int( 0,0,2),
                             new Vector3Int( 0,0,3)};

    Vector3Int[] O_Branch = new Vector3Int[3]
                            {new Vector3Int(-1,0,0),
                             new Vector3Int(-1,0,1),
                             new Vector3Int( 0,0,1)};

    Vector3Int[] L_Branch = new Vector3Int[3]
                            {new Vector3Int( 0,0,1),
                             new Vector3Int( 0,0,2),
                             new Vector3Int( 1,0,0)};

    Vector3Int[] J_Branch = new Vector3Int[3]
                            {new Vector3Int(-1,0,0),
                             new Vector3Int( 0,0,1),
                             new Vector3Int( 0,0,2)};

    Vector3Int[] S_Branch = new Vector3Int[3]
                            {new Vector3Int(-1,0,0),
                             new Vector3Int( 0,0,1),
                             new Vector3Int( 1,0,1)};

    Vector3Int[] Z_Branch = new Vector3Int[3]
                            {new Vector3Int( 1,0,1),
                             new Vector3Int( 0,0,1),
                             new Vector3Int( 1,0,0)};


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

    public eMapSections[] mapSection1 = new eMapSections[7];
    public eMapSections[] mapSection2 = new eMapSections[7];
    
    protected override void Start()
    {
        base.Start();
        // mapSection の初期化、シャッフル
        for (int i = 0; i < sections.Length; i++)
        {
            mapSection1[i] = (eMapSections)Enum.ToObject(typeof(eMapSections), i);
            mapSection2[i] = (eMapSections)Enum.ToObject(typeof(eMapSections), i);
        }
        Algorithm.Shuffle(mapSection1);
        Algorithm.Shuffle(mapSection2);
    }

    public Vector3Int BranchElement(eMapSections section,Vector3Int seedCoord,int ElementIndx)
    {
        switch (section)
        {
            case eMapSections.T_Section:
                Debug.Log(T_Branch[ElementIndx]);
                return gridField.GetOtherGridCoordinate(seedCoord, T_Branch[ElementIndx]);

            case eMapSections.I_Section:
                return gridField.GetOtherGridCoordinate(seedCoord, I_Branch[ElementIndx]);

            case eMapSections.O_Section:
                return gridField.GetOtherGridCoordinate(seedCoord, O_Branch[ElementIndx]);

            case eMapSections.L_Section:
                return gridField.GetOtherGridCoordinate(seedCoord, L_Branch[ElementIndx]);

            case eMapSections.J_Section:
                return gridField.GetOtherGridCoordinate(seedCoord, J_Branch[ElementIndx]);

            case eMapSections.S_Section:
                return gridField.GetOtherGridCoordinate(seedCoord, S_Branch[ElementIndx]);

            case eMapSections.Z_Section:
                return gridField.GetOtherGridCoordinate(seedCoord, Z_Branch[ElementIndx]);
        }
        return Vector3Int.zero;
    }
    

    void Update()
    {
        
    }
}
