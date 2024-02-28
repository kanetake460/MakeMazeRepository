using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TakeshiLibrary;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /*�I�u�W�F�N�g�Q��*/
    [SerializeField] GameManager gameManager;       // �Q�[���}�l�[�W���[
    [SerializeField] Animator flagAnim;             // �t���O�Z���T�[�̃A�j���[�V����
    [SerializeField] Animator hamburgerAnim;        // �n���o�[�K�[�Z���T�[�̃A�j���[�V����
    [SerializeField] TakeshiLibrary.CompassUI flagCompass;           // �t���O�̃R���p�X
    [SerializeField] TakeshiLibrary.CompassUI hamburgerCompass;      // �n���o�[�K�[�̃R���p�X
    [SerializeField] GameObject[] HamburgerUI;      // �n���o�[�K�[��UI�z��
    [SerializeField] TextMeshProUGUI deadCountText; // �Q�[���I�[�o�[�J�E���g�̃e�L�X�g
    [SerializeField] TextMeshProUGUI flagCountText; // �t���O�J�E���g�̃e�L�X�g
    [SerializeField] TextMeshProUGUI messageText;   // �Q�[���\�����郁�b�Z�[�W�̃e�L�X�g
    private int messageCount = 0;

    /// <summary>
    /// �t���O�̃J�E���g�̃e�L�X�g��\�����܂�
    /// </summary>
    public void CountText()
    {
        flagCountText.text = gameManager.flags + " / " + gameManager.clearFlagNum;
    }

    /// <summary>
    /// �n���o�[�K�[��UI�̕\�����Ǘ����܂�
    /// </summary>
    public void HamburgerManager()
    {
        for(int i = 0; i < gameManager.hamburgerNum; i++) 
        {
            HamburgerUI[i].SetActive(false);
        }
        for(int i = 0;i < gameManager.hamburgerCount; i++)
        {
            HamburgerUI[i].SetActive(true);
        }
    }

    /// <summary>
    /// �Q�[���I�[�o�[�J�E���g��\�����܂�
    /// </summary>
    public void CountDead()
    {
        // �n���o�[�K�[���Ȃ��Ȃ�����J�E���g���J�n
        if (gameManager.hamburgerCount <= 0)
        {
            deadCountText.enabled = true;
        }
        else// ����ȊO�ł͔�\���ɂ��ăJ�E���g��߂��܂�
        {
            deadCountText.enabled = false;
        }
            deadCountText.text = gameManager.deadCount.ToString("f2");
    }

    private void DisplayGameMessage()
    {
        if(messageCount != 0)
        {
            messageCount--;
        }
        else
        {
            messageText.color = Color.clear;
        }

    }

    public void EnterDisplayGameMessage(string message, Color color, int messageTime = 300)
    {
        messageText.text = message;
        messageText.color = color;
        messageCount = messageTime;
    }

    void Update()
    {
        flagAnim.SetFloat("CompassRotation",flagCompass.distance);
        hamburgerAnim.SetFloat("CompassRotation",hamburgerCompass.distance);
        CountText();
        HamburgerManager();
        CountDead();
        DisplayGameMessage();
    }
}
