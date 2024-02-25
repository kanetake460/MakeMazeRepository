using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TakeshiLibrary
{
    public class CompassUI : MonoBehaviour
    {
        /*�I�u�W�F�N�g�Q��*/
        [SerializeField] Image needleImage;         // �j�̃C���[�W
        [SerializeField] GameObject target;         // �j���w������
        [SerializeField] GameObject playerObj;      // �R���p�X���_

        /*�p�����[�^*/
        public string targetTag;          // �^�[�Q�b�g�̃^�O��
        public float distance;                      // �^�[�Q�b�g�Ƃ̋���


        /// <summary>
        /// �ł��߂��̃t���O�̃Q�[���I�u�W�F�N�g��Ԃ��܂��B
        /// </summary>
        /// <returns>�ł��߂��̃t���O�̃Q�[���I�u�W�F�N�g</returns>
        private GameObject FindNearestTarget(string targetTag)
        {
            // �^�����^�O�̂����I�u�W�F�N�g�����ׂĊi�[
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            // �ł��߂�����
            float nearestDist = 1000000;
            // �ł��߂��^�[�Q�b�g
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
            // �ł��߂��^�[�Q�b�g
            GameObject target = FindNearestTarget(targetTag);
            // �v���C���[�ƃ^�[�Q�b�g�̋������Ђ�������
            Vector3 pos;
            _ = target == null ? pos = transform.position : pos = playerObj.transform.position - target.transform.position;
            // �^�[�Q�b�g��y�p�x
            float dir = playerObj.transform.rotation.eulerAngles.y + (Mathf.Atan2(pos.z, pos.x) * Mathf.Rad2Deg) + 90f;

            needleImage.rectTransform.rotation = Quaternion.Euler(0, 0, dir);
        }


        /// <summary>
        /// �ł��߂��^�[�Q�b�g�̋������v�����܂�
        /// </summary>
        private void MeasureDistance()
        {
            GameObject nearestFlag = FindNearestTarget(targetTag);
            _ = nearestFlag == null ? distance = 0 : distance = Vector3.Distance(playerObj.transform.position, nearestFlag.transform.position);

        }

        void Update()
        {
            PointToTarget();
            MeasureDistance();
        }

    }
}