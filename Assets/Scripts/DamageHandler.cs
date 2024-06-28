using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float health;
    [SerializeField] float defense = 0f; //takes percentage off incoming damage
    float defReduction; //makes defense into decimal to multipy incoming gamage by (def = 25 defRed = .75)
    //[SerializeField] int expAmount = 0; //amount of exp given when killed

    [SerializeField] float invulnPeriod = 0;
    float invulnTimer = 0;
    int correctLayer;
    float invulnAnimTimer = 0.1f;

    Color shieldColor = new Color(0f, 0f, 1f, 0.3f);//Apply this to sprite that is just barly a little bigger then the normal one, when hit make A = 1 then tick back down to 0 (cannot be seen)
    [SerializeField] float shieldAlpha = 0f;
    SpriteRenderer shieldRenderer;
    
    [SerializeField] float maxSHP = 0;
    float shieldHP = 0;
    [SerializeField] float rechargeDelay = 0;
    float rechargeTime = 0;
    [SerializeField] float rechargeRate = 0;

    SpriteRenderer spriteRend;

    public event Action<float> OnDamage;
    public event Action OnDeath;

    private void Start()
    {
        SetHealth(maxHealth);
        setDefense(defense);//calculate defRed at start
        correctLayer = gameObject.layer;//get starting layer for invuln (invuln switches layers)
        spriteRend = getSpriteRend();//some enemies have their sprites fliped which needs this to fix (sprite is fliped in childComp)
        shieldHP = maxSHP;
        if(shieldHP > 0)
        {
            shieldRenderer.color = shieldColor;
        }
    }
    public void SetAllDamageHandler(float _maxHealth, float _defense, float _maxSHP, float _rechargeDelay, float _rechargeRate)
    {
        SetMaxHealth(_maxHealth);
        setDefense(_defense);
        SetShield(_maxSHP, _rechargeRate, _rechargeDelay);
    }
    private void Update()
    {
        if (invulnTimer > 0) //keep checking for if invulnerablity has been given (happens in take damage)
        {
            invulnerable();
        }

        if(maxSHP > 0)//if object has shield
        {
            rechargeShield();
        }

        //TODO Possible make this its own method, not 100% sure what it does tho
        if(shieldAlpha > 0.1f && shieldHP > 0f)
        {
            shieldAlpha -= Time.deltaTime * 0.3f;
            shieldColor.a = shieldAlpha;
            shieldRenderer.color = shieldColor;
        }
    }
    public void takeDamage(float dmg)
    {
        if (shieldHP > 0)
        {
            //Debug.Log(shieldHP);
            damageShield(dmg);
            OnDamage(dmg);
        }
        else
        {
            damageHP(dmg);
        }
        rechargeTime = rechargeDelay;

    }

    public void damageHP(float dmg)
    {
        health -= (dmg * defReduction); //take damage minus defense 
        OnDamage(dmg * defReduction);

        if (health <= 0)
        {
            OnDeath();
        }

        if (invulnPeriod > 0) //start invulnerability period
        {
            invulnTimer = invulnPeriod;
            gameObject.layer = 8;//INVULN LAYER CHANGE
        }
        //TODO put this into enemy script, use death event to call it
        //if (expAmount > 0)//If enemy charge the players charge shot
        //{
        //    GameMaster.Instance.playerInstance.GetComponent<Player>().AddChargeShotActiveTime(dmg);
        //    GameMaster.Instance.playerInstance.GetComponent<EnergySteal>().HealDmg(dmg);
        //}
    }
    void rechargeShield()
    {
        if (rechargeTime > 0.0f)//if dmg was taken and reCtimer was set start ticking
        {
            rechargeTime -= Time.deltaTime;
            //Debug.Log(shieldColor.a);
            //Debug.Log(rechargeTime);
        }
        else if (shieldHP < maxSHP && rechargeTime <= 0.0f)
        {
            shieldHP += Time.deltaTime * rechargeRate;
            if (shieldColor.a == 0f)
            {
                shieldColor.a = 0.1f;
                shieldRenderer.color = shieldColor;
            }
        }
    }
    public void damageShield(float dmg)
    {
        if(shieldHP - dmg > 0)//If shield can take the damage without breaking
        {
            shieldHP -= dmg;//damage shield
            shieldAlpha = 1;
            shieldColor.a = shieldAlpha;
            shieldRenderer.color = shieldColor;
        }
        else
        {
            shieldBreak(dmg);
        }
        
    }

    public void shieldBreak(float dmg)
    {
        dmg -= shieldHP;//Take damage done to shield out of dmg
        shieldHP = 0;//break shield
        //Debug.Log(shieldHP);
        takeDamage(dmg);//give rest of damage to HP
        shieldColor.a = 0f;
        shieldRenderer.color = shieldColor;
    }

    public float GetShieldNormalized()
    {
        return shieldHP / maxSHP;
    }

    public void SetShield(float hp, float rate, float delay)
    {
        maxSHP = hp;
        shieldHP = hp;
        rechargeRate = rate;
        rechargeDelay = delay;
    }
    public float GetHealthNormalized()
    {
        return health / maxHealth;
    }
    public void Heal(float addHealth)
    {
        health += addHealth;
    }
    public void SetHealth(float Health)
    {
        health = Health;
    }
    public void SetMaxHealth(float maxhealth)
    {
        maxHealth = maxhealth;
        SetHealth(maxHealth);
    }
    void invulnerable()
    {
        invulnTimer -= Time.deltaTime;

        if (invulnTimer <= 0)//if invuln time is over
        {
            gameObject.layer = correctLayer; //INVULN LAYER CHANGE BACK
            if (spriteRend != null)//invuln anin stop
            {
                spriteRend.enabled = true;
            }
        }
        else//if invuln timer is still going
        {
            if (spriteRend != null && invulnAnimTimer <= 0)
            {
                spriteRend.enabled = !spriteRend.enabled; //flicker sprite every invulnAnimTimer seconds
                invulnAnimTimer = 0.1f;
            }
            invulnAnimTimer -= Time.deltaTime;
        }
    }
    public void setDefense(float def) //Setter for def ALSO does defRed calc
    {
        if (def <= 100f && def >= 0f)
        {
            defense = def;
            defReduction = (100f - defense) / 100f;
        }

    }

    //Different getters and setters (most def related)
    public void addDefense(float def)
    {
        if (defense + def <= 100f && defense + def >= 0f)
        {
            defense += def;
            defReduction = (100f - defense) / 100f;
        }
    }
    public float getDefense()
    {
        return defense;
    }
    public float getDefReduction()
    {
        return defReduction;
    }

    SpriteRenderer getSpriteRend() //again for enemy sprite problem (needed to be fliped)
    {
        if (GetComponent<SpriteRenderer>() == null)
        {
            if (transform.GetComponentInChildren<SpriteRenderer>() == null)
            {
                Debug.LogError("Object '" + gameObject.name + "' has no sprite renderer");
            }
            return transform.GetComponentInChildren<SpriteRenderer>();
        }
        else
        {
            return GetComponent<SpriteRenderer>();
        }
    }


   //void Die()
   //{
   //    if(expAmount > 0)
   //    {
   //        GameMaster.Instance.playerData.levelSystem.AddExperience(expAmount);
   //    }
   //    Destroy(gameObject);
   //}
}
