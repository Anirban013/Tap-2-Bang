using UnityEngine;
using System.Collections;

public class BackgroundScroll : MonoBehaviour {


	public static bool startd = false;
	// Use this for initialization
	private Rect windowRect = new Rect(0 + 10,0 + 20, Screen.width - 25, Screen.height - 25);
	public static bool tut = true;
	//public GUISkin guiskin;
	public GUIStyle style;
    public Texture closeButton;
	public GUIStyle txtStyle;
	//public GUIContent title;
	public static string t = "INSTRUCTIONS";

    void Awake()
    {
        Powers.Reset();
    }

	void Start () {
		if (PlayerPrefs.GetString ("showInstruction") == "no")
			tut = false;
		//PlayerPrefs.DeleteAll ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Ball.isPaused && tut == false) {
			if (Input.touchSupported) {
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
					startd = true;
                    this.gameObject.GetComponent<ObstacleClone>().enabled = true;
                    //Ball.p = true;
				}
			} else {
				if (Input.GetMouseButtonDown (0)) {
					startd = true;
                    this.gameObject.GetComponent<ObstacleClone>().enabled = true;
                   // Ball.p = true; 

                }
		
			}


		}
	
			//Time.timeScale = 1;
		//renderWindow();

	}
	void OnLevelWasLoaded(){
		startd = false;
		ObstacleSpawn.force = 1.0f;
		//tut = false;
	}
	void renderWindow(){
			OnGUI ();
	}

	void OnGUI() {
		if (tut) {
			//windowRect.position = new Vector2 (0f,0f);
			windowRect = GUI.Window (0, windowRect, DoMyWindow, t, style);

		}
	}
	void DoMyWindow(int windowID) {

		GUILayout.Label ("\n\n\nTap anywhere to give direction to the pumpkin."+
            "\nYou can give direction only after the pumpkin touches the platform.\nYour primary objecive is to prevent pumpkin from falling into the fire.\n\n\n"
            + "There are Power Ups That will make your game either easier or tougher -\n\n 1. Shield: Will extinguish the fire for 5 seconds." +
            "\n\n2. Magnets: Will prevent the pumpkin to bounce of the platforms."
            + "\n\n3. Bounce Increase: It will increase the bounciness of the pumpkin.", txtStyle);
        //GUILayout.BeginScrollView(new Vector2(0 + 10, 0 + 20), txtStyle);
		//GUI.skin = guiskin;
		if (GUI.Button (new Rect (0 + 20, 0+(Screen.height - 150), 80, 80), closeButton)) {
			//print ("Got a click");
			tut = false;
			t = "";
			renderWindow ();
			//startd = false;


		}
		else if (GUI.Button (new Rect (Screen.width - 250, 0 + (Screen.height - 135), 200, 45), "Never Show this again!!!")) {
			PlayerPrefs.SetString ("showInstruction", "no");
			PlayerPrefs.Save ();
			tut = false;
			renderWindow ();
		}
	}

}
