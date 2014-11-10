using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// LocationUpdateAppState
    /// </summary>
    public enum LocationUpdateAppState : int
    {
        /// <summary>
        /// The background
        /// </summary>
        Background,
        /// <summary>
        /// The foreground
        /// </summary>
        Foreground,
    }
}
