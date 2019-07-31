using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackAreaScript : MonoBehaviour
{
    // Input
    public GameObject robot;

    // Output
    public Vector3 nextPos;
    public List<Vector3> refPoints;

    // Constants
    public float robotRadMin, robotRadMax, blockRadMin, blockRadMax, fixedDistance, allowedError, blockSpacing;
    int layerMask, xCapacity, zCapacity;

    // Global Variables
    GameObject block;
    List<GameObject> StackedBlocks;

    // Awake is called upon initialization
    void Awake()
    {
        StackedBlocks = new List<GameObject>();

        layerMask = 11 << 8;
        layerMask = ~layerMask;

        xCapacity = 4;
        zCapacity = 4;
    }

    void Start() {
        block = GameObject.FindObjectOfType<BlockScript>().gameObject;
        CalculateConstants();
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePoints();
        
        for (int i = 0; i < StackedBlocks.Count; i++) {
            if (!StackedBlocks[i].GetComponent<BlockScript>().CheckState()) {
                StackedBlocks[i].layer = 8;
                StackedBlocks.RemoveAt(i);
                i--;
            }
        }
    }

    /*
     * Calculate constants. Assumes the robot and the blocks are perfect cubes and that all blocks are the same size. X can be replaced by Y or Z without any issues.
     */
    private void CalculateConstants()
    {
        // Set the allowed error margin for the block placing
        allowedError = 0.5f;

        // Set the spacing between blocks
        blockSpacing = 1f;

        // Find the minimum radius of the robot body (half the side length of the cube)
        robotRadMin = robot.transform.Find("Body").GetComponent<Collider>().bounds.extents.x;

        // Find the maximum radius of the robot body (half the diagonal length of the cube)
        robotRadMax = robotRadMin * Mathf.Sqrt(2);

        // Find the minimum radius of the block (half the side length of the cube)
        blockRadMin = block.GetComponent<Collider>().bounds.extents.x;

        // Find the maximum radius of the block (half the diagonal length of the cube)
        blockRadMax = blockRadMin * Mathf.Sqrt(2);

        // Find the fixed distance that the reference points are away from the stack area center position
        fixedDistance = GetComponent<Collider>().bounds.extents.x + 2 * blockRadMax + robotRadMax;
    }

    // Calculate nextPos and refPoints     
    private void CalculatePoints()
    {
        // Delete old reference points
        refPoints.Clear();

        // Calculate the placement margin of this stack area
        float placementMargin = GetComponent<MeshCollider>().bounds.extents.x - blockRadMin - allowedError;

        // Find the displacement due to blocks already stacked.
        float xDisplacement = (StackedBlocks.Count % xCapacity) * (2 * blockRadMin/* + allowedError*/);
        float yDisplacement = (StackedBlocks.Count / xCapacity / zCapacity) * (2 * blockRadMin);
        float zDisplacement = (int)(StackedBlocks.Count / zCapacity) * (2 * blockRadMin/* + allowedError*/);
        Vector3 nextPosDisplacement = new Vector3(xDisplacement, yDisplacement, zDisplacement);
        
        // Calculate the next available position for the next block
        nextPos = transform.position + new Vector3(-placementMargin, blockRadMin, -placementMargin) + nextPosDisplacement;

        // Calculate all the reference points
        refPoints.Add(new Vector3(nextPos.x, robot.transform.position.y, transform.position.z + fixedDistance));
        refPoints.Add(new Vector3(transform.position.x + fixedDistance, robot.transform.position.y, nextPos.z));
        refPoints.Add(new Vector3(nextPos.x, robot.transform.position.y, transform.position.z - fixedDistance));
        refPoints.Add(new Vector3(transform.position.x - fixedDistance, robot.transform.position.y, nextPos.z));

        // Remove the obstructed reference points
        for (int i = 0; i < refPoints.Count; i++)
        {
            // Linecast from nextPos to each of the reference points
            bool isObstructed = Physics.Linecast(nextPos, refPoints[i], layerMask);
            if (isObstructed)
            {
                // Delete the reference point if the line is obstructed by another object
                refPoints.Remove(refPoints[i]);
                i--;
            }
        }
    }

    // Handles collisions
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "CollectableObject" && other.GetComponent<BlockScript>() != null)
        {
            // If the colliding gameobject is a block
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<GrabRelease>().everGrabbed)
            {
                // If the block was moved there by means other than spawning there, change its layer to the defualt layer and add it to the list of stacked blocks
                other.layer = 10;
                StackedBlocks.Add(other);
            }
            else
            {
                // If the colliding gameobject spawned on this stack area, destroy it and create a new one
                Destroy(other);
                GameObject.FindGameObjectWithTag("CollectableParent").GetComponent<RandomSpawn>().newObj();
            }
        }
    }

    // Handles collisions
    private void OnCollisionExit(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "CollectableObject"/*&& other.GetComponent<BlockScript>() != null*/)
        {
            // If the colliding object is a block, change its layer to layer 8 and remove it from the list of stacked blocks.
            other.layer = 8;
            StackedBlocks.Remove(other);
        }
    }
}
