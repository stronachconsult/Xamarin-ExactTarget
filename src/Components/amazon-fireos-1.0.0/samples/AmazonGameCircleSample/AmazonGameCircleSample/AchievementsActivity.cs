
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
using Amazon.Ags.Api.Achievements;

namespace AmazonGameCircleSample
{
	[Activity (Label = "Achievements")]			
	public class AchievementsActivity : Activity, IAmazonGamesCallback
	{
		AmazonGamesClient agsClient;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.achievements_activity);

			FindViewById<Button> (Resource.Id.buttonShowAchievements).Click += delegate {
				agsClient.AchievementsClient.ShowAchievementsOverlay ();
			};

			FindViewById<Button> (Resource.Id.buttonUnlock).Click += delegate {

				// We only need to run the sample once to unlock the achievement
				agsClient.AchievementsClient.UpdateProgress ("runsample", 100.0f);
			};
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			var features = Java.Util.EnumSet.Of (AmazonGamesFeature.Achievements);
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

			// Fetch all our achievements
			agsClient.AchievementsClient.GetAchievements ()
				.SetCallback (new AGResponseHandler<IGetAchievementsResponse> (response => {

					foreach (var ach in response.AchievementsList) {
						Console.WriteLine ("Found Achievment: {0}", ach.Title);
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

