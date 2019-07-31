using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracking : MonoBehaviour
{
    private int score;
    private int basketScore;
    private int stackScore;

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateScore();
        scoreText.text = "Score: " + score;
    }

    private void CalculateScore()
    {
        stackScore = 0;
        // Creates a list of all collectables
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("CollectableObject");
        // Prevent the robot from going after blocks already stacked. 
        foreach (GameObject collectible in collectibles)
        {
            // Check if the collectible is a block.
            BlockScript blockScript = collectible.GetComponent<BlockScript>();
            if (blockScript != null)
            {
                // Check if the collectible is stacked already.
                if (blockScript.CheckState())
                {
                    // Increase stack score
                    stackScore++;
                }
            }
        }

        score = stackScore + basketScore;
    }

    public void scoreBasket (){
        basketScore++;
    }

    public int GetScore() {
        return score;
    }
}
