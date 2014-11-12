//using Android.App;
//using Android.Content;
//namespace ExactTarget
//{
//    [BroadcastReceiver(Name = "com.exacttarget.etpushsdk.ET_GenericReceiver", Permission = "com.google.android.c2dm.permission.SEND")]
//    [IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE", "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new[] { "@PACKAGE_NAME@" })]
//    partial class ET_GenericReceiver_ : BroadcastReceiver
//    {
//        public override void OnReceive(Context context, Intent intent) { }
//    }

//    //[BroadcastReceiver(Name = "com.exacttarget.etpushsdk.ET_GenericReceiver", Permission = Amazon.Device.Messaging.Constants.SendPermission)]
//    //[IntentFilter(new[] { Amazon.Device.Messaging.Constants.ReceiveIntent, Amazon.Device.Messaging.Constants.RegistrationIntent }, Categories = new[] { Amazon.Device.Messaging.Constants.PackageName })]
//    //partial class ET_GenericReceiver2_ : BroadcastReceiver
//    //{
//    //    public override void OnReceive(Context context, Intent intent) { }
//    //}

//    [Service(Name = "com.exacttarget.etpushsdk.ETSendDataIntentService")]
//    partial class ETSendDataIntentService_ : IntentService
//    {
//        protected override void OnHandleIntent(Intent intent) { }
//    }

//    [BroadcastReceiver(Name = "com.exacttarget.etpushsdk.ETSendDataReceiver")]
//    partial class ETSendDataReceiver_ : BroadcastReceiver
//    {
//        public override void OnReceive(Context context, Intent intent) { }
//    }
//}