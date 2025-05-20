using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] TMP_Text UI_lifeCount;
    [SerializeField] Image healthBar;
    [SerializeField] Image sheildBar;
    [SerializeField] UI_SkillTree skillTree;
    public Image UI_EnergySteal;
    public GameObject UI_ActivateEnergySteal;
    public Transform UI_PrimaryFire;
    public Transform UI_SecondaryFire;
    public Transform UI_Shield;
    public GameObject UI_ActivateSecondaryFire;

    void Start()
    {
        UI_lifeCount.text = "Lives:" + GameMaster.Instance.lives.ToString();
    }

    void Update()
    {
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            skillTree.TogglePause();
        }
    }

    void UpdateUI()
    {
        if (GameMaster.Instance.playerInstance != null)
        {
            healthBar.fillAmount = GameMaster.Instance.playerInstance.GetComponent<DamageHandler>().GetHealthNormalized();
            //Debug.Log(GameMaster.Instance.playerInstance.GetComponent<DamageHandler>().GetHealthNormalized());
            if (UI_Shield.gameObject.activeSelf)
            {
                sheildBar.fillAmount = GameMaster.Instance.playerInstance.GetComponent<DamageHandler>().GetShieldNormalized();
            }
        }
        else
        {
            healthBar.fillAmount = 0;
            if (UI_Shield.gameObject.activeSelf)
            {
                sheildBar.fillAmount = 0;
            }
        }
    }

    public void UpdateLivesUI()
    {
        UI_lifeCount.text = "Lives:" + GameMaster.Instance.lives.ToString();
    }

}
