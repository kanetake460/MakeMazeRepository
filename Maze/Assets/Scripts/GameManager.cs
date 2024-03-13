using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using Unity.VisualScripting;
using static GameSceneManager;

public class GameManager : MonoBehaviour
{
    /*コンポーネント*/
    public MapGridField map;
    public TakeshiLibrary.CompassUI compassRight;
    public TakeshiLibrary.CompassUI compassLeft;
    [SerializeField] GameSceneManager sceneManager;
    [SerializeField] UIManager uiManager;

    /*ゲームオブジェクト*/
    [SerializeField]GameObject playerObj;

    /*パラメータ*/
    public bool? clearFlag = null;          // クリアしたかどうか

    [SerializeField] int flagNum;      // メイズ完成に必要なフラグの数
    [SerializeField] int hamburgerNum;      // ハンバーガーの最大値
    [SerializeField] int deadNum;
    private int _hamburgerCount = 5;             // 現在のハンバーガーの数
    private int _flagCount = 0;              // 現在の集めたフラグの数
    private float _deadCount = 30;           // ゲームオーバーカウント


    /// <summary>
    /// ハンバーガーカウントの制限
    /// </summary>
    private void HamburgerClamp()
    {
        if(_hamburgerCount > hamburgerNum)
        {
            _hamburgerCount = hamburgerNum;
        }
    }


    /// <summary>
    /// ハンバーガーがゼロになったらゲームオーバーカウントを減らす
    /// カウントが0になったらゲームオーバー
    /// </summary>
    private void CountDead()
    {
        if (_hamburgerCount <= 0)
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
    /// エスケープタイムに変更します
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
    /// コンパスを変更します
    /// </summary>
    public void ChangeCompass()
    {
        compassLeft.targetTag = "enemy";
        compassRight.targetTag = "clearFlag";
    }


    /// <summary>
    /// オブジェクトを取得する関数
    /// </summary>
    /// <param name="obj"></param>
    public void CheckInObj(GameObject obj)
    {
        string tag = obj.tag;
        obj.SetActive(false);

        switch (tag)
        {
            case "flag":
                _flagCount++;
                break;

            case "hamburger":
                _hamburgerCount++;
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
        uiManager.HamburgerManager(hamburgerNum,_hamburgerCount);
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
