using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && GameObject.FindGameObjectWithTag("TogglePlace").GetComponent<TogglePlace>().place && gameObject.transform.parent.name == "Collectables")
        {
            Destroy(gameObject);
        }
    }
}   