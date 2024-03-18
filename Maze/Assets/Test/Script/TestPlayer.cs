using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("�p�����[�^�[")]
    [SerializeField] float locoSpeed;                    // �ړ��X�s�[�h
    [SerializeField] float dashSpeed;
    [SerializeField] float viewSpeedX = 3f, viewSpeedY = 3f;    // ���_�X�s�[�h
    private Coord playerCoord;

    [Header("�Q��")]
    [SerializeField] GameObject mainCam;

    [Header("�R���|�[�l���g")]
    [SerializeField] Test1 map;
    private FPS fps;

    private void Awake()
    {
      

    }

    private void Start()
    {
        fps = new FPS(map.map);
    }


    void Update()
    {
        playerCoord = map.gridField.GetGridCoordinate(transform.position);


        // FPS���_�ݒ�
        FPS.CameraViewport(mainCam, viewSpeedX);
        FPS.PlayerViewport(gameObject, viewSpeedY);
        FPS.Locomotion(transform, locoSpeed, dashSpeed);
        fps.CursorLock();
        fps.ClampMoveRange(transform);

    }
}
