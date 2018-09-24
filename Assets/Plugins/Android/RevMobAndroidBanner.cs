using UnityEngine;
using System;
using System.Runtime.InteropServices;

#if UNITY_ANDROID
public class RevMobAndroidBanner : RevMobBanner {
	private AndroidJavaObject javaObject;
	private bool customBanner;

	public RevMobAndroidBanner(AndroidJavaObject activity, AndroidJavaObject listener, RevMob.Position position, int x, int y, int w, int h, AndroidJavaObject session) {
		this.javaObject = session;
		if(x != 0 || y != 0 || w != 0 || h != 0) this.customBanner = true;
		else this.customBanner = false;
		this.javaObject.Call("createBanner", activity, listener, (int)position, x, y, w, h);
	}
	
	public override void Show() {
		Debug.Log("BCRS showBanner");
		this.javaObject.Call("showBanner");
    }

    public override void Hide() {
		Debug.Log("BCRS hideBanner");
		if(this.customBanner == false) this.javaObject.Call("hideBanner");
		else this.javaObject.Call("hideCustomBanner");
    }

	public override void Release() {
		Debug.Log("BCRS releaseBanner");
		if(this.customBanner == false) this.javaObject.Call("releaseBanner");
		else this.javaObject.Call("releaseCustomBanner");
	}
}
#endif