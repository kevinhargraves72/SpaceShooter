using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //All values besides enemy are given by "PlayerShooting"
    float damage;
    DamageHandler enemy;

    bool piercing = false;
    int maxPierce;
    int numPierced = 0;
    public void setDamage(float dmg)
    {
        damage = dmg;
    }
    public void setPierce(int max)
    {
        if(max > 0)
        {
            piercing = true;
            maxPierce = max;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy = collision.GetComponent<DamageHandler>();

        if(enemy != null)//object can take gamage give it to them
        {
            enemy.takeDamage(damage);
            if(piercing == true && numPierced < maxPierce)//count up piercing if true
            {
                numPierced++;
            }
            else//destroy object if piercing false or reached limit
            {
                Destroy(gameObject);
            }

        }
    }
}
