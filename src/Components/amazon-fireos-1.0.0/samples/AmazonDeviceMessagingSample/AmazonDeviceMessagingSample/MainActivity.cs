using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Amazon.Device.Messaging;

[assembly: Permission(Name=Constants.PackageNameReceivePermission, ProtectionLevel=Android.Content.PM.Protection.Signature)]
[assembly: UsesPermission (Name=Constants.PackageNameReceivePermission)]
[assembly: UsesPermission (Name=Constants.ReceivePermission)]
[assembly: UsesPermission (Android.Manifest.Permission.WakeLock)]
[assembly: UsesPermission (Android.Manifest.Permission.Internet)]
[assembly: UsesFeature (Constants.AmazonDeviceMessagingFeature, Required = true)]

//IMPORTANT: Replace this Value with your own!!!
[assembly: MetaData ("APIKey", Value = "YOUR-KEY-HERE")]

namespace AmazonDeviceMessagingSample
{
	[Activity (Label = "Amazon DeviceMessaging Sample", MainLauncher = true)]
	public class MainActivity : Activity
	{
		Button myButton;
		TextView textStatus;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			MyAdmService.Registered += registrationId => RunOnUiThread (() => {
				myButton.Enabled = true;
				textStatus.Text = "Registered! \n" + registrationId;
			});
			MyAdmService.Unregistered += registrationId => RunOnUiThread (() => {
				myButton.Enabled = false;
				textStatus.Text = "Unregistered!";
			});
			MyAdmService.Error += (errorId, errorDesc) => RunOnUiThread (() => {
				textStatus.Text = errorId + "\n" + errorDesc;
			});

			textStatus = FindViewById<TextView> (Resource.Id.textViewStatus);
			myButton = FindViewById<Button> (Resource.Id.myButton);
			myButton.Enabled = false;
			myButton.Click += delegate {

				// Optionally, unregister
				if (Constants.IsADMAvailable ()) {
					var adm = new Amazon.Device.Messaging.ADM (this);
					adm.StartUnregister ();
				}

			};

			// Initiate the registration once in our app
			if (Constants.IsADMAvailable ()) {
				var adm = new Amazon.Device.Messaging.ADM (this);
				adm.StartRegister ();
			}
		}
	}

	[BroadcastReceiver (Permission = Constants.SendPermission)]
	[IntentFilter (new [] { Constants.ReceiveIntent, Constants.RegistrationIntent }, Categories = new [] { Constants.PackageName })]
	public class MyAdmReceiver : ADMReceiver<MyAdmService>
	{
	}
		
	[Service]
	public class MyAdmService : ADMService
	{
		public static event Action<string> Registered;
		public static event Action<string> Unregistered;
		public static event Action<string, string> Error;

		[Preserve]
		public MyAdmService() : base()
		{
		}

		protected override void OnMessage (Intent intent)
		{
			Console.WriteLine ("ADMService.OnMessage...");

			// You will want to do something more intelligent here
			Toast.MakeText (this, "Received Notification!", ToastLength.Short).Show ();

			if (intent == null || intent.Extras == null)
				foreach (var key in intent.Extras.KeySet())
					Console.WriteLine("Key: {0}, Value: {1}", key, intent.GetStringExtra(key));
		}

		protected override void OnRegistered (string registrationId)
		{
			// You probably want to inform your web service of this
			Console.WriteLine ("ADMService.OnRegistered: {0}", registrationId);

			if (Registered != null)
				Registered (registrationId);
		}

		protected override void OnRegistrationError (string errorId, string errorDescription)
		{
			// Error
			Console.WriteLine ("ADMService.Error: {0} ({1})", errorId, errorDescription);

			if (Error != null)
				Error (errorId, errorDescription);
		}

		protected override void OnUnregistered (string registrationId)
		{
			Console.WriteLine ("ADMService.OnUnRegistered: {0}", registrationId);

			if (Unregistered != null)
				Unregistered (registrationId);
		}
	}
}
