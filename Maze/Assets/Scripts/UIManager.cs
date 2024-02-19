using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TakeshiLibrary;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /*オブジェクト参照*/
    [SerializeField] GameManager gameManager;       // ゲームマネージャー
    [SerializeField] Animator flagAnim;             // フラグセンサーのアニメーション
    [SerializeField] Animator hamburgerAnim;        // ハンバーガーセンサーのアニメーション
    [SerializeField] TakeshiLibrary.Compass flagCompass;           // フラグのコンパス
    [SerializeField] TakeshiLibrary.Compass hamburgerCompass;      // ハンバーガーのコンパス
    [SerializeField] GameObject[] HamburgerUI;      // ハンバーガーのUI配列
    [SerializeField] TextMeshProUGUI deadCountText; // ゲームオーバーカウントのテキスト
    [SerializeField] TextMeshProUGUI flagCountText; // フラグカウントのテキスト

    /// <summary>
    /// フラグのカウントのテキストを表示します
    /// </summary>
    public void CountText()
    {
        flagCountText.text = gameManager.flags + " / " + gameManager.clearFlagNum;
    }

    /// <summary>
    /// ハンバーガーのUIの表示を管理します
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
    /// ゲームオーバーカウントを表示します
    /// </summary>
    public void CountDead()
    {
        // ハンバーガーがなくなったらカウントを開始
        if (gameManager.hamburgerCount <= 0)
        {
            deadCountText.enabled = true;
        }
        else// それ以外では非表示にしてカウントを戻します
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
