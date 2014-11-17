/*  1:   */ package com.exacttarget.etpushsdk;
/*  2:   */ 
/*  3:   */ import android.content.Context;
/*  4:   */ import android.content.Intent;
/*  5:   */ import android.support.v4.content.WakefulBroadcastReceiver;
/*  6:   */ import android.util.Log;
/*  7:   */ import com.google.android.gms.location.Geofence;
/*  8:   */ import com.google.android.gms.location.LocationClient;
/*  9:   */ import java.util.List;
/* 10:   */ 
/* 11:   */ public class ETGeofenceReceiver
/* 12:   */   extends WakefulBroadcastReceiver
/* 13:   */ {
/* 14:   */   private static final String TAG = "etpushsdk@ETGeofenceReceiver";
/* 15:   */   
/* 16:   */   public void onReceive(Context context, Intent intent)
/* 17:   */   {
/* 18:47 */     if (ETPush.getLogLevel() <= 3) {
/* 19:48 */       Log.d("etpushsdk@ETGeofenceReceiver", "onReceive()");
/* 20:   */     }
/* 21:50 */     if (LocationClient.hasError(intent))
/* 22:   */     {
/* 23:51 */       handleError(intent);
/* 24:52 */       WakefulBroadcastReceiver.completeWakefulIntent(intent);return;
/* 25:   */     }
/* 26:55 */     handleEnterExit(context, intent);
/* 27:   */   }
/* 28:   */   
/* 29:   */   private void handleEnterExit(Context context, Intent intent)
/* 30:   */   {
/* 31:60 */     if (ETPush.getLogLevel() <= 3) {
/* 32:61 */       Log.d("etpushsdk@ETGeofenceReceiver", "handleEnterExit()");
/* 33:   */     }
/* 34:   */     int transition;
/* 35:67 */     if (((transition = LocationClient.getGeofenceTransition(intent)) == 1) || (transition == 2))
/* 36:   */     {
/* 37:70 */       intent = LocationClient.getTriggeringGeofences(intent);
/* 38:   */       List<Geofence> geofences;
/* 39:71 */       int i = 0;
/* 40:72 */       for (Geofence geofence : geofences)
/* 41:   */       {
/* 42:73 */         if (ETPush.getLogLevel() <= 3)
/* 43:   */         {
/* 44:74 */           Log.d("etpushsdk@ETGeofenceReceiver", "FenceTripped: " + i + ", " + geofence.getRequestId());
/* 45:75 */           i++;
/* 46:   */         }
/* 47:   */         Intent intentService;
/* 48:78 */         (intentService = new Intent(context, ETGeofenceIntentService.class)).putExtra("et_param_database_id", geofence.getRequestId());
/* 49:79 */         intentService.putExtra("et_param_transition_type", transition);
/* 50:80 */         startWakefulService(context, intentService);
/* 51:   */       }
/* 52:83 */       return;
/* 53:   */     }
/* 54:86 */     if (ETPush.getLogLevel() <= 6) {
/* 55:87 */       Log.e("etpushsdk@ETGeofenceReceiver", "Invalid Geofence Transition Type: " + transition);
/* 56:   */     }
/* 57:89 */     WakefulBroadcastReceiver.completeWakefulIntent(???);
/* 58:   */   }
/* 59:   */   
/* 60:   */   private void handleError(Intent intent)
/* 61:   */   {
/* 62:94 */     int errorCode = LocationClient.getErrorCode(intent);
/* 63:96 */     if (ETPush.getLogLevel() <= 6) {
/* 64:97 */       Log.e("etpushsdk@ETGeofenceReceiver", "ERROR, LocationStatusCode: " + errorCode);
/* 65:   */     }
/* 66:   */   }
/* 67:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.ETGeofenceReceiver
 * JD-Core Version:    0.7.0.1
 */