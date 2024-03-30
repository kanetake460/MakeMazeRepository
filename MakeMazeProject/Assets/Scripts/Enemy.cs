using TakeshiLibrary;
using Unity.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("�p�����[�^")]
    [SerializeField] float wandSpeed;   // �p�j���X�s�[�h
    [SerializeField] float chaseSpeed;  // �ǂ��������X�s�[�h
    [SerializeField] float freezeDist;  // �t���[�Y���鋗��
    [SerializeField] int aStarCount;    // Astar�̒T����A���̒T���ɕς��t���[����
    [SerializeField] int searchLimit;   // Astar�̒T�����s����
    
    [SerializeField,ReadOnly] float _distance;  // �v���C���[�Ƃ̋���
    private const int frameSize = 3, areaX = 5, areaZ = 5;  // �p�j�͈̔͐ݒ�

    [Header("�R���|�[�l���g")]
    private GameSceneManager _sceneManager;
    private MapManager _map;
    private EnemyAI _ai;
    private AudioSource _audioSource;

    [Header("�ݒ�")]
    [SerializeField] LayerMask searchLayer;
    [SerializeField] string playerTag;
    private GameObject _playerObj;
    private Transform _enemyTrafo;

    /*�t���O*/
    private bool _isChase = false;
    private bool _isFreeze = false;
    private bool _isChaceExit = false;
    private bool _isWandExit = false;


    private void Awake()
    {
        _map = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>();
        _sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameSceneManager>();
        _playerObj = GameObject.FindGameObjectWithTag(playerTag);
    }

    private void Start()
    {
        _enemyTrafo = transform;
        _ai = new EnemyAI(_enemyTrafo,_map.map,searchLimit);
        _audioSource = gameObject.AddComponent<AudioSource>();

    }

    private void Update()
    {
        // �G�X�P�[�v�V�[���Ȃ�
        if (_sceneManager.CurrentScene == GameSceneManager.eScenes.Escape_Scene)
        {
            // �����A�v���C���[�����C�L���X�g�ɓ���������ǂ�������
            if (_ai.SearchPlayer(searchLayer, playerTag)) _isChase = true;
            HidePlayer();
            CheckAbleMovement();

            if (_isFreeze == false) 
                EnemyMovement();
        }
    }


    /// <summary>
    /// �G�l�~�[�̍s��
    /// </summary>
    private void EnemyMovement()
    {
        // �ǂ�����
        if (_isChase)
        {
            AudioManager.PlayBGM("MaxWell",_audioSource);           // BGM�𗬂�
            
            if (_isWandExit) _ai.ExitLocomotion(ref _isWandExit);   // ��x�O��̍s������߂�
            _ai.StayLocomotionToAStar(_playerObj.transform.position,chaseSpeed,aStarCount); // �v���C���[��ǂ�������
            
            _isChaceExit = true;
        }
        // �p�j
        else
        {
            AudioManager.StopBGM(_audioSource);                     // BGM�𗬂��Ȃ�

            if(_isChaceExit)_ai.ExitLocomotion(ref _isChaceExit);   // ��x�O��̍s������߂�
            _ai.CustomWandering(wandSpeed,_map.RoomCoordkList,frameSize,areaX,areaZ);// �p�j������
            
            _isWandExit = true;
        }
    }

    /// <summary>
    /// �v���C���[�������̍��W�ɂ���Ƃ��A�`�F�C�X����߂āA�G�l�~�[�̔���������܂��B
    /// </summary>
    private void HidePlayer()
    {
        Coord playerCoord = _map.gridField.GridCoordinate(_playerObj.transform.position);
        // �v���C���[�̍��W���������W���X�g�ɂ���Ȃ�
        if (_map.RoomCoordkList.Contains(playerCoord))
        {
            // �ǂ������I���A���������
            _isChase = false;
            GetComponent<BoxCollider>().enabled = false;
        }
        else GetComponent<BoxCollider>().enabled = true;
    }

    /// <summary>
    /// �G�l�~�[�����Ă������������A�����ɂ���Ȃ�G�l�~�[�͜p�j���Ȃ��悤�ɂ��܂�
    /// </summary>
    private void CheckAbleMovement()
    {
        _distance = Vector3.Distance(_enemyTrafo.position, _playerObj.transform.position);
        _isFreeze = _distance > freezeDist;
    }



}
