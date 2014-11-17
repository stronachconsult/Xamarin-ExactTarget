using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith("libetpushsdk-3.3.0.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, ForceLoad = true, Frameworks = "MobileCoreServices Security CoreLocation CoreTelephony SystemConfiguration")]
[assembly: LinkWith("libsqlite3.dylib", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, ForceLoad = true)]
