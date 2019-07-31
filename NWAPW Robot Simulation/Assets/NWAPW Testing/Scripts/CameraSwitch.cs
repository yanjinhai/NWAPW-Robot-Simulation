using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject[] Cameras;
    public GameObject CurrentCam;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            switchCamera();
        }
    }


    public void switchCamera()
    {
        int index = System.Array.IndexOf(Cameras, CurrentCam);
        if (index == Cameras.Length - 1)
        {
            index = -1;
        }
        CurrentCam = Cameras[index + 1];
        foreach (GameObject cam in Cameras)
        {
            cam.SetActive(false);
        }
        Cameras[index + 1].SetActive(true);
    }
}

