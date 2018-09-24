using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {


	public bool startd;
	public float force = 0.2f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		startd = BackgroundScroll.startd;
		if (startd) {
			//this.GetComponent<Rigidbody2D> ().gravityScale = 0;
			//this.GetComponent<Rigidbody2D> ().AddForce (Vector2.down*force);

			this.transform.position -=this.transform.up * Time.deltaTime * 1;
			//this.GetComponent<Rigidbody2D> ().;
			//this.GetComponent<Rigidbody2D> ().AddTorque (2.5f);
		}

	}


}
