using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerDetector))]

public class Enemy_Drone : Enemy
{
    [NonSerialized] public PlayerDetector PlayerDetector;
    protected override void Awake()
    {
        base.Awake();
        PlayerDetector = GetComponent<PlayerDetector>();
    }
    protected override void Update()
    {
        base.Update();
    }
}
