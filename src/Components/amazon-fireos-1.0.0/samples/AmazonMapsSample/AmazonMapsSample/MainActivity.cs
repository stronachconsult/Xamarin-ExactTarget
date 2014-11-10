using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Amazon.Geo.Maps;
using Android.Locations;

[assembly: UsesPermission (Android.Manifest.Permission.Internet)]
[assembly: UsesPermission (Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission (Android.Manifest.Permission.AccessFineLocation)]



namespace AmazonMapsSample
{
	[Activity (Label = "Amazon Maps Sample", MainLauncher = true)]
	public class MainActivity : MapActivity, ILocationListener
	{
		const int COFFEE_LATITUDE = 47624548;
		const int COFFEE_LONGITUDE = -122321006;
		const int COFFEE_ZOOM = 19;
		const int FINDME_ZOOM = 18;

		MapView mapview;
		ImageView imageFindMe;
		ImageView imageFindCoffee;

		MyLocationOverlay locationOverlay;
		CoffeeOverlay coffeeOverlay;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			mapview = FindViewById<MapView> (Resource.Id.mapview);
			imageFindMe = FindViewById<ImageView> (Resource.Id.find_me);
			imageFindCoffee = FindViewById<ImageView> (Resource.Id.find_coffee);

			mapview.SetBuiltInZoomControls (true);

			imageFindMe.Click += delegate {
				findMe ();
			};

			imageFindCoffee.Click += delegate {
				findCoffee ();
			};

			locationOverlay = new MyLocationOverlay (this, mapview);
			locationOverlay.EnableMyLocation ();
			mapview.Overlays.Add (locationOverlay);

			var coffeeMarker = Resources.GetDrawable (Resource.Drawable.coffeeshop);
			coffeeOverlay = new CoffeeOverlay (this, coffeeMarker);
			mapview.Overlays.Add (coffeeOverlay);

			findCoffee ();
		}

		void findCoffee () 
		{
			mapview.Controller.AnimateTo (new GeoPoint (COFFEE_LATITUDE, COFFEE_LONGITUDE));
			mapview.Controller.SetZoom (COFFEE_ZOOM);

			coffeeOverlay.Update (mapview.MapCenter, mapview.ZoomLevel);
		}

		void findMe ()
		{
			var lm = LocationManager.FromContext (this);
			var c = new Criteria ();
			c.Accuracy = Accuracy.Coarse;
			c.BearingRequired = false;
			c.CostAllowed = true;
			c.PowerRequirement = Power.NoRequirement;

			var provider = lm.GetBestProvider (c, true);
			var location = lm.GetLastKnownLocation (provider);

			if (location != null) {
				handleLocation (location);
				return;
			}


			lm.RequestLocationUpdates (provider, 100, 1, this);

			Toast.MakeText (this, "Finding location...", ToastLength.Short).Show ();
		}

		protected override bool IsRouteDisplayed {
			get { return false; }
		}

		public void OnLocationChanged (Location location)
		{
			var lm = LocationManager.FromContext (this);
			lm.RemoveUpdates (this);

			if (location == null) {
				Toast.MakeText (this, "Could not obtain location", ToastLength.Short).Show ();
				return;
			}

			handleLocation (location);
		}

		void handleLocation (Location location)
		{
			Toast.MakeText (this, "Found location!", ToastLength.Short).Show ();

			var point = new GeoPoint((int)Math.Round(location.Latitude * 1E6), (int)Math.Round(location.Longitude * 1E6));
			mapview.Controller.AnimateTo (point);
			mapview.Controller.SetZoom (FINDME_ZOOM);
		}

		public void OnProviderDisabled (string provider)
		{
			Console.WriteLine ("Provider Disabled: " + provider);
		}

		public void OnProviderEnabled (string provider)
		{
			Console.WriteLine ("Provider Enabled: " + provider);
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
			Console.WriteLine ("Provider Status Changed: " + provider + " (" + status + ")");
		}
	}
}


