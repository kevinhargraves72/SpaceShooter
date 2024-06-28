using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleLaunchFire : MonoBehaviour
{
    public Transform launchPoint;
    void Update()
    {
        if (launchPoint != null) 
        {
            transform.position = launchPoint.position;
            transform.rotation = launchPoint.rotation;
        }
    }
}
