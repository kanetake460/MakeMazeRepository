using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;
using static TakeshiLibrary.GridFieldMap;

public class Test1 : MonoBehaviour
{
    public GridField gridField; 
    public GridFieldMap map;

    [Header("グリッド設定")]
    /*グリッド設定*/
    [SerializeField] int gridWidth = 20;
    [SerializeField] int gridDepth = 10;
    [SerializeField] float cellWidth = 10;
    [SerializeField] float cellDepth = 10;
    [SerializeField] int y = 0;
    private void Awake()
    {
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y, GridField.eGridAnchor.center);
        map = new GridFieldMap(gridField);
    }


    void Start()
    {
        map.CreateWallsGrid();
        map.CreateWallsSurround();
        map.InstanceMapObjects();
        map.ActiveMapWallObject();
        

    }

    // Update is called once per frame
    void Update()
    {
        gridField.DrowGrid();
    }
}
