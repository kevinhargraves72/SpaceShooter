using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    DamageHandler damageHandler;
    PlayerShooting playerShooting;
    PlayerMovement playerMovement;
    PlayerSkills playerSkills;
    PlayerStats playerStats;

    [SerializeField] float defaultMaxHealth = 1f;
    [SerializeField] float defaultDefense = 0f;

    [SerializeField] float defaultAtk = 1f;
    [SerializeField] float defaultPrimaryFireDelay = 0.25f;
    int defaultPrimaryBulletFab = 0;
    int defaultPrimaryBulletSpawns = 0;

    [SerializeField] float defaultMaxSpeed = 5f;
    [SerializeField] float defaultRotSpeed = 180f;

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
            SetDefaultStats();
            GameMaster.Instance.playerData.SetPlayerStats(playerStats);
        }
        else
        {
            SetStats(GameMaster.Instance.playerData.playerStats);
        }
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        switch (e.skillType)
        {
            case PlayerSkills.SkillType.DoubleShot:
                playerStats.PrimaryBulletSpawns = 1;
                //playerShooting.SetPrimaryFireSpawn(1);
                break;
            case PlayerSkills.SkillType.TrippleShot:
                playerStats.PrimaryBulletSpawns = 2;
                //playerShooting.SetPrimaryFireSpawn(2);
                break;
            case PlayerSkills.SkillType.QuadShot:
                playerStats.PrimaryBulletSpawns = 3;
                //playerShooting.SetPrimaryFireSpawn(3);
                break;
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
        damageHandler.SetMaxHealth(stats.maxHealth);
        damageHandler.setDefense(stats.defense);
    }

    public void SetDefaultStats()
    {
        //Health
        playerStats.maxHealth = defaultMaxHealth;
        playerStats.defense = defaultDefense;

        //Attack
        playerStats.atk = defaultAtk;
        playerStats.primaryFireDelay = defaultPrimaryFireDelay;
        playerStats.PrimaryBulletFab = defaultPrimaryBulletFab;
        playerStats.PrimaryBulletSpawns = defaultPrimaryBulletSpawns;

        //Movement
        playerStats.maxSpeed = defaultMaxSpeed;
        playerStats.rotSpeed = defaultRotSpeed;

        SetStats(playerStats);
    }
}
