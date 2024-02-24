using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace TakeshiLibrary
{
    /*=====FPSの移動関連のスクリプトです=====*/
    // 参考サイト
    //https://www.popii33.com/unity-first-person-camera/
    // 
    public class FPS
    {
        Rigidbody rb;

        public enum eFourDirection
        {
            top = 0,
            bottom = 180,
            left = 270,
            right = 90,
            No = 0,
        }


        /// <summary>
        /// カメラの角度制限をします(上下)
        /// </summary>
        /// <param name="q">制限したいquoternion</param>
        /// <param name="minX">下の角度制限</param>
        /// <param name="maxX">上の角度制限</param>
        /// <returns></returns>
        public static Quaternion ClampRotation(Quaternion q, float minX, float maxX)
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


        ///<summary>カメラの視点移動関数(上下の視点移動)</summary>>
        ///<param name="camera"<pragma>カメラの初期向き設定</pragma>
        ///<param name="Xsensityvity"<pragma>視点移動スピード</pragma>
        ///<param name="minX"<pragma>下の角度制限</pragma>
        ///<param name="maxX"<pragma>上の角度制限</pragma>
        public static void CameraViewport(GameObject camera, float Xsensityvity = 3f, float minX = -90f, float maxX = 90f)
        {
            float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;       // マウスの座標代入
            camera.transform.localRotation *= Quaternion.Euler(-yRot, 0, 0);     // 角度代入

            //Updateの中で作成した関数を呼ぶ
            camera.transform.localRotation = ClampRotation(camera.transform.localRotation, minX, maxX);           // 角度制限

            //return cameraRot;
        }


        /// <summary>
        /// プレイヤーの視点移動関数(左右視点移動)
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static void PlayerViewport(GameObject player, float Ysensityvity = 3f)
        {
            float xRot = Input.GetAxis("Mouse X") * Ysensityvity;               // マウスの座標代入
            player.transform.localRotation *= Quaternion.Euler(0, xRot, 0);     // 角度代入

            //return characterRot;
        }


        /// <summary>
        /// プレイヤーをキー入力によって移動させます
        /// </summary>
        /// <param name="player">動かすプレイヤー</param>
        /// <param name="speed">移動スピード</param>
        public static void Locomotion(Transform player, float speed = 10f)
        {
            float x = 0;
            float z = 0;


            x = Input.GetAxisRaw("Horizontal") * speed;     // 移動入力
            z = Input.GetAxisRaw("Vertical") * speed;       // 移動入力

            //transform.position += new Vector3(x,0,z);

            player.position += player.forward * z * Time.deltaTime + player.right * x * Time.deltaTime;  // 移動

        }


        /// <summary>
        /// カーソルをロックします
        /// </summary>
        /// <param name="cursorLock">カーソルロックフラグ</param>
        public static void UpdateCursorLock(bool cursorLock)
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




        /// <summary>
        /// プレイヤーの向きから四方向の列挙子を返します
        /// </summary>
        /// <param name="rot">プレイヤーの向き</param>
        /// <returns>向きの列挙子</returns>
        public static eFourDirection GetFourDirection(Vector3 rot)
        {
            if (rot.y > 225f && rot.y <= 315)
            {
                return eFourDirection.left;
            }
            else if (rot.y > 45f && rot.y <= 135f)
            {
                return eFourDirection.right;
            }
            else if (rot.y > 135f && rot.y <= 225f)
            {
                return eFourDirection.bottom;
            }
            else
            {
                return eFourDirection.top;
            }
        }


        /// <summary>
        /// プレイヤーの向きからVector3の四方向を返します
        /// </summary>
        /// <param name="rot">プレイヤーの向き</param>
        /// <returns>Vector3の向き</returns>
        public static Vector3 GetVector3FourDirection(Vector3 rot)
        {
            eFourDirection fourDirection = GetFourDirection(rot);

            if (fourDirection == eFourDirection.left)
            {
                return Vector3.left;
            }
            else if (fourDirection == eFourDirection.right)
            {
                return Vector3.right;
            }
            else if (fourDirection == eFourDirection.bottom)
            {
                return Vector3.back;
            }
            else
            {
                return Vector3.forward;
            }
        }


        /// <summary>
        /// 隣の向きを返します
        /// </summary>
        /// <param name="dir">調べたい向き</param>
        /// <param name="isAnti">時計回りか、反時計回りか</param>
        /// <returns>隣の向き</returns>
        public static void ClockwiseDirection(ref eFourDirection dir , bool isAnti = false)
        {
            if (isAnti == false)
            {
                switch (dir)
                {
                    case eFourDirection.top:
                        dir = eFourDirection.right;
                        return;

                    case eFourDirection.bottom:
                        dir = eFourDirection.left;
                        return;

                    case eFourDirection.left:
                        dir = eFourDirection.top;
                        return;

                    case eFourDirection.right:
                        dir = eFourDirection.bottom;
                        return;
                }
                dir = eFourDirection.No;
            }
            else
            {
                switch (dir)
                {
                    case eFourDirection.top:
                        dir = eFourDirection.left;
                        return;

                    case eFourDirection.bottom:
                        dir = eFourDirection.right;
                        return;

                    case eFourDirection.left:
                        dir = eFourDirection.bottom;
                        return;

                    case eFourDirection.right:
                        dir = eFourDirection.top;
                        return;
                }
                dir = eFourDirection.No;
            }
        }


        /// <summary>
        /// ランダムな4方向の列挙子を返します
        /// </summary>
        /// <returns>ランダムな４方向</returns>
        public static eFourDirection RandomFourDirection()
        {
            Vector3 rand = Vector3.zero;
               rand.y = Random.Range(0.0f,360f);

            return GetFourDirection(rand);
        }


        /// <summary>
        /// ある地点まで Vector3 の値まで動かします
        /// </summary>
        /// <param name="trafo">動かす物のトランスフォーム</param>
        /// <param name="point">目的地</param>
        /// <param name="speed">動かすスピード</param>
        /// <returns>ポイントに到達したらtrueを返します</returns>
        public bool MoveToPoint(Transform trafo, Vector3 point, float speed = 1)// refけす
        {
            Vector3 pos = trafo.position;

            pos.x += pos.x <= point.x ? speed * 0.01f : speed * -0.01f;
            pos.y += pos.y <= point.y ? speed * 0.01f : speed * -0.01f;
            pos.z += pos.z <= point.z ? speed * 0.01f : speed * -0.01f;

            if (pos.x <= point.x + speed * 0.1f && pos.x >= point.x + speed * -0.1f) pos.x = point.x;
            if (pos.y <= point.y + speed * 0.1f && pos.y >= point.y + speed * -0.1f) pos.y = point.y;
            if (pos.z <= point.z + speed * 0.1f && pos.z >= point.z + speed * -0.1f) pos.z = point.z;

            trafo.position = pos;

            return pos == point;
        }

        /// <summary>
        /// プレイヤーの向きから四方向の向きの Quaternion を返します
        /// </summary>
        /// <param name="rot">プレイヤーの向き(eulerAngles)</param>
        /// <returns>四方向の角度を返します</returns>
        public static Quaternion GetFourDirectionEulerAngles(Vector3 rot)
        {
            float direction = (int)GetFourDirection(rot);

                return Quaternion.Euler(0, direction, 0);
        }
    }
}