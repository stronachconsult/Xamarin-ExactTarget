/*  1:   */ package com.exacttarget.etpushsdk.data;
/*  2:   */ 
/*  3:   */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*  4:   */ import com.fasterxml.jackson.annotation.JsonProperty;
/*  5:   */ import java.util.ArrayList;
/*  6:   */ 
/*  7:   */ @JsonIgnoreProperties(ignoreUnknown=true)
/*  8:   */ public class CloudPagesResponse
/*  9:   */ {
/* 10:   */   @JsonProperty("messages")
/* 11:   */   private ArrayList<Message> messages;
/* 12:   */   
/* 13:   */   public ArrayList<Message> getMessages()
/* 14:   */   {
/* 15:23 */     return this.messages;
/* 16:   */   }
/* 17:   */   
/* 18:   */   public void setMessages(ArrayList<Message> messages)
/* 19:   */   {
/* 20:27 */     this.messages = messages;
/* 21:   */   }
/* 22:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.CloudPagesResponse
 * JD-Core Version:    0.7.0.1
 */