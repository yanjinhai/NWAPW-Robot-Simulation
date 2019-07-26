using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableRemove : MonoBehaviour
{
    // Handles ball scoring
    void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "CollectableObject" && obj.GetComponent<MeshFilter>().sharedMesh.name == "Sphere")
        {
            Destroy(obj);
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<GrabRelease>().everGrabbed) 
            {
                GameObject.FindGameObjectWithTag("DropObject").GetComponent<Increment>().increase(1);
            }
            else {
                print("Redirected Block");
                GameObject.FindGameObjectWithTag("CollectableParent").GetComponent<RandomSpawn>().newObj();
            }
        }
    }
}
