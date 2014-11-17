/*  1:   */ package com.exacttarget.etpushsdk;
/*  2:   */ 
/*  3:   */ public class ETException
/*  4:   */   extends Exception
/*  5:   */ {
/*  6:   */   private static final long serialVersionUID = 1L;
/*  7:   */   
/*  8:   */   public ETException() {}
/*  9:   */   
/* 10:   */   public ETException(String detailMessage, Throwable throwable)
/* 11:   */   {
/* 12:26 */     super(detailMessage, throwable);
/* 13:   */   }
/* 14:   */   
/* 15:   */   public ETException(String detailMessage)
/* 16:   */   {
/* 17:30 */     super(detailMessage);
/* 18:   */   }
/* 19:   */   
/* 20:   */   public ETException(Throwable throwable)
/* 21:   */   {
/* 22:34 */     super(throwable);
/* 23:   */   }
/* 24:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.ETException
 * JD-Core Version:    0.7.0.1
 */