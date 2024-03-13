using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using System.Linq;

public class GameSceneManager : MonoBehaviour
{
    /*オブジェクト参照*/
    [SerializeField] TestPlayerController playerController;   // プレイヤーコントローラー
    [SerializeField] GameManager gameManager;   // ゲームマネージャー
    [SerializeField] GameObject UICanvas;       // UIキャンバス
    [SerializeField] Canvas titleCanvas;        // タイトルキャンバス
    [SerializeField] Canvas resultCanvas;       // リザルトキャンバス
    [SerializeField] TextMeshProUGUI resultText;// リザルトテキスト

    [SerializeField] GameObject ClearFlag;
    [SerializeField] BoxCollider[] enemyCollider;
    public enum eScenes
    {
        Title_Scene,    // タイトルシーン
        Make_Scene,     // ゲームシーン
        Escape_Scene,   // 逃げるシーン
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
                Cursor.lockState = CursorLockMode.None;
                titleCanvas.enabled = true;     // タイトルキャンバス見せる
                break;

            case eScenes.Make_Scene:        // ゲームシーンなら
                Cursor.lockState = CursorLockMode.Locked;
                playerController.enabled = true;// プレイヤーコントローラーを動かす
                UICanvas.SetActive(true);       // UI表示
                break;

            case eScenes.Escape_Scene:
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case eScenes.Result_Scene:      // リザルトなら
                Cursor.lockState = CursorLockMode.None;
                playerController.enabled = false;
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
        currentScene = eScenes.Make_Scene;
    }

    /// <summary>
    /// ゲームをリスタートします
    /// </summary>
    public void RestartButton() 
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// ゲームのクリア判定を行います
    /// </summary>
    public void GameClear()
    {
        currentScene = eScenes.Result_Scene;
        resultText.text = "GameClear";
    }


    /// <summary>
    /// エスケープシーンに変更します
    /// </summary>
    public void EscapeTime()
    {
        enemyCollider.All(e => e.enabled = true);
        currentScene = eScenes.Escape_Scene;
    }

    /// <summary>
    /// ゲームオーバー判定を行います
    /// </summary>
    public void GameOver()
    {
        currentScene = eScenes.Result_Scene;
        resultText.text = "GameOver";
    }
}


