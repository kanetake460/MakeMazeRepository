
using System.Collections;
using System.Collections.Generic;
using TakeshiClass;
using UnityEditor;
using UnityEngine;
using UnityEngine.iOS;

public class Player : MonoBehaviour
{
    /*クラス参照*/
    [SerializeField] Map map;
    [SerializeField] GridField gridField;

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

    /*=====プレイヤーのアクションによってマップを広げる関数=====*/
    private void SpreadMap()
    {
        if (Input.GetMouseButtonDown(0))
        {
                map.InstanceMapBlock(GridField.GetGridPosition(gridField,transform.position), FPS.InvestigateFourDirection(transform.eulerAngles));
        }
    }
}