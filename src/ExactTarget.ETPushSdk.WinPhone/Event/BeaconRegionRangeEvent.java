/*  1:   */ package com.exacttarget.etpushsdk.event;
/*  2:   */ 
/*  3:   */ import com.exacttarget.etpushsdk.data.Region;
/*  4:   */ 
/*  5:   */ public class BeaconRegionRangeEvent
/*  6:   */ {
/*  7:   */   private Region region;
/*  8:   */   private int proximity;
/*  9:   */   private int rssi;
/* 10:   */   private double accuracy;
/* 11:   */   
/* 12:   */   public BeaconRegionRangeEvent(Region region, int proximity, int rssi, double accuracy)
/* 13:   */   {
/* 14:21 */     this.region = region;
/* 15:22 */     this.proximity = proximity;
/* 16:23 */     this.rssi = rssi;
/* 17:24 */     this.accuracy = accuracy;
/* 18:   */   }
/* 19:   */   
/* 20:   */   public Region getRegion()
/* 21:   */   {
/* 22:28 */     return this.region;
/* 23:   */   }
/* 24:   */   
/* 25:   */   public void setRegion(Region region)
/* 26:   */   {
/* 27:32 */     this.region = region;
/* 28:   */   }
/* 29:   */   
/* 30:   */   public int getProximity()
/* 31:   */   {
/* 32:36 */     return this.proximity;
/* 33:   */   }
/* 34:   */   
/* 35:   */   public void setProximity(int proximity)
/* 36:   */   {
/* 37:40 */     this.proximity = proximity;
/* 38:   */   }
/* 39:   */   
/* 40:   */   public int getRssi()
/* 41:   */   {
/* 42:44 */     return this.rssi;
/* 43:   */   }
/* 44:   */   
/* 45:   */   public void setRssi(int rssi)
/* 46:   */   {
/* 47:48 */     this.rssi = rssi;
/* 48:   */   }
/* 49:   */   
/* 50:   */   public double getAccuracy()
/* 51:   */   {
/* 52:52 */     return this.accuracy;
/* 53:   */   }
/* 54:   */   
/* 55:   */   public void setAccuracy(double accuracy)
/* 56:   */   {
/* 57:56 */     this.accuracy = accuracy;
/* 58:   */   }
/* 59:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.event.BeaconRegionRangeEvent
 * JD-Core Version:    0.7.0.1
 */