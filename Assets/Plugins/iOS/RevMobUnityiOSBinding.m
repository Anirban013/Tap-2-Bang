#import "RevMobUnityiOSDelegate.h"
#import "RevMobAds.h"
#import "RevMobAdLink.h"

void UnitySendMessage(const char *className, const char *methodName, const char *param);

#define TO_NSSTRING( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]
#define CREATE_REVMOB_DELEGATE(_AD_TYPE_) [[RevMobUnityiOSDelegate alloc] initWithAdType:_AD_TYPE_]

// Converts NSString to C style string by way of copy (Mono will free it)
#define STRCOPY( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

static NSDictionary* revmobDelegatesDict = nil;
static NSDictionary* revmobDelegates() {
    if (revmobDelegatesDict == nil) {
        revmobDelegatesDict = [[NSDictionary alloc] initWithObjectsAndKeys:
                              CREATE_REVMOB_DELEGATE(@"Fullscreen"), @"Fullscreen",
                              CREATE_REVMOB_DELEGATE(@"Banner"), @"Banner",
                              CREATE_REVMOB_DELEGATE(@"Popup"), @"Popup",
                              CREATE_REVMOB_DELEGATE(@"Link"), @"Link", 
                              CREATE_REVMOB_DELEGATE(@"Session"), @"Session",
                              CREATE_REVMOB_DELEGATE(@"Video"), @"Video",
                              CREATE_REVMOB_DELEGATE(@"RewardedVideo"), @"RewardedVideo",
                              nil];

    }
    return revmobDelegatesDict;
}

void RevMobUnityiOSBinding_setGameObjectDelegateCallback(const char* gameObjectName) {
    for (RevMobUnityiOSDelegate* revmobDelegate in [revmobDelegates() allValues]) {
        NSLog(@"Set delegate callback to %@", TO_NSSTRING(gameObjectName));
        revmobDelegate.gameObjectName = TO_NSSTRING(gameObjectName);
    }
}

@interface RevMobAds()
+ (void) startSessionWithAppID:(NSString *)appID delegate:(NSObject<RevMobAdsDelegate> *)delegate
                   testingMode:(RevMobAdsTestingMode)testingMode
                       sdkName:(NSString *)sdkName
                    sdkVersion:(NSString *)sdkVersion;
+ (RevMobAds *)sharedObject;
@end

static RevMobAds *revmob = nil;

void RevMobUnityiOSBinding_startSession(const char* appId, const char* version) {
    // [RevMobAds startSessionWithAppID:TO_NSSTRING(appId)
    //               withSuccessHandler:^{
    //                   revmob = [RevMobAds session];
    //                   NSLog(@"Session started with block");
    //               } andFailHandler:^(NSError *error) {
    //                   NSLog(@"Session failed to start with block");
    //               }];

    [RevMobAds startSessionWithAppID:TO_NSSTRING(appId) 
                delegate:[revmobDelegates() valueForKey:@"Session"] 
                testingMode:RevMobAdsTestingModeOff 
                sdkName:@"unity-ios" 
                sdkVersion:@"9.1.0" 
                withSuccessHandler:^(void){
                    revmob = [RevMobAds session];
                      NSLog(@"Session started with block");
                }
                andFailHandler:^(NSError *error){
                      NSLog(@"Session failed to start with block");
    }];
}

void RevMobUnityiOSBinding_setTestingMode(int testingMode) {
    revmob.testingMode = testingMode;
}

void RevMobUnityiOSBinding_setTimeoutInSeconds(int timeout) {
    revmob.connectionTimeout = timeout;
}

void RevMobUnityiOSBinding_setUserAgeRangeMin(int ageMin){
    revmob.userAgeRangeMin = ageMin;
}

void RevMobUnityiOSBinding_setUserAgeRangeMax(int ageMax){
    revmob.userAgeRangeMax = ageMax;
}

NSMutableArray* supportedInterfaceOrientations(int *orientations) {
    NSMutableArray *arrayOfOrientations = nil;
    if (orientations && orientations[0] >= 1 && orientations[0] <= 4) {
        arrayOfOrientations = [[NSMutableArray alloc] initWithCapacity:4];
        for (int i = 0; i < 4; i++) {
            [arrayOfOrientations addObject:[NSNumber numberWithInt:orientations[i]]];
            if (orientations[i] < 1 || orientations[i] > 4) {
                break;
            }
        }
    }
    return arrayOfOrientations;
}

int* supportedOrientations() {
    static int orientations[4];
    
    typedef enum interfaceOrientation {
        UIInterfaceOrientationPortrait              = 1,
        UIInterfaceOrientationPortraitUpsideDown    = 2,
        UIInterfaceOrientationLandscapeRight        = 3,
        UIInterfaceOrientationLandscapeLeft         = 4
    } InterfaceOrientation;
    
    NSDictionary *interfaces = [NSDictionary dictionaryWithObjectsAndKeys:
                                [NSNumber numberWithInteger:UIInterfaceOrientationPortrait],            @"UIInterfaceOrientationPortrait",
                                [NSNumber numberWithInteger:UIInterfaceOrientationPortraitUpsideDown],  @"UIInterfaceOrientationPortraitUpsideDown",
                                [NSNumber numberWithInteger:UIInterfaceOrientationLandscapeRight],      @"UIInterfaceOrientationLandscapeRight",
                                [NSNumber numberWithInteger:UIInterfaceOrientationLandscapeLeft],       @"UIInterfaceOrientationLandscapeLeft",
                                nil];

    NSArray *arrayOfOrientations = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"UISupportedInterfaceOrientations"];
    
    for (int i = 0; i < [arrayOfOrientations count]; i++) {
        NSString *interface = [arrayOfOrientations objectAtIndex:i];
        int interfaceValue = [[interfaces objectForKey:interface] integerValue];
        orientations[i] = interfaceValue;
    }
    return orientations;
}

#pragma mark Fullscreen

static RevMobFullscreen *fullscreen = nil;
static RevMobFullscreen *video = nil;
static RevMobFullscreen *rewardedVideo = nil;

void RevMobUnityiOSBinding_createFullscreen(const char* placementId) {
    if (fullscreen != nil) {
        [fullscreen release];
        fullscreen = nil;
    }
    if (placementId == NULL) {
        fullscreen = [[revmob fullscreen] retain];
    } else {
        fullscreen = [[revmob fullscreenWithPlacementId:TO_NSSTRING(placementId)] retain];
    }
    fullscreen.delegate = [revmobDelegates() valueForKey:@"Fullscreen"];    
    fullscreen.supportedInterfaceOrientations = supportedInterfaceOrientations(supportedOrientations());
}

void RevMobUnityiOSBinding_loadFullscreen(const char* placementId) {
    RevMobUnityiOSBinding_createFullscreen(placementId);
    [fullscreen loadAd];
}

void RevMobUnityiOSBinding_showFullscreen(const char* placementId) {
    RevMobUnityiOSBinding_createFullscreen(placementId);
    [fullscreen showAd];
}

void RevMobUnityiOSBinding_hideLoadedFullscreen() {
    if (fullscreen != nil) [fullscreen hideAd];
}

void RevMobUnityiOSBinding_showLoadedFullscreen() {
    if (fullscreen != nil) [fullscreen showAd];
}

void RevMobUnityiOSBinding_releaseLoadedFullscreen() {
    NSLog(@"Releasing fullscreen.");
    [fullscreen release];
    fullscreen = nil;
}

void RevMobUnityiOSBinding_loadFullscreenWithSpecificOrientations(int *orientations) {
    RevMobUnityiOSBinding_loadFullscreen(NULL);
    fullscreen.supportedInterfaceOrientations = supportedInterfaceOrientations(orientations);
}

void RevMobUnityiOSBinding_showFullscreenWithSpecificOrientations(int *orientations) {
    RevMobUnityiOSBinding_loadFullscreenWithSpecificOrientations(orientations);
    [fullscreen showAd];
}

void RevMobUnityiOSBinding_createVideo(const char* placementId) {
    if (video != nil) {
        [video release];
        video = nil;
    }
    if (placementId == NULL) {
        video = [[revmob fullscreen] retain];
    } else {
        video = [[revmob fullscreenWithPlacementId:TO_NSSTRING(placementId)] retain];
    }
    video.delegate = [revmobDelegates() valueForKey:@"Video"];    
    video.supportedInterfaceOrientations = supportedInterfaceOrientations(supportedOrientations());
}

void RevMobUnityiOSBinding_loadVideo(const char* placementId) {
    RevMobUnityiOSBinding_createVideo(placementId);
    [video loadVideo];
}

void RevMobUnityiOSBinding_showVideo(const char* placementId) {
    if(video != nil) [video showVideo];
}

void RevMobUnityiOSBinding_hideLoadedVideo() {
    if (video != nil) [video hideAd];
}

void RevMobUnityiOSBinding_showLoadedVideo() {
    if (video != nil) [video showVideo];
}

void RevMobUnityiOSBinding_createRewardedVideo(const char* placementId) {
    if (rewardedVideo != nil) {
        [rewardedVideo release];
        rewardedVideo = nil;
    }
    if (placementId == NULL) {
        rewardedVideo = [[revmob fullscreen] retain];
    } else {
        rewardedVideo = [[revmob fullscreenWithPlacementId:TO_NSSTRING(placementId)] retain];
    }
    rewardedVideo.delegate = [revmobDelegates() valueForKey:@"RewardedVideo"];    
    rewardedVideo.supportedInterfaceOrientations = supportedInterfaceOrientations(supportedOrientations());
}

void RevMobUnityiOSBinding_loadRewardedVideo(const char* placementId) {
    RevMobUnityiOSBinding_createRewardedVideo(placementId);
    [rewardedVideo loadRewardedVideo];
}

void RevMobUnityiOSBinding_showRewardedVideo(const char* placementId) {
    if(rewardedVideo != nil) [rewardedVideo showRewardedVideo];
}

void RevMobUnityiOSBinding_hideLoadedRewardedVideo() {
    if (rewardedVideo != nil) [rewardedVideo hideAd];
}

void RevMobUnityiOSBinding_showLoadedRewardedVideo() {
    if (rewardedVideo != nil) [rewardedVideo showAd];
}

#pragma mark Banner

static RevMobBanner *revmobBanner = nil;
static RevMobBanner *revmobCustomBanner= nil;

void RevMobUnityiOSBinding_deactivateBannerAd() {
    if (revmobBanner != nil) {
        [revmobBanner hideAd];
        [revmobBanner release];
        revmobBanner = nil;
    }
}

void RevMobUnityiOSBinding_loadBanner(const char *placementId, int *orientations) {
    RevMobUnityiOSBinding_deactivateBannerAd();
    if (placementId == NULL) {
        revmobBanner = [[revmob banner] retain];
    } else {
        revmobBanner = [[revmob bannerWithPlacementId:TO_NSSTRING(placementId)] retain];
    }
    revmobBanner.supportedInterfaceOrientations = supportedInterfaceOrientations(orientations);
    revmobBanner.delegate = [revmobDelegates() valueForKey:@"Banner"];
}

void RevMobUnityiOSBinding_showBanner(const char *placementId, int *orientations) {
    RevMobUnityiOSBinding_loadBanner(placementId, orientations);
    [revmobBanner showAd];
}

void RevMobUnityiOSBinding_hideBanner() {
    if (revmobBanner != nil) [revmobBanner hideAd];
}

void RevMobUnityiOSBinding_deactivateCustomBannerAd(){
    if (revmobCustomBanner != nil) {
        [revmobCustomBanner hideAd];
        [revmobCustomBanner release];
        revmobCustomBanner = nil;
    }
}

void RevMobUnityiOSBinding_loadCustomBanner(const char *placementId, int *orientations,float x, float y, float width, float height){
    RevMobUnityiOSBinding_deactivateCustomBannerAd();
    if (placementId == NULL) {
        revmobCustomBanner = [[revmob banner] retain];
    } else {
        revmobCustomBanner = [[revmob bannerWithPlacementId:TO_NSSTRING(placementId)] retain];
    }
    revmobCustomBanner.delegate = [revmobDelegates() valueForKey:@"Banner"];
    revmobCustomBanner.supportedInterfaceOrientations = supportedInterfaceOrientations(orientations);
    [revmobCustomBanner setFrame:CGRectMake(x,y,width,height)];

}

void RevMobUnityiOSBinding_showCustomBanner(const char *placementId, int *orientations,float x, float y, float width, float height){
    RevMobUnityiOSBinding_loadCustomBanner(placementId,orientations, x,y,width,height);
    [revmobCustomBanner showAd];
}

void RevMobUnityiOSBinding_hideCustomBanner(){
    if (revmobCustomBanner != nil){
        [revmobCustomBanner hideAd];
    }
}



#pragma mark Ad Link

static RevMobAdLink *revmobAdLink = nil;

void RevMobUnityiOSBinding_loadAdLink(const char *placementId) {
    if ( revmobAdLink != nil) {
        [revmobAdLink release];
        revmobAdLink = nil;
    }
    revmobAdLink = (placementId == NULL) ? [revmob adLink] : [revmob adLinkWithPlacementId:TO_NSSTRING(placementId)];
    [revmobAdLink retain];
    revmobAdLink.delegate = [revmobDelegates() valueForKey:@"Link"];
    [revmobAdLink loadAd];
}

void RevMobUnityiOSBinding_openAdLink(const char *placementId) {
    [revmob openAdLinkWithDelegate:[revmobDelegates() valueForKey:@"Fullscreen"]];
}

void RevMobUnityiOSBinding_openLoadedAdLink() {
    if(revmobAdLink != nil) [revmobAdLink openLink];

}

#pragma mark Popup

void RevMobUnityiOSBinding_showPopup(const char *placementId) {
    if (placementId == NULL) {
        RevMobPopup *popup = [revmob popup];
        popup.delegate = [revmobDelegates() valueForKey:@"Popup"];
        [popup showAd];
    } else {
        if (revmob != nil) {
            RevMobPopup *popup = [revmob popupWithPlacementId: TO_NSSTRING(placementId)];
            popup.delegate = [revmobDelegates() valueForKey:@"Popup"];
            [popup showAd];
        }
    }
}


void RevMobUnityiOSBinding_printEnvironmentInformation() {
    if (revmob) {
        [revmob printEnvironmentInformation];
    }
}


@implementation RevMobUnityiOSDelegate

- (RevMobUnityiOSDelegate *)initWithAdType:(NSString *)theAdType {
    self = [super init];
    self.gameObjectName = @"RevMobEventListener";
    adType = theAdType;
    return self;
}

- (void)revmobSessionDidStart {
	UnitySendMessage(STRCOPY(self.gameObjectName), "SessionIsStarted", STRCOPY(adType));
}

- (void)revmobSessionDidNotStartWithError:(NSError *)error {
	UnitySendMessage(STRCOPY(self.gameObjectName), "SessionNotStarted", STRCOPY(adType));
    NSLog(@"%@", error);
}

- (void)revmobNativeDidReceive:(NSString *)placementId {
	UnitySendMessage(STRCOPY(self.gameObjectName), "AdDidReceive", STRCOPY(adType));
}

- (void)revmobNativeDidFailWithError:(NSError *)error onPlacement:(NSString *)placementId{
	UnitySendMessage(STRCOPY(self.gameObjectName), "AdDidFail", STRCOPY(adType));
    NSLog(@"%@", error);
}
- (void)revmobUserDidClickOnNative:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "UserClickedInTheAd", STRCOPY(adType));
}

- (void)revmobFullscreenDidReceive:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "AdDidReceive", STRCOPY(adType));
}

- (void)revmobFullscreenDidFailWithError:(NSError *)error onPlacement:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "AdDidFail", STRCOPY(adType));
    if ([adType isEqualToString:@"Fullscreen"]) {
        RevMobUnityiOSBinding_releaseLoadedFullscreen();
    }
    NSLog(@"%@", error);
}

- (void)revmobFullscreenDidDisplay:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "AdDisplayed", STRCOPY(adType));
}

- (void)revmobUserDidClickOnFullscreen:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "UserClickedInTheAd", STRCOPY(adType));
    if ([adType isEqualToString:@"Fullscreen"]) {
        RevMobUnityiOSBinding_releaseLoadedFullscreen();
    }
}

- (void)revmobUserDidCloseFullscreen:(NSString *)placementId{
	UnitySendMessage(STRCOPY(self.gameObjectName), "UserClosedTheAd", STRCOPY(adType));
    if ([adType isEqualToString:@"Fullscreen"]) {
        RevMobUnityiOSBinding_releaseLoadedFullscreen();
    }
}

- (void)revmobBannerDidReceive:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "AdDidReceive", STRCOPY(adType));
}

- (void)revmobBannerDidFailWithError:(NSError *)error onPlacement:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "AdDidFail", STRCOPY(adType));
    if ([adType isEqualToString:@"Fullscreen"]) {
        RevMobUnityiOSBinding_releaseLoadedFullscreen();
    }
    NSLog(@"%@", error);
}

- (void)revmobBannerDidDisplay:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "AdDisplayed", STRCOPY(adType));
}

- (void)revmobUserDidClickOnBanner:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "UserClickedInTheAd", STRCOPY(adType));
    if ([adType isEqualToString:@"Fullscreen"]) {
        RevMobUnityiOSBinding_releaseLoadedFullscreen();
    }
}

/******** Video Callbacks *******/
/**
 Fired when video is received.
 */
- (void)revmobVideoDidLoad:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "VideoLoaded", STRCOPY(adType));
}

- (void)revmobVideoDidFailWithError:(NSError *)error onPlacement:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "AdDidFail", STRCOPY(adType));
}

/**
 Fired when the video is not completely loaded or not even loading.
 */
- (void)revmobVideoNotCompletelyLoaded:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "VideoNotCompletelyLoaded", STRCOPY(adType));
}

/**
 Fired when the video starts.
 */
- (void)revmobVideoDidStart:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "VideoStarted", STRCOPY(adType));
}

/**
 Fired when the video finished.
 */
- (void)revmobVideoDidFinish:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "VideoFinished", STRCOPY(adType));
}

- (void)revmobUserDidClickOnVideo:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "UserClickedInTheAd", STRCOPY(adType));
}

- (void)revmobUserDidCloseVideo:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "UserClosedTheAd", STRCOPY(adType));
}

/////Rewarded Video Listeners/////
-(void)revmobRewardedVideoDidLoad:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "RewardedVideoLoaded", STRCOPY(adType));
}

-(void)revmobRewardedVideoNotCompletelyLoaded:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "RewardedVideoNotCompletelyLoaded", STRCOPY(adType));
}

-(void)revmobRewardedVideoDidStart:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "RewardedVideoStarted", STRCOPY(adType));
}

/**
 Fired when the rewarded video finished playing.
 */
-(void)revmobRewardedVideoDidFinish:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "RewardedVideoFinished", STRCOPY(adType));
}

- (void)revmobRewardedVideoDidFailWithError:(NSError *)error onPlacement:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "AdDidFail", STRCOPY(adType));   
}


/**
 Called when the Rewarded Video is clicked
 */
- (void)revmobUserDidClickOnRewardedVideo:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "UserClickedInTheAd", STRCOPY(adType));
}


/**
 Called when the Rewarded Video is dismissed
 */
- (void)revmobUserDidCloseRewardedVideo:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "UserClosedTheAd", STRCOPY(adType));
}

/**
 Fired when the rewarded video is complete.
 */
- (void)revmobRewardedVideoDidComplete:(NSString *)placementId{
    UnitySendMessage(STRCOPY(self.gameObjectName), "RewardedVideoCompleted", STRCOPY(adType));
}

-(void)revmobRewardedPreRollDisplayed{
    UnitySendMessage(STRCOPY(self.gameObjectName), "RewardedPreRollDisplayed", STRCOPY(adType));
}

# pragma mark Advertiser Callbacks

- (void)installDidReceive {
	UnitySendMessage(STRCOPY(self.gameObjectName), "InstallDidReceive", "InstallDidReceive");
}

- (void)installDidFail {
	UnitySendMessage(STRCOPY(self.gameObjectName), "InstallDidFail", "InstallDidFail");
}
@end