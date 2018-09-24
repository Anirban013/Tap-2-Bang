using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

#if UNITY_IPHONE
public class RevMobIosFullscreen : RevMobFullscreen {

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_createFullscreen(string placementId);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_loadFullscreen(string placementId);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_loadFullscreenWithSpecificOrientations(ScreenOrientation[] orientations);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_showLoadedFullscreen();
	
	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_hideLoadedFullscreen();

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_releaseLoadedFullscreen();

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_showVideo(string placementId);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_showRewardedVideo(string placementId);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_loadVideo(string placementId);

	[DllImport("__Internal")]
	private static extern void RevMobUnityiOSBinding_loadRewardedVideo(string placementId);


	public RevMobIosFullscreen() {
		RevMobUnityiOSBinding_createFullscreen(null);
	}

	public RevMobIosFullscreen(string placementId) {
		RevMobUnityiOSBinding_createFullscreen(placementId);
	}

	public void LoadFullscreen(){
		RevMobUnityiOSBinding_loadFullscreen(null);
	}

	public override void Show() {
		RevMobUnityiOSBinding_showLoadedFullscreen();
	}

	public override void Hide() {
		RevMobUnityiOSBinding_hideLoadedFullscreen();
	}

	public override void Release() {
		this.Hide();
		RevMobUnityiOSBinding_releaseLoadedFullscreen();
	}

	public void LoadVideo(){
		RevMobUnityiOSBinding_loadVideo(null);
	}

	public void LoadRewardedVideo(){
		RevMobUnityiOSBinding_loadRewardedVideo(null);
	}

	public override void ShowVideo() {
		RevMobUnityiOSBinding_showVideo(null);
	}

	public override void ShowRewardedVideo() {
		RevMobUnityiOSBinding_showRewardedVideo(null);
	}
}
#endif