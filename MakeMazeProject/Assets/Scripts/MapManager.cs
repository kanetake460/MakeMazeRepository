using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;

//====================================================================================================
// セクション：プレイヤーが配置するOITLJZSの形をしたオブジェクト
// エレメント：セクションを形作る一つ一つのオブジェクト
// シード：セクションの中で最も右下にあるエレメント
// ブランチ：シード以外のエレメント
//====================================================================================================

public class MapManager : MonoBehaviour
{
    [Header("ゲームオブジェクト")]
    [SerializeField] GameObject flagObj;        // 集める旗のプレハブ
    [SerializeField] GameObject hamburgerObj;   // ハンバーガーのプレハブ
    [SerializeField] GameObject enemy;          // エネミーのプレハブ
    [SerializeField] GameObject goalObj;        // ゴールのオブジェクト

    [Header("パラメーター")]
    [SerializeField, Min(0)] int hamburgerNum;  // 生成するハンバーガー部屋の数
    [SerializeField, Min(0)] int burgerRoomSize;// ハンバーガー部屋の大きさ
    [Space]
    [SerializeField, Min(0)] int flagNum;       // 生成する旗部屋の数
    [SerializeField, Min(0)] int flagRoomSize;  // 旗部屋の大きさ
    [Space]
    [SerializeField, Min(0)] int startRoomSize; // スタート部屋の大きさ
    [SerializeField] Coord startRoomCoord;      // スタート部屋の場所

    /// <summary>
    /// スタートポジション
    /// </summary>
    public Vector3 StartPos => gridField.grid[startRoomCoord.x, startRoomCoord.z] + Vector3.up * 3;


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
    // マップの縁にあるすべてのブロックのリスト
    public List<GridFieldMap.Block> borderBlockList = new List<GridFieldMap.Block>();

    // 部屋として生成された座標のリスト
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
    /// マップを初期化します
    /// </summary>
    public void InitMap()
    {
        // マップの設定
        map.CreateWallsAll();                   // すべてのブロックを壁にする
        map.InstanceMapObjects(blockHeight);    // マップオブジェクトを設定
        map.SetLayerMapObject(layerName);       // マップオブジェクトのレイヤーを設定
        map.ChangeWallTextureAll(texture);      // すべての壁のテクスチャを設定
        AddBorderList();                        // ボーダーリストをセット

        // スタート地点設定
        Instantiate(enemy, StartPos, Quaternion.identity);  // スタート地点のエネミーをインスタンス
        RoomGenerator(startRoomCoord, 1);                   // スタート部屋生成
        goalObj.transform.position = StartPos;              // ゴールオブジェクトをスタートポジションに設定

        // ハンバーガー部屋、旗部屋を生成
        GenerateRooms(hamburgerNum, burgerRoomSize, hamburgerObj);
        GenerateRooms(flagNum, flagRoomSize, flagObj, enemy);

        map.ActiveMapWallObject();      // 壁に設定されてるブロックをすべてアクティブ
    }


    /// <summary>
    /// マップの周りのセルをボーダーリストに登録します
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
    /// マップにハンバーガ部屋と、旗部屋をランダムな座標から生成します
    /// </summary>
    /// <param name="roomNum">ハンバーガー部屋のサイズ</param>
    /// <param name="roomSize">旗部屋のサイズ</param>
    /// <param name="obj">生成するオブジェクト</param>
    private void GenerateRooms(int roomNum, int roomSize, GameObject obj)
    {
        for (int i = 0; i < roomNum; i++)
        {
            Coord randCoord;

            int count = 0;
            // 部屋が生成できるランダムな地点を代入
            while (true)
            {
                count++;
                if (count == 1000)
                {
                    Debug.LogError("ハンバーガー部屋が生成できませんでした。");
                }
                randCoord = map.gridField.RandomGridCoord;
                if (CheckRoomGenerate(randCoord, roomSize))
                    break;
            }
            // ランダムな地点で部屋生成
            RoomGenerator(randCoord, roomSize);

            Instantiate(obj, map.gridField.grid[randCoord.x, randCoord.z], Quaternion.identity);

        }
    }


    /// <summary>
    /// マップにハンバーガ部屋と、旗部屋をランダムな座標から生成します
    /// </summary>
    /// <param name="roomNum">ハンバーガー部屋のサイズ</param>
    /// <param name="roomSize">旗部屋のサイズ</param>
    /// <param name="obj1">オブジェクト1</param>
    /// <paramref name="obj2">オブジェクト2</param>
    private void GenerateRooms(int roomNum, int roomSize, GameObject obj1, GameObject obj2)
    {
        for (int i = 0; i < roomNum; i++)
        {
            Coord randCoord;

            int count = 0;
            // 部屋が生成できるランダムな地点を代入
            while (true)
            {
                count++;
                if(count == 1000)
                {
                    Debug.LogError("旗部屋が生成できませんでした。");
                }
                randCoord = map.gridField.RandomGridCoord;
                if (CheckRoomGenerate(randCoord, roomSize))
                    break;
            }
            // ランダムな地点で部屋生成
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
            map.SetWallsDirection(element.x, element.z, false, false, false, false, true);
        }
    }

    /// <summary>
    /// 与えたセクションの形にシードの位置をあたえた座標からクローズしていきます
    /// </summary>
    /// <param name="seedCoord">開くセクションのシードの位置</param>
    /// <param name="sectionCoord">開きたいセクションの種類</param>
    public void CloseSection(Coord seedCoord, Coord[] sectionCoord)
    {
        foreach (Coord coord in sectionCoord)
        {
            Coord element = seedCoord + coord;
            map.SetWallsDirection(element.x, element.z, true, true, true, true, false);
        }
    }


    /// <summary>
    /// 与えたセクションが置けるかどうか確認します
    /// </summary>
    /// <param name="seedCoord">確認するセクションのシード座標</param>
    /// <param name="sectionCoord">セクション</param>
    /// <returns>置けるかどうか true：置ける</returns>
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
                // マップ上にないならfalse
                if (!map.CheckMap(confCoord)||
                    borderBlockList.Contains(map.blocks[x,z]))
                {
                    return false;
                }
                // すでに部屋が生成されているならfalse
                if (map.blocks[x, z].isSpace)
                {
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
                RoomCoordkList.Add(new Coord(x, z));
                map.SetWallsDirection(x, z, false, false, false, false, true);
                map.ChangePlaneColor(new Coord(x, z), Color.blue);

            }
        }
    }
}
