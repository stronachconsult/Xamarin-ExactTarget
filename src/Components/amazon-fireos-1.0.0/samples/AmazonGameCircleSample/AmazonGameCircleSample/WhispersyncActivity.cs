
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
using System.Threading.Tasks;
using Amazon.Ags.Api.Whispersync;

namespace AmazonGameCircleSample
{
	[Activity (Label = "Whispersync Demo")]			
	public class WhispersyncActivity : Activity, IAmazonGamesCallback
	{
		AmazonGamesClient agsClient;

		TextView currentMood;
		EditText newMood;
		Button updateMood;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.whispersync_activity);

			currentMood = FindViewById<TextView> (Resource.Id.current_mood);
			newMood = FindViewById<EditText> (Resource.Id.new_mood);
			updateMood = FindViewById<Button> (Resource.Id.update_mood);

			updateMood.Click += delegate {
				if (string.IsNullOrEmpty (newMood.Text)) {
					Toast.MakeText (this, "Type your mood in first...", ToastLength.Short).Show ();
					return;
				}

				AmazonGamesClient.WhispersyncClient.GameData.GetLatestString ("mood")
					.Set (newMood.Text);

				newMood.Text = string.Empty;

				Toast.MakeText (this, "Updated mood... Syncing should happen soon!", ToastLength.Short).Show ();

				fetchMood ();
			};

			var features = Java.Util.EnumSet.Of (AmazonGamesFeature.Whispersync);

			AmazonGamesClient.Initialize (this, this, features);
			AmazonGamesClient.WhispersyncClient.SetWhispersyncEventListener (new WhispersyncListener {
				DataChangedAction = fetchMood
			});
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			fetchMood ();
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

		}

		public void OnServiceNotReady (AmazonGamesStatus status)
		{
			Console.WriteLine ("Unable to use AGS Service: " + status);
			Toast.MakeText (this, "Unable to use Amazon Games Service", ToastLength.Short).Show ();
		}

		void fetchMood () 
		{
			Task.Factory.StartNew (() => {

				var gameData = AmazonGamesClient.WhispersyncClient.GameData;

				var latestMood = gameData.GetLatestString ("mood");

				RunOnUiThread(() => currentMood.Text = latestMood.Value);
			});
		}
	}

	public class WhispersyncListener : WhispersyncEventListener
	{
		public Action DataChangedAction { get;set; }

		public override void OnNewCloudData ()
		{
			base.OnNewCloudData ();

			Console.WriteLine ("NewCloudDate...");

			if (DataChangedAction != null)
				DataChangedAction ();
		}

		public override void OnDataUploadedToCloud ()
		{
			base.OnDataUploadedToCloud ();

			Console.WriteLine ("DataUploadedToCloud...");

			if (DataChangedAction != null)
				DataChangedAction ();
		}
	}
}

