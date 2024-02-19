using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;

public class TestPlayerController : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    /*パラメータ*/
    [SerializeField] float locoSpeed = 0.1f;                    // 移動スピード
    [SerializeField] float viewSpeedX = 3f, viewSpeedY = 3f;    // 視点スピード

    // Update is called once per frame
    void Update()
    {
        // FPS視点設定
        FPS.CameraViewport(mainCam, viewSpeedX);
        FPS.PlayerViewport(gameObject, viewSpeedY);
        FPS.Locomotion(transform, locoSpeed);

    }
}
