using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevMobScript : MonoBehaviour {

    private static readonly Dictionary<String, String> REVMOB_APP_IDS = new Dictionary<String, String>() {
        { "Android", "575beb868ffef6705da3354c"},
        { "IOS", "paste_your_RevMob_Media_ID_for_ios_here" }
    };
    private RevMob revmob;
    void Awake()
    {
        revmob = RevMob.Start(REVMOB_APP_IDS, "Your_GameObject_name");
    }
    void Start () {
        revmob.ShowFullscreen();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
