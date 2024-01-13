using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiClass;

public class GameManager : MonoBehaviour
{
    /*コンポーネント*/
    [SerializeField]GridField gf;

    /*ゲームオブジェクト*/
    [SerializeField]GameObject playerObj;

    /*パラメータ*/
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
