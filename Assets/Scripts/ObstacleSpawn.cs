using UnityEngine;
using System.Collections;

public class ObstacleSpawn : MonoBehaviour {


	public bool startd;
	public static float force = 1.0f;
	//public static bool startRotate = false;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {


		startd = BackgroundScroll.startd;
		if (startd) {

			//this.GetComponent<Rigidbody2D> ().AddForce (Vector2.down*force);
			this.transform.position -=this.transform.up * Time.deltaTime * force;
			//if(startRotate)
				//this.GetComponent<Rigidbody2D> ().AddTorque (2.5f);
		}

		}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player")
			Ball.isJumpd=false;
		if (coll.gameObject.tag == "Finish") {
			Destroy (this.gameObject);
		}

			//GameObject.Destroy (this);
	}



	}

