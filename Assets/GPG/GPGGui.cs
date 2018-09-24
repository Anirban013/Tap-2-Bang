using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms;
	
public class GPGGui : MonoBehaviour {
	
	private enum GPLoginState {loggedout, loggedin};
	private GPLoginState m_loginState = GPLoginState.loggedout;
	bool needFullSignin = false;
	private string dataToSave = "Hello World";

    private string testLeaderBoard = "< GPG Leaderboard ID >";
    private string testAchievement = "< Unlock Achievement ID >";
    private string testIncAchievement = "< Incremental Achievement ID >";
    private int testIncACTotalSteps = 20;

    double currACPercent = -1;
    double onReportACPercent = 0;

	// Use this for initialization
	void Start () {
        Social.Active = new UnityEngine.SocialPlatforms.GPGSocial();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUILayout.Label("Unity GPG Plugin Demo");
		GUILayout.Space(20);
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		
		if(GUILayout.Button("SignIn", GUILayout.Height(120))) {
			Debug.Log("Signin called");
			//needFullSignin = !NerdGPG.GPG_TrySilentSignIn();
			//NerdGPG.Instance().signIn();

            Social.localUser.Authenticate(OnAuthCB);
		}
		/*
		if(needFullSignin) {
			if(GUILayout.Button("SignIn", GUILayout.Height(60))) {
				Debug.Log("Signin called");
				NerdGPG.GPG_SignIn();
			}
		}*/
		if(m_loginState == GPLoginState.loggedin) {
            if (GUILayout.Button("SignOut", GUILayout.Height(120))) {
				//NerdGPG.GPG_SignOut();
				NerdGPG.Instance().signOut();
			}
		}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		if(Social.localUser.authenticated) {
			// we are logged in. we can do leaderboard and achievement and cloud stuff
            if (GUILayout.Button("GPG_ShowAllLeaderBoards", GUILayout.Height(120))) {
				//NerdGPG.GPG_ShowAllLeaderBoards();
				//NerdGPG.Instance().showAllLeaderBoards();
				//NerdGPG.Instance().loadAchievements(true);
                Social.ShowLeaderboardUI();
			}
            if (GUILayout.Button("GPG_ShowLeaderBoards", GUILayout.Height(120))) {
				//NerdGPG.GPG_ShowLeaderBoards(testLeaderBoard);
				NerdGPG.Instance().showLeaderBoards(testLeaderBoard);
			}

            if (GUILayout.Button("GPG_SubmitScore", GUILayout.Height(120))) {
			//	NerdGPG.GPG_SubmitScore(testLeaderBoard,80);
				//NerdGPG.Instance().submitScore(testLeaderBoard,80, null);
                Social.ReportScore(80, testLeaderBoard, OnSubmitScore);
			}

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GPG_ShowAchievements", GUILayout.Height(120))) {
                //	NerdGPG.GPG_ShowAchievements();
                //NerdGPG.Instance().showAchievements();
                Social.ShowAchievementsUI();
            }

            // See if we have loaded the achievement list
            if (NerdGPG.Instance().acList == null)
                GUI.enabled = false;
            else {
                // Check if the achievement we are trying to increment is in the ac list
                foreach (IAchievement ac in NerdGPG.Instance().acList) {
                    if (ac.id == testIncAchievement) {
                        currACPercent = ac.percentCompleted;
                    }
                }
            }

            // If we didnt find the achievement or if its already unlocked disable the increment button
            if (currACPercent < 0 || currACPercent >= 100) {
                GUI.enabled = false;
            }

            if (GUILayout.Button("GPG_IncrementAC- " + (currACPercent) + "%", GUILayout.Height(120))) {
                Debug.Log("Clicked On Increment Button");
                int stepsCompleted = (int)(currACPercent * (double)testIncACTotalSteps / 100.0f);
                stepsCompleted += 1;
                onReportACPercent = stepsCompleted * 100.0f / testIncACTotalSteps;
                Debug.Log("Increment by : " + onReportACPercent);
                Social.ReportProgress(testIncAchievement, onReportACPercent, OnSubmitAC);
			}

            GUI.enabled = true;

            if (GUILayout.Button("GPG_UnlockAC", GUILayout.Height(120))) {
                Social.ReportProgress(testAchievement, 100.0, OnUnlockAC);
            }

            if (GUILayout.Button("GPG_LoadACDesc", GUILayout.Height(120))) {
                Social.LoadAchievementDescriptions(OnLoadACDesc);

            }
            if (GUILayout.Button("GPG_LoadAC", GUILayout.Height(120))) {
                Social.LoadAchievements(OnLoadAC);
            }
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();	
			dataToSave = GUILayout.TextField(dataToSave,100);
            if (GUILayout.Button("GPG_SaveToCloud", GUILayout.Height(120))) {
				Debug.Log("Saving to cloud");
				byte[] bytes = new byte[dataToSave.Length * sizeof(char)];
    			System.Buffer.BlockCopy(dataToSave.ToCharArray(), 0, bytes, 0, bytes.Length);
				NerdGPG.Instance().saveToCloud(0,bytes);
			//	NerdGPG.GPG_SaveToCloud(0,bytes,bytes.Length);
			}
            if (GUILayout.Button("GPG_LoadFromCloud", GUILayout.Height(120))) {
				
				Debug.Log("Loading from cloud for key 0");
//				NerdGPG.GPG_LoadFromCloud(0,bytes,bytes.Length);
				NerdGPG.Instance().loadFromCloud(0);
//				GCHandle handle = GCHandle.Alloc(key0CloudData,GCHandleType.Pinned);
			//	NerdGPG.GPG_LoadFromCloud(0,handle.AddrOfPinnedObject(),key0CloudData.Length);
				//handle.Free();
			}
		}
		GUILayout.EndHorizontal();
        if (GUILayout.Button("GPG_HasAuthoriser", GUILayout.Height(120))) {
			Debug.Log("HasAuthoriser result "+NerdGPG.Instance().isSignedIn());
		}
        
        if (Social.localUser.authenticated)
            GUILayout.Label(Social.localUser.userName + " is signed in. ID: " + Social.localUser.id);

		GUILayout.EndVertical();
	}

    void OnAuthCB(bool result)
    {
        Debug.Log("GPGUI: Got Login Response: " + result);
    }

    public void OnLoadAC(IAchievement[] achievements)
    {
        Debug.Log("GPGUI: Loaded Achievements: " + achievements.Length);
    }

    public void OnLoadACDesc(IAchievementDescription[] acDesc)
    {
        Debug.Log("GPGUI: Loaded Achievement Description: " + acDesc.Length);
    }

    public void OnSubmitScore(bool result)
    {
        Debug.Log("GPGUI: OnSubmitScore: " + result);
    }

    public void OnSubmitAC(bool result)
    {
        Debug.Log("GPGUI: OnSubmitAchievement " + result);
        if (result == true) {
            NerdGPG.Instance().haveLoadedAc = false;
            Social.LoadAchievements(OnLoadAC);
        }
    }

    public void OnUnlockAC(bool result)
    {
        Debug.Log("GPGUI: OnUnlockAC " + result);
    }

	public void GPGAuthResult(string result)
	{
		// success/failed
		if(result == "success") {
			m_loginState = GPLoginState.loggedin;
		} else 
			m_loginState = GPLoginState.loggedout;
	}
	
	public void OnGPGCloudLoadResult(string result)
	{
		// result is in the format result;keyNum;length
		// where result is either success/conflict/error
		// keyNum is the key for which this result is 0-3 range as per GPG
		// length is the length of data received from GPG Cloud. Important for binary data handling
		// NOTE: In this code we are only saving/loading STRING data. but it should be fine to use it for any binary data
		Debug.Log("OnGPGCloudLoadResult "+result);
		string[] resArr = result.Split(';');
		if(resArr.Length<3)
		{
			Debug.LogError("Length of array after split is less than 3");	
			return; // weird stuff
		}
		int keyNum = System.Convert.ToInt16(resArr[1]);
		if(resArr[0]=="success") {
			// lets see what our data holds.
			byte[] data = NerdGPG.Instance().getKeyLoadedData(keyNum);
			string str = System.Text.Encoding.Unicode.GetString(data);
			Debug.Log("Data read for key "+ resArr[1] + " is " + str + " with len "+ resArr[2] + " and converted string length is "+ str.Length);
			dataToSave = str;
		}
	}
	
	public void OnGPGCloudSaveResult(string result)
	{
		// result is in the format result;keyNum
		// where result is either success/conflict/error
		// keyNum is the key for which this result is 0-3 range as per GPG
		
		Debug.Log("GPG CloudSaveResult "+result);
		string[] resArr = result.Split(';');
		if(resArr.Length<3)
		{
			Debug.LogError("Length of array after split is less than 3");	
			return; // weird stuff
		}

	}
}
