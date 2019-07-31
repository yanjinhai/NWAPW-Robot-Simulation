using UnityEngine;
using System.Collections;

public class TriggerChangeMaterial : MonoBehaviour
{
    public Material fadeMaterial;
    public GameObject cam;
    Material tempMat;
    GameObject tempBlocked;
    bool flag = false;
    int layerMask;

    private void Start()
    {
        layerMask = 1 << 9;
        layerMask = ~layerMask;
    }

    private void Update()
    {

        if (!cam.activeSelf && flag)
        {
            tempBlocked.GetComponent<Renderer>().material = tempMat;
            tempBlocked = null;
            tempMat = null;
            flag = false;
        } 

        else if (cam.activeSelf && Physics.Raycast(cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height/4, 0)), out RaycastHit rayInfo,999, layerMask))
        {
            if (rayInfo.transform.gameObject.tag != "Player" && tempMat == null)
            {
                tempBlocked = rayInfo.transform.gameObject;
                tempMat = tempBlocked.GetComponent<Renderer>().material;
                tempBlocked.GetComponent<Renderer>().material = fadeMaterial;
                flag = true;
            }
            else if (rayInfo.transform.gameObject.tag == "Player" && flag)
            {
                tempBlocked.GetComponent<Renderer>().material = tempMat;
                tempBlocked = null;
                tempMat = null;
                flag = false;
            }
        }
    }
}