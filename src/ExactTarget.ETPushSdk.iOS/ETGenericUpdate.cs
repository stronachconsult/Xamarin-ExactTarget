using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
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
        int DatabaseIdentifier { get; set; }

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
}
