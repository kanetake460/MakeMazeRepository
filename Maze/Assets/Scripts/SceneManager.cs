using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    [SerializeField] Player playerController;
    [SerializeField] Canvas titleCanvas;
    [SerializeField] Canvas resultCanvas;
    public enum eScenes
    {
        Title_Scene,    // タイトルシーン
        Game_Scene,     // ゲームシーン
        Result_Scene,   // リザルトシーン
    }

    public eScenes currentScene 
    { get; set; }

    private void Start()
    {
        currentScene = eScenes.Title_Scene;
    }

    private void Update()
    {
        SceneManagement();
    }

    private void SceneManagement()
    {
        //playerController.enabled = false;
        titleCanvas.enabled = false;
        resultCanvas.enabled = false;
        switch (currentScene)
        {
            case eScenes.Title_Scene:
                titleCanvas.enabled = true;
                break;
            case eScenes.Game_Scene:
                playerController.enabled = true;
                break;
            case eScenes.Result_Scene:
                resultCanvas.enabled = true;
                break;
        }
    }

    public void StartButton()
    {
        currentScene = eScenes.Game_Scene;
    }
}


