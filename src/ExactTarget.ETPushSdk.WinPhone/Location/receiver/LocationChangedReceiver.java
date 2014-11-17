/*  1:   */ package com.exacttarget.etpushsdk.location.receiver;
/*  2:   */ 
/*  3:   */ import android.content.BroadcastReceiver;
/*  4:   */ import android.content.Context;
/*  5:   */ import android.content.Intent;
/*  6:   */ import android.location.Location;
/*  7:   */ import android.os.Bundle;
/*  8:   */ import android.util.Log;
/*  9:   */ import com.exacttarget.etpushsdk.ETPush;
/* 10:   */ import com.exacttarget.etpushsdk.event.LastKnownLocationEvent;
/* 11:   */ import com.exacttarget.etpushsdk.util.EventBus;
/* 12:   */ 
/* 13:   */ public class LocationChangedReceiver
/* 14:   */   extends BroadcastReceiver
/* 15:   */ {
/* 16:   */   private static final String TAG = "etpushsdk@LocationChangedReceiver";
/* 17:   */   
/* 18:   */   public void onReceive(Context context, Intent intent)
/* 19:   */   {
/* 20:54 */     if (ETPush.getLogLevel() <= 3) {
/* 21:55 */       Log.d("etpushsdk@LocationChangedReceiver", "onReceive()");
/* 22:   */     }
/* 23:57 */     String locationKey = "location";
/* 24:58 */     String providerEnabledKey = "providerEnabled";
/* 25:59 */     if ((intent.hasExtra(providerEnabledKey)) && 
/* 26:60 */       (!intent.getBooleanExtra(providerEnabledKey, true)))
/* 27:   */     {
/* 28:61 */       Intent providerDisabledIntent = new Intent("com.exacttarget.etpushsdk.active_location_update_provider_disabled");
/* 29:62 */       context.sendBroadcast(providerDisabledIntent);
/* 30:   */     }
/* 31:65 */     if (intent.hasExtra(locationKey))
/* 32:   */     {
/* 33:66 */       Location location = (Location)intent.getExtras().get(locationKey);
/* 34:67 */       if (ETPush.getLogLevel() <= 3) {
/* 35:68 */         Log.d("etpushsdk@LocationChangedReceiver", "New Active Location: " + location.getLatitude() + ", " + location.getLongitude());
/* 36:   */       }
/* 37:70 */       EventBus.getDefault().postSticky(new LastKnownLocationEvent(location));
/* 38:   */     }
/* 39:   */   }
/* 40:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.location.receiver.LocationChangedReceiver
 * JD-Core Version:    0.7.0.1
 */