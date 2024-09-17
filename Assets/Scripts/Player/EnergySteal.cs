using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergySteal : MonoBehaviour
{
    Image UICoolDown;

    DamageHandler damageHandler;
    PlayerStats playerStats;

    float coolDownTimer = 0f;
    //float maxCoolDown = 2f;//Make player stat

    float activeTimer = 0f;
    //float maxActiveTimer = 5f;//Make player stat

    //float healPercent = 0.25f;//Make player stat

    //TODO
    //Get into skill tree system
    //Get into UI
    void Start()
    {
        damageHandler = GetComponent<DamageHandler>();
        playerStats = gameObject.GetComponent<Player>().GetPlayerStats();
        UICoolDown = GameMaster.Instance.UIManager.UI_EnergySteal;
    }

    void Update()
    {
        if(Input.GetButtonDown("Ability1") && playerStats.ESMaxCooldown > 0f)
        {
            if(coolDownTimer >= playerStats.ESMaxCooldown)
            {
                coolDownTimer = 0;
                activeTimer = playerStats.ESMaxActiveTimer;
            }
        }

        if(activeTimer > 0)
        {
            activeTimer -= Time.deltaTime;
            UpdateUI(activeTimer / playerStats.ESMaxActiveTimer);
        } 
        else if(coolDownTimer < playerStats.ESMaxCooldown)
        {
            coolDownTimer += Time.deltaTime;
            UpdateUI(coolDownTimer / playerStats.ESMaxCooldown);
        }
    }

    void UpdateUI(float fillAmount)
    {
        UICoolDown.fillAmount = fillAmount;
    }

    public void HealDmg(float dmg)
    {
        if(activeTimer > 0)
        {
            damageHandler.Heal(dmg * playerStats.ESHealPercent);
        }
    }
}
