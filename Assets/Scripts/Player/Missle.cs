using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    DamageHandler enemy;
    float Atk;
    [SerializeField] float rotSpeed = 90f;
    Transform currentTarget;//Target Missle is currently after
    List<Transform> targets;//All targets that were found
    [SerializeField] GameObject explosionPrefab;
    float selfDestructTimer = 5f;
    bool selfDestruct = false;
    void Update()
    {
        if(currentTarget != null)//If there is a target, face it
        {
            face();
        }
        else
        {
            SetNextTarget();
        }

        if (selfDestruct)
        {
            selfDestructTimer -= Time.deltaTime;
            SelfDestruct();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy = collision.GetComponent<DamageHandler>();
        if(enemy != null)
        {
            if(currentTarget != null && enemy.gameObject.name == currentTarget.gameObject.name)
            {
                Explode();
            }
            if(currentTarget == null)
            {
                Explode();
            }
        }
    }

    public void SetMissle(Transform target, List<Transform> targetList)
    {
        currentTarget = target;
        targets = targetList;
    }
    public void SetAtk(float atk)
    {
        Atk = atk;
    }
    void SetNextTarget()
    {
        targets.RemoveAll(x => x == null);//Get every destroyed enemy out of list
        if(targets != null && targets.Count > 0)
        {//if there is something in the list go to the first elemt
            currentTarget = targets[0];
        }
        else//if the list is empty start self destruct
        {
            selfDestruct = true;
        }
    }
    private void face()
    {
        Vector3 dir = currentTarget.position - transform.position;
        dir.Normalize();

        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }

    void SelfDestruct()
    {
        if(currentTarget == null)
        {
           //blow up when timer is done
           if(selfDestructTimer < 0)
            {
                Explode();
            }
        }
        else
        {
            selfDestruct = false;
            selfDestructTimer = 5f;
        }
    }

    void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.GetComponent<Explosion>().SetAtk(Atk);
        Destroy(gameObject);
    }

}
