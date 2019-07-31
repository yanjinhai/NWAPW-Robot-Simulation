using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableCylinderRemove : MonoBehaviour
{
    public GameObject scoreHandler;
    void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "CollectableObject" && obj.transform.position.y > transform.position.y + GetComponent<CapsuleCollider>().bounds.extents.y && obj.GetComponent<BlockScript>() == null)
        {
            obj.SetActive(false);
            Destroy(obj);
            scoreHandler.GetComponent<ScoreTracking>().scoreBasket();
        }
    }
}
