using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using System.Linq;

public class GameSceneManager : MonoBehaviour
{
    /*�I�u�W�F�N�g�Q��*/
    [SerializeField] PlayerController playerController;   // �v���C���[�R���g���[���[
    [SerializeField] GameManager gameManager;   // �Q�[���}�l�[�W���[
    [SerializeField] GameObject UICanvas;       // UI�L�����o�X
    [SerializeField] Canvas titleCanvas;        // �^�C�g���L�����o�X
    [SerializeField] Canvas resultCanvas;       // ���U���g�L�����o�X
    [SerializeField] TextMeshProUGUI resultText;// ���U���g�e�L�X�g

    [SerializeField] GameObject clearFlag;
    [SerializeField] GameObject[] enemys;
    public enum eScenes
    {
        Title_Scene,    // �^�C�g���V�[��
        Make_Scene,     // �Q�[���V�[��
        Escape_Scene,   // ������V�[��
        Result_Scene,   // ���U���g�V�[��
    }

    // ���݂̃V�[��
    public eScenes currentScene
    { get; set; }

    private void Start()
    {
        // �ŏ��̓^�C�g���V�[��
        currentScene = eScenes.Title_Scene;
    }

    private void Update()
    {
        SceneManagement();
    }

    /// <summary>
    /// �V�[�����Ǘ����܂�
    /// </summary>
    private void SceneManagement()
    {
        titleCanvas.enabled = false;    // �^�C�g���L�����o�X�����Ȃ�
        resultCanvas.enabled = false;   // ���U���g�L�����o�X�����Ȃ�

        // ���݂̃V�[����
        switch (currentScene)
        {
            case eScenes.Title_Scene:       // �^�C�g���Ȃ�
                Cursor.lockState = CursorLockMode.None;
                titleCanvas.enabled = true;     // �^�C�g���L�����o�X������
                break;

            case eScenes.Make_Scene:        // �Q�[���V�[���Ȃ�
                Cursor.lockState = CursorLockMode.Locked;
                playerController.enabled = true;// �v���C���[�R���g���[���[�𓮂���
                UICanvas.SetActive(true);       // UI�\��
                break;

            case eScenes.Escape_Scene:
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case eScenes.Result_Scene:      // ���U���g�Ȃ�
                Cursor.lockState = CursorLockMode.None;
                playerController.enabled = false;
                resultCanvas.enabled = true;    // ���U���g�L�����o�X������
                UICanvas.SetActive(false);      // UI��\��
                break;
        }
    }

    /// <summary>
    /// �Q�[�����X�^�[�g���܂�
    /// </summary>
    public void StartButton()
    {
        currentScene = eScenes.Make_Scene;

    }

    /// <summary>
    /// �Q�[�������X�^�[�g���܂�
    /// </summary>
    public void RestartButton() 
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// �Q�[���̃N���A������s���܂�
    /// </summary>
    public void GameClear()
    {
        currentScene = eScenes.Result_Scene;
        resultText.text = "GameClear";
    }


    /// <summary>
    /// �G�X�P�[�v�V�[���ɕύX���܂�
    /// </summary>
    public void EscapeTime()
    {
        clearFlag.SetActive(true);
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SetActive(true);
        }
        currentScene = eScenes.Escape_Scene;
    }

    /// <summary>
    /// �Q�[���I�[�o�[������s���܂�
    /// </summary>
    public void GameOver()
    {
        currentScene = eScenes.Result_Scene;
        resultText.text = "GameOver";
    }
}


