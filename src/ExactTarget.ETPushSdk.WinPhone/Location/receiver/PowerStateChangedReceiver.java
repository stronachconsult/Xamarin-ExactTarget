/*  1:   */ package com.exacttarget.etpushsdk.location.receiver;
/*  2:   */ 
/*  3:   */ import android.content.BroadcastReceiver;
/*  4:   */ import android.content.ComponentName;
/*  5:   */ import android.content.Context;
/*  6:   */ import android.content.Intent;
/*  7:   */ import android.content.pm.PackageManager;
/*  8:   */ import android.util.Log;
/*  9:   */ import com.exacttarget.etpushsdk.Config;
/* 10:   */ import com.exacttarget.etpushsdk.ETException;
/* 11:   */ import com.exacttarget.etpushsdk.ETLocationManager;
/* 12:   */ import com.exacttarget.etpushsdk.ETPush;
/* 13:   */ import com.exacttarget.etpushsdk.event.PowerStatusChangedEvent;
/* 14:   */ import com.exacttarget.etpushsdk.util.EventBus;
/* 15:   */ 
/* 16:   */ public class PowerStateChangedReceiver
/* 17:   */   extends BroadcastReceiver
/* 18:   */ {
/* 19:   */   private static final String TAG = "etpushsdk@PowerStateChangedReceiver";
/* 20:   */   
/* 21:   */   public void onReceive(Context context, Intent intent)
/* 22:   */   {
/* 23:   */     ;
/* 24:51 */     if (ETPush.getLogLevel() <= 3) {
/* 25:52 */       Log.d("etpushsdk@PowerStateChangedReceiver", "onReceive()");
/* 26:   */     }
/* 27:54 */     boolean batteryLow = intent.getAction().equals("android.intent.action.BATTERY_LOW");
/* 28:   */     
/* 29:56 */     PackageManager pm = context.getPackageManager();
/* 30:57 */     passiveLocationReceiver = new ComponentName(context, PassiveLocationChangedReceiver.class);
/* 31:   */     
/* 32:   */ 
/* 33:   */ 
/* 34:   */ 
/* 35:62 */     pm.setComponentEnabledSetting(passiveLocationReceiver, batteryLow ? 2 : 0, 1);
/* 36:   */     try
/* 37:   */     {
/* 38:66 */       if ((Config.isLocationManagerActive()) && 
/* 39:67 */         (ETLocationManager.locationManager().isWatchingLocation())) {
/* 40:68 */         if (batteryLow) {
/* 41:69 */           ETLocationManager.locationManager().enterLowPowerMode();
/* 42:   */         } else {
/* 43:72 */           ETLocationManager.locationManager().exitLowPowerMode();
/* 44:   */         }
/* 45:   */       }
/* 46:   */     }
/* 47:   */     catch (ETException e)
/* 48:   */     {
/* 49:78 */       if (ETPush.getLogLevel() <= 6) {
/* 50:79 */         Log.e("etpushsdk@PowerStateChangedReceiver", e.getMessage(), e);
/* 51:   */       }
/* 52:   */     }
/* 53:82 */     EventBus.getDefault().postSticky(new PowerStatusChangedEvent(batteryLow ? 0 : 1));
/* 54:   */   }
/* 55:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.location.receiver.PowerStateChangedReceiver
 * JD-Core Version:    0.7.0.1
 */