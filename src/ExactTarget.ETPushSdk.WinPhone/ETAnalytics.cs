/*   1:    */ package com.exacttarget.etpushsdk;
/*   2:    */ 
/*   3:    */ import android.content.Context;
/*   4:    */ import android.content.Intent;
/*   5:    */ import android.os.Handler;
/*   6:    */ import android.os.Looper;
/*   7:    */ import android.util.Log;
/*   8:    */ import com.exacttarget.etpushsdk.data.AnalyticItem;
/*   9:    */ import com.exacttarget.etpushsdk.data.ETSqliteOpenHelper;
/*  10:    */ import com.exacttarget.etpushsdk.event.AnalyticItemEvent;
/*  11:    */ import com.exacttarget.etpushsdk.event.AnalyticItemEventListener;
/*  12:    */ import com.exacttarget.etpushsdk.event.BackgroundEvent;
/*  13:    */ import com.exacttarget.etpushsdk.event.BackgroundEventListener;
/*  14:    */ import com.exacttarget.etpushsdk.util.EventBus;
/*  15:    */ import com.j256.ormlite.dao.Dao;
/*  16:    */ import com.j256.ormlite.stmt.QueryBuilder;
/*  17:    */ import com.j256.ormlite.stmt.Where;
/*  18:    */ import java.sql.SQLException;
/*  19:    */ import java.util.Date;
/*  20:    */ import java.util.Iterator;
/*  21:    */ import java.util.List;
/*  22:    */ 
/*  23:    */ public class ETAnalytics
/*  24:    */   implements AnalyticItemEventListener, BackgroundEventListener
/*  25:    */ {
/*  26:    */   private static final String TAG = "etpushsdk@ETAnalytics";
/*  27:    */   private static ETAnalytics engine;
/*  28:    */   private Context applicationContext;
/*  29:    */   
/*  30:    */   private ETAnalytics(Context applicationContext)
/*  31:    */   {
/*  32: 79 */     Looper looper = Looper.getMainLooper();
/*  33: 80 */     (
/*  34: 81 */       looper = new Handler(looper)).post(new Runnable()
/*  35:    */       {
/*  36:    */         public void run()
/*  37:    */         {
/*  38:    */           try
/*  39:    */           {
/*  40: 84 */             Class.forName("android.os.AsyncTask"); return;
/*  41:    */           }
/*  42:    */           catch (ClassNotFoundException e)
/*  43:    */           {
/*  44: 86 */             if (ETPush.getLogLevel() <= 6) {
/*  45: 87 */               Log.e("etpushsdk@ETAnalytics", e.getMessage(), e);
/*  46:    */             }
/*  47:    */           }
/*  48:    */         }
/*  49: 94 */       });
/*  50: 95 */     this.applicationContext = applicationContext;
/*  51:    */     
/*  52: 97 */     EventBus.getDefault().register(this);
/*  53:    */   }
/*  54:    */   
/*  55:    */   public static ETAnalytics engine()
/*  56:    */     throws ETException
/*  57:    */   {
/*  58:106 */     if (Config.isAnalyticsActive())
/*  59:    */     {
/*  60:107 */       if (engine == null) {
/*  61:108 */         throw new ETException("You forgot to call readyAimFire first.");
/*  62:    */       }
/*  63:111 */       return engine;
/*  64:    */     }
/*  65:114 */     throw new ETException("ETAnalytics is disabled.");
/*  66:    */   }
/*  67:    */   
/*  68:    */   protected static void resetAnalytics(Context applicationContext)
/*  69:    */   {
/*  70:123 */     engine = null;
/*  71:    */   }
/*  72:    */   
/*  73:    */   protected static void readyAimFire(Context applicationContext)
/*  74:    */     throws ETException
/*  75:    */   {
/*  76:131 */     if (engine == null)
/*  77:    */     {
/*  78:132 */       if (ETPush.getLogLevel() <= 3) {
/*  79:133 */         Log.d("etpushsdk@ETAnalytics", "readyAimFire()");
/*  80:    */       }
/*  81:135 */       engine = new ETAnalytics(applicationContext);return;
/*  82:    */     }
/*  83:138 */     throw new ETException("You must have called readyAimFire more than once.");
/*  84:    */   }
/*  85:    */   
/*  86:    */   public void onEvent(AnalyticItemEvent event)
/*  87:    */   {
/*  88:148 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/*  89:    */     try
/*  90:    */     {
/*  91:    */       int rowsUpdated;
/*  92:    */       Iterator i$;
/*  93:150 */       if ((event.getDatabaseIds() != null) && (event.getDatabaseIds().size() > 0))
/*  94:    */       {
/*  95:151 */         event = helper.getAnalyticItemDao().deleteIds(event.getDatabaseIds());
/*  96:152 */         if (ETPush.getLogLevel() <= 3) {
/*  97:153 */           Log.e("etpushsdk@ETAnalytics", "Error: analytic_item rows deleted = " + rowsUpdated);
/*  98:    */         }
/*  99:    */       }
/* 100:    */       else
/* 101:    */       {
/* 102:157 */         for (i$ = rowsUpdated.iterator(); i$.hasNext();)
/* 103:    */         {
/* 104:157 */           analyticItem = (AnalyticItem)i$.next();
/* 105:    */           int rowsUpdated;
/* 106:160 */           if ((rowsUpdated = helper.getAnalyticItemDao().deleteById(analyticItem.getId())) == 1)
/* 107:    */           {
/* 108:161 */             if (ETPush.getLogLevel() <= 3) {
/* 109:162 */               Log.d("etpushsdk@ETAnalytics", "removed analytic_item id: " + analyticItem.getId());
/* 110:    */             }
/* 111:    */           }
/* 112:166 */           else if (ETPush.getLogLevel() <= 6) {
/* 113:167 */             Log.e("etpushsdk@ETAnalytics", "Error: rowsUpdated = " + rowsUpdated);
/* 114:    */           }
/* 115:    */         }
/* 116:    */       }
/* 117:    */     }
/* 118:    */     catch (SQLException e)
/* 119:    */     {
/* 120:173 */       if (ETPush.getLogLevel() <= 6) {
/* 121:174 */         Log.e("etpushsdk@ETAnalytics", e.getMessage(), e);
/* 122:    */       }
/* 123:    */     }
/* 124:    */     finally
/* 125:    */     {
/* 126:    */       AnalyticItem analyticItem;
/* 127:179 */       (analyticItem = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 128:    */       {
/* 129:    */         public void run()
/* 130:    */         {
/* 131:182 */           if ((helper != null) && (helper.isOpen())) {
/* 132:183 */             helper.close();
/* 133:    */           }
/* 134:    */         }
/* 135:183 */       }, 10000L);
/* 136:    */     }
/* 137:    */   }
/* 138:    */   
/* 139:    */   protected void endTimeInAppCounter()
/* 140:    */   {
/* 141:191 */     now = System.currentTimeMillis();
/* 142:192 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 143:    */     try
/* 144:    */     {
/* 145:    */       Dao<AnalyticItem, Integer> analyticItemDao;
/* 146:    */       List localList;
/* 147:203 */       for (AnalyticItem analyticItem : localList = (analyticItemDao = helper.getAnalyticItemDao()).queryBuilder().orderBy("id", true).where().like("analytic_types", "%4%").and().isNull("value").query())
/* 148:    */       {
/* 149:204 */         int timeInApp = (int)((now - analyticItem.getEventDate().getTime()) / 1000L - 2L);
/* 150:205 */         if (ETPush.getLogLevel() <= 3) {
/* 151:206 */           Log.d("etpushsdk@ETAnalytics", "Time in app was " + timeInApp + " seconds");
/* 152:    */         }
/* 153:208 */         analyticItem.setValue(Integer.valueOf(timeInApp));
/* 154:209 */         analyticItem.setReadyToSend(Boolean.TRUE);
/* 155:210 */         analyticItemDao.update(analyticItem);
/* 156:    */       }
/* 157:    */     }
/* 158:    */     catch (SQLException e)
/* 159:    */     {
/* 160:214 */       if (ETPush.getLogLevel() <= 6) {
/* 161:215 */         Log.e("etpushsdk@ETAnalytics", e.getMessage(), e);
/* 162:    */       }
/* 163:    */     }
/* 164:    */     finally
/* 165:    */     {
/* 166:    */       Handler localHandler;
/* 167:220 */       (localHandler = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 168:    */       {
/* 169:    */         public void run()
/* 170:    */         {
/* 171:223 */           if ((helper != null) && (helper.isOpen())) {
/* 172:224 */             helper.close();
/* 173:    */           }
/* 174:    */         }
/* 175:224 */       }, 10000L);
/* 176:    */     }
/* 177:    */   }
/* 178:    */   
/* 179:    */   private boolean isCountingTimeInApp()
/* 180:    */   {
/* 181:232 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 182:    */     try
/* 183:    */     {
/* 184:    */       Object localObject1;
/* 185:243 */       return (localObject1 = (localObject1 = helper.getAnalyticItemDao()).queryBuilder().orderBy("id", true).where().like("analytic_types", "%4%").and().isNull("value").query()).size() > 0;
/* 186:    */     }
/* 187:    */     catch (SQLException e)
/* 188:    */     {
/* 189:246 */       if (ETPush.getLogLevel() <= 6) {
/* 190:247 */         Log.e("etpushsdk@ETAnalytics", ((SQLException)e).getMessage(), (Throwable)e);
/* 191:    */       }
/* 192:    */     }
/* 193:    */     finally
/* 194:    */     {
/* 195:    */       Handler localHandler;
/* 196:252 */       (localHandler = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 197:    */       {
/* 198:    */         public void run()
/* 199:    */         {
/* 200:255 */           if ((helper != null) && (helper.isOpen())) {
/* 201:256 */             helper.close();
/* 202:    */           }
/* 203:    */         }
/* 204:256 */       }, 10000L);
/* 205:    */     }
/* 206:262 */     return false;
/* 207:    */   }
/* 208:    */   
/* 209:    */   public void onEvent(BackgroundEvent event)
/* 210:    */   {
/* 211:266 */     if (Config.isAnalyticsActive())
/* 212:    */     {
/* 213:267 */       if (event.isInBackground())
/* 214:    */       {
/* 215:268 */         endTimeInAppCounter();
/* 216:    */         Intent sendDataIntent;
/* 217:272 */         (sendDataIntent = new Intent(this.applicationContext, ETSendDataReceiver.class)).putExtra("et_send_type_extra", "et_send_type_analytic_events");
/* 218:273 */         this.applicationContext.sendBroadcast(sendDataIntent);
/* 219:274 */         return;
/* 220:    */       }
/* 221:277 */       if (!isCountingTimeInApp())
/* 222:    */       {
/* 223:279 */         final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 224:    */         try
/* 225:    */         {
/* 226:281 */           Dao<AnalyticItem, Integer> analyticItemDao = helper.getAnalyticItemDao();
/* 227:    */           
/* 228:    */ 
/* 229:284 */           (
/* 230:285 */             analyticItem = new AnalyticItem(this.applicationContext)).setEventDate(new Date());
/* 231:286 */           analyticItem.addAnalyticType(4);
/* 232:287 */           analyticItemDao.create(analyticItem);
/* 233:    */         }
/* 234:    */         catch (SQLException e)
/* 235:    */         {
/* 236:290 */           if (ETPush.getLogLevel() <= 6) {
/* 237:291 */             Log.e("etpushsdk@ETAnalytics", e.getMessage(), e);
/* 238:    */           }
/* 239:    */         }
/* 240:    */         finally
/* 241:    */         {
/* 242:    */           AnalyticItem analyticItem;
/* 243:296 */           (analyticItem = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 244:    */           {
/* 245:    */             public void run()
/* 246:    */             {
/* 247:299 */               if ((helper != null) && (helper.isOpen())) {
/* 248:300 */                 helper.close();
/* 249:    */               }
/* 250:    */             }
/* 251:300 */           }, 10000L);
/* 252:    */         }
/* 253:    */       }
/* 254:    */     }
/* 255:    */   }
/* 256:    */   
/* 257:    */   protected void logFenceOrProximityMessageDisplayed(String regionId, int transitionType, int proximity, List<String> messageIds)
/* 258:    */   {
/* 259:    */     ;
/* 260:    */     ;
/* 261:311 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 262:    */     try
/* 263:    */     {
/* 264:313 */       Dao<AnalyticItem, Integer> analyticItemDao = helper.getAnalyticItemDao();
/* 265:    */       AnalyticItem analyticItem;
/* 266:316 */       (analyticItem = new AnalyticItem(this.applicationContext)).setEventDate(new Date());
/* 267:317 */       if (transitionType == 1)
/* 268:    */       {
/* 269:318 */         analyticItem.addAnalyticType(6);
/* 270:319 */         analyticItem.addObjectId(regionId);
/* 271:    */       }
/* 272:321 */       else if (transitionType == 2)
/* 273:    */       {
/* 274:322 */         analyticItem.addAnalyticType(7);
/* 275:323 */         analyticItem.addObjectId(regionId);
/* 276:    */       }
/* 277:    */       else
/* 278:    */       {
/* 279:326 */         analyticItem.addAnalyticType(12);
/* 280:327 */         analyticItem.addObjectId(regionId);
/* 281:    */       }
/* 282:330 */       analyticItem.addAnalyticType(3);
/* 283:331 */       for (i$ = messageIds.iterator(); i$.hasNext();)
/* 284:    */       {
/* 285:331 */         messageId = (String)i$.next();
/* 286:332 */         analyticItem.addObjectId(messageId);
/* 287:    */       }
/* 288:334 */       analyticItem.setReadyToSend(Boolean.TRUE);
/* 289:335 */       analyticItemDao.create(analyticItem);
/* 290:    */     }
/* 291:    */     catch (SQLException e)
/* 292:    */     {
/* 293:338 */       if (ETPush.getLogLevel() <= 6) {
/* 294:339 */         Log.e("etpushsdk@ETAnalytics", e.getMessage(), e);
/* 295:    */       }
/* 296:    */     }
/* 297:    */     finally
/* 298:    */     {
/* 299:344 */       (messageId = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 300:    */       {
/* 301:    */         public void run()
/* 302:    */         {
/* 303:347 */           if ((helper != null) && (helper.isOpen())) {
/* 304:348 */             helper.close();
/* 305:    */           }
/* 306:    */         }
/* 307:348 */       }, 10000L);
/* 308:    */     }
/* 309:    */   }
/* 310:    */   
/* 311:    */   protected void startTimeInRegionLog(String regionId, boolean isBeacon)
/* 312:    */   {
/* 313:356 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 314:    */     try
/* 315:    */     {
/* 316:358 */       Dao<AnalyticItem, Integer> analyticItemDao = helper.getAnalyticItemDao();
/* 317:    */       AnalyticItem analyticItem;
/* 318:361 */       (analyticItem = new AnalyticItem(this.applicationContext)).setEventDate(new Date());
/* 319:362 */       if (!isBeacon) {
/* 320:363 */         analyticItem.addAnalyticType(11);
/* 321:    */       } else {
/* 322:366 */         analyticItem.addAnalyticType(13);
/* 323:    */       }
/* 324:368 */       analyticItem.addObjectId(regionId);
/* 325:369 */       analyticItemDao.create(analyticItem);
/* 326:    */     }
/* 327:    */     catch (SQLException e)
/* 328:    */     {
/* 329:372 */       if (ETPush.getLogLevel() <= 6) {
/* 330:373 */         Log.e("etpushsdk@ETAnalytics", e.getMessage(), e);
/* 331:    */       }
/* 332:    */     }
/* 333:    */     finally
/* 334:    */     {
/* 335:378 */       (isBeacon = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 336:    */       {
/* 337:    */         public void run()
/* 338:    */         {
/* 339:381 */           if ((helper != null) && (helper.isOpen())) {
/* 340:382 */             helper.close();
/* 341:    */           }
/* 342:    */         }
/* 343:382 */       }, 10000L);
/* 344:    */     }
/* 345:    */   }
/* 346:    */   
/* 347:    */   protected void stopTimeInRegionLog(String regionId)
/* 348:    */   {
/* 349:390 */     long now = System.currentTimeMillis();
/* 350:391 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 351:    */     try
/* 352:    */     {
/* 353:    */       Dao<AnalyticItem, Integer> analyticItemDao;
/* 354:    */       List<AnalyticItem> analyticItems;
/* 355:402 */       if (((analyticItems = (analyticItemDao = helper.getAnalyticItemDao()).queryBuilder().where().like("object_ids", "%" + regionId + "%").and().isNull("value").query()) != null) && (analyticItems.size() > 0)) {
/* 356:403 */         for (AnalyticItem analyticItem : analyticItems)
/* 357:    */         {
/* 358:404 */           int timeInRegion = (int)((now - analyticItem.getEventDate().getTime()) / 1000L - 2L);
/* 359:405 */           if (ETPush.getLogLevel() <= 3) {
/* 360:406 */             Log.d("etpushsdk@ETAnalytics", "Time in region: " + regionId + " was " + timeInRegion + " seconds");
/* 361:    */           }
/* 362:408 */           analyticItem.setValue(Integer.valueOf(timeInRegion));
/* 363:409 */           analyticItem.setReadyToSend(Boolean.TRUE);
/* 364:410 */           analyticItemDao.update(analyticItem);
/* 365:    */         }
/* 366:    */       }
/* 367:    */     }
/* 368:    */     catch (SQLException e)
/* 369:    */     {
/* 370:415 */       if (ETPush.getLogLevel() <= 6) {
/* 371:416 */         Log.e("etpushsdk@ETAnalytics", e.getMessage(), e);
/* 372:    */       }
/* 373:    */     }
/* 374:    */     finally
/* 375:    */     {
/* 376:421 */       (now = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 377:    */       {
/* 378:    */         public void run()
/* 379:    */         {
/* 380:424 */           if ((helper != null) && (helper.isOpen())) {
/* 381:425 */             helper.close();
/* 382:    */           }
/* 383:    */         }
/* 384:425 */       }, 10000L);
/* 385:    */     }
/* 386:    */   }
/* 387:    */   
/* 388:    */   protected void logMessageReceived(String messageId)
/* 389:    */   {
/* 390:433 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 391:    */     try
/* 392:    */     {
/* 393:435 */       Dao<AnalyticItem, Integer> analyticItemDao = helper.getAnalyticItemDao();
/* 394:    */       AnalyticItem analyticItem;
/* 395:438 */       (analyticItem = new AnalyticItem(this.applicationContext)).setEventDate(new Date());
/* 396:439 */       analyticItem.addAnalyticType(10);
/* 397:440 */       analyticItem.addObjectId(messageId);
/* 398:441 */       analyticItem.setReadyToSend(Boolean.TRUE);
/* 399:442 */       analyticItemDao.create(analyticItem);
/* 400:    */     }
/* 401:    */     catch (SQLException e)
/* 402:    */     {
/* 403:445 */       if (ETPush.getLogLevel() <= 6) {
/* 404:446 */         Log.e("etpushsdk@ETAnalytics", e.getMessage(), e);
/* 405:    */       }
/* 406:    */     }
/* 407:    */     finally
/* 408:    */     {
/* 409:451 */       (e = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 410:    */       {
/* 411:    */         public void run()
/* 412:    */         {
/* 413:454 */           if ((helper != null) && (helper.isOpen())) {
/* 414:455 */             helper.close();
/* 415:    */           }
/* 416:    */         }
/* 417:455 */       }, 10000L);
/* 418:    */     }
/* 419:    */   }
/* 420:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.ETAnalytics
 * JD-Core Version:    0.7.0.1
 */