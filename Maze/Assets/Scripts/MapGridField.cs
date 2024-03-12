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
    [SerializeField] GameObject flagObj;
    [SerializeField] GameObject hamburgerObj;

    /*���W*/
    private Vector3Int _startCoord;

    /*�p�����[�^*/
    [SerializeField, Min(0)] int hamburgerNum;
    [SerializeField, Min(0)] int flagNum;

    [SerializeField, Min(0)] int hamburgerRoomSize;
    [SerializeField, Min(0)] int flagRoomSize;
    [SerializeField, Min(0)] int startRoomSize;

    /*�O���b�h�ݒ�*/
    [SerializeField] protected int gridWidth = 20;
    [SerializeField] protected int gridDepth = 10;
    [SerializeField] protected float cellWidth = 10;
    [SerializeField] protected float cellDepth = 10;
    [SerializeField] protected int y = 0;

    /*�}�b�v*/
    public GridField gridField;
    public GridFieldMap map;

    /*�}�b�v���*/
    private List<Vector3Int> _roomBlockList = new List<Vector3Int>();

    private void Awake()
    {
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y, GridField.eGridAnchor.center);
        map = new GridFieldMap(gridField);
    }

    private void Start()
    {
        InitMap(_startCoord);


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

        _startCoord = map.gridField.middleGrid;

        map.SetWallAll();

        RoomGenerator(map.gridField.middleGrid, 3);
        GenerateRooms(hamburgerNum, hamburgerRoomSize, hamburgerObj);
        GenerateRooms(flagNum, flagRoomSize, flagObj);

        map.InstanceMapObjects();
        map.ActiveMapWallObject();
    }


    /// <summary>
    /// �}�b�v�Ƀn���o�[�K�����ƁA�������𐶐����܂�
    /// </summary>
    /// <param name="hamburgerRoomSize">�n���o�[�K�[�����̃T�C�Y</param>
    /// <param name="flagRoomSize">�������̃T�C�Y</param>
    private void GenerateRooms(int roomNum, int roomSize ,GameObject obj)
    {
        for (int i = 0; i < roomNum; i++)
        {
            Vector3Int randCoord = map.gridField.randomGridCoord;

            RoomGenerator(randCoord, roomSize);
            Instantiate(obj, map.gridField.grid[randCoord.x,randCoord.z],Quaternion.identity);
        }
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
    /// �^�����Z�N�V�����̌`�ɃV�[�h�̈ʒu�������������W����I�[�v�����Ă����܂�
    /// </summary>
    /// <param name="seedCoord">�J���Z�N�V�����̃V�[�h�̈ʒu</param>
    /// <param name="sectionCoord">�J�������Z�N�V�����̎��</param>
    public void CloseSection(Vector3Int seedCoord, Vector3Int[] sectionCoord)
    {
        foreach (Vector3Int coord in sectionCoord)
        {
            Vector3Int element = seedCoord + coord;
            map.blocks[element.x, element.z].isSpace = false;
        }
    }


    /// <summary>
    /// �^�����Z�N�V�������u���邩�ǂ����m�F���܂�
    /// </summary>
    /// <param name="sectionCoord">�Z�N�V����</param>
    /// <returns>�u���邩�ǂ��� true�F�u����</returns>
    public bool CheckAbleOpen(Vector3Int seedCoord, Vector3Int[] sectionCoord)
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


    /// <summary>
    /// �����������ł��邩�ǂ����m�F���܂�
    /// </summary>
    /// <param name="generateCoord">����������W</param>
    /// <param name="roomSize">�������镔���̃T�C�Y</param>
    /// <returns>�ł��邩�ǂ���</returns>
    private bool CheckRoomGenerate(Vector3Int generateCoord, int roomSize)
    {
        for (int x = generateCoord.x - roomSize; x <= generateCoord.x + roomSize; x++)
        {
            for (int z = generateCoord.z - roomSize; z <= generateCoord.z + roomSize; z++)
            {
                Vector3Int confCoord = new Vector3Int(x, map.gridField.y, z);
                if (!map.CheckMap(confCoord))
                {
                    Debug.Log("�����ł��܂���ł���");
                    return false;
                }
                if (map.blocks[x,z].isSpace)
                {
                    Debug.Log("�����ł��܂���ł���");
                    return false;
                }
            }
        }
        return true;
    }


    /// <summary>
    /// �����𐶐����܂�
    /// </summary>
    /// <param name="generateCoord">����������W</param>
    /// <param name="roomSize">�������镔���̃T�C�Y</param>
    public void RoomGenerator(Vector3Int generateCoord, int roomSize)
    {
        // �����ł��邩�m�F�ł��Ȃ������烊�^�[��
        if (!CheckRoomGenerate(generateCoord, roomSize))
            return;

        for (int x = generateCoord.x - roomSize; x <= generateCoord.x + roomSize; x++)
        {
            for (int z = generateCoord.z - roomSize; z <= generateCoord.z + roomSize; z++)
            {
                // ���[�����X�g�ɒǉ�
                _roomBlockList.Add(new Vector3Int(x, map.gridField.y, z));
                map.blocks[x, z].isSpace = true;
            }
        }
    }
}

