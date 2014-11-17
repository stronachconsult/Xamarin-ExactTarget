/*  1:   */ package com.exacttarget.etpushsdk;
/*  2:   */ 
/*  3:   */ import android.app.IntentService;
/*  4:   */ import android.content.Intent;
/*  5:   */ import android.support.v4.content.WakefulBroadcastReceiver;
/*  6:   */ import android.util.Log;
/*  7:   */ 
/*  8:   */ public class ETGeofenceIntentService
/*  9:   */   extends IntentService
/* 10:   */ {
/* 11:   */   private static final String TAG = "etpushsdk@ETGeofenceIntentService";
/* 12:   */   protected static final String PARAM_DATABASE_ID = "et_param_database_id";
/* 13:   */   protected static final String PARAM_TRANSITION_TYPE = "et_param_transition_type";
/* 14:   */   
/* 15:   */   public ETGeofenceIntentService()
/* 16:   */   {
/* 17:46 */     super("etpushsdk@ETGeofenceIntentService");
/* 18:   */   }
/* 19:   */   
/* 20:   */   protected void onHandleIntent(Intent intent)
/* 21:   */   {
/* 22:51 */     if (ETPush.getLogLevel() <= 3) {
/* 23:52 */       Log.d("etpushsdk@ETGeofenceIntentService", "onHandleIntent()");
/* 24:   */     }
/* 25:55 */     String regionId = intent.getStringExtra("et_param_database_id");
/* 26:56 */     int transitionType = intent.getIntExtra("et_param_transition_type", -1);
/* 27:   */     try
/* 28:   */     {
/* 29:59 */       if (("~~m@g1c_f3nc3~~".equals(regionId)) && (2 == transitionType))
/* 30:   */       {
/* 31:60 */         if (ETPush.getLogLevel() <= 3) {
/* 32:61 */           Log.d("etpushsdk@ETGeofenceIntentService", "Magic fence was exited, get new fence data");
/* 33:   */         }
/* 34:63 */         ETLocationManager.locationManager().setGeofenceInvalidated(true);
/* 35:64 */         if (ETLocationManager.locationManager().isWatchingLocation())
/* 36:   */         {
/* 37:65 */           ETLocationManager.locationManager().startWatchingLocation();
/* 38:   */           break label116;
/* 39:   */         }
/* 40:   */       }
/* 41:   */       else
/* 42:   */       {
/* 43:70 */         ETPush.pushManager().showFenceOrProximityMessage(regionId, transitionType, -1);
/* 44:   */       }
/* 45:   */     }
/* 46:   */     catch (ETException e)
/* 47:   */     {
/* 48:74 */       if (ETPush.getLogLevel() <= 6) {
/* 49:75 */         Log.e("etpushsdk@ETGeofenceIntentService", e.getMessage(), e);
/* 50:   */       }
/* 51:   */     }
/* 52:   */     label116:
/* 53:79 */     WakefulBroadcastReceiver.completeWakefulIntent(intent);
/* 54:   */   }
/* 55:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.ETGeofenceIntentService
 * JD-Core Version:    0.7.0.1
 */