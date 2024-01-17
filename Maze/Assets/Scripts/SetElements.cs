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
        Seed_Element,       // ��G�������g
        Branch_Element,     // �}�G�������g
        None_Element,       // �G�������g�Ȃ�
        Room_Element,       // �t���O�����镔���G�������g
        OutRange_Element,   // �͈͊O
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
        // �v���C���[�̑O�̍��W����G�������g��
        elements[seed.x, seed.z] = eElementType.Seed_Element;

        // �Z�N�V�����̂��̂ق��̃G�������g���}�G�������g��
        for (int i = 0; i < 3; i++)
        {
            elements[branch[i].x, branch[i].z] = eElementType.Branch_Element;
        }
        return elements;
    }


    /// <summary>
    /// �����ɑΉ�����ЂƂO�̃O���b�h���W��Ԃ��܂�
    /// </summary>
    /// <param name="eulerAngles">����</param>
    /// <returns>�����Ă�������̈�O�̃O���b�h���W</returns>
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
    /// ���ꂼ��̃Z�N�V�����̂��ꂼ��̌����̎}�G�������g��Ԃ��܂�
    /// </summary>
    /// <param name="section">�Z�N�V����</param>
    /// <param name="direction">����</param>
    /// <param name="seedCoord">�Z�N�V�����̃V�[�h�̃O���b�h���W</param>
    /// <param name="elementIndx">�v�f�̃C���f�b�N�X</param>
    /// <returns>�C���f�b�N�X�ɑΉ�����G�������g�̃O���b�h���W</returns>
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
                Debug.LogError("���x�����o�܂���");
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
                Debug.LogError("���x�����o�܂���");
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
                Debug.LogError("���x�����o�܂���");
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
                Debug.LogError("���x�����o�܂���");
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
                Debug.LogError("���x�����o�܂���");
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
                Debug.LogError("���x�����o�܂���");
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
                Debug.LogError("���x�����o�܂���");
                return Vector3Int.zero;
        }
        Debug.LogError("���x�����o�܂���");
        return Vector3Int.zero;
    }

}
