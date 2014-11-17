/*  1:   */ package com.exacttarget.etpushsdk.event;
/*  2:   */ 
/*  3:   */ import com.exacttarget.etpushsdk.data.Region;
/*  4:   */ 
/*  5:   */ public class BeaconRegionExitEvent
/*  6:   */ {
/*  7:   */   private Region region;
/*  8:   */   
/*  9:   */   public BeaconRegionExitEvent(Region region)
/* 10:   */   {
/* 11:17 */     this.region = region;
/* 12:   */   }
/* 13:   */   
/* 14:   */   public Region getRegion()
/* 15:   */   {
/* 16:21 */     return this.region;
/* 17:   */   }
/* 18:   */   
/* 19:   */   public void setRegion(Region region)
/* 20:   */   {
/* 21:25 */     this.region = region;
/* 22:   */   }
/* 23:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.event.BeaconRegionExitEvent
 * JD-Core Version:    0.7.0.1
 */