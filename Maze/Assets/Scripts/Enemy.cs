using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameSceneManager sceneManager;
    [SerializeField] MakeMap map;
    [SerializeField] AudioManager audioManager;

    [SerializeField] Transform enemy;
    [SerializeField] GameObject playerObj;
    [SerializeField] LayerMask searchLayer;
    [SerializeField] string playerTag;

    [SerializeField] float wandSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float escapeDist;

    private bool isChase = false;
    private bool isInit = false;

    private FPS fps;
    private EnemyAI ai;
    private TakeshiLibrary.Compass compass;

    private bool isChaceExit = false;
    private bool isWandcExit = false;


    private void Start()
    {
        enemy = transform;
        fps = new FPS(map.map);
        ai = new EnemyAI(enemy,map.map);
        compass = new TakeshiLibrary.Compass(enemy);
    }



    private void EnemyMovement()
    {
        if(ai.SearchPlayer(searchLayer,playerTag,1.5f)) isChase = true;
        HidePlayer();

        if (isChase)
        {
            if (isWandcExit) ai.ExitLocomotion(ref isWandcExit);
            ai.StayLocomotionToAStar(playerObj.transform.position,chaseSpeed);
            audioManager.ChaseBGM();
            isChaceExit = true;
        }
        else
        {
            if(isChaceExit)ai.ExitLocomotion(ref isChaceExit);
            ai.Wandering(wandSpeed);
            audioManager.StopCheseBGM();
            isWandcExit = true;
        }
    }

    private void HidePlayer()
    {
        Vector3Int playerCoord = map.gridField.GetGridCoordinate(playerObj.transform.position);
        if(map.mapElements[playerCoord.x,playerCoord.z] == Elements.eElementType.Room_Element)
        {
            isChase = false;
        }
    }


    private void Update()
    {
        if (sceneManager.currentScene == GameSceneManager.eScenes.Escape_Scene)
        {
            if(!isInit)
            {
                isInit = true;
            }
            EnemyMovement();
        }
    }
}
