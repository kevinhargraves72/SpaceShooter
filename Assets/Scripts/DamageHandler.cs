using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public int health = 1;

    public float invulnPeriod = 1f;
    private float invulnTimer = 0;
    private int correctLayer;
    private float invulnAnimTimer = 0.1f;

    SpriteRenderer spriteRend;

    private void Start()
    {
        correctLayer = gameObject.layer;

        spriteRend = GetComponent<SpriteRenderer>();

        if(spriteRend == null)
        {
            spriteRend = transform.GetComponentInChildren<SpriteRenderer>();

            if(spriteRend == null)
            {
                Debug.LogError("Object '"+gameObject.name+"' has no sprite renderer");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health--;
        
        if(invulnPeriod > 0)
        {
            invulnTimer = invulnPeriod;
            gameObject.layer = 8;
        }
    }

    private void Update()
    {
        if(invulnTimer > 0)
        {
            invulnerable();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void invulnerable()
    {
        invulnTimer -= Time.deltaTime;

        if(invulnTimer <= 0)
        {
            gameObject.layer = correctLayer;
            if(spriteRend != null)
            {
                spriteRend.enabled = true;
            }
        }
        else
        {
            if(spriteRend != null && invulnAnimTimer <= 0)
            {
                spriteRend.enabled = !spriteRend.enabled;
                invulnAnimTimer = 0.1f;
            }
            invulnAnimTimer -= Time.deltaTime;
        }
    }


    void Die()
    {
        Destroy(gameObject);
    }
}
