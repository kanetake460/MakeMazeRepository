using TakeshiLibrary;
using Unity.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("パラメータ")]
    [SerializeField] float wandSpeed;   // 徘徊時スピード
    [SerializeField] float chaseSpeed;  // 追いかけ時スピード
    [SerializeField] float freezeDist;  // フリーズする距離
    [SerializeField] int aStarCount;    // Astarの探索後、次の探索に変わるフレーム数
    [SerializeField] int searchLimit;   // Astarの探索を行う回数
    
    [SerializeField,ReadOnly] float _distance;  // プレイヤーとの距離
    private const int frameSize = 3, areaX = 5, areaZ = 5;  // 徘徊の範囲設定

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

    private void Update()
    {
        // エスケープシーンなら
        if (_sceneManager.CurrentScene == GameSceneManager.eScenes.Escape_Scene)
        {
            // もし、プレイヤーがレイキャストに当たったら追いかける
            if (_ai.SearchPlayer(searchLayer, playerTag)) _isChase = true;
            HidePlayer();
            CheckAbleMovement();

            if (_isFreeze == false) 
                EnemyMovement();
        }
    }


    /// <summary>
    /// エネミーの行動
    /// </summary>
    private void EnemyMovement()
    {
        // 追いかけ
        if (_isChase)
        {
            AudioManager.PlayBGM("MaxWell",_audioSource);           // BGMを流す
            
            if (_isWandExit) _ai.ExitLocomotion(ref _isWandExit);   // 一度前回の行動をやめる
            _ai.StayLocomotionToAStar(_playerObj.transform.position,chaseSpeed,aStarCount); // プレイヤーを追いかける
            
            _isChaceExit = true;
        }
        // 徘徊
        else
        {
            AudioManager.StopBGM(_audioSource);                     // BGMを流さない

            if(_isChaceExit)_ai.ExitLocomotion(ref _isChaceExit);   // 一度前回の行動をやめる
            _ai.CustomWandering(wandSpeed,_map.RoomCoordkList,frameSize,areaX,areaZ);// 徘徊させる
            
            _isWandExit = true;
        }
    }

    /// <summary>
    /// プレイヤーが部屋の座標にいるとき、チェイスをやめて、エネミーの判定を消します。
    /// </summary>
    private void HidePlayer()
    {
        Coord playerCoord = _map.gridField.GridCoordinate(_playerObj.transform.position);
        // プレイヤーの座標が部屋座標リストにあるなら
        if (_map.RoomCoordkList.Contains(playerCoord))
        {
            // 追いかけ終了、判定を消す
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



}
