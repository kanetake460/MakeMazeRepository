using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using System.IO;

public class TestEnemyController : MonoBehaviour
{
    [SerializeField] GameObject pathObj;

    private Transform enemyTrafo;
    private Vector3 pathTarget;
    [SerializeField] GameObject player;

    [SerializeField] TestMap testMap;

    [SerializeField] float moveSpeed = 1;

    GridFieldAStar AS;
    private void Start()
    {
        enemyTrafo = transform;
        pathTarget = enemyTrafo.position;
    }

    private void Awake()
    {
        
    }


    /// <summary>
    /// エネミーオブジェクトがプレイヤーを追いかけます
    /// </summary>
    void EnemyMovement()
    {
        Vector3 enemyPos;

        if (enemyTrafo.position == pathTarget) 
        {
            if (AS == null || 
                AS.pathStack.Count == 0) 
            {
                AS = new GridFieldAStar(testMap.map, testMap.gridField.GetGridCoordinate(transform.position), testMap.gridField.GetGridCoordinate(player.transform.position));
                AS.AStarPath();
            }

            Vector3Int targetCoord = AS.pathStack.Pop().position;
            pathTarget = testMap.gridField.GetVector3Position(targetCoord);

        }

        enemyPos = enemyTrafo.position;

        FPS.MoveToPoint(ref enemyPos, pathTarget,moveSpeed);
            
        enemyTrafo.position = enemyPos;
    }

    void Update()
    {
        if (AS == null)
        {
            AS = new GridFieldAStar(testMap.map, testMap.gridField.GetGridCoordinate(transform.position), testMap.gridField.GetGridCoordinate(player.transform.position));
        }
        FPS.Chase(ref enemyTrafo, player.transform, testMap.map, ref AS, moveSpeed);

    }
}
