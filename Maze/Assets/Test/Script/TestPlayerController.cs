using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;

public class TestPlayerController : MonoBehaviour
{
    [SerializeField] GameObject mainCam;

    [SerializeField] TestMap map;

    /*パラメータ*/
    [SerializeField] float locoSpeed = 0.1f;                    // 移動スピード
    [SerializeField] float viewSpeedX = 3f, viewSpeedY = 3f;    // 視点スピード
    private FPS fps;

    private void Start()
    {
        fps = new FPS(map.map);
    }

    void Update()
    {
        // FPS視点設定
        FPS.CameraViewport(mainCam, viewSpeedX);
        FPS.PlayerViewport(gameObject, viewSpeedY);
        FPS.Locomotion(transform, locoSpeed);
        fps.CursorLock();
        fps.ClampMoveRange(transform);

    }
}
