using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public event EventHandler OnSkillPointsChanged;
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public SkillType skillType;
    }
    public enum SkillType
    {
        None,
        ChargeShot,//Atk abilities start
        DoubleShot,TrippleShot,QuadShot,
        SecondaryFire,
        Shield,
        EnergySteal
    }

    private List<SkillType> unlockedSkillTypeList;
    private int skillPoints = 1;
    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }
    public List<SkillType> GetUnlockedSkillList()
    {
        return unlockedSkillTypeList;
    }

    public void AddSkillPoint()
    {
        skillPoints++;
        OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    public int GetSkillPoints()
    {
        return skillPoints;
    }
    private void UnlockSkill(SkillType skillType)
    {
        unlockedSkillTypeList.Add(skillType);
        OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType });
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }
    public bool CanUnlock(SkillType skillType)
    {
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if (skillRequirement != SkillType.None)
        {
            if (IsSkillUnlocked(skillRequirement))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
    public SkillType GetSkillRequirement(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.TrippleShot: return SkillType.DoubleShot;
            case SkillType.QuadShot: return SkillType.TrippleShot;
        }
        return SkillType.None;
    }
    public bool TryUnlockSkill(SkillType skillType)
    {
        if (CanUnlock(skillType))
        {
            if(skillPoints > 0)
            {
                skillPoints--;
                OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
                UnlockSkill(skillType);
                return true;
            }
            else
            {
                return false;
            }
            
        } else
        {
            return false;
        }
    }

}
