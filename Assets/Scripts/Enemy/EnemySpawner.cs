using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float enemyRate = 5f;
    public float enemyRateIncrease = 0.9f;
    public float spawnDistance = 12f;
    float nextEnemy = 1;

    private void Update()
    {
        nextEnemy -= Time.deltaTime;

        if(nextEnemy <= 0)
        {
            nextEnemy = enemyRate;
            enemyRate *= enemyRateIncrease;
            if(enemyRate < 2)
            {
                enemyRate = 2;
            }

            Vector3 offset = Random.onUnitSphere;
            offset.z = 0;
            offset = offset.normalized * spawnDistance;
            Instantiate(enemyPrefab, transform.position + offset, transform.rotation);
        }
    }
}
