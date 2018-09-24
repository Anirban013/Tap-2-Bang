using UnityEngine;
using System.Collections;

public class BounceIncrease : MonoBehaviour {

    
    void OnCollisionEnter2D(Collision2D coll)
    {
        Powers.Reset();
        Destroy(this.gameObject);

        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("Bounce Power");
            Powers.BounceActivated = true;
        }
    }
}
