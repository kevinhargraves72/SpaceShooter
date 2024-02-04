using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform[] bulletSpawnPoints;

    public float fireDelay = 0.25f;
    float cooldownTimer = 0;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if(Input.GetButton("Fire1") && cooldownTimer <= 0)
        {
            //SHOOT!
            cooldownTimer = fireDelay;

            foreach(Transform BSP in bulletSpawnPoints)
            {
                GameObject bullet = Instantiate(bulletPrefab, BSP.position, BSP.rotation);
            }
        }
        
    }
}
