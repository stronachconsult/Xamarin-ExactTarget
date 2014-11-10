using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Amazon.InApp.Purchasing;

namespace AmazonInAppPurchasingSample
{
	[Activity (Label = "Amazon InAppPurchasing Sample", MainLauncher = true)]
	public class MainActivity : Activity
	{
		IAPObserver observer;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			observer = new IAPObserver (this) {
				OnPurchase = updateOranges
			};

			PurchasingManager.RegisterObserver (observer);

			FindViewById<Button> (Resource.Id.buy_orange_button).Click += delegate {
				var requestId = PurchasingManager.InitiatePurchaseRequest ("orange");
				Console.WriteLine ("Started Request: " + requestId);
			};

			FindViewById<Button> (Resource.Id.eat_orange_button).Click += delegate {
				var sharedPref = GetSharedPreferences ("purchases", FileCreationMode.Private);
				var existingOranges = sharedPref.GetInt ("oranges", 0);
				var eaten = sharedPref.GetInt ("eaten", 0);


				// Remove the one we ate
				existingOranges--;

				if (existingOranges < 0)
					existingOranges = 0;

				//Increment our eaten counter
				eaten++;

				var editor = sharedPref.Edit ();
				editor.PutInt ("oranges", existingOranges);
				editor.PutInt ("eaten", eaten);
				editor.Commit ();

				Toast.MakeText (this, "Mmmm, what a tasty Orange!", ToastLength.Short).Show ();

				updateOranges ();
			};
		
			updateOranges ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			try {
				PurchasingManager.InitiateGetUserIdRequest ();
			} catch (Exception ex) {
				Console.WriteLine ("InitiateGetUserIdRequest failed: " + ex);
			}

		}

		void updateOranges()
		{
			var sharedPref = GetSharedPreferences ("purchases", FileCreationMode.Private);
			var existingOranges = sharedPref.GetInt ("oranges", 0);
			var eatenOranges = sharedPref.GetInt ("eaten", 0);

			FindViewById<TextView> (Resource.Id.num_oranges).Text = existingOranges.ToString ();
			FindViewById<TextView> (Resource.Id.num_oranges_consumed).Text = eatenOranges.ToString ();
		}

	}

	public class IAPObserver : PurchasingObserver 
	{
		public IAPObserver (Context context) : base (context)
		{
			Context = context;
		}

		public Context Context { get; private set; }

		public Action<string> OnGotUserId { get; set; }
		public Action OnPurchase { get; set; }

		public override void OnGetUserIdResponse (GetUserIdResponse response)
		{
			if (response.UserIdRequestStatus == GetUserIdResponse.GetUserIdRequestStatus.Successful) {

				if (OnGotUserId != null)
					OnGotUserId (response.UserId);

				// Get all our purchases
				PurchasingManager.InitiatePurchaseUpdatesRequest (Offset.Beginning);
			}
		}

		public override void OnItemDataResponse (ItemDataResponse response)
		{
			var status = response.GetItemDataRequestStatus ();

			if (status == ItemDataResponse.ItemDataRequestStatus.SuccessfulWithUnavailableSkus) {
				foreach (var s in response.UnavailableSkus) {
					// Unavailable SKU
					Console.WriteLine ("SKU Unavailable: " + s);
				}
			} else if (status == ItemDataResponse.ItemDataRequestStatus.Successful) {
				foreach (var key in response.ItemData.Keys) {
					var item = response.ItemData [key];
					Console.WriteLine ("Item: Price={0}, Desc={1}", item.Price, item.Description);
				}
			} else {
				// Fail gracefully
			}
		}

		public override void OnPurchaseResponse (PurchaseResponse response)
		{
			var status = response.GetPurchaseRequestStatus ();

			if (status == PurchaseResponse.PurchaseRequestStatus.Successful) {

				var sharedPref = Context.GetSharedPreferences ("purchases", FileCreationMode.Private);
				var existingOranges = sharedPref.GetInt ("oranges", 0);

				// Add the one we bought
				existingOranges++;

				var editor = sharedPref.Edit ();
				editor.PutInt ("oranges", existingOranges);
				editor.Commit ();

				if (OnPurchase != null)
					OnPurchase ();

				Toast.MakeText (Context, "You purchased an orange!", ToastLength.Short).Show ();
			} else {
				Toast.MakeText (Context, "Purchase failed, please try again", ToastLength.Short).Show ();
			}
		}

		public override void OnPurchaseUpdatesResponse (PurchaseUpdatesResponse response)
		{
			Console.WriteLine ("Purchase Udpates Response...");

			if (response.GetPurchaseUpdatesRequestStatus () 
				== PurchaseUpdatesResponse.PurchaseUpdatesRequestStatus.Successful) {

				// Check for revoked SKUs
				foreach (var sku in response.RevokedSkus) {
					// Revoke access to these SKUs
					Console.WriteLine ("Revoked SKU: " + sku);
				}

				// Process receipts
				foreach (var receipt in response.Receipts) {
					if (receipt.ItemType == Item.ItemType.Entitled) {
						// Re-entitle customer to this SKU
						Console.WriteLine ("Re-Entitle: " + receipt.Sku);
					} else if (receipt.ItemType == Item.ItemType.Subscription) {
						if (receipt.SubscriptionPeriod.EndDate == null) {
							// Grant access to the subscription
							Console.WriteLine ("Valid Subscription: " + receipt.Sku);
						}
					}
				}

				var newOffset = response.Offset;
				if (response.IsMore) {
					PurchasingManager.InitiatePurchaseUpdatesRequest (newOffset);

					// Persist the offset for future use
				}
			} else {
				// Provide the user access to any previously persisted entitlements
			}
		}

		public override void OnSdkAvailable (bool isSandboxMode)
		{
			Console.WriteLine ("OnSdkAvailable, isSandbox: " + isSandboxMode);
		}
	}
}


