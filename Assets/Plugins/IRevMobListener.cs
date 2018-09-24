using System;

public interface IRevMobListener
{
	void SessionIsStarted();
  
	void SessionNotStarted(string message);

	void AdDidReceive(string revMobAdType);

	void AdDidFail(string revMobAdType);

	void AdDisplayed(string revMobAdType);

	void UserClickedInTheAd(string revMobAdType);

	void UserClosedTheAd(string revMobAdType);

	void VideoLoaded();

	void VideoNotCompletelyLoaded();

	void VideoStarted();

	void VideoFinished();	

	void RewardedVideoLoaded();

	void RewardedVideoNotCompletelyLoaded();

	void RewardedVideoStarted();

	void RewardedVideoFinished();	

	void RewardedVideoCompleted();	

	void RewardedPreRollDisplayed();
}


