using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerDetector))]

public class Enemy_Drone : Enemy
{
    [SerializeField] private float DetectionRange;
    [SerializeField] private float AttackRange;
    [SerializeField] private float AttackTimeLength;

    private PlayerDetector playerDetector;
    protected override void Awake()
    {
        base.Awake();
        playerDetector = GetComponent<PlayerDetector>();
        playerDetector.SetRange(DetectionRange);

        var wander = new State_Wander(this, DetectionRange);
        var chase = new State_Chase(this, playerDetector);
        var attack = new State_Attack_Ram(this, playerDetector);

        void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

        At(wander, chase, () => playerDetector.PlayerInRange);
        At(chase, wander, () => playerDetector.PlayerInRange == false);
        At(chase, attack, () => targetInRange(playerDetector.GetPlayerPosition(), AttackRange));
        At(attack, chase, () => attack.AttackTime >= AttackTimeLength);
        _stateMachine.SetState(wander);
    }
    protected override void Update()
    {
        base.Update();
    }
}
