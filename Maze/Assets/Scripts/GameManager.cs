using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiClass;

public class GameManager : MonoBehaviour
{
    /*�R���|�[�l���g*/
    [SerializeField]GridField gf;

    /*�Q�[���I�u�W�F�N�g*/
    [SerializeField]GameObject playerObj;

    /*�p�����[�^*/
    [SerializeField] int flags;
    public int clearFlagCount;

    void Start()
    {

    }

    private void isGameClear()
    {
        if (clearFlagCount >= flags)
        {

        }
    }


    void Update()
    {
        
    }
}
