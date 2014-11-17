/*  1:   */ package com.exacttarget.etpushsdk.data;
/*  2:   */ 
/*  3:   */ import android.content.Context;
/*  4:   */ import android.provider.Settings.Secure;
/*  5:   */ import android.util.Log;
/*  6:   */ import com.exacttarget.etpushsdk.ETPush;
/*  7:   */ import java.math.BigInteger;
/*  8:   */ import java.security.MessageDigest;
/*  9:   */ 
/* 10:   */ public class DeviceData
/* 11:   */ {
/* 12:   */   private static final String TAG = "etpushsdk@DeviceData";
/* 13:29 */   private static String hashedId = null;
/* 14:   */   
/* 15:   */   public String uniqueDeviceIdentifier(Context context)
/* 16:   */   {
/* 17:37 */     if (hashedId == null)
/* 18:   */     {
/* 19:38 */       hashedId = "";
/* 20:   */       try
/* 21:   */       {
/* 22:40 */         String preHashString = Settings.Secure.getString(context.getContentResolver(), "android_id") + "-" + context.getPackageName();
/* 23:41 */         hashedId = md5(preHashString);
/* 24:   */       }
/* 25:   */       catch (Throwable e)
/* 26:   */       {
/* 27:44 */         if (ETPush.getLogLevel() <= 6) {
/* 28:45 */           Log.e("etpushsdk@DeviceData", e.getMessage(), e);
/* 29:   */         }
/* 30:   */       }
/* 31:   */     }
/* 32:49 */     return hashedId;
/* 33:   */   }
/* 34:   */   
/* 35:   */   private String md5(String id)
/* 36:   */   {
/* 37:53 */     String idHash = "";
/* 38:   */     try
/* 39:   */     {
/* 40:55 */       MessageDigest idDigest = MessageDigest.getInstance("MD5");
/* 41:56 */       byte[] idBytes = id.getBytes();
/* 42:57 */       idDigest.update(idBytes, 0, idBytes.length);
/* 43:58 */       idHash = new BigInteger(1, idDigest.digest()).toString(16);
/* 44:   */     }
/* 45:   */     catch (Throwable e)
/* 46:   */     {
/* 47:61 */       if (ETPush.getLogLevel() <= 6) {
/* 48:62 */         Log.e("etpushsdk@DeviceData", e.getMessage(), e);
/* 49:   */       }
/* 50:   */     }
/* 51:65 */     return idHash;
/* 52:   */   }
/* 53:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.DeviceData
 * JD-Core Version:    0.7.0.1
 */