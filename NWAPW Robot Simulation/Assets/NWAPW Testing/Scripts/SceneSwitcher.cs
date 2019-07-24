using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    bool teleop = true;
    bool autonomous = false;

    public void SwitchScene() {
        teleop = !teleop;
        autonomous = !autonomous;
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<RobotAI>().run = autonomous;
        Player.GetComponent<RobotMovement>().run = autonomous;
        Player.GetComponent<PlayerMovement>().run = teleop;
    }

}
