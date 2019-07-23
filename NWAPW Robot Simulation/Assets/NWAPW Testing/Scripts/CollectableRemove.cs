using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableRemove : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CollectableObject")
        {

            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
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
