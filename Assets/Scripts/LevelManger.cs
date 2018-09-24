using UnityEngine;
using System.Collections;

public class LevelManger : MonoBehaviour {

	// Use this for initialization
	//public string lvl;
	public void LoadLvl(string lvl){
		if (lvl == "Quit")
			Application.Quit ();
		else
		Application.LoadLevel (lvl);
	} 
}
