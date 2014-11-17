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
/* 11:   */ import com.exacttarget.etpushsdk.location.LegacyLastLocationFinder;
/* 12:   */ import com.exacttarget.etpushsdk.util.EventBus;
/* 13:   */ 
/* 14:   */ public class PassiveLocationChangedReceiver
/* 15:   */   extends BroadcastReceiver
/* 16:   */ {
/* 17:   */   private static final String TAG = "etpushsdk@PassiveLocationChangedReceiver";
/* 18:   */   
/* 19:   */   public void onReceive(Context context, Intent intent)
/* 20:   */   {
/* 21:   */     ;
/* 22:56 */     if (ETPush.getLogLevel() <= 3) {
/* 23:57 */       Log.d("etpushsdk@PassiveLocationChangedReceiver", "onReceive()");
/* 24:   */     }
/* 25:59 */     String key = "location";
/* 26:60 */     if (intent.hasExtra(key))
/* 27:   */     {
/* 28:65 */       context = (Location)intent.getExtras().get(key);
/* 29:   */     }
/* 30:   */     else
/* 31:   */     {
/* 32:74 */       location = (location = new LegacyLastLocationFinder(location)).getLastBestLocation(0, System.currentTimeMillis() - 300000L);
/* 33:   */       LastKnownLocationEvent lastKnownLocationEvent;
/* 34:78 */       if ((lastKnownLocationEvent = (LastKnownLocationEvent)EventBus.getDefault().getStickyEvent(LastKnownLocationEvent.class)) != null)
/* 35:   */       {
/* 36:   */         Location lastLocation;
/* 37:87 */         if (((lastLocation = lastKnownLocationEvent.getLocation()).getTime() > System.currentTimeMillis() - 300000L) || (lastLocation.distanceTo(location) < 0.0F)) {
/* 38:88 */           location = null;
/* 39:   */         }
/* 40:   */       }
/* 41:   */     }
/* 42:94 */     if (location != null)
/* 43:   */     {
/* 44:95 */       if (ETPush.getLogLevel() <= 3) {
/* 45:96 */         Log.d("etpushsdk@PassiveLocationChangedReceiver", "New Passive Location: " + location.getLatitude() + ", " + location.getLongitude());
/* 46:   */       }
/* 47:98 */       EventBus.getDefault().postSticky(new LastKnownLocationEvent(location));
/* 48:   */     }
/* 49:   */   }
/* 50:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.location.receiver.PassiveLocationChangedReceiver
 * JD-Core Version:    0.7.0.1
 */