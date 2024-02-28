using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;

public class GameManager : MonoBehaviour
{
    /*コンポーネント*/
    public MakeMap map;
    public TakeshiLibrary.CompassUI compassRight;
    public TakeshiLibrary.CompassUI compassLeft;

    /*ゲームオブジェクト*/
    [SerializeField]GameObject playerObj;

    /*パラメータ*/
    public bool clearFlag = false;
    public int flags = 0;           // 現在の集めたフラグの数
    public int clearFlagNum = 10;   // メイズ完成に必要なフラグの数
    public int hamburgerCount;      // 現在のハンバーガーの数
    public int hamburgerNum;        // ハンバーガーの最大値
    public float deadCount = 30;    // ゲームオーバーカウント


    /// <summary>
    /// ハンバーガーカウントの制限
    /// </summary>
    private void HamburgerClamp()
    {
        if(hamburgerCount > hamburgerNum)
        {
            hamburgerCount = hamburgerNum;
        }
    }


    /// <summary>
    /// ハンバーガーがゼロになったらゲームオーバーカウントを減らす
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
