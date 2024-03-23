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

public class MapManager : MonoBehaviour
{
    [Header("�Q�[���I�u�W�F�N�g")]
    [SerializeField] GameObject flagObj;
    [SerializeField] GameObject hamburgerObj;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject goalObj;

    [Header("�p�����[�^�[")]
    [SerializeField, Min(0)] int hamburgerNum;
    [SerializeField, Min(0)] int flagNum;

    [SerializeField, Min(0)] int hamburgerRoomSize;
    [SerializeField, Min(0)] int flagRoomSize;
    [SerializeField, Min(0)] int startRoomSize;
    [SerializeField] Coord startRoomCoord;
    public Vector3 startPos => gridField.grid[startRoomCoord.x, startRoomCoord.z];


    [Header("�}�b�v�ݒ�")]
    [SerializeField] protected int gridWidth = 20;
    [SerializeField] protected int gridDepth = 10;
    [SerializeField] protected float cellWidth = 10;
    [SerializeField] protected float cellDepth = 10;
    [SerializeField] protected int y = 0;
    [SerializeField] float blockHeight;
    [SerializeField] string layerName;
    [SerializeField] Texture texture;


    [Header("�R���|�[�l���g")]
    public GridField gridField;
    public GridFieldMap map;

    /*�}�b�v���*/
    public List<GridFieldMap.Block> borderBlockList = new List<GridFieldMap.Block>();
    public List<Coord> roomBlockList { get; } = new List<Coord>();

    private void Awake()
    {
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y, GridField.eGridAnchor.bottomLeft);
        map = new GridFieldMap(gridField);
    }

    private void Start()
    {
        InitMap();
    }

    private void Update()
    {
        gridField.DrowGrid();
    }


    /// <summary>
    /// �}�b�v�����������܂�
    /// </summary>
    /// <param name="startSeed">�X�^�[�g�n�_</param>
    public void InitMap()
    {

        map.SetWallAll();
        map.InstanceMapObjects(blockHeight);
        map.SetLayerMapObject(layerName);
        SetBorderList();


        Instantiate(enemy, gridField.middle, Quaternion.identity);
        RoomGenerator(startRoomCoord, 1);
        goalObj.transform.position = startPos;
        GenerateRooms(hamburgerNum, hamburgerRoomSize, hamburgerObj);
        GenerateRooms(flagNum, flagRoomSize, flagObj, enemy);

        map.SetWallTextureAll(texture);
        map.ActiveMapWallObject();
    }


    /// <summary>
    /// �}�b�v�̎���̃Z�����{�[�_�[���X�g�ɓo�^���܂�
    /// </summary>
    private void SetBorderList()
    {
        Coord coord = new Coord();
        for (int x = 0; x < gridField.gridWidth; x++)
        {
            for (int z = 0; z < gridField.gridDepth; z++)
            {
                if (x == 0 ||
                    z == 0 ||
                    x == gridField.gridWidth - 1 ||
                    z == gridField.gridDepth - 1)
                {
                    coord.x = x;
                    coord.z = z;
                    borderBlockList.Add(map.blocks[x,z]);
                }
            }
        }
    }


    /// <summary>
    /// �}�b�v�Ƀn���o�[�K�����ƁA�������𐶐����܂�
    /// </summary>
    /// <param name="roomNum">�n���o�[�K�[�����̃T�C�Y</param>
    /// <param name="roomSize">�������̃T�C�Y</param>
    private void GenerateRooms(int roomNum, int roomSize, GameObject obj)
    {
        for (int i = 0; i < roomNum; i++)
        {
            Coord randCoord;
            while (true)
            {
                randCoord = map.gridField.randomGridCoord;
                if (CheckRoomGenerate(randCoord, roomSize))
                    break;
            }
            RoomGenerator(randCoord, roomSize);

            Instantiate(obj, map.gridField.grid[randCoord.x, randCoord.z], Quaternion.identity);

        }
    }

    /// <summary>
    /// �}�b�v�Ƀn���o�[�K�����ƁA�������𐶐����܂�
    /// </summary>
    /// <param name="roomNum">�n���o�[�K�[�����̃T�C�Y</param>
    /// <param name="roomSize">�������̃T�C�Y</param>
    private void GenerateRooms(int roomNum, int roomSize, GameObject obj1, GameObject obj2)
    {
        for (int i = 0; i < roomNum; i++)
        {
            Coord randCoord;
            while (true)
            {
                randCoord = map.gridField.randomGridCoord;
                if (CheckRoomGenerate(randCoord, roomSize))
                    break;
            }
            RoomGenerator(randCoord, roomSize);

            Instantiate(obj1, map.gridField.grid[randCoord.x, randCoord.z], Quaternion.identity);
            Instantiate(obj2, map.gridField.grid[randCoord.x, randCoord.z], Quaternion.identity);

        }
    }


    /// <summary>
    /// �^�����Z�N�V�����̌`�ɃV�[�h�̈ʒu�������������W����I�[�v�����Ă����܂�
    /// </summary>
    /// <param name="seedCoord">�J���Z�N�V�����̃V�[�h�̈ʒu</param>
    /// <param name="sectionCoord">�J�������Z�N�V�����̎��</param>
    public void OpenSection(Coord seedCoord, Coord[] sectionCoord)
    {
        foreach (Coord coord in sectionCoord)
        {
            Coord element = seedCoord + coord;
            map.SetWalls(element.x, element.z, false, false, false, false, true);
        }
    }

    /// <summary>
    /// �^�����Z�N�V�����̌`�ɃV�[�h�̈ʒu�������������W����I�[�v�����Ă����܂�
    /// </summary>
    /// <param name="seedCoord">�J���Z�N�V�����̃V�[�h�̈ʒu</param>
    /// <param name="sectionCoord">�J�������Z�N�V�����̎��</param>
    public void CloseSection(Coord seedCoord, Coord[] sectionCoord)
    {
        foreach (Coord coord in sectionCoord)
        {
            Coord element = seedCoord + coord;
            map.SetWalls(element.x, element.z, true, true, true, true, false);
        }
    }


    /// <summary>
    /// �^�����Z�N�V�������u���邩�ǂ����m�F���܂�
    /// </summary>
    /// <param name="sectionCoord">�Z�N�V����</param>
    /// <returns>�u���邩�ǂ��� true�F�u����</returns>
    public bool CheckAbleOpen(Coord seedCoord, Coord[] sectionCoord)
    {
        foreach (Coord coord in sectionCoord)
        {
            Coord element = seedCoord + coord;
            if(!map.CheckMap(element))
            {
                Debug.Log("�}�b�v�O�ł�");
                return false;
            }

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
    private bool CheckRoomGenerate(Coord generateCoord, int roomSize)
    {
        for (int x = generateCoord.x - roomSize; x <= generateCoord.x + roomSize; x++)
        {
            for (int z = generateCoord.z - roomSize; z <= generateCoord.z + roomSize; z++)
            {
                Coord confCoord = new Coord(x, z);
                if (!map.CheckMap(confCoord)||
                    borderBlockList.Contains(map.blocks[x,z]))
                {
                    Debug.Log("�����ł��܂���ł���");
                    return false;
                }
                if (map.blocks[x, z].isSpace)
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
    public void RoomGenerator(Coord generateCoord, int roomSize)
    {
        // �����ł��邩�m�F�ł��Ȃ������烊�^�[��
        if (!CheckRoomGenerate(generateCoord, roomSize))
            return;

        for (int x = generateCoord.x - roomSize; x <= generateCoord.x + roomSize; x++)
        {
            for (int z = generateCoord.z - roomSize; z <= generateCoord.z + roomSize; z++)
            {
                // ���[�����X�g�ɒǉ�
                roomBlockList.Add(new Coord(x, z));
                map.SetWalls(x, z, false, false, false, false, true);
                map.SetPlaneColor(new Coord(x, z), Color.blue);

            }
        }
    }
}