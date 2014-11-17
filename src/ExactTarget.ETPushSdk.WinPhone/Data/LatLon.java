/*  1:   */ package com.exacttarget.etpushsdk.data;
/*  2:   */ 
/*  3:   */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*  4:   */ import com.fasterxml.jackson.annotation.JsonInclude;
/*  5:   */ import com.fasterxml.jackson.annotation.JsonInclude.Include;
/*  6:   */ import com.fasterxml.jackson.annotation.JsonProperty;
/*  7:   */ 
/*  8:   */ @JsonIgnoreProperties(ignoreUnknown=true)
/*  9:   */ @JsonInclude(JsonInclude.Include.NON_NULL)
/* 10:   */ public class LatLon
/* 11:   */ {
/* 12:   */   @JsonProperty("latitude")
/* 13:   */   private Double latitude;
/* 14:   */   @JsonProperty("longitude")
/* 15:   */   private Double longitude;
/* 16:   */   
/* 17:   */   public Double getLatitude()
/* 18:   */   {
/* 19:27 */     return this.latitude;
/* 20:   */   }
/* 21:   */   
/* 22:   */   public void setLatitude(Double latitude)
/* 23:   */   {
/* 24:31 */     this.latitude = latitude;
/* 25:   */   }
/* 26:   */   
/* 27:   */   public Double getLongitude()
/* 28:   */   {
/* 29:35 */     return this.longitude;
/* 30:   */   }
/* 31:   */   
/* 32:   */   public void setLongitude(Double longitude)
/* 33:   */   {
/* 34:39 */     this.longitude = longitude;
/* 35:   */   }
/* 36:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.LatLon
 * JD-Core Version:    0.7.0.1
 */