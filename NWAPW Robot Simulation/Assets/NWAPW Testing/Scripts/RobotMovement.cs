﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Hopefully this causes a merge error and screws up camera

public class RobotMovement : MonoBehaviour
{
    Vector3 neededMove;
    public float positionDeadband = 1.05f;
    Vector3 targetPos;
    public float rotateSpeed = 50.0f;
    public float moveSpeed = 5.0f;
    int rotateDir = 1;// clockwise/counterclockwise
    public bool goGo;

    void Start()
    {
        goGo = true;
    }

    void Update()
    {
        if (goGo)
        {
            neededMove = targetPos - this.transform.position;
            if (neededMove.magnitude <= positionDeadband)
            {
                goGo = false;
                return;
            }
            float angle = Vector3.Angle(neededMove, this.transform.forward);
            if (angle >= 1)
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
        targetPos = position;
        goGo = true;
    }
}

