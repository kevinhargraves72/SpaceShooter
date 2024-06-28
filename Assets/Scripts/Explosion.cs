using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    float Atk;
    [SerializeField] float duration = 2f;
    DamageHandler enemy;

    public void SetAtk(float atk) { Atk = atk; }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy = collision.GetComponent<DamageHandler>();

        if (enemy != null)
        {
            enemy.takeDamage(Atk);
        }
    }
}
