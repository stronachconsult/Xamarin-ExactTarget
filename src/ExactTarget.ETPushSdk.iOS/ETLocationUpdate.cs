using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
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
}
