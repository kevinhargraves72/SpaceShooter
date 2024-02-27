using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private SkillUnlockPath[] skillUnlockPathArray;

    private PlayerSkills playerSkills;
    private List<SkillButton> skillButtonList;
    private TMPro.TextMeshProUGUI skillPointsText;
    [SerializeField] GameObject pauseMenu;


    private void Awake()
    {
        skillPointsText = pauseMenu.transform.Find("skillPointsText").GetComponent<TMPro.TextMeshProUGUI>();

    }
    private void Start()
    {
        SetPlayerSkills(GameMaster.Instance.playerData.playerSkills);
    }
    public void TogglePause()
    {
        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
    }
    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;

        skillButtonList = new List<SkillButton>();
        skillButtonList.Add(new SkillButton(pauseMenu.transform.Find("skillBtn_DoubleShot").GetComponent<Button>(), PlayerSkills.SkillType.DoubleShot, playerSkills));
        skillButtonList.Add(new SkillButton(pauseMenu.transform.Find("skillBtn_TrippleShot").GetComponent<Button>(), PlayerSkills.SkillType.TrippleShot, playerSkills));
        skillButtonList.Add(new SkillButton(pauseMenu.transform.Find("skillBtn_QuadShot").GetComponent<Button>(), PlayerSkills.SkillType.QuadShot, playerSkills));

        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        playerSkills.OnSkillPointsChanged += PlayerSkills_OnSkillPointsChanged;
        UpdateVisuals();
        UpdateSkillPoints();
    }

    private void PlayerSkills_OnSkillPointsChanged(object sender, System.EventArgs e)
    {
        UpdateSkillPoints();
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateSkillPoints()
    {
        skillPointsText.SetText(playerSkills.GetSkillPoints().ToString());
    }

    public void SetSkill(string skill)
    {
        playerSkills.TryUnlockSkill((PlayerSkills.SkillType)System.Enum.Parse(typeof(PlayerSkills.SkillType), skill));
    }

    private void UpdateVisuals()
    {
        foreach(SkillButton skillButton in skillButtonList)
        {
            skillButton.UpdateVisuals();
        }
        
        //Darken all links
        foreach(SkillUnlockPath skillUnlockPath in skillUnlockPathArray)
        {
            foreach(Image linkImage in skillUnlockPath.linkImageArray)
            {
                linkImage.color = new Color(.5f, .5f, .5f);
            }
        }

        foreach(SkillUnlockPath skillUnlockPath in skillUnlockPathArray)
        {
            if(playerSkills.CanUnlock(skillUnlockPath.skillType))
            {
                foreach(Image linkImage in skillUnlockPath.linkImageArray)
                {
                    linkImage.color = Color.white;
                }
            }
            if (playerSkills.IsSkillUnlocked(skillUnlockPath.skillType))
            {
                foreach (Image linkImage in skillUnlockPath.linkImageArray)
                {
                    linkImage.color = Color.yellow;
                }
            }
        }
    }

    private class SkillButton
    {
        private Button button;
        private PlayerSkills.SkillType skillType;
        private PlayerSkills playerSkills;

        public SkillButton(Button button, PlayerSkills.SkillType skillType, PlayerSkills playerSkills)
        {
            this.button = button;
            this.skillType = skillType;
            this.playerSkills = playerSkills;
        }
        public void UpdateVisuals()
        {
            if (playerSkills.IsSkillUnlocked(skillType))
            {
                button.enabled = false;
                //transform.Find("skillBtn_DoubleShot").GetComponent<Button>().enabled = false;
            }
            else
            {
                if (playerSkills.CanUnlock(skillType))
                {
                   button.interactable = true;
                }
                else
                {
                    button.interactable = false;
                }
            }
        }
    }
    [System.Serializable]
    public class SkillUnlockPath
    {
        public PlayerSkills.SkillType skillType;
        public Image[] linkImageArray;
    }

}
