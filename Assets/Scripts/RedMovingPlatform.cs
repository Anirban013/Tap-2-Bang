using UnityEngine;
using System.Collections;

public class RedMovingPlatform : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
            Ball.isJumpd = false;
        if (coll.gameObject.tag == "Finish")
        {
            Destroy(this.gameObject);
        }

        //GameObject.Destroy (this);
    }
}
