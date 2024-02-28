using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using System.IO;
using System.Linq;
using System.Drawing;

public class TestEnemyController : MonoBehaviour
{
    [SerializeField] GameObject pathObj;

    private Transform enemyTrafo;
    [SerializeField] GameObject player;

    [SerializeField] TestMap testMap;


    [SerializeField] float chaceSpeed = 5;
    [SerializeField] float wandSpeed = 1;

    [SerializeField] Material MT_Red;
    [SerializeField] Material MT_Blue;

    [SerializeField] LayerMask layerMask;

    bool isChase;
    private bool isChaceExit = false;
    private bool isWandExit = false;

    private FPS fps;
    private EnemyAI ai;
    private TakeshiLibrary.Compass compass;


    GridFieldAStar aStar;

   
    private void Start()
    {
        enemyTrafo = transform;

        ai = new EnemyAI(enemyTrafo.transform, testMap.map);
        fps = new FPS(testMap.map);
        compass = new TakeshiLibrary.Compass(enemyTrafo);
    }

    private void Awake()
    {
    }


    void Update()
    {
        //ai.SearchPlayer(layerMask, player.tag, 1.5f);

        EnemyMovement();
    }

    private void EnemyMovement()
    {
        if (ai.SearchPlayer(layerMask,player.tag,1.5f)) isChase = true;

        if (Vector3.Distance(enemyTrafo.position,player.transform.position) > 50 ) isChase = false;

        if (isChase)
        {
            if (isWandExit) ai.ExitLocomotion(ref isWandExit);
            ai.StayLocomotionToAStar(player.transform.position, chaceSpeed);
            isChaceExit = true;
        }
        else
        {
            if (isChaceExit) ai.ExitLocomotion(ref isChaceExit);
            ai.Wandering(wandSpeed, 10, 10);
            isWandExit = true;
        }
    }
}
