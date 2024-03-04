using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject space;
    [SerializeField] GameObject wall;

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
        map.InstanceMapObjects(space, wall);
    }

    private void Update()
    {
        gridField.DrowGrid();   
    }


    /// <summary>
    /// �}�b�v�����������܂�
    /// </summary>
    /// <param name="startSeed">�X�^�[�g�n�_</param>
    public void InitMap(Vector3Int startSeed)
    {
        map.SetWallAll();
        //map.SetWallGrid();

        OpenSection(startSeed, Section.O_Bottom_Branch);
    }


    /// <summary>
    /// �^�����Z�N�V�����̌`�ɃV�[�h�̈ʒu�������������W����I�[�v�����Ă����܂�
    /// </summary>
    /// <param name="seedCoord">�J���Z�N�V�����̃V�[�h�̈ʒu</param>
    /// <param name="sectionCoord">�J�������Z�N�V�����̎��</param>
    public void OpenSection(Vector3Int seedCoord,Vector3Int[] sectionCoord)
    {
        foreach(Vector3Int coord in sectionCoord)
        {
            Vector3Int element = seedCoord + coord;
            map.blocks[element.x,element.z].isSpace = true;
        }
    }
}

