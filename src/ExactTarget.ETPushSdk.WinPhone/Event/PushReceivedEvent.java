/*  1:   */ package com.exacttarget.etpushsdk.event;
/*  2:   */ 
/*  3:   */ import android.os.Bundle;
/*  4:   */ 
/*  5:   */ public class PushReceivedEvent
/*  6:   */ {
/*  7:   */   private Bundle payload;
/*  8:   */   
/*  9:   */   public PushReceivedEvent(Bundle payload)
/* 10:   */   {
/* 11:30 */     this.payload = payload;
/* 12:   */   }
/* 13:   */   
/* 14:   */   public Bundle getPayload()
/* 15:   */   {
/* 16:34 */     return this.payload;
/* 17:   */   }
/* 18:   */   
/* 19:   */   public void setPayload(Bundle payload)
/* 20:   */   {
/* 21:38 */     this.payload = payload;
/* 22:   */   }
/* 23:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.event.PushReceivedEvent
 * JD-Core Version:    0.7.0.1
 */