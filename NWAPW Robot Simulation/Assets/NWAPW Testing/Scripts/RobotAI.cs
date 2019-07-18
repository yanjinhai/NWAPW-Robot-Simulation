using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{

    GameObject[] goalAreas;

    public bool isHoldingCollectableObject;
    private bool justReleased;
    int collectables = 1;
    bool found = false;
    List<NavPoint> searchStack = new List<NavPoint>();

    void Start()
    {
        justReleased = false;
        isHoldingCollectableObject = false;
        goalAreas = GameObject.FindGameObjectsWithTag("Drop Area");
    }

    List<NavPoint> CalculateRouteMain(NavPoint target)
    {
        found = false;
        searchStack = new List<NavPoint>();

        searchStack.Add(this.gameObject.GetComponent<NavPoint>());

        while (searchStack.Count > 0 && !found) {
            searchStack.Sort(delegate (NavPoint a, NavPoint b)
            {
                return (a.fCost).CompareTo(b.fCost);
            });
            CalculateRouteRecursion(target, searchStack[0]);
        }

        NavPoint backTrack = target;
        List<NavPoint> route = new List<NavPoint>();
        do
        {
            route.Add(backTrack);
            backTrack = backTrack.from;
        } while (backTrack != this.gameObject.GetComponent<NavPoint>());
        return route;
    }


    void CalculateRouteRecursion(NavPoint target, NavPoint root) {
        RaycastHit hitInfo;
        Vector3 targetPos = target.point;
        float relativeDistance = (targetPos - root.point).magnitude;

        Vector3 relativePos = targetPos - root.point;

        Physics.Raycast(root.point, relativePos, out hitInfo);

        Debug.DrawRay(root.point, relativePos, Color.red);

        if ((hitInfo.distance < relativeDistance) && (hitInfo.transform.tag != "CollectableObject"))
        {

            GameObject obstacle = hitInfo.transform.gameObject;
            NavPoint[] obstVerts = obstacle.GetComponentsInChildren<NavPoint>();
            foreach (NavPoint current in obstVerts)
            {
                relativeDistance = (current.point - root.point).magnitude;

                if (relativeDistance + root.gCost < current.gCost) //If G cost is higher no point already more optomised
                {
                    relativePos = current.point - root.point;
                    Physics.Raycast(root.point, relativePos, out hitInfo);
                    if (hitInfo.distance < relativeDistance)
                    {
                        current.gCost = relativeDistance + root.gCost;
                        current.fCost = current.gCost + (current.point - target.point).magnitude;
                        current.from = root;
                        if (!searchStack.Contains(current))
                        {
                            searchStack.Add(current);
                        }
                    }
                }
            }

        } else
        {
            target.from = root;
            found = true;
        }
        searchStack.Remove(root);
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

    void FixedUpdate() {
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

                GameObject target = FindNearest(collectableObjects);
                List<NavPoint> route = CalculateRouteMain(target.GetComponent<NavPoint>());
                if (!gameObject.GetComponent<RobotMovement>().goGo)
                {
                    Grab();
                }
                
                Move(target.transform.position);
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
