using System;
using Foundation;
using CoreLocation;
using UIKit;

namespace ExactTarget.ETPushSdk
{
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
}
