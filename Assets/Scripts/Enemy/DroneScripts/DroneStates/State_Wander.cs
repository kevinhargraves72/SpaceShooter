using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Wander : IState
{
    private readonly Enemy _enemy;
    private float _wanderRange;
    private Vector3 _wanderTarget;

    public State_Wander(Enemy enemy, float wanderRange)
    {
        _enemy = enemy;
        _wanderRange = wanderRange;
    }

    //TO-DO
    //Get random point(transform) inside specified range
    //In tick have enemy face ponint and move forward
    //If enemy reaches point find new point in range to go to

    public void Tick() 
    {
        if (Vector3.Distance(_enemy.transform.position, _wanderTarget) > 5.0f)
        {
            _enemy.FaceTarget(_wanderTarget);
            _enemy.MoveForward();

        }
        else
        {
            _wanderTarget = RandomPointInRange();
        }

        Debug.DrawLine(_enemy.transform.position, _wanderTarget);
    }

    private Vector3 RandomPointInRange()
    {
      while (true)
      {
          Vector3 point = new Vector3(
          Random.Range(_enemy.transform.position.x - _wanderRange, _enemy.transform.position.x + _wanderRange),
          Random.Range(_enemy.transform.position.y - _wanderRange, _enemy.transform.position.y + _wanderRange),
          0);

          if (GameMaster.Instance.levelManager.IsPointInLevelBounds(point))
              return point;
      }
    }
    public void OnEnter() 
    {
        _wanderTarget = RandomPointInRange();
    }
    public void OnExit() { }

}
