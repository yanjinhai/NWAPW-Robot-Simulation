using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    public Vector3 neededMove;
    public Vector3 goal= new Vector3(0.0f,0.5f,0.0f);
    public float rotateSpeed = 20.0f;
    public int rotateDir = 1;
    public float moveSpeed = 2.0f;
    public bool goGo = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goGo)
        {
            neededMove = goal - transform.position;
            if (neededMove.magnitude <= .5)
            {
                goGo = false;
                return;
            }
            float angle = Vector3.Angle(neededMove, transform.forward);
            if (angle >= 5)
            {
                transform.Rotate(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
                float checkAngle = Vector3.Angle(neededMove, transform.forward);
                if (angle < checkAngle)
                {
                    rotateDir = rotateDir * -1;
                }

            }
            else
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
    }
    public void Move(Vector3 position)
    {
        goal = position;
        goGo = true;
        
    }
}

