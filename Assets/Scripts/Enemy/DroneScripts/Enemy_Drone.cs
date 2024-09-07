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
    [SerializeField] private float AttackCooldown;

    private float _attackCooldownTimer;

    private PlayerDetector playerDetector;
    private State_Attack_Ram attack;//Something jason did not do, he just used vars
    protected override void Awake()
    {
        base.Awake();
        playerDetector = GetComponent<PlayerDetector>();
        playerDetector.SetRange(DetectionRange);

        var wander = new State_Wander(this, DetectionRange);
        var chase = new State_Chase(this, playerDetector);
        attack = new State_Attack_Ram(this, playerDetector);

        void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

        At(wander, chase, () => playerDetector.PlayerInRange);
        At(chase, wander, () => playerDetector.PlayerInRange == false);
        At(chase, attack, () => targetInRange(playerDetector.GetPlayerPosition(), AttackRange) && _attackCooldownTimer <= 0);
        At(attack, chase, () => attack.AttackTime >= AttackTimeLength);
        _stateMachine.SetState(wander);
    }
    protected override void Update()
    {
        base.Update();
        Timers();
    }

    private void Timers()
    {
        if(_attackCooldownTimer > 0)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }else if (attack.AttackTime >= AttackTimeLength)
        {
            _attackCooldownTimer = AttackCooldown;
        }
    }
}
