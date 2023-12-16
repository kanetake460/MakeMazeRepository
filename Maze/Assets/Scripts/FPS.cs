using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TakeshiClass
{
    /*=====FPS�̈ړ��֘A�̃X�N���v�g�ł�=====*/
    // �Q�l�T�C�g
    //https://www.popii33.com/unity-first-person-camera/
    // 
    public class FPS : MonoBehaviour
    {

        /// <summary>
        /// �J�����̊p�x���������܂�(�㉺)
        /// </summary>
        /// <param name="q">����������quoternion</param>
        /// <param name="minX">���̊p�x����</param>
        /// <param name="maxX">��̊p�x����</param>
        /// <returns></returns>
        public static Quaternion ClampRotation(Quaternion q, float minX, float maxX)
        {
            //q = x,y,z,w (x,y,z�̓x�N�g���i�ʂƌ����j�Fw�̓X�J���[�i���W�Ƃ͖��֌W�̗ʁj)

            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1f;

            float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

            angleX = Mathf.Clamp(angleX, minX, maxX);

            q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

            return q;
        }

        ///<summary>�J�����̎��_�ړ��֐�(�㉺�̎��_�ړ�)</summary>>
        ///<param name="camera"<pragma>�J�����̏��������ݒ�</pragma>
        ///<param name="Xsensityvity"<pragma>���_�ړ��X�s�[�h</pragma>
        ///<param name="minX"<pragma>���̊p�x����</pragma>
        ///<param name="maxX"<pragma>��̊p�x����</pragma>
        public static void CameraViewport(GameObject camera, float Xsensityvity, float minX, float maxX)
        {
            float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;       // �}�E�X�̍��W���
            camera.transform.localRotation *= Quaternion.Euler(-yRot, 0, 0);     // �p�x���

            //Update�̒��ō쐬�����֐����Ă�
            camera.transform.localRotation = ClampRotation(camera.transform.localRotation, minX, maxX);           // �p�x����

            //return cameraRot;
        }

        /// <summary>
        /// �v���C���[�̎��_�ړ��֐�(���E���_�ړ�)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="Xsensityvity"></param>
        /// <returns></returns>
        public static void PlayerViewport(GameObject player, float Ysensityvity)
        {
            float xRot = Input.GetAxis("Mouse X") * Ysensityvity;       // �}�E�X�̍��W���
            player.transform.localRotation *= Quaternion.Euler(0, xRot, 0);   // �p�x���

            //return characterRot;
        }

        /// <summary>
        /// �v���C���[���L�[���͂ɂ���Ĉړ������܂�
        /// </summary>
        /// <param name="player">�������v���C���[</param>
        /// <param name="speed">�ړ��X�s�[�h</param>
        public static void Locomotion(Transform player, float speed)
        {
            float x = 0;
            float z = 0;

            x = Input.GetAxisRaw("Horizontal") * speed;     // �ړ�����
            z = Input.GetAxisRaw("Vertical") * speed;       // �ړ�����

            //transform.position += new Vector3(x,0,z);

            player.position += player.forward * z * 0.01f + player.right * x * 0.01f;  // �ړ�

        }

        /// <summary>
        /// �J�[�\�������b�N���܂�
        /// </summary>
        /// <param name="cursorLock">�J�[�\�����b�N�t���O</param>
        public static void UpdateCursorLock(bool cursorLock)
        {
            if (Input.GetKeyDown(KeyCode.Escape))   // �G�X�P�[�v�L�[����������
            {
                cursorLock = false;
            }
            else if (Input.GetMouseButton(0))       // �E�N���b�N
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
        /// �v���C���[�̌�������l�����̌����𒲂ׂ܂�
        /// </summary>
        /// <param name="rot">�v���C���[�̌���(eulerAngles)</param>
        /// <returns>�l�����̊p�x��Ԃ��܂�</returns>
        public static Quaternion InvestigateFourDirection(Vector3 rot)
        {
            if (rot.y > 225f && rot.y <= 315)
            {
                return Quaternion.Euler(0, 270, 0);

            }
            else if (rot.y > 45f && rot.y <= 135f)
            {
                return Quaternion.Euler(0, 90, 0);
            }
            else if (rot.y > 135f && rot.y <= 225f)
            {
                return Quaternion.Euler(0, 180, 0);
            }
            else
            {
                return Quaternion.Euler(0, 0, 0);
            }
        }
    }
}