using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    Vector3 neededMove;
    public float positionDeadband = 1.05f;
    Vector3 goal;
    public float rotateSpeed = 20.0f;
    public float moveSpeed = 2.0f;
    int rotateDir = 1;// clockwise/counterclockwise
    bool goGo;

    void Start()
    {
        goGo = false;
    }

    void Update()
    {
        if (goGo)
        {
            neededMove = goal - this.transform.position;
            if (neededMove.magnitude <= positionDeadband)
            {
                goGo = false;
                return;
            }
            float angle = Vector3.Angle(neededMove, this.transform.forward);
            if (angle >= 5)
            {
                this.transform.Rotate(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
                float checkAngle = Vector3.Angle(neededMove, this.transform.forward);
                if (angle < checkAngle)
                {
                    rotateDir = rotateDir * -1;
                }

            }
            else
            {
                this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
    }
    public void Move(Vector3 position)
    {
        goal = position;
        goGo = true;
        
    }
}

