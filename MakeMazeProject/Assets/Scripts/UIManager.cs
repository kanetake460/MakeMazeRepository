using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /*�I�u�W�F�N�g�Q��*/
    [SerializeField] Animator flagAnim;             // �t���O�Z���T�[�̃A�j���[�V����
    [SerializeField] Animator hamburgerAnim;        // �n���o�[�K�[�Z���T�[�̃A�j���[�V����
    [SerializeField] TakeshiLibrary.CompassUI flagCompass;           // �t���O�̃R���p�X
    [SerializeField] Image flagImageLeft;
    [SerializeField] Image shionImage;

    [SerializeField] TakeshiLibrary.CompassUI hamburgerCompass;      // �n���o�[�K�[�̃R���p�X
    [SerializeField] Image burgerImage;
    [SerializeField] Image flagImageRight;

    [SerializeField] GameObject[] HamburgerUI;      // �n���o�[�K�[��UI�z��
    [SerializeField] TextMeshProUGUI deadCountText; // �Q�[���I�[�o�[�J�E���g�̃e�L�X�g
    [SerializeField] TextMeshProUGUI flagCountText; // �t���O�J�E���g�̃e�L�X�g
    [SerializeField] TextMeshProUGUI messageText;   // �Q�[���\�����郁�b�Z�[�W�̃e�L�X�g
    [SerializeField] GameObject manualCanvas;
    [SerializeField] PlayerController playerController;
    public static int _messageCount = 0;

    public void ChangeImage()
    {
        shionImage.enabled = true;
        flagImageRight.enabled = true;

        flagImageLeft.enabled = false;
        burgerImage.enabled = false;
    }

    /// <summary>
    /// �}�j���A����\�����܂�
    /// </summary>
    public void ShowManual()
    {
        manualCanvas.SetActive(true);
        playerController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }


    /// <summary>
    /// �}�j���A������܂�
    /// </summary>
    public void CloseManual()
    {
        manualCanvas.SetActive(false);
        playerController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// �t���O�̃J�E���g�̃e�L�X�g��\�����܂�
    /// </summary>
    public void TextFlagCount(int flagCount, int flagNum)
    {
        flagCountText.text = flagCount + " / " + flagNum;
    }

    /// <summary>
    /// �n���o�[�K�[��UI�̕\�����Ǘ����܂�
    /// </summary>
    /// <param name="hamburgerCount">�n���o�[�K�[�J�E���g</param>
    /// <param name="hamburgerNum">�n���o�[�K�[�ő吔</param>
    public void HamburgerManager(int hamburgerNum, int hamburgerCount)
    {
        for(int i = 0; i < hamburgerNum; i++) 
        {
            HamburgerUI[i].SetActive(false);
        }
        for(int i = 0;i < hamburgerCount; i++)
        {
            HamburgerUI[i].SetActive(true);
        }
    }


    /// <summary>
    /// �Q�[���I�[�o�[�J�E���g��\�����܂�
    /// </summary>
    public void CountDead(float count,float num)
    {
        deadCountText.enabled = true;
        deadCountText.text = count.ToString("f2");
        if(count == num)
        {
            deadCountText.enabled = false;
        }
    }


    /// <summary>
    /// �Q�[�����b�Z�[�W��\�����܂�
    /// </summary>
    private void DisplayGameMessage()
    {
        if(_messageCount != 0)
        {
            _messageCount--;
        }
        else
        {
            messageText.color = Color.clear;
        }

    }


    /// <summary>
    /// �\������Q�[�����b�Z�|�W�������Ă����܂�
    /// </summary>
    /// <param name="message">���b�Z�[�W</param>
    /// <param name="color">�F</param>
    /// <param name="messageTime">�\�����鎞��</param>
    public void EnterDisplayGameMessage(string message, Color color, int messageTime = 300)
    {
        messageText.text = message;
        messageText.color = color;
        _messageCount = messageTime;
    }


    void Update()
    {
        flagAnim.SetFloat("CompassRotation",flagCompass.distance);
        hamburgerAnim.SetFloat("CompassRotation",hamburgerCompass.distance);
        DisplayGameMessage();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(manualCanvas.activeSelf)CloseManual();
            else ShowManual(); 
        }
    }
}
