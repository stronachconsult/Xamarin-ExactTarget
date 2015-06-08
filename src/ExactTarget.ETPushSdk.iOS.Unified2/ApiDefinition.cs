using System;
using Foundation;
using CoreLocation;
using UIKit;
using ObjCRuntime;

namespace ExactTarget.ETPushSdk
{
	/// <summary>
	/// This is an adaptor that allows multiple ETGenericUpdate objects of the same class to be uploaded together. It overrides some of the ETGenericUpdate base methods in favor of a different implementation, and generates a payload differently than a single update would. 
	/// 
	/// This only works if the ETGenericUpdate follows the convention of havint the tablename (as returned by tableName) match the key in the array payload. For example, the ETEvents tableName is "events", and the payload format is {"events":[...]}.
	/// 
	/// If everything is working correctly, you can use ETPhoneHome phoneHomeInBulkForGenericUpdateType: and it will take care of the rest for you, which includes marking objects as 'claimed' for sending and deleting them on success. Check the doc on that method (and it's source) for more specifics. There are some extra assumptions on bulk-updatable objects (like the DB being updated to have a claimed column), and they're outlined there.
	/// </summary>
	[BaseType(typeof(ETGenericUpdate), Delegates = new[] { "WeakDelegate" }, Events = new[] { typeof(ETGenericUpdateObjectProtocol) })]
	[DisableDefaultCtor]
	public partial interface ETBulkUpdateShim
	{
		/// <summary>
		/// Designated Initializer. This generates an ETBulkUpdateShim object suitable for passing to ETPhoneHome phoneHome:.
		/// </summary>
		/// <param name="updateClass">The type of object you want to send, not an instance of it ([ETEvent class]).</param>
		/// <param name="realObjects">An array of NSDictionaries (serialized ETGenericUpdate objects) that you want to send in.</param>
		/// <param name="success">What to do if the update worked.</param>
		/// <param name="failure">What to do if the update failed.</param>
		/// <returns>id A very special ETGenericUpdate object that can bulk send data.</returns>
		[Export("initForGenericUpdateClass:andObjects:withSuccessBlock:andFailureBlock:")]
		IntPtr Constructor(Class updateClass, NSArray realObjects, NSObject success, NSObject failure);

		/// <summary>
		/// A class pointer to the ETGenericUpdate object you wish to bulk send. Must implement the ETGenericUpdateObjectProtocol protocol.
		/// </summary>
		/// <value>
		/// The update class.
		/// </value>
		[Export("updateClass")]
		Class UpdateClass { get; }

		/// <summary>
		/// An array of NSDictionary versions of the things you want to send. This should be equivalent to what would be serailzed to send back.
		/// </summary>
		/// <value>
		/// The real objects.
		/// </value>
		[Export("realObjects")]
		NSObject[] RealObjects { get; }
	}

	/// <summary>
	/// ETEvent is the new form of analytics for the MobilePush SDK, replacing ETStatsUpdate. It is more flexible in that it can return multiple types of events in one pass. As such, things that are related to each other should be sent back together, and things that are not should be sent separately. It is also unique amongst the ETGenericUpdates in that it was designed to be bulk-sent back to ET, and not individually like the rest. 
	///
	/// An ETEvent is comprised of some combination of the following: a Value, or the amount of units that we are currently measuring, zero or many ObjectIds, or the ET encoded IDs for things like messages, fences, etc, and one or many AnalyticTypes, or what analytics are contained in this update. One common use of this is time in app - If a user spends 60 seconds (1 minute) in the app, you would make an ETEvent with a value of 60 and add the analytic type MobilePushStatisticTypeTimeInApp. If you want to add a message ID for an open, add a second analyticType of MobilePushStatisticTypeOpen and add the objectId of the message ID (_m). Easy peasey.
	/// 
	/// Some of the enum values are marked deprecated, which indicates that they should not be used in the SDK. These enum values are for analytics on the Middle Tier only. Do not use these in ETEvents.
	/// </summary>
	[BaseType(typeof(ETGenericUpdate), Delegates = new[] { "WeakDelegate" }, Events = new[] { typeof(ETGenericUpdateObjectProtocol) })]
	[DisableDefaultCtor]
	public partial interface ETEvent
	{
		/// <summary>
		/// DBID if the data was saved in the local database, or zero if this is a new object. 
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[Export("identifier")]
		nint Identifier { get; }

		/// <summary>
		/// When the ETEvent happened / was created. Is set to now when the ETEvent is alloc/init-ed.
		/// </summary>
		/// <value>
		/// The eevent date.
		/// </value>
		[Export("eventDate")]
		NSDate EventDate { get; }

		/// <summary>
		/// The descriptive value for this ETEvent. It is relative to the analyticTypes. 
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		[Export("value")]
		nint Value { get; set; }

		/// <summary>
		/// The current set of analyticTypes being tracked by this ETEvent. Add new ones through addAnalyticType:
		/// </summary>
		/// <value>
		/// The analytic types.
		/// </value>
		[Export("analyticTypes")]
		NSSet AnalyticTypes { get; }

		/// <summary>
		/// The current set of objectIds being tracked by this ETEvent. Add new ones through addObjectId:
		/// </summary>
		/// <value>
		/// The object ids.
		/// </value>
		[Export("objectIds")]
		NSSet ObjectIds { get; }

		/// <summary>
		/// Designated Initializer. Pass in an NSDictionary (usually from the cache db) to create an ETEvent.
		/// </summary>
		/// <param name="dict">A dictionary of values.</param>
		/// <returns>id An ETEvent</returns>
		[Export("initFromDictionary:")]
		IntPtr Constructor(NSDictionary dict);

		/// <summary>
		/// Adds a specific analyticType to the current set. You may add zero to many, relative to what you're reporting. 
		/// </summary>
		/// <param name="statType">The analyticType you wish to add. See MobilePushStatisticType enumeration for values.</param>
		[Export("addAnalyticType:")]
		void AddAnalyticType(MobilePushStatisticType statType);

		/// <summary>
		/// Removes an analytic type from the current set. It will echo out if it was removed, or nil if the value wasn't in there.
		/// </summary>
		/// <param name="statType">The type to remove.</param>
		/// <returns>An echo of statType if the value was removed, or nil if not.</returns>
		[Export("removeAnalyticType:")]
		MobilePushStatisticType RemoveAnalyticType(MobilePushStatisticType statType);

		/// <summary>
		/// Adds a specific ObjectID to the current set. These values are all encoded by the time they're on the device, so you can add any of them, regardless of what they are - meaning, Message ID (_m), Fence ID (ETRegion fenceIdentifier), etc. Just throw them in. 
		/// </summary>
		/// <param name="objectId">An objectID to add.</param>
		[Export("addObjectId:")]
		void AddObjectId(string objectId);

		/// <summary>
		/// Removes a specific ObjectID from the set, and echoes it if it was removed, or nil if the value wasn't in there.
		/// </summary>
		/// <param name="objectId">The objectId to add.</param>
		/// <returns>An echo of objectId if it was removed, or nil if it wasn't found.</returns>
		[Export("removeObjectId:")]
		string RemoveObjectId(string objectId);
	}

	/// <summary>
	/// ETFenceMessage is a middle class that joins together ETRegions and ETMessages, which exist in a many-to-many relationship with each other.
	/// 
	/// The SQL of this is set to ON CONFLICT REPLACE, meaning the pair of values will only ever be in there once. 
	/// 
	/// ETFenceMessages should be passed to ETPhoneHome through saveToDatabaseInstead: only. They shouldn't be sent to phoneHome:.
	/// </summary>
	[BaseType(typeof(ETGenericUpdate))]
	[DisableDefaultCtor]
	public partial interface ETFenceMessage
	{
		/// <summary>
		/// The ETRegion for this relationship.
		/// </summary>
		/// <value>
		/// The region.
		/// </value>
		[Export("region")]
		ETRegion Region { get; set; }

		/// <summary>
		/// The ETMessage for this relationship.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		[Export("message")]
		ETMessage message { get; set; }

		/// <summary>
		///  Creates a new ETFenceMessage for a specific ETRegion and ETMessage.
		/// </summary>
		/// <param name="region">The ETRegion half of this relationship.</param>
		/// <param name="message">The ETMessage half of this relationship.</param>
		/// <returns>A new ETFenceMessage.</returns>
		[Export("initWithRegion:andMessage:")]
		IntPtr Constructor(ETRegion region, ETMessage message);
	}

	/// <summary>
	/// This protocol defines methods that ETGenericUpdate objects must implement. 
	/// </summary>
	[Model, Protocol, BaseType(typeof(NSObject))]
	public partial interface ETGenericUpdateObjectProtocol
	{
		/// <summary>
		/// Allocs this instance.
		/// </summary>
		/// <returns></returns>
		//[Static, Export("alloc")]
		//IntPtr Constructor();

		/// <summary>
		/// Constructors the specified dict.
		/// </summary>
		/// <param name="dict">The dict.</param>
		/// <returns></returns>
		[Export("initFromDictionary:")]
		IntPtr Constructor(NSDictionary dict);

		/// <summary>
		/// Gets the remote route path static.
		/// </summary>
		/// <value>
		/// The remote route path static.
		/// </value>
		[Static, Export("remoteRoutePath")]
		string RemoteRoutePathStatic { get; }

		/// <summary>
		/// Gets the remote route path.
		/// </summary>
		/// <value>
		/// The remote route path.
		/// </value>
		[Export("remoteRoutePath")]
		string RemoteRoutePath { get; }

		/// <summary>
		/// Gets the table name static.
		/// </summary>
		/// <value>
		/// The table name static.
		/// </value>
		[Static, Export("tableName")]
		string TableNameStatic { get; }

		/// <summary>
		/// Gets the name of the table.
		/// </summary>
		/// <value>
		/// The name of the table.
		/// </value>
		[Export("tableName")]
		string TableName { get; }

		/// <summary>
		/// Gets the json payload as string.
		/// </summary>
		/// <value>
		/// The json payload as string.
		/// </value>
		[Export("jsonPayloadAsString")]
		string JsonPayloadAsString { get; }

		/// <summary>
		/// Gets the json payload as dictionary.
		/// </summary>
		/// <value>
		/// The json payload as dictionary.
		/// </value>
		[Export("jsonPayloadAsDictionary")]
		NSDictionary JsonPayloadAsDictionary { get; }

		/// <summary>
		/// Gets the send method.
		/// </summary>
		/// <value>
		/// The send method.
		/// </value>
		[Export("sendMethod")]
		GenericUpdateSendMethod SendMethod { get; }
	}

	/// <summary>
	/// ETGenericUpdate
	/// </summary>
	[BaseType(typeof(NSObject))]
	public partial interface ETGenericUpdate
	{
		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		/// <value>
		/// The tag.
		/// </value>
		[Export("tag")] // The property that started this whole ordeal.
		int Tag { get; set; }

		/// <summary>
		/// Gets or sets the database identifier.
		/// </summary>
		/// <value>
		/// The database identifier.
		/// </value>
		[Export("databaseIdentifier")]
		nint DatabaseIdentifier { get; set; }

		/// <summary>
		/// Gets or sets the response data.
		/// </summary>
		/// <value>
		/// The response data.
		/// </value>
		[Export("responseData")]
		NSMutableData ResponseData { get; set; }

		/// <summary>
		/// Gets or sets the response code.
		/// </summary>
		/// <value>
		/// The response code.
		/// </value>
		[Export("responseCode")]
		NSHttpUrlResponse ResponseCode { get; set; }

		/// <summary>
		/// Gets or sets the background task id.
		/// </summary>
		/// <value>
		/// The background task id.
		/// </value>
		[Export("backgroundTaskID")]
		NSObject BackgroundTaskID { get; set; } //: UIBackgroundTaskIdentifier

		// These methods need to be implemented so that ETPhoneHome works.
		#region Sending to ExactTarget

		/// <summary>
		/// The HTTP method that should be used for this call.
		/// </summary>
		/// <value>
		/// The send method.
		/// </value>
		[Export("sendMethod")]
		GenericUpdateSendMethod SendMethod { get; }

		/// <summary>
		/// The route to which the call should be made. This will be appended to the BaseURL in ETPhoneHome, and should lead with a slash.
		/// </summary>
		/// <value>
		/// The remote route path.
		/// </value>
		[Export("remoteRoutePath")]
		string RemoteRoutePath { get; }

		/// <summary>
		/// Serializes the payload for POSTing. 
		/// </summary>
		/// <value>
		/// The json payload as string.
		/// </value>
		[Export("jsonPayloadAsString")]
		string JsonPayloadAsString { get; }

		/// <summary>
		/// Gets the json payload as dictionary.
		/// </summary>
		/// <value>
		/// Serializes as a dictionary for bulk uploading.
		/// </value>
		[Export("jsonPayloadAsDictionary")]
		NSDictionary JsonPayloadAsDictionary { get; }

		/// <summary>
		/// Called by ETPhoneHome after the ETURLConnection is finished. This should handle doing anything that needs to be done to the payload after it's fully received (like, start monitoring for geofences.
		///
		/// It's called after a respondsToSelector: so it doesn't have to be implemented.
		/// </summary>
		[Export("processResults")]
		void ProcessResults();

		/// <summary>
		/// Called by ETPhone if the ETURLConnection fails. This should do it's best to recover what it can, maybe loading things from the database or whatever. 
		/// 
		/// Sometimes bad things happen when retrieving data from ExactTarget. I mean, cellular Internet isn't a perfect science.
		/// </summary>
		[Export("handleDataFailure")]
		void HandleDataFailure();

		/// <summary>
		/// Not everything should save itself to the database. By default, they should, since that's the expectation that's already set. However, sometimes, it doesn't make sense. This controls that. 
		/// 
		/// "But, if it defaults to 'YES', can't that leave to errors if the other methods aren't implemented?"
		/// 
		/// Yup. But that's on them. Also, that's why we have asserts in the base class method. 
		/// </summary>
		/// <value>
		/// <c>true</c> if [should save self to database]; otherwise, <c>false</c>.
		/// </value>
		[Export("shouldSaveSelfToDatabase")]
		bool ShouldSaveSelfToDatabase { get; }

		#endregion

		// These methods are used to save oneself to the database. The objects need to bootstrap themselves to a savable state, and these methods make that possible through a strange combination of inheritance, witch craft, and magic. And Objective-C.
		// Worth noting that these should almost always cause a crash if they come from GenericUpdate, and in reality are being superceded by their children with real values. Also, some are passthrough to other things or statics because I'm kind of making this up as I go along.
		#region Saving to the Database

		/// <summary>
		/// To make the databases self-updating (more or less), we keep track of the version of the local DB that the insert query represents. This number is stored to NSUserDefaults with the key in databaseVersionKey and checked before inserts. If the number returned is less than this number, it drops and recreates the database table. 
		/// </summary>
		/// <value>
		/// The db version number.
		/// </value>
		[Export("dbVersionNumber")]
		int DbVersionNumber { get; }

		/// <summary>
		/// And this is the key to match the dbVersionNumber. It is saved to NSUserDefaults in combination with dbVersionNumber to identify the age of the table.
		/// </summary>
		/// <value>
		/// The database version key.
		/// </value>
		[Export("databaseVersionKey")]
		string DatabaseVersionKey { get; }

		/// <summary>
		/// Passes a call to the Static method of the same name, with the correct object named in the instance variable. It needed an instance counterpart because we are dealing with a specific update at the point where this is called, and that's the perfect place to reference back to the static version.
		/// </summary>
		/// <value>
		/// <c>true</c> if [generate persistent data schema in database]; otherwise, <c>false</c>.
		/// </value>
		[Export("generatePersistentDataSchemaInDatabase")]
		bool GeneratePersistentDataSchemaInDatabase { get; }

		/// <summary>
		/// Returns the arguments that should be inserted into the database to match the query specified in the previous method. As such, the number should equal the number of question marks used in the previous method.
		///  
		/// Also, they need to all be NSObjects, and not primitives or non-object variants of NULL. So, use an NSNumber wrapper for numbers and bools, and [NSNull null] for nils. Please.
		/// </summary>
		/// <value>
		/// The insert query arguments.
		/// </value>
		[Export("insertQueryArguments")]
		NSObject[] InsertQueryArguments { get; }

		/// <summary>
		/// Returns the arguments that should be inserted into the database to match the query specified in the previous method. As such, the number should equal the number of question marks used in the previous method.
		///  
		/// Also, they need to all be NSObjects, and not primitives or non-object variants of NULL. So, use an NSNumber wrapper for numbers and bools, and [NSNull null] for nils. Please.
		/// </summary>
		/// <value>
		/// The update query arguments.
		/// </value>
		[Export("updateQueryArguments")]
		NSObject[] UpdateQueryArguments { get; }

		/// <summary>
		/// Returns the SQL string that should execute on insert. It should be ready to be prepared and bound via SQLite, so use placeholders where appropriate. 
		/// 
		/// Also, if you're new to SQLite and binding, the number of question marks in this statement should be equal to the number of arguments returned in the next method.
		/// </summary>
		/// <value>
		/// The insert query syntax.
		/// </value>
		[Export("insertQuerySyntax")]
		string InsertQuerySyntax { get; }

		/// <summary>
		/// Returns the SQL string that should execute on insert. It should be ready to be prepared and bound via SQLite, so use placeholders where appropriate. 
		/// 
		/// Also, if you're new to SQLite and binding, the number of question marks in this statement should be equal to the number of arguments returned in the next method.
		/// </summary>
		/// <value>
		/// The update query syntax.
		/// </value>
		[Export("updateQuerySyntax")]
		string UpdateQuerySyntax { get; }

		/// <summary>
		/// An exception already. This method should *not* be called from the children objects. Just pretend you can't see it.
		/// </summary>
		/// <value>
		/// <c>true</c> if [insert self into database]; otherwise, <c>false</c>.
		/// </value>
		[Export("insertSelfIntoDatabase")]
		bool InsertSelfIntoDatabase { get; }

		/// <summary>
		/// Returns the name of the table the object should save to. Since this comes in static and instance varieties (sorry), it should return a constant or static string from the object itself.
		/// </summary>
		/// <value>
		/// The name of the table.
		/// </value>
		[Export("tableName")]
		string TableName { get; }

		/// <summary>
		/// Returns the name of the table the object should save to. Since this comes in static and instance varieties (sorry), it should return a constant or static string from the object itself.
		/// </summary>
		/// <value>
		/// The name of the table.
		/// </value>
		[Static, Export("tableName")]
		string TableNameStatic { get; }

		#endregion

		// These methods apply to the generic object in question.
		#region Statics

		/// <summary>
		/// Gets the formatter of correct format.
		/// </summary>
		/// <value>
		/// The formatter of correct format.
		/// </value>
		[Static, Export("formatterOfCorrectFormat")]
		NSDateFormatter FormatterOfCorrectFormat { get; }

		/// <summary>
		/// Gets the alternative formatter of correct format.
		/// </summary>
		/// <value>
		/// The alternative formatter of correct format.
		/// </value>
		[Static, Export("alternativeFormatterOfCorrectFormat")]
		NSDateFormatter AlternativeFormatterOfCorrectFormat { get; }

		/// <summary>
		/// Dates from string.
		/// </summary>
		/// <param name="dateAsString">The date as string.</param>
		/// <returns></returns>
		[Static, Export("dateFromString:")]
		NSDate DateFromString(string dateAsString);

		/// <summary>
		/// Gets the number formatter of correct format for double.
		/// </summary>
		/// <value>
		/// The number formatter of correct format for double.
		/// </value>
		[Static, Export("numberFormatterOfCorrectFormatForDouble")]
		NSNumberFormatter NumberFormatterOfCorrectFormatForDouble { get; }

		#endregion
	}

	/// <summary>
	/// This is a simple little database wrapper that lets us persist data in a generic place and way. Because sometimes we need to do that, and clustering up NSUserDefaults is silly. 
	/// 
	/// For the sake of simplicity, this store only deals with NSStrings. Please plan accordingly. 
	/// </summary>
	[BaseType(typeof(NSObject))]
	[DisableDefaultCtor]
	public partial interface ETKeyValueStore
	{
		/// <summary>
		/// Sets the value for a specific key into the ETKeyValueStore. Both things are NSString objects.
		/// </summary>
		/// <param name="value">The value to save.</param>
		/// <param name="key">The key to save.</param>
		/// <returns>T/F if the save worked.</returns>
		[Static, Export("setValue:forKey:")]
		bool SetValue(string value, string key);

		/// <summary>
		/// Retrieves a value for a given key from the database, or nil if it's not in there.
		/// </summary>
		/// <param name="key">The key to retrieve.</param>
		/// <returns>The value, or nil if it's not there.</returns>
		[Static, Export("valueForKey:")]
		string ValueForKey(string key);
	}

	/// <summary>
	/// This is a helper class that shows webpages. These come down in several forms - sometimes a CloudPage, sometimes something from OpenDirect - and this guy shows them. It's a pretty simple class that pops up a view with a toolbar, shows a webpage, and waits to be dismissed. 
	/// </summary>
	[BaseType(typeof(UIViewController), Delegates = new[] { "Delegate" }, Events = new[] { typeof(UIWebViewDelegate) })]
	[DisableDefaultCtor]
	public partial interface ETLandingPagePresenter
	{
		// UIWebView _theWebView;
		// UILabel _pageTitle;

		/// <summary>
		/// Don't let the name fool you - this can be *any* URL, not just a landing page. It will eventually be converted to an NSURL and displayed.
		/// </summary>
		/// <value>
		/// The landing page path.
		/// </value>
		[Export("landingPagePath")]
		string LandingPagePath { get; set;  }

		/// <summary>
		/// A helper designated initializer that takes the landing page as a string.
		/// </summary>
		/// <param name="landingPage">The landing page.</param>
		/// <returns></returns>
		[Export("initForLandingPageAt:")]
		IntPtr Constructor(string landingPage);

		/// <summary>
		/// Another helper that takes it in NSURL form. We're not picky. It'd be cool of ObjC did method overloading, though.
		/// </summary>
		/// <param name="landingPage">The landing page.</param>
		/// <returns></returns>
		[Export("initForLandingPageAtWithURL:")]
		IntPtr Constructor(NSUrl landingPage);
	}

	/// <summary>
	/// ETLocationManager is the main interface to ExactTarget's Location Services. In the way that ETPush manages the push notification cycle, ETLocationMangaer manages geo services. It will use some of the information from ETPush (namely, App ID and Access Token) to function, but is an independent piece of functionality. 
	/// 
	/// Due to the invasive nature of location services, ETLocationManager defaults to off, and must be explicity turned on by the developer, whether invisibly to the user or not. To begin location services, call [[ETLocationManager locationManager] startWatchingLocation]. Similarly, to stop location services, call [[ETLocationManager locationManager] stopWatchingLocation]. 
	/// 
	/// ETLocationManager will always respect the user's wishes (as enforced by iOS), so if the user disables Location Services at the system level through Settings, the SDK will be unable to use any location services, and fence monitoring will not function. You can check for this by querying [[ETLocationManager locationManager] locationEnabled], as it will reconcile app-level permissions as well as the state of you enabling loc services. Internally, this method is used to report back to ExactTarget on the state of location services, so it is trustworthy. 
	/// 
	/// Please ensure you are linking against CoreLocation. You will get errors otherwise. 
	/// </summary>
	[BaseType(typeof(NSObject), Delegates = new[] { "Delegate" }, Events = new[] { typeof(CLLocationManagerDelegate) })]
	//[DisableDefaultCtor]
	public partial interface ETLocationManager
	{
		/// <summary>
		/// Keeps track of if we are currently in the middle of updating Geofences. That should only work one at a time.
		/// </summary>
		/// <value>
		///   <c>true</c> if [updating geofences]; otherwise, <c>false</c>.
		/// </value>
		[Export("updatingGeofences")]
		bool UpdatingGeofences { [Bind("isUpdatingGeofences")] get; set; }

		/// <summary>
		/// Returns a reference to the shared loc manager.
		/// </summary>
		/// <value>
		/// The location manager.
		/// </value>
		[Static, Export("locationManager")]
		ETLocationManager LocationManager { get; }

		/// <summary>
		/// Constructors this instance.
		/// </summary>
		/// <returns></returns>
		//[Export("init")]
		//IntPtr Constructor();

		/// <summary>
		/// Determines the state of Location Services based on developer setting and OS-level permission. This is the preferred method for checking for location state.
		/// </summary>
		/// <value>
		///   <c>true</c> if [location enabled]; otherwise, <c>false</c>.
		/// </value>
		[Export("locationEnabled")]
		bool LocationEnabled { get; }

		/// <summary>
		/// Use this method to initiate Location Services through the MobilePush SDK.
		/// </summary>
		[Export("startWatchingLocation")]
		void StartWatchingLocation();

		/// <summary>
		/// Use this method to disable Location Services through the MobilePush SDK
		/// </summary>
		[Export("stopWathingLocation")]
		void StopWathingLocation();

		/// <summary>
		/// Calls various handlers that should fire when the app enters the foreground.
		/// </summary>
		[Export("appInForeground")]
		void AppInForeground();

		/// <summary>
		/// Calls various handlers that should fire when the app enters the background.
		/// </summary>
		[Export("appInBackground")]
		void AppInBackground();

		/// <summary>
		/// Queues a send for a location update to ExactTarget.
		/// </summary>
		/// <param name="loc">The loc.</param>
		/// <param name="state">The state.</param>
		[Export("updateLocationServerWithLocation:forAppState:")]
		void UpdateLocationServerWithLocation(CLLocation loc, LocationUpdateAppState state);

		#region Location

		/// <summary>
		/// Takes in an NSSet of fences that should be monitored.
		/// </summary>
		/// <param name="fences">The set to monitor.</param>
		[Export("monitorRegions:")]
		void MonitorRegions(NSSet fences);

		/// <summary>
		/// Instructs the CLLocationManager to stop monitoring all regions.
		/// </summary>
		[Export("stopMonitoringRegions")]
		void StopMonitoringRegions();

		/// <summary>
		/// Retrieves the messages for a given ETRegion and MobilePushMessageType and schedules any messages returned for display.
		/// </summary>
		/// <param name="region">The ETRegion that prompted this action.</param>
		/// <param name="type">The MobilePushMessageType of event that prompted this action.</param>
		[Export("getAndScheduleAlertsForRegion:andMessageType:")]
		void GetAndScheduleAlertsForRegion(ETRegion region, MobilePushMessageType type);

		/// <summary>
		/// Returns the currently monitored regions.
		/// </summary>
		/// <value>
		/// An NSSet of monitored regions.
		/// </value>
		[Export("monitoredRegions")]
		NSSet MonitoredRegions { get; }

		/// <summary>
		/// A dictionary version of the Last Known Location. The dictionary will contain two keys, latitude and longitude, which are NSNumber wrappers around doubles. Use doubleValue to retrieve.
		/// </summary>
		/// <value>
		/// The last known location.
		/// </value>
		[Export("lastKnownLocation")]
		NSDictionary LastKnownLocation { get; }

		#endregion

		#region For Tests

		/// <summary>
		/// Returns if we are currently watching location.
		/// </summary>
		/// <value>
		/// T/F if locations are being watched.
		/// </value>
		[Export("getWatchingLocation")]
		bool GetWatchingLocation { get; }

		#endregion
	}

	/// <summary>
	///  { "deviceid", "required, unique id for the device. (string)" },
	///  { "latitude", "required, the latitude value for the location. (double)" },
	///  { "longitude", "required, the longitude value for the location. (double)" },
	///  { "accuracy", "required, the accuracy of the location data, in meters. (int)" },
	///  { "location_datetime_utc", "optional, the time the location data was recorded, in ISO 8601 UTC format. If not provided, current time is used. (string)" },
	/// </summary>
	[BaseType(typeof(ETGenericUpdate))]
	[DisableDefaultCtor]
	public partial interface ETLocationUpdate
	{
		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		/// <value>
		/// The location.
		/// </value>
		[Export("location")]
		CLLocation Location { get; set; }

		/// <summary>
		/// Gets or sets the event date time.
		/// </summary>
		/// <value>
		/// The event date time.
		/// </value>
		[Export("eventDateTime")]
		NSDate EventDateTime { get; set; }

		// So, I hate to use these. But I have to. Because sometimes I need to fake a location update.
		// Upside: Because they're NSObjects, they are arguably more useful than CLLocation or int. Arguably. I mean, you can put them in Arrays and Dictionaries.
		#region Kludge

		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		/// <value>
		/// The latitude.
		/// </value>
		[Export("latitude")]
		NSNumber Latitude { get; set; }

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		/// <value>
		/// The longitude.
		/// </value>
		[Export("longitude")]
		NSNumber Longitude { get; set; }

		/// <summary>
		/// Gets or sets the accuracy.
		/// </summary>
		/// <value>
		/// The accuracy.
		/// </value>
		[Export("accuracy")]
		NSNumber Accuracy { get; set; }

		/// <summary>
		/// Gets or sets the state of the app.
		/// </summary>
		/// <value>
		/// The state of the app.
		/// </value>
		[Export("appState")]
		LocationUpdateAppState AppState { get; set; }

		#endregion

		/// <summary>
		/// Gets a value indicating whether [generate persistent data schema in database].
		/// </summary>
		/// <value>
		/// <c>true</c> if [generate persistent data schema in database]; otherwise, <c>false</c>.
		/// </value>
		[Static, Export("generatePersistentDataSchemaInDatabase")]
		bool GeneratePersistentDataSchemaInDatabase { get; }

		/// <summary>
		/// Gets the name of the table.
		/// </summary>
		/// <value>
		/// The name of the table.
		/// </value>
		[Export("tableName")]
		string TableName { get; }

		/// <summary>
		/// Gets the name of the table.
		/// </summary>
		/// <value>
		/// The name of the table.
		/// </value>
		[Static, Export("tableName")]
		string TableNameStatic { get; } // both, because they're in different ways and places. Don't worry, they're CONSTANT POWERED!

		/// <summary>
		/// Constructors the specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="state">The state.</param>
		/// <returns></returns>
		[Export("initWithLocation:forAppState:")]
		IntPtr Constructor(CLLocation location, LocationUpdateAppState state);

		/// <summary>
		/// Gets the remote route path.
		/// </summary>
		/// <value>
		/// The remote route path.
		/// </value>
		[Export("remoteRoutePath")]
		string RemoteRoutePath { get; }

		/// <summary>
		/// Gets the formatted date.
		/// </summary>
		/// <value>
		/// The formatted date.
		/// </value>
		[Export("formattedDate")]
		string FormattedDate { get; }

		/// <summary>
		/// Gets the json payload as string.
		/// </summary>
		/// <value>
		/// The json payload as string.
		/// </value>
		[Export("jsonPayloadAsString")]
		string JsonPayloadAsString { get; }
	}

	/// <summary>
	/// ETMessage is the local representation of a Message from ExactTarget. They are multipurpose, sometimes representing a message that should be scheduled because of the entrance or exit of a Geofence, the proximal arrival to an iBeacon, or a CloudPage message downloaded from ET. Because of their multipurpose nature, there are a lot of different attributes on them, many of which may be null at any give time depending on the type of message. 
	/// 
	/// ETMessages also feature Message Limiting, a system of preventing a given message from firing too often. If described in a sentence with the parameters interlaced, it would read "show this message only 1 (messagesPerPeriod) time per 1 (numberOfPeriods) hour (periodType). As of a recent release, messagesPerPeriod will be defaulted to 1 on the Middle Tier, so if it is null or absent, we assume 1, otherwise take the given value.
	/// 
	/// Many of these accessors are readonly because the value should be trusted and not changed. There are specific methods to modify the message, such as markAsRead or markAsDeleted. Also, unless specifically marked in a method, only active methods are returned through the getter methods. 
	/// </summary>
	[BaseType(typeof(ETGenericUpdate))]
	[DisableDefaultCtor]
	public partial interface ETMessage
	{
		/// <summary>
		/// Encoded ID from ExactTarget. Will match the ID in MobilePush. This is the primary key. 
		/// </summary>
		/// <value>
		/// The message identifier.
		/// </value>
		[Export("messageIdentifier")]
		string MessageIdentifier { get; }

		/// <summary>
		/// This is the name which is set on ExactTargetMarketingCloud, while setting the ETMessage.
		/// </summary>
		/// <value>
		/// The name of the message.
		/// </value>
		[Export("messageName")]
		string MessageName { get; set; }

		/// <summary>
		/// The type of ETMessage being represented.
		/// </summary>
		/// <value>
		/// The type of the message.
		/// </value>
		[Export("messageType")]
		MobilePushMessageType MessageType { get; }

		/// <summary>
		/// Bitmask of features that this message has on it (CloudPage, Push only)
		/// </summary>
		/// <value>
		/// The type of the content.
		/// </value>
		[Export("contentType")]
		MobilePushContentType ContentType { get; }

		/// <summary>
		/// The alert text of the message. This displays on the screen. 
		/// </summary>
		/// <value>
		/// The alert.
		/// </value>
		[Export("alert")]
		string Alert { get; }

		/// <summary>
		/// The sound that should play, if any. Most of the time, either "default" or "custom.caf", conventions enforced in MobilePush. 
		/// </summary>
		/// <value>
		/// The sound.
		/// </value>
		[Export("sound")]
		string Sound { get; }

		/// <summary>
		/// The badge modifier. This should be a NSString in the form of "+1" or nothing at all. It's saved as a string because of that.
		/// </summary>
		/// <value>
		/// The badge.
		/// </value>
		[Export("badge")]
		string Badge { get; }

		/// <summary>
		/// An array of Key Value Pairs, or Custom Keys in local parlance, for this message. This will contain NSDictionary objects.
		/// </summary>
		/// <value>
		/// The key value pairs.
		/// </value>
		[Export("keyValuePairs")]
		NSObject[] KeyValuePairs { get; }

		/// <summary>
		/// The message's start date. Messages shouldn't show before this time. 
		/// </summary>
		/// <value>
		/// The start date.
		/// </value>
		[Export("startDate")]
		NSDate StartDate { get; }

		/// <summary>
		/// The message's end date. Messages shouldn't show after this time.
		/// </summary>
		/// <value>
		/// The end date.
		/// </value>
		[Export("endDate")]
		NSDate EndDate { get; }

		/// <summary>
		/// The Site ID for the CloudPage attached to this message. 
		/// </summary>
		/// <value>
		/// The site identifier.
		/// </value>
		[Export("siteIdentifier")]
		string SiteIdentifier { get; }

		/// <summary>
		/// The Site URL for the ClouePage attached to this message. It is saved as an NSString and converted later to NSURL.
		/// </summary>
		/// <value>
		/// The site URL as string.
		/// </value>
		[Export("siteUrlAsString")]
		string SiteUrlAsString { get; }

		/// <summary>
		/// OpenDirect payload for this message, if there is one. 
		/// </summary>
		/// <value>
		/// The open direct payload.
		/// </value>
		[Export("openDirectPayload")]
		string OpenDirectPayload { get; }

		/// <summary>
		/// The related ETRegion for this message. This is a remnant of days when the relationship was one to one. It is not anymore. 
		/// </summary>
		/// <value>
		/// The related fence.
		/// </value>
		[Obsolete]
		[Export("relatedFence")]
		ETRegion RelatedFence { get; }

		/// <summary>
		/// The total number of times, ever, that a message will show on a device. 
		/// </summary>
		/// <value>
		/// The message limit.
		/// </value>
		[Export("messageLimit")]
		int MessageLimit { get; }

		/// <summary>
		/// The total number of times for a given number of time units that a message can be shown. In the statement "show 1 time per 2 hours", this is the "1" part.
		/// 
		/// This defaults to 1 if it is not set in the received payload from ExactTarget. 
		/// </summary>
		/// <value>
		/// The messages per period.
		/// </value>
		[Export("messagesPerPeriod")]
		int MessagesPerPeriod { get; }

		/// <summary>
		/// The number of time periods in which a message should be limited. In the statement "show 1 time per 2 hours", this is the "2" part.
		/// </summary>
		/// <value>
		/// The number of periods.
		/// </value>
		[Export("numberOfPeriods")]
		int NumberOfPeriods { get; }

		/// <summary>
		/// The time unit counted in numberOfPeriods. In the statement "show 1 time per 2 hours", this is the "hours" part.
		/// </summary>
		/// <value>
		/// The type of the period.
		/// </value>
		[Export("periodType")]
		MobilePushMessageFrequencyUnit PeriodType { get; }

		/// <summary>
		/// Whether or not the period is a rolling period. Defaults to YES through code. 
		/// 
		/// Consider a message being fired at 2:19PM, and it may only be shown once per hour. In a rolling period, the next time it may show is 3:19PM. In a non-rolling period, the next earliest showing time is 3:00PM, the start of the next hour. 
		/// </summary>
		/// <value>
		///   <c>true</c> if [rolling period]; otherwise, <c>false</c>.
		/// </value>
		[Export("rollingPeriod")]
		bool RollingPeriod { [Bind("isRollingPeriod")] get; }

		/// <summary>
		/// The number of times an ETRegion must be tripped before the message shows. This is not currently used, and is a placeholder for future functionality.
		/// </summary>
		/// <value>
		/// The read.
		/// </value>
		[Export("minTripped")]
		NSNumber MinTripped { get; }

		/// <summary>
		/// Ephemeral Messages disappear when the user walks away from the iBeacon that tripped the message. The default value is NO.
		/// </summary>
		/// <value>
		///   <c>true</c> if [read]; otherwise, <c>false</c>.
		/// </value>
		/// 
		[Export("ephemeralMessage")]
		bool EphemeralMessage { [Bind("isEphemeralMessage")] get; }

		/// <summary>
		/// For iBeacon messages, the proximity the user must arrive in before the message is fired. It is treated as a "less than" value, meaning if the message is set to Far, the message can be shown in Far, Near or Immediate.
		/// </summary>
		/// <value>
		/// The proximity.
		/// </value>
		[Export("proximity")]
		CLProximity Proximity { get; }

		/// <summary>
		/// The number of seconds the user must stand near an iBeacon before the message is displayed. This is treated as an offset in scheduling the UILocalNotification, which will be cancelled if the user walks away too early. 
		/// </summary>
		/// <value>
		/// The loitering seconds.
		/// </value>
		[Export("loiteringSeconds")]
		nint loiteringSeconds { get; }

		/// <summary>
		/// Whether or not the message has been read. This must be set through markAsRead by the developer.
		/// </summary>
		/// <value>
		///   <c>true</c> if [read]; otherwise, <c>false</c>.
		/// </value>
		[Export("read")]
		bool Read { [Bind("isRead")] get; }

		/// <summary>
		/// Whether or not the message is active in the local database. 
		/// </summary>
		/// <value>
		///   <c>true</c> if [active]; otherwise, <c>false</c>.
		/// </value>
		[Export("active")]
		bool Active { [Bind("isActive")] get; }

		/// <summary>
		/// A reference to the UILocalNotification triggered for this message. It is used later to cancel the message if need be.
		/// </summary>
		/// <value>
		/// The notification.
		/// </value>
		[Export("notification")]
		UILocalNotification Notification { get; set; }

		/// <summary>
		/// Creates a new ETMessage with values in the given NSDictionary.
		/// </summary>
		/// <param name="dict">A dictionary of values to apply to the ETMessage.</param>
		/// <returns>A new ETMessage.</returns>
		[Export("initFromDictionary:")]
		IntPtr Constructor(NSDictionary dict);

		/// <summary>
		/// Designated Initializer. Creates a new ETMessage with values from an NSDictionary for a specific ETRegion.
		/// </summary>
		/// <param name="dict">A dictionary of values to apply to the ETMessage.</param>
		/// <param name="region">The ETRegion that prompted the creation of this ETMessage.</param>
		/// <returns>A new ETMessage.</returns>
		[Export("initFromDictionary:forFence:")]
		IntPtr Constructor(NSDictionary dict, ETRegion region);

		/// <summary>
		/// This is an overridden accessor for subj ect to handle some business logic around what to show. Use this for display in an inbox.
		/// </summary>
		/// <value>
		/// The subject.
		/// </value>
		[Export("subject")]
		string Subject { get; } // Public getter, now with logic.

		/// <summary>
		/// Cleanses and returns the Sites URL as a proper NSURL. This is mostly for convenience.
		/// </summary>
		/// <value>
		/// The site URL.
		/// </value>
		[Export("siteURL")]
		NSUrl SiteURL { get; }

		/// <summary>
		/// Marks a message as read in the local cache. Read messages do not show up in the Inbox.
		/// </summary>
		/// <value>
		///   <c>true</c> if [mark as read]; otherwise, <c>false</c>.
		/// </value>
		[Export("markAsRead")]
		bool MarkAsRead();

		/// <summary>
		/// Records a message as scheduled, and updates the fun, fun logic around when it should again, if it should of course. 
		/// </summary>
		/// <value>
		/// <c>true</c> if [message scheduled for display]; otherwise, <c>false</c>.
		/// </value>
		[Export("messageScheduledForDisplay")]
		bool MessageScheduledForDisplay();

		/// <summary>
		/// Marks a message as unread. Just for convenience. 
		/// </summary>
		/// <value>
		///   <c>true</c> if [mark as unread]; otherwise, <c>false</c>.
		/// </value>
		[Export("markAsUnread")]
		bool MarkAsUnread();

		/// <summary>
		/// Marks a message as deleted. They will not be returned after this, and it's irreversable.
		/// </summary>
		/// <value>
		///   <c>true</c> if [mark as deleted]; otherwise, <c>false</c>.
		/// </value>
		[Export("markAsDeleted")]
		bool MarkAsDeleted();

		#region Methods for Testing

		/// <summary>
		/// Getter for a private value, lastShownDate.
		/// </summary>
		/// <value>
		/// The get last shown date.
		/// </value>
		[Export("getLastShownDate")]
		NSDate GetLastShownDate { get; }

		/// <summary>
		/// Getter for a private value, showCount.
		/// </summary>
		/// <value>
		/// The get show count.
		/// </value>
		[Export("getShowCount")]
		int GetShowCount { get; }

		#endregion

		#region Message Retrieval Helpers

		/// <summary>
		/// Gets all active messages for a specific contentType, usually Cloud Pages.
		/// </summary>
		/// <param name="contentType">Type of the content.</param>
		/// <returns>An NSArray of ETMessages</returns>
		[Static, Export("getMessagesByContentType:")]
		NSObject[] GetMessagesByContentType(MobilePushContentType contentType);

		/// <summary>
		/// Gets a specific ETMessage for a given identifer. 
		/// </summary>
		/// <param name="identifier">The Message ID to retrieve.</param>
		/// <returns>The ETMessage, or nil if not found in the database.</returns>
		[Static, Export("getMessageByIdentifier:")]
		ETMessage GetMessageByIdentifier(string identifier);

		/// <summary>
		/// Gets all active ETMessages for a specific message type, like Fence Entry, Exit or Proximity..
		/// </summary>
		/// <param name="type">The MobilePushMessageType you'd like back.</param>
		/// <returns>An array of ETMessages.</returns>
		[Static, Export("getMessagesByType:")]
		NSObject[] GetMessagesByType(MobilePushMessageType type);

		/// <summary>
		/// Gets all active ETMessages tied to a specific ETRegion (Geofence).
		/// </summary>
		/// <param name="fence">The ETRegion for which you would like messages.</param>
		/// <returns>An NSArray of ETMessages.</returns>
		[Static, Export("getMessagesForGeofence:")]
		NSObject[] GetMessagesForGeofence(ETRegion fence);

		/// <summary>
		/// Gets all active ETMessages tied to a specific ETRegion (Geofence) and MobilePushMessageType, like Entry ot Exit.
		/// </summary>
		/// <param name="fence">The ETRegion for which you would like messages.</param>
		/// <param name="type">The MobilePushMessageType that describes the messages you want.</param>
		/// <returns>An NSArray of ETMessages that meet the criteria asked for.</returns>
		[Static, Export("getMessagesForGeofence:andMessageType:")]
		NSObject[] GetMessagesForGeofence(ETRegion fence, MobilePushMessageType type);

		/// <summary>
		/// Gets all active ETMessages for a specific ETRegion (Proximity).
		/// </summary>
		/// <param name="region">The ETRegion for which you would like messages.</param>
		/// <returns>An NSArray of ETMessages.</returns>
		[Static, Export("getProximityMessagesForRegion:")]
		NSObject[] GetProximityMessagesForRegion(ETRegion fence); // withRangedBeaconProximity:(CLProximity)prox;

		/// <summary>
		/// Triggeres a data pull from ExactTarget for messages that meet the supplied requirements.
		/// </summary>
		/// <param name="messageType">The Message Type you wish to retrieve.</param>
		/// <param name="contentType">The Content Type you wish to retrieve.</param>
		/// <remarks>Doesn't return a value, but has delegate callbacks. </remarks>
		[Static, Export("getMessagesFromExactTargetOfMessageType:andContentType:")]
		void GetMessagesFromExactTargetOfMessageType(MobilePushMessageType messageType, MobilePushContentType contentType);

		/// <summary>
		/// Marks all messages for a given type as inactive. This is done prior to processing new messages just received from ExactTarget. 
		/// </summary>
		/// <param name="type">type The MobilePushMessageType you wish to invalidate.</param>
		/// <returns>T/F if the invalidation query worked</returns>
		[Static, Export("invalidateAllMessagesForType:")]
		bool InvalidateAllMessagesForType(MobilePushMessageType type);

		/// <summary>
		/// ETMessage equality. Since object equality won't always work, this compares messageIdentifiers to determine equality.
		/// </summary>
		/// <param name="message">The ETMessage to compare self to.</param>
		/// <returns>T/F if the messages are equal.</returns>
		[Export("isEqualToMessage:")]
		bool IsEqualToMessage(ETMessage message);

		#endregion
	}

	/// <summary>
	/// ETPhoneHome is like a highway management system, governing the sending of data to and from ExactTarget, and caching that which can't get sent home. It works by marshalling around GenericUpdate object subclasses, which themselves create a common pattern for handling business. 
	/// 
	/// Data should be sent back using phoneHome:, which will start the process of sending data to ET, and failing that, save it to the database. The behavior is all controlled by methods on the GenericUpdate object.
	/// </summary>
	[BaseType(typeof(NSUrlConnectionDelegate), Delegates = new[] { "Delegate" }, Events = new[] { typeof(NSUrlConnectionDataDelegate) })]
	[DisableDefaultCtor]
	public partial interface ETPhoneHome
	{
		/// <summary>
		/// Singleton accessor. This isn't to be publicly used, so we can have a sense of humor about it.
		/// </summary>
		/// <value>
		/// The magic bicycle.
		/// </value>
		[Static, Export("magicBicycle")]
		ETPhoneHome magicBicycle { get; }

		/// <summary>
		/// Begins the process of sending data back to ExactTarget. 
		/// </summary>
		/// <param name="updateObject">A subclass of GenericUpdate that wants to be send to ET.</param>
		/// <returns>bool Whether or not it was able to send to ET.</returns>
		[Export("phoneHome:")]
		bool PhoneHome(ETGenericUpdate updateObject);

		/// <summary>
		/// Begins the process of sending data back to ExactTarget, but does so for bulk data. This is different than phoneHome: because it will send an array of things, and not just one object.
		/// </summary>
		/// <param name="updateClass">The update class.</param>
		/// <returns></returns>
		[Export("phoneHomeInBulkForGenericUpdateType:")]
		bool PhoneHomeInBulkForGenericUpdateType(Class updateClass);

		/// <summary>
		/// Saves the udpate object to the database in the event of a send failure. It is exposed in the header because some objects just need to be saved instead of sent. This method should not be used publicly.
		/// </summary>
		/// <param name="updateObject">The subclass of GenericUpdate to save to the database.</param>
		/// <returns>Whether or not the save succeeded. Sometimes they don't.</returns>
		[Export("saveToDatabaseInstead:")]
		bool SaveToDatabaseInstead(ETGenericUpdate updateObject);

		/// <summary>
		/// Checks the cache database for records that weren't successfully sent to ExactTarget, and tries to send them. No return value.
		/// </summary>
		[Export("checkForAndSendBackCachedData")]
		void CheckForAndSendBackCachedData();

		/// <summary>
		/// For ETPhoneHome_Tests
		/// </summary>
		/// <value>
		/// The get number of active connections.
		/// </value>
		[Export("getNumberOfActiveConnections")]
		int GetNumberOfActiveConnections { get; }
	}

	/// <summary>
	/// Supporting protocol for OpenDirect, part of the ExactTarget 2013-02 release. 
	/// 
	/// Implementation of this delegate is not required for OpenDirect to function, but it is provided as a convenience to developers who do not with to parse the push payload on their own. 
	/// 
	/// All OpenDirect data is passed down as a JSON String, so you get it as an NSString. Please remember to parse it appropriately from there. Also, please remember to fail gracefully if you can't take action on the message. 
	/// 
	/// Also, please note that setting an OpenDirect Delegate will negate the automatic webpage loading feature added to MobilePush recently. This is deliberately to not stomp on your URLs and deep links. 
	/// </summary>
	[Model, Protocol, BaseType(typeof(NSObject))]
	public partial interface ExactTargetOpenDirectDelegate
	{
		/// <summary>
		/// Method called when an OpenDirect payload is received from MobilePush.
		/// </summary>
		/// <param name="payload">The contents of the payload as received from MobilePush.</param>
		[Export("didReceiveOpenDirectMessageWithContents:")]
		void DidReceiveOpenDirectMessageWithContents(string payload);

		/// <summary>
		/// Allows you to define the behavior of OpenDirect based on application state. 
		///
		/// If set to YES, the OpenDirect delegate will be called if a push with an OpenDirect payload is received and the application state is running. This is counter to normal push behavior, so the default is NO.
		///
		/// Consider that if you set this to YES, and the user is running the app when a push comes in, the app will start doing things that they didn't prompt it to do. This is bad user experience since it's confusing to the user. Along these lines, iOS won't present a notification if one is received while the app is running. 
		/// </summary>
		/// <value>
		/// representing whether or not you want action to be taken.
		/// </value>
		[Export("shouldDeliverOpenDirectMessageIfAppIsRunning")]
		bool ShouldDeliverOpenDirectMessageIfAppIsRunning { get; }
	}

	/// <summary>
	/// This is the main interface to the ExactTarget MobilePush SDK. It is meant to handle a lot of the heavy lifting with regards to sending data back to ExactTarget.
	/// Please note that this is a singleton object, and you should reference it as [ETPush pushManager].
	/// </summary>
	[BaseType(typeof(NSObject))]
	[DisableDefaultCtor]
	public partial interface ETPush
	{
		// NSDate *_sessionStart;
		// NSString *_messageID;

		// BOOL _showLocalAlert;

		// // OpenDirect Delegate stuff
		// id<ExactTargetOpenDirectDelegate> _openDirectDelegate;

		#region Configure the app for ETPush

		/// <summary>
		/// Returns (or initializes) the shared pushManager instance.
		/// </summary>
		/// <value>
		/// The singleton instance of an ETPush pushManager.
		/// </value>
		[Static, Export("pushManager")]
		ETPush PushManager { get; }

		/// <summary>
		/// Returns (or initializes) the shared pushManager instance.
		/// </summary>
		/// <value>
		/// The singleton instance of an ETPush pushManager.
		/// </value>
		//[Export("init")]
		//IntPtr Constructor();

		/// <summary>
		/// This is the former main configuration for the MobilePush SDK. As of version 3.0, it is succeeded by configureSDKWithAppID:andAccessToken:withAnalytics:andLocationServices:andCloudPages:. It will continue to function, but calls it's successor with YES for all parameters. This may provide undesired results, so you are encouraged to switch your configuration method to the new one in your next release.
		/// </summary>
		/// <param name="etAppID">The App ID generated by Code@ExactTarget to identify the consumer app.</param>
		/// <param name="accessToken">The designed token given to you by Code@ExactTarget that allows you access to the API.</param>
		[Obsolete]
		[Export("configureSDKWithAppID:andAccessToken:")]
		void ConfigureSDKWithAppID(string etAppID, string accessToken);

		/// <summary>
		/// This is the main configuration method, responsible for setting credentials needed to communicate with ExactTarget. If you are unsure of your accessToken or environment, please visit Code@ExactTarget
		/// 
		/// Each of the flags in the method are used to control various aspects of the MobilePush SDK. The act as global on/off switches, meaning that if you disable one here, it is off eveywhere.
		/// </summary>
		/// <param name="etAppID">The App ID generated by Code@ExactTarget to identify the consumer app.</param>
		/// <param name="accessToken">The designed token given to you by Code@ExactTarget that allows you access to the API.</param>
		/// <param name="analyticsState">Whether or not to send analytic data back to ExactTarget.</param>
		/// <param name="locState">Whether or not to use Location Services.</param>
		/// <param name="cpState">Whether or not to use Cloud Pages.</param>
		[Export("configureSDKWithAppID:andAccessToken:withAnalytics:andLocationServices:andCloudPages:")]
		void ConfigureSDKWithAppID(string etAppID, string accessToken, bool analyticsState, bool locState, bool cpState);

		/// <summary>
		/// Sets the OpenDirect delegate.
		/// </summary>
		/// <param name="delegate_">The object you wish to be called when an OpenDirect message is delivered.</param>
		[Export("setOpenDirectDelegate:")]
		void setOpenDirectDelegate(ExactTargetOpenDirectDelegate delegate_);

		/// <summary>
		/// Returns the OpenDirect delegate.
		/// </summary>
		/// <value>
		/// The open direct delegate.
		/// </value>
		[Export("openDirectDelegate")]
		ExactTargetOpenDirectDelegate OpenDirectDelegate { get; }

		/// <summary>
		/// Triggers a data send to ExactTarget. Mostly used internally, and rarely should be called by client code.
		/// </summary>
		[Export("updateET")]
		void UpdateET();

		#endregion

		// These methods are required to make push function on iOS, and to enable the ET SDK to utilize it. All of these methods are required.
		#region Push Lifecycle
		// Refer to Availability.h for the reasoning behind why the following #if's are used. Basically, this will allow the code to be compiled for different IPHONEOS_DEPLOYMENT_TARGET values to
		// maintain backward compatibility for running on IOS 6.0 and up as well allowing for using different versions of the IOS SDK compiled using XCode 5.X, XCode 6.X and up without getting depricated warnings or undefined warnings.

		#if true

		/// <summary>
		/// Wrapper for iOS' application:registerForRemoteNotification; call. It will check that push is allowed, and if so, register with Apple for a token. If push is not enabled, it will notify ExactTarget that push is disabled.
		/// </summary>
		[Export("registerForRemoteNotifications")]
		void RegisterForRemoteNotifications();

		/// <summary>
		/// Wrapper for iOS' isRegisteredForRemoteNotifications; call.
		/// </summary>
		/// <value>
		/// <c>true</c> if [is registered for remote notifications]; otherwise, <c>false</c>.
		/// </value>
		[Export("isRegisteredForRemoteNotifications")]
		bool IsRegisteredForRemoteNotifications { get; }

		/// <summary>
		/// Wrapper for iOS' application:registerUserNotificationSettings; call.
		/// </summary>
		/// <param name="notificationSettings">The UIUserNotificationSettings object that the application would like to use for push. These are pipe-delimited, and use Apple's native values.</param>
		[Export("registerUserNotificationSettings:")]
		void RegisterUserNotificationSettings(UIUserNotificationSettings notificationSettings);

		/// <summary>
		/// Wrapper for iOS' currentUserNotificationSettings; call.
		/// </summary>
		/// <value>
		/// The current user notification settings.
		/// </value>
		[Export("currentUserNotificationSettings")]
		UIUserNotificationSettings CurrentUserNotificationSettings { get; }

		/// <summary>
		/// Wrapper for iOS' didRegisterUserNotificationSettings; callback.
		/// </summary>
		/// <param name="notificationSettings">The notification settings.</param>
		[Export("didRegisterUserNotificationSettings:")]
		void DidRegisterUserNotificationSettings(UIUserNotificationSettings notificationSettings);

		#endif

		/// <summary>
		/// Wrapper for iOS' application:registerForRemoteNotificationTypes; call. It will check that push is allowed, and if so, register with Apple for a token. If push is not enabled, it will notify ExactTarget that push is disabled.
		/// </summary>
		/// <param name="types">The UIRemoteNotificationTypes that the application would like to use for push. These are pipe-delimited, and use Apple's native values.</param>
		[Export("registerForRemoteNotificationTypes:")]
		void RegisterForRemoteNotificationTypes(UIRemoteNotificationType types);

		/// <summary>
		/// Responsible for sending a received token back to ExactTarget. It marks the end of the token registration flow. If it is unable to reach ET server, it will save the token and try again later. 
		/// 
		/// This method is necessary to implementation of ET Push.
		/// </summary>
		/// <param name="deviceToken">Token as received from Apple, still an NSData object.</param>
		[Export("registerDeviceToken:")]
		void RegisterDeviceToken(NSData deviceToken);

		/// <summary>
		/// Returns the device token as a NSString. As requested via GitHub (Issue #3).
		/// </summary>
		/// <value>
		/// A stringified version of the Device Token.
		/// </value>
		[Export("deviceToken")]
		string DeviceToken { get; }

		/// <summary>
		/// Handles a registration failure.
		/// </summary>
		/// <param name="error">The error returned to the application on a registration failure.</param>
		[Export("applicationDidFailToRegisterForRemoteNotificationsWithError:")]
		void ApplicationDidFailToRegisterForRemoteNotificationsWithError(NSError error);

		/// <summary>
		/// Reset the application's badge number to zero (aka, remove it) and let the push servers know that it should zero the count.
		/// </summary>
		[Export("resetBadgeCount")]
		void ResetBadgeCount();

		/// <summary>
		/// Tell the SDK to display a UIAlertView if a push is received while the app is already running. Default behavior is set to NO.
		/// 
		/// Please note that all push notifications received by the application will be processed, but iOS will *not* present an alert to the user if the app is running when the alert is received. If you set this value to true (YES), then the SDK will generate and present the alert for you. It will not play a sound, though.
		/// </summary>
		/// <param name="desiredState">YES/NO if you want to display an alert view while the app is running.</param>
		[Export("shouldDisplayAlertViewIfPushReceived:")]
		void ShouldDisplayAlertViewIfPushReceived(bool desiredState);

		#endregion

		// These methods are not necessary for the Push lifecycle, but are required to make the ET Push SDK perform as expected
		#region Application Lifecycle

		/// <summary>
		/// Notifies the ET SDK of an app launch, including the dictionary sent to the app by iOS. The launchOptions dictionary is necessary because it will include the APNS dictionary, necessary for processing opens and other analytic information. 
		/// </summary>
		/// <param name="launchOptions">The dictionary passed to the application by iOS on launch.</param>
		[Export("applicationLaunchedWithOptions:")]
		void ApplicationLaunchedWithOptions(NSDictionary launchOptions);

		/// <summary>
		/// Notifies the ET SDK of an app termination. Internally, this method does a lot of cleanup. 
		/// </summary>
		[Export("applicationTerminated")]
		void ApplicationTerminated();

		/// <summary>
		/// Handles a push notification received by the app when the application is already running. 
		/// 
		/// This method must be implemented in [[UIApplication sharedApplication] didReceiveRemoteNotification:userInfo].
		/// 
		/// Sometimes, when a push comes in, the application will already be running (it happens). This method rises to the occasion of handing that notification, displaying an Alert (if the SDK is configured to do so), and calling all of the analytic methods that wouldn't be called otherwise. 
		/// </summary>
		/// <param name="userInfo">The dictionary containing the push notification.</param>
		/// <param name="applicationState">State of the application at time of notification.</param>
		[Export("handleNotification:forApplicationState:")]
		void HandleNotification(NSDictionary userInfo, UIApplicationState applicationState);

		/// <summary>
		/// Handles a local notification received by the application. 
		/// 
		/// Sometimes the SDK will use local notifications to indicate something to the user. These are handled differently by iOS, and as such, need to be implemented differently in the SDK. Sorry about that. 
		/// </summary>
		/// <param name="notification">The received UILocalNotification.</param>
		[Export("handleLocalNotification:")]
		void HandleLocalNotification(UILocalNotification notification);

		#endregion

		#region Data Interaction

		/// <summary>
		/// Accepts and sets the Subscriber Key for the device's user.
		/// </summary>
		/// <param name="subscriberKey">The subscriber key to attribute to the user.</param>
		[Export("setSubscriberKey")]
		void SetSubscriberKey(string subscriberKey);

		/// <summary>
		/// Returns the subscriber key for the active user, in case you need it.
		/// </summary>
		/// <value>
		/// The get subscriber key.
		/// </value>
		[Export("getSubscriberKey")]
		string GetSubscriberKey { get; }

		/// <summary>
		/// Adds the tag.
		/// </summary>
		/// <param name="tag">A string to add to the list of tags.</param>
		[Export("addTag:")]
		void AddTag(string tag);

		/// <summary>
		/// Removes the provided Tag (NSString) from the list of tags.
		/// </summary>
		/// <param name="tag">A string to remove from the list of tags.</param>
		/// <returns>Echoes the tag back on successful removal, or nil if something failed.</returns>
		[Export("removeTag:")]
		string RemoveTag(string tag);

		/// <summary>
		/// Returns the list of tags for this device.
		/// </summary>
		/// <value>
		/// All tags.
		/// </value>
		[Export("allTags")]
		NSSet AllTags { get; }

		/// <summary>
		/// Adds an attribute to the data set sent to ExactTarget.
		/// </summary>
		/// <param name="name">The name of the attribute you wish to send. This will be the key of the pair..</param>
		/// <param name="value">The value to set for thid data pair.</param>
		[Export("addAttributeNamed:value:")]
		void AddAttributeNamed(string name, string value);

		/// <summary>
		/// Removes the provided attributef rom the data set to send to ExactTarget.
		/// </summary>
		/// <param name="name">The name of the attribute you wish to remove. .</param>
		/// <returns>Returns the value that was set. It will no longer be sent back to ExactTarget.</returns>
		[Export("removeAttributeNamed:")]
		string RemoveAttributeNamed(string name);

		/// <summary>
		/// Returns a read-only copy of the Attributes dictionary as it is right now. 
		/// </summary>
		/// <value>
		/// All attributes currently set.
		/// </value>
		[Export("allAttributes")]
		NSDictionary AllAttributes { get; }

		#endregion

		#region ETPush Convenience Methods

		/// <summary>
		/// Gets the Apple-safe, unique Device Identifier that ET will later use to identify the device.
		/// 
		/// Note that this method is compliant with Apple's compliance rules, but may not be permanent.
		/// </summary>
		/// <value>
		/// The safe device identifier.
		/// </value>
		[Static, Export("safeDeviceIdentifier")]
		string SafeDeviceIdentifier { get; }

		/// <summary>
		/// Returns the hardware identification string, like "iPhone1,1". ExactTarget uses this data for segmentation. 
		/// </summary>
		/// <value>
		/// A string of the hardware identification.
		/// </value>
		[Static, Export("hardwareIdentifier")]
		string HardwareIdentifier { get; }

		/// <summary>
		/// Returns the state of Push based on logic reflected at ExactTarget. 
		/// </summary>
		/// <value>
		///   As of this release, Push is considered enabled if the application is able to present an alert (banner, alert) to the user per Settings. Nothing else will be considered.
		/// </value>
		[Static, Export("isPushEnabled")]
		bool IsPushEnabled { get; }

		#endregion

		#region Listeners for UIApplication events

		/// <summary>
		/// Sets up the listeners.
		/// </summary>
		[Export("startListeningForApplicationNotifications")]
		void StartListeningForApplicationNotifications();

		/// <summary>
		/// Drops the listeners.
		/// </summary>
		[Export("stopListeningForApplicationNotifications")]
		void StopListeningForApplicationNotifications();

		/// <summary>
		/// Responds to the UIApplicationDidBecomeActiveNotification notification
		/// </summary>
		[Export("applicationDidBecomeActiveNotificationReceived")]
		void ApplicationDidBecomeActiveNotificationReceived(); // UIApplicationDidBecomeActiveNotification

		/// <summary>
		/// Responds to the UIApplicationDidEnterBackgroundNotification notification
		/// </summary>
		[Export("applicationDidEnterBackgroundNotificationReceived")]
		void ApplicationDidEnterBackgroundNotificationReceived(); // UIApplicationDidEnterBackgroundNotification

		/// <summary>
		/// Set the Log Level
		/// </summary>
		/// <param name="state">if set to <c>true</c> [state].</param>
		[Static, Export("setETLoggerToRequiredState:")]
		void setETLoggerToRequiredState(bool state);

		/// <summary>
		/// To Log the string whenever [ETPush setETLoggerToState:YES]
		/// </summary>
		/// <param name="stringToBeLogged">The string to be logged.</param>
		[Static, Export("ETLogger:")]
		void setETLoggerToRequiredState(string stringToBeLogged);

		#endregion
	}

	/// <summary>
	/// ETRegion is a representation of a physical region that should trigger a message to be presented to the user. This could be either macro, a geofence, or micro, an iBeacon (marketing name: Proximity), but both go through iOS' Location Manager, and are reported back to the SDK through the CLLocationManagerDelegate (which is currently always ETLocationManager).
	/// Geofences will have a latitude and longitude and radius, but will be notably absent of a proximity UUID, major and minor number. Beacons also have latitude and longitude (but no radius) because of a decision to track their physical location in the world, but it is inconsequential to functionality. A beacon is functional because of the Proximity UUID. Major and Minor number. The three of these uniquely identify a physical beacon. Per Apple's best practices (WWDC 2013), a single UUID should be used commonly amongst an entire Enterprise (example: All Starbucks locations share a UUID). Major numbers should designate a single location within the UUID (Starbucks #1234, 15th and College, Indianapolis), and a Minor number can indicate the beacon within the location designated in the Major (Point of Sale). ExactTarget suggests following this pattern when configuring beacons.
	/// ETRegions will has a zero-to-many relationship with ETMessage, which in turn, has a zero-to-many relationship with ETRegion. In plain English, one region can have many messages, and one message can belong to many regions. This is handled through ETFenceMessage.
	/// </summary>
	[BaseType(typeof(ETGenericUpdate))]
	[DisableDefaultCtor]
	public partial interface ETRegion
	{
		/// <summary>
		/// ET-generated identifier for the ETRegion in question. This should be treated as a primary key, and is stored on the device as the encoded version sent via the routes.
		/// </summary>
		/// <value>
		/// The fence identifier.
		/// </value>
		[Export("fenceIdentifier")]
		string FenceIdentifier { get; set; }

		/// <summary>
		/// The latitude of this region. Saved in an NSNumber as a double for easy passing. Be sure to call doubleValue on this property.
		/// </summary>
		/// <value>
		/// The latitude.
		/// </value>
		[Export("latitude")]
		double Latitude { get; set; }

		/// <summary>
		/// The longitude of this region. Saved in an NSNumber as a double for easy passing. Be sure to call doubleValue on this property.
		/// </summary>
		/// <value>
		/// The longitude.
		/// </value>
		[Export("longitude")]
		double Longitude { get; set; }

		/// <summary>
		/// For geofences only, the radius of the fence. This number, an integer, is in meters. 
		/// </summary>
		/// <value>
		/// The radius.
		/// </value>
		[Export("radius")]
		int Radius { get; set; }

		/// <summary>
		/// An array of related messages. It is not proper to pull a message out of this array, though. It's used for initialization and data passing.
		/// </summary>
		/// <value>
		/// The messages.
		/// </value>
		[Export("messages")]
		NSMutableArray Messages { get; set; }

		/// <summary>
		/// For beacons, the Proximity UUID of the beacon. 
		/// </summary>
		/// <value>
		/// The proximity UUID.
		/// </value>
		[Export("proximityUUID")]
		string ProximityUUID { get; set; }

		/// <summary>
		/// For beacons, the Major number. This is a uint32 per the CLBeaconRegion spec.
		/// </summary>
		/// <value>
		/// The major number.
		/// </value>
		[Export("majorNumber")]
		uint MajorNumber { get; set; }

		/// <summary>
		/// For beacons, the Minor number. This is a uint32 per the CLBeaconRegion spec.
		/// </summary>
		/// <value>
		/// The minor number.
		/// </value>
		[Export("minorNumber")]
		uint MinorNumber { get; set; }

		/// <summary>
		/// This is the number of times a region is entered as counted by the ETLocationManager. 
		/// </summary>
		/// <value>
		/// The entry count.
		/// </value>
		[Export("entryCount")]
		int EntryCount { get; set; }

		/// <summary>
		/// This is the number of times a region is exited as counted by the ETLocationManager. Ideally, it matches enter count.
		/// </summary>
		/// <value>
		/// The exit count.
		/// </value>
		[Export("exitCount")]
		int ExitCount { get; set; }

		/// <summary>
		/// This is the name which is set on ExactTargetMarketingCloud, while setting the ETRegion
		/// </summary>
		/// <value>
		/// The name of the region.
		/// </value>
		[Export("regionName")]
		string RegionName { get; set; }

		/// <summary>
		/// The type of region this ETRegion represents. This is sent from the server, so we trust it. 
		/// </summary>
		/// <value>
		/// The type of the location.
		/// </value>
		[Export("locationType")]
		MobilePushGeofenceType LocationType { get; set; }

		/// <summary>
		/// For ETGenericUpdate, this is the type of request that is being initiated. It will soon be deprecated.
		/// </summary>
		/// <value>
		/// The type of the request.
		/// </value>
		[Export("requestType")]
		ETRegionRequestType RequestType { get; set; }

		/// <summary>
		/// This is the designated initializer. Pass it in a dictionary, get out an ETRegion. 
		/// </summary>
		/// <param name="dict">NSDictionary of values of which to apply to this ETRegion.</param>
		/// <returns>A newly minted ETRegion.</returns>
		[Export("initFromDictionary:")]
		IntPtr Constructor(NSDictionary dict);

		/// <summary>
		/// Region equality. Based on the kind of ETRegion, it will compare values and determine equality.
		/// </summary>
		/// <param name="region">The other ETRegion to which the comparison should be made.</param>
		/// <returns>T/F of equality.</returns>
		[Export("isEqualToRegion:")]
		bool isEqualToRegion(ETRegion region);

		/// <summary>
		/// This returns a CLLocation representation of the current ETRegion, or nil if a beacon.
		/// </summary>
		/// <value>
		/// The region as a CLLocation.
		/// </value>
		[Export("regionAsLocation")]
		CLLocation RegionAsLocation { get; } // For Geofences

		/// <summary>
		/// Returns the ETRegion as a CLRegion for use in Beacon code, or nil if a Geofence.
		/// </summary>
		/// <value>
		/// A CLRegion representation of self.
		/// </value>
		[Export("regionAsCLRegion")]
		CLRegion RegionAsCLRegion { get; }  // For Beacons

		/// <summary>
		/// Returns self as a CLBeaconRegion, or nil if a Geofence.
		/// </summary>
		/// <value>
		/// CLBeaconRegion representation of self.
		/// </value>
		[Export("regionAsBeaconRegion")]
		CLBeaconRegion RegionAsBeaconRegion { get; }  // Also for beacons

		/// <summary>
		/// Helper to quickly determine if this is a Beacon/Proximity region.
		/// </summary>
		/// <value>
		///   T/F if a beacon.
		/// </value>
		[Export("isGeofenceRegion")]
		bool IsGeofenceRegion { get; }

		/// <summary>
		/// Returns a specific ETRegion by the ET-provided identifier, or nil if it doesn't exist. 
		/// </summary>
		/// <param name="identifier">The region ID to retrieve.</param>
		/// <returns>The region, or nil if it doesn't exist.</returns>
		[Static, Export("getRegionByIdentifier:")]
		ETRegion GetRegionByIdentifier(string identifier);

		/// <summary>
		/// Gets the beacon region for region with proximity UUID.
		/// </summary>
		/// <param name="proximityUUID">The ranged beacon UUID.</param>
		/// <param name="majorNumber">An NSNumber-wrapped uint32 of the beacon's major number.</param>
		/// <param name="minorNumber">An NSNumber-wrapped uint32 of the beacon's minor number.</param>
		/// <returns>The region, or nil if it doesn't exist. </returns>
		[Static, Export("getBeaconRegionForRegionWithProximityUUID:andMajorNumber:andMinorNumber:")]
		ETRegion GetBeaconRegionForRegionWithProximityUUID(string proximityUUID, uint majorNumber, uint minorNumber);

		/// <summary>
		/// Pulls all active regions out of the local database.
		/// </summary>
		/// <returns>An NSSet of ETRegions.</returns>
		[Static, Export("getFencesFromCache")]
		NSSet GetFencesFromCache();

		/// <summary>
		/// Pulls all active regions out of the local database.
		/// </summary>
		/// <returns></returns>
		/// <value>
		/// An NSSet of ETRegions.
		/// </value>
		[Static, Export("getFencesFromCacheIncludingInactive:")]
		NSSet GetFencesFromCacheIncludingInactive(bool getInactive);

		/// <summary>
		/// Marks all regions in the database for a specific type as inactive. This is done after successfully retrieving fences from ExactTarget.
		/// </summary>
		/// <param name="requestType">Filter to only invalidate certain types of region based on ET Request.</param>
		/// <returns>T/F if the process was successful.</returns>
		[Static, Export("invalidateAllRegionsForRequestType:")]
		bool InvalidateAllRegionsForRequestType(ETRegionRequestType requestType);

		/// <summary>
		/// Invalidates all reason. Full stop.
		/// </summary>
		/// <returns>T/F of the success of the operation.</returns>
		[Static, Export("invalidateAllRegions:")]
		bool InvalidateAllRegions();

		/// <summary>
		/// Begins fence retrieval from ET of Geofences. 
		/// </summary>
		[Static, Export("retrieveGeofencesFromET")]
		void RetrieveGeofencesFromET();

		/// <summary>
		/// Begins fence retrieval from ET of Beacons.
		/// </summary>
		[Static, Export("retrieveProximityFromET")]
		void RetrieveProximityFromET();

		/// <summary>
		/// Generates the persistent data schema in database.
		/// </summary>
		/// <returns></returns>
		[Static, Export("generatePersistentDataSchemaInDatabase")]
		bool GeneratePersistentDataSchemaInDatabase();
	}

	/// <summary>
	/// ETRegistration sends data about the device back to ExactTarget. The data in here covers a few different pieces, from things necessary to make the wheels spin (app id, access token, device id, token, etc) to extra things used for segmentation but not necessarily required (attributes, tags). 
	/// 
	/// ETRegistrations are generated nearly completely programmatically, so you just have to make one and throw it at the server. They are not saved to the database because of this - the values should not change over time, and if they're new later, the old values don't matter.
	/// </summary>
	[BaseType(typeof(ETGenericUpdate))]
	//[DisableDefaultCtor]
	public partial interface ETRegistration
	{
		/// <summary>
		/// Makes a new Registration Update object. One per session.
		/// </summary>
		/// <returns></returns>
		//[Export("init")]
		//IntPtr Constructor();
	}

	/// <summary>
	/// This is a lightweight SQLite wrapper that handles database interaction. It is only used privately, and shouldn't be used by others.
	/// Most of the methods are self-explainatory, so this class isn't heavily documented.
	/// </summary>
	[BaseType(typeof(NSObject))]
	//[DisableDefaultCtor]
	public partial interface ETSqliteHelper
	{
		// sqlite3 *_db;

		/// <summary>
		/// Constructors this instance.
		/// </summary>
		/// <returns></returns>
		//[Export("init")]
		//IntPtr Constructor();

		/// <summary>
		/// Gets or sets the max retries.
		/// </summary>
		/// <value>
		/// The max retries.
		/// </value>
		[Export("maxRetries")]
		nint MaxRetries { get; set; }

		/// <summary>
		/// Gets the max retries.
		/// </summary>
		/// <value>
		/// The max retries.
		/// </value>
		[Export("database")]
		ETSqliteHelper Database { get; }

		/// <summary>
		/// Opens this instance.
		/// </summary>
		/// <returns></returns>
		[Export("open")]
		bool Open();

		/// <summary>
		/// Closes this instance.
		/// </summary>
		[Export("close")]
		void Close();

		/// <summary>
		/// Gets the last error message.
		/// </summary>
		/// <value>
		/// The last error message.
		/// </value>
		[Export("lastErrorMessage")]
		string LastErrorMessage { get; }

		/// <summary>
		/// Gets the last error code.
		/// </summary>
		/// <value>
		/// The last error code.
		/// </value>
		[Export("lastErrorCode")]
		nint LastErrorCode { get; }

		/// <summary>
		/// Gets the rows affected.
		/// </summary>
		/// <value>
		/// The rows affected.
		/// </value>
		[Export("rowsAffected")]
		nint RowsAffected { get; }

		/// <summary>
		/// Executes the query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		//[Obsolete]
		//[Export("executeQuery:")]
		//NSObject[] ExecuteQuery(string sql, params object[] args);

		/// <summary>
		/// Executes the query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		[Export("executeQuery:arguments:")]
		NSObject[] ExecuteQuery(string sql, params NSObject[] args);

		/// <summary>
		/// Executes the query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		//[Obsolete]
		//[Export("executeUpdate:")]
		//bool ExecuteUpdate(string sql, params object[] args);

		/// <summary>
		/// Executes the query.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns></returns>
		[Export("executeUpdate:arguments:")]
		bool ExecuteUpdate(string sql, params NSObject[] arguments);

		/// <summary>
		/// Tables the exists.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns></returns>
		[Export("tableExists:")]
		bool TableExists(string tableName);

		/// <summary>
		/// Begins the transaction.
		/// </summary>
		/// <returns></returns>
		[Export("beginTransaction")]
		bool BeginTransaction();

		/// <summary>
		/// Commits the transaction.
		/// </summary>
		/// <returns></returns>
		[Export("commitTransaction")]
		bool CommitTransaction();

		/// <summary>
		/// Rollbacks the transaction.
		/// </summary>
		/// <returns></returns>
		[Export("rollbackTransaction")]
		bool RollbackTransaction();
	}

	/// <summary>
	/// ETStatsUpdate
	/// </summary>
	[BaseType(typeof(ETGenericUpdate), Delegates = new[] { "Delegate" }, Events = new[] { typeof(NSCopying) })]
	[DisableDefaultCtor]
	public partial interface ETStatsUpdate
	{
		/// <summary>
		/// Time in app, in seconds
		/// </summary>
		/// <value>
		/// The seconds in app.
		/// </value>
		[Export("secondsInApp")]
		nint SecondsInApp { get; set; }

		/// <summary>
		/// From ET, the message identifier.
		/// </summary>
		/// <value>
		/// The message id.
		/// </value>
		[Export("messageID")]
		string MessageID { get; set; }

		/// <summary>
		/// Which fence got broken
		/// </summary>
		/// <value>
		/// The fence id.
		/// </value>
		[Export("fenceID")]
		string FenceID { get; set; }

		/// <summary>
		/// For local notifications, this is shown when they fire. ie, firedate
		/// </summary>
		/// <value>
		/// The display date.
		/// </value>
		[Export("displayDate")]
		NSDate DisplayDate { get; set; }

		/// <summary>
		/// Generic open date, maybe null usually, since no one opens our apps
		/// </summary>
		/// <value>
		/// The open date.
		/// </value>
		[Export("openDate")]
		NSDate OpenDate { get; set; }

		/// <summary>
		/// Gets or sets the type of the message.
		/// </summary>
		/// <value>
		/// The type of the message.
		/// </value>
		[Export("messageType")]
		MobilePushMessageType MessageType { get; set; }
	}

	/// <summary>
	/// ETTestsHelper
	/// </summary>
	[BaseType(typeof(NSObject))]
	[DisableDefaultCtor]
	public partial interface ETTestsHelper
	{
		/// <summary>
		/// Tests the help.
		/// </summary>
		/// <returns></returns>
		[Static, Export("testHelp")]
		ETTestsHelper TestHelp();

		/// <summary>
		/// Clears the database if exists.
		/// </summary>
		[Export("clearDatabaseIfExists")]
		void ClearDatabaseIfExists();

		/// <summary>
		/// Enables the poll.
		/// </summary>
		[Export("enablePoll")]
		void EnablePoll();

		/// <summary>
		/// Disables the poll.
		/// </summary>
		[Export("disablePoll")]
		void DisablePoll();

		/// <summary>
		/// Does the additional poll.
		/// </summary>
		[Export("doAdditionalPoll")]
		void DoAdditionalPoll();
	}

	/// <summary>
	/// ETURLConnection is a wrapper around vanilla NSURLConnections that is useful because it adds things that Apple should have (tags) or things needed for ETPhoneHome to work correctly (reference to the sending object. Otherwise, it's just a regular NSURLConnection.
	/// </summary>
	[BaseType(typeof(NSUrlConnection))]
	[DisableDefaultCtor]
	public partial interface ETURLConnection
	{
		/// <summary>
		/// The tag of this particular connection. Usually the BackgroundTaskID from iOS.
		/// </summary>
		/// <value>
		/// The tag.
		/// </value>
		[Export("tag")]
		int Tag { get; set; }

		/// <summary>
		/// A reference to the sendingObject for this connection. That object will save the response data and status code, etc. This allows us to fire off a bunch of these things in parallel. 
		/// </summary>
		/// <value>
		/// The sending object.
		/// </value>
		[Export("sendingObject")]
		ETGenericUpdate SendingObject { get; set; }
	}

	/// <summary>
	/// The ExactTargetEnhancedPushDataSource is an interface object for CloudPage support. It was designed to be used as the datasource for a UITableView, and can be allocated and used as such without too much other customization. Of course, you are welcomed to use it any way you want other than that.
	/// 
	/// Should you wish to customize the display of the Data Source, you should subclass from here. At that time, you may override any typical UITableViewDataSource protocol members. You will likely be the most interested in cellForRowAtIndexPath:. If you do, you can access the current message by asking the messages array for the object corresponding to your NSIndexPath row. It will be an ETMessage object. 
	/// 
	/// Or, for the most customization, make a new one of these and only access the messages property. If you do that, you'll need to be both the delegate and data source for your table, but you can do whatever you like. The messages array will contain ETMessage objects, and you can see which properties are available on that by checking it's header.
	/// </summary>
	[BaseType(typeof(UITableViewDataSource), Delegates = new[] { "Delegate" }, Events = new[] { typeof(UITableViewDataSource) })]
	[DisableDefaultCtor]
	public partial interface ExactTargetEnhancedPushDataSource
	{
		/// <summary>
		/// This array contains ETMessages, suitable for display in a UITableView or other presentation apparatus of your liking. Please see ETMessage for a list of properties available.
		/// </summary>
		/// <value>
		/// The messages.
		/// </value>
		[Export("messages")]
		NSObject[] Messages { get; set; }

		/// <summary>
		/// This is a reference to the tableview in your UIViewController. We need a reference to it to reload data periodically.
		/// </summary>
		/// <value>
		/// The inbox table view.
		/// </value>
		[Export("inboxTableView")]
		UITableView InboxTableView { get; set;}
	}
}
