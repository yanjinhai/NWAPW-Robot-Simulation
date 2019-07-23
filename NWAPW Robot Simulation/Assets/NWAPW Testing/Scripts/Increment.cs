using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Increment : MonoBehaviour
{
    public int score = 0;
    public Text text;
    public void increase(int num) {
        score+=num;
        text.text = "Score: " + score;
    }
}
