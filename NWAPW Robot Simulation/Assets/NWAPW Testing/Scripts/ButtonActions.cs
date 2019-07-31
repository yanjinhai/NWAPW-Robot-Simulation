using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    bool teleop = true;
    bool autonomous = false;

    public bool place = true;
    public bool spawnSecond = false;

    public void SwitchScene()
    {
        teleop = !teleop;
        autonomous = !autonomous;
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<RobotAI>().enabled = autonomous;
        Player.GetComponent<RobotMovement>().enabled = autonomous;
        Player.GetComponent<PlayerMovement>().enabled = teleop;
    }

    public void TogglePlace()
    {
        place = !place;
    }

    public void PlaceSecondary()
    {
        spawnSecond = !spawnSecond;
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("TeleopAutonomous");
    }

    public GameObject[] Cameras;
    public GameObject CurrentCam;

    private void Start()
    {
        CurrentCam = Cameras[Cameras.Length-1];
        switchCamera(); 
    }

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
