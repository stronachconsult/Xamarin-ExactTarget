/*   1:    */ package com.exacttarget.etpushsdk.util;
/*   2:    */ 
/*   3:    */ import android.util.Log;
/*   4:    */ import com.exacttarget.etpushsdk.ETPush;
/*   5:    */ import com.exacttarget.etpushsdk.event.ServerErrorEvent;
/*   6:    */ import com.fasterxml.jackson.core.JsonParseException;
/*   7:    */ import com.fasterxml.jackson.core.JsonProcessingException;
/*   8:    */ import com.fasterxml.jackson.core.type.TypeReference;
/*   9:    */ import com.fasterxml.jackson.databind.JsonMappingException;
/*  10:    */ import com.fasterxml.jackson.databind.ObjectMapper;
/*  11:    */ import com.fasterxml.jackson.databind.ObjectWriter;
/*  12:    */ import java.io.IOException;
/*  13:    */ import java.util.List;
/*  14:    */ 
/*  15:    */ public class JSONUtil
/*  16:    */ {
/*  17:    */   private static final String TAG = "etpushsdk@JSONUtil";
/*  18: 35 */   private static final ObjectMapper mapper = new ObjectMapper();
/*  19:    */   
/*  20:    */   public static <T> T jsonToObject(String json, Class<T> clazz)
/*  21:    */   {
/*  22:    */     ;
/*  23: 38 */     Object response = null;
/*  24:    */     try
/*  25:    */     {
/*  26: 40 */       response = mapper.readValue(json, clazz);
/*  27:    */     }
/*  28:    */     catch (Throwable localThrowable1)
/*  29:    */     {
/*  30:    */       try
/*  31:    */       {
/*  32: 45 */         ServerErrorEvent errorMessage = (ServerErrorEvent)mapper.readValue(json, ServerErrorEvent.class);
/*  33: 46 */         if (ETPush.getLogLevel() <= 6) {
/*  34: 47 */           Log.e("etpushsdk@JSONUtil", "SERVER ERROR: " + errorMessage.getMessage());
/*  35:    */         }
/*  36: 49 */         EventBus.getDefault().post(errorMessage);
/*  37:    */       }
/*  38:    */       catch (Throwable localThrowable2)
/*  39:    */       {
/*  40:    */         try
/*  41:    */         {
/*  42: 55 */           for (ServerErrorEvent errorMessage : json = (List)mapper.readValue(json, new TypeReference() {}))
/*  43:    */           {
/*  44: 56 */             if (ETPush.getLogLevel() <= 6) {
/*  45: 57 */               Log.e("etpushsdk@JSONUtil", "SERVER ERROR: " + errorMessage.getMessage());
/*  46:    */             }
/*  47: 59 */             EventBus.getDefault().post(errorMessage);
/*  48:    */           }
/*  49:    */         }
/*  50:    */         catch (Throwable e2)
/*  51:    */         {
/*  52: 63 */           if (ETPush.getLogLevel() <= 6) {
/*  53: 64 */             Log.e("etpushsdk@JSONUtil", e2.getMessage(), e2);
/*  54:    */           }
/*  55:    */         }
/*  56:    */       }
/*  57:    */     }
/*  58: 70 */     return clazz.cast(response);
/*  59:    */   }
/*  60:    */   
/*  61:    */   public static String objectToJson(Object jsonObject)
/*  62:    */   {
/*  63: 74 */     String response = null;
/*  64:    */     try
/*  65:    */     {
/*  66: 76 */       response = mapper.writeValueAsString(jsonObject);
/*  67:    */     }
/*  68:    */     catch (JsonProcessingException e)
/*  69:    */     {
/*  70: 79 */       if (ETPush.getLogLevel() <= 6) {
/*  71: 80 */         Log.e("etpushsdk@JSONUtil", e.getMessage(), e);
/*  72:    */       }
/*  73:    */     }
/*  74: 83 */     return response;
/*  75:    */   }
/*  76:    */   
/*  77:    */   public static String jsonMessToPrettyString(String json)
/*  78:    */   {
/*  79:    */     try
/*  80:    */     {
/*  81: 88 */       return mapper.writer().withDefaultPrettyPrinter().writeValueAsString(mapper.readValue(json, Object.class));
/*  82:    */     }
/*  83:    */     catch (JsonParseException e)
/*  84:    */     {
/*  85: 91 */       if (ETPush.getLogLevel() <= 6) {
/*  86: 92 */         Log.e("etpushsdk@JSONUtil", e.getMessage(), e);
/*  87:    */       }
/*  88:    */     }
/*  89:    */     catch (JsonMappingException e)
/*  90:    */     {
/*  91: 96 */       if (ETPush.getLogLevel() <= 6) {
/*  92: 97 */         Log.e("etpushsdk@JSONUtil", e.getMessage(), e);
/*  93:    */       }
/*  94:    */     }
/*  95:    */     catch (JsonProcessingException e)
/*  96:    */     {
/*  97:101 */       if (ETPush.getLogLevel() <= 6) {
/*  98:102 */         Log.e("etpushsdk@JSONUtil", e.getMessage(), e);
/*  99:    */       }
/* 100:    */     }
/* 101:    */     catch (IOException e)
/* 102:    */     {
/* 103:106 */       if (ETPush.getLogLevel() <= 6) {
/* 104:107 */         Log.e("etpushsdk@JSONUtil", e.getMessage(), e);
/* 105:    */       }
/* 106:    */     }
/* 107:110 */     return null;
/* 108:    */   }
/* 109:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.util.JSONUtil
 * JD-Core Version:    0.7.0.1
 */