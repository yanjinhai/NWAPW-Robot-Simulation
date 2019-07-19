﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableRemove : MonoBehaviour
{
    public Text text;
    public int score = 0;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CollectableObject")
        {
            score++;
            text.text = "Score: " + score;
            Destroy(collision.gameObject);
        }
    }
}
