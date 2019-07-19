using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject CollectableParent;
    public GameObject CollectablePremade;
    GameObject obj1 = null;
    GameObject obj2 = null;
    GameObject obj3 = null;
    // Start is called before the first frame update
    void Awake()
    {
        
            newObj(obj1, 1);
            newObj(obj2, 2);
            newObj(obj3, 3);

      
    }
    void newObj(GameObject obj, int num) {
        obj = (GameObject)(Instantiate(CollectablePremade, new Vector3(Random.Range(-23.0f, 23.0f), 0.5f, Random.Range(-23.0f, 23.0f)), Quaternion.identity));
        obj.name = "Obj "+num;
        obj.SetActive(true);
        obj.transform.parent = CollectableParent.transform;
    }
}
