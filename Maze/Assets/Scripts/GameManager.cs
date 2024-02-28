using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;

public class GameManager : MonoBehaviour
{
    /*�R���|�[�l���g*/
    public MakeMap map;
    public TakeshiLibrary.CompassUI compassRight;
    public TakeshiLibrary.CompassUI compassLeft;

    /*�Q�[���I�u�W�F�N�g*/
    [SerializeField]GameObject playerObj;

    /*�p�����[�^*/
    public bool clearFlag = false;
    public int flags = 0;           // ���݂̏W�߂��t���O�̐�
    public int clearFlagNum = 10;   // ���C�Y�����ɕK�v�ȃt���O�̐�
    public int hamburgerCount;      // ���݂̃n���o�[�K�[�̐�
    public int hamburgerNum;        // �n���o�[�K�[�̍ő�l
    public float deadCount = 30;    // �Q�[���I�[�o�[�J�E���g


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
    /// </summary>
    private void CountDead()
    {
        if(hamburgerCount <= 0) 
        {
            deadCount -= Time.deltaTime;
        }
    }

    public void ChangeCompass()
    {
        compassLeft.targetTag = "enemy";
        compassRight.targetTag = "clearFlag";
    }


    void Update()
    {
        HamburgerClamp();
        CountDead();
    }
}
