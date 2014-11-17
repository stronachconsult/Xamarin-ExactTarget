/*  1:   */ package com.exacttarget.etpushsdk;
/*  2:   */ 
/*  3:   */ import android.content.Context;
/*  4:   */ import android.content.Intent;
/*  5:   */ import android.support.v4.content.WakefulBroadcastReceiver;
/*  6:   */ import android.util.Log;
/*  7:   */ 
/*  8:   */ public class ETLocationWakeupReceiver
/*  9:   */   extends WakefulBroadcastReceiver
/* 10:   */ {
/* 11:   */   private static final String TAG = "etpushsdk@ETLocationWakeupReceiver";
/* 12:   */   
/* 13:   */   public void onReceive(Context context, Intent intent)
/* 14:   */   {
/* 15:42 */     if (ETPush.getLogLevel() <= 3) {
/* 16:43 */       Log.d("etpushsdk@ETLocationWakeupReceiver", "onReceive()");
/* 17:   */     }
/* 18:45 */     Intent serviceIntent = new Intent(context, ETLocationWakeupService.class);
/* 19:46 */     startWakefulService(context, serviceIntent);
/* 20:   */   }
/* 21:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.ETLocationWakeupReceiver
 * JD-Core Version:    0.7.0.1
 */