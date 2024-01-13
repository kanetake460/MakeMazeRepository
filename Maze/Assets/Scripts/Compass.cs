using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    [SerializeField] Image needleImage;
    public Quaternion needleRotation;
    public Vector3 rotation;
    [SerializeField] GameObject flag;
    [SerializeField] GameObject playerObj;


    void Start()
    {
        
    }

    /// <summary>
    /// ÉtÉâÉOÇÃèÍèäÇéwÇµé¶ÇµÇ‹Ç∑
    /// </summary>
    private void PointToFlag()
    {
        Vector3 pos = playerObj.transform.position - flag.transform.position;
        float dir = playerObj.transform.rotation.eulerAngles.y + (Mathf.Atan2(pos.z, pos.x) * Mathf.Rad2Deg) + 90f;
        
        needleImage.rectTransform.rotation = Quaternion.Euler(0, 0, dir);
    }


    void Update()
    {
        PointToFlag();
    }
}
