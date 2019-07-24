using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{

    GameObject[] goalAreas;
    private bool targetChanged;

    public bool isHoldingCollectableObject;
    public NavPoint targetPos;
    private bool justReleased;
    private bool justGrabbed;
    public bool everGrabbed;
    bool found = false;
    public bool run;

    List<NavPoint> route = new List<NavPoint>();
    List<NavPoint> searchStack = new List<NavPoint>();
    int layerMask;

    void Start()
    {
        // Bool set up
        targetChanged = true;
        justReleased = false;
        justGrabbed = false;
        isHoldingCollectableObject = false;
        // Non-bool set up
        layerMask = 1 << 8;
        layerMask = ~layerMask;
        goalAreas = GameObject.FindGameObjectsWithTag("Drop Area");
        // Initial route set up
        ResetNavPoints();
        GameObject[] collectableObjects = GameObject.FindGameObjectsWithTag("CollectableObject");
        targetPos = FindNearest(collectableObjects).GetComponent<NavPoint>();

    }

    List<NavPoint> CalculateRouteMain(NavPoint target)
    {
        // Resets Search Stack from previous Nav
        bool found = false;
        searchStack.Clear();
        searchStack.TrimExcess();
        // Adds This object as the first NavPoint in the search stack.
        searchStack.Add(this.gameObject.GetComponent<NavPoint>());

        while (searchStack.Count > 0 && !found) {
            // Checks if it can reach the target and adds follow up points
            found = CalculateRouteRecursion(target, searchStack[0]);
            // Sorts list by fCost (Cost to point on current route + As the crow flies to Target)
            searchStack.Sort(delegate (NavPoint a, NavPoint b) 
            {
                return (a.fCost).CompareTo(b.fCost);
            });
        }
        // Goes back along the route from the target to This Object using NavPoint.from
        // Crashes in this loop is no route is found
        NavPoint backTrack = target;
        List<NavPoint> route = new List<NavPoint>();
        do
        {
            route.Add(backTrack);
            backTrack = backTrack.from;
        } while (backTrack != this.gameObject.GetComponent<NavPoint>());
        return route;
    }


    bool CalculateRouteRecursion(NavPoint target, NavPoint root) {

        float relativeDistance;

        // If there is an obstacle between the current NavPoint and the Target (ball or drop area)
        // This ignores the player and collectables using the layerMask
        if (Physics.Linecast(root.point, target.point, out RaycastHit hitInfo, layerMask))
        {
            // Loops through all the NavPoints from around the Obstacle (Currently the only thing that can be in the way)
            NavPoint[] obstVerts = hitInfo.transform.gameObject.GetComponentsInChildren<NavPoint>();
            foreach (NavPoint current in obstVerts)
            {
                relativeDistance = (current.point - root.point).magnitude;
                // If G cost (distance to the NavPoint from the origin) is higher than existing, a more optimised route already exists to here
                if (relativeDistance + root.gCost < current.gCost) 
                {
                    // Checks Line of Sight to the NavPoint from the root 
                    if (!Physics.Linecast(root.point, current.point, layerMask))
                    {
                        // Updates g, f, and from on the NavPoint with the better route
                        current.gCost = relativeDistance + root.gCost;
                        current.fCost = current.gCost + (current.point - target.point).magnitude;
                        current.from = root;
                        // Adds the NavPoint to the search stack if it has not been already
                        if (!searchStack.Contains(current))
                        {
                            searchStack.Add(current);
                        }
                    }
                }
            }

        } else // Else Line of Sight from the current NavPoint to the Target.
        {
            // Makes the Target 's from be the current NavPoint and set found to true to break the while loop
            target.from = root;
            return true;
        }
        // Removes the curren NavPoint from the search stack
        searchStack.Remove(root);
        return false;
    }



    GameObject FindNearest(GameObject[] gameObjects)
    {
        GameObject Closest = gameObjects[0];
        float shortestDistance = (gameObjects[0].transform.position - this.transform.position).magnitude;
        foreach (GameObject obj in gameObjects)
        {
            Vector3 CurrPos = obj.transform.position;
            float relativeDistance = (CurrPos - this.transform.position).magnitude;

            if (relativeDistance < shortestDistance)
            {
                shortestDistance = relativeDistance;
                Closest = obj;
            }
        }
        return Closest;

    }

    void FixedUpdate()
    {
        if (run)
        {
            GameObject[] collectibles = GameObject.FindGameObjectsWithTag("CollectableObject");
            if (collectibles.Length > 0)
            {
                if (!isHoldingCollectableObject)
                {
                    if (FollowRoute() && !justReleased)
                    {
                        Grab();
                    }

                    GameObject closestCollectible = FindNearest(collectibles);
                    NavPoint closestNavPoint = closestCollectible.GetComponent<NavPoint>();
                    if (closestNavPoint != targetLoc)
                    {
                        targetLoc = closestNavPoint;
                        targetChanged = true;
                        justReleased = false;
                    }
                }
                else
                {
                    if (FollowRoute() && !justGrabbed)
                    {
                        Release();
                        //Toss();
                    }
                    if (FindNearest(goalAreas).GetComponent<NavPoint>() != targetLoc)
                    {
                        targetLoc = FindNearest(goalAreas).GetComponent<NavPoint>();
                        targetChanged = true;
                        justGrabbed = false;
                    }
                }
            }
        }
    }
    bool FollowRoute()
    {
        if (targetChanged)
        {
            route.Clear();
            route.TrimExcess();
            route = CalculateRouteMain(targetPos);
            gameObject.GetComponent<RobotMovement>().isMoving = true;
            targetChanged = false;
            ResetNavPoints();
        }
        if (!gameObject.GetComponent<RobotMovement>().isMoving)
        {
            if (route.Count == 1)
            {
                return true;
            }
            route.RemoveAt(route.Count - 1);
        }
        Move(route[route.Count - 1].point);
        return false;
    }

    /**
     * Doesn't include the NavPoint on the Robot game object.
     */
    private void ResetNavPoints()
    {
        // Obstacles
        GameObject[] Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        List<GameObject> ObstacleVertices = new List<GameObject>();
        foreach(GameObject obs in Obstacles) {
            for (int i = 0; i < obs.transform.childCount; i++) {
                ObstacleVertices.Add(obs.transform.GetChild(i).gameObject);
            }
        }
        foreach (GameObject vertex in ObstacleVertices) {
            vertex.GetComponent<NavPoint>().ResetValues();
        }

        // Collectibles
        GameObject[] Collectibles = GameObject.FindGameObjectsWithTag("CollectableObject");
        foreach (GameObject collectible in Collectibles)
        {
            collectible.GetComponent<NavPoint>().ResetValues();
        }

        // GropAreas
        foreach (GameObject goalArea in goalAreas)
        {
            goalArea.GetComponent<NavPoint>().ResetValues();
        }
    }

    void Move(Vector3 position)
    {
        gameObject.GetComponent<RobotMovement>().Move(position);
    }

    void Grab() {
        everGrabbed = true;
        if (isHoldingCollectableObject) {
            return;
        }
        if (gameObject.GetComponent<GrabRelease>().Grab())
        {
            isHoldingCollectableObject = true;
            justGrabbed = true;
        }
    }

    void Release() {
        justReleased = true;
        if (!isHoldingCollectableObject)
        {
            return;
        }
        isHoldingCollectableObject = false;
        gameObject.GetComponent<GrabRelease>().Release();
    }
    //to toss the ball
    void Toss()
    {
        justReleased = true;
        if (!isHoldingCollectableObject)
        {
            return;
        }
        isHoldingCollectableObject = false;
        gameObject.GetComponent<GrabRelease>().Toss();
    }
}
