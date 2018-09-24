using UnityEngine;
using System.Collections;

public class MagnetPower : MonoBehaviour {

    
    void OnCollisionEnter2D(Collision2D coll)
    {
       Powers.Reset();
        Destroy(this.gameObject);

        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("Magnet Power");
           Powers.MagnetActivated = true;
        }
    }
}
