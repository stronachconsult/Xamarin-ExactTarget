/*   1:    */ package com.exacttarget.etpushsdk.location;
/*   2:    */ 
/*   3:    */ import android.app.PendingIntent;
/*   4:    */ import android.content.BroadcastReceiver;
/*   5:    */ import android.content.Context;
/*   6:    */ import android.content.Intent;
/*   7:    */ import android.content.IntentFilter;
/*   8:    */ import android.location.Criteria;
/*   9:    */ import android.location.Location;
/*  10:    */ import android.location.LocationListener;
/*  11:    */ import android.location.LocationManager;
/*  12:    */ import android.os.Bundle;
/*  13:    */ import android.util.Log;
/*  14:    */ import com.exacttarget.etpushsdk.ETException;
/*  15:    */ import com.exacttarget.etpushsdk.ETLocationManager;
/*  16:    */ import com.exacttarget.etpushsdk.ETPush;
/*  17:    */ import com.exacttarget.etpushsdk.event.LastKnownLocationEvent;
/*  18:    */ import com.exacttarget.etpushsdk.util.EventBus;
/*  19:    */ import java.util.List;
/*  20:    */ 
/*  21:    */ public class ETLastLocationFinder
/*  22:    */   implements ILastLocationFinder
/*  23:    */ {
/*  24:    */   private static final String TAG = "etpushsdk@LastLocationFinder";
/*  25: 59 */   protected static String SINGLE_LOCATION_UPDATE_ACTION = "com.exacttarget.etpushsdk.SINGLE_LOCATION_UPDATE_ACTION";
/*  26:    */   protected PendingIntent singleUpatePI;
/*  27:    */   protected LocationListener locationListener;
/*  28:    */   protected LocationManager locationManager;
/*  29:    */   protected Context context;
/*  30:    */   protected Criteria criteria;
/*  31:    */   
/*  32:    */   public ETLastLocationFinder(Context context)
/*  33:    */   {
/*  34: 73 */     this.context = context;
/*  35: 74 */     this.locationManager = ((LocationManager)context.getSystemService("location"));
/*  36: 75 */     this.criteria = new Criteria();
/*  37: 76 */     this.criteria.setAccuracy(1);
/*  38:    */     
/*  39:    */ 
/*  40:    */ 
/*  41: 80 */     Intent updateIntent = new Intent(SINGLE_LOCATION_UPDATE_ACTION);
/*  42: 81 */     this.singleUpatePI = PendingIntent.getBroadcast(context, 0, updateIntent, 134217728);
/*  43:    */   }
/*  44:    */   
/*  45:    */   public Location getLastBestLocation(int minDistance, long minTime)
/*  46:    */   {
/*  47: 95 */     Location bestResult = null;
/*  48: 96 */     float bestAccuracy = 3.4028235E+38F;
/*  49: 97 */     long bestTime = -9223372036854775808L;
/*  50:    */     List localList;
/*  51:103 */     for (String provider : localList = this.locationManager.getAllProviders())
/*  52:    */     {
/*  53:    */       Location location;
/*  54:105 */       if ((location = this.locationManager.getLastKnownLocation(provider)) != null)
/*  55:    */       {
/*  56:106 */         float accuracy = location.getAccuracy();
/*  57:    */         long time;
/*  58:113 */         if (((time = location.getTime()) > minTime) && (accuracy <= bestAccuracy) && (time > bestTime))
/*  59:    */         {
/*  60:114 */           bestResult = location;
/*  61:115 */           bestAccuracy = accuracy;
/*  62:116 */           bestTime = time;
/*  63:    */         }
/*  64:118 */         else if ((time < minTime) && (bestAccuracy == 3.4028235E+38F) && (time > bestTime))
/*  65:    */         {
/*  66:119 */           bestResult = location;
/*  67:120 */           bestTime = time;
/*  68:    */         }
/*  69:    */       }
/*  70:    */     }
/*  71:131 */     if ((bestTime < minTime) || (bestAccuracy > minDistance))
/*  72:    */     {
/*  73:132 */       if (ETPush.getLogLevel() <= 3) {
/*  74:133 */         Log.d("etpushsdk@LastLocationFinder", "starting singleUpdateReceiver");
/*  75:    */       }
/*  76:135 */       IntentFilter locIntentFilter = new IntentFilter(SINGLE_LOCATION_UPDATE_ACTION);
/*  77:136 */       this.context.registerReceiver(this.singleUpdateReceiver, locIntentFilter);
/*  78:    */       try
/*  79:    */       {
/*  80:138 */         this.locationManager.requestSingleUpdate(this.criteria, this.singleUpatePI);
/*  81:    */       }
/*  82:    */       catch (IllegalArgumentException e)
/*  83:    */       {
/*  84:141 */         if (ETPush.getLogLevel() <= 6) {
/*  85:142 */           Log.e("etpushsdk@LastLocationFinder", e.getMessage(), e);
/*  86:    */         }
/*  87:    */       }
/*  88:    */     }
/*  89:    */     else
/*  90:    */     {
/*  91:148 */       EventBus.getDefault().postSticky(new LastKnownLocationEvent(bestResult));
/*  92:    */     }
/*  93:151 */     return bestResult;
/*  94:    */   }
/*  95:    */   
/*  96:159 */   protected BroadcastReceiver singleUpdateReceiver = new BroadcastReceiver()
/*  97:    */   {
/*  98:    */     public void onReceive(Context context, Intent intent)
/*  99:    */     {
/* 100:    */       ;
/* 101:162 */       if (ETPush.getLogLevel() <= 3) {
/* 102:163 */         Log.d("etpushsdk@LastLocationFinder", "onReceive()");
/* 103:    */       }
/* 104:    */       try
/* 105:    */       {
/* 106:167 */         context.unregisterReceiver(ETLastLocationFinder.this.singleUpdateReceiver);
/* 107:    */       }
/* 108:    */       catch (IllegalArgumentException ex)
/* 109:    */       {
/* 110:170 */         if (ETPush.getLogLevel() <= 6) {
/* 111:171 */           Log.e("etpushsdk@LastLocationFinder", ex.getMessage());
/* 112:    */         }
/* 113:173 */         return;
/* 114:    */       }
/* 115:    */       try
/* 116:    */       {
/* 117:177 */         if (ETLocationManager.locationManager().areLocationProvidersAvailable())
/* 118:    */         {
/* 119:179 */           String key = "location";
/* 120:180 */           Location location = (Location)intent.getExtras().get(key);
/* 121:182 */           if ((ETLastLocationFinder.this.locationListener != null) && (location != null)) {
/* 122:183 */             ETLastLocationFinder.this.locationListener.onLocationChanged(location);
/* 123:    */           }
/* 124:185 */           EventBus.getDefault().postSticky(new LastKnownLocationEvent(location));
/* 125:    */         }
/* 126:188 */         ETLastLocationFinder.this.locationManager.removeUpdates(ETLastLocationFinder.this.singleUpatePI); return;
/* 127:    */       }
/* 128:    */       catch (ETException e)
/* 129:    */       {
/* 130:191 */         if (ETPush.getLogLevel() <= 6) {
/* 131:192 */           Log.e("etpushsdk@LastLocationFinder", e.getMessage());
/* 132:    */         }
/* 133:    */       }
/* 134:    */     }
/* 135:    */   };
/* 136:    */   
/* 137:    */   public void setChangedLocationListener(LocationListener l)
/* 138:    */   {
/* 139:202 */     this.locationListener = l;
/* 140:    */   }
/* 141:    */   
/* 142:    */   public void cancel()
/* 143:    */   {
/* 144:209 */     this.locationManager.removeUpdates(this.singleUpatePI);
/* 145:    */   }
/* 146:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.location.ETLastLocationFinder
 * JD-Core Version:    0.7.0.1
 */