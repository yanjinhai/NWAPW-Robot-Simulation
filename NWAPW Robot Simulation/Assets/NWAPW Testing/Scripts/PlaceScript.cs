using UnityEngine;
using System.Collections;

public class PlaceScript : MonoBehaviour
{

    public GameObject collectable;
    public GameObject collectable2;
    public GameObject collectablesParent;
    public GameObject cameraSwitcher;
    GameObject currentCam;
    int layerMask;

    void Start()
    {
        layerMask = 1 << 8;
        layerMask = ~layerMask;
    }


    void Update()
     {
        currentCam = cameraSwitcher.GetComponent<ButtonActions>().CurrentCam;
        if (Input.GetButtonDown("Fire1") && GameObject.FindGameObjectWithTag("ActionHandler").GetComponent<ButtonActions>().place)
         {
            RaycastHit rayInfo;

            if (Physics.Raycast(currentCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), out rayInfo,500, layerMask))
            {
                Vector3 vectorHit = rayInfo.point;
                vectorHit = new Vector3(vectorHit.x, vectorHit.y + 0.5f, vectorHit.z);
                if (!GameObject.FindGameObjectWithTag("ActionHandler").GetComponent<ButtonActions>().spawnSecond)
                {
                    GameObject newBall = Instantiate(collectable, vectorHit, Quaternion.identity);
                    newBall.transform.parent = collectablesParent.transform;
                }
                else
                {
                    GameObject newBlock = Instantiate(collectable2, vectorHit, Quaternion.identity);
                    newBlock.transform.parent = collectablesParent.transform;
                }
            }
        }
    }
}