using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiClass
{
    public class Algorithm : MonoBehaviour
    {
        public static void Shuffle<T>(T[] arry)
        {
            int rand;
            for (int i = 0; i < arry.Length; i++) 
            {
                rand = Random.Range(0, arry.Length);
                //Swap(arry[i], arry[rand]);
                T swap = arry[i];
                arry[i] = arry[rand];
                arry[rand] = swap;
            }
        }

        public static void Swap<T>(T lhs, T rhs)
        {
            T swap = lhs;
            lhs = rhs;
            rhs = swap;
        }


    }
}
