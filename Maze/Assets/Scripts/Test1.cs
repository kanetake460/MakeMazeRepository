using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    GridField gridField;
    // Start is called before the first frame update
    void Start()
    {
        gridField = gameObject.GetComponent<GridField>();
    }

    // Update is called once per frame
    void Update()
    {
        GridField.DrowGrid(gridField);
    }
}
