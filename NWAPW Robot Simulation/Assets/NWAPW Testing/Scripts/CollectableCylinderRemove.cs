using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableCylinderRemove : MonoBehaviour
{
    public Text text;
    public int score = 0;
    GameObject obj = null;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CollectableObject" &&
            collision.gameObject.transform.position.y > transform.position.y +
            GetComponent<CapsuleCollider>().bounds.size.y / 2)
        {
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            score++;
            text.text = "Score: " + score;
        }
    }
}
