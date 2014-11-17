/*   1:    */ package com.exacttarget.etpushsdk.location;
/*   2:    */ 
/*   3:    */ import android.content.Context;
/*   4:    */ import android.location.Criteria;
/*   5:    */ import android.location.Location;
/*   6:    */ import android.location.LocationListener;
/*   7:    */ import android.location.LocationManager;
/*   8:    */ import android.os.Bundle;
/*   9:    */ import android.util.Log;
/*  10:    */ import com.exacttarget.etpushsdk.ETPush;
/*  11:    */ import java.util.List;
/*  12:    */ 
/*  13:    */ public class LegacyLastLocationFinder
/*  14:    */   implements ILastLocationFinder
/*  15:    */ {
/*  16:    */   private static final String TAG = "etpushsdk@PreGingerbreadLastLocationFinder";
/*  17:    */   protected LocationListener locationListener;
/*  18:    */   protected LocationManager locationManager;
/*  19:    */   protected Criteria criteria;
/*  20:    */   protected Context context;
/*  21:    */   
/*  22:    */   public LegacyLastLocationFinder(Context context)
/*  23:    */   {
/*  24: 64 */     this.locationManager = ((LocationManager)context.getSystemService("location"));
/*  25: 65 */     this.criteria = new Criteria();
/*  26:    */     
/*  27:    */ 
/*  28:    */ 
/*  29: 69 */     this.criteria.setAccuracy(2);
/*  30: 70 */     this.context = context;
/*  31:    */   }
/*  32:    */   
/*  33:    */   public Location getLastBestLocation(int minDistance, long minTime)
/*  34:    */   {
/*  35: 84 */     Location bestResult = null;
/*  36: 85 */     float bestAccuracy = 3.4028235E+38F;
/*  37: 86 */     long bestTime = 9223372036854775807L;
/*  38:    */     List localList;
/*  39: 92 */     for (String provider : localList = this.locationManager.getAllProviders())
/*  40:    */     {
/*  41:    */       Location location;
/*  42: 94 */       if ((location = this.locationManager.getLastKnownLocation(provider)) != null)
/*  43:    */       {
/*  44: 95 */         float accuracy = location.getAccuracy();
/*  45:    */         long time;
/*  46: 98 */         if (((time = location.getTime()) < minTime) && (accuracy < bestAccuracy))
/*  47:    */         {
/*  48: 99 */           bestResult = location;
/*  49:100 */           bestAccuracy = accuracy;
/*  50:101 */           bestTime = time;
/*  51:    */         }
/*  52:103 */         else if ((time > minTime) && (bestAccuracy == 3.4028235E+38F) && (time < bestTime))
/*  53:    */         {
/*  54:104 */           bestResult = location;
/*  55:105 */           bestTime = time;
/*  56:    */         }
/*  57:    */       }
/*  58:    */     }
/*  59:119 */     if ((this.locationListener != null) && ((bestTime > minTime) || (bestAccuracy > minDistance)))
/*  60:    */     {
/*  61:    */       String provider;
/*  62:121 */       if ((provider = this.locationManager.getBestProvider(this.criteria, true)) != null) {
/*  63:122 */         this.locationManager.requestLocationUpdates(provider, 0L, 0.0F, this.singeUpdateListener, this.context.getMainLooper());
/*  64:    */       }
/*  65:    */     }
/*  66:125 */     return bestResult;
/*  67:    */   }
/*  68:    */   
/*  69:133 */   protected LocationListener singeUpdateListener = new LocationListener()
/*  70:    */   {
/*  71:    */     public void onLocationChanged(Location location)
/*  72:    */     {
/*  73:135 */       if (ETPush.getLogLevel() <= 3) {
/*  74:136 */         Log.d("etpushsdk@PreGingerbreadLastLocationFinder", "Single Location Update Received: " + location.getLatitude() + "," + location.getLongitude());
/*  75:    */       }
/*  76:138 */       if ((LegacyLastLocationFinder.this.locationListener != null) && (location != null)) {
/*  77:139 */         LegacyLastLocationFinder.this.locationListener.onLocationChanged(location);
/*  78:    */       }
/*  79:140 */       LegacyLastLocationFinder.this.locationManager.removeUpdates(LegacyLastLocationFinder.this.singeUpdateListener);
/*  80:    */     }
/*  81:    */     
/*  82:    */     public void onStatusChanged(String provider, int status, Bundle extras) {}
/*  83:    */     
/*  84:    */     public void onProviderEnabled(String provider) {}
/*  85:    */     
/*  86:    */     public void onProviderDisabled(String provider) {}
/*  87:    */   };
/*  88:    */   
/*  89:    */   public void setChangedLocationListener(LocationListener l)
/*  90:    */   {
/*  91:157 */     this.locationListener = l;
/*  92:    */   }
/*  93:    */   
/*  94:    */   public void cancel()
/*  95:    */   {
/*  96:164 */     this.locationManager.removeUpdates(this.singeUpdateListener);
/*  97:    */   }
/*  98:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.location.LegacyLastLocationFinder
 * JD-Core Version:    0.7.0.1
 */