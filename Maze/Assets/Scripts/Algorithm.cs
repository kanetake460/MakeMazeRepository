using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiClass
{
    public class Algorithm : MonoBehaviour
    {
        public static T[] Shuffle<T>(T[] arry)
        {
            int rand;
            for (int i = 0; i < arry.Length; i++) 
            {
                rand = Random.Range(0, arry.Length);
                Swap(arry[i], arry[rand]);
            }
            return arry;
        }

        public static void Swap<T>(T lhs, T rhs)
        {
            
            T swap = lhs;
            lhs = rhs;
            rhs = swap;
        }


    }
}
