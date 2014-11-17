/*  1:   */ package com.exacttarget.etpushsdk.event;
/*  2:   */ 
/*  3:   */ public class PowerStatusChangedEvent
/*  4:   */ {
/*  5:   */   public static final int BATTERY_LOW = 0;
/*  6:   */   public static final int BATTERY_OK = 1;
/*  7:   */   private int status;
/*  8:   */   
/*  9:   */   public PowerStatusChangedEvent(int status)
/* 10:   */   {
/* 11:29 */     this.status = status;
/* 12:   */   }
/* 13:   */   
/* 14:   */   public int getStatus()
/* 15:   */   {
/* 16:33 */     return this.status;
/* 17:   */   }
/* 18:   */   
/* 19:   */   public void setStatus(int status)
/* 20:   */   {
/* 21:37 */     this.status = status;
/* 22:   */   }
/* 23:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.event.PowerStatusChangedEvent
 * JD-Core Version:    0.7.0.1
 */