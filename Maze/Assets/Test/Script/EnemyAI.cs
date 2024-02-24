using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    public class EnemyAI
    {
        private GridFieldAStar _aStar;          // AStar
        private GridFieldMap _map;              // マップ
        
        private Vector3Int _pathTargetCoord;    // 道のりのターゲットの座標
        private Transform _enemyTrafo;          // エネミーのトランスフォーム
        
        private int _stayCount = 0;             // AStarLocomotionの再探索までのカウント


        public EnemyAI(Transform enemyTrafo,GridFieldMap map)
        {
            _aStar = new GridFieldAStar();
            _map = map;
            _enemyTrafo = enemyTrafo;
            _pathTargetCoord = _map.gridField.GetGridCoordinate(_enemyTrafo.position);
        }


        /// <summary>
        /// ある地点まで Vector3 の値まで動かします
        /// </summary>
        /// <param name="trafo">動かす物のトランスフォーム</param>
        /// <param name="point">目的地</param>
        /// <param name="speed">動かすスピード</param>
        /// <returns>ポイントに到達したらtrueを返します</returns>
        public bool MoveToPoint(Transform trafo, Vector3 point, float speed = 1)
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
        public bool LocomotionToCoordPoint(Vector3Int coord, float speed = 1)
        {
            Vector3 pos = _enemyTrafo.position;
            Vector3 point = _map.gridField.grid[coord.x, coord.z];

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
        /// 最短経路で目的地まで動かします。一度目的地についたら終了します。
        /// </summary>
        /// <param name="moveSpeed">追いかけるスピード</param>
        /// <returns>到着したら true</returns>
        public bool LocomotionToAStar( float moveSpeed = 1)
        {
            // パススタックがあるなら
            if (_aStar.pathStack.Count != 0)
            {
                // パスターゲットに追いついたら
                if (LocomotionToCoordPoint(_pathTargetCoord, moveSpeed)) 
                {
                    // 新しいパスターゲットをポップ
                    _pathTargetCoord = _aStar.pathStack.Pop().position;
                    Debug.DrawLine(_map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z], _map.gridField.grid[_pathTargetCoord.x, _pathTargetCoord.z] + Vector3.up, Color.red, 0.1f);
                }
            }
            // ない場合
            else
            {
                // エネミーが最後のパスターゲットの位置に来てなかったら
                if(_enemyTrafo.position != _map.gridField.grid[_pathTargetCoord.x,_pathTargetCoord.z])
                {
                    return LocomotionToCoordPoint(_pathTargetCoord, moveSpeed);
                }
            }

            return false;
        }


        /// <summary>
        /// 最短経路で目的地まで動かし続けます。
        /// </summary>
        /// <param name="goalPos">追いかける物の位置</param>
        /// <param name="map">マップ</param>
        /// <param name="aStarCount">再探索を行う間隔</param>
        /// <param name="moveSpeed">追いかけるスピード</param>
        /// <returns>追いついたらtrue</returns>
        public void StayLocomotionToAStar(Vector3 goalPos,float moveSpeed = 1, int aStarCount = 360)
        {
            LocomotionToAStar(moveSpeed);

            _stayCount++;
            if(_stayCount > aStarCount)
            {
                _stayCount = 0;
                EnterLocomotionToAStar(goalPos);
            }
        }


        /// <summary>
        /// AStarクラスからパスを設定します。
        /// </summary>
        /// <param name="goalPos"></param>
        public void EnterLocomotionToAStar(Vector3 goalPos)
        {
            var enemyCoord = _map.gridField.GetGridCoordinate(_enemyTrafo.position) ;
            var locoGoalCoord = _map.gridField.GetGridCoordinate(goalPos);

            // パスを作って、エネミーのいる場所を最初の場所にする
            _aStar.AStarPath(_map, enemyCoord, locoGoalCoord);

            /// デバッグ
            ///_map.SetAStar(_enemyTrafo.position,_map.gridField.grid[_locoGoalPoint.x,_locoGoalPoint.z],_aStar);
            
            _pathTargetCoord = _aStar.pathStack.Pop().position;
        }


        /// <summary>
        /// 移動を終了させます
        /// </summary>
        /// <param name="isExit"></param>
        public void ExitLocomotion(ref bool isExit)
        {
            isExit = false;

            _stayCount = 0;
            _aStar.pathStack.Clear();
        }


        /// <summary>
        /// エネミーを徘徊させます
        /// </summary>
        public void Wandering(float moveSpeed, int areaX = 10, int areaZ = 10)
        {
            // 徘徊ポイントについたらランダムな位置を徘徊ポイントにする
            if (LocomotionToAStar(moveSpeed))
            {
                var enemyCoord = _map.gridField.GetGridCoordinate(_enemyTrafo.position);
                var locoGoalCoord = _map.GetRandomPoint(enemyCoord, areaX, areaZ);
                
                EnterLocomotionToAStar(_map.gridField.grid[locoGoalCoord.x,locoGoalCoord.z]);
            }
        }
    }
}