using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    //Health
    public float maxHealth;
    public float defense;
        //Shield
    public float MaxSHP;
    public float SRechargeDelay;
    public float SRechargeRate;
        //EnergySteal
    public float ESMaxCooldown;
    public float ESMaxActiveTimer;
    public float ESHealPercent;

    //Attack
    public float atk;
    public float primaryFireDelay;
    public int PrimaryBulletFab;
    public int PrimaryBulletSpawns;
        //ChargeShot
    public float MaxChargeActiveTime;//Upgradeable slot for how much charge you can hold
    public float ChargeShotActiveTime;//How much charge player is currently holding
        //SecondaryFire
    public float MissleCoolDown;//Cooldown for each individual missle
    public float MaxMissleCount;//Max amount of missles player can cuurrently hold
    public float AtkMultiplyer;//Missle Atk multiplyer

    //Movement
    public float maxSpeed;
    public float rotSpeed;

    //DefaultStats-------------------------------------------------------------------
    
    //Health
    public float defaultMaxHealth = 10f;
    public float defaultDefense = 0f;
        //Shield
    public float DefaultMaxSHP = 0;
    public float DefaultSRechargeDelay = 0;
    public float DefaultSRechargeRate = 0;
        //EnergySteal
    public float DefaultESMaxCooldown = 0f;
    public float DefaultESMaxActiveTimer = 0f;
    public float DefaultESHealPercent = 0f;


    //Attack
    public float defaultAtk = 1f;
    public float defaultPrimaryFireDelay = 0.25f;
    public int defaultPrimaryBulletFab = 0;
    public int defaultPrimaryBulletSpawns = 0;
        //ChargeShot
    public float defaultMaxChargeActiveTime = 0f;
    public float defaultChargeShotActiveTime = 0f;
        //SecondaryFire
    public float defaultMissleCoolDown = 0f;
    public float defaultMaxMissleCount = 0f;
    public float defaultAtkMultiplyer = 0f;
    
    //Movement
    public float defaultMaxSpeed = 20f;//was 5, then 10, now set to 20 for testing
    public float defaultRotSpeed = 0.15f; //was 180 before system switched to torque (instead of manipulating transform)

    public void SetDefaultStats()
    {
        //Health
        maxHealth = defaultMaxHealth;
        defense = defaultDefense;
            //EnergySteal
        ESMaxCooldown = DefaultESMaxCooldown;
        ESMaxActiveTimer = DefaultESMaxActiveTimer;
        ESHealPercent = DefaultESHealPercent;

    //Attack
        atk = defaultAtk;
        primaryFireDelay = defaultPrimaryFireDelay;
        PrimaryBulletFab = defaultPrimaryBulletFab;
        PrimaryBulletSpawns = defaultPrimaryBulletSpawns;
        //ChargeShot
        MaxChargeActiveTime = defaultMaxChargeActiveTime;
        ChargeShotActiveTime = defaultChargeShotActiveTime;
        //SecondaryFire
        MissleCoolDown = defaultMissleCoolDown;
        MaxMissleCount = defaultMaxMissleCount;
        AtkMultiplyer = defaultAtkMultiplyer;

        //Movement
        maxSpeed = defaultMaxSpeed;
        rotSpeed = defaultRotSpeed;

       
    }
}
