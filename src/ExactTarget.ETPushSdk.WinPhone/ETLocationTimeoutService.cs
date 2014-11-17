/*  1:   */ package com.exacttarget.etpushsdk;
/*  2:   */ 
/*  3:   */ import android.app.IntentService;
/*  4:   */ import android.content.Intent;
/*  5:   */ import android.util.Log;
/*  6:   */ import com.exacttarget.etpushsdk.location.ILastLocationFinder;
/*  7:   */ 
/*  8:   */ public class ETLocationTimeoutService
/*  9:   */   extends IntentService
/* 10:   */ {
/* 11:   */   private static final String TAG = "etpushsdk@ETLocationTimeoutService";
/* 12:   */   
/* 13:   */   public ETLocationTimeoutService()
/* 14:   */   {
/* 15:40 */     super("ETLocationTimeoutService");
/* 16:   */   }
/* 17:   */   
/* 18:   */   protected void onHandleIntent(Intent intent)
/* 19:   */   {
/* 20:46 */     if (ETPush.getLogLevel() <= 3) {
/* 21:47 */       Log.d("etpushsdk@ETLocationTimeoutService", "onHandleIntent()");
/* 22:   */     }
/* 23:   */     try
/* 24:   */     {
/* 25:50 */       if (ETLocationManager.locationManager().lastLocationFinder != null) {
/* 26:51 */         ETLocationManager.locationManager().lastLocationFinder.cancel();
/* 27:   */       }
/* 28:53 */       ETLocationTimeoutReceiver.completeWakefulIntent(intent);
/* 29:54 */       ETLocationManager.locationManager().completeWakefulIntent(); return;
/* 30:   */     }
/* 31:   */     catch (ETException e)
/* 32:   */     {
/* 33:57 */       if (ETPush.getLogLevel() <= 6) {
/* 34:58 */         Log.e("etpushsdk@ETLocationTimeoutService", e.getMessage(), e);
/* 35:   */       }
/* 36:   */     }
/* 37:   */   }
/* 38:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.ETLocationTimeoutService
 * JD-Core Version:    0.7.0.1
 */