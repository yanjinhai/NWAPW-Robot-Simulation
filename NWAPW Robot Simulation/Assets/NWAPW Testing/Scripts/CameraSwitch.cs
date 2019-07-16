using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject FirstPerson;
    public GameObject ThirdPerson;
    public GameObject TopView;
    public int camMode;
    public int test;
    // Start is called before the first frame update
    void Start()
    {
        camMode = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switchCamera();
    
    }
    void switchCamera() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            camMode++;
            if (camMode > 2) {
                camMode = 0;
            }
            if (camMode == 0)
            {
   
                TopView.SetActive(true);
                ThirdPerson.SetActive(false);
                FirstPerson.SetActive(false);
            }
            else if (camMode == 1)
            {
                FirstPerson.SetActive(false);
                ThirdPerson.SetActive(true);
                TopView.SetActive(false);
            }
            else if (camMode == 2)
            {
                FirstPerson.SetActive(true);
                ThirdPerson.SetActive(false);
                TopView.SetActive(false);

            }
        }
    }
}
