using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    [SerializeField] Image needleImage;
    [SerializeField] GameObject flag;
    [SerializeField] GameObject playerObj;

    public float distance;


    void Start()
    {
        
    }

    /// <summary>
    /// 最も近くのフラグのゲームオブジェクトを返します。
    /// </summary>
    /// <returns>最も近くのフラグのゲームオブジェクト</returns>
    private GameObject FindNearestFlag()
    {
        GameObject[] flag = GameObject.FindGameObjectsWithTag("flag");
        float nearestDist = 1000000;
        GameObject nearestFlag = null;

        // すべてのフラグの距離をループ処理で比べる
        foreach (GameObject t in flag)
        {
            // 距離計測
            float tDist = Vector3.Distance(playerObj.transform.position, t.transform.position);

            // 最も近いフラグを保存
            if (tDist < nearestDist)
            {
                nearestDist = tDist;
                nearestFlag = t;
            }
        }
        return nearestFlag;
    }

    /// <summary>
    /// フラグの場所を指し示します
    /// </summary>
    private void PointToFlag()
    {
        GameObject nearestFlag = FindNearestFlag();
        Vector3 pos = playerObj.transform.position - nearestFlag.transform.position;
        float dir = playerObj.transform.rotation.eulerAngles.y + (Mathf.Atan2(pos.z, pos.x) * Mathf.Rad2Deg) + 90f;
        
        needleImage.rectTransform.rotation = Quaternion.Euler(0, 0, dir);
    }

    private void MeasureDistance()
    {
        GameObject nearestFlag = FindNearestFlag();
        distance = Vector3.Distance(playerObj.transform.position, nearestFlag.transform.position);
    }

    void Update()
    {
        MeasureDistance();
        PointToFlag();
    }
}
