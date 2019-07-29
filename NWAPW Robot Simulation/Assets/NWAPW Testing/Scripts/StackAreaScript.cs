using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackAreaScript : MonoBehaviour
{
    // Input
    public GameObject robot, block;

    // Constants
    public float robotRadMin, robotRadMax, blockRadMin, blockRadMax, FixedDistance;
    int layerMask;

    // Output
    public Vector3 nextPos;
    public List<Vector3> refPoints;

    // Awake is called upon initialization
    void Awake()
    {
        layerMask = 11 << 8;
        layerMask = ~layerMask;
        /*
         * Initializes constants. Assumes the robot and the blocks are perfect cubes. X can be replaced by Y or Z without any issues.
         */

        // Finds the minimum radius of the robot body (half the side length of the cube)
        robotRadMin = robot.transform.Find("Body").GetComponent<Collider>().bounds.extents.x;

        // Finds the maximum radius of the robot body (half the diagonal length of the cube)
        robotRadMax = robotRadMin * Mathf.Sqrt(2);

        // Finds the minimum radius of the block (half the side length of the cube)
        blockRadMin = block.GetComponent<Collider>().bounds.extents.x;
        print(block.GetComponent<Collider>());

        // Finds the maximum radius of the block (half the diagonal length of the cube)
        blockRadMax = blockRadMin * Mathf.Sqrt(2);
        
        // Finds the fixed distance that the reference points are away from the stack area center position
        FixedDistance = GetComponent<Collider>().bounds.extents.x + 2 * blockRadMax + robotRadMax;
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePoints();
    }

    // Calculates nextPos and refPoints
    void CalculatePoints()
    {
        // Delete old reference points
        refPoints.Clear();

        // Calculate the next available position for the next block
        nextPos = transform.position + new Vector3(GetComponent<MeshCollider>().bounds.extents.x - blockRadMin, block.transform.position.y, GetComponent<MeshCollider>().bounds.extents.z - blockRadMin);

        // Calculate all the reference points
        refPoints.Add(new Vector3(nextPos.x, robot.transform.position.y, transform.position.z + FixedDistance));
        refPoints.Add(new Vector3(transform.position.x + FixedDistance, robot.transform.position.y, nextPos.z));
        refPoints.Add(new Vector3(nextPos.x, robot.transform.position.y, transform.position.z - FixedDistance));
        refPoints.Add(new Vector3(transform.position.x - FixedDistance, robot.transform.position.y, nextPos.z));
        
        // Remove the obstructed reference points
        for (int i = 0; i < refPoints.Count; i++)
        {
            bool isObstructed = Physics.Linecast(nextPos, refPoints[i], layerMask);
            if (isObstructed)
            {
                refPoints.Remove(refPoints[i]);
            }
        }
    }
}
