using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerStats playerStats;
    Rigidbody2D rb;

    bool canThrust = false;
    float shipBoundaryRadius = 0.5f; // size of shit to offset when it hits boundary
    Vector3 pos;
    
    private void Start()
    {
        playerStats = gameObject.GetComponent<Player>().GetPlayerStats();
        rb = this.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //Rotate the ship
        if (Input.GetAxis("Horizontal") != 0)
            Rotate(Input.GetAxis("Horizontal"));
        //MOVE the ship
        if (Input.GetAxis("Vertical") > 0)
            canThrust = true;
        if (Input.GetAxis("Vertical") == 0)
            canThrust = false;
        
    }
    private void FixedUpdate()
    {
        Thrust(canThrust);
        rb.velocity = transform.up.normalized * rb.velocity.magnitude;//TODO Toggle this for drifting?!?!?
    }

    void CheckBounds(Vector3 pos)
    {
        //RESTRICT the player to the camera's bounds
        if (pos.y + shipBoundaryRadius > Camera.main.orthographicSize)
        {
            pos.y = Camera.main.orthographicSize - shipBoundaryRadius;
        }
        if (pos.y - shipBoundaryRadius < -Camera.main.orthographicSize)
        {
            pos.y = -Camera.main.orthographicSize + shipBoundaryRadius;
        }
        //calculate ortho width based on screen ratio
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float widthOrtho = Camera.main.orthographicSize * screenRatio;

        if (pos.x + shipBoundaryRadius > widthOrtho)
        {
            pos.x = widthOrtho - shipBoundaryRadius;
        }
        if (pos.x - shipBoundaryRadius < -widthOrtho)
        {
            pos.x = -widthOrtho + shipBoundaryRadius;
        }
        transform.position = pos;
    }
    void Thrust(bool thrust)
    {
       if (thrust)
       {
           rb.AddForce(transform.up * playerStats.maxSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
       }
       
       pos = transform.position;
       //Vector3 velocity = new Vector3(0, input * playerStats.maxSpeed * Time.deltaTime, 0);
       //pos += transform.rotation * velocity;
        //CheckBoundaries
       CheckBounds(pos);
    }
    void Rotate(float input)
    {
        //Grab rotation quaternion
        Quaternion rot = transform.rotation;
        //Grab z euler angle
        float z = rot.eulerAngles.z;
        //Change the Z angle based on input
        z -= input * playerStats.rotSpeed * Time.deltaTime;
        //recreate quaternion
        rot = Quaternion.Euler(0, 0, z);
        //Feed quaternion into rotation
        transform.rotation = rot;
    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
    }
}
