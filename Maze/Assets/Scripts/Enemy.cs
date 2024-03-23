using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("パラメータ")]
    [SerializeField] float wandSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float escapeDist;
    [SerializeField] float freezeDist;
    [SerializeField] int aStarCount;
    [SerializeField] int searchLimit;
    [SerializeField,ReadOnly] float _distance;

    [Header("コンポーネント")]
    private GameSceneManager _sceneManager;
    private MapManager _map;
    private AudioSource audioSource;

    [Header("設定")]
    [SerializeField] LayerMask searchLayer;
    [SerializeField] string playerTag;
    private GameObject _playerObj;

    private Transform _enemy;

    private bool _isChase = false;
    private bool _isFreeze = false;

    private EnemyAI ai;

    private bool isChaceExit = false;
    private bool isWandExit = false;


    private void Awake()
    {
        _map = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>();
        _sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameSceneManager>();
        _playerObj = GameObject.FindGameObjectWithTag(playerTag);
    }

    private void Start()
    {
        _enemy = transform;
        ai = new EnemyAI(_enemy,_map.map,searchLimit);
        audioSource = gameObject.AddComponent<AudioSource>();

    }


    private void EnemyMovement()
    {

        if (_isChase)
        {
            AudioManager.PlayBGM("MaxWell",audioSource);
            if (isWandExit) ai.ExitLocomotion(ref isWandExit);
            ai.StayLocomotionToAStar(_playerObj.transform.position,chaseSpeed,10);
            isChaceExit = true;
        }
        else
        {
            AudioManager.StopBGM(audioSource);
            if(isChaceExit)ai.ExitLocomotion(ref isChaceExit);
            ai.CustomWandering(wandSpeed,_map.roomBlockList,3,5,5);
            isWandExit = true;
        }
    }

    /// <summary>
    /// プレイヤーが青いマスにいるとき、チェイスをやめて、エネミーの判定を消します。
    /// </summary>
    private void HidePlayer()
    {
        Coord playerCoord = _map.gridField.GetGridCoordinate(_playerObj.transform.position);
        if (_map.roomBlockList.Contains(playerCoord))
        {
            _isChase = false;
            GetComponent<BoxCollider>().enabled = false;
        }
        else GetComponent<BoxCollider>().enabled = true;
    }


    private void CheckAbleMovement()
    {
        _distance = Vector3.Distance(_enemy.position, _playerObj.transform.position);
        _isFreeze = _distance > freezeDist;
    }


    private void Update()
    {
        if (_sceneManager.currentScene == GameSceneManager.eScenes.Escape_Scene)
        {
            //if (!_isInit)
            //{
            //    _isInit = true;
            //}
            if (ai.SearchPlayer(searchLayer, playerTag, 5)) _isChase = true;
            HidePlayer();
            CheckAbleMovement();

            if (_isFreeze == false) 
                EnemyMovement();
        }
    }
}
