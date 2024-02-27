using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public bool testLoad = true; //TODO IMPLEMENT REAL LOAD SYSTEM

    public int numLives = 4;

    public PlayerStats playerStats { get; private set; }
    public LevelSystem levelSystem { get; private set; }
    public PlayerSkills playerSkills { get; private set; }


    public PlayerData()
    {
        levelSystem = new LevelSystem();
        playerSkills = new PlayerSkills();
        playerStats = new PlayerStats();
    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
    }
    public void Save(PlayerData playerData)
    {
        //TODO IMPLEMENT REAL SAVE SYSTEM
    }
    
    public bool Load()
    {
        //TODO IMPLEMENT REAL LOAD SYSTEM
        return testLoad;
    }


}
