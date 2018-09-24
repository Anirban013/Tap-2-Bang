using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


public abstract class RevMob {
	protected static readonly string REVMOB_VERSION = "9.1.0";
	protected string gameObjectName;

	public enum Test {
		DISABLED = 0,
		WITH_ADS = 1,
		WITHOUT_ADS = 2
	}
#if UNITY_ANDROID
	public enum Position {
		BOTTOM = 0,
		TOP = 1
	}
#endif
	public abstract void PrintEnvironmentInformation();
	public abstract void SetTestingMode(RevMob.Test test);
	public abstract void SetTimeoutInSeconds(int timeout);
	public abstract bool IsDevice();

	public abstract void SetUserAgeRangeMin(int minAge);
	public abstract void SetUserAgeRangeMax(int maxAge);

	public abstract void ShowFullscreen(string placementId);
	public abstract RevMobFullscreen CreateFullscreen(string placementId);
	public abstract RevMobFullscreen CreateVideo(string placementId);
	public abstract RevMobFullscreen CreateRewardedVideo(string placementId);
	public abstract RevMobLink OpenButton(string placementId);
	public abstract RevMobLink CreateButton(string placementId);
#if UNITY_ANDROID
	public abstract RevMobBanner CreateBanner(RevMob.Position position, int leftMargin, int topMargin, int w, int h);
	public abstract void ShowBanner(RevMob.Position position, int x, int y, int w, int h);
	public abstract void HideBanner();
#elif UNITY_IPHONE
	public abstract RevMobBanner CreateBanner(float x, float y, float width, float height, ScreenOrientation[] orientations);
	public abstract RevMobPopup ShowPopup(string placementId);
	public abstract RevMobPopup CreatePopup(string placementId);
#endif
	public abstract RevMobLink OpenLink(string placementId);
	public abstract RevMobLink CreateLink(string placementId);



	public static RevMob Start(Dictionary<string, string> appIds) {
		return Start(appIds, null);
	}

	public static RevMob Start(Dictionary<string, string> appIds, string gameObjectName) {
		Debug.Log("Creating RevMob Session");
#if UNITY_EDITOR
		Debug.Log("It Can't run in Unity Editor. Only in iOS or Android devices.");
		return null;
#elif UNITY_ANDROID
		RevMob session = new RevMobAndroid(appIds["Android"], gameObjectName);
		return session;
#elif UNITY_IPHONE
		RevMob session = new RevMobIos(appIds["IOS"], gameObjectName);
		return session;
#else
		return null;
#endif
	}

	public void ShowFullscreen() {
		this.ShowFullscreen(null);
	}

	public RevMobFullscreen CreateFullscreen() {
		return this.CreateFullscreen(null);
	}

	public RevMobFullscreen CreateVideo() {
		return this.CreateVideo(null);
	}

	public RevMobFullscreen CreateRewardedVideo() {
		return this.CreateRewardedVideo(null);
	}

	public RevMobLink OpenButton() {
		return this.OpenButton(null);
	}

	public RevMobLink CreateButton() {
		return this.CreateButton(null);
	}

#if UNITY_ANDROID
	public RevMobBanner CreateBanner() {
		return this.CreateBanner(0, 0, 0, 0, 0);
	}
	
	public RevMobBanner CreateBanner(RevMob.Position position) {
		return this.CreateBanner(position, 0, 0, 0, 0);
	}

	public void ShowBanner() {
		this.ShowBanner(0, 0, 0, 0, 0);
	}

	public void ShowBanner(RevMob.Position position) {
		this.ShowBanner(position, 0, 0, 0, 0);
	}
#elif UNITY_IPHONE
	public RevMobBanner CreateBanner() {
		return this.CreateBanner(0, 0, 0, 0, null);
	}


    public RevMobBanner CreateBanner(ScreenOrientation[] orientations) {
	    return this.CreateBanner(0, 0, 0, 0, orientations);
	}

	public RevMobPopup ShowPopup() {
		return this.ShowPopup(null);
	}

	public RevMobPopup CreatePopup() {
		return this.CreatePopup(null);
	}
#endif

	public RevMobLink OpenLink() {
		return this.OpenLink(null);
	}

	public RevMobLink CreateLink() {
		return this.CreateLink(null);
	}

}

