using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    public Vector3 point;
    public float gCost = Mathf.Infinity;
    public NavPoint from;
    public float fCost = Mathf.Infinity;

    void Awake()
    {
        point = new Vector3 (this.transform.position.x,.5f, this.transform.position.z);
        from = this;
    }

    void Update()
    {
        point = new Vector3(this.transform.position.x, .5f, this.transform.position.z);
    }

    public void ResetValues() {
        if (this.gameObject.tag != "Player")
        {
            gCost = Mathf.Infinity;
            fCost = Mathf.Infinity;
            from = this;
        }
    }
}
