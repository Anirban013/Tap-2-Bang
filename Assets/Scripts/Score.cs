using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	// Use this for initialization
	public static int score;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<TextMesh> ().text = "Score :"+score+" sec";
		if(score > 1 && ObstacleSpawn.force<1.8f){
		if (score % 30 == 0) {
				print ("Force"+ObstacleSpawn.force);
			ObstacleSpawn.force += 0.1f; 
			}
		}
	}
}
