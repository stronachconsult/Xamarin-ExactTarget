/*  1:   */ package com.exacttarget.etpushsdk.event;
/*  2:   */ 
/*  3:   */ import android.location.Location;
/*  4:   */ 
/*  5:   */ public class LastKnownLocationEvent
/*  6:   */ {
/*  7:   */   private Location location;
/*  8:   */   
/*  9:   */   public LastKnownLocationEvent(Location location)
/* 10:   */   {
/* 11:27 */     this.location = location;
/* 12:   */   }
/* 13:   */   
/* 14:   */   public Location getLocation()
/* 15:   */   {
/* 16:31 */     return this.location;
/* 17:   */   }
/* 18:   */   
/* 19:   */   public void setLocation(Location location)
/* 20:   */   {
/* 21:35 */     this.location = location;
/* 22:   */   }
/* 23:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.event.LastKnownLocationEvent
 * JD-Core Version:    0.7.0.1
 */