using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableRemove : MonoBehaviour
{
    public Text text;
    public int score = 0;
    GameObject obj = null;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CollectableObject")
        {

            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<RobotAI>().everGrabbed) 
            {
                score++;
                text.text = "Score: " + score;
            }
            else {
                print("Redirected Block");
                GameObject.FindGameObjectWithTag("CollectableParent").GetComponent<RandomSpawn>().newObj();
            }


        }
    }
}
