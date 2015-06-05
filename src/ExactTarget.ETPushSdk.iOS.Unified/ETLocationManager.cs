using System;
using Foundation;
using CoreLocation;
using UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// ETLocationManager is the main interface to ExactTarget's Location Services. In the way that ETPush manages the push notification cycle, ETLocationMangaer manages geo services. It will use some of the information from ETPush (namely, App ID and Access Token) to function, but is an independent piece of functionality. 
    /// 
    /// Due to the invasive nature of location services, ETLocationManager defaults to off, and must be explicity turned on by the developer, whether invisibly to the user or not. To begin location services, call [[ETLocationManager locationManager] startWatchingLocation]. Similarly, to stop location services, call [[ETLocationManager locationManager] stopWatchingLocation]. 
    /// 
    /// ETLocationManager will always respect the user's wishes (as enforced by iOS), so if the user disables Location Services at the system level through Settings, the SDK will be unable to use any location services, and fence monitoring will not function. You can check for this by querying [[ETLocationManager locationManager] locationEnabled], as it will reconcile app-level permissions as well as the state of you enabling loc services. Internally, this method is used to report back to ExactTarget on the state of location services, so it is trustworthy. 
    /// 
    /// Please ensure you are linking against CoreLocation. You will get errors otherwise. 
    /// </summary>
    [BaseType(typeof(NSObject), Delegates = new[] { "Delegate" }, Events = new[] { typeof(CLLocationManagerDelegate) })]
    //[DisableDefaultCtor]
    public partial interface ETLocationManager
    {
        /// <summary>
        /// Keeps track of if we are currently in the middle of updating Geofences. That should only work one at a time.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [updating geofences]; otherwise, <c>false</c>.
        /// </value>
        [Export("updatingGeofences")]
        bool UpdatingGeofences { [Bind("isUpdatingGeofences")] get; set; }

        /// <summary>
        /// Returns a reference to the shared loc manager.
        /// </summary>
        /// <value>
        /// The location manager.
        /// </value>
        [Static, Export("locationManager")]
        ETLocationManager LocationManager { get; }

        /// <summary>
        /// Constructors this instance.
        /// </summary>
        /// <returns></returns>
        //[Export("init")]
        //IntPtr Constructor();

        /// <summary>
        /// Determines the state of Location Services based on developer setting and OS-level permission. This is the preferred method for checking for location state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [location enabled]; otherwise, <c>false</c>.
        /// </value>
        [Export("locationEnabled")]
        bool LocationEnabled { get; }

        /// <summary>
        /// Use this method to initiate Location Services through the MobilePush SDK.
        /// </summary>
        [Export("startWatchingLocation")]
        void StartWatchingLocation();

        /// <summary>
        /// Use this method to disable Location Services through the MobilePush SDK
        /// </summary>
        [Export("stopWathingLocation")]
        void StopWathingLocation();

        /// <summary>
        /// Calls various handlers that should fire when the app enters the foreground.
        /// </summary>
        [Export("appInForeground")]
        void AppInForeground();

        /// <summary>
        /// Calls various handlers that should fire when the app enters the background.
        /// </summary>
        [Export("appInBackground")]
        void AppInBackground();

        /// <summary>
        /// Queues a send for a location update to ExactTarget.
        /// </summary>
        /// <param name="loc">The loc.</param>
        /// <param name="state">The state.</param>
        [Export("updateLocationServerWithLocation:forAppState:")]
        void UpdateLocationServerWithLocation(CLLocation loc, LocationUpdateAppState state);

        #region Location

        /// <summary>
        /// Takes in an NSSet of fences that should be monitored.
        /// </summary>
        /// <param name="fences">The set to monitor.</param>
        [Export("monitorRegions:")]
        void MonitorRegions(NSSet fences);

        /// <summary>
        /// Instructs the CLLocationManager to stop monitoring all regions.
        /// </summary>
        [Export("stopMonitoringRegions")]
        void StopMonitoringRegions();

        /// <summary>
        /// Retrieves the messages for a given ETRegion and MobilePushMessageType and schedules any messages returned for display.
        /// </summary>
        /// <param name="region">The ETRegion that prompted this action.</param>
        /// <param name="type">The MobilePushMessageType of event that prompted this action.</param>
        [Export("getAndScheduleAlertsForRegion:andMessageType:")]
        void GetAndScheduleAlertsForRegion(ETRegion region, MobilePushMessageType type);

        /// <summary>
        /// Returns the currently monitored regions.
        /// </summary>
        /// <value>
        /// An NSSet of monitored regions.
        /// </value>
        [Export("monitoredRegions")]
        NSSet MonitoredRegions { get; }

        /// <summary>
        /// A dictionary version of the Last Known Location. The dictionary will contain two keys, latitude and longitude, which are NSNumber wrappers around doubles. Use doubleValue to retrieve.
        /// </summary>
        /// <value>
        /// The last known location.
        /// </value>
        [Export("lastKnownLocation")]
        NSDictionary LastKnownLocation { get; }

        #endregion

        #region For Tests

        /// <summary>
        /// Returns if we are currently watching location.
        /// </summary>
        /// <value>
        /// T/F if locations are being watched.
        /// </value>
        [Export("getWatchingLocation")]
        bool GetWatchingLocation { get; }

        #endregion
    }
}
