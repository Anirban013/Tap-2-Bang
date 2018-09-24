using UnityEngine;
using System.Collections;

using UnityEngine.SocialPlatforms;

public class SignIn : MonoBehaviour {

    
    public void Awake(){

		Social.Active = new UnityEngine.SocialPlatforms.GPGSocial();
       
        Social.localUser.Authenticate((OnAuthCB) => {
			if (OnAuthCB) {
                Social.LoadAchievements(OnLoadAC);
                Debug.Log("You've successfully logged in");
			} else {
				Debug.Log("Login failed for some reason");
			}
		});

		
	}
	void OnAuthCB(bool result)
	{
        
        Debug.Log("GPGUI: Got Login Response: " + result);
	}
    public void OnLoadAC(IAchievement[] achievements)
    {
        Debug.Log("GPGUI: Loaded Achievements: " + achievements.Length);
    }

}
