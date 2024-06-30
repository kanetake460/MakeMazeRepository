using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Runtime.CompilerServices;
using System;

public class TestEnemyController : MonoBehaviour
{
    [Header("パラメーター")]
    [SerializeField] float chaseSpeed;
    [SerializeField] float wandSpeed;
    [SerializeField] float searchRaySize;
    [SerializeField] int searchLimit;

    [Header("設定")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] Material MT_Red;
    [SerializeField] Material MT_Blue;

    [Header("オブジェクト参照")]
    [SerializeField] GameObject pathObj;
    private GameObject player;

    [Header("コンポーネント")]
    [SerializeField] Test1 testMap;

    private Transform _enemyTrafo;

    /*フラグ*/
    private bool _isChase;
    private bool _isChaceExit = false;
    private bool _isWandExit = false;

    /*コンポーネント*/
    private FPS fps;
    private EnemyAI ai;
    private TakeshiLibrary.Compass compass;


    GridFieldAStar aStar;

    private void Awake()
    {
        _enemyTrafo = transform;
    }


    private void Start()
    {
        //testMap = GameObject.FindGameObjectWithTag("TestMap").GetComponent<MapGridField>();
        ai = new EnemyAI(_enemyTrafo.transform, testMap.map, searchLimit);
        fps = new FPS(testMap.map);
        compass = new TakeshiLibrary.Compass(_enemyTrafo);
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        //if(ai.LocomotionToAStar(chaceSpeed))

        //ai.CustomWandering(wandSpeed, new List<Coord>(), 1, 10, 10);

        EnemyMovement();
    }

    private void EnemyMovement()
    {
        if (ai.SearchPlayer(layerMask, player.tag, searchRaySize)) _isChase = true;

        if (Vector3.Distance(_enemyTrafo.position, player.transform.position) > 50) _isChase = false;

        if (_isChase)
        {
            AudioManager.PlayBGM("MaxWell");
            GetComponentInChildren<Renderer>().material.color = UnityEngine.Color.red;
            if (_isWandExit) ai.ExitLocomotion(ref _isWandExit);
            ai.StayLocomotionToAStar(player.transform.position, chaseSpeed, 60);
            _isChaceExit = true;
        }
        else
        {
            AudioManager.StopBGM();
            GetComponentInChildren<Renderer>().material.color = UnityEngine.Color.white;

            if (_isChaceExit) ai.ExitLocomotion(ref _isChaceExit);
            ai.CustomWandering(wandSpeed, new List<Coord>(), 1, 10, 10);
            _isWandExit = true;
        }
    }
}
