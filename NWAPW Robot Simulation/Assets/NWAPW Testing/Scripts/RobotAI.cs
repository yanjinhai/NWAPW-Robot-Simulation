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

    
    void CalculateRouteMain(NavPoint target)
    {
        List<NavPoint> searchStack = new List<NavPoint>();

        
    }
    
    void CalculateRouteRecursion(NavPoint target, NavPoint root = this.gameObject.GetComponent<NavPoint>) {
        RaycastHit hitInfo;
        Vector3 targetPos = target.point;
        float relativeDistance = (targetPos - root.point).magnitude;

        Vector3 relativePos = targetPos - root.point;

        Physics.Raycast(root.point, relativePos, out hitInfo);

        Debug.DrawRay(root.point, relativePos, Color.red);

        if ((hitInfo.distance < relativeDistance - 0.5)&& (hitInfo.transform.tag != "CollectableObject"))
        {

            GameObject obstacle = hitInfo.transform.gameObject;
            NavPoint[] obstVerts = obstacle.GetComponentsInChildren<NavPoint>();
            foreach(NavPoint current in obstVerts)
            {
                relativeDistance = (NavPoint.point - root.point).magnitude;

                if ((relativeDistance).magnitude+root.gCost < NavPoint.gCost) //If G cost is higher no point already more optomised
                {
                    relativePos = NavPoint.point - root.point;
                    Physics.Raycast(root.point, relativePos, out hitInfo);
                    if (hitInfo.distance < relativeDistance)
                    {
                        NavPoint.gCost = (relativeDistance).magnitude + root.gCost;
                        NavPoint.fCost = NavPoint.gCost + (NavPoint.point - target.point).magnitude;
                    }
                }
            }

        } else
        {

        }
    }
    

    GameObject FindNearest(GameObject[] gameObjects)
    {
        GameObject closest = gameObjects[0];
        float shortestDistance = (gameObjects[0].transform.position - this.transform.position).magnitude;
        foreach (GameObject obj in gameObjects)
        {
            Vector3 currPos = obj.transform.position;
            float relativeDistance = (currPos - this.transform.position).magnitude;

            if (relativeDistance < shortestDistance)
            {
                shortestDistance = relativeDistance;
                closest = obj;
            }
        }

        return closest;
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
