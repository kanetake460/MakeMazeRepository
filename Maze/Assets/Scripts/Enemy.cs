using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] SceneManager sceneManager;
    [SerializeField] MakeMap map;

    [SerializeField] Transform enemy;
    [SerializeField] Transform player;

    [SerializeField] float moveSpeed;

    private bool isChase = false;
    private bool isInit = false;

    private FPS fps;
    private EnemyAI ai;
    private TakeshiLibrary.Compass compass;

    private bool isExit = false;

    Physics p;

    private void Awake()
    {

    }

    private void Start()
    {
        enemy = transform;
        fps = new FPS(map.map);
        ai = new EnemyAI(enemy,map.map);
        compass = new TakeshiLibrary.Compass(enemy);
    }



    private void EnemyMovement()
    {

        if (Vector3.Distance(enemy.position, player.position) < 50)
        {
            //gameObject.GetComponent<Renderer>().sharedMaterial.color = Color.red;
            isChase = true;
        }
        else
        {
            //gameObject.GetComponent<Renderer>().sharedMaterial.color = Color.blue;
            isChase = false;
        }

        if (isChase)
        {
            ai.StayLocomotionToAStar(player.position,moveSpeed);
            isExit = true;
        }
        else
        {
            if(isExit)ai.ExitLocomotion(ref isExit);
            ai.Wandering(moveSpeed);
            //Debug.Log(moveSpeed);
        }
    }


    private void Update()
    {
        

        if (sceneManager.currentScene == SceneManager.eScenes.Escape_Scene)
        {
            if(!isInit)
            {
                isInit = true;
                //Vector3Int randCoord = map.map.GetRandomPoint(map.gridField.GetGridCoordinate(enemy.position), map.gridField.gridWidth, map.gridField.gridDepth);
                //transform.position = map.gridField.grid[randCoord.x, randCoord.z];
            }
            EnemyMovement();
            //gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            //gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
