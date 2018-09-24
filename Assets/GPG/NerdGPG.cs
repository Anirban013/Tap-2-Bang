using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

public class NerdGPG : MonoBehaviour {

	public const string appID = "760657958456";

    public struct ExtraACData
    {
        public int totalSteps;
        public int currSteps;

        public int type;
        public int state;
    };

    public List<GPGAchievementDescription> acDescList;
    public List<GPGAchievement> acList;
    public List<ExtraACData> acExtraData;

    // Hack to allow a better social implementation
    [System.NonSerialized]
    public bool haveLoadedAc = false;

	private static NerdGPG m_Instance;

    private enum GPLoginState { loggedout, loggedin };
    private GPLoginState m_loginState = GPLoginState.loggedout;

    private Action<bool> CBOnAuth = null;
    private Action<bool> CBSubmitScore = null;
    private Action<bool> CBUnlockAchievement = null;

    private Action onACLoadCB;

#if UNITY_IPHONE
	private byte[] key0CloudData = new byte[1024*128]; // 128k is max length. set it to what you think is you need
	private byte[] key1CloudData = new byte[1024*128]; // 128k is max length. set it to what you think is you need
	private byte[] key2CloudData = new byte[1024*128]; // 128k is max length. set it to what you think is you need
	private byte[] key3CloudData = new byte[1024*128]; // 128k is max length. set it to what you think is you need
#endif
	
#if UNITY_ANDROID
	private AndroidJavaObject mCurrentActivity;
    private AndroidJavaObject mNerdGPG = null;
#endif
	
	public static NerdGPG Instance() {
		if(m_Instance!=null)
			return m_Instance;
		Debug.Log("NerdGPG instance not found. Creating new one");

		GameObject owner = new GameObject("NerdGPG");
        m_Instance = owner.AddComponent<NerdGPG>();
		DontDestroyOnLoad(m_Instance);
		return m_Instance;
	}
	
#region PUBLIC_API
	public bool init() {
#if UNITY_IPHONE
		GPG_Init(appID + ".apps.googleusercontent.com");
		GPG_SetGameObjectName(name);
		return true;
#elif UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		mCurrentActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		Debug.Log("Current activity "+mCurrentActivity);
		mNerdGPG = new AndroidJavaObject ("com.nerdiacs.nerdgpgplugin.NerdGPG");
		if(mCurrentActivity!=null && mNerdGPG!=null) {
			mNerdGPG.Set<bool>("mDebugLog",true); // enable logging
			mNerdGPG.SetStatic<string>("gameObjectName",name);
			mNerdGPG.Call<bool>("init",mCurrentActivity);
			return true;
		}
		else 
			return false;
#endif
	}
	
	public bool signIn(Action<bool> cb) {
#if UNITY_IPHONE
		if(!GPG_TrySilentSignIn()) {
			GPG_SignIn();
			return true;
		} else {
			return true;
		}
#elif UNITY_ANDROID
		Debug.Log("mNerdGPG is " + mNerdGPG);
        if (mNerdGPG == null) {
            init();
        }

        CBOnAuth = cb;
		return mNerdGPG.Call<bool>("signIn");
#endif // UNITY_ANDROID
	}
	
	public void signOut() {
#if UNITY_IPHONE
		GPG_SignOut();
#elif UNITY_ANDROID
		mNerdGPG.Call("signOut");
#endif // UNITY_ANDROID
	}
	
	public void showAllLeaderBoards() {
#if UNITY_IPHONE
		GPG_ShowAllLeaderBoards();
#elif UNITY_ANDROID
		mNerdGPG.Call("showAllLeaderBoards");
#endif // UNITY_ANDROID		
	}
	
	public void showLeaderBoards(string leaderBoardId) {
#if UNITY_IPHONE
		GPG_ShowLeaderBoards(leaderBoardId);
#elif UNITY_ANDROID
		mNerdGPG.Call("showLeaderBoards",leaderBoardId);
#endif
	}
	
	public void showAchievements() {
#if UNITY_IPHONE
		GPG_ShowAchievements();
#elif UNITY_ANDROID
		mNerdGPG.Call("showAchievements");
#endif 
	}

	public void submitScore(string leaderBoardId, long score, Action<bool> cb) {
#if UNITY_IPHONE
		GPG_SubmitScore(leaderBoardId,score);
#elif UNITY_ANDROID
        CBSubmitScore = cb;
		mNerdGPG.Call("submitScore",leaderBoardId,score);
#endif
	}
	
	public bool isSignedIn() {
#if UNITY_IPHONE
		return GPG_HasAuthoriser();
#elif UNITY_ANDROID
		return mNerdGPG.Call<bool>("hasAuthorised");
#endif 
	}
	public void setGameObjectName(string gameObjectName) {
#if UNITY_IPHONE
		GPG_SetGameObjectName(gameObjectName);
#elif UNITY_ANDROID
		mNerdGPG.SetStatic<string>("gameObjectName",gameObjectName);
#endif 
	}

    public string getPlayerName()
    {
#if UNITY_IPHONE
        Debug.LogWarning("NerdGPG.GetPlayerName() is not valid for iOS");
		return null;
#elif UNITY_ANDROID
        return mNerdGPG.Call<string>("getPlayerName");
#endif
    }

    public string getPlayerID()
    {
#if UNITY_IPHONE
        Debug.LogWarning("NerdGPG.getPlayerID() is not valid for iOS");
		return null;
#elif UNITY_ANDROID
        return mNerdGPG.Call<string>("getPlayerID");
#endif
    }
	
	public void unlockAchievement(string achievementId, Action<bool> cb) {
#if UNITY_IPHONE
		GPG_UnlocAchievement(achievementId);
#elif UNITY_ANDROID
        CBUnlockAchievement = cb;
		mNerdGPG.Call("unlockAchievement",achievementId);
#endif
	}

	public bool loadAchievements(Action cb, bool bForceReload=false) {
#if UNITY_IPHONE
		Debug.LogWarning("Method loadAchievements not implemented for iOS");
		return true;
#elif UNITY_ANDROID
        if (haveLoadedAc == false)
            haveLoadedAc = true;
        else {
            cb();
            return false;
        }

        onACLoadCB = cb;
		mNerdGPG.Call("loadAchievements",bForceReload);
        return true;
#endif
	}
	
	public void incrementAchievement(string achievementId, int numSteps, Action<bool> cb) {
#if UNITY_IPHONE
		Debug.LogWarning("Method loadAchievements not implemented for iOS");
#elif UNITY_ANDROID
        CBUnlockAchievement = cb;
		mNerdGPG.Call("incrementAchievement",achievementId,numSteps);
#endif
	}
	
	
	public void saveToCloud(int keyNum, byte[] bytes)
	{
		if(bytes==null || bytes.Length<1) {
			Debug.LogError("Empty bytes param for saveToCloud");
			return; // no point 
		}
#if UNITY_IPHONE
		GPG_SaveToCloud(keyNum,bytes,bytes.Length);
#elif UNITY_ANDROID
		//object obj = AndroidJNI.ToByteArray(bytes);
		
		/*
		IntPtr id_saveToCloud = AndroidJNIHelper.GetMethodID(mNerdGPG.GetRawClass(),"saveToCloud","(I[B)V");
		Debug.Log("id_saveToCloud is "+id_saveToCloud);
		if(id_saveToCloud!=IntPtr.Zero) {
			IntPtr jbytearray_val = AndroidJNI.ToByteArray(bytes);
			Debug.Log("jbytearray_val "+jbytearray_val);
			object[] args = {1,jbytearray_val};
			AndroidJNI.CallVoidMethod(mNerdGPG.GetRawObject(),id_saveToCloud,AndroidJNIHelper.CreateJNIArgArray(args));
		}
		mNerdGPG.Call("saveToCloud",keyNum, AndroidJNI.ToByteArray(bytes));
		*/
		
		// There was no better way i could find to pass on bytearray. so using it as string converted to base64
		string strData = Convert.ToBase64String(bytes);
		mNerdGPG.Call("saveToCloud",keyNum,strData);
#endif
	}
	
	public void loadFromCloud(int keyNum)
	{
#if UNITY_IPHONE
		try {
			switch(keyNum) {
			case 0:
				GCHandle handle = GCHandle.Alloc(key0CloudData,GCHandleType.Pinned);	
				NerdGPG.GPG_LoadFromCloud(keyNum,handle.AddrOfPinnedObject(),key0CloudData.Length);
				handle.Free();
				break;
			case 1:
				GCHandle handle1 = GCHandle.Alloc(key1CloudData,GCHandleType.Pinned);	
				NerdGPG.GPG_LoadFromCloud(keyNum,handle1.AddrOfPinnedObject(),key1CloudData.Length);
				handle1.Free();
				break;
			case 2:
				GCHandle handle2 = GCHandle.Alloc(key2CloudData,GCHandleType.Pinned);	
				NerdGPG.GPG_LoadFromCloud(keyNum,handle2.AddrOfPinnedObject(),key2CloudData.Length);
				handle2.Free();
				break;
			case 3:
				GCHandle handle3 = GCHandle.Alloc(key3CloudData,GCHandleType.Pinned);	
				NerdGPG.GPG_LoadFromCloud(keyNum,handle3.AddrOfPinnedObject(),key3CloudData.Length);
				handle3.Free();
				break;
			}
		}
		catch (Exception e) {
				Debug.LogException(e);
		}
		
		Debug.LogError("Invlaid keynum "+ keyNum +" given or gc was unable to pin");
		
#elif UNITY_ANDROID
		mNerdGPG.Call("loadFromCloud",keyNum);
#endif
	}
	
	public byte[] getKeyLoadedData(int keyNum) 
	{
#if UNITY_IPHONE
		byte[] data = null;
		switch(keyNum) {
		case 0:
			data = key0CloudData;
			break;
		case 1:
			data = key1CloudData;
			break;
		case 2:
			data = key2CloudData;
			break;
		case 3:
			data = key3CloudData;
			break;
		}
		return data;
#elif UNITY_ANDROID
		/*System.IntPtr data = System.IntPtr.Zero;
		switch(keyNum) {
		case 0:
			data = mNerdGPG.Get<System.IntPtr>("mKey0Data");
			break;
		case 1:
			data = mNerdGPG.Get<System.IntPtr>("mKey1Data");
			break;
		case 2:
			data = mNerdGPG.Get<System.IntPtr>("mKey2Data");
			break;
		case 3:
			data = mNerdGPG.Get<System.IntPtr>("mKey3Data");
			break;
		}
		Debug.Log("Loaded data with ptr "+data);
		if(data!=System.IntPtr.Zero) {
			return AndroidJNI.FromByteArray(data);
		}
		*/
		
		string str = mNerdGPG.Call<string>("getLoadedData",keyNum);
		if(str!=null) {
			byte[] data = Convert.FromBase64String(str);
			return data;
		}
		
		// if we reach here then key was empty
		Debug.LogError("Empty data received from cloud???");
		return null; // nothing received.
#endif 
	}
	

#endregion	// PUBLIC_API

#region GPGCallbacks
    public void GPGAuthResult(string result)
    {
        // success/failed
        if (result == "success") {
            m_loginState = GPLoginState.loggedin;
        } else
            m_loginState = GPLoginState.loggedout;

        if (CBOnAuth != null)
            CBOnAuth(m_loginState == GPLoginState.loggedin);
    }

    public void OnGPGCloudLoadResult(string result)
    {
        // result is in the format result;keyNum;length
        // where result is either success/conflict/error
        // keyNum is the key for which this result is 0-3 range as per GPG
        // length is the length of data received from GPG Cloud. Important for binary data handling
        // NOTE: In this code we are only saving/loading STRING data. but it should be fine to use it for any binary data
        Debug.Log("OnGPGCloudLoadResult " + result);
        string[] resArr = result.Split(';');
        if (resArr.Length < 3) {
            Debug.LogError("Length of array after split is less than 3");
            return; // weird stuff
        }
        int keyNum = System.Convert.ToInt16(resArr[1]);
        /*if (resArr[0] == "success") {
            // lets see what our data holds.
            byte[] data = NerdGPG.Instance().getKeyLoadedData(keyNum);
            string str = System.Text.Encoding.Unicode.GetString(data);
            Debug.Log("Data read for key " + resArr[1] + " is " + str + " with len " + resArr[2] + " and converted string length is " + str.Length);
            dataToSave = str;
        }*/
    }

    public void OnGPGCloudSaveResult(string result)
    {
        // result is in the format result;keyNum
        // where result is either success/conflict/error
        // keyNum is the key for which this result is 0-3 range as per GPG

        Debug.Log("GPG CloudSaveResult " + result);
        string[] resArr = result.Split(';');
        if (resArr.Length < 3) {
            Debug.LogError("Length of array after split is less than 3");
            return; // weird stuff
        }

    }

    public void OnGPGUnlockAchievementResult(string result)
    {
        Debug.Log("OnGPGUnlockAchievementResult: " + result);

        if (CBUnlockAchievement != null)
            CBUnlockAchievement(result == "true" ? true : false);
    }

    public void OnGPGSubmitScoreResult(string result)
    {
        if (CBSubmitScore != null)
            CBSubmitScore(result == "true" ? true : false);
    }

    public void OnAchievementsLoaded(string result)
    {
        // Result is a multiline string with new line terminating character for each achievement
        // format of string is
        // achievementId;name;type;description;state;totalSteps;currentSteps
        // for more information look at 
        // https://developer.android.com/reference/com/google/android/gms/games/achievement/Achievement.html
        /* code from java
        String sach = ach.getAchievementId()+";"+ach.getName()+";"+ach.getType()+";"
                                +ach.getDescription()+";"+ach.getState()+";"+totSteps+";"
                                +currentSteps;

                        achs = achs + sach +"\n";
        */

        Debug.Log("OnAchievementsLoaded: " + result);

        string[] achievements = result.Split('\n');

        acDescList = new List<GPGAchievementDescription>(achievements.Length);
        acList = new List<GPGAchievement>(achievements.Length);
        acExtraData = new List<ExtraACData>(achievements.Length);

        for (int i=0;i<achievements.Length;i++)
        {
            string[] acInfo = achievements[i].Split(';');

            if (acInfo.Length != 7)
                continue;

            GPGAchievementDescription desc = new GPGAchievementDescription();
            GPGAchievement ac = new GPGAchievement();
            ExtraACData acData = new ExtraACData();

            desc.id = acInfo[0];
            desc.title = acInfo[1];
            desc.achievedDescription = acInfo[3];
            desc.unachievedDescription = acInfo[3];
            desc.hidden = (acInfo[4] == "STATE_HIDDEN");
            desc.image = null;
            desc.points = 0;

            ac.id = acInfo[0];
            ac.completed = (acInfo[4] == "STATE_UNLOCKED");
            ac.hidden = (acInfo[4] == "STATE_HIDDEN");
            ac.percentCompleted = (double)((double)System.Convert.ToInt32(acInfo[6]) / (double)System.Convert.ToInt32(acInfo[5])) * 100.0;

            acDescList.Add(desc);
            acList.Add(ac);

            acData.type = System.Convert.ToInt32(acInfo[2]);
            acData.state = System.Convert.ToInt32(acInfo[4]);
            acData.totalSteps = System.Convert.ToInt32(acInfo[5]);
            acData.currSteps = System.Convert.ToInt32(acInfo[6]);
            acExtraData.Add(acData);
        }

        if (onACLoadCB != null)
            onACLoadCB();
    }

    public void OnAchievementsLoadFailed(string result)
    {
        Debug.Log("OnAchievementsLoadFailed: " + result);
    }
#endregion


#if UNITY_IPHONE	
	// Imports
	[DllImport("__Internal", CharSet = CharSet.Ansi)]
    public static extern void GPG_Init([In, MarshalAs(UnmanagedType.LPStr)]string clientId);
	[DllImport("__Internal")]
    public static extern bool GPG_TrySilentSignIn ();
	[DllImport("__Internal")]
    public static extern void GPG_SignIn();
	[DllImport("__Internal")]
    public static extern void GPG_SignOut();
	
	[DllImport("__Internal")]
    public static extern void GPG_GameSignIn();
	
	[DllImport("__Internal", CharSet = CharSet.Ansi)]
	/// <summary>
	/// SetGameObjectName
	/// </summary>
	public static extern void GPG_SetGameObjectName([In, MarshalAs(UnmanagedType.LPStr)]string gameObjectName);
 	
	
	[DllImport("__Internal")]
    public static extern void GPG_ShowAllLeaderBoards();
	
	[DllImport("__Internal")]
    public static extern void GPG_ShowAchievements();
	
	[DllImport("__Internal", CharSet = CharSet.Ansi)]
	/// <summary>
	/// GPs the g_ show leader boards.
	/// </summary>
	/// <param name='leaderboardId'>
	/// Leaderboard identifier.
	/// </param>
    public static extern void GPG_ShowLeaderBoards([In, MarshalAs(UnmanagedType.LPStr)]string leaderBoardId);
	
	[DllImport("__Internal", CharSet = CharSet.Ansi)]
	/// <summary>
	/// GPs the g_ submit score.
	/// </summary>
	/// <param name='leaderBoardId'>
	/// Leader board identifier.
	/// </param>
	/// <param name='value'>
	/// Score Value.
	/// </param>
    public static extern void GPG_SubmitScore([In, MarshalAs(UnmanagedType.LPStr)]string leaderBoardId, long score);
	
	[DllImport("__Internal", CharSet = CharSet.Ansi)]
	/// <summary>
	/// GPs the g_ unloc achievement.
	/// </summary>
	/// <param name='achievementId'>
	/// Achievement identifier.
	/// </param>
    public static extern void GPG_UnlocAchievement([In, MarshalAs(UnmanagedType.LPStr)]string achievementId);
	
	
	// Cloud SAVE/LOAD API
	
	[DllImport("__Internal", CharSet = CharSet.Ansi)]
	/// <summary>
	/// GPs the g_ save to cloud.
	/// Note: in case of conflict the localdata always wins. change it in plugin if you want different behaviour
	/// </summary>
	/// <param name='keyNum'>
	/// Key number. 0-3 as per GPG 
	/// </param>
	/// <param name='bytes'>
	/// Bytes.
	/// </param>
	/// <param name='len'>
	/// Length.
	/// </param>
    public static extern void GPG_SaveToCloud(int keyNum, byte[] bytes, int len);
	
	[DllImport("__Internal", CharSet = CharSet.Ansi)]
	/// <summary>
	/// GPs the g_ load from cloud.
	/// Note: in case of conflict the localdata always wins. change it in plugin if you want different behaviour
	/// </summary>
	/// <param name='keyNum'>
	/// Key number.
	/// </param>
	/// <param name='bytes'>
	/// Bytes.
	/// </param>
	/// <param name='len'>
	/// Length.
	/// </param>
    public static extern void GPG_LoadFromCloud(int keyNum, System.IntPtr bytes, int len);
	
	[DllImport("__Internal")]
	/// <summary>
	/// GPs the g_ has authoriser.
	/// </summary>
	/// <returns>
	/// true if user has authorised. false if not.
	/// </returns>
	public static extern bool GPG_HasAuthoriser();	
	
#elif UNITY_ANDROID




#endif // UNITY_ANDROID
}
