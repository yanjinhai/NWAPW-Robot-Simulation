using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{

    GameObject[] goalAreas;

    public bool isHoldingCollectableObject;
    private bool justReleased;
    int collectables = 1;

    void Start()
    {
        justReleased = false;
        isHoldingCollectableObject = false;

        goalAreas = GameObject.FindGameObjectsWithTag("Drop Area");
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

    GameObject FindNearest(GameObject[] gameObjects) {
        GameObject closestObj = gameObjects[0];
        float shortestDistance = (gameObjects[0].transform.position - this.transform.position).magnitude;
        foreach (GameObject obj in gameObjects) {
            Vector3 currPos = obj.transform.position;


            float relativeDistance = (currPos - this.transform.position).magnitude;

//            CalculateRoute(currPos);

            if (relativeDistance < shortestDistance) {
                shortestDistance = relativeDistance;
                closestObj = obj;
            }
        }
            
        return closestObj;
    }

    void FixedUpdate()
    {
        if (GameObject.FindGameObjectsWithTag("CollectableObject").Length > 0)
        {
            if (justReleased)
            {
                gameObject.GetComponent<RobotMovement>().goGo = true;
                justReleased = false;
            }
            if (!isHoldingCollectableObject)
            {


                GameObject[] collectableObjects = GameObject.FindGameObjectsWithTag("CollectableObject");

                Vector3 targetPos = FindNearest(collectableObjects).transform.position;

                if (!gameObject.GetComponent<RobotMovement>().goGo)
                {
                    Grab();
                }
                Move(targetPos);
            }
            if (isHoldingCollectableObject)
            {// Gogo needs to be true the first time this is run to skip release, so I'm taking advantage of Move().
                if (!gameObject.GetComponent<RobotMovement>().goGo)
                {
                    Release();
                }
                GameObject closestGoal = FindNearest(goalAreas);

                Move(new Vector3(closestGoal.transform.position.x, 0.5f, closestGoal.transform.position.z));
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
        if (gameObject.GetComponent<GrabRelease>().Grab())
        {
            isHoldingCollectableObject = true;
        }

    }

    void Release() {
        if (!isHoldingCollectableObject)
        {
            return;
        }
        justReleased = true;
        isHoldingCollectableObject = false;
        gameObject.GetComponent<GrabRelease>().Release();
    }
}
