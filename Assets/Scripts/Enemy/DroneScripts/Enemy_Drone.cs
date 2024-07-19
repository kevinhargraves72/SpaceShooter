using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerDetector))]

public class Enemy_Drone : Enemy
{
    [SerializeField] private float DetectionRange;

    private PlayerDetector playerDetector;
    protected override void Awake()
    {
        base.Awake();
        playerDetector = GetComponent<PlayerDetector>();
        playerDetector.SetRange(DetectionRange);

        var wander = new State_Wander(this, DetectionRange);
        var chase = new State_Chase(this, playerDetector);

        void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

        At(wander, chase, () => playerDetector.PlayerInRange);
        At(chase, wander, () => playerDetector.PlayerInRange == false);

        _stateMachine.SetState(wander);
    }
    protected override void Update()
    {
        base.Update();
    }
}
