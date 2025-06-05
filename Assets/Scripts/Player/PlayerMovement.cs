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
    bool rightDrift = false;
    bool leftDrift = false;
    float canControl = 0;
    float shipBoundaryRadius = 0.5f; // size of shit to offset when it hits boundary

    
    private void Start()
    {
        playerStats = gameObject.GetComponent<Player>().GetPlayerStats();
        rb = this.GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        canControl = 1;
    }
    void Update()
    {
        //if (Input.GetAxis("Vertical") == 1)//cannot thrust while drifting        //&& !isDrifting (these are to stop thrust while drifting)
        //    canThrust = true;
        //if (Input.GetAxis("Vertical") != 1)                                     // || isDrifting
        //    canThrust = false;
        ////If player does leftShift input turns is drifting to true
        if (Input.GetButton("Ability2"))
        {
            isDrifting = true;
            if(Input.GetAxis("Horizontal") > 0)
            {
                rightDrift = true;
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                leftDrift = true;
            }
        }
        else
        {
            isDrifting = false;
            
        }
    }
    private void FixedUpdate()
    {
        if (canControl <= 0)
        {
            if (Input.GetAxis("Horizontal") != 0)
                if (!isDrifting)
                {
                    Rotate(Input.GetAxis("Horizontal"), playerStats.rotSpeed);
                }
                else
                {
                    Rotate(Input.GetAxis("Horizontal"), playerStats.rotSpeed / 2f);
                }

            drift();
            Thrust(Input.GetAxis("Vertical"));
            
            //Debug.Log(Input.GetAxis("Vertical"));
            //Debug.Log("velocity: " + rb.velocity + "transform.up.normalized: " + transform.up.normalized + "velocity.magnitude" + rb.velocity.magnitude);
            //TODO Toggle this for drifting?!?!?
            //Make this only run if isDrifting == false, otherwise it runs
        }
        else
        {
            canControl -= Time.deltaTime;
        }
        CheckBounds(transform.position);


    }
    void drift()
    {
        if (!isDrifting)
        {
            rb.velocity = transform.up.normalized * rb.velocity.magnitude; // If you dont have the the movement feels wayyy more normal physics based, probably interesting to play around with
        }
        if (isDrifting)
        {
            if (rightDrift && !leftDrift)
            {
                rb.velocity = -(transform.right).normalized * rb.velocity.magnitude;
            }
            if (leftDrift && !rightDrift)
            {
                rb.velocity = (transform.right).normalized * rb.velocity.magnitude;
            }
            //rb.velocity = (transform.up - transform.right * Input.GetAxis("Horizontal")).normalized * rb.velocity.magnitude;
            //rb.velocity = -(transform.right * Input.GetAxis("Horizontal")).normalized * rb.velocity.magnitude;
        }
    }

    void CheckBounds(Vector3 pos)
    {
        //RESTRICT the player to the camera's bounds
        if (pos.y + shipBoundaryRadius > Camera.main.orthographicSize + Camera.main.transform.position.y)
        {
            pos.y = Camera.main.orthographicSize + Camera.main.transform.position.y - shipBoundaryRadius;
            transform.position = pos;
        }
        if (pos.y - shipBoundaryRadius < -Camera.main.orthographicSize + Camera.main.transform.position.y)
        {
            pos.y = -Camera.main.orthographicSize + Camera.main.transform.position.y + shipBoundaryRadius;
            transform.position = pos;
        }
        //calculate ortho width based on screen ratio
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float widthOrtho = Camera.main.orthographicSize * screenRatio;

        if (pos.x + shipBoundaryRadius > widthOrtho + Camera.main.transform.position.x)
        {
            pos.x = widthOrtho + Camera.main.transform.position.x - shipBoundaryRadius;
            transform.position = pos;
        }
        if (pos.x - shipBoundaryRadius < -widthOrtho + Camera.main.transform.position.x)
        {
            pos.x = -widthOrtho + Camera.main.transform.position.x + shipBoundaryRadius;
            transform.position = pos;
        }
        
    }
    void Thrust(float input)
    {
       if (input == 1)
       {
            if (!isDrifting)
            {
                rb.AddForce(transform.up * playerStats.maxSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(-(transform.right * Input.GetAxis("Horizontal")).normalized * playerStats.maxSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
           
       }
       
        //Vector3 velocity = new Vector3(0, input * playerStats.maxSpeed * Time.deltaTime, 0);
        //pos += transform.rotation * velocity;
        //CheckBoundaries
        

    }

    void Rotate(float input, float rotSpeed)
    {
        rb.AddTorque(-input * rotSpeed, ForceMode2D.Impulse);
        Debug.Log(rb.angularVelocity);
    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
    }
}
