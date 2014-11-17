using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// ETFenceMessage is a middle class that joins together ETRegions and ETMessages, which exist in a many-to-many relationship with each other.
    /// 
    /// The SQL of this is set to ON CONFLICT REPLACE, meaning the pair of values will only ever be in there once. 
    /// 
    /// ETFenceMessages should be passed to ETPhoneHome through saveToDatabaseInstead: only. They shouldn't be sent to phoneHome:.
    /// </summary>
    [BaseType(typeof(ETGenericUpdate))]
    [DisableDefaultCtor]
    public partial interface ETFenceMessage
    {
        /// <summary>
        /// The ETRegion for this relationship.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        [Export("region")]
        ETRegion Region { get; set; }

        /// <summary>
        /// The ETMessage for this relationship.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [Export("message")]
        ETMessage message { get; set; }

        /// <summary>
        ///  Creates a new ETFenceMessage for a specific ETRegion and ETMessage.
        /// </summary>
        /// <param name="region">The ETRegion half of this relationship.</param>
        /// <param name="message">The ETMessage half of this relationship.</param>
        /// <returns>A new ETFenceMessage.</returns>
        [Export("initWithRegion:andMessage:")]
        IntPtr Constructor(ETRegion region, ETMessage message);
    }
}
