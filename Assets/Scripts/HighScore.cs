using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class HighScore : MonoBehaviour {
    public Texture leaderboardBtn;
    public Texture achievmentsBtn;
    public Texture logoutBtn;
    public Texture loginBtn;
    private string testAchievement = "CgkIuKz91ZEWEAIQAw";
    private string testLeaderBoard = "CgkIuKz91ZEWEAIQAg";
    private Rect leaderBoardwindowRect = new Rect(Screen.width / 2 -146, Screen.height / 2 -70 , 300, 340);
    //private Rect loginwindowRect = new Rect(Screen.width / 2 - 80, Screen.height / 2 - 30, 100, 100);
    private Rect loginwindowRect = new Rect(Screen.width / 2 - 80, Screen.height / 2 -30, 160, 150);
    //private Rect achievmentsBoardwindowRect = new Rect(Screen.width / 2 - 165, Screen.height / 2 + 50, 96, 95);
    // private Rect logoutwindowRect = new Rect(Screen.width / 2 - 140, Screen.height / 2 + 50, 180, 130);
    // Use this for initialization
    public void Start() {
        Debug.Log("width :" + (Screen.width- 50));

        if (Social.localUser.authenticated)
        {
            //Social.LoadAchievements(OnLoadAC);
            Social.ReportScore((long)PlayerPrefs.GetFloat("highScore"), testLeaderBoard, OnSubmitScore);
        }
    }
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<TextMesh>().text = "High Score :"+(long)PlayerPrefs.GetFloat("highScore")+" Sec";
	}

    void OnGUI()
    {
     if (Social.localUser.authenticated)
        {
            //windowRect.position = new Vector2 (0f,0f);
            leaderBoardwindowRect = GUI.Window(0, leaderBoardwindowRect, DoMyWindow, "");


            // achievmentsBoardwindowRect = GUI.Window(1, achievmentsBoardwindowRect, DoMyWindow, "");
            //logoutwindowRect = GUI.Window(2, logoutwindowRect, DoMyWindow, "sdasd");

        }
      else
            loginwindowRect = GUI.Window(1, loginwindowRect, SignInWindow, "");
    }
    void DoMyWindow(int windowID)
    {


        // GUILayout.Label("Tap anywhere to give direction to the pumpkin.\nYou can give direction only after the pumpkin touches the platform.\nYour primary objecive is to prevent pumpkin from falling into the fire.\n");
        //GUI.skin = guiskin;

        //if (windowID == 0)
        {
            if (GUILayout.Button(leaderboardBtn, GUILayout.Height(100)))
            {
                //NerdGPG.GPG_ShowLeaderBoards(testLeaderBoard);
                NerdGPG.Instance().showLeaderBoards(testLeaderBoard);
            }
        }
        //else if(windowID == 1) 
        {
            if (GUILayout.Button(achievmentsBtn, GUILayout.Height(100)))
            {
                //	NerdGPG.GPG_ShowAchievements();
                //NerdGPG.Instance().showAchievements();
                Social.ShowAchievementsUI();
            }
          //  else
            {
                if (GUILayout.Button(logoutBtn, GUILayout.Height(100)))
                {
                    //NerdGPG.GPG_SignOut();
                  
                    NerdGPG.Instance().signOut();
                    SceneManager.LoadScene("Win_Scene");
                }
            }
        }/*
             if (GUILayout.Button("GPG_UnlockAC", GUILayout.Height(120)))
             {
                 Social.ReportProgress(testAchievement, 100.0, OnUnlockAC);
             }*/

    }
    void SignInWindow(int windowId)
    {
        if (GUILayout.Button(loginBtn, GUILayout.Height(120)))
        {
            //NerdGPG.GPG_SignOut();
            Social.localUser.Authenticate((OnAuthCB) => {
                if (OnAuthCB)
                {
                    Social.LoadAchievements(OnLoadAC);
                    SceneManager.LoadScene("Win_Scene");
                    Debug.Log("You've successfully logged in");
                }
                else {
                    Debug.Log("Login failed for some reason");
                }
            });
        }

    }

    public void OnSubmitScore(bool result)
    {
        Debug.Log("GPGUI: OnSubmitScore: " + result);
    }
    public void OnUnlockAC(bool result)
    {
        Debug.Log("GPGUI: OnUnlockAC " + result);
    }
    public void OnLoadAC(IAchievement[] achievements)
    {
        Debug.Log("GPGUI: Loaded Achievements: " + achievements.Length);
    }
    void OnAuthCB(bool result)
    {

        Debug.Log("GPGUI: Got Login Response: " + result);
    }
  
}
