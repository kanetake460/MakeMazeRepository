using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    /*ゲームオブジェクト*/

    /*パラメータ*/
    [SerializeField] Vector3Int pos;

    /*グリッド設定*/
    [SerializeField] protected int gridWidth = 20;
    [SerializeField] protected int gridDepth = 10;
    [SerializeField] protected float cellWidth = 10;
    [SerializeField] protected float cellDepth = 10;
    [SerializeField] protected int y = 0;

    /*マップ*/
    public GridField gridField;
    public GridFieldMap map;

    /*マップ情報*/
    private List<Vector3Int> _roomBlockList = new List<Vector3Int>();

    private void Awake()
    {
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y, GridField.eGridAnchor.bottomLeft);
        map = new GridFieldMap(gridField);
    }

    private void Start()
    {
        InitMap(pos);
        map.InstanceMapObjects();
        map.ActiveMapWallObject();

    }

    private void Update()
    {
        gridField.DrowGrid();
    }


    /// <summary>
    /// マップを初期化します
    /// </summary>
    /// <param name="startSeed">スタート地点</param>
    public void InitMap(Vector3Int startSeed)
    {
        map.SetWallAll();

        for(int i = 0; i < gridWidth; i++) 
        {
            RoomGenerator(2);
        }
        OpenSection(startSeed, SectionTable.T.Top);

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
            map.blocks[element.x, element.z].isSpace = true;
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
            map.blocks[element.x, element.z].isSpace = false;
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


    public void RoomGenerator(int roomSize)
    {
        Vector3Int generateCoord = map.gridField.randomGridCoord;

        if (!CheckRoomGenerate(generateCoord, roomSize))
            return;

        for (int x = generateCoord.x - roomSize; x <= generateCoord.x + roomSize; x++)
        {
            for (int z = generateCoord.z - roomSize; z <= generateCoord.z + roomSize; z++)
            {
                _roomBlockList.Add(new Vector3Int(x, map.gridField.y, z));
                map.blocks[x, z].isSpace = true;
            }
        }
    }
}

