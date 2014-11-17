/*  1:   */ package com.exacttarget.etpushsdk;
/*  2:   */ 
/*  3:   */ import android.app.IntentService;
/*  4:   */ import android.content.Intent;
/*  5:   */ import android.util.Log;
/*  6:   */ 
/*  7:   */ public class ETLocationWakeupService
/*  8:   */   extends IntentService
/*  9:   */ {
/* 10:   */   private static final String TAG = "etpushsdk@ETLocationWakeupService";
/* 11:   */   
/* 12:   */   public ETLocationWakeupService()
/* 13:   */   {
/* 14:40 */     super("ETLocationWakeupService");
/* 15:   */   }
/* 16:   */   
/* 17:   */   protected void onHandleIntent(Intent intent)
/* 18:   */   {
/* 19:45 */     if (ETPush.getLogLevel() <= 3) {
/* 20:46 */       Log.d("etpushsdk@ETLocationWakeupService", "onHandleIntent()");
/* 21:   */     }
/* 22:   */     try
/* 23:   */     {
/* 24:49 */       ETLocationManager.locationManager().awokenByIntent = intent;
/* 25:50 */       if (ETLocationManager.locationManager().isWatchingLocation())
/* 26:   */       {
/* 27:51 */         ETLocationManager.locationManager().startWatchingLocation();
/* 28:   */       }
/* 29:   */       else
/* 30:   */       {
/* 31:54 */         ETLocationManager.locationManager().stopWatchingLocation(); return;
/* 32:   */       }
/* 33:   */     }
/* 34:   */     catch (ETException e)
/* 35:   */     {
/* 36:58 */       if (ETPush.getLogLevel() <= 6) {
/* 37:59 */         Log.e("etpushsdk@ETLocationWakeupService", e.getMessage(), e);
/* 38:   */       }
/* 39:   */     }
/* 40:   */   }
/* 41:   */ }



/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar

 * Qualified Name:     com.exacttarget.etpushsdk.ETLocationWakeupService

 * JD-Core Version:    0.7.0.1

 */