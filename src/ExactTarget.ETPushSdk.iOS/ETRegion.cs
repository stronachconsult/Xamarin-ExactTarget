using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
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
}
