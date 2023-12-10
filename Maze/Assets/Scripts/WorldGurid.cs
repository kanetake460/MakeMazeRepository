using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGurid : MonoBehaviour
{
    public Vector3[,] grid = new Vector3[10,10];
    [SerializeField] GameObject whiteCellPrefab;
    [SerializeField] GameObject grayCellPrefab;
    void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                grid[x, z] = new Vector3(x * 10, 0, z * 10);
                    Debug.Log(x + z);
                if ((x + z) % 2 == 0)
                {
                    Instantiate(whiteCellPrefab, grid[x, z], Quaternion.identity.normalized);
                }
                else
                {
                    Instantiate(grayCellPrefab, grid[x, z], Quaternion.identity.normalized);
                }
            }
        }

    }

    public void Grid()
    {

    }

    void Update()
    {

    }
}
