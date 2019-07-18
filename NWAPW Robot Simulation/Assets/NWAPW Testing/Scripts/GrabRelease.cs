using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRelease : MonoBehaviour
{
    public GameObject grabbedObj;
    public GameObject[] grabbableObjs;
    public GameObject releasedObjs;
    
    Vector3 offset = new Vector3(0, 0.1f, 1.05f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<RobotAI>().isHoldingCollectableObject)
        {
            grabbedObj.transform.localPosition = offset;
        }
    }

    public void Grab()
    {
        grabbableObjs = GameObject.FindGameObjectsWithTag("CollectableObject");
        grabbedObj = FindNearest(grabbableObjs);
        grabbedObj.transform.parent = this.transform;
        grabbedObj.GetComponent<Rigidbody>().useGravity = false;
        
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
        grabbedObj.transform.parent = releasedObjs.transform;
        grabbedObj.GetComponent<Rigidbody>().useGravity = true;
        grabbedObj = null;

    }
}
