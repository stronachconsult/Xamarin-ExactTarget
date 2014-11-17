using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// This is a simple little database wrapper that lets us persist data in a generic place and way. Because sometimes we need to do that, and clustering up NSUserDefaults is silly. 
    /// 
    /// For the sake of simplicity, this store only deals with NSStrings. Please plan accordingly. 
    /// </summary>
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    public partial interface ETKeyValueStore
    {
        /// <summary>
        /// Sets the value for a specific key into the ETKeyValueStore. Both things are NSString objects.
        /// </summary>
        /// <param name="value">The value to save.</param>
        /// <param name="key">The key to save.</param>
        /// <returns>T/F if the save worked.</returns>
        [Static, Export("setValue:forKey:")]
        bool SetValue(string value, string key);

        /// <summary>
        /// Retrieves a value for a given key from the database, or nil if it's not in there.
        /// </summary>
        /// <param name="key">The key to retrieve.</param>
        /// <returns>The value, or nil if it's not there.</returns>
        [Static, Export("valueForKey:")]
        string ValueForKey(string key);
    }
}
