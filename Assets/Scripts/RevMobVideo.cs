using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevMobVideo : MonoBehaviour, IRevMobListener
{
    private static readonly Dictionary<String, String> REVMOB_APP_IDS = new Dictionary<String, String>() {
        { "Android", "575beb868ffef6705da3354c"},
        { "IOS", "paste_your_RevMob_Media_ID_for_ios_here" }
    };
    private RevMob revmob;
    private RevMobFullscreen video;
    void Awake()
    {
        revmob = RevMob.Start(REVMOB_APP_IDS, "Your_GameObject_name");
    }
    #region IRevMobListener implementation
    public void SessionIsStarted()
    {
        video = revmob.CreateVideo();
    }
    public void VideoLoaded()
    {
        if (video != null) video.ShowVideo();
        else
            revmob.ShowFullscreen();
    }
    //You must also implement all the other listeners
    #endregion

    public void VideoNotCompletelyLoaded()
    {
        Debug.Log(">>> VideoNotCompletelyLoaded");
    }
    public void VideoStarted()
    {
        Debug.Log(">>> VideoStarted");
    }
    public void VideoFinished()
    {
        Debug.Log(">>> VideoFinished");
    }

   
    public void SessionNotStarted(string revMobAdType)
    {
        Debug.Log("Session not started.");
    }
    public void AdDidReceive(string revMobAdType)
    {
        Debug.Log("Ad did receive.");
    }
    public void AdDidFail(string revMobAdType)
    {
        Debug.Log("Ad did fail.");
    }
    public void AdDisplayed(string revMobAdType)
    {
        Debug.Log("Ad displayed.");
    }
    public void UserClickedInTheAd(string revMobAdType)
    {
        Debug.Log("Ad clicked.");
    }
    public void UserClosedTheAd(string revMobAdType)
    {
        Debug.Log("Ad closed.");
    }
    public void RewardedVideoLoaded()
    {
        Debug.Log("RewardedVideoLoaded.");
    }
    public void RewardedVideoNotCompletelyLoaded()
    {
        Debug.Log("RewardedVideoNotCompletelyLoaded.");
    }
    public void RewardedVideoStarted()
    {
        Debug.Log("RewardedVideoStarted.");
    }
    public void RewardedVideoFinished()
    {
        Debug.Log("RewardedVideoFinished.");
    }
    public void RewardedVideoCompleted()
    {
        Debug.Log("RewardedVideoCompleted.");
    }
    public void RewardedPreRollDisplayed()
    {
        Debug.Log("RewardedPreRollDisplayed.");
    }
  
  
   
    
}