using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
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
