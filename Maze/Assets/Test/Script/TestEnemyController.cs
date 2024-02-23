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

    GridFieldAStar aStar;
    private void Start()
    {
        enemyTrafo = transform;
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
            fps.Chase(enemyTrafo, player.transform.position, testMap.map, moveSpeed);
        }
        else
        {
            fps.Wandering(enemyTrafo, testMap.map, moveSpeed, 5, 5);
        }


    }
}
