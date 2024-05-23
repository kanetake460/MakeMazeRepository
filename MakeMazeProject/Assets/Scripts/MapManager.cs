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
    [SerializeField] GameObject flagObj;        // �W�߂���̃v���n�u
    [SerializeField] GameObject hamburgerObj;   // �n���o�[�K�[�̃v���n�u
    [SerializeField] GameObject enemy;          // �G�l�~�[�̃v���n�u
    [SerializeField] GameObject goalObj;        // �S�[���̃I�u�W�F�N�g

    [Header("�p�����[�^�[")]
    [SerializeField, Min(0)] int hamburgerNum;  // ��������n���o�[�K�[�����̐�
    [SerializeField, Min(0)] int burgerRoomSize;// �n���o�[�K�[�����̑傫��
    [Space]
    [SerializeField, Min(0)] int flagNum;       // ��������������̐�
    [SerializeField, Min(0)] int flagRoomSize;  // �������̑傫��
    [Space]
    [SerializeField, Min(0)] int startRoomSize; // �X�^�[�g�����̑傫��
    [SerializeField] Coord startRoomCoord;      // �X�^�[�g�����̏ꏊ

    /// <summary>
    /// �X�^�[�g�|�W�V����
    /// </summary>
    public Vector3 StartPos => gridField.grid[startRoomCoord.x, startRoomCoord.z] + Vector3.up * 3;


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
    // �}�b�v�̉��ɂ��邷�ׂẴu���b�N�̃��X�g
    public List<GridFieldMap.Block> borderBlockList = new List<GridFieldMap.Block>();

    // �����Ƃ��Đ������ꂽ���W�̃��X�g
    public List<Coord> RoomCoordkList { get; } = new List<Coord>();

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
    public void InitMap()
    {
        // �}�b�v�̐ݒ�
        map.CreateWallsAll();                   // ���ׂẴu���b�N��ǂɂ���
        map.InstanceMapObjects(blockHeight);    // �}�b�v�I�u�W�F�N�g��ݒ�
        map.SetLayerMapObject(layerName);       // �}�b�v�I�u�W�F�N�g�̃��C���[��ݒ�
        map.ChangeWallTextureAll(texture);      // ���ׂĂ̕ǂ̃e�N�X�`����ݒ�
        AddBorderList();                        // �{�[�_�[���X�g���Z�b�g

        // �X�^�[�g�n�_�ݒ�
        Instantiate(enemy, StartPos, Quaternion.identity);  // �X�^�[�g�n�_�̃G�l�~�[���C���X�^���X
        RoomGenerator(startRoomCoord, 1);                   // �X�^�[�g��������
        goalObj.transform.position = StartPos;              // �S�[���I�u�W�F�N�g���X�^�[�g�|�W�V�����ɐݒ�

        // �n���o�[�K�[�����A�������𐶐�
        GenerateRooms(hamburgerNum, burgerRoomSize, hamburgerObj);
        GenerateRooms(flagNum, flagRoomSize, flagObj, enemy);

        map.ActiveMapWallObject();      // �ǂɐݒ肳��Ă�u���b�N�����ׂăA�N�e�B�u
    }


    /// <summary>
    /// �}�b�v�̎���̃Z�����{�[�_�[���X�g�ɓo�^���܂�
    /// </summary>
    private void AddBorderList()
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
    /// �}�b�v�Ƀn���o�[�K�����ƁA�������������_���ȍ��W���琶�����܂�
    /// </summary>
    /// <param name="roomNum">�n���o�[�K�[�����̃T�C�Y</param>
    /// <param name="roomSize">�������̃T�C�Y</param>
    /// <param name="obj">��������I�u�W�F�N�g</param>
    private void GenerateRooms(int roomNum, int roomSize, GameObject obj)
    {
        for (int i = 0; i < roomNum; i++)
        {
            Coord randCoord;

            int count = 0;
            // �����������ł��郉���_���Ȓn�_����
            while (true)
            {
                count++;
                if (count == 1000)
                {
                    Debug.LogError("�n���o�[�K�[�����������ł��܂���ł����B");
                }
                randCoord = map.gridField.RandomGridCoord;
                if (CheckRoomGenerate(randCoord, roomSize))
                    break;
            }
            // �����_���Ȓn�_�ŕ�������
            RoomGenerator(randCoord, roomSize);

            Instantiate(obj, map.gridField.grid[randCoord.x, randCoord.z], Quaternion.identity);

        }
    }


    /// <summary>
    /// �}�b�v�Ƀn���o�[�K�����ƁA�������������_���ȍ��W���琶�����܂�
    /// </summary>
    /// <param name="roomNum">�n���o�[�K�[�����̃T�C�Y</param>
    /// <param name="roomSize">�������̃T�C�Y</param>
    /// <param name="obj1">�I�u�W�F�N�g1</param>
    /// <paramref name="obj2">�I�u�W�F�N�g2</param>
    private void GenerateRooms(int roomNum, int roomSize, GameObject obj1, GameObject obj2)
    {
        for (int i = 0; i < roomNum; i++)
        {
            Coord randCoord;

            int count = 0;
            // �����������ł��郉���_���Ȓn�_����
            while (true)
            {
                count++;
                if(count == 1000)
                {
                    Debug.LogError("�������������ł��܂���ł����B");
                }
                randCoord = map.gridField.RandomGridCoord;
                if (CheckRoomGenerate(randCoord, roomSize))
                    break;
            }
            // �����_���Ȓn�_�ŕ�������
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
            map.SetWallsDirection(element.x, element.z, false, false, false, false, true);
        }
    }

    /// <summary>
    /// �^�����Z�N�V�����̌`�ɃV�[�h�̈ʒu�������������W����N���[�Y���Ă����܂�
    /// </summary>
    /// <param name="seedCoord">�J���Z�N�V�����̃V�[�h�̈ʒu</param>
    /// <param name="sectionCoord">�J�������Z�N�V�����̎��</param>
    public void CloseSection(Coord seedCoord, Coord[] sectionCoord)
    {
        foreach (Coord coord in sectionCoord)
        {
            Coord element = seedCoord + coord;
            map.SetWallsDirection(element.x, element.z, true, true, true, true, false);
        }
    }


    /// <summary>
    /// �^�����Z�N�V�������u���邩�ǂ����m�F���܂�
    /// </summary>
    /// <param name="seedCoord">�m�F����Z�N�V�����̃V�[�h���W</param>
    /// <param name="sectionCoord">�Z�N�V����</param>
    /// <returns>�u���邩�ǂ��� true�F�u����</returns>
    public bool CheckAbleOpen(Coord seedCoord, Coord[] sectionCoord)
    {
        foreach (Coord coord in sectionCoord)
        {
            Coord element = seedCoord + coord;
            if(!map.CheckMap(element))
            {
                return false;
            }

            if (map.blocks[element.x, element.z].isSpace)
            {
                if(RoomCoordkList.Contains(element))
                {
                    continue;
                }
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
                // �}�b�v��ɂȂ��Ȃ�false
                if (!map.CheckMap(confCoord)||
                    borderBlockList.Contains(map.blocks[x,z]))
                {
                    return false;
                }
                // ���łɕ�������������Ă���Ȃ�false
                if (map.blocks[x, z].isSpace)
                {
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
                RoomCoordkList.Add(new Coord(x, z));
                map.SetWallsDirection(x, z, false, false, false, false, true);
                map.ChangePlaneColor(new Coord(x, z), Color.blue);

            }
        }
    }
}
