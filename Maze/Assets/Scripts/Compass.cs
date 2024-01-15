using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    [SerializeField] Image needleImage;         // �j�̃C���[�W
    [SerializeField] GameObject target;         // �j���w������
    [SerializeField] GameObject playerObj;      // �R���p�X���_

    [SerializeField] string targetTag;          // �^�[�Q�b�g�̃^�O��
    public float distance;


    void Start()
    {
        
    }

    /// <summary>
    /// �ł��߂��̃t���O�̃Q�[���I�u�W�F�N�g��Ԃ��܂��B
    /// </summary>
    /// <returns>�ł��߂��̃t���O�̃Q�[���I�u�W�F�N�g</returns>
    private GameObject FindNearestTarget(string targetTag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float nearestDist = 1000000;
        GameObject nearestTarget = null;

        // ���ׂẴt���O�̋��������[�v�����Ŕ�ׂ�
        foreach (GameObject t in targets)
        {
            // �����v��
            float tDist = Vector3.Distance(playerObj.transform.position, t.transform.position);

            // �ł��߂��t���O��ۑ�
            if (tDist < nearestDist)
            {
                nearestDist = tDist;
                nearestTarget = t;
            }
        }
        return nearestTarget;
    }

    /// <summary>
    /// �t���O�̏ꏊ���w�������܂�
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
