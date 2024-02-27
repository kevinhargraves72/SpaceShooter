using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelWindow : MonoBehaviour
{
    TMP_Text levelText;
    Image expBarImage;
    LevelSystem levelSystem;

    private void Awake()
    {
        levelText = transform.Find("levelText").GetComponent<TMP_Text>();
        expBarImage = transform.Find("expBar").Find("expBar").GetComponent<Image>();
    }

    private void Start()
    {
        SetLevelSystem(GameMaster.Instance.playerData.levelSystem);
    }

    void SetExpBarSize(float expNormalized)
    {
        expBarImage.fillAmount = expNormalized;
    }

    void SetLevelNumber(int levelNumber)
    {
        levelText.text = "Level:" + (levelNumber + 1);
    }

    public void SetLevelSystem(LevelSystem lvlSystem)
    {
        levelSystem = lvlSystem; //Set levelSystem

        SetLevelNumber(levelSystem.GetLevelNumber());//update that starting variables
        SetExpBarSize(levelSystem.GetExperienceNormalized());

        levelSystem.OnExpChanged += LevelSystem_OnExpChanged; //Subscribe to ExpChanged event in LevelSystem
        levelSystem.OnLvlChanged += LevelSystem_OnLvlChanged; //Subscribe to LvlChanged event in LevelSystem
    }

    private void LevelSystem_OnLvlChanged(object sender, System.EventArgs e) //What to do when LvlChanged event is called
    {
        SetLevelNumber(levelSystem.GetLevelNumber());//update text
        GameMaster.Instance.playerData.playerSkills.AddSkillPoint();//Add skill point to playerskills (in game master)
    }

    private void LevelSystem_OnExpChanged(object sender, System.EventArgs e) //What to do when ExpChanged event is called
    {
        SetExpBarSize(levelSystem.GetExperienceNormalized());//update expbar size
    }
}
