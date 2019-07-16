using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{

    public GameObject collectableObjectParent;


    private bool isHoldingCollectableObject;
    

    void Start()
    {
        isHoldingCollectableObject = false;
    }

    Vector3 FindNearest(Transform[] transforms) {
        Vector3 closestPosition = new Vector3(0.0f, 0.5f, 0.0f);
        float shortestDistance = (transforms[0].position - this.transform.position).magnitude;
        foreach (Transform comparableTransform in transforms) {
            Vector3 currPos = comparableTransform.position;

            float relativeDistance = (currPos - this.transform.position).magnitude;

            if (relativeDistance < shortestDistance) {
                shortestDistance = relativeDistance;
                closestPosition = currPos;
            }
        }
            
        return closestPosition;
    }

    void FixedUpdate()
    {
        if (!isHoldingCollectableObject) {

            Transform[] collectableObjectTransforms = collectableObjectParent.GetComponentsInChildren<Transform>();

            Vector3 targetPos = FindNearest(collectableObjectTransforms);

            Move(targetPos);

            if (!gameObject.GetComponent<RobotMovement>().goGo) {
                Grab();
            }
        }
    }

    void Move(Vector3 position) {
        gameObject.GetComponent<RobotMovement>().Move(position);
    }

    void Grab() {
        if (isHoldingCollectableObject) {
            return;
        }
        isHoldingCollectableObject = true;

        // Unfinished. Needs to actually pick up object.

    }

    void Release() {
    }
}
