
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
    [SerializeField] GameManager gameManager;

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
    bool canLocomotion;
    void Start()
    {
        latestPos = transform.position;
        canLocomotion = true;
    }

    void Update()
    {
        FPS.CameraViewport(mainCam, Xsensityvity, minX, maxX);
        FPS.PlayerViewport(gameObject, Ysensityvity);
        FPS.UpdateCursorLock(cursorLock);
        Vector3Int playerGridPos = map.gridField.GetGridCoordinate(transform.position);

        // �����A�v���C���[�̃|�W�V�������Z�N�V�����̏�łȂ����
        if (map.mapElements[playerGridPos.x, playerGridPos.z] == SetElements.eElementType.None_Element ||
            map.mapElements[playerGridPos.x, playerGridPos.z] == SetElements.eElementType.OutRange_Element)
        {
            transform.position = latestPos;     // �|�W�V��������t���[���O�̈ʒu�ɌŒ�
        }
        latestPos = transform.position;         // �|�W�V�������i�[

        if (canLocomotion) FPS.Locomotion(transform, speed);

        // �����A�n���o�[�K�[������Ȃ�
        //if (gameManager.hamburgerCount > 0)
        {
            SpreadMap();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "flag")
        {
            CheckInItem(other.gameObject,gameManager.flags);
        }
         if(other.gameObject.tag == "hamberger")
        {
            CheckInItem(other.gameObject,gameManager.hamburgerCount);
        }
    }

    /*=====�v���C���[�̃A�N�V�����ɂ���ă}�b�v���L����֐�=====*/
    private void SpreadMap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            map.InstanceMapBlock(transform.position, FPS.GetFourDirectionEulerAngles(transform.eulerAngles));
            gameManager.hamburgerCount--;
                //map.InstanceMapBlock(GridField.GetGridPosition(gridField,transform.position), FPS.InvestigateFourDirection(transform.eulerAngles));
        }
    }

    /// <summary>
    /// �}�E�X�̉E�N���b�N�ŃA�C�e����������܂�
    /// </summary>
    /// <param name="item">�������A�C�e��</param>
    /// <param name="param">�������A�C�e���̃p�����[�^</param>
    private void CheckInFlag(GameObject item)
    {
        if (Input.GetMouseButton(1))
        {
            item.SetActive(false);
        }
    }    /// <summary>
         /// �}�E�X�̉E�N���b�N�ŃA�C�e����������܂�
         /// </summary>
         /// <param name="item">�������A�C�e��</param>
         /// <param name="param">�������A�C�e���̃p�����[�^</param>
    private void CheckInHambureger(GameObject item)
    {
        if (Input.GetMouseButton(1))
        {
            item.SetActive(false);
        }
    }
}