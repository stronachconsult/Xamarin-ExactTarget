using System;
using Foundation;
using CoreLocation;
using UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// ETStatsUpdate
    /// </summary>
    [BaseType(typeof(ETGenericUpdate), Delegates = new[] { "Delegate" }, Events = new[] { typeof(NSCopying) })]
    [DisableDefaultCtor]
    public partial interface ETStatsUpdate
	{
        /// <summary>
        /// Time in app, in seconds
        /// </summary>
        /// <value>
        /// The seconds in app.
        /// </value>
        [Export("secondsInApp")]
        nint SecondsInApp { get; set; }

        /// <summary>
        /// From ET, the message identifier.
        /// </summary>
        /// <value>
        /// The message id.
        /// </value>
        [Export("messageID")]
        string MessageID { get; set; }

        /// <summary>
        /// Which fence got broken
        /// </summary>
        /// <value>
        /// The fence id.
        /// </value>
        [Export("fenceID")]
        string FenceID { get; set; }

        /// <summary>
        /// For local notifications, this is shown when they fire. ie, firedate
        /// </summary>
        /// <value>
        /// The display date.
        /// </value>
        [Export("displayDate")]
        NSDate DisplayDate { get; set; }

        /// <summary>
        /// Generic open date, maybe null usually, since no one opens our apps
        /// </summary>
        /// <value>
        /// The open date.
        /// </value>
        [Export("openDate")]
        NSDate OpenDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        [Export("messageType")]
        MobilePushMessageType MessageType { get; set; }
    }
}
