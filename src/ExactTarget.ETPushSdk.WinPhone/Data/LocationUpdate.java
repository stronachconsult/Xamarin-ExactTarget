/*   1:    */ package com.exacttarget.etpushsdk.data;
/*   2:    */ 
/*   3:    */ import android.content.Context;
/*   4:    */ import com.fasterxml.jackson.annotation.JsonFormat;
/*   5:    */ import com.fasterxml.jackson.annotation.JsonFormat.Shape;
/*   6:    */ import com.fasterxml.jackson.annotation.JsonIgnore;
/*   7:    */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*   8:    */ import com.fasterxml.jackson.annotation.JsonProperty;
/*   9:    */ import com.j256.ormlite.field.DataType;
/*  10:    */ import com.j256.ormlite.field.DatabaseField;
/*  11:    */ import com.j256.ormlite.table.DatabaseTable;
/*  12:    */ import java.util.Date;
/*  13:    */ 
/*  14:    */ @DatabaseTable(tableName="location_update")
/*  15:    */ @JsonIgnoreProperties(ignoreUnknown=true)
/*  16:    */ public class LocationUpdate
/*  17:    */   extends DeviceData
/*  18:    */ {
/*  19:    */   public static final String COLUMN_LAST_SENT = "last_sent";
/*  20:    */   @DatabaseField(generatedId=true)
/*  21:    */   @JsonIgnore
/*  22:    */   private Integer id;
/*  23:    */   @DatabaseField(columnName="device_id")
/*  24:    */   @JsonProperty("deviceId")
/*  25:    */   private String deviceId;
/*  26:    */   @DatabaseField(columnName="latitude")
/*  27:    */   @JsonProperty("latitude")
/*  28:    */   private Double latitude;
/*  29:    */   @DatabaseField(columnName="longitude")
/*  30:    */   @JsonProperty("longitude")
/*  31:    */   private Double longitude;
/*  32:    */   @DatabaseField(columnName="accuracy")
/*  33:    */   @JsonProperty("accuracy")
/*  34:    */   private Integer accuracy;
/*  35:    */   @DatabaseField(columnName="timestamp", dataType=DataType.DATE_STRING, format="yyyy-MM-dd'T'HH:mm:ss.SSS'Z'")
/*  36:    */   @JsonProperty("location_DateTime_Utc")
/*  37:    */   @JsonFormat(shape=JsonFormat.Shape.STRING, pattern="yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", locale="ENGLISH", timezone="UTC")
/*  38:    */   private Date timestamp;
/*  39:    */   @DatabaseField(columnName="last_sent")
/*  40:    */   @JsonIgnore
/*  41: 54 */   private Long lastSent = Long.valueOf(0L);
/*  42:    */   
/*  43:    */   public LocationUpdate() {}
/*  44:    */   
/*  45:    */   public LocationUpdate(Context context)
/*  46:    */   {
/*  47: 64 */     this.deviceId = uniqueDeviceIdentifier(context);
/*  48:    */   }
/*  49:    */   
/*  50:    */   public Integer getId()
/*  51:    */   {
/*  52: 68 */     return this.id;
/*  53:    */   }
/*  54:    */   
/*  55:    */   public void setId(Integer id)
/*  56:    */   {
/*  57: 72 */     this.id = id;
/*  58:    */   }
/*  59:    */   
/*  60:    */   public String getDeviceId()
/*  61:    */   {
/*  62: 76 */     return this.deviceId;
/*  63:    */   }
/*  64:    */   
/*  65:    */   public void setDeviceId(String deviceId)
/*  66:    */   {
/*  67: 80 */     this.deviceId = deviceId;
/*  68:    */   }
/*  69:    */   
/*  70:    */   public Double getLatitude()
/*  71:    */   {
/*  72: 84 */     return this.latitude;
/*  73:    */   }
/*  74:    */   
/*  75:    */   public void setLatitude(Double latitude)
/*  76:    */   {
/*  77: 88 */     this.latitude = latitude;
/*  78:    */   }
/*  79:    */   
/*  80:    */   public Double getLongitude()
/*  81:    */   {
/*  82: 92 */     return this.longitude;
/*  83:    */   }
/*  84:    */   
/*  85:    */   public void setLongitude(Double longitude)
/*  86:    */   {
/*  87: 96 */     this.longitude = longitude;
/*  88:    */   }
/*  89:    */   
/*  90:    */   public Integer getAccuracy()
/*  91:    */   {
/*  92:100 */     return this.accuracy;
/*  93:    */   }
/*  94:    */   
/*  95:    */   public void setAccuracy(Integer accuracy)
/*  96:    */   {
/*  97:104 */     this.accuracy = accuracy;
/*  98:    */   }
/*  99:    */   
/* 100:    */   public Date getTimestamp()
/* 101:    */   {
/* 102:108 */     return this.timestamp;
/* 103:    */   }
/* 104:    */   
/* 105:    */   public void setTimestamp(Date timestamp)
/* 106:    */   {
/* 107:112 */     this.timestamp = timestamp;
/* 108:    */   }
/* 109:    */   
/* 110:    */   public Long getLastSent()
/* 111:    */   {
/* 112:116 */     return this.lastSent;
/* 113:    */   }
/* 114:    */   
/* 115:    */   public void setLastSent(Long lastSent)
/* 116:    */   {
/* 117:120 */     this.lastSent = lastSent;
/* 118:    */   }
/* 119:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.LocationUpdate
 * JD-Core Version:    0.7.0.1
 */