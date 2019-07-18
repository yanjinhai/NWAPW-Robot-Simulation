using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{

    public GameObject goalArea;

    public bool isHoldingCollectableObject;
    private bool justReleased;
    int collectables = 1;

    void Start()
    {
        justReleased = false;
        isHoldingCollectableObject = false;
    }



    
    void CalculateRoute(Vector3 targetPos) {
        RaycastHit hitInfo;

        float relativeDistance = (targetPos - this.transform.position).magnitude;

        Vector3 relativePos = targetPos - this.transform.position;

        Physics.Raycast(transform.position, relativePos, out hitInfo);

        Debug.DrawRay(transform.position, relativePos, Color.red);

        if ((hitInfo.distance < relativeDistance - 0.5)&& (hitInfo.transform.tag != "CollectableObject"))
        {

            GameObject obstacle = hitInfo.transform.gameObject;
            NavPoint[] obstVerts = obstacle.GetComponentsInChildren<NavPoint>();

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
                Move(new Vector3(goalArea.transform.position.x, 0.5f, goalArea.transform.position.z));
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
