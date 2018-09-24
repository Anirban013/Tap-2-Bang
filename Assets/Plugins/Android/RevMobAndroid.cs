using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;


#if UNITY_ANDROID
public class RevMobAndroid : RevMob {
	private AndroidJavaObject session;

	public RevMobAndroid(string appId, string gameObjectName) {
		this.gameObjectName = gameObjectName;
		AndroidJavaClass unityRevMobClass = new AndroidJavaClass("com.revmob.unity.UnityRevMob");
		this.session = unityRevMobClass.CallStatic<AndroidJavaObject>("start",
	                                                               RevMobAndroid.CurrentActivity(),
	                                                               appId,
	                                                               "unity-android",
	                                                               REVMOB_VERSION,
                                                                 new AndroidJavaObject("com.revmob.unity.RevMobAdsUnityListener", gameObjectName, "session"));
	}


	public static AndroidJavaObject CurrentActivity() {
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public override bool IsDevice() {
		return (Application.platform == RuntimePlatform.Android);
	}  
      
  private AndroidJavaObject adUnitWrapperCall(string methodName, string placementId, string adUnit) {
		if (placementId == null) {
			placementId = "";
		}
		AndroidJavaObject publisherListener = CreateRevMobListener(this.gameObjectName, adUnit);
		AndroidJavaObject obj = this.session.Call<AndroidJavaObject>(methodName, CurrentActivity(), placementId, publisherListener);
		return obj;
	}

	private AndroidJavaObject CreateRevMobListener(String gameObjectName, String adUnityType) {
		return new AndroidJavaObject("com.revmob.unity.RevMobAdsUnityListener", gameObjectName, adUnityType);
	}

	public override void PrintEnvironmentInformation() {
		session.Call("printEnvironmentInformation", CurrentActivity());
	}

	public override void SetTestingMode(RevMob.Test test) {
		session.Call("setTestingMode", (int)test);
	}

	public override void SetTimeoutInSeconds(int timeout) {
		session.Call("setTimeoutInSeconds", timeout);
	}

	public override void ShowFullscreen(string placementId) {
		session.Call("showFullscreen",CurrentActivity(),placementId,CreateRevMobListener(this.gameObjectName,"Fullscreen"));
	}

	public override void SetUserAgeRangeMin(int ageMin){
		session.Call("setUserAgeRangeMin", ageMin);
	}

	public override void SetUserAgeRangeMax(int ageMax){
		session.Call("setUserAgeRangeMax", ageMax);
	}

	public override RevMobFullscreen CreateFullscreen(string placementId) {
		if (!IsDevice ()) return null;
		AndroidJavaObject javaObject = this.adUnitWrapperCall("createFullscreen", placementId, "Fullscreen");
		return new RevMobAndroidFullscreen(javaObject);
	}

	public override RevMobFullscreen CreateVideo(string placementId) {
		if (!IsDevice ()) return null;
		AndroidJavaObject javaObject = this.adUnitWrapperCall("createVideo", placementId, "Video");
		return new RevMobAndroidFullscreen(javaObject);
	}

	public override RevMobFullscreen CreateRewardedVideo(string placementId) {
		if (!IsDevice ()) return null;
		AndroidJavaObject javaObject = this.adUnitWrapperCall("createRewardedVideo", placementId, "RewardedVideo");
		return new RevMobAndroidFullscreen(javaObject);
	}

	public override RevMobLink OpenButton(string placementId){
		return (IsDevice()) ? new RevMobAndroidLink(this.adUnitWrapperCall("openLink", placementId, "Link")) : null;
	}

	public override RevMobLink CreateButton(string placementId){
		if (!IsDevice ()) return null;
		AndroidJavaObject javaObject = this.adUnitWrapperCall("createLink", placementId, "Link");
		return new RevMobAndroidLink(javaObject);
	}

	public override RevMobBanner CreateBanner(RevMob.Position position, int x, int y, int w, int h) {
		return (IsDevice()) ? new RevMobAndroidBanner(CurrentActivity(), CreateRevMobListener(this.gameObjectName, "Banner"), position, x, y, w, h, session) : null;
	}

	public override void ShowBanner(RevMob.Position position, int x, int y, int w, int h) {
		if (!IsDevice ()) return;
		this.session.Call("showBanner", CurrentActivity(), CreateRevMobListener(this.gameObjectName, "Banner"), (int)position, x, y, w, h);
	}

	public override void HideBanner() {
		this.session.Call("hideBanner", CurrentActivity());
	}

	public override RevMobLink OpenLink(string placementId) {
		return new RevMobAndroidLink(this.adUnitWrapperCall("openLink", placementId, "Link"));
	}

	public override RevMobLink CreateLink(string placementId)	{
		if (!IsDevice ()) return null;
		AndroidJavaObject javaObject = this.adUnitWrapperCall("createLink", placementId, "Link");
		return new RevMobAndroidLink(javaObject);
	}
}
#endif