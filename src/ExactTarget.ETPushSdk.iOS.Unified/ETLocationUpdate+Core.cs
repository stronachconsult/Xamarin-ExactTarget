using System;
using Foundation;
using CoreLocation;
using UIKit;

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
