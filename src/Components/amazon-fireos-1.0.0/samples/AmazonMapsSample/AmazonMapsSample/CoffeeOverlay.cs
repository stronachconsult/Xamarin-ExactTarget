using System;
using Android.Graphics.Drawables;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Geo.Maps;
using Android.Content;

namespace AmazonMapsSample
{
	public class CoffeeOverlay : Amazon.Geo.Maps.ItemizedOverlay
	{
		public Context Context { get; private set; }

		public CoffeeOverlay (Context context, Drawable defaultMarker) : base(defaultMarker)
		{
			Context = context;
			BoundCenter (defaultMarker);
		}

		List<CoffeeShop> shops = new List<CoffeeShop> ();

		protected override Java.Lang.Object CreateItem (int position)
		{
			Console.WriteLine ("CreateItem...");

			var shop = shops [position];

			return new OverlayItem (shop.Location, shop.Title, shop.Address);
			//return shops [position];
		}

		protected override bool OnTap (int position)
		{
			var shop = shops [position];

			CoffeeDetails.Display (Context, shop);

			return true;
		}

		public override int Size ()
		{
			Console.WriteLine ("Size...");

			return shops.Count;
		}

		public void Update (GeoPoint mapCenter, int zoomLevel)
		{
			shops.Clear ();

			var items = CoffeeFetcher.FetchCoffee (mapCenter, zoomLevel);

			shops.AddRange (items);

			//Tell the ui to update
			Populate ();
		}
	}
}

