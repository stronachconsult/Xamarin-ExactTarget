using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// Local definitions of the Middle Tier Statistic Type enumeration. 
    /// </summary>
    public enum MobilePushStatisticType : uint
    {
        /// <summary>
        /// Unknown value.
        /// </summary>
        None = 0,
        /// <summary>
        /// Designates a push send.
        /// </summary>
        [Obsolete]
        Send,
        /// <summary>
        /// Push Message or Local Notification opened. Must be accompanied by a message ID in objectIds
        /// </summary>
        Open,
        /// <summary>
        /// Used when we schedule a UILocalNotification for display. Must be accompanied by the ETMessage messageIdentifier and ETRegion fenceIdentifier.
        /// </summary>
        Display,
        /// <summary>
        /// How long the user was in the app. Must be accmopanied by a non-zero Value.
        /// </summary>
        TimeInApp,
        /// <summary>
        /// Combined value for legacy purposes.
        /// </summary>
        [Obsolete]
        TimeInAppOpens,
        /// <summary>
        /// Used to mark entrance in an ETRegion. Must be accompanied by a ETRegion fenceIdentifier.
        /// </summary>
        FenceEntry,
        /// <summary>
        /// Used to mark exit from an ETRegion. Must be accompanied by a ETRegion fenceIdentifier.
        /// </summary>
        FenceExit,
        /// <summary>
        /// ActiveUsers
        /// </summary>
        [Obsolete]
        ActiveUsers,
        /// <summary>
        /// ActiveUsers
        /// </summary>
        [Obsolete]
        InactiveUsers,
        /// <summary>
        /// FUTURE - Use to indicate push message receipt on device.
        /// </summary>
        Received,
        /// <summary>
        /// How long the user spent in a given ETRegion - GEOFENCE ONLY. Must be accompanied by a non-zero Value.
        /// </summary>
        TimeInLocation,
        /// <summary>
        /// Indicates which ETRegion iBeacon prompted the trigger. Must be accompanied by a ETRegion fenceIdentifier.
        /// </summary>
        BeaconInRange,
        /// <summary>
        /// How long the user spent in *any* proximity of a known ETRegion iBeacon. Must be accompanied by a non-zero Value and an ETRegion fenceIdentifier.
        /// </summary>
        TimeWithBeaconInRange,
    }
}
