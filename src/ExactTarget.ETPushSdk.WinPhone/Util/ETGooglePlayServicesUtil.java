/*  1:   */ package com.exacttarget.etpushsdk.util;
/*  2:   */ 
/*  3:   */ import android.app.Notification;
/*  4:   */ import android.app.NotificationManager;
/*  5:   */ import android.app.PendingIntent;
/*  6:   */ import android.content.Context;
/*  7:   */ import android.content.Intent;
/*  8:   */ import android.content.SharedPreferences;
/*  9:   */ import android.content.SharedPreferences.Editor;
/* 10:   */ import android.content.pm.ApplicationInfo;
/* 11:   */ import android.net.Uri;
/* 12:   */ import android.preference.PreferenceManager;
/* 13:   */ import android.provider.Settings.System;
/* 14:   */ import android.support.v4.app.NotificationCompat.Builder;
/* 15:   */ import android.util.Log;
/* 16:   */ import com.exacttarget.etpushsdk.Config;
/* 17:   */ import com.exacttarget.etpushsdk.ETPush;
/* 18:   */ import com.google.android.gms.common.GooglePlayServicesUtil;
/* 19:   */ 
/* 20:   */ public class ETGooglePlayServicesUtil
/* 21:   */ {
/* 22:   */   private static final String TAG = "etpushsdk@ETGooglePlayServicesUtil";
/* 23:21 */   private static boolean googleAvailable = false;
/* 24:   */   private static final String NOTIFICATION_REQUEST_CODE = "et_notification_play_services_error_code_key";
/* 25:   */   private static final int NOTIFICATION_ID = 913131313;
/* 26:   */   
/* 27:   */   public static boolean isAvailable(Context applicationContext, boolean enablingPush)
/* 28:   */   {
/* 29:26 */     if (!ETAmazonDeviceMessagingUtil.isAmazonDevice())
/* 30:   */     {
/* 31:27 */       if (googleAvailable) {
/* 32:   */         break label75;
/* 33:   */       }
/* 34:   */       int status;
/* 35:32 */       if ((status = GooglePlayServicesUtil.isGooglePlayServicesAvailable(applicationContext)) == 0)
/* 36:   */       {
/* 37:33 */         googleAvailable = true;
/* 38:   */         break label75;
/* 39:   */       }
/* 40:36 */       if (GooglePlayServicesUtil.isUserRecoverableError(status)) {
/* 41:37 */         showErrorNotification(applicationContext, "Google Play Services Error: " + GooglePlayServicesUtil.getErrorString(status), true, enablingPush);
/* 42:   */       } else {
/* 43:41 */         showErrorNotification(applicationContext, "Google Play Services is not supported on this Device.", false, enablingPush);
/* 44:   */       }
/* 45:   */     }
/* 46:43 */     googleAvailable = false;
/* 47:   */     label75:
/* 48:46 */     return googleAvailable;
/* 49:   */   }
/* 50:   */   
/* 51:   */   protected static void showErrorNotification(Context applicationContext, String alertMessage, boolean userRecoverable, boolean enablingPush)
/* 52:   */   {
/* 53:   */     try
/* 54:   */     {
/* 55:   */       ;
/* 56:52 */       if ((ETPush.pushManager().isPushEnabled() | enablingPush)) {
/* 57:53 */         displayErrorNotification(applicationContext, alertMessage, userRecoverable);
/* 58:   */       }
/* 59:   */       return;
/* 60:   */     }
/* 61:   */     catch (Exception e)
/* 62:   */     {
/* 63:57 */       if (ETPush.getLogLevel() <= 6) {
/* 64:58 */         Log.e("etpushsdk@ETGooglePlayServicesUtil", e.getMessage(), e);
/* 65:   */       }
/* 66:   */     }
/* 67:   */   }
/* 68:   */   
/* 69:   */   protected static void displayErrorNotification(Context applicationContext, String alertMessage, boolean userRecoverable)
/* 70:   */   {
/* 71:   */     ;
/* 72:65 */     NotificationCompat.Builder builder = new NotificationCompat.Builder(applicationContext);
/* 73:66 */     int appIconResourceId = applicationContext.getApplicationInfo().icon;
/* 74:67 */     builder.setSmallIcon(appIconResourceId);
/* 75:68 */     builder.setAutoCancel(true);
/* 76:   */     
/* 77:70 */     int appLabelResourceId = applicationContext.getApplicationInfo().labelRes;
/* 78:71 */     String app_name = applicationContext.getString(appLabelResourceId);
/* 79:72 */     builder.setContentTitle(app_name);
/* 80:   */     
/* 81:74 */     builder.setTicker(alertMessage);
/* 82:75 */     builder.setContentText(alertMessage);
/* 83:76 */     builder.setSound(Settings.System.DEFAULT_NOTIFICATION_URI);
/* 84:   */     
/* 85:78 */     pendingIntent = null;
/* 86:   */     Intent launchIntent;
/* 87:   */     SharedPreferences sp;
/* 88:80 */     if (userRecoverable)
/* 89:   */     {
/* 90:82 */       (pendingIntent = new Intent("android.intent.action.VIEW")).setData(Uri.parse("market://details?id=com.google.android.gms"));
/* 91:   */       
/* 92:84 */       sp = applicationContext.getSharedPreferences("ETPush", 0);
/* 93:86 */       synchronized ("et_notification_play_services_error_code_key")
/* 94:   */       {
/* 95:87 */         int uniqueId = ((Integer)Config.getETSharedPref(applicationContext, PreferenceManager.getDefaultSharedPreferences(applicationContext), "et_notification_play_services_error_code_key", Integer.valueOf(0))).intValue();
/* 96:88 */         launchIntent = PendingIntent.getActivity(applicationContext, uniqueId, launchIntent, 0);
/* 97:89 */         uniqueId++;
/* 98:90 */         sp.edit().putInt("et_notification_play_services_error_code_key", uniqueId).commit();
/* 99:   */       }
/* :0:   */     }
/* :1:94 */     builder.setContentIntent(launchIntent);
/* :2:   */     
/* :3:96 */     Notification notification = builder.build();
/* :4:97 */     (
/* :5:98 */       sp = (NotificationManager)applicationContext.getSystemService("notification")).notify(913131313, notification);
/* :6:   */   }
/* :7:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.util.ETGooglePlayServicesUtil
 * JD-Core Version:    0.7.0.1
 */