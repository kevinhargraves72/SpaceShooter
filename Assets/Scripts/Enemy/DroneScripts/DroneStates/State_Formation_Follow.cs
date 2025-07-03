using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Formation_Follow : IState
{
    // Start is called before the first frame update
    private readonly Enemy _enemy;
    private readonly FormationHandler _formationHandler;
    private readonly Vector2 _offset;

    private readonly float initialMaxSpeed;
    public State_Formation_Follow(Enemy enemy, FormationHandler formationHandler, Vector2 offset)
    {
        _enemy = enemy;
        _formationHandler = formationHandler;
        _offset = offset;
        initialMaxSpeed = enemy.GetInitialMaxSpeed();
    }
    public void Tick()
    {
        //Debug.Log(Vector3.Distance(_enemy.transform.position, _formationHandler.CalculateFormationFollowTarget(_offset)));
        //if ((_enemy.transform.position.y * _formationHandler.GetLeaderUp()).y - (_formationHandler.CalculateFormationFollowTarget(_offset).y * _formationHandler.GetLeaderUp()).y  <= _formationHandler.followBuffer)
        //Debug.Log(((_enemy.transform.position - _formationHandler.CalculateFormationFollowTarget(_offset)).sqrMagnitude < _formationHandler.followBuffer));
        if(Vector3.Distance(_enemy.transform.position, _formationHandler.CalculateFormationFollowTarget(_offset)) <= _formationHandler.followBuffer)
        {
            //Debug.Log("sSpeed:" + _enemy.Speed);
            if (_enemy.Speed > initialMaxSpeed)
            {

               _enemy.Speed -= 4.15f * Time.deltaTime;
                
            }
            //_enemy.Speed -= 4.15f * Time.deltaTime;
        }
        else 
        {
            if ((_enemy.transform.position * _enemy.transform.up.y).y < (_formationHandler.CalculateFormationFollowTarget(_offset) * _enemy.transform.up.y).y)
            {
                //Debug.Log("Speed:" + _enemy.Speed);
                if (_enemy.Speed <= initialMaxSpeed + (initialMaxSpeed * 0.25f))
                {
                    _enemy.Speed += 3.25f * Time.deltaTime;
                }
            }
            if ((_enemy.transform.position * _enemy.transform.up.y).y > (_formationHandler.CalculateFormationFollowTarget(_offset) * _enemy.transform.up.y).y)
            {
                //Debug.Log("Speed:" + _enemy.Speed);
                if (_enemy.Speed >= initialMaxSpeed - (initialMaxSpeed * 0.25f))
                {
                    _enemy.Speed -= 5.25f * Time.deltaTime;
                }
            }


        }
        _enemy.MoveForward();
        _enemy.FaceTarget(_formationHandler.CalculateFormationFollowTarget(_offset));

        //if(Vector3.Distance(_enemy.transform.position, _formationHandler.CalculateFormationFollowTarget(_offset)) > _formationHandler.followBuffer && _enemy.Speed == initialMaxSpeed){
        //    SpeedUp(initialMaxSpeed * 0.25f);
        //}
        //else if(_enemy.Speed > initialMaxSpeed)
        //{
        //    Slowdown(initialMaxSpeed * 0.25f);
        //}




    }

    public void OnEnter() { }
    public void OnExit() { }
}
