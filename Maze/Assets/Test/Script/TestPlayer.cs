using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("パラメーター")]
    [SerializeField] float locoSpeed;                    // 移動スピード
    [SerializeField] float dashSpeed;
    [SerializeField] float viewSpeedX = 3f, viewSpeedY = 3f;    // 視点スピード
    private Coord playerCoord;

    [Header("参照")]
    [SerializeField] GameObject mainCam;

    [Header("コンポーネント")]
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


        // FPS視点設定
        FPS.CameraViewport(mainCam, viewSpeedX);
        FPS.PlayerViewport(gameObject, viewSpeedY);
        FPS.Locomotion(transform, locoSpeed, dashSpeed);
        fps.CursorLock();
        fps.ClampMoveRange(transform);

    }
}
