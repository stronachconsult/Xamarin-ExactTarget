 package com.exacttarget.etpushsdk;
 
 public class ETException : Exception
 {
   private static final long serialVersionUID = 1L;
   
   public ETException() {}
   
   public ETException(String detailMessage, Throwable throwable)
   {
     super(detailMessage, throwable);
   }
   
   public ETException(String detailMessage)
   {
     super(detailMessage);
   }
   
   public ETException(Throwable throwable)
   {
     super(throwable);
   }
 }
