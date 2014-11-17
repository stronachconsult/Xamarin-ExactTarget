/*  1:   */ package com.exacttarget.etpushsdk.data;
/*  2:   */ 
/*  3:   */ import com.j256.ormlite.field.DatabaseField;
/*  4:   */ import com.j256.ormlite.table.DatabaseTable;
/*  5:   */ 
/*  6:   */ @DatabaseTable(tableName="region_message")
/*  7:   */ public class RegionMessage
/*  8:   */ {
/*  9:   */   public static final String COLUMN_REGION_ID = "region_id";
/* 10:   */   public static final String COLUMN_MESSAGE_ID = "message_id";
/* 11:   */   @DatabaseField(generatedId=true)
/* 12:   */   private Integer id;
/* 13:   */   @DatabaseField(foreign=true, columnName="region_id")
/* 14:   */   private Region region;
/* 15:   */   @DatabaseField(foreign=true, columnName="message_id")
/* 16:   */   private Message message;
/* 17:   */   
/* 18:   */   public RegionMessage() {}
/* 19:   */   
/* 20:   */   public RegionMessage(Region region, Message message)
/* 21:   */   {
/* 22:40 */     this.region = region;
/* 23:41 */     this.message = message;
/* 24:   */   }
/* 25:   */   
/* 26:   */   public Integer getId()
/* 27:   */   {
/* 28:45 */     return this.id;
/* 29:   */   }
/* 30:   */   
/* 31:   */   public void setId(Integer id)
/* 32:   */   {
/* 33:49 */     this.id = id;
/* 34:   */   }
/* 35:   */   
/* 36:   */   public Region getRegion()
/* 37:   */   {
/* 38:53 */     return this.region;
/* 39:   */   }
/* 40:   */   
/* 41:   */   public void setRegion(Region region)
/* 42:   */   {
/* 43:57 */     this.region = region;
/* 44:   */   }
/* 45:   */   
/* 46:   */   public Message getMessage()
/* 47:   */   {
/* 48:61 */     return this.message;
/* 49:   */   }
/* 50:   */   
/* 51:   */   public void setMessage(Message message)
/* 52:   */   {
/* 53:65 */     this.message = message;
/* 54:   */   }
/* 55:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.RegionMessage
 * JD-Core Version:    0.7.0.1
 */