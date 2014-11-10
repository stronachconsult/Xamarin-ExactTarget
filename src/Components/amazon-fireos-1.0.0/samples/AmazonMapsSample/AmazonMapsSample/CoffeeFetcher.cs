using System;
using System.Collections.Generic;
using Amazon.Geo.Maps;

namespace AmazonMapsSample
{
	public class CoffeeFetcher
	{
		public CoffeeFetcher ()
		{
		}

		static List<CoffeeShop> Shops = new List<CoffeeShop> {
			new CoffeeShop {
				Title = "Joe Bar Cafe",
				Address = "810 East Roy Street",
				Phone = "(206) 324-0407",
				Location = new GeoPoint(47625148, -122321623)
			},
			new CoffeeShop {
				Title = "Roy Street Coffee and Tea",
				Address = "700 Broadway Ave East",
				Phone = "(206) 325-2211",
				Location = new GeoPoint(47625206, -122321108)
			},
			new CoffeeShop {
				Title = "Vivace Espresso Bar",
				Address = "532 Broadway Ave East",
				Phone = "(206) 860-2722",
				Location = new GeoPoint(47623749, -122320861)
			},
			new CoffeeShop {
				Title = "Dilettante Mocha Cafe",
				Address = "538 Broadway Ave East",
				Phone = "(206) 329-6463",
				Location = new GeoPoint(47623922, -122320861)
			},
			new CoffeeShop {
				Title = "Starbucks",
				Address = "700 Broadway Ave East",
				Phone = "(206) 325-2211",
				Location = new GeoPoint(47625495, -122321287)
			},
			new CoffeeShop {
				Title = "Starbucks",
				Address = "434 Broadway Ave East",
				Phone = "(206) 323-7888",
				Location = new GeoPoint(47623058, -122320792)
			},
			new CoffeeShop {
				Title = "Cafe Canape",
				Address = "700 Broadway Ave East",
				Phone = "(206) 708-1210",
				Location = new GeoPoint(47625401,-122320791)
			},
		};

		public static List<CoffeeShop> FetchCoffee (GeoPoint mapCenter, int zoomLevel)
		{
			// You would probably want to substitute your own web service in here...
			return Shops;
		}
	}
}

