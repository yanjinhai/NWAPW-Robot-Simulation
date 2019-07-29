using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{

    // Variables, some with default values
    public Vector3 point;
    public float gCost = Mathf.Infinity;
    public NavPoint from;
    public float fCost = Mathf.Infinity;
    public float deadBand = -0.5f;

    // Awake is used here to prempt intial pathfinding run on start
    void Awake()
    {
        // Set up point at its location and resets values to defaults
        point = new Vector3(this.transform.position.x, .5f, this.transform.position.z);
        ResetValues();

        // Sets deadband for objects the robot runs into
        if (this.gameObject.tag == "CollectableObject")
        {
            deadBand = this.gameObject.GetComponent<Collider>().bounds.size.x / 2;
        }

        if (this.gameObject.tag == "Basket")
        {
            deadBand = 11.674f;
        }
    }

    void Update()
    {
        // Updates point for moving objects
        point = new Vector3(this.transform.position.x, .5f, this.transform.position.z);
    }

    // Resets from and costs to default values, except player, due to it having special values
    public void ResetValues()
    {
        if (this.gameObject.tag != "Player")
        {
            gCost = Mathf.Infinity;
            fCost = Mathf.Infinity;

        }
        from = this;
    }
}
