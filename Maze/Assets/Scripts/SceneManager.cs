using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class SceneManager : MonoBehaviour
{
    /*オブジェクト参照*/
    [SerializeField] Player playerController;   // プレイヤーコントローラー
    [SerializeField] GameManager gameManager;   // ゲームマネージャー
    [SerializeField] GameObject UICanvas;       // UIキャンバス
    [SerializeField] Canvas titleCanvas;        // タイトルキャンバス
    [SerializeField] Canvas resultCanvas;       // リザルトキャンバス
    [SerializeField] TextMeshProUGUI resultText;// リザルトテキスト

    public enum eScenes
    {
        Title_Scene,    // タイトルシーン
        Game_Scene,     // ゲームシーン
        Result_Scene,   // リザルトシーン
    }

    // 現在のシーン
    public eScenes currentScene
    { get; set; }

    private void Start()
    {
        // 最初はタイトルシーン
        currentScene = eScenes.Title_Scene;
    }

    private void Update()
    {
        SceneManagement();
        GameClear();
        GameOver();
    }

    /// <summary>
    /// シーンを管理します
    /// </summary>
    private void SceneManagement()
    {
        titleCanvas.enabled = false;    // タイトルキャンバス見せない
        resultCanvas.enabled = false;   // リザルトキャンバス見せない

        // 現在のシーンが
        switch (currentScene)
        {
            case eScenes.Title_Scene:       // タイトルなら
                titleCanvas.enabled = true;     // タイトルキャンバス見せる
                break;
            case eScenes.Game_Scene:        // ゲームマシーンなら
                playerController.enabled = true;// プレイヤーコントローラーを動かす
                UICanvas.SetActive(true);       // UI表示
                break;
            case eScenes.Result_Scene:      // リザルトなら
                resultCanvas.enabled = true;    // リザルトキャンバス見せる
                UICanvas.SetActive(false);      // UI非表示
                break;
        }
    }

    /// <summary>
    /// ゲームをスタートします
    /// </summary>
    public void StartButton()
    {
        currentScene = eScenes.Game_Scene;
    }

    /// <summary>
    /// ゲームのクリア判定を行います
    /// </summary>
    private void GameClear()
    {
        // 旗がすべて集まったら
        if (gameManager.flags >= gameManager.clearFlagNum)
        {
            currentScene = eScenes.Result_Scene;
            resultText.text = "GameClear";
        }
    }

    /// <summary>
    /// ゲームオーバー判定を行います
    /// </summary>
    private void GameOver()
    {
        // ゲームオーバーカウントが0になったら
        if (gameManager.deadCount < 0)
        {
            currentScene = eScenes.Result_Scene;
            resultText.text = "GameOver";
        }
    }
}


