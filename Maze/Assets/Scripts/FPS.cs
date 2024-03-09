using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace TakeshiLibrary
{
    /*=====FPS�̈ړ��֘A�̃X�N���v�g�ł�=====*/
    // �Q�l�T�C�g
    //https://www.popii33.com/unity-first-person-camera/
    // 
    public class FPS
    {
        private GridFieldMap _map;
        private Vector3 _latePos;
        private bool _cursorLock;

        public FPS(GridFieldMap map)
        {
            _map = map;
        }

        public enum eFourDirection
        {
            top = 0,
            bottom = 180,
            left = 270,
            right = 90,
            No = 0,
        }


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
        public static void CameraViewport(GameObject camera, float Xsensityvity = 3f, float minX = -90f, float maxX = 90f)
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
        /// <returns></returns>
        public static void PlayerViewport(GameObject player, float Ysensityvity = 3f)
        {
            float xRot = Input.GetAxis("Mouse X") * Ysensityvity;               // �}�E�X�̍��W���
            player.transform.localRotation *= Quaternion.Euler(0, xRot, 0);     // �p�x���

            //return characterRot;
        }


        /// <summary>
        /// �v���C���[���L�[���͂ɂ���Ĉړ������܂�
        /// </summary>
        /// <param name="player">�������v���C���[</param>
        /// <param name="speed">�ړ��X�s�[�h</param>
        public static void Locomotion(Transform player, float speed = 10f,float dashSpeed = 15,KeyCode dashuKey = KeyCode.LeftShift)
        {
            float x = 0;
            float z = 0;

            if (Input.GetKey(dashuKey)) speed = dashSpeed;

            x = Input.GetAxisRaw("Horizontal") * speed;     // �ړ�����
            z = Input.GetAxisRaw("Vertical") * speed;       // �ړ�����

            //transform.position += new Vector3(x,0,z);

            player.position += player.forward * z * Time.deltaTime + player.right * x * Time.deltaTime;  // �ړ�

        }


        /// <summary>
        /// �J�[�\�������b�N���܂�
        /// </summary>
        /// <param name="cursorLock">�J�[�\�����b�N�t���O</param>
        public void CursorLock()
        {
            if (Input.GetKeyDown(KeyCode.Escape))   // �G�X�P�[�v�L�[����������
            {
                _cursorLock = false;
            }
            else if (Input.GetMouseButton(0))       // ���N���b�N
            {
                _cursorLock = true;
            }
            if (_cursorLock)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (_cursorLock == false)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }


        /// <summary>
        /// �^�����g�����X�t�H�[�����ǃu���b�N�ɓ���Ȃ��悤�ɂ��܂�
        /// </summary>
        /// <param name="trafo"></param>
        public void ClampMoveRange(Transform trafo)
        {
            Vector3Int coord = _map.gridField.GetGridCoordinate(trafo.position);

            if (_map.blocks[coord.x,coord.z].isSpace == false)
            {
                trafo.position = _latePos;
            }
            _latePos = trafo.position;
        }


        /// <summary>
        /// �v���C���[�̌�������l�����̗񋓎q��Ԃ��܂�
        /// </summary>
        /// <param name="rot">�v���C���[�̌���</param>
        /// <returns>�����̗񋓎q</returns>
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
        /// �v���C���[�̌�������Vector3�̎l������Ԃ��܂�
        /// </summary>
        /// <param name="rot">�v���C���[�̌���</param>
        /// <returns>Vector3�̌���</returns>
        public static Vector3Int GetVector3FourDirection(Vector3 rot)
        {
            eFourDirection fourDirection = GetFourDirection(rot);

            if (fourDirection == eFourDirection.left)
            {
                return Vector3Int.left;
            }
            else if (fourDirection == eFourDirection.right)
            {
                return Vector3Int.right;
            }
            else if (fourDirection == eFourDirection.bottom)
            {
                return Vector3Int.back;
            }
            else
            {
                return Vector3Int.forward;
            }
        }

        public static Stack<Vector3Int> RandomVector3DirectionStack()
        {
            Stack<Vector3Int> dirStack = new Stack<Vector3Int>();

            dirStack.Push(Vector3Int.left);
            dirStack.Push(Vector3Int.right);
            dirStack.Push(Vector3Int.forward);
            dirStack.Push(Vector3Int.back);

            Algorithm.ShuffleStack(ref dirStack);
            Debug.Log(dirStack.Pop());
            Debug.Log(dirStack.Pop());
            Debug.Log(dirStack.Pop());
            Debug.Log(dirStack.Pop());
            return dirStack;
        }

        public static Vector3Int GetRandomVector3FourDirection()
        {
            int rand = Random.RandomRange(0, 4);

            if (rand == 0)
                return Vector3Int.left;
            else if (rand == 1)
                return Vector3Int.right;
            else if (rand == 2)
                return Vector3Int.forward;
            else
                return Vector3Int.back;
        }


        /// <summary>
        /// �ׂ̌�����Ԃ��܂�
        /// </summary>
        /// <param name="dir">���ׂ�������</param>
        /// <param name="isAnti">���v��肩�A�����v��肩</param>
        /// <returns>�ׂ̌���</returns>
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
        /// �����_����4�����̗񋓎q��Ԃ��܂�
        /// </summary>
        /// <returns>�����_���ȂS����</returns>
        public static eFourDirection RandomFourDirection()
        {
            Vector3 rand = Vector3.zero;
               rand.y = Random.Range(0.0f,360f);

            return GetFourDirection(rand);
        }


        /// <summary>
        /// ����n�_�܂� Vector3 �̒l�܂œ������܂�
        /// </summary>
        /// <param name="trafo">���������̃g�����X�t�H�[��</param>
        /// <param name="point">�ړI�n</param>
        /// <param name="speed">�������X�s�[�h</param>
        /// <returns>�|�C���g�ɓ��B������true��Ԃ��܂�</returns>
        public bool MoveToPoint(Transform trafo, Vector3 point, float speed = 1)// ref����
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
        /// �v���C���[�̌�������l�����̌����� Quaternion ��Ԃ��܂�
        /// </summary>
        /// <param name="rot">�v���C���[�̌���(eulerAngles)</param>
        /// <returns>�l�����̊p�x��Ԃ��܂�</returns>
        public static Quaternion GetFourDirectionEulerAngles(Vector3 rot)
        {
            float direction = (int)GetFourDirection(rot);

                return Quaternion.Euler(0, direction, 0);
        }
    }
}