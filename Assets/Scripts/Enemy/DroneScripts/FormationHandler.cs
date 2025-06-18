using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationHandler : MonoBehaviour
{
    public GameObject[] formationGroup;
    public float followBuffer = 1000f;
    
    private int formationPosition;
    private int leaderPosition = 0;
    private bool isLeader;
    void Start()
    {
        if (formationGroup.Length > 1)
        {
            formationPosition = FindFormationPosition();
            isLeader = IsLeader();
        }
        else//this part is proabably not needed yet, to be possibly used with formation finding drones
        {
            formationPosition = 0;
            isLeader = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLeader)
        {
            if (!LeaderCheck())//if leader was destroyed
            {
                LeaderChange();
            }
        }
    }

    private int FindFormationPosition()
    {
        for (int i = 0; i < formationGroup.Length; i++)
        {
            if(formationGroup[i] == this.gameObject)
            {
                return i;
            }
        }
        return 0; //this should never actually return because the list should always have the object in it
                   // I guess having it return 0 could be handy tho if the list is only the current drone? idk
    }

    public bool IsLeader()
    {
        if (formationGroup[leaderPosition] == this.gameObject)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool LeaderCheck()//returns true if leader is alive (not null) and false if it was destroyed (null)
    {
        if (formationGroup[leaderPosition] == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void LeaderChange()
    {
        for(int i = leaderPosition; i < formationGroup.Length; i++)
        {
            if (formationGroup[i] != null)
            {
                --formationPosition;
                leaderPosition = i;
                if (IsLeader()) { isLeader = true; }
            }
            else
            {
                --formationPosition;
            }
        }
    }

    public IState LeaderCurrentState()
    {
        return formationGroup[leaderPosition].GetComponent<Enemy>().GetCurrentState();
    }

    private Vector3 CalculateTrueOffset(Vector2 offset)
    {
        return (offset.y * formationPosition * (formationGroup[leaderPosition].GetComponent<Transform>().up)) + (offset.x * formationPosition * (formationGroup[leaderPosition].GetComponent<Transform>().right));
    } 

    public Vector3 CalculateFormationFollowTarget(Vector2 offset)
    {
        return CalculateTrueOffset(offset) + formationGroup[leaderPosition].GetComponent<Transform>().position;
    }

    public Vector3 GetLeaderUp()
    {
        return formationGroup[leaderPosition].GetComponent<Transform>().up;
    }
}
