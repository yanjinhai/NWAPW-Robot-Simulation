using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal") *5;
        float v = Input.GetAxis("Vertical") * 5;
        Vector3 vel = rb.velocity;
        vel.x = h;
        vel.z = v;
        rb.velocity = vel;
    }
}
