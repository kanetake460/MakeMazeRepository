
using System;
using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using UnityEditor;
using UnityEngine;
using UnityEngine.iOS;
using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour
{
    /*クラス参照*/
    [SerializeField] Map map;
    public GridField gridField;

    /*パラメータ*/
    [SerializeField] float speed = 0.1f;                            // 移動スピード
    [SerializeField] float Xsensityvity = 3f, Ysensityvity = 3f;    // 視点スピード

    /*オブジェクト*/
    [SerializeField] GameObject mainCam;            // プレイヤーの視点カメラ

    /*視点*/
    private bool cursorLock = true;                 // カーソルロックのON/OFF
    float minX = -90f, maxX = 90f;                  // 角度の制限

    void Start()
    {
        gridField = new GridField(20, 15, 10, 10, 0, GridField.eGridAnchor.bottomLeft);

    }



    void Update()
    {
        FPS.CameraViewport(mainCam, Xsensityvity, minX, maxX);
        FPS.PlayerViewport(gameObject, Ysensityvity);
        FPS.Locomotion(transform, speed);
        FPS.UpdateCursorLock(cursorLock);

        SpreadMap();
        gridField.DrowGrid();
    }

    /*=====プレイヤーのアクションによってマップを広げる関数=====*/
    private void SpreadMap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            map.InstanceMapBlock(transform.position, FPS.GetFourDirectionEulerAngles(transform.eulerAngles));
                //map.InstanceMapBlock(GridField.GetGridPosition(gridField,transform.position), FPS.InvestigateFourDirection(transform.eulerAngles));
        }
    }
}