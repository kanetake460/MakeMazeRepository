using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;

public class TestPlayerController : MonoBehaviour
{
    [SerializeField] GameObject mainCam;

    [SerializeField] TestMap map;

    /*�p�����[�^*/
    [SerializeField] float locoSpeed = 0.1f;                    // �ړ��X�s�[�h
    [SerializeField] float viewSpeedX = 3f, viewSpeedY = 3f;    // ���_�X�s�[�h
    private FPS fps;

    private void Start()
    {
        fps = new FPS(map.map);
    }

    void Update()
    {
        // FPS���_�ݒ�
        FPS.CameraViewport(mainCam, viewSpeedX);
        FPS.PlayerViewport(gameObject, viewSpeedY);
        FPS.Locomotion(transform, locoSpeed);
        fps.CursorLock();
        fps.ClampMoveRange(transform);

    }
}
