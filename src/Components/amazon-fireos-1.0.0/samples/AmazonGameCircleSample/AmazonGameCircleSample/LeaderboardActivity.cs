
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Amazon.Ags.Api;
using Amazon.Ags.Api.Leaderboards;

namespace AmazonGameCircleSample
{
	[Activity (Label = "Leaderboards")]			
	public class LeaderboardActivity : Activity, IAmazonGamesCallback
	{
		AmazonGamesClient agsClient;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.leaderboard_activity);

			FindViewById<Button>(Resource.Id.buttonShowLeaderboard).Click += delegate {
				agsClient.LeaderboardsClient.ShowLeaderboardsOverlay ();
			};
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			var features = Java.Util.EnumSet.Of (AmazonGamesFeature.Leaderboards);
			AmazonGamesClient.Initialize (this, this, features);
		}

		protected override void OnPause ()
		{
			base.OnPause ();

			// Release the client
			if (agsClient != null) {
				agsClient.Dispose ();
				agsClient = null;
			}
		}

		public void OnServiceReady (AmazonGamesClient client)
		{
			agsClient = client;

			agsClient.LeaderboardsClient.GetLeaderboards ()
				.SetCallback (new AGResponseHandler<IGetLeaderboardsResponse> (response => {
					foreach (var leaderboard in response.Leaderboards) {
						Console.WriteLine ("Found Leaderboard: {0}", leaderboard.Name);
					}
				}));
		}

		public void OnServiceNotReady (AmazonGamesStatus status)
		{
			Console.WriteLine ("Unable to use AGS Service: " + status);
			Toast.MakeText (this, "Unable to use Amazon Games Service", ToastLength.Short).Show ();
		}
	}
}

