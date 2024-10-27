using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [NonSerialized] public Transform myTarget;

    private float _minX, _maxX, _minY, _maxY;

    // Update is called once per frame
    void Update()
    {
        if(myTarget != null)
        {
            Vector3 targPos = myTarget.position;
            targPos.z = transform.position.z;
            CheckBounds(targPos);
            //transform.position = targPos;
        }
    }

    public void SetBounds()
    {
        Level_Manager levelManager = GameMaster.Instance.levelManager;
        _maxX = levelManager._maxX;
        _minX = levelManager._minX;
        _minY = levelManager._minY;
        _maxY = levelManager._maxY;
    }

    void CheckBounds(Vector3 targetPos)
    {
        if (targetPos.y > _maxY)
            targetPos.y = _maxY;
        if (targetPos.x > _maxX)
            targetPos.x = _maxX;

        if (targetPos.y < _minY)
            targetPos.y = _minY;
        if (targetPos.x < _minX)
            targetPos.x = _minX;

        transform.position = targetPos;
    }
}
