using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerDetector))]
[RequireComponent(typeof(FormationHandler))]


public class Enemy_FormationDrone : Enemy
{
    [SerializeField] private float DetectionRange;
    [SerializeField] private float ChaseRange;
    [SerializeField] Vector2 offset;

    [SerializeField] private float AttackRange;
    [SerializeField] private float AttackTimeLength;
    [SerializeField] private float AttackCooldown;
    [SerializeField] private float formationAttackDelay;

    private float _attackCooldownTimer;
    //private float _formationAttackDelayTimer;

    private PlayerDetector playerDetector;
    private FormationHandler formationHandler;
    private State_Formation_Attack_Ram attack;
    protected override void Awake()
    {
        base.Awake();
        playerDetector = GetComponent<PlayerDetector>();
        playerDetector.SetRange(DetectionRange);

        formationHandler = GetComponent<FormationHandler>();

        var dormant = new State_Formation_Dormant(this); // probably dont need to give it 'this' and could take it out of the states constructor
        var formation_follow = new State_Formation_Follow(this, formationHandler, offset);

        var chase = new State_Chase(this, playerDetector);
        attack = new State_Formation_Attack_Ram(this, playerDetector, formationHandler, formationAttackDelay);

        void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

        At(dormant, chase, () => formationHandler.IsLeader() && playerDetector.SearchInRange(ChaseRange));
        At(chase, attack, () => targetInRange(playerDetector.GetPlayerPosition(), AttackRange) && _attackCooldownTimer <= 0);
        At(attack, chase, () => formationHandler.IsLeader() && attack.AttackTime >= AttackTimeLength);

        At(dormant, formation_follow, () => !formationHandler.IsLeader() && formationHandler.LeaderCurrentState().GetType() == typeof(State_Chase));//This might be a weird sticky point
        At(formation_follow, chase, () => formationHandler.IsLeader());
        At(formation_follow, attack, () => formationHandler.LeaderCurrentState().GetType() == typeof(State_Formation_Attack_Ram));
        At(attack, formation_follow, () => !formationHandler.IsLeader() && attack.AttackTime >= AttackTimeLength);



        _stateMachine.SetState(dormant);
    }
    protected override void Update()
    {
        base.Update();
        Timers();
    }

    private void Timers()
    {
        if (_attackCooldownTimer > 0)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }
        else if (attack.AttackTime >= AttackTimeLength)
        {
            _attackCooldownTimer = AttackCooldown;
        }

    }

}
