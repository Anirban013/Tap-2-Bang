using UnityEngine;
using System.Collections;
using ChartboostSDK;
using System.Collections.Generic;


public class CBSample : MonoBehaviour {

	// Use this for initialization

        void Awake()
    {
        Chartboost.cacheInterstitial(CBLocation.Default);
       // Chartboost.cacheRewardedVideo(CBLocation.Default);

    }
	
    public void Start()
    {
        Chartboost.showInterstitial(CBLocation.Default);
    }
    
}
