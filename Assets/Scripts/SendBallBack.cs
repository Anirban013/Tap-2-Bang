using UnityEngine;
using System.Collections;

public class SendBallBack : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {

            coll.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1, -5f);
        }
    } 
}
