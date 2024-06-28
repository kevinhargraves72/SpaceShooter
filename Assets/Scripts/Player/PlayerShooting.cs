using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    PlayerStats playerStats;
    PrimaryFire primaryFire;
    SecondaryFire secondaryFire;

    private void Start()
    {
        primaryFire = gameObject.GetComponent<PrimaryFire>();
        secondaryFire = gameObject.GetComponent<SecondaryFire>();
        playerStats = gameObject.GetComponent<Player>().GetPlayerStats();
    }
    void Update()
    {
        //PrimaryFire
        if (Input.GetButton("Fire1") && playerStats.ChargeShotActiveTime > 0)
        {
            primaryFire.Charge(playerStats.MaxChargeActiveTime, playerStats.ChargeShotActiveTime, playerStats.atk);
        }
        if(Input.GetButtonDown("Fire1")) //Check for player shooting
        {
            primaryFire.Shoot(playerStats.atk, playerStats.primaryFireDelay, playerStats.PrimaryBulletSpawns, playerStats.PrimaryBulletFab);
        }
        if (Input.GetButtonUp("Fire1") && playerStats.MaxChargeActiveTime != 0)
        {
            primaryFire.ReleaseCharge();
        }
        //SecondaryFire
        if(playerStats.MaxMissleCount > 0)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                secondaryFire.ActivateLaunchers(playerStats.atk);
            }
            if (Input.GetButtonUp("Fire2") || Input.GetButtonDown("Fire1"))
            {
                secondaryFire.ReleaseTargeting();
            }
        }

    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
    }
}
