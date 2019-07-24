using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{

    // Variables, some with default values
    Vector3 targetPos;
    Vector3 relativePos;
    public float positionDeadband = 0.05f;
    public float rotateSpeed = 50.0f;
    public float moveSpeed = 5.0f;
    public bool isMoving;

    // Run is used to swap between teleop and auto
    public bool run;

    void Start()
    {

        // Robot set to be "moving" intialy to properly go through if statements
        isMoving = true;
    }

    void Update()
    {

        // Runs if the robot is set to auto and has not reached its target
        if (isMoving && run)
        {

            // Checks whether the robot is within the deadband this varies by target
            relativePos = targetPos - this.transform.position;
            if (relativePos.magnitude <= positionDeadband)
            {

                // Sets the robot to not moving and ends the update
                isMoving = false;
                return;
            }

            // Checks if the robot is pointing at the target
            float relativeAngle = Vector3.SignedAngle(relativePos, this.transform.forward, this.transform.up);
            float relativeRotationDir = relativeAngle / (Mathf.Abs(relativeAngle));
            if (Mathf.Abs(relativeAngle) > 1)
            {

                // Rotates the robot towards the target
                this.transform.Rotate(0, rotateSpeed * Time.deltaTime * relativeRotationDir * -1, 0);
            }

            // Else the robot is pointing at the target
            else
            {

                // Moves the robot forward, toward the target
                this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
    }

    // Move function which is used as an interface by the AI
    public void Move(Vector3 position, float deadBand)
    {

        // Sets the target, sets the robot to moving, and sets the deadband
        targetPos = position;
        isMoving = true;
        positionDeadband = deadBand;
    }
}

