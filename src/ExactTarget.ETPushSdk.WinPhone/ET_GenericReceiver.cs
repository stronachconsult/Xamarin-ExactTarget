/*   1:    */ package com.exacttarget.etpushsdk;
/*   2:    */ 
/*   3:    */ import android.app.Notification;
/*   4:    */ import android.app.NotificationManager;
/*   5:    */ import android.app.PendingIntent;
/*   6:    */ import android.content.BroadcastReceiver;
/*   7:    */ import android.content.Context;
/*   8:    */ import android.content.Intent;
/*   9:    */ import android.content.SharedPreferences;
/*  10:    */ import android.content.SharedPreferences.Editor;
/*  11:    */ import android.content.pm.ApplicationInfo;
/*  12:    */ import android.content.pm.PackageManager;
/*  13:    */ import android.net.Uri;
/*  14:    */ import android.os.Bundle;
/*  15:    */ import android.os.Handler;
/*  16:    */ import android.preference.PreferenceManager;
/*  17:    */ import android.provider.Settings.System;
/*  18:    */ import android.support.v4.app.NotificationCompat.Builder;
/*  19:    */ import android.util.Log;
/*  20:    */ import android.webkit.URLUtil;
/*  21:    */ import com.exacttarget.etpushsdk.data.ETSqliteOpenHelper;
/*  22:    */ import com.exacttarget.etpushsdk.data.Message;
/*  23:    */ import com.exacttarget.etpushsdk.event.PushReceivedEvent;
/*  24:    */ import com.exacttarget.etpushsdk.event.ServerErrorEvent;
/*  25:    */ import com.exacttarget.etpushsdk.util.EventBus;
/*  26:    */ import com.j256.ormlite.dao.Dao;
/*  27:    */ import java.lang.reflect.Field;
/*  28:    */ import java.sql.SQLException;
/*  29:    */ 
/*  30:    */ public class ET_GenericReceiver
/*  31:    */   extends BroadcastReceiver
/*  32:    */ {
/*  33:    */   private static final String TAG = "etpushsdk@ET_GenericReceiver";
/*  34: 84 */   private SharedPreferences sp = null;
/*  35:    */   private static final String NOTIFICATION_REQUEST_CODE = "et_notification_request_code_key";
/*  36:    */   private static final String NOTIFICATION_ID = "et_notification_id_key";
/*  37:    */   
/*  38:    */   public final void onReceive(Context context, Intent intent)
/*  39:    */   {
/*  40:    */     try
/*  41:    */     {
/*  42:    */       String newUnregistered;
/*  43: 93 */       if (intent.getAction().equals("com.google.android.c2dm.intent.REGISTRATION"))
/*  44:    */       {
/*  45: 95 */         if (ETPush.getLogLevel() <= 3) {
/*  46: 96 */           Log.d("etpushsdk@ET_GenericReceiver", "Received a registration event from Google.");
/*  47:    */         }
/*  48: 99 */         String newRegistration = intent.getStringExtra("registration_id");
/*  49:100 */         String newError = intent.getStringExtra("error");
/*  50:101 */         intent = intent.getStringExtra("unregistered");
/*  51:103 */         if (newError != null)
/*  52:    */         {
/*  53:104 */           if (ETPush.getLogLevel() <= 6) {
/*  54:105 */             Log.e("etpushsdk@ET_GenericReceiver", "GCM Registration error: " + newError);
/*  55:    */           }
/*  56:    */           ServerErrorEvent errorEvent;
/*  57:108 */           (errorEvent = new ServerErrorEvent()).setMessage("GCM Registration error: " + newError);
/*  58:109 */           EventBus.getDefault().post(errorEvent);
/*  59:    */         }
/*  60:111 */         else if (newUnregistered != null)
/*  61:    */         {
/*  62:112 */           if (ETPush.getLogLevel() <= 3) {
/*  63:113 */             Log.d("etpushsdk@ET_GenericReceiver", "GCM Unregistered: " + newUnregistered);
/*  64:    */           }
/*  65:115 */           ETPush.pushManager().unregisterDeviceToken();
/*  66:    */         }
/*  67:    */         else
/*  68:    */         {
/*  69:117 */           if (newRegistration != null)
/*  70:    */           {
/*  71:118 */             if (ETPush.getLogLevel() <= 3) {
/*  72:119 */               Log.d("etpushsdk@ET_GenericReceiver", "GCM Registration complete. ID: " + newRegistration);
/*  73:    */             }
/*  74:121 */             ETPush.pushManager().registerDeviceToken(newRegistration);
/*  75:    */           }
/*  76:124 */           return;
/*  77:    */         }
/*  78:    */       }
/*  79:    */       else
/*  80:    */       {
/*  81:    */         boolean newUnregistered;
/*  82:125 */         if (newUnregistered.getAction().equals("com.amazon.device.messaging.intent.REGISTRATION"))
/*  83:    */         {
/*  84:127 */           if (ETPush.getLogLevel() <= 3) {
/*  85:128 */             Log.d("etpushsdk@ET_GenericReceiver", "Received a registration event from Amazon.");
/*  86:    */           }
/*  87:130 */           String newRegistration = newUnregistered.getStringExtra("registration_id");
/*  88:131 */           String newError = newUnregistered.getStringExtra("error");
/*  89:132 */           newUnregistered = newUnregistered.getBooleanExtra("unregistered", false);
/*  90:134 */           if (newError != null)
/*  91:    */           {
/*  92:135 */             if (ETPush.getLogLevel() <= 6)
/*  93:    */             {
/*  94:136 */               Log.e("etpushsdk@ET_GenericReceiver", "ADM Registration error: " + newError);
/*  95:    */               break label987;
/*  96:    */             }
/*  97:    */           }
/*  98:    */           else
/*  99:    */           {
/* 100:139 */             if (newUnregistered)
/* 101:    */             {
/* 102:140 */               if (ETPush.getLogLevel() <= 3) {
/* 103:141 */                 Log.d("etpushsdk@ET_GenericReceiver", "ADM Unregistered: " + newUnregistered);
/* 104:    */               }
/* 105:143 */               ETPush.pushManager().unregisterDeviceToken();
/* 106:    */               break label987;
/* 107:    */             }
/* 108:145 */             if (newRegistration != null)
/* 109:    */             {
/* 110:146 */               if (ETPush.getLogLevel() <= 3) {
/* 111:147 */                 Log.d("etpushsdk@ET_GenericReceiver", "ADM Registration complete. ID: " + newRegistration);
/* 112:    */               }
/* 113:149 */               ETPush.pushManager().registerDeviceToken(newRegistration);
/* 114:    */             }
/* 115:    */           }
/* 116:151 */           return;
/* 117:    */         }
/* 118:152 */         else if ((newUnregistered.getAction().equals("com.google.android.c2dm.intent.RECEIVE")) || (newUnregistered.getAction().equals("com.amazon.device.messaging.intent.RECEIVE")))
/* 119:    */         {
/* 120:155 */           if (ETPush.getLogLevel() <= 3) {
/* 121:156 */             Log.d("etpushsdk@ET_GenericReceiver", "Hello from ExactTarget! Push Message received.");
/* 122:    */           }
/* 123:159 */           if (!ETPush.pushManager().isPushEnabled())
/* 124:    */           {
/* 125:161 */             if (ETPush.getLogLevel() <= 3) {
/* 126:162 */               Log.d("etpushsdk@ET_GenericReceiver", "Push is disabled. Thanks for playing.");
/* 127:    */             }
/* 128:164 */             return;
/* 129:    */           }
/* 130:168 */           payload = newUnregistered.getExtras();
/* 131:169 */           if (ETPush.getLogLevel() <= 3)
/* 132:    */           {
/* 133:170 */             String payloadStr = "";
/* 134:171 */             for (String key : payload.keySet()) {
/* 135:172 */               payloadStr = payloadStr + "[" + key + ":" + payload.get(key) + "] ";
/* 136:    */             }
/* 137:175 */             Log.d("etpushsdk@ET_GenericReceiver", "Payload: " + payloadStr);
/* 138:    */           }
/* 139:178 */           if ((!payload.containsKey("regionId")) && (payload.containsKey("_m"))) {
/* 140:180 */             if (Config.isAnalyticsActive()) {
/* 141:181 */               ETAnalytics.engine().logMessageReceived(payload.getString("_m"));
/* 142:    */             }
/* 143:    */           }
/* 144:185 */           Intent launchIntent = setupLaunchIntent(context, payload);
/* 145:    */           Intent openIntent;
/* 146:188 */           (openIntent = new Intent("com.exacttarget.MESSAGE_OPENED")).putExtra("et_open_intent", launchIntent);
/* 147:    */           NotificationCompat.Builder builder;
/* 148:191 */           if (((builder = setupNotificationBuilder(context, payload)) != null) && (launchIntent != null))
/* 149:    */           {
/* 150:192 */             PendingIntent pendingIntent = setupLaunchPendingIntent(context, openIntent);
/* 151:193 */             builder.setContentIntent(pendingIntent);
/* 152:    */           }
/* 153:196 */           if (builder != null)
/* 154:    */           {
/* 155:197 */             Notification notification = builder.build();
/* 156:198 */             NotificationManager nm = (NotificationManager)context.getApplicationContext().getSystemService("notification");
/* 157:200 */             if (this.sp == null) {
/* 158:201 */               this.sp = context.getSharedPreferences("ETPush", 0);
/* 159:    */             }
/* 160:204 */             synchronized ("et_notification_id_key")
/* 161:    */             {
/* 162:205 */               int notifyId = ((Integer)Config.getETSharedPref(context, PreferenceManager.getDefaultSharedPreferences(context), "et_notification_id_key", Integer.valueOf(0))).intValue();
/* 163:206 */               nm.notify(notifyId, notification);
/* 164:207 */               if (payload.getString("_m") != null)
/* 165:    */               {
/* 166:208 */                 final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(context.getApplicationContext());
/* 167:    */                 try
/* 168:    */                 {
/* 169:    */                   Message message;
/* 170:212 */                   if ((message = (Message)(messageDao = helper.getMessageDao()).queryForId(payload.getString("_m"))) != null)
/* 171:    */                   {
/* 172:213 */                     message.setNotifyId(Integer.valueOf(notifyId));
/* 173:214 */                     messageDao.update(message);
/* 174:    */                   }
/* 175:    */                 }
/* 176:    */                 catch (SQLException e)
/* 177:    */                 {
/* 178:    */                   Dao<Message, String> messageDao;
/* 179:218 */                   if (ETPush.getLogLevel() <= 6) {
/* 180:219 */                     Log.e("etpushsdk@ET_GenericReceiver", e.getMessage(), e);
/* 181:    */                   }
/* 182:    */                 }
/* 183:    */                 finally
/* 184:    */                 {
/* 185:224 */                   (context = new Handler(context.getApplicationContext().getMainLooper())).postDelayed(new Runnable()
/* 186:    */                   {
/* 187:    */                     public void run()
/* 188:    */                     {
/* 189:227 */                       if ((helper != null) && (helper.isOpen())) {
/* 190:228 */                         helper.close();
/* 191:    */                       }
/* 192:    */                     }
/* 193:228 */                   }, 10000L);
/* 194:    */                 }
/* 195:    */               }
/* 196:234 */               notifyId++;
/* 197:235 */               this.sp.edit().putInt("et_notification_id_key", notifyId).commit();
/* 198:    */             }
/* 199:    */           }
/* 200:239 */           EventBus.getDefault().post(new PushReceivedEvent(payload));
/* 201:    */         }
/* 202:    */       }
/* 203:    */       label987:
/* 204:246 */       return;
/* 205:    */     }
/* 206:    */     catch (ETException e)
/* 207:    */     {
/* 208:243 */       if (ETPush.getLogLevel() <= 6) {
/* 209:244 */         Log.e("etpushsdk@ET_GenericReceiver", e.getMessage(), e);
/* 210:    */       }
/* 211:    */     }
/* 212:    */   }
/* 213:    */   
/* 214:    */   protected PendingIntent setupLaunchPendingIntent(Context context, Intent launchIntent)
/* 215:    */   {
/* 216:    */     ;
/* 217:256 */     if (this.sp == null) {
/* 218:258 */       this.sp = context.getSharedPreferences("ETPush", 0);
/* 219:    */     }
/* 220:261 */     synchronized ("et_notification_request_code_key")
/* 221:    */     {
/* 222:262 */       int uniqueId = ((Integer)Config.getETSharedPref(context, PreferenceManager.getDefaultSharedPreferences(context), "et_notification_request_code_key", Integer.valueOf(0))).intValue();
/* 223:    */       
/* 224:    */ 
/* 225:    */ 
/* 226:266 */       pendingIntent = PendingIntent.getBroadcast(context, uniqueId, launchIntent, 268435456);
/* 227:267 */       uniqueId++;
/* 228:268 */       this.sp.edit().putInt("et_notification_request_code_key", uniqueId).commit();
/* 229:    */     }
/* 230:271 */     return pendingIntent;
/* 231:    */   }
/* 232:    */   
/* 233:    */   public Intent setupLaunchIntent(Context context, Bundle payload)
/* 234:    */   {
/* 235:282 */     Intent launchIntent = null;
/* 236:    */     try
/* 237:    */     {
/* 238:284 */       if ((ETPush.pushManager().getNotificationAction() != null) && (ETPush.pushManager().getNotificationActionUri() != null))
/* 239:    */       {
/* 240:285 */         if (ETPush.getLogLevel() <= 3) {
/* 241:286 */           Log.d("etpushsdk@ET_GenericReceiver", "Launch Intent set to NotificationUri: " + ETPush.pushManager().getNotificationActionUri());
/* 242:    */         }
/* 243:289 */         (launchIntent = new Intent(ETPush.pushManager().getNotificationAction(), ETPush.pushManager().getNotificationActionUri())).putExtras(payload);
/* 244:    */       }
/* 245:303 */       else if (payload.getString("_x") != null)
/* 246:    */       {
/* 247:    */         String url;
/* 248:306 */         if ((URLUtil.isValidUrl(url = payload.getString("_x"))) && ((URLUtil.isHttpUrl(url)) || (URLUtil.isHttpsUrl(url))))
/* 249:    */         {
/* 250:307 */           if (ETPush.getLogLevel() <= 3) {
/* 251:308 */             Log.d("etpushsdk@ET_GenericReceiver", "Launch Intent set to Cloud Page: " + payload.getString("_x"));
/* 252:    */           }
/* 253:312 */           (launchIntent = new Intent(context, ETLandingPagePresenter.class)).putExtra("loadURL", url);
/* 254:313 */           launchIntent.putExtras(payload);
/* 255:    */         }
/* 256:    */         else
/* 257:    */         {
/* 258:316 */           if (ETPush.getLogLevel() <= 3) {
/* 259:317 */             Log.d("etpushsdk@ET_GenericReceiver", "Launch Intent set to Launch Package");
/* 260:    */           }
/* 261:321 */           (launchIntent = context.getPackageManager().getLaunchIntentForPackage(context.getPackageName())).putExtras(payload);
/* 262:    */           
/* 263:323 */           return launchIntent;
/* 264:    */         }
/* 265:    */       }
/* 266:324 */       else if ((ETPush.pushManager().getOpenDirectRecipient() != null) && (payload.getString("_od") != null))
/* 267:    */       {
/* 268:325 */         if (ETPush.getLogLevel() <= 3) {
/* 269:326 */           Log.d("etpushsdk@ET_GenericReceiver", "Launch Intent set to Open Direct: " + ETPush.pushManager().getOpenDirectRecipient());
/* 270:    */         }
/* 271:330 */         (launchIntent = new Intent(context, ETPush.pushManager().getOpenDirectRecipient())).putExtra("open_direct_payload", payload.getString("_od"));
/* 272:331 */         launchIntent.putExtras(payload);
/* 273:    */       }
/* 274:333 */       else if ((ETPush.pushManager().getOpenDirectRecipient() == null) && (payload.getString("_od") != null))
/* 275:    */       {
/* 276:    */         String url;
/* 277:336 */         if ((URLUtil.isValidUrl(url = payload.getString("_od"))) && ((URLUtil.isHttpUrl(url)) || (URLUtil.isHttpsUrl(url))))
/* 278:    */         {
/* 279:337 */           if (ETPush.getLogLevel() <= 3) {
/* 280:338 */             Log.d("etpushsdk@ET_GenericReceiver", "Launch Intent set to Open Direct: " + url);
/* 281:    */           }
/* 282:342 */           (launchIntent = new Intent(context, ETLandingPagePresenter.class)).putExtras(payload);
/* 283:    */         }
/* 284:    */         else
/* 285:    */         {
/* 286:345 */           if (ETPush.getLogLevel() <= 3) {
/* 287:346 */             Log.d("etpushsdk@ET_GenericReceiver", "Launch Intent set to Launch Package");
/* 288:    */           }
/* 289:350 */           (launchIntent = context.getPackageManager().getLaunchIntentForPackage(context.getPackageName())).putExtras(payload);
/* 290:    */           
/* 291:352 */           return launchIntent;
/* 292:    */         }
/* 293:    */       }
/* 294:353 */       else if (ETPush.pushManager().getNotificationRecipientClass() != null)
/* 295:    */       {
/* 296:354 */         if (ETPush.getLogLevel() <= 3) {
/* 297:355 */           Log.d("etpushsdk@ET_GenericReceiver", "Launch Intent set to Nofification Recipient: " + ETPush.pushManager().getNotificationRecipientClass());
/* 298:    */         }
/* 299:360 */         (launchIntent = new Intent(context, ETPush.pushManager().getNotificationRecipientClass())).putExtras(payload);
/* 300:    */       }
/* 301:    */       else
/* 302:    */       {
/* 303:364 */         if (ETPush.getLogLevel() <= 3) {
/* 304:365 */           Log.d("etpushsdk@ET_GenericReceiver", "Launch Intent set to Launch Package");
/* 305:    */         }
/* 306:368 */         (launchIntent = context.getPackageManager().getLaunchIntentForPackage(context.getPackageName())).putExtras(payload);
/* 307:    */       }
/* 308:    */     }
/* 309:    */     catch (ETException e)
/* 310:    */     {
/* 311:372 */       if (ETPush.getLogLevel() <= 6) {
/* 312:373 */         Log.e("etpushsdk@ET_GenericReceiver", e.getMessage(), e);
/* 313:    */       }
/* 314:    */     }
/* 315:377 */     return launchIntent;
/* 316:    */   }
/* 317:    */   
/* 318:    */   public NotificationCompat.Builder setupNotificationBuilder(Context context, Bundle payload)
/* 319:    */   {
/* 320:    */     ;
/* 321:389 */     NotificationCompat.Builder builder = new NotificationCompat.Builder(context);
/* 322:390 */     int appIconResourceId = context.getApplicationInfo().icon;
/* 323:391 */     builder.setSmallIcon(appIconResourceId);
/* 324:392 */     builder.setAutoCancel(true);
/* 325:    */     
/* 326:394 */     int appLabelResourceId = context.getApplicationInfo().labelRes;
/* 327:395 */     String app_name = context.getString(appLabelResourceId);
/* 328:396 */     builder.setContentTitle(app_name);
/* 329:398 */     if (payload.getString("alert") != null)
/* 330:    */     {
/* 331:400 */       builder.setTicker(payload.getString("alert"));
/* 332:401 */       builder.setContentText(payload.getString("alert"));
/* 333:    */     }
/* 334:405 */     if (payload.getString("sound") != null) {
/* 335:407 */       if (payload.getString("sound").equals("custom.caf")) {
/* 336:    */         try
/* 337:    */         {
/* 338:412 */           Field custom = (payload = Class.forName(payload = context.getPackageName() + ".R$raw")).getDeclaredField("custom");
/* 339:413 */           customSound = Uri.parse("android.resource://" + context.getPackageName() + "/" + custom.getInt(null));
/* 340:414 */           builder.setSound(customSound);
/* 341:    */         }
/* 342:    */         catch (ClassNotFoundException e)
/* 343:    */         {
/* 344:417 */           if (ETPush.getLogLevel() <= 5) {
/* 345:418 */             Log.w("etpushsdk@ET_GenericReceiver", "R.raw.custom sound requested but not defined, reverting to default notification sound.", e);
/* 346:    */           }
/* 347:420 */           builder.setSound(Settings.System.DEFAULT_NOTIFICATION_URI);
/* 348:    */         }
/* 349:    */         catch (NoSuchFieldException e)
/* 350:    */         {
/* 351:423 */           if (ETPush.getLogLevel() <= 5) {
/* 352:424 */             Log.w("etpushsdk@ET_GenericReceiver", "R.raw.custom sound requested but not defined, reverting to default notification sound.", e);
/* 353:    */           }
/* 354:426 */           builder.setSound(Settings.System.DEFAULT_NOTIFICATION_URI);
/* 355:    */         }
/* 356:    */         catch (IllegalArgumentException e)
/* 357:    */         {
/* 358:428 */           if (ETPush.getLogLevel() <= 5) {
/* 359:429 */             Log.w("etpushsdk@ET_GenericReceiver", "R.raw.custom sound requested but not defined, reverting to default notification sound.", e);
/* 360:    */           }
/* 361:431 */           builder.setSound(Settings.System.DEFAULT_NOTIFICATION_URI);
/* 362:    */         }
/* 363:    */         catch (IllegalAccessException e)
/* 364:    */         {
/* 365:433 */           if (ETPush.getLogLevel() <= 5) {
/* 366:434 */             Log.w("etpushsdk@ET_GenericReceiver", "R.raw.custom sound requested but not defined, reverting to default notification sound.", e);
/* 367:    */           }
/* 368:436 */           builder.setSound(Settings.System.DEFAULT_NOTIFICATION_URI);
/* 369:    */         }
/* 370:    */       } else {
/* 371:440 */         builder.setSound(Settings.System.DEFAULT_NOTIFICATION_URI);
/* 372:    */       }
/* 373:    */     }
/* 374:444 */     return builder;
/* 375:    */   }
/* 376:    */ }



/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar

 * Qualified Name:     com.exacttarget.etpushsdk.ET_GenericReceiver

 * JD-Core Version:    0.7.0.1

 */