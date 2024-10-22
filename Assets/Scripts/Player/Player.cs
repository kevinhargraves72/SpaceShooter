using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSkills;

public class Player : MonoBehaviour
{
    public UI_Manager UIManager;

    DamageHandler damageHandler;
    PlayerShooting playerShooting;
    PlayerMovement playerMovement;
    PlayerSkills playerSkills;
    PlayerStats playerStats;
    [SerializeField] GameObject chargeCannon;
    [SerializeField] GameObject missleLaunchers;

    private void Awake()
    {
        playerStats = new PlayerStats();
        damageHandler = gameObject.GetComponent<DamageHandler>();//Weird because of serialized fields for enemies in here
    }
    private void Start()
    {
        playerSkills = GameMaster.Instance.playerData.playerSkills;
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        playerShooting = gameObject.GetComponent<PlayerShooting>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();

        if (GameMaster.Instance.playerData.Load() == true)//TODO IMPLEMENT REAL LOAD SYSTEM
        {
            GameMaster.Instance.playerData.testLoad = false;
            playerStats.SetDefaultStats();
            SetStats(playerStats);
            GameMaster.Instance.playerData.SetPlayerStats(playerStats);
        }
        else
        {
            ResetSkills(playerSkills.GetUnlockedSkillList());
            SetStats(GameMaster.Instance.playerData.playerStats);
        }
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        SetSkill(e.skillType);
    }
    void SetSkill(PlayerSkills.SkillType skill)
    {
        switch (skill)
        {
            case PlayerSkills.SkillType.DoubleShot:
                playerStats.PrimaryBulletSpawns = 1;
                break;
            case PlayerSkills.SkillType.TrippleShot:
                playerStats.PrimaryBulletSpawns = 2;
                break;
            case PlayerSkills.SkillType.QuadShot:
                playerStats.PrimaryBulletSpawns = 3;
                break;
            case PlayerSkills.SkillType.ChargeShot:
                playerStats.MaxChargeActiveTime = 1f;
                chargeCannon.SetActive(true);
                break;
            case PlayerSkills.SkillType.SecondaryFire:
                playerStats.MissleCoolDown = 10f;
                playerStats.MaxMissleCount = 4f;
                playerStats.AtkMultiplyer = 5f;
                missleLaunchers.SetActive(true);
                UIManager.UI_ActivateSecondaryFire.SetActive(true);
                break;
            case PlayerSkills.SkillType.Shield:
                playerStats.MaxSHP = 10f;
                playerStats.SRechargeRate = 1f;
                playerStats.SRechargeDelay = 10f;
                SetDamageHandler(playerStats);
                UIManager.UI_Shield.gameObject.SetActive(true);
                break;
            case PlayerSkills.SkillType.EnergySteal:
                playerStats.ESMaxCooldown = 2f;
                playerStats.ESMaxActiveTimer = 5f;
                playerStats.ESHealPercent = 0.25f;
                UIManager.UI_ActivateEnergySteal.SetActive(true);
                break;

        }
    }
    void ResetSkills(List<SkillType> skills)
    {
        foreach(PlayerSkills.SkillType skill in skills)
        {
            SetSkill(skill);
        }
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }

    public void SetStats(PlayerStats stats)//Make stats into its own class or somthin for save and better system (to pass into this)
    {
        playerStats = stats;

        SetDamageHandler(stats);
        playerMovement.SetPlayerStats(playerStats);
        playerShooting.SetPlayerStats(playerStats);
    }

    public void SetDamageHandler(PlayerStats stats)
    {
        damageHandler.SetAllDamageHandler(stats.maxHealth, stats.defense, stats.MaxSHP, stats.SRechargeDelay, stats.SRechargeRate);
    }

    public void SetChargeShotActiveTime(float time)
    {
        playerStats.ChargeShotActiveTime = time;
    }
    public void AddChargeShotActiveTime(float time)
    {
        if(playerStats.ChargeShotActiveTime + time <= playerStats.MaxChargeActiveTime)
        {
            playerStats.ChargeShotActiveTime += time;
        }
    }
    public float GetChargeShotActiveTimeNormalized()
    {
        if(playerStats.MaxChargeActiveTime != 0)
        {
            return playerStats.ChargeShotActiveTime / playerStats.MaxChargeActiveTime;
        }
        else
        {
            return 0;
        }
        
    }
    public void SetUI_Manager(UI_Manager manager)
    {
        UIManager = manager;
    }
}
