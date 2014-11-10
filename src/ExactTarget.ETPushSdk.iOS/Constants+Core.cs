using System;
using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    public partial class Constants
    {
        // SDK Version
        const string ETPushSDKVersionString = @"3.3.0";
        const int ETPushSDKVersionNumber = 3300;

        // Notification that is sent when a push fails for some reason.
        // @constant ETPush Notifications Sent when the data request process has started
        const string ETRequestStarted = @"ETRequestStarted";
        const string ETRequestNoData = @"ETRequestNoData";
        const string ETRequestFailed = @"ETRequestFailed";
        const string ETRequestComplete = @"ETRequestComplete";
        const string ETRequestFailedOnLoadingResult = @"ETRequestFailedOnLoadingResult";
        const string ETRequestServiceReturnedError = @"ETRequestServiceReturnedError";
        const string ETRequestServiceResponseSuccess = @"ETRequestServiceResponseSuccess";
        const string ETRequestFinishedWithFailure = @"ETRequestFinishedWithFailure";

        // Constants for dealing with other stuff
        const string AppLifecycleForeground = @"AppLifecycleForeground";
        const string AppLifecycleBackground = @"AppLifecycleBackground";

        // Notifications around Messages
        const string RichMessagesNowAvailable = @"RichMessagesNowAvailable";

        // Geofence Constants
        const int ETLargeGeofence = 433; // Get it? 433 North Capitol. It's been a long journey getting here.
        const string ETLargeGeofenceIdentifier = @"ExactTargetMagicGlobalFence";

        // Caches
        const string ET_TAG_CACHE = @"ET_TAG_CACHE";
        const string ET_SUBKEY_CACHE = @"ET_SUBKEY_CACHE";
        const string ET_ATTR_CACHE = @"ET_ATTR_CACHE";

        // Tracks the BOOL for each in NSUserDefaults
        const string ETLocationServicesActive = @"ETLocationServicesActive";
        const string ETCloudPagesActive = @"ETCloudPagesActive";
        const string ETAnalyticsActive = @"ETAnalyticsActive";

        public enum PushOriginationState : int
        {
            Background = 0,
            Foreground,
        }
    }
}

