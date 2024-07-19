using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class State_Attack_Ram : IState
{
    private Enemy _enemy;
    private readonly PlayerDetector _playerDetector;

    float _originalRotSpeed;
    float _newRotSpeed;

    float _originalMovementSpeed;
    float _newMovementSpeed;

    Vector3 _targetPosition;
    //Quaternion _targetRotation;

    public State_Attack_Ram(Enemy enemy, PlayerDetector playerDetector)
    {
        _enemy = enemy;
        _playerDetector = playerDetector;
    }

    public void Tick() 
    {
        if(_enemy.transform.rotation != DesiredRotation(_targetPosition)) 
        {
            _enemy.FaceTarget(_targetPosition);
        }
        else
        {
            //RamAttack(_targetPosition);
        }
    }

    public void OnEnter()
    {
        _targetPosition = _playerDetector.GetPlayerPosition();

        _originalRotSpeed = _enemy.RotationSpeed;
        _originalMovementSpeed = _enemy.Speed;
        _newRotSpeed = _enemy.RotationSpeed * 2;
        _newMovementSpeed = _enemy.Speed * 2;

        _enemy.RotationSpeed = _newRotSpeed;
        _enemy.Speed = _newMovementSpeed;
    }

    public void OnExit()
    {
        _enemy.RotationSpeed = _originalRotSpeed;
        _enemy.Speed = _originalMovementSpeed;
    }

    private Quaternion DesiredRotation(Vector3 target)
    {
        Vector3 dir = target - _enemy.transform.position;
        dir.Normalize();

        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);
        
        return desiredRot;
    }
}
