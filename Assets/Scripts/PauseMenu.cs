using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	public GameObject ball;
	public Text resume;
	public Text quit;
    public Camera cam;
    
	public void resumeGame(){
		Ball.isPaused = false;
        Ball.p = true;
        FollowPath.speed = 3f;
        BackgroundScroll.startd = true;
        ObstacleClone._activate = false;
        ObstacleClone.spawn = true;
        ball.GetComponent<Rigidbody2D>().isKinematic = false;
		resume.transform.position = new Vector3(5000f,1.68f,1f);
		quit.transform.position = new Vector3(5000f,1.68f,1f);
       StartCoroutine(Clone());

    }
	public void quitGame(){
		Application.Quit ();
	}

    IEnumerator Clone() {
        //yield return new WaitForSeconds(10);

        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        cam.GetComponent<ObstacleCloneParameters>().enabled = true;
    }
}
