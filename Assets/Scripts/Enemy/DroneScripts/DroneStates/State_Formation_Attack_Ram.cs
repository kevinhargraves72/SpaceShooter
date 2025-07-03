using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class State_Formation_Attack_Ram : IState
{
    private Enemy _enemy;
    private readonly PlayerDetector _playerDetector;
    private readonly FormationHandler _formationHandler;

    float _originalRotSpeed;
    float _newRotSpeed;

    float _originalMovementSpeed;
    float _newMovementSpeed;

    Vector3 _targetPosition;
    Quaternion _targetRotation;
    Quaternion _previousRotation;

    public float AttackTime;

    float originalAttackDelay;
    float AttackDelay;
    bool targetLockOn = true;

    public State_Formation_Attack_Ram(Enemy enemy, PlayerDetector playerDetector, FormationHandler formationHandler, float attackDelay)
    {
        _enemy = enemy;
        _playerDetector = playerDetector;
        _formationHandler = formationHandler;
        originalAttackDelay = attackDelay;
    }

    public void Tick() 
    {
        if(AttackDelay > 0)
        {
            _enemy.MoveForward();
            AttackDelay -= Time.deltaTime;
            targetLockOn = false;
            //this is where i should have the formation drones in delay move forward (could also possibly have them chase the player or somthin, but having them keep changing the leader / who they follow based on this would be tough)
        }
        else if (!targetLockOn)
        {//this is where the drones that are in delay are geared up (stat wise) for attack, and given the proper player location to aim at
            _targetPosition = _playerDetector.GetPlayerPosition();
            _targetRotation = DesiredRotation(_targetPosition);
            _enemy.RotationSpeed = _newRotSpeed;
            _enemy.Speed = _newMovementSpeed;
            targetLockOn = true;
        }
        else if (_enemy.transform.rotation != _previousRotation && AttackTime == 0)
        {
            _previousRotation = _enemy.transform.rotation;
            _enemy.FaceTarget(_targetPosition);

            //Debug.Log(_enemy.gameObject.name + _enemy.transform.rotation);
            //Debug.Log(_enemy.gameObject.name + _targetRotation);
        }
        else
        {
            //attacktime is set to 0 onEnter so the transition chosses how long it wants AT to add up before transitioning
            _enemy.MoveForward();
            AttackTime += Time.deltaTime;
            //Debug.Log(_enemy.gameObject.name + "drone" + _enemy.transform.rotation);
            //Debug.Log(_enemy.gameObject.name + "target" + _targetRotation);
        }

        //if (_enemy.transform.rotation != _previousRotation) 
        //{
        //    _previousRotation = _enemy.transform.rotation;
        //    _enemy.FaceTarget(_targetPosition);

        //    //Debug.Log(_enemy.transform.rotation);
        //    //Debug.Log(_targetRotation); might not be needed anymore
        //}
        //else
        //{
        //    //attacktime is set to 0 onEnter so the transition chosses how long it wants AT to add up before transitioning
        //    _enemy.MoveForward();
        //    AttackTime += Time.deltaTime;
        //}
    }

    public void OnEnter()
    {
        AttackDelay = originalAttackDelay * _formationHandler.GetFormationPosition();

        _originalRotSpeed = _enemy.RotationSpeed;
        _originalMovementSpeed = _enemy.Speed;

        _newRotSpeed = _enemy.RotationSpeed * 2;
        _newMovementSpeed = _enemy.Speed * 2;

        if (AttackDelay == 0)//if the drone is the leader OnEntering this state
        {
            _targetPosition = _playerDetector.GetPlayerPosition();
            _targetRotation = DesiredRotation(_targetPosition);
            _enemy.RotationSpeed = _newRotSpeed;
            _enemy.Speed = _newMovementSpeed;
        }

        AttackTime = 0f;
    }

    public void OnExit()
    {
        _enemy.RotationSpeed = _originalRotSpeed;
        _enemy.Speed = _originalMovementSpeed;
        _previousRotation = new Quaternion(0, 0, 0, 0);
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
