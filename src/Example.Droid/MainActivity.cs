using Android.App;
using Android.OS;
using Android.Util;
using Example.Droid;
//using ExactTarget.ETPushSdk;
//using ExactTarget.ETPushSdk.Data;
//using ExactTarget.ETPushSdk.Event;
//using ExactTarget.ETPushSdk.Util;

namespace Example.Android
{
    [Activity(Label = "Example", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly string TAG = "MainActivity";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Log.Info(TAG, "MAINACTIVITY MAINACTIVITY MAINACTIVITY");
            //EventBus.Default.Register(this);
            //try
            //{
            //    if (ETPush.PushManager().IsPushEnabled)
            //        ETPush.PushManager().EnablePush();
            //}
            //catch (ETException e) { if (ETPush.LogLevel <= LogPriority.Error) Log.Error(TAG, e.Message, e); }

            SetContentView(Resource.Layout.Main);
        }

        protected override void OnDestroy()
        {
            //EventBus.Default.Unregister(this);
            base.OnDestroy();
        }

        protected override void OnResume()
        {
            base.OnResume();
            //try { ETPush.PushManager().ActivityResumed(this); }
            //catch (ETException e) { if (ETPush.LogLevel <= LogPriority.Error) Log.Error(TAG, e.Message, e); }
        }

        protected override void OnPause()
        {
            base.OnPause();
            //try { ETPush.PushManager().ActivityPaused(this); }
            //catch (Exception e) { if (ETPush.LogLevel <= LogPriority.Error) Log.Error(TAG, e.Message, e); }
        }

        #region Events

        //public void OnEvent(RegistrationEvent e)
        //{
        //    Log.Info(TAG, "Registration occurred.  You could now save registration details in your own data stores...");
        //    Log.Info(TAG, "Device ID:" + e.DeviceId);
        //    Log.Info(TAG, "Device Token:" + e.DeviceToken);
        //    Log.Info(TAG, "Subscriber key:" + e.SubscriberKey);
        //}

        //public void OnEvent(ServerErrorEvent e)
        //{
        //    var errorJson = JSONUtil.ObjectToJson(e);
        //    Log.Error(TAG, "ServerErrorEvent: " + errorJson);
        //}

        //public void OnEvent(CloudPagesResponse e)
        //{
        //    Log.Info(TAG, "CloudPages returned...");
        //    Log.Info(TAG, "Num messages: " + e.Messages.Count);
        //}

        #endregion
    }
}
