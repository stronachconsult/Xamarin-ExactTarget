/*  1:   */ package com.exacttarget.etpushsdk.event;
/*  2:   */ 
/*  3:   */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*  4:   */ import java.io.Serializable;
/*  5:   */ 
/*  6:   */ @JsonIgnoreProperties(ignoreUnknown=true)
/*  7:   */ public class ServerErrorEvent
/*  8:   */   implements Serializable
/*  9:   */ {
/* 10:   */   private static final long serialVersionUID = 1L;
/* 11:   */   private String message;
/* 12:   */   private String documentation;
/* 13:   */   private Integer errorcode;
/* 14:   */   
/* 15:   */   public String getMessage()
/* 16:   */   {
/* 17:35 */     return this.message;
/* 18:   */   }
/* 19:   */   
/* 20:   */   public void setMessage(String message)
/* 21:   */   {
/* 22:39 */     this.message = message;
/* 23:   */   }
/* 24:   */   
/* 25:   */   public String getDocumentation()
/* 26:   */   {
/* 27:43 */     return this.documentation;
/* 28:   */   }
/* 29:   */   
/* 30:   */   public void setDocumentation(String documentation)
/* 31:   */   {
/* 32:47 */     this.documentation = documentation;
/* 33:   */   }
/* 34:   */   
/* 35:   */   public Integer getErrorcode()
/* 36:   */   {
/* 37:51 */     return this.errorcode;
/* 38:   */   }
/* 39:   */   
/* 40:   */   public void setErrorcode(Integer errorcode)
/* 41:   */   {
/* 42:55 */     this.errorcode = errorcode;
/* 43:   */   }
/* 44:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.event.ServerErrorEvent
 * JD-Core Version:    0.7.0.1
 */