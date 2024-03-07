using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TakeshiLibrary;
using UnityEngine;

//====================================================================================================
// �Z�N�V�����F�v���C���[���z�u����OITLJZS�̌`�������I�u�W�F�N�g
// �G�������g�F�Z�N�V�������`�����̃I�u�W�F�N�g
// �V�[�h�F�Z�N�V�����̒��ōł��E���ɂ���G�������g
// �u�����`�F�V�[�h�ȊO�̃G�������g


//====================================================================================================

public class MapGridField : MonoBehaviour
{
    /*�Q�[���I�u�W�F�N�g*/

    /*�p�����[�^*/
    [SerializeField] Vector3Int pos;

    /*�O���b�h�ݒ�*/
    
    [SerializeField] protected int gridWidth = 20;
    [SerializeField] protected int gridDepth = 10;
    [SerializeField] protected float cellWidth = 10;
    [SerializeField] protected float cellDepth = 10;
    [SerializeField] protected float y = 0;

    /*�}�b�v*/
    public GridField gridField;
    public GridFieldMap map;

    private void Awake()
    {
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y, GridField.eGridAnchor.center);
        map = new GridFieldMap(gridField);
    }

    private void Start()
    {
        InitMap(pos);
        map.InstanceMapObjects();
    }

    private void Update()
    {
        gridField.DrowGrid();
        map.ActiveMapWallObject();
    }


    /// <summary>
    /// �}�b�v�����������܂�
    /// </summary>
    /// <param name="startSeed">�X�^�[�g�n�_</param>
    public void InitMap(Vector3Int startSeed)
    {
        map.SetWallAll();

        OpenSection(startSeed, SectionTable.T.Top);
    }


    /// <summary>
    /// �^�����Z�N�V�����̌`�ɃV�[�h�̈ʒu�������������W����I�[�v�����Ă����܂�
    /// </summary>
    /// <param name="seedCoord">�J���Z�N�V�����̃V�[�h�̈ʒu</param>
    /// <param name="sectionCoord">�J�������Z�N�V�����̎��</param>
    public void OpenSection(Vector3Int seedCoord,Vector3Int[] sectionCoord)
    {
        foreach (Vector3Int coord in sectionCoord)
        {
            Vector3Int element = seedCoord + coord;
            map.blocks[element.x, element.z].isSpace = true;
        }
    }


    /// <summary>
    /// �^�����Z�N�V�������u���邩�ǂ����m�F���܂�
    /// </summary>
    /// <param name="sectionCoord">�Z�N�V����</param>
    /// <returns>�u���邩�ǂ��� true�F�u����</returns>
    public bool CheckAbleOpen(Vector3Int seedCoord,Vector3Int[] sectionCoord)
    {
        foreach (Vector3Int coord in sectionCoord)
        {
            Vector3Int element = seedCoord + coord;
            if (map.blocks[element.x, element.z].isSpace)
            {
                return false;
            }
        }
        return true;
    }
}

