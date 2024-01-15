using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiClass;

public class GameManager : MonoBehaviour
{
    /*コンポーネント*/
    public Map map;

    /*ゲームオブジェクト*/
    [SerializeField]GameObject playerObj;

    /*パラメータ*/
    public int flags = 0;     // 現在の集めたフラグの数
    public int clearFlagNum = 10;   // クリアに必要なフラグの数
    public int hamburgerCount;      // 現在のハンバーガーの数
    public int hamburgerNum;        // ハンバーガーの最大値

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
