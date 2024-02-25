
using TakeshiLibrary;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*�N���X�Q��*/
    [SerializeField] MakeMap map;                   // �}�b�v
    [SerializeField] GameManager gameManager;   // �Q�[���}�l�[�W���[
    FPS fps;

    /*�p�����[�^*/
    [SerializeField] float speed = 0.1f;                            // �ړ��X�s�[�h
    [SerializeField] float Xsensityvity = 3f, Ysensityvity = 3f;    // ���_�X�s�[�h
    private Vector3 _latestPos;

    /*�I�u�W�F�N�g*/
    [SerializeField] GameObject mainCam;            // �v���C���[�̎��_�J����

    /*���_*/
    private bool cursorLock = true;                 // �J�[�\�����b�N��ON/OFF
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
        fps.CursorLock();
        //fps.ClampMoveRange(transform);

        // �v���C���[�̃O���b�h���W
        Vector3Int playerGridPos = map.gridField.GetGridCoordinate(transform.position);

        // �����A�v���C���[�̃|�W�V�������Z�N�V�����̏�łȂ����
        if (map.mapElements[playerGridPos.x, playerGridPos.z] == Elements.eElementType.None_Element ||
            map.mapElements[playerGridPos.x, playerGridPos.z] == Elements.eElementType.OutRange_Element)
        {
            transform.position = _latestPos;     // �|�W�V��������t���[���O�̈ʒu�ɌŒ�
        }
        _latestPos = transform.position;         // �|�W�V�������i�[

        // �v���C���[�̈ړ�
        FPS.Locomotion(transform, speed);

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
            CheckInFlag(other.gameObject);
        }
        // �n���o�[�K�[�ɓ������Ă�����
        if (other.gameObject.tag == "hamburger")
        {
            // �n���o�[�K�[�J�E���g���}�b�N�X�łȂ����
            if (gameManager.hamburgerCount < gameManager.hamburgerNum)
            {
                CheckInHamburger(other.gameObject);
            }
        }
    }

    /*=====�v���C���[�̃A�N�V�����ɂ���ă}�b�v���L����֐�=====*/
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
            gameManager.hamburgerCount += 2;
        }
    }
}