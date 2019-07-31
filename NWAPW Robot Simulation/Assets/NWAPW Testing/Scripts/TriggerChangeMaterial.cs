using UnityEngine;
using System.Collections;

public class TriggerChangeMaterial : MonoBehaviour
{

    public Material fadeMaterial;
    public GameObject cam;
    Material tempMat;
    GameObject tempBlocked;
    bool flag = false;

   /* void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player has entered the trigger");

        if (other.gameObject.tag == "Player")
        {
            GetComponent<Renderer>().material = fadeMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Player has exited the trigger");

        if (other.gameObject.tag == "Player")
        {
            GetComponent<Renderer>().material = baseMaterial;
        }
    }*/

    private void Update()
    {
        RaycastHit rayInfo;

        if (!cam.activeSelf && flag)
        {
            tempBlocked.GetComponent<Renderer>().material = tempMat;
            tempBlocked = null;
            tempMat = null;
        }

        else if (Physics.Raycast(cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, 200, 0)), out rayInfo))
        {
            if (rayInfo.transform.gameObject.tag != "Player" && tempMat == null)
            {
                print("blocked");
                tempBlocked = rayInfo.transform.gameObject;
                tempMat = tempBlocked.GetComponent<Renderer>().material;
                tempBlocked.GetComponent<Renderer>().material = fadeMaterial;
                flag = true;
            }
            else if (rayInfo.transform.gameObject.tag == "Player")
            {
                tempBlocked.GetComponent<Renderer>().material = tempMat;
                tempBlocked = null;
                tempMat = null;
            }
        }
    }
}