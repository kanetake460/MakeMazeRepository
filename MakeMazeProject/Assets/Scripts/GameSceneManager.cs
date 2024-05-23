using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using System.Linq;

public class GameSceneManager : MonoBehaviour
{
    [Header("�p�����[�^�[")]
    [SerializeField] float titleVolume;
    [SerializeField] float makeVolume;

    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] PlayerController playerController;   // �v���C���[�R���g���[���[
    [SerializeField] GameManager gameManager;   // �Q�[���}�l�[�W���[

    [Header("UI")]
    [SerializeField] GameObject UICanvas;       // UI�L�����o�X
    [SerializeField] GameObject titleCanvas;        // �^�C�g���L�����o�X
    [SerializeField] GameObject resultCanvas;       // ���U���g�L�����o�X
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

    private eScenes _scenes;

    // ���݂̃V�[��
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
        // �ŏ��̓^�C�g���V�[��
        CurrentScene = eScenes.Title_Scene;
        // ���ׂẴG�l�~�[���A�N�e�B�u�ɂ���
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
        titleCanvas.SetActive(false);    // �^�C�g���L�����o�X�����Ȃ�
        resultCanvas.SetActive(false);   // ���U���g�L�����o�X�����Ȃ�

        // ���݂̃V�[����
        switch (CurrentScene)
        {
            case eScenes.Title_Scene:       // �^�C�g���Ȃ�
                playerController.enabled = false;// �v���C���[�R���g���[���[�𓮂���
                titleCanvas.SetActive(true);     // �^�C�g���L�����o�X������
                AudioManager.PlayBGM("TitleBGM");
                AudioManager.SetVolumeBGM(titleVolume);
                break;

            case eScenes.Make_Scene:        // �Q�[���V�[���Ȃ�
                UICanvas.SetActive(true);       // UI�\��
                AudioManager.PlayBGM("MakeBGM",makeVolume);
                AudioManager.SetVolumeBGM(makeVolume);

                break;

            case eScenes.Escape_Scene:
                break;

            case eScenes.Result_Scene:      // ���U���g�Ȃ�
                Cursor.lockState = CursorLockMode.None;
                playerController.enabled = false;
                resultCanvas.SetActive(true);    // ���U���g�L�����o�X������
                UICanvas.SetActive(false);      // UI��\��
                break;
        }
    }

    /// <summary>
    /// �Q�[�����X�^�[�g���܂�
    /// </summary>
    public void StartButton()
    {
        CurrentScene = eScenes.Make_Scene;
        playerController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
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
        CurrentScene = eScenes.Result_Scene;
        resultText.text = "GameClear";
    }


    /// <summary>
    /// �G�X�P�[�v�V�[���ɕύX���܂�
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
    /// �Q�[���I�[�o�[������s���܂�
    /// </summary>
    public void GameOver()
    {
        CurrentScene = eScenes.Result_Scene;
        AudioManager.PlaySEStart("GameOver",1);
        resultText.text = "GameOver";
    }
}


