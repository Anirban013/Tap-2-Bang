using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
    static int m = 0;
	// Use this for initialization
	void Start () {
        GameObject.DontDestroyOnLoad(gameObject);
        m++;
        if(m>1)
        {
            GameObject.Destroy(gameObject);
        }
        Debug.Log(m);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
