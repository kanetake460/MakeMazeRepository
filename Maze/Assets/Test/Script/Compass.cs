using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass
{
    private Transform _trafo;

    public Compass(Transform trafo) 
    {
        _trafo = trafo;
    
    }

    public bool TurnTowardToPoint(Vector3 point)
    {
            float dir = _trafo.rotation.eulerAngles.y + (Mathf.Atan2(point.z, point.x) * Mathf.Rad2Deg) + 90f;

        _trafo.rotation = Quaternion.Euler(0,0,dir);


        return true;
    }



}
