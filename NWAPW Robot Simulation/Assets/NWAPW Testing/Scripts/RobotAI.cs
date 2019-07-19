using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{

    GameObject[] goalAreas;
    private bool targetChanged;
    public bool resetNav;
    public bool isHoldingCollectableObject;
    public NavPoint targetLoc;
    private bool justReleased;
    private bool justGrabbed;
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
        resetNav = true;
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



        if (Physics.Linecast(root.point,target.point,out RaycastHit hitInfo, layerMask))
        {

            NavPoint[] obstVerts = hitInfo.transform.gameObject.GetComponentsInChildren<NavPoint>();
            foreach (NavPoint current in obstVerts)
            {
                float relativeDistance = (current.point - root.point).magnitude;

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

    void Update()
    {
        resetNav = false;
        if (GameObject.FindGameObjectsWithTag("CollectableObject").Length > 0)
        {
            if (!isHoldingCollectableObject)
            {
                if (FollowRoute() && !justReleased)
                {
                    Debug.Log("Grab");
                    Grab();
                }

                if (FindNearest(GameObject.FindGameObjectsWithTag("CollectableObject")).GetComponent<NavPoint>() != targetLoc)
                {
                    targetLoc = FindNearest(GameObject.FindGameObjectsWithTag("CollectableObject")).GetComponent<NavPoint>();
                    targetChanged = true;
                    justReleased = false;
                    Debug.Log("ChangedCol");
                }
            }
            else
            {
                if (FollowRoute() && !justGrabbed)
                {
                    Debug.Log("Release");
                    Release();
                }
                if (FindNearest(goalAreas).GetComponent<NavPoint>() != targetLoc)
                {
                    targetLoc = FindNearest(goalAreas).GetComponent<NavPoint>();
                    targetChanged = true;
                    justGrabbed = false;
                    Debug.Log("ChangedDro");
                }
            }
        }
    }
    void Move(Vector3 position) {
        gameObject.GetComponent<RobotMovement>().Move(position);
    }
    bool FollowRoute()
    {
        Debug.Log("Loop");
        if (targetChanged)
        {
            Debug.Log("targetChangedRun");
            route.Clear();
            route = CalculateRouteMain(targetLoc);
            gameObject.GetComponent<RobotMovement>().goGo = true;
            targetChanged = false;
            resetNav = true;
        }
        if (gameObject.GetComponent<RobotMovement>().goGo)
        {
            Move(route[route.Count - 1].point);
        }
        else
        {
            Debug.Log("NoGogo");
            if (route.Count == 1)
            {
                return true;
            }
            route.RemoveAt(route.Count - 1);
            Move(route[route.Count - 1].point);
        }
        return false;
    }

    void Grab() {
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
        if (!isHoldingCollectableObject)
        {
            return;
        }
        justReleased = true;
        isHoldingCollectableObject = false;
        gameObject.GetComponent<GrabRelease>().Release();
    }
}
