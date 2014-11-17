/*  1:   */ package com.exacttarget.etpushsdk.data;
/*  2:   */ 
/*  3:   */ import android.content.Context;
/*  4:   */ import com.j256.ormlite.field.DatabaseField;
/*  5:   */ import com.j256.ormlite.table.DatabaseTable;
/*  6:   */ 
/*  7:   */ @DatabaseTable(tableName="beacon_request")
/*  8:   */ public class BeaconRequest
/*  9:   */   extends DeviceData
/* 10:   */ {
/* 11:   */   @DatabaseField(generatedId=true)
/* 12:   */   private Integer id;
/* 13:   */   @DatabaseField(columnName="device_id")
/* 14:   */   private String deviceId;
/* 15:   */   @DatabaseField(columnName="latitude")
/* 16:   */   private Double latitude;
/* 17:   */   @DatabaseField(columnName="longitude")
/* 18:   */   private Double longitude;
/* 19:   */   
/* 20:   */   public BeaconRequest() {}
/* 21:   */   
/* 22:   */   public BeaconRequest(Context context)
/* 23:   */   {
/* 24:37 */     this.deviceId = uniqueDeviceIdentifier(context);
/* 25:   */   }
/* 26:   */   
/* 27:   */   public Integer getId()
/* 28:   */   {
/* 29:41 */     return this.id;
/* 30:   */   }
/* 31:   */   
/* 32:   */   public void setId(Integer id)
/* 33:   */   {
/* 34:45 */     this.id = id;
/* 35:   */   }
/* 36:   */   
/* 37:   */   public String getDeviceId()
/* 38:   */   {
/* 39:49 */     return this.deviceId;
/* 40:   */   }
/* 41:   */   
/* 42:   */   public void setDeviceId(String deviceId)
/* 43:   */   {
/* 44:53 */     this.deviceId = deviceId;
/* 45:   */   }
/* 46:   */   
/* 47:   */   public Double getLatitude()
/* 48:   */   {
/* 49:57 */     return this.latitude;
/* 50:   */   }
/* 51:   */   
/* 52:   */   public void setLatitude(Double latitude)
/* 53:   */   {
/* 54:61 */     this.latitude = latitude;
/* 55:   */   }
/* 56:   */   
/* 57:   */   public Double getLongitude()
/* 58:   */   {
/* 59:65 */     return this.longitude;
/* 60:   */   }
/* 61:   */   
/* 62:   */   public void setLongitude(Double longitude)
/* 63:   */   {
/* 64:69 */     this.longitude = longitude;
/* 65:   */   }
/* 66:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.BeaconRequest
 * JD-Core Version:    0.7.0.1
 */