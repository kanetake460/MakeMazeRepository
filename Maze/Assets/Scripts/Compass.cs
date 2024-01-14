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
    /// �ł��߂��̃t���O�̃Q�[���I�u�W�F�N�g��Ԃ��܂��B
    /// </summary>
    /// <returns>�ł��߂��̃t���O�̃Q�[���I�u�W�F�N�g</returns>
    private GameObject FindNearestFlag()
    {
        GameObject[] flag = GameObject.FindGameObjectsWithTag("flag");
        float nearestDist = 1000000;
        GameObject nearestFlag = null;

        // ���ׂẴt���O�̋��������[�v�����Ŕ�ׂ�
        foreach (GameObject t in flag)
        {
            // �����v��
            float tDist = Vector3.Distance(playerObj.transform.position, t.transform.position);

            // �ł��߂��t���O��ۑ�
            if (tDist < nearestDist)
            {
                nearestDist = tDist;
                nearestFlag = t;
            }
        }
        return nearestFlag;
    }

    /// <summary>
    /// �t���O�̏ꏊ���w�������܂�
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
