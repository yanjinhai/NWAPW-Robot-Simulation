using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    void Start()
    {

    }


    void Update()
    {
        //CheckState();
    }

    /**
     * This works for cube shapes, not sure about anything else.
     */
    public bool CheckState()
    {
        GameObject stackAreaParent = GameObject.FindGameObjectWithTag("DropObject");
        // Find the extents of the block's collider.
        Vector3 blockExtents = this.gameObject.GetComponent<Collider>().bounds.extents;
        // Find the local x and z coordinates of the block's corners (Relative to the block).
        Vector3[] blockCornerPositions = new Vector3[4];
        blockCornerPositions[0] = new Vector3(blockExtents.x, 0, blockExtents.z);
        blockCornerPositions[1] = new Vector3(-blockExtents.x, 0, blockExtents.z);
        blockCornerPositions[2] = new Vector3(-blockExtents.x, 0, -blockExtents.z);
        blockCornerPositions[3] = new Vector3(blockExtents.x, 0, -blockExtents.z);
        // Convert the coordinates of the block's corners from local to global (Relative to the world space).
        for (int i = 0; i < blockCornerPositions.Length; i++)
        {
            blockCornerPositions[i] += this.transform.position;
        }
        // Compare with every stack area in the scene.
        for (int j = 0; j < stackAreaParent.transform.childCount; j++)
        {
            // Get the current stack area being considered.
            Transform currArea = stackAreaParent.transform.GetChild(j); 
            // Get the extents of the area.
            Vector3 areaExtents = currArea.gameObject.GetComponent<Collider>().bounds.extents;
            // Check if the block is in the bounds of the stack area being considered, corner by corner.
            bool inAreaBounds = true;
            for (int i = 0; i < blockCornerPositions.Length; i++)
            {
                // For each corner, find its position relative to the center of the stack area. 
                Vector3 relativePosition = blockCornerPositions[i] - currArea.position;
                // If the corner's x or z coordinates relative the center of the stack area is greater than the extents of the stack area in the respective dimensions,
                // then the block is not in the bounds of the currently considered stack area.
                if (Mathf.Abs((relativePosition.x)) > areaExtents.x || Mathf.Abs((relativePosition.z)) > areaExtents.z)
                {
                    inAreaBounds = false;
                }
            }
            // If the block is in the bounds of the stack area (for x and z at least), then it counts as a point.
            if (inAreaBounds)
            {
                return true;
            }
        }
        // If the block is not in the bounds of any stack area, then it doesn't count as a point.
        return false;
    }
}
