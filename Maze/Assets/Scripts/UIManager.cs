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
    [SerializeField] TakeshiLibrary.CompassUI flagCompass;           // フラグのコンパス
    [SerializeField] TakeshiLibrary.CompassUI hamburgerCompass;      // ハンバーガーのコンパス
    [SerializeField] GameObject[] HamburgerUI;      // ハンバーガーのUI配列
    [SerializeField] TextMeshProUGUI deadCountText; // ゲームオーバーカウントのテキスト
    [SerializeField] TextMeshProUGUI flagCountText; // フラグカウントのテキスト
    [SerializeField] TextMeshProUGUI messageText;   // ゲーム表示するメッセージのテキスト
    private int messageCount = 0;

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
        }
            deadCountText.text = gameManager.deadCount.ToString("f2");
    }

    private void DisplayGameMessage()
    {
        if(messageCount != 0)
        {
            messageCount--;
        }
        else
        {
            messageText.color = Color.clear;
        }

    }

    public void EnterDisplayGameMessage(string message, Color color, int messageTime = 300)
    {
        messageText.text = message;
        messageText.color = color;
        messageCount = messageTime;
    }

    void Update()
    {
        flagAnim.SetFloat("CompassRotation",flagCompass.distance);
        hamburgerAnim.SetFloat("CompassRotation",hamburgerCompass.distance);
        CountText();
        HamburgerManager();
        CountDead();
        DisplayGameMessage();
    }
}
