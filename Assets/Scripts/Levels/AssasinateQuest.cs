using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assasination : Quest
{
    [SerializeField] Enemy[] EnemiesToAssassinate;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Enemy enemy in EnemiesToAssassinate)
        {
            //enemy.DamageHandler.OnDeath += 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void AssasinationQuest_OnEnemyDeath(object sender, System.EventArgs e)
    {

    }
}
