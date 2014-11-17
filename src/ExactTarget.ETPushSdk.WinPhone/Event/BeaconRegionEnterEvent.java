/*  1:   */ package com.exacttarget.etpushsdk.event;
/*  2:   */ 
/*  3:   */ import com.exacttarget.etpushsdk.data.Region;
/*  4:   */ 
/*  5:   */ public class BeaconRegionEnterEvent
/*  6:   */ {
/*  7:   */   private Region region;
/*  8:   */   
/*  9:   */   public BeaconRegionEnterEvent(Region region)
/* 10:   */   {
/* 11:18 */     this.region = region;
/* 12:   */   }
/* 13:   */   
/* 14:   */   public Region getRegion()
/* 15:   */   {
/* 16:22 */     return this.region;
/* 17:   */   }
/* 18:   */   
/* 19:   */   public void setRegion(Region region)
/* 20:   */   {
/* 21:26 */     this.region = region;
/* 22:   */   }
/* 23:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.event.BeaconRegionEnterEvent
 * JD-Core Version:    0.7.0.1
 */