using System;
using Amazon.Geo.Maps;

namespace AmazonMapsSample
{
	public class CoffeeShop : Java.Lang.Object
	{
		public CoffeeShop ()
		{
		}

		public string Title { get;set; }
		public string Address { get;set; }
		public string Phone { get;set; }
		public GeoPoint Location { get;set; }
		public OverlayItem OverlayItem { 
			get {
				return new OverlayItem (Location, Title, null);
			}
		}
	}
}

