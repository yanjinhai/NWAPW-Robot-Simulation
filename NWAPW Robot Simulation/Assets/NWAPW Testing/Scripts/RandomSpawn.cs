using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject CollectableParent;
    public GameObject CollectablePremade;
    void Awake()
    {
        // Spawn initial collectable objects
        for (int i = 0; i < 20; i++) {
            newObj();
        } 
    }
    public void newObj() {
       GameObject obj = (GameObject)(Instantiate(CollectablePremade, new Vector3(Random.Range(-23.0f, 23.0f), 0.5f, Random.Range(-23.0f, 23.0f)), Quaternion.identity));
       //GameObject obj = (GameObject)(Instantiate(CollectablePremade, new Vector3(11, 0.5f, 0), Quaternion.identity));

        obj.name = "Obj ";
        obj.SetActive(true);
        obj.transform.parent = CollectableParent.transform;
    }
}
