using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackAreaScript : MonoBehaviour
{
    // Input
    public GameObject robot, block;

    // Constants
    float robotRadMin, robotRadMax, blockRadMin, blockRadMax, FixedDistance;

    // Output
    public Vector3 nextPos;
    public List<Vector3> refPoints;

    void Awake()
    {
        // Initializes constants. Assumes robot and blocks are perfect cubes. X can be replaced by Y or Z without any issues.
        robotRadMin = robot.transform.Find("Body").GetComponent<Collider>().bounds.size.x / 2; // Half the side length of the cube. Minimum radius of the robot body.
        robotRadMax = robotRadMin * Mathf.Sqrt(2); // Half the diagonal length of the cube. Maximum radius of the robot body.
        blockRadMin = block.GetComponent<Collider>().bounds.size.x / 2; // Half the side length of the cube. Minimum radius of the block.
        blockRadMax = blockRadMin * Mathf.Sqrt(2); // Half the diagonal length of the cube. Maximum radius of the block.
        FixedDistance = GetComponent<Collider>().bounds.size.x/2 + 2 * blockRadMax + robotRadMax; // Fixed distance that the reference points are away from the stack area position.
    }

    void Update()
    {
        CalculatePoints();
    }

    void CalculatePoints()
    {
        // Delete old reference points.
        refPoints.Clear();

        // Calculate the next available position for the next block.
        nextPos = transform.position + new Vector3(GetComponent<Collider>().bounds.size.x / 2, block.transform.position.y, GetComponent<Collider>().bounds.size.z / 2);

        // Calculate all the reference points.
        refPoints.Add(new Vector3(nextPos.x, robot.transform.position.y, transform.position.z + FixedDistance));
        refPoints.Add(new Vector3(transform.position.x + FixedDistance, robot.transform.position.y, nextPos.z));
        refPoints.Add(new Vector3(nextPos.x, robot.transform.position.y, transform.position.z - FixedDistance));
        refPoints.Add(new Vector3(transform.position.x - FixedDistance, robot.transform.position.y, nextPos.z));
        
        // Remove the obstructed reference points.
        for (int i = 0; i < refPoints.Count; i++)
        {
            bool isObstructed = Physics.Linecast(nextPos, refPoints[i]);
            if (isObstructed)
            {
                refPoints.Remove(refPoints[i]);
            }
        }
    }
}
