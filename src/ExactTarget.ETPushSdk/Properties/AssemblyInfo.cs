using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;
using Android.Content.PM;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ExactTarget.ETPushSdk.Droid")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("ExactTarget.ETPushSdk.Droid")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Permissions for Amazon Device Messaging
[assembly: Permission(Name = Amazon.Device.Messaging.Constants.PackageNameReceivePermission, ProtectionLevel = Protection.Signature)]
[assembly: UsesPermission(Amazon.Device.Messaging.Constants.PackageNameReceivePermission)]
[assembly: UsesPermission(Amazon.Device.Messaging.Constants.ReceivePermission)]
[assembly: UsesFeature(Amazon.Device.Messaging.Constants.AmazonDeviceMessagingFeature, Required = true)]
// Permissions for Google Play Services
[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE", ProtectionLevel = Protection.Signature)]
[assembly: UsesPermission("@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission("com.google.android.c2dm.permission.RECEIVE")]
// Required permissions for ETPush
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.GetAccounts)]
[assembly: UsesPermission(Android.Manifest.Permission.WakeLock)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessWifiState)]