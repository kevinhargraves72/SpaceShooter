using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem
{
    public event EventHandler OnExpChanged;
    public event EventHandler OnLvlChanged;

    int level;
    int experience;
    int experienceToNextLevel;

    public LevelSystem()
    {
        level = 0;
        experience = 0;
        experienceToNextLevel = 100;
    }

    public void AddExperience(int amount)
    {
        experience += amount;//Add exp
        if(experience >= experienceToNextLevel) //If there is enough experience to level up
        {
            level++;//increase level
            experience -= experienceToNextLevel;//take experience that was needed to get to that level
            if (OnLvlChanged != null) OnLvlChanged(this, EventArgs.Empty);//Call level changed event
        }
        if (OnExpChanged != null) OnExpChanged(this, EventArgs.Empty);//call exp changed event
    }

    public int GetLevelNumber()
    {
        return level;
    }

    public float GetExperienceNormalized()
    {
        return (float)experience / experienceToNextLevel;
    }
}
