using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using Unity.VisualScripting;
using static GameSceneManager;

public class GameManager : MonoBehaviour
{
    [Header("コンポーネント")]
    public MapGridField map;
    [SerializeField] TakeshiLibrary.CompassUI compassRight;
    [SerializeField] TakeshiLibrary.CompassUI compassLeft;
    [SerializeField] GameSceneManager sceneManager;
    [SerializeField] UIManager uiManager;

    /*ゲームオブジェクト*/
    [SerializeField]GameObject playerObj;

    [Header("パラメーター")]
    [SerializeField] int flagNum;       // メイズ完成に必要なフラグの数
    [SerializeField] int hamburgerNum;  // ハンバーガーの最大値
    [SerializeField] int deadNum;       // ゲームオーバーカウントの値
    [SerializeField] int hamburgerIncrease; // ハンバーガーの増量値
    [HideInInspector] public bool? clearFlag = null;          // クリアしたかどうか
    [SerializeField] public int hamburgerCount = 5;             // 現在のハンバーガーの数
    private int _flagCount = 0;              // 現在の集めたフラグの数
    private float _deadCount = 30;           // ゲームオーバーカウント

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

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
    /// カウントが0になったらゲームオーバー
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
        if (obj == null)
            return;
        string tag = obj.tag;
        obj.SetActive(false);

        switch (tag)
        {
            case "flag":
                AudioManager.PlayOneShot("GetFlag");
                _flagCount++;
                if(_flagCount == flagNum)
                {
                    sceneManager.ActiveObject();
                }
                break;

            case "hamburger":
                AudioManager.PlayOneShot("EatBurger");
                hamburgerCount += hamburgerIncrease;
                break;

            case "clearFlag":
            AudioManager.PlayOneShot("Clear");
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
