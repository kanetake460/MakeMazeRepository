using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using System.Linq;
using TakeshiLibrary;
using Unity.VisualScripting;

public class GameSceneManager : MonoBehaviour
{
    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] PlayerController playerController;   // �v���C���[�R���g���[���[
    [SerializeField] GameManager gameManager;   // �Q�[���}�l�[�W���[

    [Header("UI")]
    [SerializeField] GameObject UICanvas;       // UI�L�����o�X
    [SerializeField] Canvas titleCanvas;        // �^�C�g���L�����o�X
    [SerializeField] Canvas resultCanvas;       // ���U���g�L�����o�X
    [SerializeField] TextMeshProUGUI resultText;// ���U���g�e�L�X�g

    [Header("�Q�[���I�u�W�F�N�g")]
    [SerializeField] GameObject clearFlag;
    [SerializeField] Transform playerTrafo;
    private List<GameObject> _enemys = new List<GameObject>();
    
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

    private void Awake()
    {

    }

    private void Start()
    {
        // �ŏ��̓^�C�g���V�[��
        currentScene = eScenes.Title_Scene;
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
                playerController.enabled = false;// �v���C���[�R���g���[���[�𓮂���
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
        currentScene = eScenes.Escape_Scene;
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
    /// �Q�[���I�[�o�[������s���܂�
    /// </summary>
    public void GameOver()
    {
        currentScene = eScenes.Result_Scene;
        resultText.text = "GameOver";
    }
}


