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

    /*ÉOÉäÉbÉhê›íË*/
    [SerializeField] int gridWidth = 20;
    [SerializeField] int gridDepth = 10;
    [SerializeField] float cellWidth = 10;
    [SerializeField] float cellDepth = 10;
    [SerializeField] float y = 0;

    private void Start()
    {
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y, GridField.eGridAnchor.center);
        map = new GridFieldMap(gridField);

        map.SetWallGrid();
        map.InstanceMapObjects(space, wall);
    }

    void Update()
    {

    }
}
