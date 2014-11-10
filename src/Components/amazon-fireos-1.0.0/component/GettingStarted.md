## Amazon Developer Account & App Setup

If you do not already have an account, you should go to [Amazon Developer Console](https://developer.amazon.com/) and sign up.

Once your account is created and your are signed in, go to the [My Apps](https://developer.amazon.com/myapps.html) page and create a new Application.  Choose ***Android*** and click Next.  Enter a Title, and pick a Category.  When you are done, click Save and your app should be created.

## Create a Security Profile and Generate an API Key

Some of the API's require an ***APIKey*** to be generated.  API Key's are associated with a *Security Profile*.  Each app can be associated with a *Security Profile*.

1. In your app's dashboard on the Developer Console, go to the ***Security Profile*** section, and click the *Create New Security Profile* button.

2. On the next page, give your new profile a name and description, and Save it.

3. Next, click the ***View Security Profile*** button for the newly created profile.

4. Click the ***Android/Kindle Settings*** tab for the Security Profile.

5. Enter a **Name** for a new API Key that you will generate shortly.

6. Enter the **Package Name** of your app.  This package name ***must*** be exactly the same as the package name you setup in your Xamarin.Android app (in the *Project Settings -> Android Application -> Package Name* field)

7. Find your app's MD5 Signature and paste it into the **Signature** field.  You can find your MD5 Signature by following this guide: [Finding your Keystore's MD5 or SHA1 Signature](http://developer.xamarin.com/guides/android/deployment,_testing,_and_metrics/MD5_SHA1/).
8. Click ***Generate New Key***


Once your API Key has been generated, you will need to add it to your application's `AndroidManifest.xml` as metadata.  Copy the long **Key** string from the details page of the API Key you generated and add the following assembly attribute to your `AssemblyInfo.cs` file:

```csharp
[assembly: MetaData ("APIKey", Value="YOUR-API-KEY-HERE")]
```

NOTE: In Amazon's documentation, it states you can add your API Key to a file named api_key.txt in your assets folder, however we have found this does not currently work in Xamarin.Android, so please use the [assembly: MetaData (..)] attribute instead!


------------------------------------------------------------------


# Getting Started with Amazon Maps

### Developer Portal Maps Setup
Before you can use Maps in your app, you must setup your app on the Amazon Developer Portal and activate Maps functionality.

1. In the *My Apps* section of the  Amazon Developer Portal, navigate to the App you have created and click the *Maps* section.

2. Click the *Add Debug Registration* button.

3. Enter the **Package Name** of your app (this must be identical to the package name in your Xamarin.Android app's Project Settings).

4. Enter the MD5 Signature from your debug Keystore (you've already done this in the *Generate an API Key* section above) into the **Debug Signature** field.

5. Click *Submit*.


### Setup your Android Manifest
Next, you will need to add a few things to your `AndroidManifest.xml` file.

First, add the following namespace declaration attribute to the `<manifest ... >` tag:

```xml
<manifest xmlns:amazon="http://schemas.amazon.com/apk/res/android" ... >
```

Now, you can add the following tag inside of your `AndroidManifest.xml`'s `<application>...</application>` tags:

```xml
<application>
	<!-- ... -->
	
	<!-- Enable Amazon Maps on Amazon devices. -->
	<amazon:enable-feature android:name="com.amazon.geo.maps" android:required="false" />
	<!-- ... -->
</application>
```

### Adding Maps to your App
The Amazon Maps API is nearly identical to the Google Maps V1 API.  You can generally follow code samples for it and achieve the same results in Amazon Maps.

To add a map to your layout, use the following XML:

```xml
<com.amazon.geo.maps.MapView
        android:id="@+id/mapview"
        android:layout_width="fill_parent"
        android:layout_height="fill_parent"
        android:layout_weight="1.0"
        android:enabled="true"
        android:clickable="true" />
```

### Testing Maps
To Test maps on your device in Debug builds, you must be signed into the same Amazon Account on your Fire OS device that you used to register the app in the Developer Portal.  Go ahead and Make sure you are signed into your device with the same account now.











-------------------------------------------------------------------

# Getting Started with Amazon Device Messaging


Your app needs to declare and use some special permissions for Amazon Device Messaging to work.  You can simply copy the assembly attributes below, into your `AssemblyInfo.cs` file in your own app:

```csharp
[assembly: Permission(Name=Constants.PackageNameReceivePermission, ProtectionLevel=Android.Content.PM.Protection.Signature)]
[assembly: UsesPermission (Name=Constants.PackageNameReceivePermission)]
[assembly: UsesPermission (Name=Constants.ReceivePermission)]
[assembly: UsesPermission (Android.Manifest.Permission.WakeLock)]
[assembly: UsesPermission (Android.Manifest.Permission.Internet)]
[assembly: UsesFeature (Constants.AmazonDeviceMessagingFeature, Required = true)]
```

You'll have to also make some modifications to the `AndroidManifest.xml` file manually.

First, add the following namespace declaration attribute to the `<manifest ... >` tag:

```xml
<manifest xmlns:amazon="http://schemas.amazon.com/apk/res/android" ... >

```

You also need to declare that the app is using the specific device messaging feature.  To do so, you need to add the following tag inside of the `<application></application>` tags:

```
<application>
	<!-- ... -->
	
	<amazon:enable-feature android:name="com.amazon.device.messaging" 
						   android:required="false" />
						   
	<!-- ... -->
</application>
```


Next, you need to add a Service and Broadcast Receiver implementation.  The easiest way to do this is to copy and paste the following code which subclasses some existing helper classes into a new file in your own project:

```csharp
[BroadcastReceiver (Permission = Constants.SendPermission)]
[IntentFilter (new [] { Constants.ReceiveIntent, Constants.RegistrationIntent }, Categories = new [] { Constants.PackageName })]
public class MyAdmReceiver : ADMReceiver<MyAdmService>
{
}
	
[Service]
public class MyAdmService : ADMService
{
	[Preserve]
	public MyAdmService() : base()
	{
	}

	protected override void OnMessage (Intent intent)
	{
		Console.WriteLine ("ADMService.OnMessage...");

		// You will want to do something more intelligent here

		if (intent == null || intent.Extras == null)
			foreach (var key in intent.Extras.KeySet())
				Console.WriteLine("Key: {0}, Value: {1}", key, intent.GetStringExtra(key));
	}

	protected override void OnRegistered (string registrationId)
	{
		// You probably want to inform your web service of this
		Console.WriteLine ("ADMService.OnRegistered: {0}", registrationId);
	}

	protected override void OnRegistrationError (string errorId)
	{
		// Error
		Console.WriteLine ("ADMService.Error: {0}", errorId);
	}

	protected override void OnUnregistered (string registrationId)
	{
		Console.WriteLine ("ADMService.OnUnRegistered: {0}", registrationId);
	}
}
```

You will obviously need to add your own code to the overrides in `MyAdmService` to respond to Device Messaging events appropriately.

Finally, you need to register for ADM in your app.  Usually you would do this once in the `OnCreate` of your main activity, or somewhere else once in the lifecycle of your application:

```csharp
// Check if ADM exists on this device (only works on Amazon devices)
if (Constants.IsADMAvailable ()) {
	var adm = new Amazon.Device.Messaging.ADM (this);
	
	// If we aren't already register, register
	if (string.IsNullOrEmpty (adm.RegistrationId))
		adm.StartRegister ();
}
```












-------------------------------------------------------------------

# Getting Started with Amazon In-App Purchasing

### Updating the Android Manifest
Your app needs to receive broadcast intents from the Amazon Client via the ResponseReceiver class. You never use this class directly in your app. Instead, you simply include the following lines directly in the `<application></application>` tags in the `AndroidManifest.xml` file for your app:


```xml
<application>

	<!-- ... -->
	
	<receiver android:name = "com.amazon.inapp.purchasing.ResponseReceiver" >
		<intent-filter>
			<action android:name = "com.amazon.inapp.purchasing.NOTIFY"
		    	android:permission = "com.amazon.inapp.purchasing.Permission.NOTIFY" />
		</intent-filter>
	</receiver>
	
	<!-- ... -->

</application>
```

### Implement the Purchasing Observer
Once you have setup the ResponseReceiver to listen for broadcast intents from the Amazon Client, you need the ability to process the callbacks triggered from the ResponseReceiver. This is done with the `PurchasingObserver`. Your app can implement this by subclassing the `PurchasingObserver` abstract class.

```csharp
public class IAPObserver : PurchasingObserver 
{
	public IAPObserver (Activity iapActivity) : base (iapActivity)
	{
	}

	public override void OnGetUserIdResponse (GetUserIdResponse response)
	{
		// Got user response, ready for using the API
	}

	public override void OnItemDataResponse (ItemDataResponse response)
	{
		// Received response for request for item data
	}

	public override void OnPurchaseResponse (PurchaseResponse response)
	{
		// Received response for purchase
	}

	public override void OnPurchaseUpdatesResponse (PurchaseUpdatesResponse response)
	{
		// Received response about updates to purchases
	}

	public override void OnSdkAvailable (bool isSandboxMode)
	{
		// Received SDK available, ok to call some methods now
	}
}
```

### Register the Purchasing Observer
Next, register your PurchasingObserver with the PurchasingManager so that you can begin calling the In-App Purchasing API and receiving callbacks.  **This must be done in the `OnCreate(...)` method of your Main Activity.**

```csharp
var observer = new IAPObserver (this);
PurchasingManager.RegisterObserver (observer);
```

The Purchasing Observer callback matching the register call is `OnSDKAvailable(...)`. The callback returns a boolean that you can check to see whether the app is running in test mode against SDKTester or in the live production environment.


### Sync Access Rights for Current User

During your Main Activity’s `OnResume()` method, retrieve the user Id of the customer currently logged into the Amazon Appstore by calling :

```csharp
PurchasingManager.InitiateGetUserIdRequest();
```

The `PurchasingObserver.OnGetUserIdResponse` callback will be triggered in response to the `PurchasingManager.InitiateGetUserIdRequest()` call.

The first thing to do in the `PurchasingObserver.OnGetUserIdResponse` callback is to persist the user Id returned in the `GetUserIdResponse` object in memory.  This is the user Id of the customer currently logged into the Amazon Appstore.  As such, the user Id is unique to both the user and the app.

```csharp
public class IAPObserver : PurchasingObserver 
{
	// ...
 
	string currentUserID = null;
 
    public override void OnGetUserIdResponse (GetUserIdResponse response)
    {
        if (response.UserIdRequestStatus == 
        	GetUserIdResponse.GetUserIdRequestStatus.Successful) {
        	
			// Save the current user id
            currentUserID = response.UserId;
            
			// Just use Offset.Beginning for consumables only app
			PurchasingManager.InitiatePurchaseUpdatesRequest (Offset.Beginning);
        }
        else {
            // Fail gracefully.
        }
    }
    
	//...
}
```

Next, with the user returned, within the `PurchasingObserver.OnGetUserIdResponse()`, call `PurchasingManager.InitiatePurchaseUpdatesRequest()` with an offset value to retrieve the current state of receipts for your app’s entitlements and subscriptions. You must use this method to sync purchases made from other devices onto this device, and to sync revoked entitlements across all instances of your app.

**NOTE:** Although the corresponding `PurchasingService.OnPurchaseUpdatesResponse()` callback will not return any consumable information, you must make a call to `PurchasingManager.InitiatePurchaseUpdatesRequest()` even if your app only contains consumable items.  In this case, it is `OnPurchaseResponse()` which will be returned to your app if there are any pending consumable purchases when `PurchasingManager.InitiatePurchaseUpdatesRequest()` is called.

The offset represents a position in a set of paginated results. You can use the offset to get the next set of results. You can also pass in `Offset.Beginning` to get the entire list of results.  Offset values are base64 encoded values and not human readable.

It is best practice to only query the system for updates ie. to only retrieve new receipts generated since the last call to `PurchasingManager.InitiatePurchaseUpdatesRequest()`.  Use the user Id returned above in from the `OnGetUserId` Response to retrieve the persisted offset and pass this in into the `PurchasingManager.InitiatePurchaseUpdatesRequest()`. You can use `Offset.Beginning` when making a call to `PurchasingManager.InitiatePurchaseUpdatesRequest()` in the consumable case.

#### Consumables
No code needs to be added to the `PurchasingObserver.OnGetPurchaseUpdatesResponse()` callback if your app deals solely with consumables, since no receipt information will be sent back for Consumable purchases after their initial purchase.

#### Entitlements
When the `PurchasingObserver.OnGetPurchaseUpdatesResponse()` callback is triggered, process receipts returned by fulfilling and enabling all entitlements returned and process any revoked SKUs. First, check the request status returned in the `PurchaseUpdatesResponse.GetPurchaseUpdatesRequestStatus()`.
 
If the `requestStatus` is successful:

 - Retrieve all receipts and then fulfill/entitle them accordingly.
 - Only entitlements can be revoked. Call `PurchaseUpdatesResponse.GetRevokedSkus()` and remove entitlement to any SKUs returned in this call.

```csharp
public class IAPObserver : PurchasingObserver 
{
	// ...
	
 	public override void OnPurchaseUpdatesResponse (PurchaseUpdatesResponse response)
	{
		var requestStatus = response.GetPurchaseUpdatesRequestStatus ();
		
		if (requestStatus == 
			PurchaseUpdatesResponse.PurchaseUpdatesRequestStatus.Successful) {

			// Check for revoked SKUs
			foreach (var sku in response.RevokedSkus) {
				// Revoke access to these SKUs
			}

			// Process receipts
			foreach (var receipt in response.Receipts) {
				if (receipt.ItemType == Item.ItemType.Entitled) {
					// Re-entitle customer to this SKU
				}
			}
		} else {
			// Provide the user access to any previously persisted entitlements
		}
	}
	
	// ...
}
```


#### Subscriptions

When the `PurchasingObserver.OnGetPurchaseUpdatesResponse()` callback is triggered, process receipts returned by fulfilling and enabling all subscriptions returned. First, check the request status returned in the `PurchaseUpdatesResponse.GetPurchaseUpdatesRequestStatus()`.
 
If the `requestStatus` is successful:

 - Retrieve all receipts and then fulfill/entitle them accordingly. Active subscriptions are denoted by a null end date in the receipt. Expired subscriptions are determined by a non-null end date on the receipt. It is your app's responsibility to manage expired subscriptions.

```csharp
public class IAPObserver : PurchasingObserver 
{
	// ...
	
 	public override void OnPurchaseUpdatesResponse (PurchaseUpdatesResponse response)
	{
		if (response.GetPurchaseUpdatesRequestStatus () 
			== PurchaseUpdatesResponse.PurchaseUpdatesRequestStatus.Successful) {

			// Check for revoked SKUs
			foreach (var sku in response.RevokedSkus) {
				// Revoke access to these SKUs
			}

			// Process receipts
			foreach (var receipt in response.Receipts) {
				if (receipt.ItemType == Item.ItemType.Subscription) {
					
					// Check subscription period for validity
					if (receipt.SubscriptionPeriod.EndDate == null) {
						// Grant access to the subscription
					}
				}
			}
		} else {
			// Provide the user access to any previously persisted subscriptions
		}
	}
	
	// ...
}
```


#### Handling the Offset

In both the entitlement and subscription case, when persisting the offset and returned receipts, they should be associated with the current user Id. This is to handle the case that multiple customers share the same physical device. The user Id returned in `GetPurchaseUpdatesResponse` can be used to for storing the receipt data and offset.

If `PurchaseUpdatesResponse.IsMore()` returns `true`, make a recursive call to `PurchasingManager.InitiatePurchaseUpdatesRequest()` with the value returned in `PurchaseUpdatesResponse.GetOffset()`.

```csharp
public class IAPObserver : PurchasingObserver 
{
	// ...
	
 	public override void OnPurchaseUpdatesResponse (PurchaseUpdatesResponse response)
	{
		if (response.GetPurchaseUpdatesRequestStatus () 
			== PurchaseUpdatesResponse.PurchaseUpdatesRequestStatus.Successful) {

			var newOffset = response.Offset;
						
			if (response.IsMore) {
				PurchasingManager.InitiatePurchaseUpdatesRequest (newOffset);

				// Persist the offset for future use
			}

		} else {
			// Provide the user access to any previously persisted entitlements
		}
	}
	
	// ...
}
```

#### Validate SKUs to be Used in Your App
Calling `PurchasingManager.InitiateItemDataRequest()` allows you to retrieve the most up-to-date and localized price, title and description information for each of your SKUs. This must be done for all of the SKUs you are offering for sale within your app. Proper SKU validation will also prevent an invalid SKU status from being returned in the `PurchasingObserver.OnPurchaseResponse()`.

```csharp
public class MainActivity : Activity {
    
    //  ...
    
    protected override void OnResume() {
        // ...
		var skuSet = new HashSet<String>();
        skuSet.Add("com.amazon.example.iap.consumable");
        skuSet.Add("com.amazon.example.iap.entitlement");
        skuSet.Add("com.amazon.example.iap.subscription");
 
        PurchasingManager.InitiateItemDataRequest (skuSet);
    }
    
    // ...
}
```

The `PurchasingObserver.OnItemDataResponse()` callback is triggered after the `PurchasingManager.IntiateItemDataRequest()` method has been called. You should check the request status returned in the `PurchaseUpdatesResponse.GetItemDataRequestStatus()` and only offer for sale purchasable items or SKUs validated by this call.
 
If the `status` is successful, retrieve the item data map (item type, icon url, localized price, title and description) keyed on sku for display in the app.
 
If the `status` is successful with unavailable SKUs, this indicates that the request was successful but item data for one or more of the provided SKUs was not available. Developers should retrieve the item data for each available sku for display in the app. Access to purchase should be degraded gracefully for any SKUs returned in `PurchaseUpdatesResponse.GetUnavailableSkus()`.
 
If the `status` is failed, you should disable IAP functionality in your app.


```csharp
public class IAPObserver : PurchasingObserver 
{
	// ...
	
	public override void OnItemDataResponse (ItemDataResponse response)
	{
		var status = response.GetItemDataRequestStatus ();

		if (status == ItemDataResponse.ItemDataRequestStatus.SuccessfulWithUnavailableSkus) {
			foreach (var s in response.UnavailableSkus) {
				// Unavailable SKU: s
			}
		} else if (status == ItemDataResponse.ItemDataRequestStatus.Successful) {
			foreach (var key in response.ItemData.Keys) {
				var item = response.ItemData [key];
				// Item is available
			}
		} else {
			// Fail gracefully
		}
	}
	
	// ...
}
```





### Initiating In-App Purchase

You can now make a call to the In-App Purchasing API to initiate a purchase for a specific SKU.

If an item is purchased in your app, record the `requestId` from the `PurchaseResponse` object. From `OnPurchaseResponse()`, save the `requestId` found in the PurchaseResponse object to ensure that your app does not grant an item multiple times. Save the `RequestId` to a server or the device’s local storage.

If the item corresponding to the `requestId` has already been granted, the app does not need to take any action. Allowing the SDK to continue processing will remove the `Receipt` so that the same `Receipt` and `requestId` will not be sent to the app again.

Before your app grants an item to the customer, your app must verify that the item corresponding to the `requestId` has not already been granted.

Note: The SKU used directly in the code here for simplicity but it is a best practice to store SKUs in resource file (e.g strings.xml) or to pull them from a server.

```csharp
var requestId = PurchasingManager.InitiatePurchaseRequest ("com.amazon.example.iap.consumable");
```

The matching `OnPurchaseResponse` callback contains all the information you need to verify the purchase.  **It is important that you make sure your code can handle the `OnPurchaseResponse` callback at any time while your app is running.**

Purchase request status is has four possible values:

 - **Successful** - Indicating that the purchase was successfully completed.
 - **Failed** - Indicating that the purchase failed.
 - **InvalidSku** - Indicating that the SKU originally provided to the `PurchasingManager.InitiatePurchaseRequest(string)` method is not valid.
 - **AlreadyEntitled** - Indicating that the customer already owns the provided SKU.

If the Purchase request status is **Successful** then a receipt will be returned in the `PurchaseResponse`.

Every receipt will contain the item type, sku, subscription period, if the item type is a subscription and a PurchaseToken that can be used to validate a purchase via the Receipt Verification Service. The receipt is secure, and you can safely rely solely on it to authorize access to content or functionality within your app.

The `PurchaseToken` is dynamically generated each time receipt data is returned and is not a unique order identifier. They are unique values for out-of-app verification of the receipt with RVS.

Receipt data is returned in the `PurchaseRsponse` object and will contain a purchase token. The PurchaseToken of a Receipt returned in a PurchaseResponse will be different than the PurchaseToken of a Receipt for the same user and sku returned in a PurchaseUpdatesResponse Regardless of the difference, each PurchaseToken will successfully validate against the Receipt Verification Service at any given time.

Receipt processing in the successful `OnPurchaseResponse(...)` case differs between consumables, entitlements and subscriptions.


#### Purchasing Consumables
In the consumable case, **this is the only time you will see a receipt**. You therefore must persist the relevant receipt data to allow you to provide the user with access to the consumable that they have purchased. Remember, consumable purchasable items are only valid for the user on the device that it was purchased on.

```csharp
public class IAPObserver : PurchasingObserver 
{
	// ...
	
	public override void OnPurchaseResponse (PurchaseResponse response)
	{
		var status = response.GetPurchaseRequestStatus ();
		
		if (status == PurchaseResponse.PurchaseRequestStatus.Successful) {		

			var receipt = response.Receipt;
			
			Console.WriteLine ("{0}: {1}, {2}", 
				receipt.Sku, receipt.ItemType, receipt.PurchaseToken);

			// Store receipt and enable access to consumable			
		}
	}
	
	// ...
}
```


#### Purchasing Entitlements

For entitlements, the receipt parsing code is the same as for consumables.



#### Purchasing Subscriptions

For subscriptions, an additional parameter will be returned inside the `Receipt` object - the `SubscriptionPeriod`. The `SubscriptionPeriod` object contains a start date and an end date representing the period of time during which a subscription is valid. If a subscription is valid, it will have a null end date. If the subscription has expired, the end date will contain the date that the subscription is no longer valid.


```csharp
public class IAPObserver : PurchasingObserver 
{
	// ...
	
	public override void OnPurchaseResponse (PurchaseResponse response)
	{
		var status = response.GetPurchaseRequestStatus ();
		
		if (status == PurchaseResponse.PurchaseRequestStatus.Successful) {		

			var receipt = response.Receipt;		
			var subPeriod = receipt.SubscriptionPeriod;
			
			Console.WriteLine ("{0}: {1}, {2}", 
				receipt.Sku, receipt.ItemType, receipt.PurchaseToken);

			if (subPeriod.EndDate == null) {		
				// Entitle subscription
			}
		}
	}
	
	// ...
}
```




### Testing your App with SDK Tester
It's important to note that there are some specific steps and setup you must go through in order to be able to test debug builds of your app with the In-App Purchasing API.

You can find the full guide to using the SDK Tester here: [Testing In-App Purchases](https://developer.amazon.com/public/apis/earn/in-app-purchasing/docs/testing-iap).

**NOTE**: You will need to download the full Mobile SDK from Amazon to get the SDKTester.apk tool.  You can find that SDK here: [Amazon Mobile SDK](https://developer.amazon.com/public/resources/development-tools/sdk).

### Receipt Validation
It's possible to validate your receipts.  To do this, you would use RVS (Receipt Verification Service).  There's a full guide on how to implement RVS and how to test it here: [Receipt Verification Service Implementation and Testing](https://developer.amazon.com/public/apis/earn/in-app-purchasing/docs/rvs)











-------------------------------------------------------------------

# Getting Started with Amazon GameCircle


First, you should follow the [Get Set Up for GameCircle](https://developer.amazon.com/public/apis/engage/gamecircle/docs/get-set-up-for-gamecircle).  This will help you with configuring your GameCircle game in the Developer Console (defining achievements, leaderboards, and test accounts).

When you have completed the guide, continue below.

## Initializing GameCircle on Android / Fire OS

You will need to add a couple permissions to your app.  You can do this by editing the `AndroidManifest.xml` file or by including the following assembly level attributes in you C# code:

```csharp
[assembly: UsesPermission (Android.Manifest.Permission.Internet)]
[assembly: UsesPermission (Android.Manifest.Permission.AccessNetworkState)]
```

In order to fully leverage built in GameCircle UI's, we need to add some information manually to our `AndroidManifest.xml` to register Amazon's activities and broadcast receivers so the SDK can work fully.

For targeting Android API Levels 11 and above, add this to your `AndroidManifest.xml` file, inside the `<application>...</application>` tags (**NOTE**: You must replace the `YOUR_PACKAGE_NAME_HERE` in the snippet below with your own Package Name):

```xml
<activity android:name="com.amazon.ags.html5.overlay.GameCircleUserInterface"
android:theme="@style/GCOverlay" android:hardwareAccelerated="false"></activity>
<activity
  android:name="com.amazon.identity.auth.device.authorization.AuthorizationActivity"
  android:theme="@android:style/Theme.NoDisplay"
  android:allowTaskReparenting="true"
  android:launchMode="singleTask">
  <intent-filter>
     <action android:name="android.intent.action.VIEW" />
     <category android:name="android.intent.category.DEFAULT" />
     <category android:name="android.intent.category.BROWSABLE" />
     <data android:host="YOUR_PACKAGE_NAME_HERE" android:scheme="amzn" />
  </intent-filter>
</activity>
<activity android:name="com.amazon.ags.html5.overlay.GameCircleAlertUserInterface"
android:theme="@style/GCAlert" android:hardwareAccelerated="false"></activity>
<receiver
  android:name="com.amazon.identity.auth.device.authorization.PackageIntentReceiver"
  android:enabled="true">
  <intent-filter>
     <action android:name="android.intent.action.PACKAGE_INSTALL" />
     <action android:name="android.intent.action.PACKAGE_ADDED" />
     <data android:scheme="package" />
  </intent-filter>
</receiver>
```

For targeting Android API Levels 10 and below add this *instead*.  The differences are to do with hardware acceleration differences between the API levels (**NOTE**: You must replace the `YOUR_PACKAGE_NAME_HERE` in the snippet below with your own Package Name):
```xml
<activity android:name="com.amazon.ags.html5.overlay.GameCircleUserInterface"
android:theme="@style/GCOverlay"></activity>
<activity
  android:name="com.amazon.identity.auth.device.authorization.AuthorizationActivity"
  android:theme="@android:style/Theme.NoDisplay"
  android:allowTaskReparenting="true"
  android:launchMode="singleTask">
  <intent-filter>
     <action android:name="android.intent.action.VIEW" />
     <category android:name="android.intent.category.DEFAULT" />
     <category android:name="android.intent.category.BROWSABLE" />
     <data android:host="YOUR_PACKAGE_NAME_HERE" android:scheme="amzn" />
  </intent-filter>
</activity>
<activity android:name="com.amazon.ags.html5.overlay.GameCircleAlertUserInterface"
android:theme="@style/GCAlert"></activity>
<receiver
  android:name="com.amazon.identity.auth.device.authorization.PackageIntentReceiver"
  android:enabled="true">
  <intent-filter>
     <action android:name="android.intent.action.PACKAGE_INSTALL" />
     <action android:name="android.intent.action.PACKAGE_ADDED" />
     <data android:scheme="package" />
  </intent-filter>
</receiver>
```

### Initialize and Dispose the GameCircle client

```csharp
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

	// Dispose this instance of the client when we are done with it
	// to prevent measuring the customer's time played while in the background
	if (agsClient != null) {
		agsClient.Dispose ();
		agsClient = null;
	}
}

protected override void OnDestroy ()
{
	base.OnDestroy ();

	// Shutdown the client when we are completely done with it
	AmazonGamesClient.Shutdown ();
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
```


## Implementing Whispersync

With Whispersync, you get and set values in a data map. It is the first solution to offer both auto-conflict resolution and player-choice conflict resolution as options, and it queues when a device is offline. You can set up the data map in just a few minutes.

The GameCircle SDK incorporates the Login with Amazon service to manage Whispersync authentication.

### How Whispersync for Games Works

Prior to Whispersync for Games, many developers implemented separate methods to store game data to disk and to cloud. Whispersync replaces your local storage solution and provides the added benefit of background synchronization with the cloud and Android and iOS devices.

With the `GameDataMap` interface, you get and set numbers and strings, and organize them in lists and maps. `GameDataMap` is a first-class citizen that you can treat as a variable, pass into a method, or return from a method.

Here’s how you get your game data:

```csharp
var gameDataMap = AmazonGamesClient.WhispersyncClient.GameData;
```

Unlike the GameCircle leaderboard and achievements clients, the Whispersync client is available immediately after initializing `AmazonGamesClient`. Whispersync data is always accessible, even when the player is not registered with Amazon or is offline.

`GameDataMap` provides several ways to access your data. For example, to retrieve a player’s highest score:

```csharp
var highScore = gameDataMap.GetHighestNumber ("highScore");
```

In this example, because you retrieve highScore as a highest number, it will always reflect the maximum value ever assigned to it, from any device.

To set the high score:

```csharp
// Where 1000 represents a player's score, not a maximum
highScore.Set(1000);
```

### Conflict Resolution Options

Amazon offers two conflict resolution options to provide the best possible player experience for each game:

 - **Auto resolution**, in which your game automatically syncs the best score (highest or lowest, depending on the game), most recent achievements, and most recent purchases across all devices and to the Amazon cloud, without further action on both the player and the developer’s part. To implement auto-conflict resolution, use any of the Whispersync data type described below, except for DeveloperString (which is used exclusively for manual conflict resolution).

 - **Manual resolution via DeveloperString**, in which the developer manually performs the conflict resolution. You should first get and deserialize both locally and remotely stored strings, and then set specific values via game logic to determine the current game state. If there is no easy way to auto-resolve, you can optionally prompt the player for input.
 
For implementation details of manual resolution via DeveloperString, see Example 4 below.

**IMPORTANT:** Amazon recommends that you exercise caution with manual resolution via DeveloperString. Your game logic may differ from players’ expectations. For example, if a player buys an item for 100 Coins on offline Device A, and later buys a different item with the same 100 Coins on offline Device B, the player may want to keep the item from Device A, which represents the older data. In this case, it may be best to involve the player via a popup to let them decide which item they would rather keep. Alternatively, you may choose to model the above situation with auto-resolvable types such as `SyncableAccumulatingNumber` and `SyncableStringSet`.


You should read the guide: [Implementing Whispersync for Games in your Android or Fire OS game](https://developer.amazon.com/public/apis/engage/gamecircle/docs/whispersync) for more information.




## Implementing Achievements

### Send an Achievement Update

After initializing Amazon GameCircle in your game, you can submit achievement progress updates to the service.

The second input parameter to the `UpdateProgress' method is a floating-point value representing the player’s completion percentage for the achievement.  The valid range of this parameter is `0.0` to `100.0`, where `0.0` represents ***fully locked*** and `100.0` represents ***fully unlocked***.  Completion percentage can only increase. If the method submits a completion percentage value that is lower than the current value, the completion percentage shown for that achievement will not change. 

```csharp
// Replace YOUR_ACHIEVEMENT_ID with an actual achievement ID from your game.
var acClient = agsClient.AchievementsClient;

acClient.UpdateProgress (YOUR_ACHIEVEMENT_ID, 100.0f);
```


### Add a Link to the Achievement Overlay in Your Game

To help customers find their achievements, make sure to add a link somewhere in your game to the in-game overlay.

```csharp
Button btnAchievementsOverlay;

// ...

btnOpenAchievementsOverlay.Click += delegate {
	agsClient.AchievementsClient.ShowAchievementsOverlay ();
};
```

### Get a List of Achievements
The achievements client can generate a list of the player's achievements in your game.  Notice that the `AGResponseHandler<T>` is used to register for a callback.  This pattern is used commonly in the GameCircle clients.

```csharp
agsClient.AchievementsClient.GetAchievements ()
	.SetCallback (new AGResponseHandler<IGetAchievementssResponse> (response => {
		foreach (var achievement in response.Achievements) {
			Console.WriteLine ("Found Achievement: {0}", achievement.Title);
		}
	}));
```



## Implementing Leaderboards

### Submitting a score to a Leaderboard
After initializing Amazon GameCircle in your game, you can submit scores to the service. Scores must be positive numbers.

```csharp
agsClient.LeaderboardsClient.SubmitScore (YOUR_LEADERBOARD_ID, longScoreValue);
```

### Add a Link to the Leaderboards Overlay in Your Game
To help customers find the leaderboard, make sure to add a link to the in-game overlay somewhere in your game.

```csharp
Button btnLeaderboardsOverlay;

// ...

btnOpenLeaderboardsOverlay.Click += delegate {
	agsClient.LeaderboardsClient.ShowLeaderboardsOverlay ();
};
```


### Get a List of Leaderboards
The leaderboards client can generate a list of leaderboards in your game.  Notice that the `AGResponseHandler<T>` is used to register for a callback.  This pattern is used commonly in the GameCircle clients.

```csharp
agsClient.LeaderboardsClient.GetLeaderboards ()
	.SetCallback (new AGResponseHandler<IGetLeaderboardsResponse> (response => {
		foreach (var leaderboard in response.Leaderboards) {
			Console.WriteLine ("Found Leaderboard: {0}", leaderboard.Name);
		}
	}));
```



