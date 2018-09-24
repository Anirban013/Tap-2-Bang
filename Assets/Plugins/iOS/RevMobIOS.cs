using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;


#if UNITY_IPHONE
public class RevMobIos : RevMob {
	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_startSession(string appId, string version);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_setTestingMode(int testingMode);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_showFullscreen(string placementId);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_showFullscreenWithSpecificOrientations(ScreenOrientation[] orientations);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_openAdLink(string placementId);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_showPopup(string placementId);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_printEnvironmentInformation();

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_setGameObjectDelegateCallback(string gameObjectName);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_setTimeoutInSeconds(int timeout);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_setUserAgeRangeMin(int ageMin);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_setUserAgeRangeMax(int ageMax);

	public RevMobIos(string appId, string gameObjectName) {
		this.gameObjectName = gameObjectName;
		RevMobUnityiOSBinding_startSession(appId, REVMOB_VERSION);
		RevMobUnityiOSBinding_setGameObjectDelegateCallback(gameObjectName);
	}


	public override bool IsDevice() {
		return (Application.platform == RuntimePlatform.IPhonePlayer);
	}



	public override void PrintEnvironmentInformation() {
		RevMobUnityiOSBinding_printEnvironmentInformation();
	}

	public override void SetTestingMode(RevMob.Test test) {
		RevMobUnityiOSBinding_setTestingMode((int)test);
	}

	public override void SetUserAgeRangeMin(int ageMin){
		RevMobUnityiOSBinding_setUserAgeRangeMin(ageMin);
	}

	public override void SetUserAgeRangeMax(int ageMax){
		RevMobUnityiOSBinding_setUserAgeRangeMax(ageMax);
	}

	public override void SetTimeoutInSeconds(int timeout) {
		RevMobUnityiOSBinding_setTimeoutInSeconds(timeout);
	}

	public override void ShowFullscreen(string placementId) {
		RevMobUnityiOSBinding_showFullscreen(placementId);
	}

	public override RevMobFullscreen CreateFullscreen(string placementId) {
		RevMobIosFullscreen fs = (IsDevice()) ? new RevMobIosFullscreen(placementId) : null;
		fs.LoadFullscreen();
		return fs;
	}

	public override RevMobFullscreen CreateVideo(string placementId) {
		RevMobIosFullscreen fs = (IsDevice()) ? new RevMobIosFullscreen(placementId) : null;
		fs.LoadVideo();
		return fs;
	}

	public override RevMobFullscreen CreateRewardedVideo(string placementId) {
		RevMobIosFullscreen fs = (IsDevice()) ? new RevMobIosFullscreen(placementId) : null;
		fs.LoadRewardedVideo();
		return fs;
	}

	public void ShowFullscreen(ScreenOrientation[] orientations) {
		RevMobUnityiOSBinding_showFullscreenWithSpecificOrientations(orientations);
		
	}

	public override RevMobBanner CreateBanner(float x, float y, float width, float height, ScreenOrientation[] orientations) {
		return (IsDevice()) ? new RevMobIosBanner(orientations, x, y, width, height) : null;
	}

	public override RevMobLink OpenButton(string placementId){
		RevMobUnityiOSBinding_openAdLink(placementId);
		return null;
	}

	public override RevMobLink CreateButton(string placementId){
		return (IsDevice()) ? new RevMobIosLink(placementId) : null;
	}
		
	public override RevMobLink OpenLink(string placementId) {
		RevMobUnityiOSBinding_openAdLink(placementId);
		return null;
	}

	public override RevMobLink CreateLink(string placementId)	{
		return (IsDevice()) ? new RevMobIosLink(placementId) : null;
	}

	public override RevMobPopup ShowPopup(string placementId) {
		RevMobUnityiOSBinding_showPopup(placementId);
		return null;
	}

	public override RevMobPopup CreatePopup(string placementId) {
		return this.ShowPopup(placementId); // TODO: iOS does not have Popup pre-load
	}
}
#endif