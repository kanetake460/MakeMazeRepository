using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;

//====================================================================================================
// セクション：プレイヤーが配置するOITLJZSの形をしたオブジェクト
// エレメント：セクションを形作る一つ一つのオブジェクト
// シード：セクションの中で最も右下にあるエレメント
// ブランチ：シード以外のエレメント


//====================================================================================================

public class MapGridField : MonoBehaviour
{
    [Header("ゲームオブジェクト")]
    [SerializeField] GameObject flagObj;
    [SerializeField] GameObject hamburgerObj;
    [SerializeField] GameObject enemy;

    [Header("パラメーター")]
    [SerializeField, Min(0)] int hamburgerNum;
    [SerializeField, Min(0)] int flagNum;

    [SerializeField, Min(0)] int hamburgerRoomSize;
    [SerializeField, Min(0)] int flagRoomSize;
    [SerializeField, Min(0)] int startRoomSize;

    [Header("マップ設定")]
    [SerializeField] protected int gridWidth = 20;
    [SerializeField] protected int gridDepth = 10;
    [SerializeField] protected float cellWidth = 10;
    [SerializeField] protected float cellDepth = 10;
    [SerializeField] protected int y = 0;
    [SerializeField] float blockHeight;
    [SerializeField] string layerName;
    [SerializeField] Texture texture;


    [Header("コンポーネント")]
    public GridField gridField;
    public GridFieldMap map;

    /*マップ情報*/
    public List<Coord> roomBlockList { get; } = new List<Coord>();

    private void Awake()
    {
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y, GridField.eGridAnchor.center);
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
    /// マップを初期化します
    /// </summary>
    /// <param name="startSeed">スタート地点</param>
    public void InitMap()
    {


        map.SetWallAll();
        map.InstanceMapObjects(blockHeight);
        map.SetLayerMapObject(layerName);

        Instantiate(enemy, gridField.middle, Quaternion.identity);
        RoomGenerator(map.gridField.middleGrid, 1);
        GenerateRooms(hamburgerNum, hamburgerRoomSize, hamburgerObj);
        GenerateRooms(flagNum, flagRoomSize, flagObj,enemy);

        map.SetWallTextureAll(texture);
        map.ActiveMapWallObject();
    }


    /// <summary>
    /// マップにハンバーガ部屋と、旗部屋を生成します
    /// </summary>
    /// <param name="roomNum">ハンバーガー部屋のサイズ</param>
    /// <param name="roomSize">旗部屋のサイズ</param>
    private void GenerateRooms(int roomNum, int roomSize ,GameObject obj)
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

            Instantiate(obj, map.gridField.grid[randCoord.x,randCoord.z],Quaternion.identity);

        }
    }

    /// <summary>
    /// マップにハンバーガ部屋と、旗部屋を生成します
    /// </summary>
    /// <param name="roomNum">ハンバーガー部屋のサイズ</param>
    /// <param name="roomSize">旗部屋のサイズ</param>
    private void GenerateRooms(int roomNum, int roomSize, GameObject obj1,GameObject obj2)
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
    /// 与えたセクションの形にシードの位置をあたえた座標からオープンしていきます
    /// </summary>
    /// <param name="seedCoord">開くセクションのシードの位置</param>
    /// <param name="sectionCoord">開きたいセクションの種類</param>
    public void OpenSection(Coord seedCoord, Coord[] sectionCoord)
    {
        foreach (Coord coord in sectionCoord)
        {
            Coord element = seedCoord + coord;
            map.SetWalls(element.x, element.z, false,false,false,false,true);
        }
    }

    /// <summary>
    /// 与えたセクションの形にシードの位置をあたえた座標からオープンしていきます
    /// </summary>
    /// <param name="seedCoord">開くセクションのシードの位置</param>
    /// <param name="sectionCoord">開きたいセクションの種類</param>
    public void CloseSection(Coord seedCoord, Coord[] sectionCoord)
    {
        foreach (Coord coord in sectionCoord)
        {
            Coord element = seedCoord + coord;
            map.SetWalls(element.x, element.z, true, true, true, true, false);
        }
    }


    /// <summary>
    /// 与えたセクションが置けるかどうか確認します
    /// </summary>
    /// <param name="sectionCoord">セクション</param>
    /// <returns>置けるかどうか true：置ける</returns>
    public bool CheckAbleOpen(Coord seedCoord, Coord[] sectionCoord)
    {
        foreach (Coord coord in sectionCoord)
        {
            Coord element = seedCoord + coord;
            if (map.blocks[element.x, element.z].isSpace)
            {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    /// 部屋が生成できるかどうか確認します
    /// </summary>
    /// <param name="generateCoord">生成する座標</param>
    /// <param name="roomSize">生成する部屋のサイズ</param>
    /// <returns>できるかどうか</returns>
    private bool CheckRoomGenerate(Coord generateCoord, int roomSize)
    {
        for (int x = generateCoord.x - roomSize; x <= generateCoord.x + roomSize; x++)
        {
            for (int z = generateCoord.z - roomSize; z <= generateCoord.z + roomSize; z++)
            {
                Coord confCoord = new Coord(x, z);
                if (!map.CheckMap(confCoord))
                {
                    Debug.Log("生成できませんでした");
                    return false;
                }
                if (map.blocks[x,z].isSpace)
                {
                    Debug.Log("生成できませんでした");
                    return false;
                }
            }
        }
        return true;
    }


    /// <summary>
    /// 部屋を生成します
    /// </summary>
    /// <param name="generateCoord">生成する座標</param>
    /// <param name="roomSize">生成する部屋のサイズ</param>
    public void RoomGenerator(Coord generateCoord, int roomSize)
    {
        // 生成できるか確認できなかったらリターン
        if (!CheckRoomGenerate(generateCoord, roomSize))
            return;

        for (int x = generateCoord.x - roomSize; x <= generateCoord.x + roomSize; x++)
        {
            for (int z = generateCoord.z - roomSize; z <= generateCoord.z + roomSize; z++)
            {
                // ルームリストに追加
                roomBlockList.Add(new Coord(x, z));
                map.SetWalls(x,z,false,false,false,false,true);
                map.SetPlaneColor(new Coord(x, z), Color.blue);

            }
        }
    }
}

