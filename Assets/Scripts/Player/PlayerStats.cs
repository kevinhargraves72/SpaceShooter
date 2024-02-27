using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    //Health
    public float maxHealth;
    public float defense;

    //Attack
    public float atk;
    public float primaryFireDelay;
    public int PrimaryBulletFab;
    public int PrimaryBulletSpawns;
    public float MaxChargeShotTime;//Upgradeable slot for how much charge you can hold

    //Movement
    public float maxSpeed;
    public float rotSpeed;
}
