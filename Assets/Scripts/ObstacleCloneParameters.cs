using UnityEngine;

using System.Collections;

public class ObstacleCloneParameters : MonoBehaviour {

	public GameObject[] platforms;
	public float[] wCords;
	public float[] rCords;
	public float[] gCords;
   
	

    public void Start() {
       // Debug.Log("asdasdasdasdasdasdddddd");
        //if(BackgroundScroll.startd)
       
    }

    

	/*void start(){
		wCords = new float[4];
		gCords = new float[3];
		rCords = new float[5];

		wCords [0]= 1.73f;
		wCords [1] = -1.89f;
		wCords [2] = -0.91f;
		wCords [3] = 0.87f;

		rCords [0]= 2.04f;
		rCords [1] = 1.19f;
		rCords [2] = 0.36f;
		rCords [3] = -0.87f;
		rCords [4]= -1.84f;

		gCords [0] = 1.45f;
		gCords [1] = 0.56f;
		gCords [2] = -1.84f;
	}*/

	/*void OnTriggerExit2D(Collider2D coll){
		
		print ("Green "+gCords [0]);
		print (this.gameObject.tag);
		if (this.gameObject.tag == "RedPlatform") {
			x = rCords[Random.Range (0, 4)];
			print ("X " + rCords);
			CircleCollider2D ccl1 = this.gameObject.AddComponent<CircleCollider2D> ();
			CircleCollider2D ccl2 = this.gameObject.AddComponent<CircleCollider2D> ();
			ccl1.offset = new Vector2 (3.36f, 0f); 
			ccl1.radius = 1.01f;
			ccl2.offset = new Vector2 (-3.36f, 0f);
			ccl2.radius = 1.01f;
		} else if (this.gameObject.tag == "LongGreenPlatform") {
			
			x = gCords[Random.Range (0, 2)];

			print ("X " + gCords);

			CircleCollider2D ccl1 = this.gameObject.AddComponent<CircleCollider2D> ();
			CircleCollider2D ccl2 = this.gameObject.AddComponent<CircleCollider2D> ();
			ccl1.offset = new Vector2 (3.59f, 0f); 
			ccl1.radius = 0.77f;
			ccl2.offset = new Vector2 (-3.59f, 0f);
			ccl2.radius = 0.77f;
		} else if (this.gameObject.tag == "Platform") {
			x = wCords[Random.Range (0, 3)];
			print ("X " + wCords);
		}
		else if (this.gameObject.tag == "Player") {
		
			coll.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (1, -5f);
		}
		//print ("trigg");
		//ObstacleSpawn.startRotate = true;
		int i=Random.Range(0,3);
		print ("index " + i);
		GameObject clone = platforms[i];

		//GameObject clone = platforms[Random.Range(0,2)];
		//print (clone);
		Instantiate (clone, new Vector3 (x, 6.5f ,0f), Quaternion.identity);


	}*/

}
