/*   1:    */ package com.exacttarget.etpushsdk.location.receiver;
/*   2:    */ 
/*   3:    */ import android.app.PendingIntent;
/*   4:    */ import android.content.BroadcastReceiver;
/*   5:    */ import android.content.Context;
/*   6:    */ import android.content.Intent;
/*   7:    */ import android.location.LocationManager;
/*   8:    */ import android.os.Handler;
/*   9:    */ import android.preference.PreferenceManager;
/*  10:    */ import android.util.Log;
/*  11:    */ import com.exacttarget.etpushsdk.Config;
/*  12:    */ import com.exacttarget.etpushsdk.ETException;
/*  13:    */ import com.exacttarget.etpushsdk.ETLocationManager;
/*  14:    */ import com.exacttarget.etpushsdk.ETPush;
/*  15:    */ import com.exacttarget.etpushsdk.data.ETSqliteOpenHelper;
/*  16:    */ import com.exacttarget.etpushsdk.data.Region;
/*  17:    */ import com.exacttarget.etpushsdk.location.ETLocationUpdateRequester;
/*  18:    */ import com.exacttarget.etpushsdk.location.LocationUpdateRequester;
/*  19:    */ import com.j256.ormlite.dao.Dao;
/*  20:    */ import com.j256.ormlite.stmt.UpdateBuilder;
/*  21:    */ import java.sql.SQLException;
/*  22:    */ 
/*  23:    */ public class BootReceiver
/*  24:    */   extends BroadcastReceiver
/*  25:    */ {
/*  26:    */   private static final String TAG = "etpushsdk@BootReceiver";
/*  27:    */   
/*  28:    */   public void onReceive(Context context, Intent intent)
/*  29:    */   {
/*  30: 58 */     if (ETPush.getLogLevel() <= 3) {
/*  31: 59 */       Log.d("etpushsdk@BootReceiver", "onReceive()");
/*  32:    */     }
/*  33: 64 */     if ((intent = (Boolean)Config.getETSharedPref(context, PreferenceManager.getDefaultSharedPreferences(context), "et_key_run_once", Boolean.valueOf(false))).booleanValue())
/*  34:    */     {
/*  35: 65 */       LocationManager locationManager = (LocationManager)context.getSystemService("location");
/*  36:    */       
/*  37:    */ 
/*  38:    */ 
/*  39:    */ 
/*  40: 70 */       LocationUpdateRequester locationUpdateRequester = new ETLocationUpdateRequester(locationManager);
/*  41:    */       
/*  42:    */ 
/*  43:    */ 
/*  44: 74 */       followLocationChanges = (Boolean)Config.getETSharedPref(context, PreferenceManager.getDefaultSharedPreferences(context), "et_key_follow_location_changes", Boolean.valueOf(true));
/*  45:    */       
/*  46:    */ 
/*  47:    */ 
/*  48: 78 */       final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(context.getApplicationContext());
/*  49:    */       try
/*  50:    */       {
/*  51: 81 */         (updateBuilder = helper.getRegionDao().updateBuilder()).updateColumnValue("active", Boolean.FALSE);
/*  52: 82 */         updateBuilder.update();
/*  53:    */       }
/*  54:    */       catch (SQLException e)
/*  55:    */       {
/*  56:    */         UpdateBuilder<Region, String> updateBuilder;
/*  57: 85 */         if (ETPush.getLogLevel() <= 6) {
/*  58: 86 */           Log.e("etpushsdk@BootReceiver", e.getMessage(), e);
/*  59:    */         }
/*  60:    */       }
/*  61:    */       finally
/*  62:    */       {
/*  63: 91 */         (context = new Handler(context.getApplicationContext().getMainLooper())).postDelayed(new Runnable()
/*  64:    */         {
/*  65:    */           public void run()
/*  66:    */           {
/*  67: 94 */             if ((helper != null) && (helper.isOpen())) {
/*  68: 95 */               helper.close();
/*  69:    */             }
/*  70:    */           }
/*  71: 95 */         }, 10000L);
/*  72:    */       }
/*  73:101 */       if (followLocationChanges.booleanValue())
/*  74:    */       {
/*  75:104 */         Intent passiveIntent = new Intent(context, PassiveLocationChangedReceiver.class);
/*  76:105 */         PendingIntent locationListenerPassivePendingIntent = PendingIntent.getActivity(context, 0, passiveIntent, 134217728);
/*  77:106 */         locationUpdateRequester.requestPassiveLocationUpdates(300000L, 0L, locationListenerPassivePendingIntent);
/*  78:    */       }
/*  79:    */       try
/*  80:    */       {
/*  81:110 */         if (Config.isLocationManagerActive())
/*  82:    */         {
/*  83:111 */           if (ETLocationManager.locationManager().isWatchingLocation())
/*  84:    */           {
/*  85:112 */             ETLocationManager.locationManager().setGeofenceInvalidated(true);
/*  86:113 */             ETLocationManager.locationManager().startWatchingLocation();
/*  87:    */           }
/*  88:115 */           if (ETLocationManager.locationManager().isWatchingProximity())
/*  89:    */           {
/*  90:116 */             ETLocationManager.locationManager().setProximityInvalidated(true);
/*  91:117 */             ETLocationManager.locationManager().startWatchingProximity();
/*  92:    */           }
/*  93:    */         }
/*  94:    */         return;
/*  95:    */       }
/*  96:    */       catch (ETException e)
/*  97:    */       {
/*  98:122 */         if (ETPush.getLogLevel() <= 6) {
/*  99:123 */           Log.e("etpushsdk@BootReceiver", e.getMessage(), e);
/* 100:    */         }
/* 101:    */       }
/* 102:    */     }
/* 103:    */   }
/* 104:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.location.receiver.BootReceiver
 * JD-Core Version:    0.7.0.1
 */