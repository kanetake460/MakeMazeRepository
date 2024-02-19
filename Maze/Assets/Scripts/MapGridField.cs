using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;

public class MapGridField : MonoBehaviour
{
    /*ÉOÉäÉbÉhê›íË*/
    public GridField gridField;
    [SerializeField] protected int gridWidth = 20;
    [SerializeField] protected int gridDepth = 10;
    [SerializeField] protected float cellWidth = 10;
    [SerializeField] protected float cellDepth = 10;
    [SerializeField] protected float y = 0;

    public GridFieldMap map;
    protected virtual void Start()
    {
        // gridField ÇÃèâä˙âª
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y, GridField.eGridAnchor.center);
        map = new GridFieldMap(gridField);
    }
}
