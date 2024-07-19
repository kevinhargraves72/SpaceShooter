using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DamageHandler))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float InitialAtk;
    protected float Atk;
    [SerializeField] protected float InitialMaxSpeed;
    protected float Speed;
    [SerializeField] protected float InitialRotationSpeed;
    protected float RotationSpeed;

    protected DamageHandler DamageHandler;
    [SerializeField] protected int ExpAmount;

    protected StateMachine _stateMachine;

    protected virtual void Awake()
    {
        Atk = InitialAtk;
        Speed = InitialMaxSpeed;
        RotationSpeed = InitialRotationSpeed;

        DamageHandler = GetComponent<DamageHandler>();

        DamageHandler.OnDamage += OnDamage;
        DamageHandler.OnDeath += OnDeath;

        _stateMachine = new StateMachine();

        //void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

    }

    protected virtual void Update()
    {
        _stateMachine.Tick();
    }

    //public abstract void Attack();
    //public abstract void MoveTo(Vector3 destination);

    public void MoveForward()
    {
        Vector3 pos = transform.position;

        Vector3 velocity = new Vector3(0, Speed * Time.deltaTime, 0);

        pos += transform.rotation * velocity;

        transform.position = pos;
    }

    public void FaceTarget(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.Normalize();

        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, RotationSpeed * Time.deltaTime);
    }

    public Transform FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        return player.transform;
    }

    public float targetDistance(Transform target)
    {
        return Vector3.Distance(target.position, transform.position);
    }

    public bool targetInRange(Transform target, float range)
    {
        if(targetDistance(target) <= range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void OnDamage(float damage)
    {
        GameMaster.Instance.playerInstance.GetComponent<Player>().AddChargeShotActiveTime(damage);
        GameMaster.Instance.playerInstance.GetComponent<EnergySteal>().HealDmg(damage);
    }

    protected virtual void OnDeath() //ToDo this needs to be changed to allow calling of on death script
    {
        GameMaster.Instance.playerData.levelSystem.AddExperience(ExpAmount);
        Destroy(gameObject);
    }    
}
