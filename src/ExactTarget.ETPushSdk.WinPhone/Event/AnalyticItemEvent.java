/*  1:   */ package com.exacttarget.etpushsdk.event;
/*  2:   */ 
/*  3:   */ import com.exacttarget.etpushsdk.data.AnalyticItem;
/*  4:   */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*  5:   */ import java.util.ArrayList;
/*  6:   */ import java.util.List;
/*  7:   */ 
/*  8:   */ @JsonIgnoreProperties(ignoreUnknown=true)
/*  9:   */ public class AnalyticItemEvent
/* 10:   */   extends ArrayList<AnalyticItem>
/* 11:   */ {
/* 12:   */   private static final long serialVersionUID = 1L;
/* 13:   */   private List<Integer> databaseIds;
/* 14:   */   
/* 15:   */   public List<Integer> getDatabaseIds()
/* 16:   */   {
/* 17:24 */     return this.databaseIds;
/* 18:   */   }
/* 19:   */   
/* 20:   */   public void setDatabaseIds(List<Integer> databaseIds)
/* 21:   */   {
/* 22:28 */     this.databaseIds = databaseIds;
/* 23:   */   }
/* 24:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.event.AnalyticItemEvent
 * JD-Core Version:    0.7.0.1
 */