using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Hopefully this causes a merge error and screws up camera

public class RobotMovement : MonoBehaviour
{
    Vector3 targetPos;
    Vector3 prevTargetPos;
    Vector3 relativePos;
    public float positionDeadband = 1.05f;
    public float basketDistLim;
    public float rotateSpeed = 50.0f;
    public float moveSpeed = 5.0f;
    public bool isMoving;
    public bool run;
    public bool needsToGoBack = true;
    bool goingBack = false;
    void Start()
    {
        isMoving = true;
        basketDistLim = 12.224f;
    }

    void Update()
    {
        float relativeAngle;
        float relativeRotationDir;
        if (isMoving && run)
        {
            relativePos = targetPos - this.transform.position;
            if (relativePos.magnitude <= positionDeadband || (GetComponent<RobotAI>().targetIsBasket && Mathf.Abs(relativePos.magnitude - basketDistLim) <= 1.0f))
            {
                if (GetComponent<RobotAI>().targetIsBasket && Mathf.Abs(relativePos.magnitude - basketDistLim) <= 1.0f || goingBack)
                {
                    print("In range now");
                    print(prevTargetPos);
                    //Shoot in the right direction
                    relativeAngle = Vector3.SignedAngle(prevTargetPos - this.transform.position, this.transform.forward, this.transform.up);
                    print(relativeAngle);
                    if (!(Mathf.Abs(relativeAngle) > 1))
                    {
                        print("Shooting!");
                        isMoving = false;
                        goingBack = false;
                    }
                    else
                    {
                        print("needs rotate :)"); 
                    }  
                }
                else
                {
                    isMoving = false;
                    goingBack = false;
                }
            }
            else if(GetComponent<RobotAI>().targetIsBasket && relativePos.magnitude < basketDistLim && needsToGoBack)
            {
                print("going back");
                Vector3 tempPos = targetPos;
                prevTargetPos = targetPos;
                targetPos.Normalize();
                targetPos = tempPos - targetPos * basketDistLim * 1.1f;
                targetPos.y = .5f;
                needsToGoBack = false;
                goingBack = true;
                //GetComponent<RobotAI>().targetIsBasket = false;
            }
            relativePos = targetPos - this.transform.position;

            relativeAngle = Vector3.SignedAngle(relativePos, this.transform.forward, this.transform.up);
            relativeRotationDir = relativeAngle / (Mathf.Abs(relativeAngle));
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
        if(!goingBack) targetPos = position;
        isMoving = true;

    }
}

