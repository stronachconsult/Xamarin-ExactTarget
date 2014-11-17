using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// ETEvent is the new form of analytics for the MobilePush SDK, replacing ETStatsUpdate. It is more flexible in that it can return multiple types of events in one pass. As such, things that are related to each other should be sent back together, and things that are not should be sent separately. It is also unique amongst the ETGenericUpdates in that it was designed to be bulk-sent back to ET, and not individually like the rest. 
    ///
    /// An ETEvent is comprised of some combination of the following: a Value, or the amount of units that we are currently measuring, zero or many ObjectIds, or the ET encoded IDs for things like messages, fences, etc, and one or many AnalyticTypes, or what analytics are contained in this update. One common use of this is time in app - If a user spends 60 seconds (1 minute) in the app, you would make an ETEvent with a value of 60 and add the analytic type MobilePushStatisticTypeTimeInApp. If you want to add a message ID for an open, add a second analyticType of MobilePushStatisticTypeOpen and add the objectId of the message ID (_m). Easy peasey.
    /// 
    /// Some of the enum values are marked deprecated, which indicates that they should not be used in the SDK. These enum values are for analytics on the Middle Tier only. Do not use these in ETEvents.
    /// </summary>
    [BaseType(typeof(ETGenericUpdate), Delegates = new[] { "WeakDelegate" }, Events = new[] { typeof(ETGenericUpdateObjectProtocol) })]
    [DisableDefaultCtor]
    public partial interface ETEvent
    {
        /// <summary>
        /// DBID if the data was saved in the local database, or zero if this is a new object. 
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Export("identifier")]
        int Identifier { get; }

        /// <summary>
        /// When the ETEvent happened / was created. Is set to now when the ETEvent is alloc/init-ed.
        /// </summary>
        /// <value>
        /// The eevent date.
        /// </value>
        [Export("eventDate")]
        NSDate EventDate { get; }

        /// <summary>
        /// The descriptive value for this ETEvent. It is relative to the analyticTypes. 
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [Export("value")]
        int Value { get; set; }

        /// <summary>
        /// The current set of analyticTypes being tracked by this ETEvent. Add new ones through addAnalyticType:
        /// </summary>
        /// <value>
        /// The analytic types.
        /// </value>
        [Export("analyticTypes")]
        NSSet AnalyticTypes { get; }

        /// <summary>
        /// The current set of objectIds being tracked by this ETEvent. Add new ones through addObjectId:
        /// </summary>
        /// <value>
        /// The object ids.
        /// </value>
        [Export("objectIds")]
        NSSet ObjectIds { get; }

        /// <summary>
        /// Designated Initializer. Pass in an NSDictionary (usually from the cache db) to create an ETEvent.
        /// </summary>
        /// <param name="dict">A dictionary of values.</param>
        /// <returns>id An ETEvent</returns>
        [Export("initFromDictionary:")]
        IntPtr Constructor(NSDictionary dict);

        /// <summary>
        /// Adds a specific analyticType to the current set. You may add zero to many, relative to what you're reporting. 
        /// </summary>
        /// <param name="statType">The analyticType you wish to add. See MobilePushStatisticType enumeration for values.</param>
        [Export("addAnalyticType:")]
        void AddAnalyticType(MobilePushStatisticType statType);

        /// <summary>
        /// Removes an analytic type from the current set. It will echo out if it was removed, or nil if the value wasn't in there.
        /// </summary>
        /// <param name="statType">The type to remove.</param>
        /// <returns>An echo of statType if the value was removed, or nil if not.</returns>
        [Export("removeAnalyticType:")]
        MobilePushStatisticType RemoveAnalyticType(MobilePushStatisticType statType);

        /// <summary>
        /// Adds a specific ObjectID to the current set. These values are all encoded by the time they're on the device, so you can add any of them, regardless of what they are - meaning, Message ID (_m), Fence ID (ETRegion fenceIdentifier), etc. Just throw them in. 
        /// </summary>
        /// <param name="objectId">An objectID to add.</param>
        [Export("addObjectId:")]
        void AddObjectId(string objectId);

        /// <summary>
        /// Removes a specific ObjectID from the set, and echoes it if it was removed, or nil if the value wasn't in there.
        /// </summary>
        /// <param name="objectId">The objectId to add.</param>
        /// <returns>An echo of objectId if it was removed, or nil if it wasn't found.</returns>
        [Export("removeObjectId:")]
        string RemoveObjectId(string objectId);
    }
}
