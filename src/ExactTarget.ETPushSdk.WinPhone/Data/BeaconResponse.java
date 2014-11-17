/*  1:   */ package com.exacttarget.etpushsdk.data;
/*  2:   */ 
/*  3:   */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*  4:   */ import com.fasterxml.jackson.annotation.JsonInclude;
/*  5:   */ import com.fasterxml.jackson.annotation.JsonInclude.Include;
/*  6:   */ import com.fasterxml.jackson.annotation.JsonProperty;
/*  7:   */ import java.util.List;
/*  8:   */ 
/*  9:   */ @JsonIgnoreProperties(ignoreUnknown=true)
/* 10:   */ @JsonInclude(JsonInclude.Include.NON_NULL)
/* 11:   */ public class BeaconResponse
/* 12:   */ {
/* 13:   */   @JsonProperty("beacons")
/* 14:   */   private List<Region> beacons;
/* 15:   */   
/* 16:   */   public List<Region> getBeacons()
/* 17:   */   {
/* 18:26 */     return this.beacons;
/* 19:   */   }
/* 20:   */   
/* 21:   */   public void setBeacons(List<Region> beacons)
/* 22:   */   {
/* 23:30 */     this.beacons = beacons;
/* 24:   */   }
/* 25:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.BeaconResponse
 * JD-Core Version:    0.7.0.1
 */