using System;
using ObjCRuntime;
using Foundation;
using UIKit;

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

	// This enumeration defines what HTTP method should be used in sending the data. These are standard HTTP methods. 
	/// <summary>
	/// GenericUpdateSendMethod
	/// </summary>
	public enum GenericUpdateSendMethod : int
	{
		/// <summary>
		/// The get
		/// </summary>
		Get,
		/// <summary>
		/// The post
		/// </summary>
		Post,
		/// <summary>
		/// The put
		/// </summary>
		Put,
		/// <summary>
		/// The delete
		/// </summary>
		Delete
	}

	partial class Constants
	{
		/// <summary>
		/// The et request base URL
		/// </summary>
		const string ETRequestBaseURL = @"https://consumer.exacttargetapis.com";
	}

	partial class Constants
	{
		/// <summary>
		/// The et app id
		/// </summary>
		const string ETAppID = @"ETAppID";
		/// <summary>
		/// The access token
		/// </summary>
		const string AccessToken = @"AccessToken";
		/// <summary>
		/// The device token
		/// </summary>
		const string DeviceToken = @"DeviceToken";
		/// <summary>
		/// The device identifier
		/// </summary>
		const string DeviceIdentifier = @"DeviceID";
		/// <summary>
		/// The subscriber key
		/// </summary>
		const string SubscriberKey = @"SubscriberKey";

		/// <summary>
		/// The tags
		/// </summary>
		const string Tags = @"Tags";
		/// <summary>
		/// The attributes
		/// </summary>
		const string Attributes = @"Attributes";

		/// <summary>
		/// The cache delimeter
		/// </summary>
		const string CacheDelimeter = @"|^|";
		/// <summary>
		/// The cache key value delimeter
		/// </summary>
		const string CacheKeyValueDelimeter = @"|:|";
	}

	/// <summary>
	/// LocationUpdateAppState
	/// </summary>
	public enum LocationUpdateAppState : int
	{
		/// <summary>
		/// The background
		/// </summary>
		Background,
		/// <summary>
		/// The foreground
		/// </summary>
		Foreground,
	}

	/// <summary>
	/// Enumeration of the type of ETMessage this is. 
	/// </summary>
	public enum MobilePushMessageType : uint
	{
		/// <summary>
		/// Unknown
		/// </summary>
		Unknown,
		/// <summary>
		/// Basic - A standard push message
		/// </summary>
		Basic,
		/// <summary>
		/// Was a CloudPage message, but that is a ContentType now
		/// </summary>
		[Obsolete]
		Enhanced,
		/// <summary>
		/// Geofence Entry
		/// </summary>
		FenceEntry,
		/// <summary>
		/// Geofence Exit
		/// </summary>
		FenceExit,
		/// <summary>
		/// Proximity
		/// </summary>
		Proximity,
	}

	/// <summary>
	/// Bitmask of features that a message has. This is the representation of Push (AlertMessage), Push+Page (AlertMessage + Page), Page Only (Page) in the MobilePush UI.
	/// </summary>
	[Flags]
	public enum MobilePushContentType : uint
	{
		/// <summary>
		/// Unknown
		/// </summary>
		None = 0,
		/// <summary>
		/// Push Message
		/// </summary>
		AlertMessage = 1 << 0,
		/// <summary>
		/// CloudPage
		/// </summary>
		Page = 1 << 1,
	}

	/// <summary>
	/// Tracks where the currently parsing dictionary came from, because we run the values through twice to merge them together. 
	/// </summary>
	public enum MPMessageSource : int
	{
		/// <summary>
		/// Database
		/// </summary>
		Database,
		/// <summary>
		/// ExactTarget via REST
		/// </summary>
		Remote,
	}

	/// <summary>
	/// Time Unit enumeration for Message limiting.
	/// </summary>
	public enum MobilePushMessageFrequencyUnit : uint
	{
		/// <summary>
		/// Unknown
		/// </summary>
		None,
		/// <summary>
		/// Year
		/// </summary>
		Year,
		/// <summary>
		/// Month
		/// </summary>
		Month,
		/// <summary>
		/// Week
		/// </summary>
		Week,
		/// <summary>
		/// Day
		/// </summary>
		Day,
		/// <summary>
		/// Hour
		/// </summary>
		Hour,
	}

	partial class Constants
	{
		/// <summary>
		/// The base request URL
		/// </summary>
		const string BaseRequestURL = @"https://consumer.exacttargetapis.com";
	}

	/// <summary>
	/// Enumeration to keep track of if the request is for Geofences or Proximity messages. 
	/// </summary>
	public enum ETRegionRequestType : uint
	{
		/// <summary>
		/// The unknown
		/// </summary>
		Unknown,
		/// <summary>
		/// The geofence
		/// </summary>
		Geofence,
		/// <summary>
		/// The proximity
		/// </summary>
		Proximity,
	}

	/// <summary>
	/// Enumeration of the type of ETRegion that this is - Circle (Geofence) or Proximity (ibeacon). Polygon is not currently used.
	/// </summary>
	public enum MobilePushGeofenceType : uint
	{
		/// <summary>
		/// The none
		/// </summary>
		None = 0,
		/// <summary>
		/// The circle
		/// </summary>
		Circle,
		/// <summary>
		/// The polygon
		/// </summary>
		[Obsolete]
		Polygon, // Not currently in use.
		/// <summary>
		/// The proximity
		/// </summary>
		Proximity,
	}
}

