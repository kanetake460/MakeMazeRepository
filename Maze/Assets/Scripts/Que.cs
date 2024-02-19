using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    /*=====アルゴリズム関数です=====*/
    // 主に授業で作ったアルゴリズムを参考に作成
    // 
    public class Que : MonoBehaviour
    {
        public int[] data;
        public int tail = 0;　// 最後尾
        public int head = 0;   // 先頭
        public bool isEmptyQue = true;


        public int size
        {
            
            get
            {
                return tail - head;
            }
        }


        /// <summary>
        /// Queの初期化
        /// </summary>
        public Que()
        {
             
            tail = 0;
            head = 0;
            isEmptyQue = true;
        }

        /// <summary>
        /// エンキュー
        /// </summary>
        /// <param name="que">キュー</param>
        /// <param name="data">入れたいデータ</param>
        /// <param name="value">入れる値</param>
        public void Enque(int value)
        {
            data[tail] = value;
            tail++;
            if (tail == head)
            {
                Debug.LogError("データがオーバーフローしました");
                Debug.Break();
            }
        }

        /// <summary>
        /// デキュー
        /// </summary>
        /// <param name="que">キュー</param>
        /// <returns>出ていくデータ</returns>
        public int Deque()
        {
            int ret = 0;
            if (tail <= head)
            {
                isEmptyQue = true;
                Debug.LogWarning("データがありません");
            }
            else
            {
                isEmptyQue = false;
                ret = data[head];
                head++;
            }
            return ret;
        }

        /// <summary>
        /// 最後尾の値を消します
        /// </summary>
        public void Remove()
        {
            tail--;
        }
    }
}