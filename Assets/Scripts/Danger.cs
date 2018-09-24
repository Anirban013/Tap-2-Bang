using UnityEngine;
using System.Collections;

public class Danger : MonoBehaviour {

   
    void OnCollisionEnter2D(Collision2D coll)
    {
        Powers.Reset();
        Destroy(this.gameObject);

        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("Danger");
            Powers.DangerActivated = true;
        }
    }
}
