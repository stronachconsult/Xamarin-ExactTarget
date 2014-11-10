using Android.App;
using Android.Content;
using Android.Support.V4.Content;
namespace ExactTarget.ETPushSdk
{
    [BroadcastReceiver(Name = "com.exacttarget.etpushsdk.ET_GenericReceiver", Permission = "com.google.android.c2dm.permission.SEND", Exported = true)]
    [IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE", "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new[] { "@PACKAGE_NAME@" })]    
    partial class ET_GenericReceiver : BroadcastReceiver { }

    //[BroadcastReceiver(Name = "com.exacttarget.etpushsdk.ET_GenericReceiver", Permission = Amazon.Device.Messaging.Constants.SendPermission, Exported = true)]
    //[IntentFilter(new[] { Amazon.Device.Messaging.Constants.ReceiveIntent, Amazon.Device.Messaging.Constants.RegistrationIntent }, Categories = new[] { Amazon.Device.Messaging.Constants.PackageName })]
    //partial class ET_GenericReceiver2 : BroadcastReceiver
    //{
    //    public override void OnReceive(Context context, Intent intent) { }
    //}

    [Service(Name = "com.exacttarget.etpushsdk.ETSendDataIntentService", Exported = true)]
    partial class ETSendDataIntentService : IntentService { }

    [BroadcastReceiver(Name = "com.exacttarget.etpushsdk.ETSendDataReceiver", Exported = true)]
    partial class ETSendDataReceiver : WakefulBroadcastReceiver { }
}