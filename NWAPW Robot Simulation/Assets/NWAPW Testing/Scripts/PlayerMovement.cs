using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1;
    public bool run;
   // private Rigidbody rb;

    void Start()
    {

        //rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //if (run)
        //{
            if (Input.GetKey("right"))
            {
                transform.Rotate(0, 1.5f, 0);
            }
            else if (Input.GetKey("left"))
            {
                transform.Rotate(0, -1.5f, 0);
            }
            if (Input.GetKey("up"))
            {
                transform.position += transform.forward * 0.3f;
            }
            else if (Input.GetKey("down"))
            {
                transform.position -= transform.forward * 0.3f;

            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<GrabRelease>().Grab();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<GrabRelease>().Release();
            }
            //more toss
            if (Input.GetKeyDown(KeyCode.S)) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<GrabRelease>().Toss();

            }
        //}
    }
}
