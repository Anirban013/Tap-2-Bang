using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject ball;
	public AudioSource audSrc;
	public AudioClip src;
   
	//public float score;
	// Use this for initialization
	void Start(){
        
		//ball = GameObject.FindObjectOfType<Ball> ();
	}
	void Update(){
		if (ball == null)
			Application.LoadLevel ("Win_Scene");
	}

	void OnCollisionEnter2D(Collision2D coll){
		//print ("Collided");
		print(coll.gameObject.tag);
        if (coll.gameObject.tag == "Player")
        {
            if (!Powers.ShieldActivated)
            {

                Ball.isJumpd = false;
                BackgroundScroll.startd = false;
                //coll.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 55;
                //coll.gameObject.GetComponent<CircleCollider2D> ().sharedMaterial.bounciness=0;
                Destroy(GameObject.FindGameObjectWithTag("Spark"));
                Destroy(coll.gameObject.GetComponent<CircleCollider2D>());
                Destroy(coll.gameObject.GetComponent<Rigidbody2D>());
                Destroy(coll.gameObject.GetComponent<Ball>());
                coll.gameObject.GetComponent<Animator>().SetTrigger("Collided");
                coll.gameObject.transform.position = new Vector3(coll.gameObject.transform.position.x, -4.2f, 0);

                coll.gameObject.transform.rotation = Quaternion.identity;
                coll.gameObject.transform.localScale = new Vector3(1, 2.2f, 1);
                audSrc.clip = src;
                audSrc.Play();



                Ball.t = 0.0f;

                if (Score.score > PlayerPrefs.GetFloat("highScore"))
                {
                    Debug.Log("HS :" + PlayerPrefs.GetFloat("highScore"));
                    PlayerPrefs.SetFloat("highScore", Score.score);
                    PlayerPrefs.Save();
                }


                //Application.LoadLevel ("Win_Scene");
                GameObject.Destroy(coll.gameObject, 0.9f);

            }
            else
                Ball.isJumpd = false;
        }
		//GameObject.Destroy(coll.gameObject);
	}

}
