using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrimaryFire : MonoBehaviour
{
    [SerializeField] GameObject[] bulletPrefabs;
    [SerializeField] GameObject[] bulletSpawnPoints;
    [SerializeField] GameObject chargedProjectile;
    [SerializeField] Image chargeVisual;
    Image UI_primaryFireCD;
    Image UI_chargeShotGuage;
    Transform[] shotPoints;

    float cooldownTimer = 0;
    float delay = 1;
    float chargeShotActiveTime;//currently how long the charge shot will last

    [SerializeField]float maxChargeTime = 1f;//how long it takes to charge 
    float chargeTime;//Tracker for when the player is charging the shot, shoots when it is = maxChargeTime
    bool isCharging;
    float Attack;

    private void Start()
    {
        UI_primaryFireCD = GameMaster.Instance.UI_PrimaryFire.Find("PrimaryFireCD_Bar").GetComponent<Image>();
        UI_chargeShotGuage = GameMaster.Instance.UI_PrimaryFire.Find("ChargeShotCharge_Bar").GetComponent<Image>();
    }
    private void Update()
    {
        UpdateUI();
        if (cooldownTimer > 0)//if there is time left on timer keep ticking it down
        {
            cooldownTimer -= Time.deltaTime;
        }
        if(isCharging == true && chargeTime < maxChargeTime)
        {
            chargeTime += Time.deltaTime;
            //chargeVisual.fillAmount = chargeTime;
        } else if(isCharging == true && chargeTime >= maxChargeTime)
        {
            ChargeShot();
        }
    }
    void UpdateUI()
    {
        chargeVisual.fillAmount = chargeTime;
        UI_primaryFireCD.fillAmount = cooldownTimer/delay;
        UI_chargeShotGuage.fillAmount = gameObject.GetComponent<Player>().GetChargeShotActiveTimeNormalized();
    }
    public void Shoot(float atk, float fireDelay, int spawnIndex, int bulletFabIndex)//take care of shooting
    {
        if(cooldownTimer <= 0)
        {
            cooldownTimer = fireDelay;//set shot cooldown timer 
            delay = fireDelay;
            shotPoints = bulletSpawnPoints[spawnIndex].GetComponentsInChildren<Transform>();

            for(int i=1; i < shotPoints.Length; i++)//Goes through the transforms of children starting at 1 not 0
            {
                GameObject bullet = Instantiate(bulletPrefabs[bulletFabIndex], shotPoints[i].position, shotPoints[i].rotation);
                bullet.GetComponent<Bullet>().setDamage(atk);
            }

        }
        
    }
    public void ChargeShot()
    {
        if (chargeShotActiveTime > 0)
        {
            if(chargedProjectile.activeSelf == false)
            {
                chargedProjectile.SetActive(true);
                chargedProjectile.GetComponent<Bullet>().setDamage(Attack); 
            }
            chargeShotActiveTime -= Time.deltaTime;
            gameObject.GetComponent<Player>().SetChargeShotActiveTime(chargeShotActiveTime);

        }
        else
        {
            ReleaseCharge();
        }
        
    }
    public void Charge(float max, float active, float attack)
    {
        if (!isCharging && chargedProjectile.activeSelf == false)
        {
            isCharging = true;
            maxChargeTime = max;
            chargeShotActiveTime = active;
            Attack = attack;
        }
        
    }
    public void ReleaseCharge()
    {
        isCharging = false;
        chargeTime = 0;
        //chargeVisual.fillAmount = chargeTime;
        if (chargedProjectile.activeSelf == true)
        {
            chargedProjectile.SetActive(false);
        }
    }

}
