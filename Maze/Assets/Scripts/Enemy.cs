using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] SceneManager manager;
    [SerializeField] MakeMap map;

    GridFieldAStar aStar;

    [SerializeField] Transform enemy;
    [SerializeField] Transform player;

    [SerializeField] float speed;
    FPS.eFourDirection dir = FPS.eFourDirection.right;



    private void Start()
    {
        enemy = transform;
    }

    private void ChasePlayer()
    {
        if (aStar == null)
        {
            aStar = new GridFieldAStar(map.map, map.gridField.GetGridCoordinate(transform.position), map.gridField.GetGridCoordinate(player.transform.position));
        }

        FPS.Chase(ref enemy, player, map.map, ref aStar, speed);
    }

    /// <summary>
    /// 徘徊します
    /// </summary>
    private void Wandering()
    {
        // 向き
        Vector3 rot = new Vector3(0,(int)dir,0);

        enemy.localEulerAngles = rot;

        // 向きに対応するエネミーの一つ前
        Vector3Int prev = map.gridField.GetPreviousCoordinate(dir);
        // エネミーのひとつ前のグリッド座標
        Vector3Int prevPos = map.gridField.GetOtherGridCoordinate(enemy.position,prev);
        Vector3 enemyPos = enemy.position;
        if(map.map.blocks[prevPos.x,prevPos.z].isSpace == false)
        {
            FPS.ClockwiseDirection(ref dir,false);
            Debug.Log(dir);
        }

        FPS.MoveToPoint(ref enemyPos, map.gridField.grid[prevPos.x,prevPos.z]);

        enemy.position = enemyPos;
    }

    private void Update()
    {
        if (manager.currentScene == SceneManager.eScenes.Escape_Scene)
        {
            //ChasePlayer();
            Wandering();
        }
    }
}
