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
/* 11:   */ public class GeofenceResponse
/* 12:   */ {
/* 13:   */   @JsonProperty("refreshCenter")
/* 14:   */   private LatLon refreshCenter;
/* 15:   */   @JsonProperty("refreshRadius")
/* 16:   */   private Integer refreshRadius;
/* 17:   */   @JsonProperty("fences")
/* 18:   */   private List<Region> fences;
/* 19:   */   
/* 20:   */   public LatLon getRefreshCenter()
/* 21:   */   {
/* 22:32 */     return this.refreshCenter;
/* 23:   */   }
/* 24:   */   
/* 25:   */   public void setRefreshCenter(LatLon refreshCenter)
/* 26:   */   {
/* 27:36 */     this.refreshCenter = refreshCenter;
/* 28:   */   }
/* 29:   */   
/* 30:   */   public Integer getRefreshRadius()
/* 31:   */   {
/* 32:40 */     return this.refreshRadius;
/* 33:   */   }
/* 34:   */   
/* 35:   */   public void setRefreshRadius(Integer refreshRadius)
/* 36:   */   {
/* 37:44 */     this.refreshRadius = refreshRadius;
/* 38:   */   }
/* 39:   */   
/* 40:   */   public List<Region> getFences()
/* 41:   */   {
/* 42:48 */     return this.fences;
/* 43:   */   }
/* 44:   */   
/* 45:   */   public void setFences(List<Region> fences)
/* 46:   */   {
/* 47:52 */     this.fences = fences;
/* 48:   */   }
/* 49:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.GeofenceResponse
 * JD-Core Version:    0.7.0.1
 */