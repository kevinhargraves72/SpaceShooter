using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FormationHandler : MonoBehaviour
{
    public GameObject[] formationGroup;
    public float followBuffer = 0.5f;

    public int maxFormationLength;
    public float formationSearchRange;

    private int formationPosition;
    private int leaderPosition = 0;
    private bool isLeader;

    private bool preset = false;

    private LayerMask _enemyLayerMask;

    Color _gizmoIdleColor = Color.yellow;
    Color _gizmoDetectedColor = Color.red;
    void Start()
    {
        if (formationGroup.Length > 1)
        {
            formationPosition = FindFormationPosition();
            isLeader = IsLeader();
            preset = true;
        }
        else//this part is proabably not needed yet, to be possibly used with formation finding drones
        {
            formationPosition = 0;
            isLeader = true;
            formationGroup = new GameObject[maxFormationLength];
            formationGroup[0] = this.gameObject;
        }
        _enemyLayerMask = LayerMask.GetMask("Enemy");
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
                leaderPosition = i;
                if (IsLeader()) { isLeader = true; }
                break;
            }
            else
            {
                --formationPosition;
            }
        }
    }

    public int GetFormationPosition()
    {
        return formationPosition;
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

    public bool getPreset()
    {
        return preset;
    }

    public int GetCurrentFormationSize()
    {
        int size = 0;
        foreach(GameObject drone in  formationGroup)
        {
            if (drone != null)
            {
                size++;
            }
        }
        return size;
    }

    public float DistanceToPlayer(Vector3 enemyPosition)
    {
        return Vector3.Distance(enemyPosition, this.GetComponent<PlayerDetector>().GetPlayerPosition());
    }

    private Collider2D[] DetectFormationInRange()
    {
        Collider2D[] colliders = new Collider2D[maxFormationLength - GetCurrentFormationSize()];
        int index = 0;
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, formationSearchRange, _enemyLayerMask))
        {
            if(index < colliders.Length)
            {
                if (collider.GetComponent<FormationHandler>() != null)
                {
                    if (collider.GetComponent<FormationHandler>().getPreset() == false && collider.GetComponent<FormationHandler>().GetCurrentFormationSize() <= colliders.Length - index) //checks theres enough room in colliders for all drones in this ones formation (subtracts the index to account for any drones added so far)
                    {
                        if(collider.GetComponent<FormationHandler>().GetCurrentFormationSize() < GetCurrentFormationSize())
                        {
                            foreach (GameObject drone in collider.GetComponent<FormationHandler>().formationGroup)
                            {
                                if (drone != null)
                                {
                                    colliders[index] = drone.GetComponent<Collider2D>();
                                    index++;
                                }
                            }
                            //colliders[index] = collider;
                            //index++;
                            //Adding to the formation needs to be done differently from this to add all the drones in the added drones formation
                        }
                        else if(collider.GetComponent<FormationHandler>().GetCurrentFormationSize() == GetCurrentFormationSize())
                        {
                            if(DistanceToPlayer(this.transform.position) < DistanceToPlayer(collider.transform.position)) //Could this have issues because playerdetector.getplayerposition returns (0,0,0) if the player == null? (probably shouldnt cuz of how drone states work)
                            {
                                foreach (GameObject drone in collider.GetComponent<FormationHandler>().formationGroup)
                                {
                                    if (drone != null)
                                    {
                                        colliders[index] = drone.GetComponent<Collider2D>();
                                        index++;
                                    }
                                }
                                //colliders[index] = collider;
                                //index++;
                            }
                            //If THIS drone is closer to the player then the drone / "collider" its checking, then add the formation to THIS ones array
                            //Make method to figure this out
                        }
                    }
                }
            }
            else
            {
                break;
            }
            
        }
        
        return colliders;
    }

    //ToDo:
    //Goiong to need a public function that the drones state machine can call that will call the function above and use its output
    //to create the new formation group and give all the drones the updated information they need
    //which may just be the new formation group array and offset (might need another offset for when the drone is not the leader)
}
