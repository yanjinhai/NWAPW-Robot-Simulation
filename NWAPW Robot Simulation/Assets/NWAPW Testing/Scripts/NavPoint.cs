using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    public Vector3 point;
    public float gCost;
    public GameObject from;
    public float fCost;
    // Start is called before the first frame update
    void Awake()
    {
        point = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
