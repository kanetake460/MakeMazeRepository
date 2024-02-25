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

    bool isChase;
    private bool isExit = false;

    private FPS fps;
    private EnemyAI ai;

    GridFieldAStar aStar;

   
    private void Start()
    {
        enemyTrafo = transform;

        ai = new EnemyAI(enemyTrafo.transform, testMap.map);
        fps = new FPS(testMap.map);
    }

    private void Awake()
    {

    }

    private void SearchPlayer()
    {
        RaycastHit hit;
        var point1 = gameObject.transform.position - Vector3.up * 5;//0.5ä|ÇØÇÈÇ±Ç∆Ç≈CapsuleÇ∆ìØÇ∂ëÂÇ´Ç≥ÇÃRayÇ…Ç∑ÇÈ
        var point2 = gameObject.transform.position + Vector3.up * 5;

        if (Physics.CapsuleCast(point1, point2, 5f, Vector3.forward, out hit,50))
        {
            if (hit.collider.tag == "Player")
            {
                Debug.Log("ÇÕÇ¡ÇØÇÒÅIÅI");
                isChase = true;
            }
        }
    }




    void Update()
    {
        Debug.Log(enemyTrafo.rotation);
        float dir = Mathf.Atan2(player.transform.position.z, player.transform.position.x) * Mathf.Rad2Deg;
        enemyTrafo.rotation = Quaternion.Euler(enemyTrafo.rotation.eulerAngles.x, -dir, enemyTrafo.rotation.eulerAngles.z);


        //EnemyMovement();

    }

    private void EnemyMovement()
    {
        if (isChase)
        {
            ai.StayLocomotionToAStar(player.transform.position, chaceSpeed);
            isExit = true;
        }
        else
        {
            if (isExit) ai.ExitLocomotion(ref isExit);
            ai.Wandering(wandSpeed, 10, 10);
        }
        SearchPlayer();
    }
}
