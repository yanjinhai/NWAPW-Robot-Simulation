using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRelease : MonoBehaviour
{
    public GameObject Collectables;
    public GameObject grabbedObj;
    private GameObject[] grabbableObjs;
    public Camera camera;
    public Camera shootCamera;

    public bool isHoldingCollectableObject;
    public bool everGrabbed;
    Vector3 offset = new Vector3(0, 0.1f, 1.05f);

    void Awake()
    {
        isHoldingCollectableObject = false;
    }


    void Update()
    {

        if (isHoldingCollectableObject)
        {
            grabbedObj.transform.localPosition = offset;
        }
    }

    public bool Grab()
    {
        grabbableObjs = GameObject.FindGameObjectsWithTag("CollectableObject");
        grabbedObj = FindNearest(grabbableObjs);
        if ((grabbedObj.transform.position - this.transform.position).magnitude <= 1.2f)
        {
            if (infront(grabbedObj.transform))
            {
                everGrabbed = true;
                grabbedObj.transform.parent = this.transform;
                grabbedObj.GetComponent<Rigidbody>().useGravity = false;
                grabbedObj.transform.rotation = new Quaternion();
                grabbedObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                isHoldingCollectableObject = true;
                return true;
            }
        }
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
        isHoldingCollectableObject = false;
        grabbedObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        grabbedObj.GetComponent<Rigidbody>().useGravity = true;
        grabbedObj.transform.parent = Collectables.transform;
        grabbedObj = null;

    }
    //toss the ball
    public void Toss() {
        isHoldingCollectableObject = false;
        grabbedObj.transform.parent = Collectables.transform;
        Rigidbody rb = grabbedObj.GetComponent<Rigidbody>();
        rb.useGravity = true;
        
        grabbedObj.transform.position = transform.position + shootCamera.transform.forward * 2;
        rb.velocity = shootCamera.transform.forward * 15;
        grabbedObj = null;
    }
    public bool infront(Transform target)
    {
        Vector3 visTest = camera.WorldToViewportPoint(target.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }
}
