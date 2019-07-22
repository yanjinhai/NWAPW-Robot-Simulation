using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleopGrabRelease : MonoBehaviour
{
    public GameObject Collectables;
    public Camera camera;
    public GameObject grabbedObj;
    private GameObject[] grabbableObjs;
    public bool isHoldingCollectableObject;
    Vector3 offset = new Vector3(0, 0.1f, 1.05f);
    // Start is called before the first frame update
    void Start()
    {
        isHoldingCollectableObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoldingCollectableObject) {
            grabbedObj.transform.localPosition = offset;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            grab();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            release();
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
    void grab()
    {
        if (isHoldingCollectableObject)
        {
            return;
        }   
         grabbableObjs = GameObject.FindGameObjectsWithTag("CollectableObject");
         grabbedObj = FindNearest(grabbableObjs);
         if ((grabbedObj.transform.position - this.transform.position).magnitude <= 1.5f)
         {
            if (infront(grabbedObj.transform))
            {
                grabbedObj.transform.parent = this.transform;
                grabbedObj.GetComponent<Rigidbody>().useGravity = false;
                isHoldingCollectableObject = true;
            }
            
         }
         
    }
    void release()
    {
        if (!isHoldingCollectableObject)
        {
            return;
        }
        isHoldingCollectableObject = false;
        grabbedObj.GetComponent<Rigidbody>().useGravity = true;
        grabbedObj.transform.parent = Collectables.transform;
        grabbedObj = null;

    }
    public bool infront(Transform target) {
        Vector3 visTest = camera.WorldToViewportPoint(target.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }
}
