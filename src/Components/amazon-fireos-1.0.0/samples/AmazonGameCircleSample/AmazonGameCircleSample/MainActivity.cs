using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Amazon.Ags.Api;

[assembly: UsesPermission (Android.Manifest.Permission.Internet)]
[assembly: UsesPermission (Android.Manifest.Permission.AccessNetworkState)]

//IMPORTANT: Replace this Value with your own!!!
[assembly: MetaData ("APIKey", Value="YOUR-KEY-HERE")]

namespace AmazonGameCircleSample
{
	[Activity (Label = "Amazon GameCircle Sample", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			FindViewById<Button>(Resource.Id.go_whispersync).Click += delegate {
				StartActivity (typeof (WhispersyncActivity));
			};

			FindViewById<Button>(Resource.Id.go_leaderboard).Click += delegate {
				StartActivity (typeof (LeaderboardActivity));
			};

			FindViewById<Button>(Resource.Id.go_achievements).Click += delegate {
				StartActivity (typeof (AchievementsActivity));
			};
		}

		AmazonGamesClient agsClient;

		// List of features your game uses (only add those you need)
		Java.Util.EnumSet gameFeatures = Java.Util.EnumSet.Of(AmazonGamesFeature.Achievements,
			AmazonGamesFeature.Leaderboards, AmazonGamesFeature.Whispersync);

		protected override void OnResume ()
		{
			base.OnResume ();

			AmazonGamesClient.Initialize (this, new AmznGamesCallback {
				ServiceNotReadyAction = status => {
					// Unable to use service
				},
				ServiceReadyAction = client => {
					agsClient = client;

					// Ready to use GameCircle
				}
			}, gameFeatures);
		}

		protected override void OnPause ()
		{
			base.OnPause ();

			if (agsClient != null) {
				agsClient.Dispose ();
				agsClient = null;
			}
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy (); 

			AmazonGamesClient.Shutdown ();
		}
	}

	public class AmznGamesCallback : Java.Lang.Object, IAmazonGamesCallback
	{
		public Action<AmazonGamesStatus> ServiceNotReadyAction { get; set; }
		public Action<AmazonGamesClient> ServiceReadyAction { get; set; }

		public void OnServiceNotReady (AmazonGamesStatus status)
		{
			if (ServiceNotReadyAction != null)
				ServiceNotReadyAction (status);
		}

		public void OnServiceReady (AmazonGamesClient client)
		{
			if (ServiceReadyAction != null)
				ServiceReadyAction (client);
		}
	}
}


