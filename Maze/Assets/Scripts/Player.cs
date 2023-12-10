
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*クラス参照*/
    [SerializeField] Map map;


    /*パラメータ*/
    [SerializeField] float speed = 0.1f;                            // 移動スピード
    [SerializeField] float Xsensityvity = 3f, Ysensityvity = 3f;    // 視点スピード

    /*オブジェクト*/
    [SerializeField] GameObject mainCam;            // プレイヤーの視点カメラ

    /*視点*/
    private float x, z;                             // 移動方向
    private bool cursorLock = true;                 // カーソルロックのON/OFF
    private Quaternion cameraRot, characterRot;     // カメラ、プレイヤーのベクトル
    float minX = -90f, maxX = 90f;                  // 角度の制限


    void Start()
    {
        cameraRot = mainCam.transform.localRotation;    // メインカメラの回転を代入
        characterRot = transform.localRotation;         // キャラクターの回転を代入
    }

    void Update()
    {
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;       // マウスの座標代入
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;       // マウスの座標代入

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);     // 角度代入
        characterRot *= Quaternion.Euler(0, xRot, 0);   // 角度代入

        //Updateの中で作成した関数を呼ぶ
        cameraRot = ClampRotation(cameraRot);           // 角度制限

        mainCam.transform.localRotation = cameraRot;    // メインカメラに回転の値を代入
        transform.localRotation = characterRot;         // プレイヤーに回転の値を代入

        UpdateCursorLock();                             // カーソルロック関数

        SpreadMap();
    }

    private void FixedUpdate()
    {
        x = 0;
        z = 0;

        x = Input.GetAxisRaw("Horizontal") * speed;     // 移動入力
        z = Input.GetAxisRaw("Vertical") * speed;       // 移動入力

        //transform.position += new Vector3(x,0,z);

       transform.position += transform.forward * z + transform.right * x;  // 移動
    }

    /*=====カーソルロックのON/OFF関数=====*/
    public void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   // エスケープキーを押したら
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))       // 右クリック
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

    /*=====角度制限関数=====*/
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,zはベクトル（量と向き）：wはスカラー（座標とは無関係の量）)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }

    /*=====プレイヤーのアクションによってマップを広げる関数=====*/
    private void SpreadMap()
    {
        if(Input.GetMouseButtonDown(0))
        {
            map.InstanceMapBlock(transform.position,transform.rotation) ;
        }
    }

}