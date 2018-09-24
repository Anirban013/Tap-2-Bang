using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

    public static float t = 0;
	// Use this for initialization
	void Update () {
        t += Time.deltaTime;
        print((int)t);
        if((int)t == 1)
        Application.LoadLevel("Start_Scene");
	
	}
	
	// Update is called once per frame
	
}
