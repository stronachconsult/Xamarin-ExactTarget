using System;
using ObjCRuntime;

[assembly: LinkWith ("libetpushsdk-3.3.0.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator | LinkTarget.Simulator64 | LinkTarget.Arm64, SmartLink = true, ForceLoad = true)]
