using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform[] bulletSpawnPoints;

    public float Atk;
    public float fireDelay = 0.5f;
    float cooldownTimer = 0;
    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            //SHOOT!
            Shoot();
        }

    }

    void Shoot()
    {
        cooldownTimer = fireDelay;

        foreach (Transform BSP in bulletSpawnPoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, BSP.position, BSP.rotation);
            bullet.GetComponent<Bullet>().setDamage(Atk);
            //give each bullet their propper damage 
        }
    }
}
