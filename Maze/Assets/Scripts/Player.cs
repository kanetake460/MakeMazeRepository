
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TakeshiClass;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.iOS;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    /*�N���X�Q��*/
    [SerializeField] Map map;

    /*�p�����[�^*/
    [SerializeField] float speed = 0.1f;                            // �ړ��X�s�[�h
    [SerializeField] float Xsensityvity = 3f, Ysensityvity = 3f;    // ���_�X�s�[�h
    private Vector3 latestPos;

    /*�I�u�W�F�N�g*/
    [SerializeField] GameObject mainCam;            // �v���C���[�̎��_�J����

    /*���_*/
    private bool cursorLock = true;                 // �J�[�\�����b�N��ON/OFF
    float minX = -90f, maxX = 90f;                  // �p�x�̐���

    Ray ray;

    void Start()
    {
        latestPos = transform.position;
    }



    void Update()
    {
        FPS.CameraViewport(mainCam, Xsensityvity, minX, maxX);
        FPS.PlayerViewport(gameObject, Ysensityvity);
        FPS.UpdateCursorLock(cursorLock);
        Vector3Int playerGridPos = map.gridField.GetGridCoordinate(transform.position);
        
        // �����A�v���C���[�̃|�W�V�������Z�N�V�����̏�łȂ����
        if (map.mapElements[playerGridPos.x, playerGridPos.z] != Elements.eElementType.Seed_Element &&
            map.mapElements[playerGridPos.x, playerGridPos.z] != Elements.eElementType.Branch_Element)
        {
            transform.position = latestPos;     // �|�W�V��������t���[���O�̈ʒu�ɌŒ�
        }
        latestPos = transform.position;         // �|�W�V�������i�[
        
        FPS.Locomotion(transform, speed);

        SpreadMap();
    }

    /*=====�v���C���[�̃A�N�V�����ɂ���ă}�b�v���L����֐�=====*/
    private void SpreadMap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            map.InstanceMapBlock(transform.position, FPS.GetFourDirectionEulerAngles(transform.eulerAngles));
                //map.InstanceMapBlock(GridField.GetGridPosition(gridField,transform.position), FPS.InvestigateFourDirection(transform.eulerAngles));
        }
    }
}