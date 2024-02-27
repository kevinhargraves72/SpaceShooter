using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    PlayerStats playerStats;
    PrimaryFire primaryFire;

    private void Start()
    {
        primaryFire = gameObject.GetComponent<PrimaryFire>();

        playerStats = gameObject.GetComponent<Player>().GetPlayerStats();
    }
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            primaryFire.Charge();
        }
        if(Input.GetButtonDown("Fire1")) //Check for player shooting
        {
            primaryFire.Shoot(playerStats.atk, playerStats.primaryFireDelay, playerStats.PrimaryBulletSpawns, playerStats.PrimaryBulletFab);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            primaryFire.ReleaseCharge();
        }
        
    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
    }
}
