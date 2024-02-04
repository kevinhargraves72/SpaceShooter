using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float rotSpeed = 180f;

    float shipBoundaryRadius = 0.5f; // size of shit to offset when it hits boundary
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate the ship

        //Grab rotation quaternion
        Quaternion rot = transform.rotation;
        //Grab z euler angle
        float z = rot.eulerAngles.z;
        //Change the Z angle based on input
        z -= Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        //recreate quaternion
        rot = Quaternion.Euler(0, 0, z);
        //Feed quaternion into rotation
        transform.rotation = rot;

        //MOVE the ship
        Vector3 pos = transform.position;

        Vector3 velocity = new Vector3(0, Input.GetAxis("Vertical") * maxSpeed * Time.deltaTime, 0);

        pos += rot * velocity;

        //RESTRICT the player to the camera's bounds
        if(pos.y + shipBoundaryRadius > Camera.main.orthographicSize)
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
}
