
using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using UnityEditor;
using UnityEngine;
using UnityEngine.iOS;

public class Player : MonoBehaviour
{
    /*�N���X�Q��*/
    [SerializeField] Map map;
    [SerializeField] GridField gridField;

    /*�p�����[�^*/
    [SerializeField] float speed = 0.1f;                            // �ړ��X�s�[�h
    [SerializeField] float Xsensityvity = 3f, Ysensityvity = 3f;    // ���_�X�s�[�h

    /*�I�u�W�F�N�g*/
    [SerializeField] GameObject mainCam;            // �v���C���[�̎��_�J����

    /*���_*/
    private bool cursorLock = true;                 // �J�[�\�����b�N��ON/OFF
    float minX = -90f, maxX = 90f;                  // �p�x�̐���

    void Start()
    {
        gridField = new GridField(20, 15, 10, 10, 0, GridField.eGridAnchor.bottomLeft);
        a[0] = 10;
        a[1] = 0;
    }

    int[] a = new int[2];




    void Update()
    {
        FPS.CameraViewport(mainCam, Xsensityvity, minX, maxX);
        FPS.PlayerViewport(gameObject, Ysensityvity);
        FPS.Locomotion(transform, speed);
        FPS.UpdateCursorLock(cursorLock);


        Algorithm.Swap(a[0], a[1]);
        Debug.Log(a[0]);

        SpreadMap();
        GridField.DrowGrid(gridField);
    }

    /*=====�v���C���[�̃A�N�V�����ɂ���ă}�b�v���L����֐�=====*/
    private void SpreadMap()
    {
        if (Input.GetMouseButtonDown(0))
        {
                map.InstanceMapBlock(GridField.GetGridPosition(gridField,transform.position), FPS.InvestigateFourDirection(transform.eulerAngles));
        }
    }
}