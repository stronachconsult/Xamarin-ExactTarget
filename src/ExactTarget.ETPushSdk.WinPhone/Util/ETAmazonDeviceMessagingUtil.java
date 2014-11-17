/*  1:   */ package com.exacttarget.etpushsdk.util;
/*  2:   */ 
/*  3:   */ import android.content.Context;
/*  4:   */ import android.os.Build;
/*  5:   */ import android.util.Log;
/*  6:   */ import com.exacttarget.etpushsdk.ETException;
/*  7:   */ import com.exacttarget.etpushsdk.ETPush;
/*  8:   */ import java.lang.reflect.Method;
/*  9:   */ 
/* 10:   */ public class ETAmazonDeviceMessagingUtil
/* 11:   */ {
/* 12:13 */   private static Boolean admAvailable = Boolean.valueOf(false);
/* 13:   */   private static final String TAG = "etpushsdk@ETAmazonDeviceMessagingUtil";
/* 14:   */   
/* 15:   */   public static boolean isAvailable(Context applicationContext, boolean enablingPush)
/* 16:   */   {
/* 17:18 */     if (!isAmazonDevice())
/* 18:   */     {
/* 19:20 */       admAvailable = Boolean.valueOf(false);
/* 20:   */     }
/* 21:   */     else
/* 22:   */     {
/* 23:   */       try
/* 24:   */       {
/* 25:24 */         Class.forName("com.amazon.device.messaging.ADM");
/* 26:25 */         ADMManifest_checkManifestAuthoredProperly(applicationContext);
/* 27:26 */         admAvailable = Boolean.TRUE;
/* 28:   */       }
/* 29:   */       catch (ClassNotFoundException localClassNotFoundException)
/* 30:   */       {
/* 31:29 */         if (ETPush.getLogLevel() <= 3) {
/* 32:30 */           Log.e("etpushsdk@ETAmazonDeviceMessagingUtil", "Amazon ADM API's not found.");
/* 33:   */         }
/* 34:32 */         admAvailable = Boolean.FALSE;
/* 35:   */       }
/* 36:   */       catch (Exception e)
/* 37:   */       {
/* 38:35 */         if (ETPush.getLogLevel() <= 6) {
/* 39:36 */           Log.e("etpushsdk@ETAmazonDeviceMessagingUtil", e.getMessage());
/* 40:   */         }
/* 41:38 */         admAvailable = Boolean.FALSE;
/* 42:   */       }
/* 43:41 */       if (!admAvailable.booleanValue()) {
/* 44:42 */         ETGooglePlayServicesUtil.showErrorNotification(applicationContext, "Amazon Device Messaging not available.", false, enablingPush);
/* 45:   */       }
/* 46:   */     }
/* 47:46 */     return admAvailable.booleanValue();
/* 48:   */   }
/* 49:   */   
/* 50:   */   public static boolean isAmazonDevice()
/* 51:   */   {
/* 52:50 */     return Build.MANUFACTURER.equals("Amazon");
/* 53:   */   }
/* 54:   */   
/* 55:   */   private static void ADMManifest_checkManifestAuthoredProperly(Context applicationContext)
/* 56:   */     throws ETException
/* 57:   */   {
/* 58:   */     try
/* 59:   */     {
/* 60:   */       Object localObject;
/* 61:57 */       (localObject = (localObject = Class.forName("com.amazon.device.messaging.development.ADMManifest")).getDeclaredMethod("checkManifestAuthoredProperly", new Class[] { Context.class })).invoke(null, new Object[] { applicationContext });
/* 62:   */       
/* 63:   */ 
/* 64:   */ 
/* 65:   */ 
/* 66:   */ 
/* 67:   */ 
/* 68:64 */       return;
/* 69:   */     }
/* 70:   */     catch (ClassNotFoundException localClassNotFoundException)
/* 71:   */     {
/* 72:60 */       throw new ETException("etpushsdk@ETAmazonDeviceMessagingUtil unable to find com.amazon.device.messaging.development.ADMManifest");
/* 73:   */     }
/* 74:   */     catch (Exception e)
/* 75:   */     {
/* 76:63 */       throw new ETException(e.getCause().getMessage());
/* 77:   */     }
/* 78:   */   }
/* 79:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.util.ETAmazonDeviceMessagingUtil
 * JD-Core Version:    0.7.0.1
 */