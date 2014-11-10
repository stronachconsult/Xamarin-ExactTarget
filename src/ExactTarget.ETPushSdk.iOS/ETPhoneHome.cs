using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// ETPhoneHome is like a highway management system, governing the sending of data to and from ExactTarget, and caching that which can't get sent home. It works by marshalling around GenericUpdate object subclasses, which themselves create a common pattern for handling business. 
    /// 
    /// Data should be sent back using phoneHome:, which will start the process of sending data to ET, and failing that, save it to the database. The behavior is all controlled by methods on the GenericUpdate object.
    /// </summary>
	[BaseType(typeof(NSUrlConnectionDelegate))] //: NSUrlConnectionDataDelegate
    public partial interface ETPhoneHome
    {
        /// <summary>
        /// Singleton accessor. This isn't to be publicly used, so we can have a sense of humor about it.
        /// </summary>
        /// <value>
        /// The magic bicycle.
        /// </value>
        [Static, Export("magicBicycle")]
        ETPhoneHome magicBicycle { get; }

        /// <summary>
        /// Begins the process of sending data back to ExactTarget. 
        /// </summary>
        /// <param name="updateObject">A subclass of GenericUpdate that wants to be send to ET.</param>
        /// <returns>bool Whether or not it was able to send to ET.</returns>
        [Export("phoneHome:")]
        bool PhoneHome(ETGenericUpdate updateObject);

        /// <summary>
        /// Begins the process of sending data back to ExactTarget, but does so for bulk data. This is different than phoneHome: because it will send an array of things, and not just one object.
        /// </summary>
        /// <param name="updateClass">The update class.</param>
        /// <returns></returns>
        [Export("phoneHomeInBulkForGenericUpdateType:")]
        bool PhoneHomeInBulkForGenericUpdateType(Class updateClass);

        /// <summary>
        /// Saves the udpate object to the database in the event of a send failure. It is exposed in the header because some objects just need to be saved instead of sent. This method should not be used publicly.
        /// </summary>
        /// <param name="updateObject">The subclass of GenericUpdate to save to the database.</param>
        /// <returns>Whether or not the save succeeded. Sometimes they don't.</returns>
        [Export("saveToDatabaseInstead:")]
        bool SaveToDatabaseInstead(ETGenericUpdate updateObject);

        /// <summary>
        /// Checks the cache database for records that weren't successfully sent to ExactTarget, and tries to send them. No return value.
        /// </summary>
        [Export("checkForAndSendBackCachedData")]
        void CheckForAndSendBackCachedData();

        /// <summary>
        /// For ETPhoneHome_Tests
        /// </summary>
        /// <value>
        /// The get number of active connections.
        /// </value>
        [Export("getNumberOfActiveConnections")]
        int GetNumberOfActiveConnections { get; }
    }
}
