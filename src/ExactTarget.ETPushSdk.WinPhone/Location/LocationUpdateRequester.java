/*  1:   */ package com.exacttarget.etpushsdk.location;
/*  2:   */ 
/*  3:   */ import android.app.PendingIntent;
/*  4:   */ import android.location.Criteria;
/*  5:   */ import android.location.LocationManager;
/*  6:   */ 
/*  7:   */ public abstract class LocationUpdateRequester
/*  8:   */ {
/*  9:   */   protected LocationManager locationManager;
/* 10:   */   
/* 11:   */   protected LocationUpdateRequester(LocationManager locationManager)
/* 12:   */   {
/* 13:18 */     this.locationManager = locationManager;
/* 14:   */   }
/* 15:   */   
/* 16:   */   public void requestLocationUpdates(long minTime, long minDistance, Criteria criteria, PendingIntent pendingIntent) {}
/* 17:   */   
/* 18:   */   public void requestPassiveLocationUpdates(long minTime, long minDistance, PendingIntent pendingIntent) {}
/* 19:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.location.LocationUpdateRequester
 * JD-Core Version:    0.7.0.1
 */