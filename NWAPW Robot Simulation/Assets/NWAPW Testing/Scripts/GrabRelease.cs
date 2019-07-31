using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRelease : MonoBehaviour
{
    public GameObject Collectables;
    public GameObject grabbedObj;
    private GameObject[] grabbableObjs;
    public Camera firstPersonCamera;
    public Camera shootCamera;

    public bool isHoldingCollectableObject;
    public bool everGrabbed;
    Vector3 offset = new Vector3(0, 0.1f, 1.05f);

    int originalChildCount;

    void Awake()
    {
        isHoldingCollectableObject = false;

        /*
         * I've come up with an alternative method of keeping track of isHoldingCollectableObject.
         * Because all grabbed objects are children objects of the robot, the value of isHoldingCollectableObject
         * will be true if the current child count is greater than the child count at the start of the simulation,
         * because the robot isn't grabbing any objects at that time.
         * 
         * I tested this method and it works!
         * 
         * Jinhai
         */
        originalChildCount = gameObject.transform.childCount;
    }

    void Update()
    {
        // Update value.
        isHoldingCollectableObject = gameObject.transform.childCount > originalChildCount;

        if (isHoldingCollectableObject)
        {
            grabbedObj.transform.localPosition = offset;
            grabbedObj.transform.localRotation = new Quaternion();
        }
    }

    public bool Grab()
    {
        grabbableObjs = GameObject.FindGameObjectsWithTag("CollectableObject");
        grabbedObj = FindNearest(grabbableObjs);
        if ((grabbedObj.transform.position - this.transform.position).magnitude <= 1.25f)
        {
            if (infront(grabbedObj.transform))
            {
                everGrabbed = true;
                grabbedObj.transform.parent = this.transform;
                grabbedObj.GetComponent<Rigidbody>().useGravity = false;
                return true;
            }
        }
        grabbedObj = null;
        return false;
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
    public void Release()
    {
        grabbedObj.transform.parent = Collectables.transform;
        grabbedObj.GetComponent<Rigidbody>().useGravity = true;
        grabbedObj = null;
    }

    //toss the ball
    public void Toss() {
        grabbedObj.transform.parent = Collectables.transform;
        Rigidbody rb = grabbedObj.GetComponent<Rigidbody>();
        rb.useGravity = true;
        
        grabbedObj.transform.position = transform.position + shootCamera.transform.forward * 2;
        rb.velocity = shootCamera.transform.forward * 15;
        grabbedObj = null;
    }
    public bool infront(Transform target)
    {
        Vector3 visTest = firstPersonCamera.WorldToViewportPoint(target.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }
}
