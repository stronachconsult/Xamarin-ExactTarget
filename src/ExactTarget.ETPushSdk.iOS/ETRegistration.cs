using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// ETRegistration sends data about the device back to ExactTarget. The data in here covers a few different pieces, from things necessary to make the wheels spin (app id, access token, device id, token, etc) to extra things used for segmentation but not necessarily required (attributes, tags). 
    /// 
    /// ETRegistrations are generated nearly completely programmatically, so you just have to make one and throw it at the server. They are not saved to the database because of this - the values should not change over time, and if they're new later, the old values don't matter.
    /// </summary>
    [BaseType(typeof(ETGenericUpdate))]
    public partial interface ETRegistration
    {
        /// <summary>
        /// Makes a new Registration Update object. One per session.
        /// </summary>
        /// <returns></returns>
        //[Export("init")]
        //IntPtr Constructor();
    }
}
