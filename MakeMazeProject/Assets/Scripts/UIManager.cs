using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /*オブジェクト参照*/
    [SerializeField] Animator flagAnim;             // フラグセンサーのアニメーション
    [SerializeField] Animator hamburgerAnim;        // ハンバーガーセンサーのアニメーション
    [SerializeField] TakeshiLibrary.CompassUI flagCompass;           // フラグのコンパス
    [SerializeField] Image flagImageLeft;
    [SerializeField] Image shionImage;

    [SerializeField] TakeshiLibrary.CompassUI hamburgerCompass;      // ハンバーガーのコンパス
    [SerializeField] Image burgerImage;
    [SerializeField] Image flagImageRight;

    [SerializeField] GameObject[] HamburgerUI;      // ハンバーガーのUI配列
    [SerializeField] TextMeshProUGUI deadCountText; // ゲームオーバーカウントのテキスト
    [SerializeField] TextMeshProUGUI flagCountText; // フラグカウントのテキスト
    [SerializeField] TextMeshProUGUI messageText;   // ゲーム表示するメッセージのテキスト
    [SerializeField] GameObject manualCanvas;
    [SerializeField] PlayerController playerController;
    public static int _messageCount = 0;

    public void ChangeImage()
    {
        shionImage.enabled = true;
        flagImageRight.enabled = true;

        flagImageLeft.enabled = false;
        burgerImage.enabled = false;
    }

    /// <summary>
    /// マニュアルを表示します
    /// </summary>
    public void ShowManual()
    {
        manualCanvas.SetActive(true);
        playerController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }


    /// <summary>
    /// マニュアルを閉じます
    /// </summary>
    public void CloseManual()
    {
        manualCanvas.SetActive(false);
        playerController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// フラグのカウントのテキストを表示します
    /// </summary>
    public void TextFlagCount(int flagCount, int flagNum)
    {
        flagCountText.text = flagCount + " / " + flagNum;
    }

    /// <summary>
    /// ハンバーガーのUIの表示を管理します
    /// </summary>
    /// <param name="hamburgerCount">ハンバーガーカウント</param>
    /// <param name="hamburgerNum">ハンバーガー最大数</param>
    public void HamburgerManager(int hamburgerNum, int hamburgerCount)
    {
        for(int i = 0; i < hamburgerNum; i++) 
        {
            HamburgerUI[i].SetActive(false);
        }
        for(int i = 0;i < hamburgerCount; i++)
        {
            HamburgerUI[i].SetActive(true);
        }
    }


    /// <summary>
    /// ゲームオーバーカウントを表示します
    /// </summary>
    public void CountDead(float count,float num)
    {
        deadCountText.enabled = true;
        deadCountText.text = count.ToString("f2");
        if(count == num)
        {
            deadCountText.enabled = false;
        }
    }


    /// <summary>
    /// ゲームメッセージを表示します
    /// </summary>
    private void DisplayGameMessage()
    {
        if(_messageCount != 0)
        {
            _messageCount--;
        }
        else
        {
            messageText.color = Color.clear;
        }

    }


    /// <summary>
    /// 表示するゲームメッセ－ジをせっていします
    /// </summary>
    /// <param name="message">メッセージ</param>
    /// <param name="color">色</param>
    /// <param name="messageTime">表示する時間</param>
    public void EnterDisplayGameMessage(string message, Color color, int messageTime = 300)
    {
        messageText.text = message;
        messageText.color = color;
        _messageCount = messageTime;
    }


    void Update()
    {
        flagAnim.SetFloat("CompassRotation",flagCompass.distance);
        hamburgerAnim.SetFloat("CompassRotation",hamburgerCompass.distance);
        DisplayGameMessage();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(manualCanvas.activeSelf)CloseManual();
            else ShowManual(); 
        }
    }
}
