using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerDetector))]
[RequireComponent(typeof(FormationHandler))]


public class Enemy_Dormant_FormationDrone : Enemy
{
    [SerializeField] private float DetectionRange;
    [SerializeField] private float ChaseRange;
    [SerializeField] Vector2 offset;

    private PlayerDetector playerDetector;
    private FormationHandler formationHandler;
    protected override void Awake()
    {
        base.Awake();
        playerDetector = GetComponent<PlayerDetector>();
        playerDetector.SetRange(DetectionRange);

        formationHandler = GetComponent<FormationHandler>();

        var dormant = new State_Formation_Dormant(this); // probably dont need to give it 'this' and could take it out of the states constructor
        var formation_follow = new State_Formation_Follow(this, formationHandler, offset);

        var chase = new State_Chase(this, playerDetector);

        void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

        At(dormant, chase, () => formationHandler.IsLeader() && playerDetector.SearchInRange(ChaseRange));

        At(dormant, formation_follow, () => !formationHandler.IsLeader() && formationHandler.LeaderCurrentState().GetType() == typeof(State_Chase));//This might be a weird sticky point
        At(formation_follow, chase, () => formationHandler.IsLeader());

        _stateMachine.SetState(dormant);
    }
    protected override void Update()
    {
        base.Update();
    }

}
