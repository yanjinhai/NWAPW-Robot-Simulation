using UnityEngine;
using System.Collections;

public class PlaceScript : MonoBehaviour
{

    public GameObject Collectable;
    public GameObject CollectablesParent;
    public GameObject CameraSwitcher;
    GameObject CurrentCam;

    
    void Update()
     {

         if (Input.GetButtonDown("Fire1") && GameObject.FindGameObjectWithTag("TogglePlace").GetComponent<TogglePlace>().place)
         {
            CurrentCam = CameraSwitcher.GetComponent<CameraSwitch>().CurrentCam;

            RaycastHit rayInfo;

            if (Physics.Raycast(CurrentCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), out rayInfo))
            {
                Vector3 vectorHit = rayInfo.point;
                vectorHit = new Vector3(vectorHit.x, vectorHit.y + 0.5f, vectorHit.z);
                GameObject newBall = Instantiate(Collectable, vectorHit, Quaternion.identity);
                newBall.transform.parent = CollectablesParent.transform;
            }
        }
    }
    
}