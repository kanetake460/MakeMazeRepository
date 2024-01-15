using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    [SerializeField] Image needleImage;         // 針のイメージ
    [SerializeField] GameObject target;         // 針が指すもの
    [SerializeField] GameObject playerObj;      // コンパス原点

    [SerializeField] string targetTag;          // ターゲットのタグ名
    public float distance;


    void Start()
    {
        
    }

    /// <summary>
    /// 最も近くのフラグのゲームオブジェクトを返します。
    /// </summary>
    /// <returns>最も近くのフラグのゲームオブジェクト</returns>
    private GameObject FindNearestTarget(string targetTag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float nearestDist = 1000000;
        GameObject nearestTarget = null;

        // すべてのフラグの距離をループ処理で比べる
        foreach (GameObject t in targets)
        {
            // 距離計測
            float tDist = Vector3.Distance(playerObj.transform.position, t.transform.position);

            // 最も近いフラグを保存
            if (tDist < nearestDist)
            {
                nearestDist = tDist;
                nearestTarget = t;
            }
        }
        return nearestTarget;
    }

    /// <summary>
    /// フラグの場所を指し示します
    /// </summary>
    private void PointToTarget()
    {
        GameObject target = FindNearestTarget(targetTag);
        Vector3 pos = playerObj.transform.position - target.transform.position;
        float dir = playerObj.transform.rotation.eulerAngles.y + (Mathf.Atan2(pos.z, pos.x) * Mathf.Rad2Deg) + 90f;
        
        needleImage.rectTransform.rotation = Quaternion.Euler(0, 0, dir);
    }

    private void MeasureDistance()
    {
        GameObject nearestFlag = FindNearestTarget(targetTag);
        distance = Vector3.Distance(playerObj.transform.position, nearestFlag.transform.position);
    }

    void Update()
    {
        MeasureDistance();
        PointToTarget();
    }
}
