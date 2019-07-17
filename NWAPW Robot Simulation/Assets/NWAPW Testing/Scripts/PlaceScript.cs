using UnityEngine;
using System.Collections;

public class PlaceScript : MonoBehaviour
{

    public GameObject Sphere;
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    public GameObject CameraSwitcher;
    GameObject Camera;

    
    void Update()
     {

         if (Input.GetButtonDown("Fire1"))
         {
             int active = CameraSwitcher.GetComponent<CameraSwitch>().camMode;
             switch (active)
             {
                 case (0):
                     Camera = Camera1;
                     break;
                 case (1):
                     Camera = Camera2;
                     break;
                 case (2):
                     Camera = Camera3;
                     break;
             }
            RaycastHit rayInfo;

            if (Physics.Raycast(Camera.GetComponent<Camera>().ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), out rayInfo))
            {
                Instantiate(Sphere, rayInfo.point, Quaternion.identity);
            }
        }

     }
    
}