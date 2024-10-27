using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assasination : Quest
{
    public enum AssasinationStatus
    {
        Alive,
        KIA
    }

    public List<Enemy> EnemiesToAssassinate;
    private Dictionary<string, AssasinationStatus> BingoBook;

    void Start()
    {
        BingoBook = new Dictionary<string, AssasinationStatus>();
        foreach (Enemy enemy in EnemiesToAssassinate)
        {
            //GameObject enemyInstance
            if (enemy != null)
            {
                BingoBook.Add(enemy.gameObject.name, AssasinationStatus.Alive);
                enemy.wantedStatus = Enemy.WantedStatus.Wanted;
                enemy.DamageHandler.OnDeath += AssasinationQuest_OnEnemyDeath;
            }
        }
    }

    void Update()
    {
        
    }

    protected override void InvokeOnQuestCompleted()
    {
        base.InvokeOnQuestCompleted();
    }

    private void AssasinationQuest_OnEnemyDeath(GameObject victim)
    {
        BingoBook[victim.name] = AssasinationStatus.KIA; // mark enemy KIA

        // Remove enemy from enemies to assasinate list
        Enemy enemyToRemove = victim.GetComponent<Enemy>();
        EnemiesToAssassinate.Remove(enemyToRemove);

        // Destroy enemy game object
        Destroy(victim);

        if (EnemiesToAssassinate.Count ==  0)
        {
            InvokeOnQuestCompleted();
        }
    }
}
