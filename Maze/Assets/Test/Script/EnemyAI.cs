using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    public class EnemyAI
    {
        private GridFieldAStar _aStar;        // AStar
        private GridFieldMap _map;
        private Vector3Int _locoGoalPoint;
        private Vector3Int _pathTargetCoord;      // ターゲットの座標
        private Transform _enemyTrafo;
        private Vector3Int _enemyCoord;
        private bool isStay;
        private bool isExit;


        public EnemyAI(Transform enemyTrafo,GridFieldMap map)
        {
            _aStar = new GridFieldAStar();
            _map = map;
            _enemyTrafo = enemyTrafo;
            _enemyCoord = _map.gridField.GetGridCoordinate(_enemyTrafo.position);
            _locoGoalPoint = _enemyCoord;
            _pathTargetCoord = _enemyCoord;
        }


        /// <summary>
        /// ある地点まで Vector3 の値まで動かします
        /// </summary>
        /// <param name="trafo">動かす物のトランスフォーム</param>
        /// <param name="point">目的地</param>
        /// <param name="speed">動かすスピード</param>
        /// <returns>ポイントに到達したらtrueを返します</returns>
        public bool MoveToPoint(Transform trafo, Vector3 point, float speed = 1)// refけす
        {
            Vector3 pos = trafo.position;

            pos.x += pos.x <= point.x ? speed * 0.01f : speed * -0.01f;
            pos.y += pos.y <= point.y ? speed * 0.01f : speed * -0.01f;
            pos.z += pos.z <= point.z ? speed * 0.01f : speed * -0.01f;

            if (pos.x <= point.x + speed * 0.1f && pos.x >= point.x + speed * -0.1f) pos.x = point.x;
            if (pos.y <= point.y + speed * 0.1f && pos.y >= point.y + speed * -0.1f) pos.y = point.y;
            if (pos.z <= point.z + speed * 0.1f && pos.z >= point.z + speed * -0.1f) pos.z = point.z;

            trafo.position = pos;

            return pos == point;
        } 
        
        /// <summary>
        /// ある地点まで Vector3 の値まで動かします
        /// </summary>
        /// <param name="trafo">動かす物のトランスフォーム</param>
        /// <param name="point">目的地</param>
        /// <param name="speed">動かすスピード</param>
        /// <returns>ポイントに到達したらtrueを返します</returns>
        public bool LocomotionToCoordPoint( Vector3Int coord,GridFieldMap map, float speed = 1)// refけす
        {
            Vector3 pos = _enemyTrafo.position;
            Vector3 point = map.gridField.grid[coord.x, coord.z];

            pos.x += pos.x <= point.x ? speed * 0.01f : speed * -0.01f;
            pos.y += pos.y <= point.y ? speed * 0.01f : speed * -0.01f;
            pos.z += pos.z <= point.z ? speed * 0.01f : speed * -0.01f;

            if (pos.x <= point.x + speed * 0.1f && pos.x >= point.x + speed * -0.1f) pos.x = point.x;
            if (pos.y <= point.y + speed * 0.1f && pos.y >= point.y + speed * -0.1f) pos.y = point.y;
            if (pos.z <= point.z + speed * 0.1f && pos.z >= point.z + speed * -0.1f) pos.z = point.z;

            _enemyTrafo.position = pos;

            return pos == point;
        }

        /// <summary>
        /// enter,stay,exitでわけて
        /// </summary>



        /// <summary>
        /// 最短経路で目的地まで動かします。一度目的地についたら終了します。
        /// </summary>
        /// <param name="map">マップ</param>
        /// <param name="moveSpeed">追いかけるスピード</param>
        /// <returns>到着したら true</returns>
        public bool LocomotionToAStar(GridFieldMap map, float moveSpeed = 1)
        {
            //Debug.Log(map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z]);
            //Debug.Log(_enemyTrafo.position);

            // パススタックがあるなら
            if (_aStar.pathStack.Count != 0)
            {
                // パスターゲットに追いついたら
                if (LocomotionToCoordPoint(_pathTargetCoord, map, moveSpeed))
                {
                    // 新しいパスターゲットをポップ
                    _pathTargetCoord = _aStar.pathStack.Pop().position;
                    Debug.DrawLine(map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z], map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z] + Vector3.up, Color.red, 0.1f);
                }
            }
            // ない場合
            else
            {
                // エネミーが最後のパスターゲットの位置に来てなかったら
                if(_enemyTrafo.position != map.gridField.grid[_pathTargetCoord.x,_pathTargetCoord.z])
                {
                    return LocomotionToCoordPoint(_pathTargetCoord, map, moveSpeed);
                }
            }

            return false;
        }


        /// <summary>
        /// 最短経路で目的地まで動か続けます。
        /// </summary>
        /// <param name="goalPos">追いかける物の位置</param>
        /// <param name="map">マップ</param>
        /// <param name="aStarCount">再探索を行う間隔</param>
        /// <param name="moveSpeed">追いかけるスピード</param>
        /// <returns>追いついたらtrue</returns>
        public void StayLocomotionToAStar(Vector3 goalPos, GridFieldMap map,int aStarCount = 60, float moveSpeed = 1)
        {
            if (_aStar.pathStack.Count != 0)
            {
                // パスターゲットに追いついたら
                if (LocomotionToCoordPoint(_pathTargetCoord, map, moveSpeed))
                {
                    // 新しいパスターゲットをポップ
                    _pathTargetCoord = _aStar.pathStack.Pop().position;
                    Debug.DrawLine(map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z], map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z] + Vector3.up, Color.red, 0.1f);
                }

                // パススタックがなくなったら新しくパスを作る
                if (_aStar.pathStack.Count == 0)
                {
                    _locoGoalPoint = map.gridField.GetGridCoordinate(goalPos);
                    _enemyCoord = map.gridField.GetGridCoordinate(_enemyTrafo.position);

                    _aStar.AStarPath(map, _enemyCoord, _locoGoalPoint);
                    _pathTargetCoord = _enemyCoord;
                }
            }
        }

        /// <summary>
        /// AStarクラスからパスを設定します。
        /// </summary>
        /// <param name="goalPos"></param>
        /// <param name="map"></param>
        public void EnterLocomotionToAStar(Vector3 goalPos, GridFieldMap map)
        {
            _locoGoalPoint = map.gridField.GetGridCoordinate(goalPos);
            _enemyCoord = map.gridField.GetGridCoordinate(_enemyTrafo.position) ;

            // パスを作って、エネミーのいる場所を最初の場所にする
            _aStar.AStarPath(map, _enemyCoord, _locoGoalPoint);
            Debug.Log(_aStar.pathStack.Count);

            map.SetAStar(_enemyTrafo.position,map.gridField.grid[_locoGoalPoint.x,_locoGoalPoint.z],_aStar);
            _pathTargetCoord = _aStar.pathStack.Pop().position;
            
            // デバッグ
            //Debug.Log(_pathTargetCoord);
        }

        public void ExitLocomotion(ref bool isExit)
        {
            isExit = false;
            isStay = false;

            _locoGoalPoint = _enemyCoord;
            _aStar.pathStack.Clear();
        }

        /// <summary>
        /// エネミーを徘徊させます
        /// </summary>
        public void Wandering(GridFieldMap map, float moveSpeed, int areaX = 10, int areaZ = 10)
        {
            // 徘徊ポイントについたらランダムな位置を徘徊ポイントにする
            if (LocomotionToAStar(map, moveSpeed))
            {
                _locoGoalPoint = map.GetRandomPoint(_enemyCoord, areaX, areaZ);
            }
        }
    }
}