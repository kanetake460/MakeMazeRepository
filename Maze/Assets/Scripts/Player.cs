
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*�N���X�Q��*/
    [SerializeField] Map map;


    /*�p�����[�^*/
    [SerializeField] float speed = 0.1f;                            // �ړ��X�s�[�h
    [SerializeField] float Xsensityvity = 3f, Ysensityvity = 3f;    // ���_�X�s�[�h

    /*�I�u�W�F�N�g*/
    [SerializeField] GameObject mainCam;            // �v���C���[�̎��_�J����

    /*���_*/
    private float x, z;                             // �ړ�����
    private bool cursorLock = true;                 // �J�[�\�����b�N��ON/OFF
    private Quaternion cameraRot, characterRot;     // �J�����A�v���C���[�̃x�N�g��
    float minX = -90f, maxX = 90f;                  // �p�x�̐���


    void Start()
    {
        cameraRot = mainCam.transform.localRotation;    // ���C���J�����̉�]����
        characterRot = transform.localRotation;         // �L�����N�^�[�̉�]����
    }

    void Update()
    {
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;       // �}�E�X�̍��W���
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;       // �}�E�X�̍��W���

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);     // �p�x���
        characterRot *= Quaternion.Euler(0, xRot, 0);   // �p�x���

        //Update�̒��ō쐬�����֐����Ă�
        cameraRot = ClampRotation(cameraRot);           // �p�x����

        mainCam.transform.localRotation = cameraRot;    // ���C���J�����ɉ�]�̒l����
        transform.localRotation = characterRot;         // �v���C���[�ɉ�]�̒l����

        UpdateCursorLock();                             // �J�[�\�����b�N�֐�

        SpreadMap();
    }

    private void FixedUpdate()
    {
        x = 0;
        z = 0;

        x = Input.GetAxisRaw("Horizontal") * speed;     // �ړ�����
        z = Input.GetAxisRaw("Vertical") * speed;       // �ړ�����

        //transform.position += new Vector3(x,0,z);

       transform.position += transform.forward * z + transform.right * x;  // �ړ�
    }

    /*=====�J�[�\�����b�N��ON/OFF�֐�=====*/
    public void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   // �G�X�P�[�v�L�[����������
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))       // �E�N���b�N
        {
            cursorLock = true;
        }

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /*=====�p�x�����֐�=====*/
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,z�̓x�N�g���i�ʂƌ����j�Fw�̓X�J���[�i���W�Ƃ͖��֌W�̗ʁj)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }

    /*=====�v���C���[�̃A�N�V�����ɂ���ă}�b�v���L����֐�=====*/
    private void SpreadMap()
    {
        if(Input.GetMouseButtonDown(0))
        {
            map.InstanceMapBlock(transform.position,transform.rotation) ;
        }
    }

}