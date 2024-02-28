
using TakeshiLibrary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    /*�N���X�Q��*/
    [SerializeField] MakeMap map;                   // �}�b�v
    [SerializeField] GameManager gameManager;   // �Q�[���}�l�[�W���[
    [SerializeField] UIManager uiManager;
    FPS fps;

    /*�p�����[�^*/
    [SerializeField] float speed;                            // �ړ��X�s�[�h
    [SerializeField] float dashSpeed;
    [SerializeField] float Xsensityvity = 3f, Ysensityvity = 3f;    // ���_�X�s�[�h
    private Vector3 _latestPos;

    /*�I�u�W�F�N�g*/
    [SerializeField] GameObject mainCam;            // �v���C���[�̎��_�J����

    /*���_*/
    float minX = -90f, maxX = 90f;                  // �p�x�̐���

    void Start()
    {
        fps = new FPS(map.map);
    }

    void Update()
    {
        // FPS���_�ݒ�
        FPS.CameraViewport(mainCam, Xsensityvity, minX, maxX);
        FPS.PlayerViewport(gameObject, Ysensityvity);

        // �v���C���[�̈ړ�
        FPS.Locomotion(transform, speed, dashSpeed);
        ClampLocomotionRange();

        // �����A�n���o�[�K�[������Ȃ�
        if (gameManager.hamburgerCount > 0)
        {
            SpreadMap();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // flag�ɓ������Ă�����
        if (other.gameObject.tag == "flag")
        {
            uiManager.EnterDisplayGameMessage("�E�N���b�N�I",Color.black,10);
            CheckInFlag(other.gameObject);
        }
        // �n���o�[�K�[�ɓ������Ă�����
        if (other.gameObject.tag == "hamburger")
        {
            uiManager.EnterDisplayGameMessage("�E�N���b�N�I", Color.black, 10);

            // �n���o�[�K�[�J�E���g���}�b�N�X�łȂ����
            if (gameManager.hamburgerCount < gameManager.hamburgerNum)
            {
                CheckInHamburger(other.gameObject);
            }
        }
        if (other.gameObject.tag == "clearFlag")
        {
            if (gameManager.clearFlagNum <= gameManager.flags)
            {
                uiManager.EnterDisplayGameMessage("�E�N���b�N�I", Color.yellow, 10);

                if (Input.GetMouseButton(1))
                {
                    gameManager.clearFlag = true;
                }
            }
        }
        if(other.gameObject.tag == "enemy")
        {
            gameManager.deadCount = 0;
        }
    }


    /// <summary>
    /// �v���C���[�̃A�N�V�����ɂ���ă}�b�v���L����
    /// </summary>
    private void SpreadMap()
    {
        // �E�N���b�N������
        if (Input.GetMouseButtonDown(0))
        {
            // �C���X�^���X
            if (map.InstanceMapBlock(transform.position, FPS.GetFourDirectionEulerAngles(transform.eulerAngles))
                == true)
            {
                // �n���o�[�K�[�����炷
                gameManager.hamburgerCount--;
            }
        }
    }


    /// <summary>
    /// �v���C���[���t�B�[���h�O�ɏo��Ȃ��悤�ɂ��܂�
    /// </summary>
    private void ClampLocomotionRange()
    {
        // �v���C���[�̃O���b�h���W
        Vector3Int playerGridPos = map.gridField.GetGridCoordinate(transform.position);

        // �����A�v���C���[�̃|�W�V�������Z�N�V�����̏�łȂ����
        if (map.mapElements[playerGridPos.x, playerGridPos.z] == Elements.eElementType.None_Element ||
            map.mapElements[playerGridPos.x, playerGridPos.z] == Elements.eElementType.OutRange_Element)
        {
            transform.position = _latestPos;     // �|�W�V��������t���[���O�̈ʒu�ɌŒ�
        }
        _latestPos = transform.position;         // �|�W�V�������i�[
    }


    /// <summary>
    /// �}�E�X�̉E�N���b�N�ŃA�C�e����������܂�
    /// </summary>
    /// <param name="item">�������A�C�e��</param>
    private void CheckInFlag(GameObject item)
    {

        if (Input.GetMouseButton(1))
        {
            item.SetActive(false);
            gameManager.flags++;
        }
    }


    /// <summary>
    /// �}�E�X�̉E�N���b�N�ŃA�C�e����������܂�
    /// </summary>
    /// <param name="item">�������A�C�e��</param>
    private void CheckInHamburger(GameObject item)
    {
        if (Input.GetMouseButton(1))
        {
            item.SetActive(false);
            gameManager.deadCount = 30;
            gameManager.hamburgerCount += 2;
        }
    }


    /// <summary>
    /// �}�E�X�̉E�N���b�N�ŃA�C�e����������܂�
    /// </summary>
    /// <param name="item">�������A�C�e��</param>
    private void CheckInClearFlag(GameObject item)
    {

    }
}