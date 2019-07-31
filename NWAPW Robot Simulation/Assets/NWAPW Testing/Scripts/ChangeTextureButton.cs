using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextureButton : MonoBehaviour
{
    public void ChangeTexturesButton()
    {
        GameObject[] enviorments = GameObject.FindGameObjectsWithTag("Enviorment");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] dropAreas = GameObject.FindGameObjectsWithTag("Drop Area");
        foreach (GameObject enviorment in enviorments)
        {
            enviorment.GetComponent<ChangeTextures>().ChangeTexture();
        }
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<ChangeTextures>().ChangeTexture();
        }
        foreach (GameObject dropArea in dropAreas)
        {
            dropArea.GetComponent<ChangeTextures>().ChangeTexture();
        }

    }
}
