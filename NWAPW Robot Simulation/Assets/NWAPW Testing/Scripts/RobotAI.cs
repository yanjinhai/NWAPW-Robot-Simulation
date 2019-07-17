﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{

    private bool isHoldingCollectableObject;
    

    void Start()
    {
        isHoldingCollectableObject = false;
    }

    /*
    void CalculateRoute(Vector3 targetPos) {
        RaycastHit hitInfo;

        float relativeDistance = (targetPos - this.transform.position).magnitude;

        Vector3 relativePos = targetPos - this.transform.position;

        Physics.Raycast(transform.position, relativePos, out hitInfo);

        Debug.DrawRay(transform.position, relativePos, Color.red);

        if (hitInfo.distance < relativeDistance - 0.5)
        {
            if (hitInfo.transform.tag != "CollectableObject")
            {
                Transform obstacle = hitInfo.transform;
                Quaternion angle = this.transform.rotation;
                while (hitInfo.transform.Equals(obstacle)) {
                    Physics.Raycast(transform.position, angle.eulerAngles, out hitInfo);
                    Vector3 newAngle = angle.eulerAngles;
                    newAngle.y += 5;
                    angle.eulerAngles = newAngle;
                }
                Debug.DrawRay(transform.position, angle.eulerAngles, Color.green);
                //hitInfo.collider.bounds;
                //hitInfo.transform.localScale.x;
                //hitInfo.transform.localScale.z;
            }
        }
    }
    */
    Vector3 FindNearest(GameObject[] gameObjects) {
        Vector3 closestPosition = gameObjects[0].transform.position;
        float shortestDistance = (gameObjects[0].transform.position - this.transform.position).magnitude;
        foreach (GameObject obj in gameObjects) {
            Vector3 currPos = obj.transform.position;

            float relativeDistance = (currPos - this.transform.position).magnitude;

//            CalculateRoute(currPos);

            if (relativeDistance < shortestDistance) {
                shortestDistance = relativeDistance;
                closestPosition = currPos;
            }
        }
            
        return closestPosition;
    }

    void FixedUpdate()
    {
        if (!isHoldingCollectableObject) {

            GameObject[] collectableObjects = GameObject.FindGameObjectsWithTag("CollectableObject");

            Vector3 targetPos = FindNearest(collectableObjects);

            Move(targetPos);

            if (!gameObject.GetComponent<RobotMovement>().goGo) {
                Grab();
            }
        }
    }

    void Move(Vector3 position) {
        gameObject.GetComponent<RobotMovement>().Move(position);
    }

    void Grab() {
        if (isHoldingCollectableObject) {
            return;
        }
        isHoldingCollectableObject = true;

        // Unfinished. Needs to actually pick up object.

    }

    void Release() {
    }
}
