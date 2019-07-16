using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{

    public GameObject collectableObjectParent;
    public float positionDeadband;


    private bool isHoldingCollectableObject;
    

    void Start()
    {
        isHoldingCollectableObject = false;
    }

    Vector3 FindNearest(Transform[] transforms) {
        Vector3 closestPosition = new Vector3(0.0f, 0.0f, 0.0f);
        float shortestDistance = 0;
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

            float distanceFromTarget = (targetPos - this.transform.position).magnitude;

            if (distanceFromTarget < positionDeadband) {
                Grab();
            }
        }
    }

    void Move(Vector3 position) {

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
