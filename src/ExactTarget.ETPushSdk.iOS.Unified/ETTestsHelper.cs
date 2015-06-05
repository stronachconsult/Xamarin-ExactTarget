using System;
using Foundation;
using CoreLocation;
using UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// ETTestsHelper
    /// </summary>
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    public partial interface ETTestsHelper
    {
        /// <summary>
        /// Tests the help.
        /// </summary>
        /// <returns></returns>
        [Static, Export("testHelp")]
        ETTestsHelper TestHelp();

        /// <summary>
        /// Clears the database if exists.
        /// </summary>
        [Export("clearDatabaseIfExists")]
        void ClearDatabaseIfExists();

        /// <summary>
        /// Enables the poll.
        /// </summary>
        [Export("enablePoll")]
        void EnablePoll();

        /// <summary>
        /// Disables the poll.
        /// </summary>
        [Export("disablePoll")]
        void DisablePoll();

        /// <summary>
        /// Does the additional poll.
        /// </summary>
        [Export("doAdditionalPoll")]
        void DoAdditionalPoll();
    }
}
