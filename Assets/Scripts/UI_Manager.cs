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
        
    }

    void Update()
    {
        
    }
}
