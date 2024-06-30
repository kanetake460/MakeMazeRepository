using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace TakeshiLibrary
{
    public class CompassUI : MonoBehaviour
    {
        private void Start()
        {
            _targets = GameObject.FindGameObjectsWithTag(targetTag);
        }

        private void Update()
        {
            PointToTarget();
            MeasureDistance();
        }

        /*オブジェクト参照*/
        [SerializeField] Image needleImage;         // 針のイメージ
        [SerializeField] GameObject target;         // 針が指すもの
        [SerializeField] GameObject playerObj;      // コンパス原点

        /*パラメータ*/
        public string targetTag;          // ターゲットのタグ名
        public float distance;                      // ターゲットとの距離
        private GameObject[] _targets;



        public void SetTargetsWithTag(string targetTag)
        {
            _targets = GameObject.FindGameObjectsWithTag(targetTag);
        }


        /// <summary>
        /// 最も近くのフラグのゲームオブジェクトを返します。
        /// </summary>
        /// <returns>最も近くのフラグのゲームオブジェクト</returns>
        private GameObject FindNearestTarget()
        {
            // 与えたタグのついたオブジェクトをすべて格納
            // 最も近い距離
            float nearestDist = 1000000;
            // 最も近いターゲット
            GameObject nearestTarget = null;

            // すべてのフラグの距離をループ処理で比べる
            foreach (GameObject t in _targets)
            {
                if (t.activeSelf == false) continue;
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
            // 最も近いターゲット
            GameObject target = FindNearestTarget();
            // プレイヤーとターゲットの距離をひいたもの
            Vector3 pos;
            _ = target == null ? pos = transform.position : pos = playerObj.transform.position - target.transform.position;
            // ターゲットのy角度
            float dir = playerObj.transform.rotation.eulerAngles.y + (Mathf.Atan2(pos.z, pos.x) * Mathf.Rad2Deg) + 90f;

            needleImage.rectTransform.rotation = Quaternion.Euler(0, 0, dir);
        }


        /// <summary>
        /// 最も近いターゲットの距離を計測します
        /// </summary>
        private void MeasureDistance()
        {
            GameObject nearestFlag = FindNearestTarget();
            _ = nearestFlag == null ? distance = 0 : distance = Vector3.Distance(playerObj.transform.position, nearestFlag.transform.position);

        }

 

    }
}