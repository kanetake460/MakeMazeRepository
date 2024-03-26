using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("パラメータ")]
    [SerializeField] float wandSpeed;   // 徘徊時スピード
    [SerializeField] float chaseSpeed;  // 追いかけ時スピード
    [SerializeField] float freezeDist;  // フリーズする距離
    [SerializeField] int aStarCount;    // Astarの探索後、次の探索に変わるフレーム数
    [SerializeField] int searchLimit;   // Astarの探索を行う回数
    // プレイヤーとの距離
    [SerializeField,ReadOnly] float _distance;
    // 徘徊の範囲設定
    private const int frameSize = 3, areaX = 5, areaZ = 5;

    [Header("コンポーネント")]
    private GameSceneManager _sceneManager;
    private MapManager _map;
    private EnemyAI _ai;
    private AudioSource _audioSource;

    [Header("設定")]
    [SerializeField] LayerMask searchLayer;
    [SerializeField] string playerTag;
    private GameObject _playerObj;
    private Transform _enemyTrafo;

    /*フラグ*/
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
    /// エネミーの行動
    /// </summary>
    private void EnemyMovement()
    {
        // 追いかけ
        if (_isChase)
        {
            // BGMを流す
            AudioManager.PlayBGM("MaxWell",_audioSource);

            // 一度前回の行動をやめる
            if (_isWandExit) _ai.ExitLocomotion(ref _isWandExit);
            // プレイヤーを追いかける
            _ai.StayLocomotionToAStar(_playerObj.transform.position,chaseSpeed,10);
            
            _isChaceExit = true;
        }
        // 徘徊
        else
        {
            // BGMを流さない
            AudioManager.StopBGM(_audioSource);

            // 一度前回の行動をやめる
            if(_isChaceExit)_ai.ExitLocomotion(ref _isChaceExit);
            // 徘徊させる
            _ai.CustomWandering(wandSpeed,_map.RoomCoordkList,frameSize,areaX,areaZ);
            
            _isWandExit = true;
        }
    }

    /// <summary>
    /// プレイヤーが部屋の座標にいるとき、チェイスをやめて、エネミーの判定を消します。
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
    /// エネミーがしていした距離より、遠くにいるならエネミーは徘徊しないようにします
    /// </summary>
    private void CheckAbleMovement()
    {
        _distance = Vector3.Distance(_enemyTrafo.position, _playerObj.transform.position);
        _isFreeze = _distance > freezeDist;
    }


    private void Update()
    {
        // エスケープシーンなら
        if (_sceneManager.currentScene == GameSceneManager.eScenes.Escape_Scene)
        {
            // もし、プレイヤーがレイキャストに当たったら追いかける
            if (_ai.SearchPlayer(searchLayer, playerTag)) _isChase = true;
            HidePlayer();
            CheckAbleMovement();

            if (_isFreeze == false) 
                EnemyMovement();
        }
    }
}
