/*  1:   */ package com.exacttarget.etpushsdk;
/*  2:   */ 
/*  3:   */ import android.app.IntentService;
/*  4:   */ import android.content.Intent;
/*  5:   */ import android.util.Log;
/*  6:   */ 
/*  7:   */ public class ETLocationProviderChangeService
/*  8:   */   extends IntentService
/*  9:   */ {
/* 10:   */   private static final String TAG = "etpushsdk@ETLocationProviderChangeService";
/* 11:   */   
/* 12:   */   public ETLocationProviderChangeService()
/* 13:   */   {
/* 14:32 */     super("etpushsdk@ETLocationProviderChangeService");
/* 15:   */   }
/* 16:   */   
/* 17:   */   protected void onHandleIntent(Intent intent)
/* 18:   */   {
/* 19:38 */     if (ETPush.getLogLevel() <= 3) {
/* 20:39 */       Log.d("etpushsdk@ETLocationProviderChangeService", "onHandleIntent()");
/* 21:   */     }
/* 22:   */     try
/* 23:   */     {
/* 24:42 */       if (ETLocationManager.locationManager().areLocationProvidersAvailable())
/* 25:   */       {
/* 26:43 */         if (ETPush.getLogLevel() <= 3) {
/* 27:44 */           Log.d("etpushsdk@ETLocationProviderChangeService", "Location Provider enabled.");
/* 28:   */         }
/* 29:46 */         if (ETLocationManager.locationManager().isWatchingLocation()) {
/* 30:47 */           ETLocationManager.locationManager().startWatchingLocation();
/* 31:   */         }
/* 32:   */       }
/* 33:   */       else
/* 34:   */       {
/* 35:51 */         if (ETPush.getLogLevel() <= 3) {
/* 36:52 */           Log.d("etpushsdk@ETLocationProviderChangeService", "Location Provider disabled.");
/* 37:   */         }
/* 38:54 */         if (ETLocationManager.locationManager().isWatchingLocation())
/* 39:   */         {
/* 40:55 */           ETLocationManager.locationManager().stopWatchingLocation();
/* 41:   */           
/* 42:   */ 
/* 43:58 */           ETLocationManager.locationManager().setGeoEnabled(true);
/* 44:   */         }
/* 45:   */       }
/* 46:62 */       ETLocationProviderChangeReceiver.completeWakefulIntent(intent); return;
/* 47:   */     }
/* 48:   */     catch (ETException e)
/* 49:   */     {
/* 50:65 */       if (ETPush.getLogLevel() <= 6) {
/* 51:66 */         Log.e("etpushsdk@ETLocationProviderChangeService", e.getMessage(), e);
/* 52:   */       }
/* 53:   */     }
/* 54:   */   }
/* 55:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.ETLocationProviderChangeService
 * JD-Core Version:    0.7.0.1
 */