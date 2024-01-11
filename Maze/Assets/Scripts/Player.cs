
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
    /*クラス参照*/
    [SerializeField] Map map;

    /*パラメータ*/
    [SerializeField] float speed = 0.1f;                            // 移動スピード
    [SerializeField] float Xsensityvity = 3f, Ysensityvity = 3f;    // 視点スピード
    private Vector3 latestPos;

    /*オブジェクト*/
    [SerializeField] GameObject mainCam;            // プレイヤーの視点カメラ

    /*視点*/
    private bool cursorLock = true;                 // カーソルロックのON/OFF
    float minX = -90f, maxX = 90f;                  // 角度の制限

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
        
        // もし、プレイヤーのポジションがセクションの上でなければ
        if (map.mapElements[playerGridPos.x, playerGridPos.z] != Elements.eElementType.Seed_Element &&
            map.mapElements[playerGridPos.x, playerGridPos.z] != Elements.eElementType.Branch_Element)
        {
            transform.position = latestPos;     // ポジションを一フレーム前の位置に固定
        }
        latestPos = transform.position;         // ポジションを格納
        
        FPS.Locomotion(transform, speed);

        SpreadMap();
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