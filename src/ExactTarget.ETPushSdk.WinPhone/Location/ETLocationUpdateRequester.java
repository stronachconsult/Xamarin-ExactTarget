/*  1:   */ package com.exacttarget.etpushsdk.location;
/*  2:   */ 
/*  3:   */ import android.app.PendingIntent;
/*  4:   */ import android.location.Criteria;
/*  5:   */ import android.location.LocationManager;
/*  6:   */ import android.util.Log;
/*  7:   */ import com.exacttarget.etpushsdk.ETPush;
/*  8:   */ 
/*  9:   */ public class ETLocationUpdateRequester
/* 10:   */   extends LocationUpdateRequester
/* 11:   */ {
/* 12:   */   private static final String TAG = "etpushsdk@ETLocationUpdateRequester";
/* 13:   */   
/* 14:   */   public ETLocationUpdateRequester(LocationManager locationManager)
/* 15:   */   {
/* 16:21 */     super(locationManager);
/* 17:   */   }
/* 18:   */   
/* 19:   */   public void requestPassiveLocationUpdates(long minTime, long minDistance, PendingIntent pendingIntent)
/* 20:   */   {
/* 21:29 */     if (ETPush.getLogLevel() <= 3) {
/* 22:30 */       Log.d("etpushsdk@ETLocationUpdateRequester", "requestPassiveLocationUpdates");
/* 23:   */     }
/* 24:35 */     this.locationManager.requestLocationUpdates("passive", minTime, (float)minDistance, pendingIntent);
/* 25:   */   }
/* 26:   */   
/* 27:   */   public void requestLocationUpdates(long minTime, long minDistance, Criteria criteria, PendingIntent pendingIntent)
/* 28:   */   {
/* 29:43 */     if (ETPush.getLogLevel() <= 3) {
/* 30:44 */       Log.d("etpushsdk@ETLocationUpdateRequester", "requestLocationUpdates");
/* 31:   */     }
/* 32:50 */     this.locationManager.requestLocationUpdates(minTime, (float)minDistance, criteria, pendingIntent);
/* 33:   */   }
/* 34:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.location.ETLocationUpdateRequester
 * JD-Core Version:    0.7.0.1
 */