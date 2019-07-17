using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRelease : MonoBehaviour
{
    public GameObject grabbedObj;
    public GameObject grabbableObjs;
    Vector3 offset = new Vector3(0, 0.1f, 1.05f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grabbedObj.transform.localPosition = offset;
    }

    public void Grab()
    {
        Transform[] grabbableObjsTrans = grabbableObjs.GetComponentsInChildren<Transform>();
        grabbedObj = FindNearestTrans(grabbableObjsTrans).gameObject;
        grabbedObj.transform.parent = this.transform;
        Debug.Log("Parent: " + grabbedObj.transform.parent.name);
        grabbedObj.GetComponent<Rigidbody>().useGravity = false;
        
    }
    Transform FindNearestTrans(Transform[] transforms)
    {
        Transform closestTrans = transforms[0];
        float shortestDistance = (transforms[0].position - this.transform.position).magnitude;
        foreach (Transform comparableTransform in transforms)
        {
            Vector3 currPos = comparableTransform.position;    
            float relativeDistance = (currPos - this.transform.position).magnitude;

            if (relativeDistance < shortestDistance)
            {
                shortestDistance = relativeDistance;
                closestTrans = comparableTransform;
            }
        }

        return closestTrans;
    }
}
