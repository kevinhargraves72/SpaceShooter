using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryFire : MonoBehaviour
{
    [SerializeField] GameObject[] bulletPrefabs;
    [SerializeField] GameObject[] bulletSpawnPoints;
    [SerializeField] GameObject chargedProjectile;
    Transform[] shotPoints;
    float cooldownTimer = 0;

    float chargeTime;
    [SerializeField] float maxChargeTime = 2f;
    
    bool isCharging;
    bool isChargeShotActive = false;
    GameObject chargeShot;

    float chargeShotActiveTime = 3; //make max active time into stat variable then use this to fill up when enemy hit, toward max
   
    private void Update()
    {
        if (cooldownTimer > 0)//if there is time left on timer keep ticking it down
        {
            cooldownTimer -= Time.deltaTime;
        }
        if(isCharging == true && chargeTime < maxChargeTime)
        {
            chargeTime += Time.deltaTime;
        } else if(isCharging == true && chargeTime >= maxChargeTime)
        {
            ChargeShot();
        }

    }
    public void Shoot(float atk, float fireDelay, int spawnIndex, int bulletFabIndex)//take care of shooting
    {
        if(cooldownTimer <= 0)
        {
            cooldownTimer = fireDelay;//set shot cooldown timer 
            shotPoints = bulletSpawnPoints[spawnIndex].GetComponentsInChildren<Transform>();

            for(int i=1; i < shotPoints.Length; i++)
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
            if(isChargeShotActive == false)
            {
                chargeShot = Instantiate(chargedProjectile, shotPoints[1].position, shotPoints[1].rotation);
                isChargeShotActive = true;
            }
            else
            {
                chargeShot.transform.position = shotPoints[1].position;
                chargeShot.transform.rotation = shotPoints[1].rotation;
            }
            Debug.Log(chargeShotActiveTime);
            chargeShotActiveTime -= Time.deltaTime;
        }
        else
        {
            ReleaseCharge();
        }
        
    }
    public void Charge()
    {
        isCharging = true;
    }
    public void ReleaseCharge()
    {
        isCharging = false;
        chargeTime = 0;
        if(chargeShot != null)
        {
            Destroy(chargeShot);
        }
    }

}
