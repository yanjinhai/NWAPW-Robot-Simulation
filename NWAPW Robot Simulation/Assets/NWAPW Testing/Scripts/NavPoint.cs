using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    public Vector3 point;
    public float gCost = Mathf.Infinity;
    public NavPoint from;
    public float fCost = Mathf.Infinity;
    // Start is called before the first frame update
    void Awake()
    {
        point = new Vector3 (this.transform.position.x,.5f, this.transform.position.y);
        from = this;
    }

    // Update is called once per frame
    void Update()
    {
        point = new Vector3(this.transform.position.x, .5f, this.transform.position.y);
    }
}
