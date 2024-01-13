using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    [SerializeField] Image needleImage;
    public Quaternion needleRotation = Quaternion.identity;
    public Vector3 rotation;


    void Start()
    {
        
    }

    private void PointToFlag()
    {
        needleRotation.eulerAngles = rotation;
        needleImage.rectTransform.rotation = needleRotation;

    }

    void Update()
    {
        PointToFlag();
    }
}
