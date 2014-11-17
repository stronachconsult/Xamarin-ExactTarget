using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// ETMessage is the local representation of a Message from ExactTarget. They are multipurpose, sometimes representing a message that should be scheduled because of the entrance or exit of a Geofence, the proximal arrival to an iBeacon, or a CloudPage message downloaded from ET. Because of their multipurpose nature, there are a lot of different attributes on them, many of which may be null at any give time depending on the type of message. 
    /// 
    /// ETMessages also feature Message Limiting, a system of preventing a given message from firing too often. If described in a sentence with the parameters interlaced, it would read "show this message only 1 (messagesPerPeriod) time per 1 (numberOfPeriods) hour (periodType). As of a recent release, messagesPerPeriod will be defaulted to 1 on the Middle Tier, so if it is null or absent, we assume 1, otherwise take the given value.
    /// 
    /// Many of these accessors are readonly because the value should be trusted and not changed. There are specific methods to modify the message, such as markAsRead or markAsDeleted. Also, unless specifically marked in a method, only active methods are returned through the getter methods. 
    /// </summary>
    [BaseType(typeof(ETGenericUpdate))]
    [DisableDefaultCtor]
    public partial interface ETMessage
    {
        /// <summary>
        /// Encoded ID from ExactTarget. Will match the ID in MobilePush. This is the primary key. 
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        [Export("messageIdentifier")]
        string MessageIdentifier { get; }

        /// <summary>
        /// This is the name which is set on ExactTargetMarketingCloud, while setting the ETMessage.
        /// </summary>
        /// <value>
        /// The name of the message.
        /// </value>
        [Export("messageName")]
        string MessageName { get; set; }

        /// <summary>
        /// The type of ETMessage being represented.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        [Export("messageType")]
        MobilePushMessageType MessageType { get; }

        /// <summary>
        /// Bitmask of features that this message has on it (CloudPage, Push only)
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [Export("contentType")]
        MobilePushContentType ContentType { get; }

        /// <summary>
        /// The alert text of the message. This displays on the screen. 
        /// </summary>
        /// <value>
        /// The alert.
        /// </value>
        [Export("alert")]
        string Alert { get; }

        /// <summary>
        /// The sound that should play, if any. Most of the time, either "default" or "custom.caf", conventions enforced in MobilePush. 
        /// </summary>
        /// <value>
        /// The sound.
        /// </value>
        [Export("sound")]
        string Sound { get; }

        /// <summary>
        /// The badge modifier. This should be a NSString in the form of "+1" or nothing at all. It's saved as a string because of that.
        /// </summary>
        /// <value>
        /// The badge.
        /// </value>
        [Export("badge")]
        string Badge { get; }

        /// <summary>
        /// An array of Key Value Pairs, or Custom Keys in local parlance, for this message. This will contain NSDictionary objects.
        /// </summary>
        /// <value>
        /// The key value pairs.
        /// </value>
        [Export("keyValuePairs")]
        NSObject[] KeyValuePairs { get; }

        /// <summary>
        /// The message's start date. Messages shouldn't show before this time. 
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Export("startDate")]
        NSDate StartDate { get; }

        /// <summary>
        /// The message's end date. Messages shouldn't show after this time.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Export("endDate")]
        NSDate EndDate { get; }

        /// <summary>
        /// The Site ID for the CloudPage attached to this message. 
        /// </summary>
        /// <value>
        /// The site identifier.
        /// </value>
        [Export("siteIdentifier")]
        string SiteIdentifier { get; }

        /// <summary>
        /// The Site URL for the ClouePage attached to this message. It is saved as an NSString and converted later to NSURL.
        /// </summary>
        /// <value>
        /// The site URL as string.
        /// </value>
        [Export("siteUrlAsString")]
        string SiteUrlAsString { get; }

        /// <summary>
        /// OpenDirect payload for this message, if there is one. 
        /// </summary>
        /// <value>
        /// The open direct payload.
        /// </value>
        [Export("openDirectPayload")]
        string OpenDirectPayload { get; }

        /// <summary>
        /// The related ETRegion for this message. This is a remnant of days when the relationship was one to one. It is not anymore. 
        /// </summary>
        /// <value>
        /// The related fence.
        /// </value>
        [Obsolete]
        [Export("relatedFence")]
        ETRegion RelatedFence { get; }

        /// <summary>
        /// The total number of times, ever, that a message will show on a device. 
        /// </summary>
        /// <value>
        /// The message limit.
        /// </value>
        [Export("messageLimit")]
        int MessageLimit { get; }

        /// <summary>
        /// The total number of times for a given number of time units that a message can be shown. In the statement "show 1 time per 2 hours", this is the "1" part.
        /// 
        /// This defaults to 1 if it is not set in the received payload from ExactTarget. 
        /// </summary>
        /// <value>
        /// The messages per period.
        /// </value>
        [Export("messagesPerPeriod")]
        int MessagesPerPeriod { get; }

        /// <summary>
        /// The number of time periods in which a message should be limited. In the statement "show 1 time per 2 hours", this is the "2" part.
        /// </summary>
        /// <value>
        /// The number of periods.
        /// </value>
        [Export("numberOfPeriods")]
        int NumberOfPeriods { get; }

        /// <summary>
        /// The time unit counted in numberOfPeriods. In the statement "show 1 time per 2 hours", this is the "hours" part.
        /// </summary>
        /// <value>
        /// The type of the period.
        /// </value>
        [Export("periodType")]
        MobilePushMessageFrequencyUnit PeriodType { get; }

        /// <summary>
        /// Whether or not the period is a rolling period. Defaults to YES through code. 
        /// 
        /// Consider a message being fired at 2:19PM, and it may only be shown once per hour. In a rolling period, the next time it may show is 3:19PM. In a non-rolling period, the next earliest showing time is 3:00PM, the start of the next hour. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if [rolling period]; otherwise, <c>false</c>.
        /// </value>
        [Export("rollingPeriod")]
        bool RollingPeriod { [Bind("isRollingPeriod")] get; }

        /// <summary>
        /// The number of times an ETRegion must be tripped before the message shows. This is not currently used, and is a placeholder for future functionality.
        /// </summary>
        /// <value>
        /// The read.
        /// </value>
        [Export("minTripped")]
        NSNumber MinTripped { get; }

        /// <summary>
        /// Ephemeral Messages disappear when the user walks away from the iBeacon that tripped the message. The default value is NO.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [read]; otherwise, <c>false</c>.
        /// </value>
        /// 
        [Export("ephemeralMessage")]
        bool EphemeralMessage { [Bind("isEphemeralMessage")] get; }

        /// <summary>
        /// For iBeacon messages, the proximity the user must arrive in before the message is fired. It is treated as a "less than" value, meaning if the message is set to Far, the message can be shown in Far, Near or Immediate.
        /// </summary>
        /// <value>
        /// The proximity.
        /// </value>
        [Export("proximity")]
        CLProximity Proximity { get; }

        /// <summary>
        /// The number of seconds the user must stand near an iBeacon before the message is displayed. This is treated as an offset in scheduling the UILocalNotification, which will be cancelled if the user walks away too early. 
        /// </summary>
        /// <value>
        /// The loitering seconds.
        /// </value>
        [Export("loiteringSeconds")]
        int loiteringSeconds { get; }

        /// <summary>
        /// Whether or not the message has been read. This must be set through markAsRead by the developer.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [read]; otherwise, <c>false</c>.
        /// </value>
        [Export("read")]
        bool Read { [Bind("isRead")] get; }

        /// <summary>
        /// Whether or not the message is active in the local database. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if [active]; otherwise, <c>false</c>.
        /// </value>
        [Export("active")]
        bool Active { [Bind("isActive")] get; }

        /// <summary>
        /// A reference to the UILocalNotification triggered for this message. It is used later to cancel the message if need be.
        /// </summary>
        /// <value>
        /// The notification.
        /// </value>
        [Export("notification")]
        UILocalNotification Notification { get; set; }

        /// <summary>
        /// Creates a new ETMessage with values in the given NSDictionary.
        /// </summary>
        /// <param name="dict">A dictionary of values to apply to the ETMessage.</param>
        /// <returns>A new ETMessage.</returns>
        [Export("initFromDictionary:")]
        IntPtr Constructor(NSDictionary dict);

        /// <summary>
        /// Designated Initializer. Creates a new ETMessage with values from an NSDictionary for a specific ETRegion.
        /// </summary>
        /// <param name="dict">A dictionary of values to apply to the ETMessage.</param>
        /// <param name="region">The ETRegion that prompted the creation of this ETMessage.</param>
        /// <returns>A new ETMessage.</returns>
        [Export("initFromDictionary:forFence:")]
        IntPtr Constructor(NSDictionary dict, ETRegion region);

        /// <summary>
        /// This is an overridden accessor for subj ect to handle some business logic around what to show. Use this for display in an inbox.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [Export("subject")]
        string Subject { get; } // Public getter, now with logic.

        /// <summary>
        /// Cleanses and returns the Sites URL as a proper NSURL. This is mostly for convenience.
        /// </summary>
        /// <value>
        /// The site URL.
        /// </value>
        [Export("siteURL")]
        NSUrl SiteURL { get; }

        /// <summary>
        /// Marks a message as read in the local cache. Read messages do not show up in the Inbox.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [mark as read]; otherwise, <c>false</c>.
        /// </value>
        [Export("markAsRead")]
        bool MarkAsRead();

        /// <summary>
        /// Records a message as scheduled, and updates the fun, fun logic around when it should again, if it should of course. 
        /// </summary>
        /// <value>
        /// <c>true</c> if [message scheduled for display]; otherwise, <c>false</c>.
        /// </value>
        [Export("messageScheduledForDisplay")]
        bool MessageScheduledForDisplay();

        /// <summary>
        /// Marks a message as unread. Just for convenience. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if [mark as unread]; otherwise, <c>false</c>.
        /// </value>
        [Export("markAsUnread")]
        bool MarkAsUnread();

        /// <summary>
        /// Marks a message as deleted. They will not be returned after this, and it's irreversable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [mark as deleted]; otherwise, <c>false</c>.
        /// </value>
        [Export("markAsDeleted")]
        bool MarkAsDeleted();

        #region Methods for Testing

        /// <summary>
        /// Getter for a private value, lastShownDate.
        /// </summary>
        /// <value>
        /// The get last shown date.
        /// </value>
        [Export("getLastShownDate")]
        NSDate GetLastShownDate { get; }

        /// <summary>
        /// Getter for a private value, showCount.
        /// </summary>
        /// <value>
        /// The get show count.
        /// </value>
        [Export("getShowCount")]
        int GetShowCount { get; }

        #endregion

        #region Message Retrieval Helpers

        /// <summary>
        /// Gets all active messages for a specific contentType, usually Cloud Pages.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>An NSArray of ETMessages</returns>
        [Static, Export("getMessagesByContentType:")]
        NSObject[] GetMessagesByContentType(MobilePushContentType contentType);

        /// <summary>
        /// Gets a specific ETMessage for a given identifer. 
        /// </summary>
        /// <param name="identifier">The Message ID to retrieve.</param>
        /// <returns>The ETMessage, or nil if not found in the database.</returns>
        [Static, Export("getMessageByIdentifier:")]
        ETMessage GetMessageByIdentifier(string identifier);

        /// <summary>
        /// Gets all active ETMessages for a specific message type, like Fence Entry, Exit or Proximity..
        /// </summary>
        /// <param name="type">The MobilePushMessageType you'd like back.</param>
        /// <returns>An array of ETMessages.</returns>
        [Static, Export("getMessagesByType:")]
        NSObject[] GetMessagesByType(MobilePushMessageType type);

        /// <summary>
        /// Gets all active ETMessages tied to a specific ETRegion (Geofence).
        /// </summary>
        /// <param name="fence">The ETRegion for which you would like messages.</param>
        /// <returns>An NSArray of ETMessages.</returns>
        [Static, Export("getMessagesForGeofence:")]
        NSObject[] GetMessagesForGeofence(ETRegion fence);

        /// <summary>
        /// Gets all active ETMessages tied to a specific ETRegion (Geofence) and MobilePushMessageType, like Entry ot Exit.
        /// </summary>
        /// <param name="fence">The ETRegion for which you would like messages.</param>
        /// <param name="type">The MobilePushMessageType that describes the messages you want.</param>
        /// <returns>An NSArray of ETMessages that meet the criteria asked for.</returns>
        [Static, Export("getMessagesForGeofence:andMessageType:")]
        NSObject[] GetMessagesForGeofence(ETRegion fence, MobilePushMessageType type);

        /// <summary>
        /// Gets all active ETMessages for a specific ETRegion (Proximity).
        /// </summary>
        /// <param name="region">The ETRegion for which you would like messages.</param>
        /// <returns>An NSArray of ETMessages.</returns>
        [Static, Export("getProximityMessagesForRegion:")]
		NSObject[] GetProximityMessagesForRegion(ETRegion fence); // withRangedBeaconProximity:(CLProximity)prox;

        /// <summary>
        /// Triggeres a data pull from ExactTarget for messages that meet the supplied requirements.
        /// </summary>
        /// <param name="messageType">The Message Type you wish to retrieve.</param>
        /// <param name="contentType">The Content Type you wish to retrieve.</param>
        /// <remarks>Doesn't return a value, but has delegate callbacks. </remarks>
        [Static, Export("getMessagesFromExactTargetOfMessageType:andContentType:")]
        void GetMessagesFromExactTargetOfMessageType(MobilePushMessageType messageType, MobilePushContentType contentType);

        /// <summary>
        /// Marks all messages for a given type as inactive. This is done prior to processing new messages just received from ExactTarget. 
        /// </summary>
        /// <param name="type">type The MobilePushMessageType you wish to invalidate.</param>
        /// <returns>T/F if the invalidation query worked</returns>
        [Static, Export("invalidateAllMessagesForType:")]
        bool InvalidateAllMessagesForType(MobilePushMessageType type);

        /// <summary>
        /// ETMessage equality. Since object equality won't always work, this compares messageIdentifiers to determine equality.
        /// </summary>
        /// <param name="message">The ETMessage to compare self to.</param>
        /// <returns>T/F if the messages are equal.</returns>
        [Export("isEqualToMessage:")]
        bool IsEqualToMessage(ETMessage message);

        #endregion
    }
}
