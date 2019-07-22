using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{

    GameObject[] goalAreas;
    private bool targetChanged;

    public bool isHoldingCollectableObject;
    public NavPoint targetLoc;
    private bool justReleased;
    private bool justGrabbed;
    public bool everGrabbed;
    bool found = false;
    List<NavPoint> route = new List<NavPoint>();
    List<NavPoint> searchStack = new List<NavPoint>();
    List<NavPoint> closedSearch = new List<NavPoint>();
    int layerMask;

    void Start()
    {
        targetChanged = true;
        justReleased = false;
        justGrabbed = false;
        ResetNavPoints();
        isHoldingCollectableObject = false;

        layerMask = 1 << 8;
        layerMask = ~layerMask;
        goalAreas = GameObject.FindGameObjectsWithTag("Drop Area");
        GameObject[] collectableObjects = GameObject.FindGameObjectsWithTag("CollectableObject");
        targetLoc = FindNearest(collectableObjects).GetComponent<NavPoint>();

    }

    List<NavPoint> CalculateRouteMain(NavPoint target)
    {
        found = false;
        searchStack.Clear();
        closedSearch.Clear();
        searchStack.TrimExcess();
        closedSearch.TrimExcess();
        searchStack.Add(this.gameObject.GetComponent<NavPoint>());

        while (searchStack.Count > 0 && !found) {           
            CalculateRouteRecursion(target, searchStack[0]);
            searchStack.Sort(delegate (NavPoint a, NavPoint b)
            {
                return (a.fCost).CompareTo(b.fCost);
            });
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

        float relativeDistance;

        // if there is an obstacle between the current NavPoint and the target (ball or drop area)
        if (Physics.Linecast(root.point, target.point, out RaycastHit hitInfo, layerMask))
        {
            NavPoint[] obstVerts = hitInfo.transform.gameObject.GetComponentsInChildren<NavPoint>();
            foreach (NavPoint current in obstVerts)
            {
                relativeDistance = (current.point - root.point).magnitude;
                if (relativeDistance + root.gCost < current.gCost && !closedSearch.Contains(current)) //If G cost is higher no point already more optomised
                {
                    if (!Physics.Linecast(root.point, current.point, layerMask))
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
        closedSearch.Add(root);
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
    bool FollowRoute()
    {
        if (targetChanged)
        {
            route.Clear();
            route.TrimExcess();
            route = CalculateRouteMain(targetLoc);
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

        //Collectibles
        GameObject[] Collectibles = GameObject.FindGameObjectsWithTag("CollectableObject");
        foreach (GameObject collectible in Collectibles)
        {
            collectible.GetComponent<NavPoint>().ResetValues();
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
}
