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
            if (ai.LocomotionToAStar(testMap.map, moveSpeed))
                Debug.Log("“ž’…‚µ‚Ü‚µ‚½");
        }
        else
        {
            //ai.Wandering(testMap.map, moveSpeed, 5, 5);
        }

        if(Input.GetMouseButtonDown(0)) { ai.EnterLocomotionToAStar(player.transform.position,testMap.map); }


    }
}
