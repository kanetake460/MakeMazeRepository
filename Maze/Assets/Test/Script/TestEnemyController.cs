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
    bool isChase;

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
            gameObject.GetComponent<Renderer>().sharedMaterial.color = Color.red;
            isChase = true;
        }
        else
        {
            gameObject.GetComponent<Renderer>().sharedMaterial.color = Color.blue;
            isChase = false;
        }

        if (isChase)
        {
            FPS.Chase(ref enemyTrafo, player.transform.position, testMap.map, moveSpeed);
        }
        else
        {
            FPS.Wandering(ref enemyTrafo, testMap.map, moveSpeed, 5, 5);
        }


    }
}
