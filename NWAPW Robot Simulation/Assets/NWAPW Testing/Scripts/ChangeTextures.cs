using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextures : MonoBehaviour
{
    public Material mat1;
    public Material mat2;
    public bool matType = false;
    public void ChangeTexture() { 
            if (matType)
            {
                gameObject.GetComponent<Renderer>().material = mat1;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material = mat2;
            }
            matType = !matType;
        
    }
}
