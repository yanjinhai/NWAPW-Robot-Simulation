using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableCylinderRemove : MonoBehaviour
{
    public Text text;
    GameObject obj = null;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CollectableObject" &&
            collision.gameObject.transform.position.y > transform.position.y +
            GetComponent<CapsuleCollider>().bounds.extents.y)
        {
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            int score = int.Parse(text.text.Substring(6));
            score++;
            text.text = "Score: " + score;
        }
    }
}
