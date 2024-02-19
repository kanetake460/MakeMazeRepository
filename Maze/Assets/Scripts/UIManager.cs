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
    [SerializeField] TakeshiLibrary.Compass flagCompass;           // �t���O�̃R���p�X
    [SerializeField] TakeshiLibrary.Compass hamburgerCompass;      // �n���o�[�K�[�̃R���p�X
    [SerializeField] GameObject[] HamburgerUI;      // �n���o�[�K�[��UI�z��
    [SerializeField] TextMeshProUGUI deadCountText; // �Q�[���I�[�o�[�J�E���g�̃e�L�X�g
    [SerializeField] TextMeshProUGUI flagCountText; // �t���O�J�E���g�̃e�L�X�g

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
            gameManager.deadCount = 30;
        }
            deadCountText.text = gameManager.deadCount.ToString("f2");
    }


    void Update()
    {
        flagAnim.SetFloat("CompassRotation",flagCompass.distance);
        hamburgerAnim.SetFloat("CompassRotation",hamburgerCompass.distance);
        CountText();
        HamburgerManager();
        CountDead();
    }
}
