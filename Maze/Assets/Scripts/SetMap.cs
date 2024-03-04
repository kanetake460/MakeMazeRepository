using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;

// プレイヤーがおいたブロックのマスをマップに設定します
public class SetMap : MonoBehaviour
{
    private GridFieldMap map;

    private void Awake()
    {
        map = GetComponent<GridFieldMap>();
    }

    private void Start()
    {
        map = map;
    }

    public void InitMap(Vector3Int startCoord)
    {
        map.SetWallAll();
        map.blocks[startCoord.x, startCoord.z].isSpace = true;

        for (int i = 0; i < 3; i++)
        {
            Debug.Log(startCoord + Section.O_Top_Branch[i]);
        }
    }

    private void Update()
    {
        InitMap(Vector3Int.zero);
    }
}