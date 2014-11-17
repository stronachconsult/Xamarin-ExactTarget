/*  1:   */ package com.exacttarget.etpushsdk.data;
/*  2:   */ 
/*  3:   */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*  4:   */ import com.fasterxml.jackson.annotation.JsonInclude;
/*  5:   */ import com.fasterxml.jackson.annotation.JsonInclude.Include;
/*  6:   */ import com.fasterxml.jackson.annotation.JsonProperty;
/*  7:   */ import java.io.Serializable;
/*  8:   */ 
/*  9:   */ @JsonIgnoreProperties(ignoreUnknown=true)
/* 10:   */ @JsonInclude(JsonInclude.Include.NON_NULL)
/* 11:   */ public class Attribute
/* 12:   */   implements Serializable, Comparable<Attribute>
/* 13:   */ {
/* 14:   */   private static final long serialVersionUID = 1L;
/* 15:   */   @JsonProperty("key")
/* 16:   */   private String key;
/* 17:   */   @JsonProperty("value")
/* 18:   */   private String value;
/* 19:   */   
/* 20:   */   public Attribute() {}
/* 21:   */   
/* 22:   */   public Attribute(String key, String value)
/* 23:   */   {
/* 24:46 */     this.key = key;
/* 25:47 */     this.value = value;
/* 26:   */   }
/* 27:   */   
/* 28:   */   public String getKey()
/* 29:   */   {
/* 30:51 */     return this.key;
/* 31:   */   }
/* 32:   */   
/* 33:   */   public void setKey(String key)
/* 34:   */   {
/* 35:54 */     this.key = key;
/* 36:   */   }
/* 37:   */   
/* 38:   */   public String getValue()
/* 39:   */   {
/* 40:57 */     return this.value;
/* 41:   */   }
/* 42:   */   
/* 43:   */   public void setValue(String value)
/* 44:   */   {
/* 45:60 */     this.value = value;
/* 46:   */   }
/* 47:   */   
/* 48:   */   public boolean equals(Object o)
/* 49:   */   {
/* 50:65 */     if (!(o instanceof Attribute)) {
/* 51:66 */       return false;
/* 52:   */     }
/* 53:68 */     Attribute another = (Attribute)o;
/* 54:69 */     if ((this.key == null) && (another.key == null)) {
/* 55:70 */       return true;
/* 56:   */     }
/* 57:72 */     return this.key.equalsIgnoreCase(another.key);
/* 58:   */   }
/* 59:   */   
/* 60:   */   public int hashCode()
/* 61:   */   {
/* 62:77 */     if (this.key == null) {
/* 63:78 */       return 0;
/* 64:   */     }
/* 65:80 */     return this.key.hashCode();
/* 66:   */   }
/* 67:   */   
/* 68:   */   public int compareTo(Attribute another)
/* 69:   */   {
/* 70:85 */     if ((this.key == null) || (another == null) || (another.key == null)) {
/* 71:86 */       return 0;
/* 72:   */     }
/* 73:89 */     return this.key.compareToIgnoreCase(another.key);
/* 74:   */   }
/* 75:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.Attribute
 * JD-Core Version:    0.7.0.1
 */