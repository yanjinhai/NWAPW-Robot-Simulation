using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRemove : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CollectableObject")
        {
            Destroy(collision.gameObject);
        }
    }
}
