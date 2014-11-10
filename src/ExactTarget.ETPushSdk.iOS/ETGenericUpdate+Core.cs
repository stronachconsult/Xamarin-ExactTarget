using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
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
}
