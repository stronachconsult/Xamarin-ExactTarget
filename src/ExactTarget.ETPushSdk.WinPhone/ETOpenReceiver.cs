using android.content.BroadcastReceiver;
 using android.content.Context;
 using android.content.Intent;
 using android.os.Bundle;
 using android.os.Handler;
 using android.util.Log;
 using com.exacttarget.etpushsdk.data.AnalyticItem;
 using com.exacttarget.etpushsdk.data.ETSqliteOpenHelper;
 using com.j256.ormlite.dao.Dao;
 using java.sql.SQLException;
 using java.util.Date;
 
namespace ExactTarget.ETPushSdk
 {
 
 public class ETOpenReceiver : BroadcastReceiver
 {
   private static final String TAG = "etpushsdk@ETOpenReceiver";
   protected static final String OPEN_INTENT = "et_open_intent";
   protected static final String MESSAGE_OPENED = "com.exacttarget.MESSAGE_OPENED";
   
   public void onReceive(Context context, Intent intent)
   {
     if (ETPush.getLogLevel() <= 3) {
       Log.d("etpushsdk@ETOpenReceiver", "onReceive");
     }
     Intent launchIntent;
     if ((launchIntent = (Intent)intent.getExtras().getParcelable("et_open_intent")) != null)
     {
       launchIntent.addFlags(268435456);
       context.getApplicationContext().startActivity(launchIntent);
       if (Config.isAnalyticsActive())
       {
         String messageId = launchIntent.getExtras().getString("_m");
         int transitionType = launchIntent.getExtras().getInt("transitionType", -1);
         regionId = launchIntent.getExtras().getString("regionId");
         if ((messageId != null) && (messageId.length() > 0))
         {
           final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(context.getApplicationContext());
           try
           {
             analyticItemDao = helper.getAnalyticItemDao();
             
 
             ETAnalytics.engine().endTimeInAppCounter();
             AnalyticItem event;
             (event = new AnalyticItem(context.getApplicationContext())).setEventDate(new Date());
             event.addAnalyticType(2);
             event.addAnalyticType(4);
             event.addObjectId(messageId);
             if (regionId != null)
             {
               if (transitionType == 1) {
                 event.addAnalyticType(6);
               } else if (transitionType == 2) {
                 event.addAnalyticType(7);
               } else {
                 event.addAnalyticType(12);
               }
               event.addObjectId(regionId);
             }
             analyticItemDao.create(event);
           }
           catch (SQLException e)
           {
             Dao<AnalyticItem, Integer> analyticItemDao;
             if (ETPush.getLogLevel() <= 6) {
               Log.e("etpushsdk@ETOpenReceiver", e.getMessage(), e);
             }
           }
           catch (ETException e)
           {
             if (ETPush.getLogLevel() <= 6) {
               Log.e("etpushsdk@ETOpenReceiver", e.getMessage(), e);
             }
           }
           finally
           {
             (context = new Handler(context.getApplicationContext().getMainLooper())).postDelayed(new Runnable()
             {
               public void run()
               {
                 if ((helper != null) && (helper.isOpen())) {
                   helper.close();
                 }
               }
             }, 10000L);
           }
         }
       }
     }
     else if (ETPush.getLogLevel() <= 6)
     {
       Log.e("etpushsdk@ETOpenReceiver", "Received invalid Intent.");
     }
   }
   
   public void startActivity(Context context, Intent launchIntent)
   {
     context.startActivity(launchIntent);
   }
 }

}