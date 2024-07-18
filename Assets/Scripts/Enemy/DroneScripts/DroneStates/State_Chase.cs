using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Chase : IState
{
    private readonly Enemy _enemy;
    private readonly PlayerDetector _playerDetector;

    public State_Chase(Enemy enemy, PlayerDetector playerDetector)
    {
        _enemy = enemy;
        _playerDetector = playerDetector;
    }
    public void Tick() 
    {
        _enemy.MoveForward();
        _enemy.FaceTarget(_playerDetector.GetPlayerPosition());
    }

    public void OnEnter() { }

    public void OnExit() { }
}
