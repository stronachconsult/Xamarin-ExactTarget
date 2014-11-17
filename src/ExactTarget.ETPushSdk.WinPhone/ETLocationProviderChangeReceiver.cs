/*  1:   */ package com.exacttarget.etpushsdk;
/*  2:   */ 
/*  3:   */ import android.content.Context;
/*  4:   */ import android.content.Intent;
/*  5:   */ import android.support.v4.content.WakefulBroadcastReceiver;
/*  6:   */ import android.util.Log;
/*  7:   */ 
/*  8:   */ public class ETLocationProviderChangeReceiver
/*  9:   */   extends WakefulBroadcastReceiver
/* 10:   */ {
/* 11:   */   private static final String TAG = "etpushsdk@ETLocationProviderChangeReceiver";
/* 12:   */   
/* 13:   */   public void onReceive(Context context, Intent intent)
/* 14:   */   {
/* 15:34 */     if (ETPush.getLogLevel() <= 3) {
/* 16:35 */       Log.d("etpushsdk@ETLocationProviderChangeReceiver", "onReceive()");
/* 17:   */     }
/* 18:38 */     if (intent.getAction().matches("android.location.PROVIDERS_CHANGED"))
/* 19:   */     {
/* 20:39 */       if (ETPush.getLogLevel() <= 3) {
/* 21:40 */         Log.d("etpushsdk@ETLocationProviderChangeReceiver", "Providers changed.");
/* 22:   */       }
/* 23:42 */       Intent serviceIntent = new Intent(context, ETLocationProviderChangeService.class);
/* 24:43 */       startWakefulService(context, serviceIntent);
/* 25:   */     }
/* 26:   */   }
/* 27:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.ETLocationProviderChangeReceiver
 * JD-Core Version:    0.7.0.1
 */