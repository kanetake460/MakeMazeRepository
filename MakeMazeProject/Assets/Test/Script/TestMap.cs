using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;

public class TestMap : MonoBehaviour
{
    public GridField gridField;
    public GridFieldMap map;

    [SerializeField] GameObject space;
    [SerializeField] GameObject wall;

    [Header("�O���b�h�ݒ�")]
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

    private void Start()
    {


        //map.SetWallGrid();
        //map.SetWallSurround();
        //map.InstanceMapObjects(space, wall);
    }

    void Update()
    {
        //gridField.DrowGrid();
    }
}