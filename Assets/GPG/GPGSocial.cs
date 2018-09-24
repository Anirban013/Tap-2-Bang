using UnityEngine;
using System.Collections;
using System;

namespace UnityEngine.SocialPlatforms
{
    enum GPGACType
    {
        Type_Standard = 0,
        Type_Incremental
    };

    enum GPGACState
    {
        State_Unlocked = 0,
        State_Revealed,
        State_Hidden
    };

    public class GPGLocalUser : ILocalUser
    {
        public string id { get; set; }
        public Texture2D image { get; set; }
        public bool isFriend { get; set; }
        public UserState state { get; set; }
        public string userName { get; set; }

        public bool authenticated { get; set; }
        public IUserProfile[] friends { get; set; }
        public bool underage { get; set; }

        private Action<bool> authCB;

        public void Authenticate(System.Action<bool> callback)
        {
            authCB = callback;

            NerdGPG.Instance().init();
            NerdGPG.Instance().signIn(Authresponse);
        }

        void Authresponse(bool result)
        {
            authenticated = result;

            id = NerdGPG.Instance().getPlayerID();
            userName = NerdGPG.Instance().getPlayerName();

            authCB(result);
        }

        public void LoadFriends(System.Action<bool> callback)
        {

        }
    }

    public class GPGAchievementDescription : IAchievementDescription
    {
        public string achievedDescription { get; set; }
        public bool hidden { get; set; }
        public string id { get; set; }
        public Texture2D image { get; set; }
        public int points { get; set; }
        public string title { get; set; }
        public string unachievedDescription { get; set; }
    }

    public class GPGAchievement : IAchievement
    {
        public bool completed { get; set; }
        public bool hidden { get; set; }
        public string id { get; set; }
        public DateTime lastReportedDate { get; set; }
        public double percentCompleted { get; set; }

        public void ReportProgress(Action<bool> callback)
        {
            Debug.LogWarning("Reporting Progress From IAchievement. This is not implemented in GPG, use Social.ReportProgress");
        }
    }

    public class GPGLeaderboard : ILeaderboard
    {
        public string id { get; set; }
        public bool loading
        {
            get
            {
                return false;
            }
        }

        public IScore localUserScore
        {
            get
            {
                return null;
            }
        }

        public uint maxRange
        {
            get
            {
                return 0;
            }
        }

        public Range range { get; set; }
        public IScore[] scores
        {
            get
            {
                return null;
            }
        }

        public TimeScope timeScope { get; set; }
        public string title
        {
            get
            {
                return "";
            }
        }
        public UserScope userScope { get; set; }

        public void LoadScores(Action<bool> callback)
        {

        }

        public void SetUserFilter(string[] userIDs)
        {

        }
    }

    public class GPGSocial : ISocialPlatform
    {
        private GPGLocalUser gpgUser = null;

        public ILocalUser localUser 
        { 
            get 
            {
                if (gpgUser == null)
                    gpgUser = new GPGLocalUser();

                return gpgUser;
            } 
        }

        public void Authenticate(ILocalUser user, Action<bool> callback)
        {
            GPGLocalUser tUser = user as GPGLocalUser;
            tUser.Authenticate(callback);
        }

        public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback) { }
        public void LoadFriends(ILocalUser user, Action<bool> callback) { }

        public IAchievement CreateAchievement() { return null; }
        public ILeaderboard CreateLeaderboard() { return null; }
        public bool GetLoading(ILeaderboard board) { return false; }

        //////////////////////////////////////////////////// Code For Loading Achiements ////////////////////////////////////
        // Social interfrace wants us to have LoadAchievementDescriptions and LoadAchievements as different functions with
        // thier own callbacks, but GPG allows us to do one call to the server and get descriptions + achievement data
        // together. So what we do is, first time we call load achievements it actually initiates a request to the api
        // to get the achievements and flood lists for both descriptions and achievement data. When we call this for a second
        // time it does not send another request, instead calls the callback immediately and fills the data from the last
        // request. 
        //
        // Note: If you want to force a refresh, there is a second parameter to load achievements to do that or if you want
        // to do this within the social api call NerdGPG.Instance().haveLoadedAc = false; Its a hack but it allows things to
        // be more optimized.

        private Action<IAchievementDescription[]> acDescCB;
        private Action<IAchievement[]> acCB;
        private bool haveLoadedAC;
        private bool loadACForDesc;

        public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
        {
            acDescCB = callback;
            loadACForDesc = true;

            NerdGPG.Instance().loadAchievements(OnACLoadCB);
        }

        public void LoadAchievements(Action<IAchievement[]> callback)
        {
            acCB = callback;
            loadACForDesc = false;

            NerdGPG.Instance().loadAchievements(OnACLoadCB);
        }

        void OnACLoadCB()
        {
            if (loadACForDesc) {
                if (acDescCB != null) {
                    acDescCB(NerdGPG.Instance().acDescList.ToArray());
                }
            } else {
                if (acCB != null) {
                    acCB(NerdGPG.Instance().acList.ToArray());
                }
            }
        } 
        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void LoadScores(ILeaderboard board, Action<bool> callback) 
        { 
        
        }

        public void LoadScores(string leaderboardID, Action<IScore[]> callback) 
        { 
            
        }
        
        public void ReportProgress(string achievementID, double progress, Action<bool> callback)
        {
            Debug.Log("GPGSocial - ReportProgress");

            NerdGPG gpgInst = NerdGPG.Instance();

            if (gpgInst.acList == null || gpgInst.acList.Count == 0) {
                Debug.Log("GPGSocial - ReportProgress failed: " + gpgInst.acList);
                callback(false);
                return;
            }

            bool foundAc = false;

            for (int i = 0; i < gpgInst.acList.Count; i++) {
                if (gpgInst.acList[i].id == achievementID) {
                    if (gpgInst.acExtraData[i].state == (int)GPGACState.State_Unlocked) {
                        Debug.Log("GPGSocial - ReportProgress failed: AlreadyUnlocked ");
                        callback(false);
                        return;
                    }

                    foundAc = true;

                    if (gpgInst.acExtraData[i].type == (int)GPGACType.Type_Standard) {
                        if (progress != 100) {
                            Debug.LogError("Reported progress other then 100 to a standard achievement. Unlocking Anyways");
                        } else {
                            gpgInst.unlockAchievement(achievementID, callback);
                        }
                    } else if (gpgInst.acExtraData[i].type == (int)GPGACType.Type_Incremental) {
                        int stepsCompleted = (int)(gpgInst.acList[i].percentCompleted * (double)gpgInst.acExtraData[i].totalSteps / 100.0);

                        if (stepsCompleted != gpgInst.acExtraData[i].currSteps)
                            Debug.LogWarning("Calculated steps for achievement and known steps dont match! Calculated Steps: " + stepsCompleted + " Known: " + gpgInst.acExtraData[i].currSteps);

                        int newSteps = (int)(progress * (double)gpgInst.acExtraData[i].totalSteps / 100.0);
                        int finalStepsIncrement = newSteps - stepsCompleted;

                        Debug.Log("PrevSteps: " + stepsCompleted + "  newSteps: " + newSteps + " TotalSteps: " + gpgInst.acExtraData[i].totalSteps);

                        gpgInst.incrementAchievement(achievementID, finalStepsIncrement, callback);
                    }
                }
            }

            if (!foundAc) {
                Debug.Log("GPGSocial - ReportProgress failed: DidntFindAC ");
                callback(false);
            }
        }
        
        public void ReportScore(long score, string board, Action<bool> callback) 
        {
            NerdGPG.Instance().submitScore(board, score, callback);
        }

        public void ShowAchievementsUI() { NerdGPG.Instance().showAchievements(); }
        public void ShowLeaderboardUI() { NerdGPG.Instance().showAllLeaderBoards(); }
    }
}