using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacesPlayer : MonoBehaviour
{
    public float rotSpeed = 90f;

    Transform player;
    void Update()
    {
        if(player == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player"); //Find player ship

            if(go != null)
            {
                player = go.transform;
            }
        }
        //At this point, player is either found or does not exist right now
        if (player == null)
            return;//try again next frame

        //Here -- we know for sure we have a player. Turn to face it
        face();
    }

    private void face()
    {
        Vector3 dir = player.position - transform.position;
        dir.Normalize();

        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }
}
