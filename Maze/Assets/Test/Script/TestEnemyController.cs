using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using System.IO;
using System.Linq;

public class TestEnemyController : MonoBehaviour
{
    [SerializeField] GameObject pathObj;

    private Transform enemyTrafo;
    [SerializeField] GameObject player;

    [SerializeField] TestMap testMap;

    [SerializeField] float moveSpeed = 1;

    [SerializeField] Material MT_Red;
    [SerializeField] Material MT_Blue;

    bool isChase;
    private bool isExit = false;

    private FPS fps = new FPS();
    private EnemyAI ai;

    GridFieldAStar aStar;

   
    private void Start()
    {
        enemyTrafo = transform;

        ai = new EnemyAI(enemyTrafo.transform, testMap.map);
    }

    private void Awake()
    {

    }
    

    void Update()
    {
        if (Vector3.Distance(enemyTrafo.position, player.transform.position) < 50)
        {
            gameObject.GetComponent<MeshRenderer>().material = MT_Red;
            isChase = true;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = MT_Blue;
            isChase = false;
        }

        if (isChase)
        {
            ai.StayLocomotionToAStar(player.transform.position,moveSpeed);
            isExit = true;
        }
        else
        {
            if (isExit) ai.ExitLocomotion(ref isExit);
            ai.Wandering(moveSpeed, 10, 10);
        }



    }
}
