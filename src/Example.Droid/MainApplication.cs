using Android.App;
using Android.Runtime;
using Android.Util;
using ExactTarget.ETPushSdk;
using System;

namespace Example.Android
{
    [Application]
    public class MainApplication : Application
    {
        static readonly string TAG = "MainApplication";

        public MainApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Info(TAG, "START START START START START START START START");

            ETPush.LogLevel = LogPriority.Debug;
            ETPush.ReadyAimFire(this, "dc350e01-fdb6-4281-8d6d-b9eafa10d088", "4drqxbywnrxrz3u6wf5czuwt", false, false, false);
            Log.Info(TAG, "END END END END END END END END END END END END");

            //try
            //{
            //    ETPush.LogLevel = LogPriority.Debug;
            //    ETPush.ReadyAimFire(this, "dc350e01-fdb6-4281-8d6d-b9eafa10d088", "4drqxbywnrxrz3u6wf5czuwt", false, false, false);
            //    var pushManager = ETPush.PushManager();
            //    pushManager.SetGcmSenderID("211878710923");
            //    var versionName = PackageManager.GetPackageInfo(PackageName, 0).VersionName;
            //    pushManager.AddTag(versionName);
            //}
            //catch (ETException e) { if (ETPush.LogLevel <= LogPriority.Error) Log.Error(TAG, e.Message, e); }
            //catch (PackageManager.NameNotFoundException e) { Log.Error(TAG, e.Message, e); }
        }
    }
}

