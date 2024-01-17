using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class SceneManager : MonoBehaviour
{
    /*�I�u�W�F�N�g�Q��*/
    [SerializeField] Player playerController;   // �v���C���[�R���g���[���[
    [SerializeField] GameManager gameManager;   // �Q�[���}�l�[�W���[
    [SerializeField] GameObject UICanvas;       // UI�L�����o�X
    [SerializeField] Canvas titleCanvas;        // �^�C�g���L�����o�X
    [SerializeField] Canvas resultCanvas;       // ���U���g�L�����o�X
    [SerializeField] TextMeshProUGUI resultText;// ���U���g�e�L�X�g

    public enum eScenes
    {
        Title_Scene,    // �^�C�g���V�[��
        Game_Scene,     // �Q�[���V�[��
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
        GameClear();
        GameOver();
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
                titleCanvas.enabled = true;     // �^�C�g���L�����o�X������
                break;
            case eScenes.Game_Scene:        // �Q�[���}�V�[���Ȃ�
                playerController.enabled = true;// �v���C���[�R���g���[���[�𓮂���
                UICanvas.SetActive(true);       // UI�\��
                break;
            case eScenes.Result_Scene:      // ���U���g�Ȃ�
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
        currentScene = eScenes.Game_Scene;
    }

    /// <summary>
    /// �Q�[���̃N���A������s���܂�
    /// </summary>
    private void GameClear()
    {
        // �������ׂďW�܂�����
        if (gameManager.flags >= gameManager.clearFlagNum)
        {
            currentScene = eScenes.Result_Scene;
            resultText.text = "GameClear";
        }
    }

    /// <summary>
    /// �Q�[���I�[�o�[������s���܂�
    /// </summary>
    private void GameOver()
    {
        // �Q�[���I�[�o�[�J�E���g��0�ɂȂ�����
        if (gameManager.deadCount < 0)
        {
            currentScene = eScenes.Result_Scene;
            resultText.text = "GameOver";
        }
    }
}


