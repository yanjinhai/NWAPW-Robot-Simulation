using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Hopefully this causes a merge error and screws up camera

public class RobotMovement : MonoBehaviour
{
    Vector3 targetPos;
    Vector3 relativePos;
    public float positionDeadband = 1.05f;
    public float rotateSpeed = 50.0f;
    public float moveSpeed = 5.0f;
    public bool isMoving;

    void Start()
    {
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            relativePos = targetPos - this.transform.position;
            if (relativePos.magnitude <= positionDeadband)
            {
                isMoving = false;
                return;
            }

            float relativeAngle = Vector3.SignedAngle(relativePos, this.transform.forward, this.transform.up);
            float relativeRotationDir = relativeAngle / (Mathf.Abs(relativeAngle));
            if (Mathf.Abs(relativeAngle) > 1)
            {
                this.transform.Rotate(0, rotateSpeed * Time.deltaTime * relativeRotationDir * -1, 0);
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
        isMoving = true;
    }
}

