using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// This is a helper class that shows webpages. These come down in several forms - sometimes a CloudPage, sometimes something from OpenDirect - and this guy shows them. It's a pretty simple class that pops up a view with a toolbar, shows a webpage, and waits to be dismissed. 
    /// </summary>
    [BaseType(typeof(UIViewController))]
    public partial interface ETLandingPagePresenter
    {
        // UIWebView _theWebView;
        // UILabel _pageTitle;

        /// <summary>
        /// Don't let the name fool you - this can be *any* URL, not just a landing page. It will eventually be converted to an NSURL and displayed.
        /// </summary>
        /// <value>
        /// The landing page path.
        /// </value>
        [Export("landingPagePath")]
        string LandingPagePath { get; set;  }

        /// <summary>
        /// A helper designated initializer that takes the landing page as a string.
        /// </summary>
        /// <param name="landingPage">The landing page.</param>
        /// <returns></returns>
        [Export("initForLandingPageAt:")]
        IntPtr Constructor(string landingPage);

        /// <summary>
        /// Another helper that takes it in NSURL form. We're not picky. It'd be cool of ObjC did method overloading, though.
        /// </summary>
        /// <param name="landingPage">The landing page.</param>
        /// <returns></returns>
        [Export("initForLandingPageAtWithURL:")]
        IntPtr Constructor(NSUrl landingPage);
    }
}
