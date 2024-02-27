using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float health;
    [SerializeField] float defense = 0f; //takes percentage off incoming damage
    float defReduction; //makes defense into decimal to multipy incoming gamage by (def = 25 defRed = .75)
    [SerializeField] int expAmount = 0; //amount of exp given when killed

    [SerializeField] float invulnPeriod = 0;
    float invulnTimer = 0;
    int correctLayer;
    float invulnAnimTimer = 0.1f;

    SpriteRenderer spriteRend;

    private void Start()
    {
        SetHealth(maxHealth);
        setDefense(defense);//calculate defRed at start
        correctLayer = gameObject.layer;//get starting layer for invuln (invuln switches layers)
        spriteRend = getSpriteRend();//some enemies have their sprites fliped which needs this to fix (sprite is fliped in childComp)
    }
    private void Update()
    {
        if (health <= 0) //keep checkin if this mf needs to be killed
        {
            Die();
        }

        if (invulnTimer > 0) //keep checking for if invulnerablity has been given (happens in take damage)
        {
            invulnerable();
        }
    }
    public void takeDamage(float dmg)
    {
        health -= (dmg * defReduction); //take damage minus defense 

        if (health <= 0)
        {
            Die();
        }

        if (invulnPeriod > 0) //start invulnerability period
        {
            invulnTimer = invulnPeriod;
            gameObject.layer = 8;//INVULN LAYER CHANGE
        }
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


    void Die()
    {
        if(expAmount > 0)
        {
            GameMaster.Instance.playerData.levelSystem.AddExperience(expAmount);
        }
        Destroy(gameObject);
    }
}
