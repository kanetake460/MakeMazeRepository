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


    [Header("コンポーネント")]
    public GridField gridField;
    public GridFieldMap map;

    /*マップ情報*/
    public List<Vector3Int> roomBlockList { get; } = new List<Vector3Int>();

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

        RoomGenerator(map.gridField.middleGrid, 3);
        GenerateRooms(hamburgerNum, hamburgerRoomSize, hamburgerObj);
        GenerateRooms(flagNum, flagRoomSize, flagObj);

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
            Vector3Int randCoord;
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
    /// 与えたセクションの形にシードの位置をあたえた座標からオープンしていきます
    /// </summary>
    /// <param name="seedCoord">開くセクションのシードの位置</param>
    /// <param name="sectionCoord">開きたいセクションの種類</param>
    public void OpenSection(Vector3Int seedCoord,Vector3Int[] sectionCoord)
    {
        foreach (Vector3Int coord in sectionCoord)
        {
            Vector3Int element = seedCoord + coord;
            map.SetWalls(element.x, element.z, false,false,false,false,true);
        }
    }

    /// <summary>
    /// 与えたセクションの形にシードの位置をあたえた座標からオープンしていきます
    /// </summary>
    /// <param name="seedCoord">開くセクションのシードの位置</param>
    /// <param name="sectionCoord">開きたいセクションの種類</param>
    public void CloseSection(Vector3Int seedCoord, Vector3Int[] sectionCoord)
    {
        foreach (Vector3Int coord in sectionCoord)
        {
            Vector3Int element = seedCoord + coord;
            map.SetWalls(element.x, element.z, true, true, true, true, false);
        }
    }


    /// <summary>
    /// 与えたセクションが置けるかどうか確認します
    /// </summary>
    /// <param name="sectionCoord">セクション</param>
    /// <returns>置けるかどうか true：置ける</returns>
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
    /// 部屋が生成できるかどうか確認します
    /// </summary>
    /// <param name="generateCoord">生成する座標</param>
    /// <param name="roomSize">生成する部屋のサイズ</param>
    /// <returns>できるかどうか</returns>
    private bool CheckRoomGenerate(Vector3Int generateCoord, int roomSize)
    {
        for (int x = generateCoord.x - roomSize; x <= generateCoord.x + roomSize; x++)
        {
            for (int z = generateCoord.z - roomSize; z <= generateCoord.z + roomSize; z++)
            {
                Vector3Int confCoord = new Vector3Int(x, map.gridField.y, z);
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
    public void RoomGenerator(Vector3Int generateCoord, int roomSize)
    {
        // 生成できるか確認できなかったらリターン
        if (!CheckRoomGenerate(generateCoord, roomSize))
            return;

        for (int x = generateCoord.x - roomSize; x <= generateCoord.x + roomSize; x++)
        {
            for (int z = generateCoord.z - roomSize; z <= generateCoord.z + roomSize; z++)
            {
                // ルームリストに追加
                roomBlockList.Add(new Vector3Int(x, map.gridField.y, z));
                map.SetWalls(x,z,false,false,false,false,true);
                map.SetPlaneColor(new Vector3Int(x, gridField.y, z), Color.blue);

            }
        }
    }
}

