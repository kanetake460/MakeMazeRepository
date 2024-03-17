using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using Unity.VisualScripting;
using static GameSceneManager;

public class GameManager : MonoBehaviour
{
    [Header("�R���|�[�l���g")]
    public MapGridField map;
    [SerializeField] TakeshiLibrary.CompassUI compassRight;
    [SerializeField] TakeshiLibrary.CompassUI compassLeft;
    [SerializeField] GameSceneManager sceneManager;
    [SerializeField] UIManager uiManager;

    /*�Q�[���I�u�W�F�N�g*/
    [SerializeField]GameObject playerObj;

    [Header("�p�����[�^�[")]
    [SerializeField] int flagNum;       // ���C�Y�����ɕK�v�ȃt���O�̐�
    [SerializeField] int hamburgerNum;  // �n���o�[�K�[�̍ő�l
    [SerializeField] int deadNum;       // �Q�[���I�[�o�[�J�E���g�̒l
    [SerializeField] int hamburgerIncrease; // �n���o�[�K�[�̑��ʒl
    [HideInInspector] public bool? clearFlag = null;          // �N���A�������ǂ���
    [SerializeField] public int hamburgerCount = 5;             // ���݂̃n���o�[�K�[�̐�
    private int _flagCount = 0;              // ���݂̏W�߂��t���O�̐�
    private float _deadCount = 30;           // �Q�[���I�[�o�[�J�E���g


    /// <summary>
    /// �n���o�[�K�[�J�E���g�̐���
    /// </summary>
    private void HamburgerClamp()
    {
        if(hamburgerCount > hamburgerNum)
        {
            hamburgerCount = hamburgerNum;
        }
    }


    /// <summary>
    /// �n���o�[�K�[���[���ɂȂ�����Q�[���I�[�o�[�J�E���g�����炷
    /// �J�E���g��0�ɂȂ�����Q�[���I�[�o�[
    /// </summary>
    private void CountDead()
    {
        if (hamburgerCount <= 0)
        {
            _deadCount -= Time.deltaTime;
        }
        else
            _deadCount = deadNum;
        
        if(_deadCount <= 0) 
            clearFlag = false;

        uiManager.CountDead(_deadCount,deadNum);
    }


    /// <summary>
    /// �G�X�P�[�v�^�C���ɕύX���܂�
    /// </summary>
    private void Escape()
    {
        if (_flagCount >= flagNum)
        {
            sceneManager.EscapeTime();
            ChangeCompass();
        }

    }

    /// <summary>
    /// �R���p�X��ύX���܂�
    /// </summary>
    public void ChangeCompass()
    {
        compassLeft.targetTag = "enemy";
        compassRight.targetTag = "clearFlag";
    }


    /// <summary>
    /// �I�u�W�F�N�g���擾����֐�
    /// </summary>
    /// <param name="obj"></param>
    public void CheckInObj(GameObject obj)
    {
        if (obj == null)
            return;
        string tag = obj.tag;
        obj.SetActive(false);

        switch (tag)
        {
            case "flag":
                _flagCount++;
                break;

            case "hamburger":
                hamburgerCount += hamburgerIncrease;
                break;

            case "goal":
                clearFlag = true;
                break;

        }
    }



    void Update()
    {
        HamburgerClamp();
        CountDead();
        Escape();
        uiManager.HamburgerManager(hamburgerNum,hamburgerCount);
        uiManager.TextFlagCount(_flagCount,flagNum);

        if(clearFlag == true)
        {
            sceneManager.GameClear();
        }
        else if(clearFlag == false) 
        {
            sceneManager.GameOver();
        }
    }
}
