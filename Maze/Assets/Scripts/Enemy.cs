using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] SceneManager manager;
    [SerializeField] MakeMap map;

    [SerializeField] Transform enemy;
    [SerializeField] Transform player;

    [SerializeField] float moveSpeed;

    private bool isChase = false;
    private bool isInit = false;

    private FPS fps = new FPS();

    Physics p;


    private void Start()
    {
        enemy = transform;
    }

    private void EnemyMovement()
    {
        if (Vector3.Distance(enemy.position, player.position) < 50)
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
            fps.Chase(enemy, player.transform.position, map.map, moveSpeed);
        }
        else
        {
            fps.Wandering(enemy, map.map, moveSpeed, 5, 5);
        }
    }


    private void Update()
    {
        

        if (manager.currentScene == SceneManager.eScenes.Escape_Scene)
        {
            if(!isInit)
            {
                isInit = true;
                //Vector3Int randCoord = map.map.GetRandomPoint(map.gridField.GetGridCoordinate(enemy.position), map.gridField.gridWidth, map.gridField.gridDepth);
                //transform.position = map.gridField.grid[randCoord.x, randCoord.z];
            }
            EnemyMovement();
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        //p.spher
    }
}
