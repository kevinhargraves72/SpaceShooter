using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerStats playerStats;
    Rigidbody2D rb;

    bool canThrust = false;
    bool isDrifting = false;
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
            if (!isDrifting)
            {
                Rotate(Input.GetAxis("Horizontal"), playerStats.rotSpeed);
            }
            else
            {
                Rotate(Input.GetAxis("Horizontal"), playerStats.rotSpeed / 1.5f);
            }
            
        //MOVE the ship
        if (Input.GetAxis("Vertical") == 1)//cannot thrust while drifting        //&& !isDrifting (these are to stop thrust while drifting)
            canThrust = true;
        if (Input.GetAxis("Vertical") != 1)                                     // || isDrifting
            canThrust = false;
        //If player does leftShift input turns is drifting to true
        if (Input.GetButton("Ability2"))
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }
    }
    private void FixedUpdate()
    {
        
        if (!isDrifting)
        {
            rb.velocity = transform.up.normalized * rb.velocity.magnitude;
        }
        else if(canThrust)
        {
            //rb.velocity = (transform.up - transform.right * Input.GetAxis("Horizontal")).normalized * rb.velocity.magnitude;
            if(Input.GetAxis("Horizontal") != 0)
            {
                rb.velocity = -(transform.right * Input.GetAxis("Horizontal")).normalized * rb.velocity.magnitude;
            }
            
        }
        Thrust(canThrust);
        //Debug.Log(Input.GetAxis("Vertical"));
        //Debug.Log("velocity: " + rb.velocity + "transform.up.normalized: " + transform.up.normalized + "velocity.magnitude" + rb.velocity.magnitude);
        //TODO Toggle this for drifting?!?!?
        //Make this only run if isDrifting == false, otherwise it runs
    }

    void CheckBounds(Vector3 pos)
    {
        //RESTRICT the player to the camera's bounds
        if (pos.y + shipBoundaryRadius > Camera.main.orthographicSize + Camera.main.transform.position.y)
        {
            pos.y = Camera.main.orthographicSize + Camera.main.transform.position.y - shipBoundaryRadius;
        }
        if (pos.y - shipBoundaryRadius < -Camera.main.orthographicSize + Camera.main.transform.position.y)
        {
            pos.y = -Camera.main.orthographicSize + Camera.main.transform.position.y + shipBoundaryRadius;
        }
        //calculate ortho width based on screen ratio
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float widthOrtho = Camera.main.orthographicSize * screenRatio;

        if (pos.x + shipBoundaryRadius > widthOrtho + Camera.main.transform.position.x)
        {
            pos.x = widthOrtho + Camera.main.transform.position.x - shipBoundaryRadius;
        }
        if (pos.x - shipBoundaryRadius < -widthOrtho + Camera.main.transform.position.x)
        {
            pos.x = -widthOrtho + Camera.main.transform.position.x + shipBoundaryRadius;
        }
        transform.position = pos;
    }
    void Thrust(bool thrust)
    {
       if (thrust)
       {
            if (!isDrifting)
            {
                rb.AddForce(transform.up * playerStats.maxSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            else if (Input.GetAxis("Horizontal") == 1 || Input.GetAxis("Horizontal") == -1)
            {
                rb.AddForce(-(transform.right * Input.GetAxis("Horizontal")).normalized * playerStats.maxSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
           
       }
       
       pos = transform.position;
       //Vector3 velocity = new Vector3(0, input * playerStats.maxSpeed * Time.deltaTime, 0);
       //pos += transform.rotation * velocity;
        //CheckBoundaries
       CheckBounds(pos);
    }
    void Rotate(float input, float rotSpeed)
    {
        //Grab rotation quaternion
        Quaternion rot = transform.rotation;
        //Grab z euler angle
        float z = rot.eulerAngles.z;
        //Change the Z angle based on input
        z -= input * rotSpeed * Time.deltaTime;
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
