using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ExactTarget.ETPushSdk;

namespace ETMobilePushSample
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			// If you have defined a root view controller, set it here:
			window.RootViewController = new MainPageViewController();
			
			// make the window visible
			window.MakeKeyAndVisible ();

			//example code: https://github.com/ExactTarget/MobilePushSDK-iOS/blob/master/practicefield/PublicDemo/PUDAppDelegate.m
			try {
				var launchOptions = new NSMutableDictionary ();
				ETPush.PushManager.ConfigureSDKWithAppID ("etid", "accessToken", true, true, true);
				ETPush.PushManager.OpenDirectDelegate = new HandleOpenDirectDelegate();
				ETPush.PushManager.ApplicationLaunchedWithOptions (launchOptions);
				ETPush.PushManager.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound);
				ETPush.PushManager.ShouldDisplayAlertViewIfPushReceived(true);
				ETLocationManager.LocationManager.StartWatchingLocation ();
				Console.WriteLine("== DEVICE ID ==\\nThe ExactTarget Device ID is: {0}", ETPush.SafeDeviceIdentifier);
			} catch (Exception ex) {
				Console.WriteLine ("Error Message {0}", ex.Message);
			}

			return true;
		}

		class HandleOpenDirectDelegate: ExactTarget.ETPushSdk.ExactTargetOpenDirectDelegate {
			public virtual bool ShouldDeliverOpenDirectMessageIfAppIsRunning {
				get {
					return false;
				}
			}

			public virtual void DidReceiveOpenDirectMessageWithContents (string payload)
			{
				//todo: handle payload

			}
		}

		public override void WillEnterForeground (UIApplication application)
		{
			ETPush.PushManager.ResetBadgeCount();
		}

		public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
			ETPush.PushManager.RegisterDeviceToken (deviceToken);
		}

		public override void FailedToRegisterForRemoteNotifications (UIApplication application, NSError error)
		{
			ETPush.PushManager.ApplicationDidFailToRegisterForRemoteNotificationsWithError (error);
		}

		public override void DidReceiveRemoteNotification (UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			ETPush.PushManager.HandleNotification (userInfo, application.ApplicationState);
		}
	}
}

