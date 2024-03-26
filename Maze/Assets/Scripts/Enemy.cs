using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("�p�����[�^")]
    [SerializeField] float wandSpeed;   // �p�j���X�s�[�h
    [SerializeField] float chaseSpeed;  // �ǂ��������X�s�[�h
    [SerializeField] float freezeDist;  // �t���[�Y���鋗��
    [SerializeField] int aStarCount;    // Astar�̒T����A���̒T���ɕς��t���[����
    [SerializeField] int searchLimit;   // Astar�̒T�����s����
    // �v���C���[�Ƃ̋���
    [SerializeField,ReadOnly] float _distance;
    // �p�j�͈̔͐ݒ�
    private const int frameSize = 3, areaX = 5, areaZ = 5;

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


    /// <summary>
    /// �G�l�~�[�̍s��
    /// </summary>
    private void EnemyMovement()
    {
        // �ǂ�����
        if (_isChase)
        {
            // BGM�𗬂�
            AudioManager.PlayBGM("MaxWell",_audioSource);

            // ��x�O��̍s������߂�
            if (_isWandExit) _ai.ExitLocomotion(ref _isWandExit);
            // �v���C���[��ǂ�������
            _ai.StayLocomotionToAStar(_playerObj.transform.position,chaseSpeed,10);
            
            _isChaceExit = true;
        }
        // �p�j
        else
        {
            // BGM�𗬂��Ȃ�
            AudioManager.StopBGM(_audioSource);

            // ��x�O��̍s������߂�
            if(_isChaceExit)_ai.ExitLocomotion(ref _isChaceExit);
            // �p�j������
            _ai.CustomWandering(wandSpeed,_map.RoomCoordkList,frameSize,areaX,areaZ);
            
            _isWandExit = true;
        }
    }

    /// <summary>
    /// �v���C���[�������̍��W�ɂ���Ƃ��A�`�F�C�X����߂āA�G�l�~�[�̔���������܂��B
    /// </summary>
    private void HidePlayer()
    {
        Coord playerCoord = _map.gridField.GetGridCoordinate(_playerObj.transform.position);
        if (_map.RoomCoordkList.Contains(playerCoord))
        {
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


    private void Update()
    {
        // �G�X�P�[�v�V�[���Ȃ�
        if (_sceneManager.currentScene == GameSceneManager.eScenes.Escape_Scene)
        {
            // �����A�v���C���[�����C�L���X�g�ɓ���������ǂ�������
            if (_ai.SearchPlayer(searchLayer, playerTag)) _isChase = true;
            HidePlayer();
            CheckAbleMovement();

            if (_isFreeze == false) 
                EnemyMovement();
        }
    }
}
