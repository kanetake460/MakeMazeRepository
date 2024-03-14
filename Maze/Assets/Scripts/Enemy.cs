using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("パラメータ")]
    [SerializeField] float wandSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float escapeDist;
    [SerializeField] int aStarCount;

    [Header("コンポーネント")]
    [SerializeField] GameSceneManager sceneManager;
    [SerializeField] MapGridField map;
    [SerializeField] AudioManager audioManager;

    [Header("設定")]
    [SerializeField] GameObject playerObj;
    [SerializeField] LayerMask searchLayer;
    [SerializeField] string playerTag;

    private Transform _enemy;

    private bool _isChase = false;
    private bool _isInit = false;

    private FPS fps;
    private EnemyAI ai;
    private TakeshiLibrary.Compass compass;

    private bool isChaceExit = false;
    private bool isWandcExit = false;


    private void Start()
    {
        _enemy = transform;
        fps = new FPS(map.map);
        ai = new EnemyAI(_enemy,map.map);
        compass = new TakeshiLibrary.Compass(_enemy);
    }



    private void EnemyMovement()
    {
        if(ai.SearchPlayer(searchLayer,playerTag,5)) _isChase = true;

        if (_isChase)
        {
            AudioManager.PlayBGM("MaxWell");
            if (isWandcExit) ai.ExitLocomotion(ref isWandcExit);
            ai.StayLocomotionToAStar(playerObj.transform.position,chaseSpeed,aStarCount);
            isChaceExit = true;
        }
        else
        {
            AudioManager.StopBGM();
            Debug.Log("wandering");
            if(isChaceExit)ai.ExitLocomotion(ref isChaceExit);
            ai.Wandering(wandSpeed);
            isWandcExit = true;
        }
    }

    //private void HidePlayer()
    //{
    //    Vector3Int playerCoord = map.gridField.GetGridCoordinate(playerObj.transform.position);
    //    if(map.mapElements[playerCoord.x,playerCoord.z] == Elements.eElementType.Room_Element)
    //    {
    //        isChase = false;
    //    }
    //}


    private void Update()
    {
        if (sceneManager.currentScene == GameSceneManager.eScenes.Escape_Scene)
        {
            if(!_isInit)
            {
                _isInit = true;
            }
            EnemyMovement();
        }
    }
}
