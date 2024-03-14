using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Runtime.CompilerServices;

public class TestEnemyController : MonoBehaviour
{
    [Header("�p�����[�^�[")]
    [SerializeField] float chaceSpeed;
    [SerializeField] float wandSpeed;
    [SerializeField] float searchRaySize;

    [Header("�ݒ�")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] Material MT_Red;
    [SerializeField] Material MT_Blue;

    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] GameObject pathObj;
    [SerializeField] GameObject player;

    [Header("�R���|�[�l���g")]
    [SerializeField] MapGridField testMap;

    private Transform _enemyTrafo;

    /*�t���O*/
    private bool _isChase;
    private bool _isChaceExit = false;
    private bool _isWandExit = false;

    /*�R���|�[�l���g*/
    private FPS fps;
    private EnemyAI ai;
    private TakeshiLibrary.Compass compass;


    GridFieldAStar aStar;

   
    private void Start()
    {
        _enemyTrafo = transform;

        ai = new EnemyAI(_enemyTrafo.transform, testMap.map);
        fps = new FPS(testMap.map);
        compass = new TakeshiLibrary.Compass(_enemyTrafo);
    }

    private void Awake()
    {
    }


    void Update()
    {
        EnemyMovement();
    }

    private void EnemyMovement()
    {
        if (ai.SearchPlayer(layerMask,player.tag,searchRaySize)) _isChase = true;

        if (Vector3.Distance(_enemyTrafo.position,player.transform.position) > 50 ) _isChase = false;

        if (_isChase)
        {
            AudioManager.PlayBGM("MaxWell");
            if (_isWandExit) ai.ExitLocomotion(ref _isWandExit);
            ai.StayLocomotionToAStar(player.transform.position, chaceSpeed,60);
            _isChaceExit = true;
        }
        else
        {
            AudioManager.StopBGM();
            if (_isChaceExit) ai.ExitLocomotion(ref _isChaceExit);
            ai.Wandering(wandSpeed, 10, 10);
            _isWandExit = true;
        }
    }
}
