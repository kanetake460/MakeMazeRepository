using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using UnityEngine;

public class MapGridField : MonoBehaviour
{
    /*ÉOÉäÉbÉhê›íË*/
    protected GridField gridField;
    [SerializeField] protected int gridWidth = 20;
    [SerializeField] protected int gridDepth = 10;
    [SerializeField] protected float cellWidth = 10;
    [SerializeField] protected float cellDepth = 10;
    [SerializeField] protected float y = 0;
    protected virtual void Start()
    {
        // gridField ÇÃèâä˙âª
        gridField = new GridField(gridWidth, gridDepth, cellWidth, cellDepth, y, GridField.eGridAnchor.center);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
