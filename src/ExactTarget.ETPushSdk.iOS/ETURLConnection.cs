using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// ETURLConnection is a wrapper around vanilla NSURLConnections that is useful because it adds things that Apple should have (tags) or things needed for ETPhoneHome to work correctly (reference to the sending object. Otherwise, it's just a regular NSURLConnection.
    /// </summary>
    [BaseType(typeof(NSUrlConnection))]
    public partial interface ETURLConnection
    {
        /// <summary>
        /// The tag of this particular connection. Usually the BackgroundTaskID from iOS.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        [Export("tag")]
        int Tag { get; set; }

        /// <summary>
        /// A reference to the sendingObject for this connection. That object will save the response data and status code, etc. This allows us to fire off a bunch of these things in parallel. 
        /// </summary>
        /// <value>
        /// The sending object.
        /// </value>
        [Export("sendingObject")]
        ETGenericUpdate SendingObject { get; set; }
    }
}
