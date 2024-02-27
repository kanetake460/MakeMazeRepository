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

    void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward * 10, new Vector3(10, 10, 10));
    }


    private void SearchPlayer()
    {
        RaycastHit hit;
        var rayHalfExtents = new Vector3(5,10,5);
        var size = 5;
        var point = gameObject.transform.position;//0.5ä|ÇØÇÈÇ±Ç∆Ç≈CapsuleÇ∆ìØÇ∂ëÂÇ´Ç≥ÇÃRayÇ…Ç∑ÇÈ
        var dir = transform.forward;

        Debug.DrawRay(point, dir, UnityEngine.Color.black, 1f);

        if (Physics.BoxCast(point, Vector3.one * size, dir, out hit,Quaternion.identity))
        {
            if (hit.collider.tag == "Player")
            {
                Debug.Log("ÇÕÇ¡ÇØÇÒÅIÅI");
                isChase = true;
            }
        }
    }




    Quaternion rot;
    void Update()
    {

        //enemyTrafo.rotation = Quaternion.Lerp(rot, compass.GetPointAngle(player.transform.position),0.01f);




        //Debug.Log(compass.GetPointAngle(player.transform.position));
        //compass.TurnTowardToPoint(ai.pathTargetPos);
        SearchPlayer();
        EnemyMovement();
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
