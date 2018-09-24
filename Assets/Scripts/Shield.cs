using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
   

    void OnCollisionEnter2D(Collision2D coll)
    {
       // _power.Reset();
        Destroy(this.gameObject);

        if (coll.gameObject.tag == "Player")
        {
            Powers.ShieldActivated = true;
            Debug.Log("Shield Power");
           
            Ball._shieldTime = (int)Score.score;
        }
    }
}
