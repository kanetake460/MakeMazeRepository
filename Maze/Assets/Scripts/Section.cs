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

    /*���ꂼ��̃Z�N�V�����̃e�[�u��*/
    // ��������ݒ肵�Ă��܂�

    // === T ======================================
    private static readonly Vector3Int[] T_Top_Branch =
    {
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1)
    };

    private static readonly Vector3Int[] T_Bottom_Branch =
    {
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1)
    };

    private static readonly Vector3Int[] T_Left_Branch = 
    {
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1)
    };

    private static readonly Vector3Int[] T_Right_Branch = new Vector3Int[3]
    {                    
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1)
    };


    // === I ======================================
    private static readonly Vector3Int[] I_Top_Branch =
    {
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2),
        new Vector3Int( 0,0, 3)
    };

    private static readonly Vector3Int[] I_Bottom_Branch = 
    {
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2),
        new Vector3Int( 0,0,-3)
    };

    private static readonly Vector3Int[] I_Left_Branch = 
    {
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0),
        new Vector3Int(-3,0, 0)
    };

    private static readonly Vector3Int[] I_Right_Branch = 
    {
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0),
        new Vector3Int( 3,0, 0)
    };


    // === O ======================================
    private static readonly Vector3Int[] O_Top_Branch = 
    {
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1)
    };

    private static readonly Vector3Int[] O_Bottom_Branch =
    {
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1)
    };

    private static readonly Vector3Int[] O_Left_Branch = 
    {
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0)
    };

    private static readonly Vector3Int[] O_Right_Branch = 
    {
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0)
    };


    // === L ======================================
    private static readonly Vector3Int[] L_Top_Branch =
    {
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2),
        new Vector3Int( 1,0, 0)
    };

    private static readonly Vector3Int[] L_Bottom_Branch =
    {
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2),
        new Vector3Int(-1,0, 0)
    };

    private static readonly Vector3Int[] L_Left_Branch =
    {
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0),
        new Vector3Int( 0,0, 1)
    };

    private static readonly Vector3Int[] L_Right_Branch =
    {
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0),
        new Vector3Int( 0,0,-1)
    };


    // === J ======================================
    private static readonly Vector3Int[] J_Top_Branch =
    {
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 0,0, 2)
    };

    private static readonly Vector3Int[] J_Bottom_Branch =
    {
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int( 0,0,-2)
    };

    private static readonly Vector3Int[] J_Left_Branch =
    {
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-2,0, 0)
    };

    private static readonly Vector3Int[] J_Right_Branch =
    {
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 2,0, 0)
    };


    // === S ======================================
    private static readonly Vector3Int[] S_Top_Branch =
    {
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 1)
    };

    private static readonly Vector3Int[] S_Bottom_Branch =
    {
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0,-1)
    };

    private static readonly Vector3Int[] S_Left_Branch =
    {
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int(-1,0, 1)
    };

    private static readonly Vector3Int[] S_Right_Branch =
    {
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 1,0,-1)
    };


    // === Z ======================================
    private static readonly Vector3Int[] Z_Top_Branch =
    {
        new Vector3Int(-1,0, 1),
        new Vector3Int( 0,0, 1),
        new Vector3Int( 1,0, 0)
    };

    private static readonly Vector3Int[] Z_Bottom_Branch =
    {
        new Vector3Int( 1,0,-1),
        new Vector3Int( 0,0,-1),
        new Vector3Int(-1,0, 0)
    };

    private static readonly Vector3Int[] Z_Left_Branch =
    {
        new Vector3Int(-1,0,-1),
        new Vector3Int(-1,0, 0),
        new Vector3Int( 0,0, 1)
    };

    private static readonly Vector3Int[] Z_Right_Branch =
    {
        new Vector3Int( 1,0, 1),
        new Vector3Int( 1,0, 0),
        new Vector3Int( 0,0,-1)
    };


    /*�Z�N�V�����̗�*/
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

    public eMapSections[] mapSection1 = new eMapSections[7];    // �Z�N�V����1
    public eMapSections[] mapSection2 = new eMapSections[7];    // �Z�N�V����2
    
    protected override void Start()
    {
        base.Start();

        // mapSection �̏������A�V���b�t��
        for (int i = 0; i < sections.Length; i++)
        {
            mapSection1[i] = (eMapSections)Enum.ToObject(typeof(eMapSections), i);
            mapSection2[i] = (eMapSections)Enum.ToObject(typeof(eMapSections), i);
        }
        Algorithm.Shuffle(mapSection1);
        Algorithm.Shuffle(mapSection2);
    }


    /// <summary>
    /// ���ꂼ��̃Z�N�V�����̂��ꂼ��̌����̎}�G�������g��Ԃ��܂�
    /// </summary>
    /// <param name="section">�Z�N�V����</param>
    /// <param name="direction">����</param>
    /// <param name="seedCoord">�Z�N�V�����̃V�[�h�̃O���b�h���W</param>
    /// <param name="elementIndx">�v�f�̃C���f�b�N�X</param>
    /// <returns>�C���f�b�N�X�ɑΉ�����G�������g�̃O���b�h���W</returns>
    public Vector3Int GetBranchElement(eMapSections section, eFourDirection direction, Vector3Int seedCoord, int elementIndx)
    {
        switch (section)
        {
            // === T ======================================
            case eMapSections.T_Section:
                switch (direction)
                {
                    case eFourDirection.top:
                        return seedCoord + T_Top_Branch[elementIndx];

                    case eFourDirection.bottom:
                        return seedCoord + T_Bottom_Branch[elementIndx];

                    case eFourDirection.left:
                        return seedCoord + T_Left_Branch[elementIndx];

                    case eFourDirection.right:
                        return seedCoord + T_Right_Branch[elementIndx];
                }
                Debug.LogError("���x�����o�܂���");
                return Vector3Int.zero;


            // === I ======================================
            case eMapSections.I_Section:
                switch (direction)
                {
                    case eFourDirection.top:
                        return seedCoord + I_Top_Branch[elementIndx];

                    case eFourDirection.bottom:
                        return seedCoord + I_Bottom_Branch[elementIndx];

                    case eFourDirection.left:
                        return seedCoord + I_Left_Branch[elementIndx];

                    case eFourDirection.right:
                        return seedCoord + I_Right_Branch[elementIndx];
                }
                Debug.LogError("���x�����o�܂���");
                return Vector3Int.zero;


            // === O ======================================
            case eMapSections.O_Section:
                switch (direction)
                {
                    case eFourDirection.top:
                        return seedCoord + O_Top_Branch[elementIndx];

                    case eFourDirection.bottom:
                        return seedCoord + O_Bottom_Branch[elementIndx];

                    case eFourDirection.left:
                        return seedCoord + O_Left_Branch[elementIndx];

                    case eFourDirection.right:
                        return seedCoord + O_Right_Branch[elementIndx];
                }
                Debug.LogError("���x�����o�܂���");
                return Vector3Int.zero;


            // === L ======================================
            case eMapSections.L_Section:
                switch (direction)
                {
                    case eFourDirection.top:
                        return seedCoord + L_Top_Branch[elementIndx];

                    case eFourDirection.bottom:
                        return seedCoord + L_Bottom_Branch[elementIndx];

                    case eFourDirection.left:
                        return seedCoord + L_Left_Branch[elementIndx];

                    case eFourDirection.right:
                        return seedCoord + L_Right_Branch[elementIndx];
                }
                Debug.LogError("���x�����o�܂���");
                return Vector3Int.zero;


            // === J ======================================
            case eMapSections.J_Section:
                switch (direction)
                {
                    case eFourDirection.top:
                        return seedCoord + J_Top_Branch[elementIndx];

                    case eFourDirection.bottom:
                        return seedCoord + J_Bottom_Branch[elementIndx];

                    case eFourDirection.left:
                        return seedCoord + J_Left_Branch[elementIndx];

                    case eFourDirection.right:
                        return seedCoord + J_Right_Branch[elementIndx];
                }
                Debug.LogError("���x�����o�܂���");
                return Vector3Int.zero;


            // === S ======================================
            case eMapSections.S_Section:
                switch (direction)
                {
                    case eFourDirection.top:
                        return seedCoord + S_Top_Branch[elementIndx];

                    case eFourDirection.bottom:
                        return seedCoord + S_Bottom_Branch[elementIndx];

                    case eFourDirection.left:
                        return seedCoord + S_Left_Branch[elementIndx];

                    case eFourDirection.right:
                        return seedCoord + S_Right_Branch[elementIndx];
                }
                Debug.LogError("���x�����o�܂���");
                return Vector3Int.zero;


            // === Z ======================================
            case eMapSections.Z_Section:
                switch (direction)
                {
                    case eFourDirection.top:
                        return seedCoord + Z_Top_Branch[elementIndx];

                    case eFourDirection.bottom:
                        return seedCoord + Z_Bottom_Branch[elementIndx];

                    case eFourDirection.left:
                        return seedCoord + Z_Left_Branch[elementIndx];

                    case eFourDirection.right:
                        return seedCoord + Z_Right_Branch[elementIndx];
                }
                Debug.LogError("���x�����o�܂���");
                return Vector3Int.zero;
        }
        Debug.LogError("���x�����o�܂���");
        return Vector3Int.zero;
    }
    

    void Update()
    {
        
    }
}
