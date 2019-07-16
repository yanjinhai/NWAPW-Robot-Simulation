using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1;
    // Start is called before the first frame update
   // private Rigidbody rb;

    void Start()
    {

        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        float h = Input.GetAxis("Horizontal") *5;
        float v = Input.GetAxis("Vertical") * 5;
        Vector3 vel = rb.velocity;
        vel.x = h;
        vel.z = v;
        rb.velocity = vel;*/
        if (Input.GetKey("right"))
        {
            transform.Rotate(0, 1.5f, 0);
        }
        else if (Input.GetKey("left")) {
            transform.Rotate(0,-1.5f, 0);
        }
        if (Input.GetKey("up"))
        {
            transform.position += transform.forward * 0.3f;
        }
        else if (Input.GetKey("down")) {
            transform.position -= transform.forward * 0.3f;

        }
    }
}
