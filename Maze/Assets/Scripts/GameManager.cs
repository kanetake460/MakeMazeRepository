using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiClass;

public class GameManager : MonoBehaviour
{
    /*�R���|�[�l���g*/
    public Map map;

    /*�Q�[���I�u�W�F�N�g*/
    [SerializeField]GameObject playerObj;

    /*�p�����[�^*/
    public int flags = 0;     // ���݂̏W�߂��t���O�̐�
    public int clearFlagNum = 10;   // �N���A�ɕK�v�ȃt���O�̐�
    public int hamburgerCount;      // ���݂̃n���o�[�K�[�̐�
    public int hamburgerNum;        // �n���o�[�K�[�̍ő�l

    void Start()
    {

    }

    private void isGameClear()
    {
        if (flags >= clearFlagNum)
        {

        }
    }

    private void HamburgerClamp()
    {
        if(hamburgerCount > hamburgerNum)
        {
            hamburgerCount = hamburgerNum;
        }
    }


    void Update()
    {
        HamburgerClamp();
    }
}
