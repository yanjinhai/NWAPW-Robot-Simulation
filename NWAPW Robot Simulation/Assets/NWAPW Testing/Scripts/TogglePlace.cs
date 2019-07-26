using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlace : MonoBehaviour
{
    public bool place = true;
    public void togglePlace() {
        place = !place;
    }
}
