using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using System.Linq;

public class GameSceneManager : MonoBehaviour
{
    [Header("パラメーター")]
    [SerializeField] float titleVolume;
    [SerializeField] float makeVolume;

    [Header("オブジェクト参照")]
    [SerializeField] PlayerController playerController;   // プレイヤーコントローラー
    [SerializeField] GameManager gameManager;   // ゲームマネージャー

    [Header("UI")]
    [SerializeField] GameObject UICanvas;       // UIキャンバス
    [SerializeField] GameObject titleCanvas;        // タイトルキャンバス
    [SerializeField] GameObject resultCanvas;       // リザルトキャンバス
    [SerializeField] TextMeshProUGUI resultText;// リザルトテキスト

    [Header("ゲームオブジェクト")]
    [SerializeField] GameObject clearFlag;
    [SerializeField] Transform playerTrafo;
    private List<GameObject> _enemys = new List<GameObject>();
    
    public enum eScenes
    {
        Title_Scene,    // タイトルシーン
        Make_Scene,     // ゲームシーン
        Escape_Scene,   // 逃げるシーン
        Result_Scene,   // リザルトシーン
    }

    private eScenes _scenes;

    // 現在のシーン
    public eScenes CurrentScene
    {
        get
        {
            return _scenes;
        }
        set
        {
            _scenes = value;
            AudioManager.StopBGM();
        }
    }

    private void Awake()
    {

    }

    private void Start()
    {
        // 最初はタイトルシーン
        CurrentScene = eScenes.Title_Scene;
        // すべてのエネミーを非アクティブにする
        _enemys.AddRange(GameObject.FindGameObjectsWithTag("enemy"));
        foreach (GameObject enemy in _enemys)
        {
            enemy.SetActive(false);
        }
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
        titleCanvas.SetActive(false);    // タイトルキャンバス見せない
        resultCanvas.SetActive(false);   // リザルトキャンバス見せない

        // 現在のシーンが
        switch (CurrentScene)
        {
            case eScenes.Title_Scene:       // タイトルなら
                playerController.enabled = false;// プレイヤーコントローラーを動かす
                titleCanvas.SetActive(true);     // タイトルキャンバス見せる
                AudioManager.PlayBGM("TitleBGM");
                AudioManager.SetVolumeBGM(titleVolume);
                break;

            case eScenes.Make_Scene:        // ゲームシーンなら
                UICanvas.SetActive(true);       // UI表示
                AudioManager.PlayBGM("MakeBGM",makeVolume);
                AudioManager.SetVolumeBGM(makeVolume);

                break;

            case eScenes.Escape_Scene:
                break;

            case eScenes.Result_Scene:      // リザルトなら
                Cursor.lockState = CursorLockMode.None;
                playerController.enabled = false;
                resultCanvas.SetActive(true);    // リザルトキャンバス見せる
                UICanvas.SetActive(false);      // UI非表示
                break;
        }
    }

    /// <summary>
    /// ゲームをスタートします
    /// </summary>
    public void StartButton()
    {
        CurrentScene = eScenes.Make_Scene;
        playerController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
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
        CurrentScene = eScenes.Result_Scene;
        resultText.text = "GameClear";
    }


    /// <summary>
    /// エスケープシーンに変更します
    /// </summary>
    public void EscapeTime()
    {
        CurrentScene = eScenes.Escape_Scene;
    }

    public void ActiveObject()
    {
        clearFlag.SetActive(true);
        
        foreach (GameObject enemy in _enemys)
        {
            enemy.SetActive(true);
        }
        _enemys.OrderBy(e => Vector3.Distance(e.transform.position, playerTrafo.position)).First().SetActive(false);
    }

    /// <summary>
    /// ゲームオーバー判定を行います
    /// </summary>
    public void GameOver()
    {
        CurrentScene = eScenes.Result_Scene;
        AudioManager.PlaySEStart("GameOver",1);
        resultText.text = "GameOver";
    }
}


