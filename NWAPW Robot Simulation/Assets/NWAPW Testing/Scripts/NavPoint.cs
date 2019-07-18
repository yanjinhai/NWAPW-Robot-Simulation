using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    public Vector3 point;
    public float gCost = Mathf.Infinity;
    public GameObject from;
    public float fCost = Mathf.Infinity;
    // Start is called before the first frame update
    void Awake()
    {
        point = this.transform.position;
        from = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
