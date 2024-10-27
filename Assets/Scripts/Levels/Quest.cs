using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    public event EventHandler OnQuestCompleted;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void InvokeOnQuestCompleted()
    {
        OnQuestCompleted(this, EventArgs.Empty);
    }
}
