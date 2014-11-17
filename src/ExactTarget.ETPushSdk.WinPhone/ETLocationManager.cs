/*    1:     */ package com.exacttarget.etpushsdk;
/*    2:     */ 
/*    3:     */ import android.app.AlarmManager;
/*    4:     */ import android.app.NotificationManager;
/*    5:     */ import android.app.PendingIntent;
/*    6:     */ import android.content.BroadcastReceiver;
/*    7:     */ import android.content.Context;
/*    8:     */ import android.content.Intent;
/*    9:     */ import android.content.IntentFilter;
/*   10:     */ import android.content.ServiceConnection;
/*   11:     */ import android.content.SharedPreferences;
/*   12:     */ import android.content.SharedPreferences.Editor;
/*   13:     */ import android.location.Location;
/*   14:     */ import android.location.LocationManager;
/*   15:     */ import android.os.Bundle;
/*   16:     */ import android.os.Handler;
/*   17:     */ import android.os.Looper;
/*   18:     */ import android.os.RemoteException;
/*   19:     */ import android.os.SystemClock;
/*   20:     */ import android.util.Log;
/*   21:     */ import com.exacttarget.etpushsdk.data.BeaconRequest;
/*   22:     */ import com.exacttarget.etpushsdk.data.ETSqliteOpenHelper;
/*   23:     */ import com.exacttarget.etpushsdk.data.GeofenceRequest;
/*   24:     */ import com.exacttarget.etpushsdk.data.LocationUpdate;
/*   25:     */ import com.exacttarget.etpushsdk.data.Message;
/*   26:     */ import com.exacttarget.etpushsdk.data.RegionMessage;
/*   27:     */ import com.exacttarget.etpushsdk.event.BackgroundEvent;
/*   28:     */ import com.exacttarget.etpushsdk.event.BackgroundEventListener;
/*   29:     */ import com.exacttarget.etpushsdk.event.BeaconRegionEnterEvent;
/*   30:     */ import com.exacttarget.etpushsdk.event.BeaconRegionExitEvent;
/*   31:     */ import com.exacttarget.etpushsdk.event.BeaconRegionRangeEvent;
/*   32:     */ import com.exacttarget.etpushsdk.event.BeaconResponseEvent;
/*   33:     */ import com.exacttarget.etpushsdk.event.GeofenceResponseEvent;
/*   34:     */ import com.exacttarget.etpushsdk.event.GeofenceResponseEventListener;
/*   35:     */ import com.exacttarget.etpushsdk.event.LastKnownLocationEvent;
/*   36:     */ import com.exacttarget.etpushsdk.event.LastKnownLocationEventListener;
/*   37:     */ import com.exacttarget.etpushsdk.event.LocationUpdateEvent;
/*   38:     */ import com.exacttarget.etpushsdk.event.LocationUpdateEventListener;
/*   39:     */ import com.exacttarget.etpushsdk.event.PowerStatusChangedEvent;
/*   40:     */ import com.exacttarget.etpushsdk.location.ETLastLocationFinder;
/*   41:     */ import com.exacttarget.etpushsdk.location.ETLocationUpdateRequester;
/*   42:     */ import com.exacttarget.etpushsdk.location.ILastLocationFinder;
/*   43:     */ import com.exacttarget.etpushsdk.location.LocationUpdateRequester;
/*   44:     */ import com.exacttarget.etpushsdk.location.receiver.PassiveLocationChangedReceiver;
/*   45:     */ import com.exacttarget.etpushsdk.util.ETGooglePlayServicesUtil;
/*   46:     */ import com.exacttarget.etpushsdk.util.EventBus;
/*   47:     */ import com.google.android.gms.common.ConnectionResult;
/*   48:     */ import com.google.android.gms.common.GooglePlayServicesClient.ConnectionCallbacks;
/*   49:     */ import com.google.android.gms.common.GooglePlayServicesClient.OnConnectionFailedListener;
/*   50:     */ import com.google.android.gms.location.Geofence;
/*   51:     */ import com.google.android.gms.location.LocationClient;
/*   52:     */ import com.google.android.gms.location.LocationClient.OnAddGeofencesResultListener;
/*   53:     */ import com.google.android.gms.location.LocationClient.OnRemoveGeofencesResultListener;
/*   54:     */ import com.j256.ormlite.dao.Dao;
/*   55:     */ import com.j256.ormlite.stmt.QueryBuilder;
/*   56:     */ import com.j256.ormlite.stmt.UpdateBuilder;
/*   57:     */ import com.j256.ormlite.stmt.Where;
/*   58:     */ import com.radiusnetworks.ibeacon.BleNotAvailableException;
/*   59:     */ import com.radiusnetworks.ibeacon.IBeacon;
/*   60:     */ import com.radiusnetworks.ibeacon.IBeaconConsumer;
/*   61:     */ import com.radiusnetworks.ibeacon.IBeaconManager;
/*   62:     */ import com.radiusnetworks.ibeacon.MonitorNotifier;
/*   63:     */ import com.radiusnetworks.ibeacon.RangeNotifier;
/*   64:     */ import java.sql.SQLException;
/*   65:     */ import java.util.ArrayList;
/*   66:     */ import java.util.Collection;
/*   67:     */ import java.util.Date;
/*   68:     */ import java.util.Iterator;
/*   69:     */ import java.util.List;
/*   70:     */ 
/*   71:     */ public class ETLocationManager
/*   72:     */   implements BackgroundEventListener, GeofenceResponseEventListener, LastKnownLocationEventListener, LocationUpdateEventListener
/*   73:     */ {
/*   74:     */   private static final String TAG = "etpushsdk@ETLocationManager";
/*   75:     */   private static final String GEO_ENABLED_KEY = "et_geo_enabled_key";
/*   76:     */   private static final String GEOFENCE_INVALIDATED_KEY = "et_geofence_invalidated_key";
/*   77:     */   private static final String PROXIMITY_ENABLED_KEY = "et_proximity_enabled_key";
/*   78:     */   private static final String PROXIMITY_INVALIDATED_KEY = "et_proximity_invalidated_key";
/*   79:     */   private static ETLocationManager locationManager;
/*   80:     */   private Context applicationContext;
/*   81:     */   private SharedPreferences prefs;
/*   82:     */   private AlarmManager alarmManager;
/*   83:     */   private PendingIntent locationTimeoutPendingIntent;
/*   84:     */   private PendingIntent locationWakeupPendingIntent;
/*   85:     */   protected Intent awokenByIntent;
/*   86:     */   protected ILastLocationFinder lastLocationFinder;
/*   87:     */   private LocationClient locationClient;
/*   88:     */   private RegionMonitor regionMonitor;
/*   89:     */   private IBeaconManager iBeaconManager;
/*   90:     */   private IBeaconMonitor iBeaconMonitor;
/*   91: 113 */   private boolean isWatchingBluetoothChange = false;
/*   92: 114 */   private BroadcastReceiver bluetoothChangeReceiver = new BroadcastReceiver()
/*   93:     */   {
/*   94:     */     public void onReceive(Context context, Intent intent)
/*   95:     */     {
/*   96: 118 */       if ("android.bluetooth.adapter.action.STATE_CHANGED".equals(intent.getAction())) {
/*   97: 119 */         if (intent.getIntExtra("android.bluetooth.adapter.extra.STATE", -1) == 10)
/*   98:     */         {
/*   99: 120 */           if (ETPush.getLogLevel() <= 3) {
/*  100: 121 */             Log.d("etpushsdk@ETLocationManager", "Bluetooth OFF");
/*  101:     */           }
/*  102: 124 */           if (ETLocationManager.this.isWatchingProximity())
/*  103:     */           {
/*  104: 125 */             ETLocationManager.this.stopWatchingProximity();
/*  105: 126 */             ETLocationManager.this.setProximityEnabled(true);
/*  106:     */           }
/*  107:     */         }
/*  108: 129 */         else if (intent.getIntExtra("android.bluetooth.adapter.extra.STATE", -1) == 12)
/*  109:     */         {
/*  110: 130 */           if (ETPush.getLogLevel() <= 3) {
/*  111: 131 */             Log.d("etpushsdk@ETLocationManager", "Bluetooth ON");
/*  112:     */           }
/*  113: 133 */           if (ETLocationManager.this.isWatchingProximity())
/*  114:     */           {
/*  115: 134 */             if (!ETLocationManager.this.iBeaconManager.isBound(ETLocationManager.this.iBeaconMonitor))
/*  116:     */             {
/*  117: 135 */               ETLocationManager.this.setProximityInvalidated(true);
/*  118: 136 */               ETLocationManager.this.iBeaconManager.bind(ETLocationManager.this.iBeaconMonitor);return;
/*  119:     */             }
/*  120: 140 */             ETLocationManager.this.startWatchingProximity();
/*  121:     */           }
/*  122:     */         }
/*  123:     */       }
/*  124:     */     }
/*  125:     */   };
/*  126:     */   
/*  127:     */   private ETLocationManager(Context applicationContext)
/*  128:     */   {
/*  129: 149 */     Looper looper = Looper.getMainLooper();
/*  130: 150 */     (
/*  131: 151 */       looper = new Handler(looper)).post(new Runnable()
/*  132:     */       {
/*  133:     */         public void run()
/*  134:     */         {
/*  135:     */           try
/*  136:     */           {
/*  137: 154 */             Class.forName("android.os.AsyncTask"); return;
/*  138:     */           }
/*  139:     */           catch (ClassNotFoundException e)
/*  140:     */           {
/*  141: 157 */             if (ETPush.getLogLevel() <= 6) {
/*  142: 158 */               Log.e("etpushsdk@ETLocationManager", e.getMessage(), e);
/*  143:     */             }
/*  144:     */           }
/*  145:     */         }
/*  146: 166 */       });
/*  147: 167 */     this.applicationContext = applicationContext;
/*  148:     */     
/*  149: 169 */     EventBus.getDefault().register(this);
/*  150:     */     
/*  151: 171 */     this.lastLocationFinder = new ETLastLocationFinder(applicationContext);
/*  152:     */     
/*  153: 173 */     this.prefs = applicationContext.getSharedPreferences("ETPush", 0);
/*  154:     */     
/*  155: 175 */     this.alarmManager = ((AlarmManager)applicationContext.getSystemService("alarm"));
/*  156:     */     
/*  157: 177 */     this.regionMonitor = new RegionMonitor(null);
/*  158:     */     
/*  159: 179 */     this.iBeaconManager = IBeaconManager.getInstanceForApplication(applicationContext);
/*  160: 180 */     this.iBeaconManager.setBackgroundScanPeriod(5000L);
/*  161: 181 */     this.iBeaconManager.setBackgroundBetweenScanPeriod(10000L);
/*  162: 182 */     this.iBeaconMonitor = new IBeaconMonitor(null);
/*  163:     */     
/*  164: 184 */     setGeofenceInvalidated(true);
/*  165: 185 */     setProximityInvalidated(true);
/*  166:     */   }
/*  167:     */   
/*  168:     */   public static ETLocationManager locationManager()
/*  169:     */     throws ETException
/*  170:     */   {
/*  171: 194 */     if (Config.isLocationManagerActive())
/*  172:     */     {
/*  173: 195 */       if (locationManager == null) {
/*  174: 196 */         throw new ETException("You forgot to call readyAimFire first.");
/*  175:     */       }
/*  176: 199 */       return locationManager;
/*  177:     */     }
/*  178: 202 */     throw new ETException("ETLocationManager disabled. Ensure you called readyAimFire and enabled it first.");
/*  179:     */   }
/*  180:     */   
/*  181:     */   protected static void resetLocationManager(Context applicationContext)
/*  182:     */   {
/*  183: 212 */     locationManager = null;
/*  184:     */   }
/*  185:     */   
/*  186:     */   protected static void readyAimFire(Context applicationContext)
/*  187:     */     throws ETException
/*  188:     */   {
/*  189: 221 */     if (locationManager == null)
/*  190:     */     {
/*  191: 222 */       if (ETPush.getLogLevel() <= 3) {
/*  192: 223 */         Log.d("etpushsdk@ETLocationManager", "readyAimFire()");
/*  193:     */       }
/*  194: 225 */       locationManager = new ETLocationManager(applicationContext);
/*  195:     */       SharedPreferences localSharedPreferences;
/*  196: 230 */       (localSharedPreferences = applicationContext.getSharedPreferences("ETPush", 0)).edit().putBoolean("et_key_run_once", true).apply();
/*  197:     */       
/*  198:     */ 
/*  199: 233 */       LocationManager locationManager = (LocationManager)applicationContext.getSystemService("location");
/*  200: 234 */       Object locationUpdateRequester = new ETLocationUpdateRequester(locationManager);
/*  201: 235 */       Intent passiveIntent = new Intent(applicationContext, PassiveLocationChangedReceiver.class);
/*  202: 236 */       PendingIntent locationListenerPassivePendingIntent = PendingIntent.getActivity(applicationContext, 0, passiveIntent, 134217728);
/*  203: 237 */       ((LocationUpdateRequester)locationUpdateRequester).requestPassiveLocationUpdates(300000L, 0L, locationListenerPassivePendingIntent);
/*  204: 239 */       if ((!Config.isAnalyticsActive()) && (locationManager.isWatchingLocation()))
/*  205:     */       {
/*  206: 241 */         locationManager.setGeofenceInvalidated(true);
/*  207: 242 */         locationManager.startWatchingLocation();
/*  208:     */       }
/*  209: 245 */       if ((!Config.isAnalyticsActive()) && (locationManager.isWatchingProximity()))
/*  210:     */       {
/*  211: 247 */         locationManager.setProximityInvalidated(true);
/*  212: 248 */         locationManager.startWatchingProximity();
/*  213:     */       }
/*  214: 250 */       return;
/*  215:     */     }
/*  216: 252 */     throw new ETException("You must have called readyAimFire more than once.");
/*  217:     */   }
/*  218:     */   
/*  219:     */   public void startWatchingLocation()
/*  220:     */   {
/*  221: 260 */     if (!Config.isLocationManagerActive()) {
/*  222: 261 */       return;
/*  223:     */     }
/*  224: 264 */     if (ETPush.getLogLevel() <= 3) {
/*  225: 265 */       Log.d("etpushsdk@ETLocationManager", "startWatchingLocation()");
/*  226:     */     }
/*  227: 268 */     setGeoEnabled(true);
/*  228: 269 */     setGeofenceInvalidated(true);
/*  229:     */     
/*  230: 271 */     startLocationBackgroundWatcher();
/*  231:     */   }
/*  232:     */   
/*  233:     */   private void startLocationBackgroundWatcher()
/*  234:     */   {
/*  235: 275 */     if (!areLocationProvidersAvailable())
/*  236:     */     {
/*  237: 287 */       if (ETPush.getLogLevel() <= 5) {
/*  238: 288 */         Log.w("etpushsdk@ETLocationManager", "No Location Providers available.  ET will wait patiently until they are turned on to start watching location.");
/*  239:     */       }
/*  240:     */       return;
/*  241:     */     }
/*  242:     */     PowerStatusChangedEvent powerStatus;
/*  243: 295 */     if (((powerStatus = (PowerStatusChangedEvent)EventBus.getDefault().getStickyEvent(PowerStatusChangedEvent.class)) == null) || (powerStatus.getStatus() == 1))
/*  244:     */     {
/*  245: 296 */       this.lastLocationFinder.getLastBestLocation(100, System.currentTimeMillis() - 900000L);
/*  246:     */       
/*  247:     */ 
/*  248: 299 */       Intent locationTimeoutIntent = new Intent(this.applicationContext, ETLocationTimeoutReceiver.class);
/*  249: 300 */       this.locationTimeoutPendingIntent = PendingIntent.getBroadcast(this.applicationContext, 0, locationTimeoutIntent, 134217728);
/*  250: 301 */       this.alarmManager.set(2, SystemClock.elapsedRealtime() + 60000L, this.locationTimeoutPendingIntent);
/*  251: 303 */       if (this.locationWakeupPendingIntent == null)
/*  252:     */       {
/*  253: 305 */         if (ETPush.getLogLevel() <= 3) {
/*  254: 306 */           Log.d("etpushsdk@ETLocationManager", "Set recurring 15-minute locationWakeup Alarm.");
/*  255:     */         }
/*  256: 308 */         Intent locationWakeupIntent = new Intent(this.applicationContext, ETLocationWakeupReceiver.class);
/*  257: 309 */         this.locationWakeupPendingIntent = PendingIntent.getBroadcast(this.applicationContext, 0, locationWakeupIntent, 134217728);
/*  258: 310 */         this.alarmManager.setInexactRepeating(2, SystemClock.elapsedRealtime() + 900000L, 900000L, this.locationWakeupPendingIntent);
/*  259:     */       }
/*  260:     */     }
/*  261:     */   }
/*  262:     */   
/*  263:     */   private void stopLocationBackgroundWatcher()
/*  264:     */   {
/*  265: 317 */     if ((!isWatchingProximity()) && (!isWatchingLocation()))
/*  266:     */     {
/*  267: 318 */       if (this.lastLocationFinder != null) {
/*  268: 319 */         this.lastLocationFinder.cancel();
/*  269:     */       }
/*  270: 321 */       if (this.locationTimeoutPendingIntent != null)
/*  271:     */       {
/*  272: 322 */         this.alarmManager.cancel(this.locationTimeoutPendingIntent);
/*  273: 323 */         this.locationTimeoutPendingIntent = null;
/*  274:     */       }
/*  275: 325 */       if (this.locationWakeupPendingIntent != null)
/*  276:     */       {
/*  277: 326 */         this.alarmManager.cancel(this.locationWakeupPendingIntent);
/*  278: 327 */         this.locationWakeupPendingIntent = null;
/*  279:     */       }
/*  280:     */     }
/*  281:     */   }
/*  282:     */   
/*  283:     */   protected void completeWakefulIntent()
/*  284:     */   {
/*  285: 333 */     if (this.awokenByIntent != null)
/*  286:     */     {
/*  287: 334 */       ETLocationWakeupReceiver.completeWakefulIntent(this.awokenByIntent);
/*  288: 335 */       this.awokenByIntent = null;
/*  289:     */     }
/*  290:     */   }
/*  291:     */   
/*  292:     */   public boolean startWatchingProximity()
/*  293:     */     throws BleNotAvailableException
/*  294:     */   {
/*  295: 345 */     if (!Config.isLocationManagerActive())
/*  296:     */     {
/*  297: 346 */       Log.w("etpushsdk@ETLocationManager", "LocationManager must be set active in readyAimFire to use proximity.");
/*  298: 347 */       return false;
/*  299:     */     }
/*  300: 350 */     if (ETPush.getLogLevel() <= 3) {
/*  301: 351 */       Log.d("etpushsdk@ETLocationManager", "startWatchingProximity()");
/*  302:     */     }
/*  303: 353 */     setProximityEnabled(true);
/*  304: 354 */     setProximityInvalidated(true);
/*  305: 356 */     if (!this.isWatchingBluetoothChange)
/*  306:     */     {
/*  307: 357 */       IntentFilter filter = new IntentFilter("android.bluetooth.adapter.action.STATE_CHANGED");
/*  308: 358 */       this.applicationContext.registerReceiver(this.bluetoothChangeReceiver, filter);
/*  309: 359 */       this.isWatchingBluetoothChange = true;
/*  310:     */     }
/*  311: 362 */     if (!this.iBeaconManager.checkAvailability())
/*  312:     */     {
/*  313: 363 */       Log.w("etpushsdk@ETLocationManager", "Bluetooth LE available, but not currently turned on in settings.");
/*  314: 364 */       return false;
/*  315:     */     }
/*  316: 367 */     if (!this.iBeaconManager.isBound(this.iBeaconMonitor)) {
/*  317: 368 */       this.iBeaconManager.bind(this.iBeaconMonitor);
/*  318:     */     } else {
/*  319: 371 */       startLocationBackgroundWatcher();
/*  320:     */     }
/*  321: 374 */     return true;
/*  322:     */   }
/*  323:     */   
/*  324:     */   public void stopWatchingLocation()
/*  325:     */   {
/*  326: 381 */     if (ETPush.getLogLevel() <= 3) {
/*  327: 382 */       Log.d("etpushsdk@ETLocationManager", "stopWatchingLocation()");
/*  328:     */     }
/*  329: 385 */     setGeoEnabled(false);
/*  330:     */     
/*  331: 387 */     unmonitorAllGeofences();
/*  332:     */     
/*  333: 389 */     completeWakefulIntent();
/*  334:     */     
/*  335: 391 */     stopLocationBackgroundWatcher();
/*  336:     */   }
/*  337:     */   
/*  338:     */   public void stopWatchingProximity()
/*  339:     */   {
/*  340: 398 */     if (ETPush.getLogLevel() <= 3) {
/*  341: 399 */       Log.d("etpushsdk@ETLocationManager", "stopWatchingProximity()");
/*  342:     */     }
/*  343: 401 */     setProximityEnabled(false);
/*  344:     */     
/*  345:     */ 
/*  346: 404 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/*  347:     */     try
/*  348:     */     {
/*  349: 406 */       Dao<com.exacttarget.etpushsdk.data.Region, String> regionDao = helper.getRegionDao();
/*  350: 407 */       Dao<Message, String> messageDao = helper.getMessageDao();
/*  351:     */       UpdateBuilder<com.exacttarget.etpushsdk.data.Region, String> updateBuilder;
/*  352: 411 */       (updateBuilder = regionDao.updateBuilder()).where().eq("location_type", Integer.valueOf(3));
/*  353: 412 */       updateBuilder.updateColumnValue("active", Boolean.FALSE);
/*  354: 413 */       updateBuilder.updateColumnValue("has_entered", Boolean.FALSE);
/*  355: 414 */       updateBuilder.update();
/*  356:     */       UpdateBuilder<Message, String> messageUpdater;
/*  357: 417 */       (messageUpdater = messageDao.updateBuilder()).updateColumnValue("has_entered", Boolean.FALSE);
/*  358: 418 */       messageUpdater.update();
/*  359: 420 */       for (com.radiusnetworks.ibeacon.Region region : this.iBeaconManager.getMonitoredRegions()) {
/*  360:     */         try
/*  361:     */         {
/*  362: 422 */           if (ETPush.getLogLevel() <= 3) {
/*  363: 423 */             Log.d("etpushsdk@ETLocationManager", "stopMonitoringBeaconRegion: " + region.getUniqueId());
/*  364:     */           }
/*  365: 425 */           this.iBeaconManager.stopMonitoringBeaconsInRegion(region);
/*  366:     */         }
/*  367:     */         catch (RemoteException e)
/*  368:     */         {
/*  369: 428 */           if (ETPush.getLogLevel() > 6) {}
/*  370:     */         }
/*  371:     */       }
/*  372: 433 */       for (com.radiusnetworks.ibeacon.Region region : this.iBeaconManager.getRangedRegions()) {
/*  373:     */         try
/*  374:     */         {
/*  375: 435 */           if (ETPush.getLogLevel() <= 3) {
/*  376: 436 */             Log.d("etpushsdk@ETLocationManager", "stopRangingBeaconRegion: " + region.getUniqueId());
/*  377:     */           }
/*  378: 438 */           this.iBeaconManager.stopRangingBeaconsInRegion(region);
/*  379:     */         }
/*  380:     */         catch (RemoteException e)
/*  381:     */         {
/*  382: 441 */           if (ETPush.getLogLevel() > 6) {}
/*  383:     */         }
/*  384:     */       }
/*  385: 446 */       if (this.iBeaconManager.getMonitoredRegions().size() > 0) {
/*  386: 447 */         Log.e("etpushsdk@ETLocationManager", "monitoredRegions SHOULD BE ZERO!!!");
/*  387:     */       }
/*  388: 449 */       if (this.iBeaconManager.getRangedRegions().size() > 0) {
/*  389: 450 */         Log.e("etpushsdk@ETLocationManager", "rangedRegions SHOULD BE ZERO!!!");
/*  390:     */       }
/*  391: 453 */       if (this.iBeaconManager.isBound(this.iBeaconMonitor)) {
/*  392: 454 */         this.iBeaconManager.unBind(this.iBeaconMonitor);
/*  393:     */       }
/*  394:     */     }
/*  395:     */     catch (SQLException e)
/*  396:     */     {
/*  397: 458 */       if (ETPush.getLogLevel() <= 6) {
/*  398: 459 */         Log.e("etpushsdk@ETLocationManager", e.getMessage(), e);
/*  399:     */       }
/*  400:     */     }
/*  401:     */     finally
/*  402:     */     {
/*  403: 464 */       (e = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/*  404:     */       {
/*  405:     */         public void run()
/*  406:     */         {
/*  407: 467 */           if ((helper != null) && (helper.isOpen())) {
/*  408: 468 */             helper.close();
/*  409:     */           }
/*  410:     */         }
/*  411: 468 */       }, 10000L);
/*  412:     */     }
/*  413: 474 */     stopLocationBackgroundWatcher();
/*  414:     */   }
/*  415:     */   
/*  416:     */   private void unmonitorAllGeofences()
/*  417:     */   {
/*  418: 478 */     this.regionMonitor.setRemoveAllGeofences(Boolean.TRUE);
/*  419: 479 */     updateRegionMonitoring();
/*  420:     */   }
/*  421:     */   
/*  422:     */   public void enterLowPowerMode()
/*  423:     */   {
/*  424: 486 */     if (ETPush.getLogLevel() <= 3) {
/*  425: 487 */       Log.d("etpushsdk@ETLocationManager", "enterLowPowerMode()");
/*  426:     */     }
/*  427: 489 */     if (this.lastLocationFinder != null) {
/*  428: 490 */       this.lastLocationFinder.cancel();
/*  429:     */     }
/*  430: 492 */     if (this.locationTimeoutPendingIntent != null)
/*  431:     */     {
/*  432: 493 */       this.alarmManager.cancel(this.locationTimeoutPendingIntent);
/*  433: 494 */       this.locationTimeoutPendingIntent = null;
/*  434:     */     }
/*  435: 496 */     if (this.locationWakeupPendingIntent != null)
/*  436:     */     {
/*  437: 497 */       this.alarmManager.cancel(this.locationWakeupPendingIntent);
/*  438: 498 */       this.locationWakeupPendingIntent = null;
/*  439:     */     }
/*  440: 500 */     this.awokenByIntent = null;
/*  441:     */   }
/*  442:     */   
/*  443:     */   public void exitLowPowerMode()
/*  444:     */   {
/*  445: 507 */     startWatchingLocation();
/*  446:     */   }
/*  447:     */   
/*  448:     */   public boolean isWatchingLocation()
/*  449:     */   {
/*  450: 514 */     return ((Boolean)Config.getETSharedPref(this.applicationContext, this.applicationContext.getSharedPreferences("etpushsdk@ETLocationManager", 0), "et_geo_enabled_key", Boolean.valueOf(false))).booleanValue();
/*  451:     */   }
/*  452:     */   
/*  453:     */   public void setGeoEnabled(boolean value)
/*  454:     */   {
/*  455: 518 */     this.prefs.edit().putBoolean("et_geo_enabled_key", value).apply();
/*  456:     */   }
/*  457:     */   
/*  458:     */   protected boolean getGeofenceInvalidated()
/*  459:     */   {
/*  460: 522 */     return ((Boolean)Config.getETSharedPref(this.applicationContext, this.applicationContext.getSharedPreferences("etpushsdk@ETLocationManager", 0), "et_geofence_invalidated_key", Boolean.valueOf(true))).booleanValue();
/*  461:     */   }
/*  462:     */   
/*  463:     */   public void setGeofenceInvalidated(boolean value)
/*  464:     */   {
/*  465: 526 */     this.prefs.edit().putBoolean("et_geofence_invalidated_key", value).apply();
/*  466:     */   }
/*  467:     */   
/*  468:     */   public boolean isWatchingProximity()
/*  469:     */   {
/*  470: 533 */     return ((Boolean)Config.getETSharedPref(this.applicationContext, this.applicationContext.getSharedPreferences("etpushsdk@ETLocationManager", 0), "et_proximity_enabled_key", Boolean.valueOf(false))).booleanValue();
/*  471:     */   }
/*  472:     */   
/*  473:     */   public void setProximityEnabled(boolean value)
/*  474:     */   {
/*  475: 537 */     this.prefs.edit().putBoolean("et_proximity_enabled_key", value).apply();
/*  476:     */   }
/*  477:     */   
/*  478:     */   protected boolean getProximityInvalidated()
/*  479:     */   {
/*  480: 541 */     return ((Boolean)Config.getETSharedPref(this.applicationContext, this.applicationContext.getSharedPreferences("etpushsdk@ETLocationManager", 0), "et_proximity_invalidated_key", Boolean.valueOf(true))).booleanValue();
/*  481:     */   }
/*  482:     */   
/*  483:     */   public void setProximityInvalidated(boolean value)
/*  484:     */   {
/*  485: 545 */     this.prefs.edit().putBoolean("et_proximity_invalidated_key", value).apply();
/*  486:     */   }
/*  487:     */   
/*  488:     */   public boolean areLocationProvidersAvailable()
/*  489:     */   {
/*  490:     */     LocationManager locationManager;
/*  491: 550 */     return ((locationManager = (LocationManager)this.applicationContext.getSystemService("location")).isProviderEnabled("gps")) || (locationManager.isProviderEnabled("network"));
/*  492:     */   }
/*  493:     */   
/*  494:     */   public void onEvent(BackgroundEvent event)
/*  495:     */   {
/*  496: 561 */     if (Config.isLocationManagerActive()) {
/*  497: 562 */       if (!event.isInBackground())
/*  498:     */       {
/*  499: 564 */         if (ETPush.getLogLevel() <= 3) {
/*  500: 565 */           Log.d("etpushsdk@ETLocationManager", "In FOREGROUND");
/*  501:     */         }
/*  502: 567 */         if (isWatchingLocation())
/*  503:     */         {
/*  504: 569 */           setGeofenceInvalidated(true);
/*  505: 570 */           startWatchingLocation();
/*  506:     */         }
/*  507: 573 */         if (isWatchingProximity())
/*  508:     */         {
/*  509: 574 */           if (this.iBeaconManager.isBound(this.iBeaconMonitor))
/*  510:     */           {
/*  511: 575 */             if (ETPush.getLogLevel() <= 3) {
/*  512: 576 */               Log.d("etpushsdk@ETLocationManager", "BeaconManager: In FOREGROUND");
/*  513:     */             }
/*  514: 578 */             this.iBeaconManager.setBackgroundMode(this.iBeaconMonitor, false);
/*  515:     */           }
/*  516: 581 */           setProximityInvalidated(true);
/*  517: 582 */           startWatchingProximity();
/*  518:     */         }
/*  519:     */       }
/*  520: 586 */       else if ((isWatchingProximity()) && 
/*  521: 587 */         (this.iBeaconManager.isBound(this.iBeaconMonitor)))
/*  522:     */       {
/*  523: 588 */         if (ETPush.getLogLevel() <= 3) {
/*  524: 589 */           Log.d("etpushsdk@ETLocationManager", "BeaconManager: In BACKGROUND");
/*  525:     */         }
/*  526: 591 */         this.iBeaconManager.setBackgroundMode(this.iBeaconMonitor, true);
/*  527:     */       }
/*  528:     */     }
/*  529:     */   }
/*  530:     */   
/*  531:     */   public void onEvent(LocationUpdateEvent event)
/*  532:     */   {
/*  533: 606 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/*  534:     */     try
/*  535:     */     {
/*  536:     */       int rowsUpdated;
/*  537: 610 */       if ((rowsUpdated = helper.getLocationUpdateDao().deleteById(event.getId())) == 1)
/*  538:     */       {
/*  539: 611 */         if (ETPush.getLogLevel() <= 3) {
/*  540: 612 */           Log.d("etpushsdk@ETLocationManager", "removed locationupdate id: " + event.getId());
/*  541:     */         }
/*  542:     */       }
/*  543: 616 */       else if (ETPush.getLogLevel() <= 6) {
/*  544: 617 */         Log.e("etpushsdk@ETLocationManager", "Error: rowsUpdated = " + rowsUpdated);
/*  545:     */       }
/*  546:     */     }
/*  547:     */     catch (SQLException e)
/*  548:     */     {
/*  549: 622 */       if (ETPush.getLogLevel() <= 6) {
/*  550: 623 */         Log.e("etpushsdk@ETLocationManager", e.getMessage(), e);
/*  551:     */       }
/*  552:     */     }
/*  553:     */     finally
/*  554:     */     {
/*  555: 628 */       (e = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/*  556:     */       {
/*  557:     */         public void run()
/*  558:     */         {
/*  559: 631 */           if ((helper != null) && (helper.isOpen())) {
/*  560: 632 */             helper.close();
/*  561:     */           }
/*  562:     */         }
/*  563: 632 */       }, 10000L);
/*  564:     */     }
/*  565:     */   }
/*  566:     */   
/*  567:     */   public void onEvent(LastKnownLocationEvent event)
/*  568:     */   {
/*  569: 647 */     if (ETPush.getLogLevel() <= 3) {
/*  570: 648 */       Log.d("etpushsdk@ETLocationManager", "onEventLocationChanged()");
/*  571:     */     }
/*  572: 651 */     if (System.currentTimeMillis() - event.getLocation().getTime() < 900000L)
/*  573:     */     {
/*  574: 653 */       if (this.locationTimeoutPendingIntent != null) {
/*  575: 654 */         this.alarmManager.cancel(this.locationTimeoutPendingIntent);
/*  576:     */       }
/*  577: 657 */       if (ETPush.getLogLevel() <= 3) {
/*  578: 658 */         Log.d("etpushsdk@ETLocationManager", "Provider: " + event.getLocation().getProvider() + ", Lat: " + event.getLocation().getLatitude() + ", Lon: " + event.getLocation().getLongitude() + ", Accuracy: " + event.getLocation().getAccuracy() + ", Timestamp: " + new Date(event.getLocation().getTime()));
/*  579:     */       }
/*  580: 661 */       final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/*  581:     */       try
/*  582:     */       {
/*  583: 663 */         Dao<LocationUpdate, Integer> locationDao = helper.getLocationUpdateDao();
/*  584: 664 */         Date timestamp = new Date(event.getLocation().getTime());
/*  585: 665 */         if (locationDao.queryForEq("timestamp", timestamp).isEmpty())
/*  586:     */         {
/*  587:     */           LocationUpdate locationUpdate;
/*  588: 669 */           (locationUpdate = new LocationUpdate(this.applicationContext)).setLatitude(Double.valueOf(event.getLocation().getLatitude()));
/*  589: 670 */           locationUpdate.setLongitude(Double.valueOf(event.getLocation().getLongitude()));
/*  590: 671 */           locationUpdate.setAccuracy(Integer.valueOf(Math.round(event.getLocation().getAccuracy())));
/*  591: 672 */           locationUpdate.setTimestamp(timestamp);
/*  592: 673 */           locationDao.create(locationUpdate);
/*  593:     */         }
/*  594:     */         Intent sendDataIntent;
/*  595: 678 */         (sendDataIntent = new Intent(this.applicationContext, ETSendDataReceiver.class)).putExtra("et_send_type_extra", "et_send_type_location");
/*  596: 679 */         this.applicationContext.sendBroadcast(sendDataIntent);
/*  597: 681 */         if ((isWatchingLocation()) && (getGeofenceInvalidated()))
/*  598:     */         {
/*  599:     */           GeofenceRequest geofenceRequest;
/*  600: 685 */           (geofenceRequest = new GeofenceRequest(this.applicationContext)).setLatitude(Double.valueOf(event.getLocation().getLatitude()));
/*  601: 686 */           geofenceRequest.setLongitude(Double.valueOf(event.getLocation().getLongitude()));
/*  602:     */           
/*  603: 688 */           helper.getGeofenceRequestDao().create(geofenceRequest);
/*  604:     */           Intent sendDataIntent1;
/*  605: 692 */           (sendDataIntent1 = new Intent(this.applicationContext, ETSendDataReceiver.class)).putExtra("et_send_type_extra", "et_send_type_geofence");
/*  606: 693 */           this.applicationContext.sendBroadcast(sendDataIntent1);
/*  607:     */         }
/*  608: 696 */         if ((isWatchingProximity()) && (getProximityInvalidated()))
/*  609:     */         {
/*  610:     */           BeaconRequest beaconRequest;
/*  611: 698 */           (beaconRequest = new BeaconRequest(this.applicationContext)).setLatitude(Double.valueOf(event.getLocation().getLatitude()));
/*  612: 699 */           beaconRequest.setLongitude(Double.valueOf(event.getLocation().getLongitude()));
/*  613:     */           
/*  614: 701 */           helper.getBeaconRequestDao().create(beaconRequest);
/*  615:     */           
/*  616:     */ 
/*  617: 704 */           (
/*  618: 705 */             sendDataIntent2 = new Intent(this.applicationContext, ETSendDataReceiver.class)).putExtra("et_send_type_extra", "et_send_type_proximity");
/*  619: 706 */           this.applicationContext.sendBroadcast(sendDataIntent2);
/*  620:     */         }
/*  621:     */       }
/*  622:     */       catch (SQLException e)
/*  623:     */       {
/*  624:     */         Intent sendDataIntent2;
/*  625: 710 */         if (ETPush.getLogLevel() <= 6) {
/*  626: 711 */           Log.e("etpushsdk@ETLocationManager", e.getMessage(), e);
/*  627:     */         }
/*  628:     */       }
/*  629:     */       finally
/*  630:     */       {
/*  631: 716 */         (e = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/*  632:     */         {
/*  633:     */           public void run()
/*  634:     */           {
/*  635: 719 */             if ((helper != null) && (helper.isOpen())) {
/*  636: 720 */               helper.close();
/*  637:     */             }
/*  638:     */           }
/*  639: 720 */         }, 10000L);
/*  640:     */       }
/*  641: 727 */       completeWakefulIntent();
/*  642: 728 */       return;
/*  643:     */     }
/*  644: 731 */     if (ETPush.getLogLevel() <= 3) {
/*  645: 732 */       Log.d("etpushsdk@ETLocationManager", "stale location, older than 15 minutes.");
/*  646:     */     }
/*  647:     */   }
/*  648:     */   
/*  649:     */   public void onEvent(GeofenceResponseEvent event)
/*  650:     */   {
/*  651: 738 */     if (ETPush.getLogLevel() <= 3) {
/*  652: 739 */       Log.d("etpushsdk@ETLocationManager", "onEventGeofenceResponse()");
/*  653:     */     }
/*  654: 741 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/*  655:     */     try
/*  656:     */     {
/*  657:     */       Handler localHandler;
/*  658: 743 */       if (event.getRefreshCenter() == null)
/*  659:     */       {
/*  660: 744 */         if (ETPush.getLogLevel() <= 5) {
/*  661: 745 */           Log.w("etpushsdk@ETLocationManager", "Got a bad response from retrieving geofences. Try to get them the next time we get a location.");
/*  662:     */         }
/*  663: 747 */         setGeofenceInvalidated(true);
/*  664:     */       }
/*  665:     */       else
/*  666:     */       {
/*  667: 752 */         Object regionDao = helper.getRegionDao();
/*  668: 753 */         Dao<RegionMessage, Integer> regionMessageDao = helper.getRegionMessageDao();
/*  669: 754 */         Dao<Message, String> messageDao = helper.getMessageDao();
/*  670: 755 */         List<RegionMessage> regionMessages = regionMessageDao.queryForAll();
/*  671: 756 */         List<RegionMessage> regionMessagesToDelete = new ArrayList();
/*  672: 757 */         for (RegionMessage regionMessage : regionMessages)
/*  673:     */         {
/*  674:     */           com.exacttarget.etpushsdk.data.Region localRegion1;
/*  675: 759 */           if ((localRegion1 = (com.exacttarget.etpushsdk.data.Region)((Dao)regionDao).queryForId(regionMessage.getRegion().getId())).getLocationType().intValue() == 1) {
/*  676: 760 */             regionMessagesToDelete.add(regionMessage);
/*  677:     */           }
/*  678:     */         }
/*  679: 763 */         if (!regionMessagesToDelete.isEmpty()) {
/*  680: 764 */           regionMessageDao.delete(regionMessagesToDelete);
/*  681:     */         }
/*  682:     */         QueryBuilder<com.exacttarget.etpushsdk.data.Region, String> queryBuilder;
/*  683: 769 */         (queryBuilder = ((Dao)regionDao).queryBuilder()).where().eq("location_type", Integer.valueOf(1)).and().eq("active", Boolean.TRUE);
/*  684: 770 */         List<com.exacttarget.etpushsdk.data.Region> monitoredRegions = queryBuilder.query();
/*  685: 771 */         this.regionMonitor.setMonitoredRegions(monitoredRegions);
/*  686:     */         Object updateBuilder;
/*  687: 775 */         (updateBuilder = ((Dao)regionDao).updateBuilder()).where().eq("location_type", Integer.valueOf(1));
/*  688: 776 */         ((UpdateBuilder)updateBuilder).updateColumnValue("active", Boolean.FALSE);
/*  689: 777 */         ((UpdateBuilder)updateBuilder).update();
/*  690:     */         List<com.exacttarget.etpushsdk.data.Region> magicFences;
/*  691:     */         com.exacttarget.etpushsdk.data.Region magicFence;
/*  692: 781 */         if (((magicFences = ((Dao)regionDao).queryForEq("id", "~~m@g1c_f3nc3~~")) != null) && (magicFences.size() > 0)) {
/*  693: 783 */           magicFence = (com.exacttarget.etpushsdk.data.Region)magicFences.get(0);
/*  694:     */         } else {
/*  695: 787 */           (magicFence = new com.exacttarget.etpushsdk.data.Region()).setId("~~m@g1c_f3nc3~~");
/*  696:     */         }
/*  697: 790 */         magicFence.setActive(Boolean.TRUE);
/*  698: 791 */         magicFence.setCenter(event.getRefreshCenter());
/*  699: 792 */         magicFence.setRadius(event.getRefreshRadius());
/*  700: 793 */         ((Dao)regionDao).createOrUpdate(magicFence);
/*  701: 796 */         for (i$ = event.getFences().iterator(); i$.hasNext();)
/*  702:     */         {
/*  703: 797 */           (region = (com.exacttarget.etpushsdk.data.Region)i$.next()).setActive(Boolean.TRUE);
/*  704:     */           com.exacttarget.etpushsdk.data.Region dbRegion;
/*  705: 799 */           if ((dbRegion = (com.exacttarget.etpushsdk.data.Region)((Dao)regionDao).queryForId(region.getId())) != null)
/*  706:     */           {
/*  707: 800 */             region.setEntryCount(dbRegion.getEntryCount());
/*  708: 801 */             region.setExitCount(dbRegion.getExitCount());
/*  709:     */           }
/*  710: 803 */           ((Dao)regionDao).createOrUpdate(region);
/*  711: 805 */           for (Message message : region.getMessages())
/*  712:     */           {
/*  713:     */             Object dbMessages;
/*  714: 807 */             if (((dbMessages = messageDao.queryForEq("id", message.getId())) != null) && (((List)dbMessages).size() > 0))
/*  715:     */             {
/*  716: 808 */               Message dbMessage = (Message)((List)dbMessages).get(0);
/*  717: 809 */               message.setLastShownDate(dbMessage.getLastShownDate());
/*  718: 810 */               message.setNextAllowedShow(dbMessage.getNextAllowedShow());
/*  719: 811 */               message.setShowCount(dbMessage.getShowCount());
/*  720: 812 */               if (dbMessage.getPeriodType().equals(message.getPeriodType())) {
/*  721: 813 */                 message.setPeriodShowCount(dbMessage.getPeriodShowCount());
/*  722:     */               } else {
/*  723: 817 */                 message.setPeriodShowCount(Integer.valueOf(0));
/*  724:     */               }
/*  725:     */             }
/*  726: 821 */             if ((message.getMessagesPerPeriod().intValue() <= 0) && (message.getNumberOfPeriods().intValue() > 0) && (!message.getPeriodType().equals(Integer.valueOf(0)))) {
/*  727: 822 */               message.setMessagesPerPeriod(Integer.valueOf(1));
/*  728:     */             }
/*  729: 825 */             messageDao.createOrUpdate(message);
/*  730:     */             
/*  731:     */ 
/*  732: 828 */             regionMessageDao.create(new RegionMessage(region, message));
/*  733:     */           }
/*  734:     */         }
/*  735:     */         com.exacttarget.etpushsdk.data.Region region;
/*  736: 832 */         setGeofenceInvalidated(false);
/*  737:     */         
/*  738: 834 */         updateRegionMonitoring(); return;
/*  739:     */       }
/*  740:     */     }
/*  741:     */     catch (SQLException e)
/*  742:     */     {
/*  743: 837 */       if (ETPush.getLogLevel() <= 6) {
/*  744: 838 */         Log.e("etpushsdk@ETLocationManager", ((SQLException)e).getMessage(), (Throwable)e);
/*  745:     */       }
/*  746:     */     }
/*  747:     */     finally
/*  748:     */     {
/*  749: 843 */       (e = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/*  750:     */       {
/*  751:     */         public void run()
/*  752:     */         {
/*  753: 846 */           if ((helper != null) && (helper.isOpen())) {
/*  754: 847 */             helper.close();
/*  755:     */           }
/*  756:     */         }
/*  757: 847 */       }, 10000L);
/*  758:     */     }
/*  759:     */   }
/*  760:     */   
/*  761:     */   public void onEvent(BeaconResponseEvent event)
/*  762:     */   {
/*  763: 855 */     if (ETPush.getLogLevel() <= 3) {
/*  764: 856 */       Log.d("etpushsdk@ETLocationManager", "onEvent(BeaconResponseEvent)");
/*  765:     */     }
/*  766: 859 */     if (this.iBeaconManager.checkAvailability())
/*  767:     */     {
/*  768: 860 */       if (!this.iBeaconManager.isBound(this.iBeaconMonitor))
/*  769:     */       {
/*  770: 861 */         this.iBeaconManager.bind(this.iBeaconMonitor);
/*  771: 862 */         if (ETPush.getLogLevel() <= 5) {
/*  772: 863 */           Log.w("etpushsdk@ETLocationManager", "for some weird reason, iBeaconMonitor wasn't bound.");
/*  773:     */         }
/*  774: 865 */         return;
/*  775:     */       }
/*  776: 869 */       final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/*  777:     */       try
/*  778:     */       {
/*  779: 872 */         Dao<com.exacttarget.etpushsdk.data.Region, String> regionDao = helper.getRegionDao();
/*  780: 873 */         Dao<RegionMessage, Integer> regionMessageDao = helper.getRegionMessageDao();
/*  781: 874 */         Dao<Message, String> messageDao = helper.getMessageDao();
/*  782:     */         
/*  783: 876 */         List<RegionMessage> regionMessages = regionMessageDao.queryForAll();
/*  784: 877 */         List<RegionMessage> regionMessagesToDelete = new ArrayList();
/*  785: 878 */         for (RegionMessage regionMessage : regionMessages)
/*  786:     */         {
/*  787:     */           com.exacttarget.etpushsdk.data.Region localRegion1;
/*  788: 880 */           if ((localRegion1 = (com.exacttarget.etpushsdk.data.Region)regionDao.queryForId(regionMessage.getRegion().getId())).getLocationType().intValue() == 3) {
/*  789: 881 */             regionMessagesToDelete.add(regionMessage);
/*  790:     */           }
/*  791:     */         }
/*  792: 884 */         if (!regionMessagesToDelete.isEmpty()) {
/*  793: 885 */           regionMessageDao.delete(regionMessagesToDelete);
/*  794:     */         }
/*  795: 889 */         if ((event.getBeacons() != null) && (!event.getBeacons().isEmpty()))
/*  796:     */         {
/*  797: 890 */           for (Iterator i$ = event.getBeacons().iterator(); i$.hasNext();) {
/*  798: 891 */             for (Message message : (region = (com.exacttarget.etpushsdk.data.Region)i$.next()).getMessages())
/*  799:     */             {
/*  800:     */               List<Message> dbMessages;
/*  801: 893 */               if (((dbMessages = messageDao.queryForEq("id", message.getId())) != null) && (dbMessages.size() > 0))
/*  802:     */               {
/*  803: 894 */                 Message dbMessage = (Message)dbMessages.get(0);
/*  804: 895 */                 message.setLastShownDate(dbMessage.getLastShownDate());
/*  805: 896 */                 message.setNextAllowedShow(dbMessage.getNextAllowedShow());
/*  806: 897 */                 message.setShowCount(dbMessage.getShowCount());
/*  807: 898 */                 message.setHasEntered(dbMessage.getHasEntered());
/*  808: 899 */                 if (dbMessage.getPeriodType().equals(message.getPeriodType())) {
/*  809: 900 */                   message.setPeriodShowCount(dbMessage.getPeriodShowCount());
/*  810:     */                 } else {
/*  811: 904 */                   message.setPeriodShowCount(Integer.valueOf(0));
/*  812:     */                 }
/*  813:     */               }
/*  814: 908 */               if ((message.getMessagesPerPeriod().intValue() <= 0) && (message.getNumberOfPeriods().intValue() > 0) && (!message.getPeriodType().equals(Integer.valueOf(0)))) {
/*  815: 909 */                 message.setMessagesPerPeriod(Integer.valueOf(1));
/*  816:     */               }
/*  817: 912 */               messageDao.createOrUpdate(message);
/*  818:     */               
/*  819:     */ 
/*  820: 915 */               regionMessageDao.create(new RegionMessage(region, message));
/*  821:     */             }
/*  822:     */           }
/*  823:     */           com.exacttarget.etpushsdk.data.Region region;
/*  824: 921 */           for (com.exacttarget.etpushsdk.data.Region regionInDb : region = (i$ = regionDao.queryBuilder()).where().eq("location_type", Integer.valueOf(3)).and().eq("active", Boolean.TRUE).query()) {
/*  825: 922 */             if (!event.getBeacons().contains(regionInDb)) {
/*  826:     */               try
/*  827:     */               {
/*  828: 925 */                 regionInDb.setActive(Boolean.FALSE);
/*  829: 926 */                 regionInDb.setHasEntered(Boolean.FALSE);
/*  830: 927 */                 regionDao.update(regionInDb);
/*  831:     */                 
/*  832: 929 */                 region = new com.radiusnetworks.ibeacon.Region(regionInDb.getId(), regionInDb.getGuid(), regionInDb.getMajor(), regionInDb.getMinor());
/*  833: 930 */                 this.iBeaconManager.stopMonitoringBeaconsInRegion(region);
/*  834: 931 */                 this.iBeaconManager.stopRangingBeaconsInRegion(region);
/*  835: 932 */                 if (ETPush.getLogLevel() > 3) {}
/*  836:     */               }
/*  837:     */               catch (RemoteException e)
/*  838:     */               {
/*  839:     */                 com.radiusnetworks.ibeacon.Region region;
/*  840: 937 */                 if (ETPush.getLogLevel() > 6) {}
/*  841:     */               }
/*  842:     */             }
/*  843:     */           }
/*  844: 944 */           for (com.exacttarget.etpushsdk.data.Region regionToRange : event.getBeacons())
/*  845:     */           {
/*  846:     */             com.exacttarget.etpushsdk.data.Region regionInDb;
/*  847: 946 */             if ((regionInDb = (com.exacttarget.etpushsdk.data.Region)regionDao.queryForId(regionToRange.getId())) == null)
/*  848:     */             {
/*  849: 948 */               (regionInDb = regionToRange).setActive(Boolean.TRUE);
/*  850: 949 */               regionDao.create(regionInDb);
/*  851:     */             }
/*  852:     */             else
/*  853:     */             {
/*  854: 952 */               if (Boolean.TRUE.equals(regionInDb.getActive()))
/*  855:     */               {
/*  856: 953 */                 com.radiusnetworks.ibeacon.Region region = new com.radiusnetworks.ibeacon.Region(regionInDb.getId(), regionInDb.getGuid(), regionInDb.getMajor(), regionInDb.getMinor());
/*  857: 954 */                 if (this.iBeaconManager.getRangedRegions().contains(region))
/*  858:     */                 {
/*  859: 955 */                   if (ETPush.getLogLevel() > 3) {
/*  860:     */                     continue;
/*  861:     */                   }
/*  862: 956 */                   Log.d("etpushsdk@ETLocationManager", "alreadyRangingBeacon - {id: '" + region.getUniqueId() + "', uuid: '" + region.getProximityUuid() + "', major: " + region.getMajor() + ", minor: " + region.getMinor() + "}"); continue;
/*  863:     */                 }
/*  864:     */               }
/*  865: 962 */               regionInDb.setActive(Boolean.TRUE);
/*  866: 963 */               regionInDb.setGuid(regionToRange.getGuid());
/*  867: 964 */               regionInDb.setMajor(regionToRange.getMajor());
/*  868: 965 */               regionInDb.setMinor(regionToRange.getMinor());
/*  869: 966 */               regionDao.update(regionInDb);
/*  870:     */             }
/*  871: 969 */             com.radiusnetworks.ibeacon.Region region = new com.radiusnetworks.ibeacon.Region(regionInDb.getId(), regionInDb.getGuid(), regionInDb.getMajor(), regionInDb.getMinor());
/*  872:     */             try
/*  873:     */             {
/*  874: 972 */               if (!this.iBeaconManager.getRangedRegions().contains(region))
/*  875:     */               {
/*  876: 973 */                 if (ETPush.getLogLevel() <= 3) {
/*  877: 974 */                   Log.d("etpushsdk@ETLocationManager", "startRangingBeacon - {id: '" + region.getUniqueId() + "', uuid: '" + region.getProximityUuid() + "', major: " + region.getMajor() + ", minor: " + region.getMinor() + "}");
/*  878:     */                 }
/*  879: 976 */                 this.iBeaconManager.startMonitoringBeaconsInRegion(region);
/*  880: 977 */                 this.iBeaconManager.startRangingBeaconsInRegion(region);
/*  881:     */               }
/*  882: 980 */               else if (ETPush.getLogLevel() > 3) {}
/*  883:     */             }
/*  884:     */             catch (RemoteException e)
/*  885:     */             {
/*  886: 986 */               if (ETPush.getLogLevel() <= 6) {
/*  887: 987 */                 Log.e("etpushsdk@ETLocationManager", e.getMessage(), e);
/*  888:     */               }
/*  889: 990 */               regionInDb.setActive(Boolean.FALSE);
/*  890:     */             }
/*  891:     */           }
/*  892:     */         }
/*  893: 997 */         else if (ETPush.getLogLevel() <= 3)
/*  894:     */         {
/*  895: 998 */           Log.d("etpushsdk@ETLocationManager", "Empty beacon list from server.");
/*  896:     */         }
/*  897:1001 */         setProximityInvalidated(false);
/*  898:     */       }
/*  899:     */       catch (SQLException e)
/*  900:     */       {
/*  901:1005 */         if (ETPush.getLogLevel() <= 6) {
/*  902:1006 */           Log.e("etpushsdk@ETLocationManager", e.getMessage(), e);
/*  903:     */         }
/*  904:     */       }
/*  905:     */       finally
/*  906:     */       {
/*  907:1011 */         (e = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/*  908:     */         {
/*  909:     */           public void run()
/*  910:     */           {
/*  911:1014 */             if ((helper != null) && (helper.isOpen())) {
/*  912:1015 */               helper.close();
/*  913:     */             }
/*  914:     */           }
/*  915:1015 */         }, 10000L);
/*  916:     */       }
/*  917:     */     }
/*  918:     */   }
/*  919:     */   
/*  920:     */   private void updateRegionMonitoring()
/*  921:     */   {
/*  922:1025 */     if (ETPush.getLogLevel() <= 3) {
/*  923:1026 */       Log.d("etpushsdk@ETLocationManager", "updateRegionMonitoring()");
/*  924:     */     }
/*  925:1029 */     if (ETGooglePlayServicesUtil.isAvailable(this.applicationContext, false))
/*  926:     */     {
/*  927:1030 */       if (this.locationClient == null) {
/*  928:1031 */         this.locationClient = new LocationClient(this.applicationContext, this.regionMonitor, this.regionMonitor);
/*  929:     */       }
/*  930:1034 */       if ((!this.locationClient.isConnected()) && (!this.locationClient.isConnecting()))
/*  931:     */       {
/*  932:1035 */         if (ETPush.getLogLevel() <= 3) {
/*  933:1036 */           Log.d("etpushsdk@ETLocationManager", "locationClient.connect() ...");
/*  934:     */         }
/*  935:1038 */         this.locationClient.connect();return;
/*  936:     */       }
/*  937:1041 */       if (ETPush.getLogLevel() <= 5) {
/*  938:1042 */         Log.w("etpushsdk@ETLocationManager", "locationClientConnecting == true, trying reconnect");
/*  939:     */       }
/*  940:1044 */       this.locationClient.disconnect();
/*  941:1045 */       this.locationClient = null;
/*  942:1046 */       updateRegionMonitoring();return;
/*  943:     */     }
/*  944:1050 */     if (ETPush.getLogLevel() <= 6) {
/*  945:1051 */       Log.e("etpushsdk@ETLocationManager", "Play Services Not available");
/*  946:     */     }
/*  947:     */   }
/*  948:     */   
/*  949:     */   private class RegionMonitor
/*  950:     */     implements GooglePlayServicesClient.ConnectionCallbacks, GooglePlayServicesClient.OnConnectionFailedListener, LocationClient.OnAddGeofencesResultListener, LocationClient.OnRemoveGeofencesResultListener
/*  951:     */   {
/*  952:     */     private static final String TAG = "etpushsdk@RegionMonitor";
/*  953:1059 */     private Boolean removeAllGeofences = Boolean.FALSE;
/*  954:1060 */     private List<com.exacttarget.etpushsdk.data.Region> monitoredRegions = null;
/*  955:1061 */     private List<Geofence> geofencesToStartMonitoring = null;
/*  956:     */     private PendingIntent geofencePendingIntent;
/*  957:     */     
/*  958:     */     private RegionMonitor() {}
/*  959:     */     
/*  960:     */     public void setMonitoredRegions(List<com.exacttarget.etpushsdk.data.Region> monitoredRegions)
/*  961:     */     {
/*  962:1064 */       this.monitoredRegions = monitoredRegions;
/*  963:     */     }
/*  964:     */     
/*  965:     */     public void setRemoveAllGeofences(Boolean removeAllGeofences)
/*  966:     */     {
/*  967:1068 */       this.removeAllGeofences = removeAllGeofences;
/*  968:     */     }
/*  969:     */     
/*  970:     */     private PendingIntent getGeofencePendingIntent()
/*  971:     */     {
/*  972:1075 */       if (this.geofencePendingIntent == null)
/*  973:     */       {
/*  974:1076 */         if (ETPush.getLogLevel() <= 3) {
/*  975:1077 */           Log.d("etpushsdk@RegionMonitor", "create geofencePendingIntent");
/*  976:     */         }
/*  977:1079 */         Intent geofenceIntent = new Intent(ETLocationManager.this.applicationContext, ETGeofenceReceiver.class);
/*  978:1080 */         this.geofencePendingIntent = PendingIntent.getBroadcast(ETLocationManager.this.applicationContext, 0, geofenceIntent, 134217728);
/*  979:     */       }
/*  980:1083 */       return this.geofencePendingIntent;
/*  981:     */     }
/*  982:     */     
/*  983:     */     private void updateGeofencesFromDatabase()
/*  984:     */     {
/*  985:1087 */       if (ETPush.getLogLevel() <= 3) {
/*  986:1088 */         Log.d("etpushsdk@RegionMonitor", "updateGeofencesFromDatabase()");
/*  987:     */       }
/*  988:1091 */       final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(ETLocationManager.this.applicationContext);
/*  989:     */       try
/*  990:     */       {
/*  991:1093 */         Dao<com.exacttarget.etpushsdk.data.Region, String> regionDao = helper.getRegionDao();
/*  992:1094 */         regionMessageDao = helper.getRegionMessageDao();
/*  993:1095 */         Dao<Message, String> messageDao = helper.getMessageDao();
/*  994:     */         
/*  995:1097 */         this.geofencesToStartMonitoring = new ArrayList();
/*  996:1098 */         List<com.exacttarget.etpushsdk.data.Region> regions = regionDao.queryBuilder().where().eq("location_type", Integer.valueOf(1)).and().eq("active", Boolean.TRUE).query();
/*  997:     */         
/*  998:1100 */         List<String> regionsToStopMonitoring = new ArrayList();
/*  999:1101 */         List<com.exacttarget.etpushsdk.data.Region> regionsToStartMonitoring = new ArrayList();
/* 1000:1102 */         for (com.exacttarget.etpushsdk.data.Region monitoredRegion : this.monitoredRegions) {
/* 1001:1103 */           if ((!regions.contains(monitoredRegion)) || ("~~m@g1c_f3nc3~~".equals(monitoredRegion.getId()))) {
/* 1002:1104 */             regionsToStopMonitoring.add(monitoredRegion.getId());
/* 1003:     */           }
/* 1004:     */         }
/* 1005:1108 */         for (com.exacttarget.etpushsdk.data.Region unmonitoredRegion : regions) {
/* 1006:1109 */           if ((!this.monitoredRegions.contains(unmonitoredRegion)) || ("~~m@g1c_f3nc3~~".equals(unmonitoredRegion.getId()))) {
/* 1007:1110 */             regionsToStartMonitoring.add(unmonitoredRegion);
/* 1008:     */           }
/* 1009:     */         }
/* 1010:1114 */         for (com.exacttarget.etpushsdk.data.Region region : regionsToStartMonitoring)
/* 1011:     */         {
/* 1012:1116 */           messages = new ArrayList();
/* 1013:1118 */           for (RegionMessage regionMessage : regionsToStartMonitoring = regionMessageDao.queryForEq("region_id", region.getId()))
/* 1014:     */           {
/* 1015:     */             List<Message> dbMessages;
/* 1016:1120 */             if (((dbMessages = messageDao.queryForEq("id", regionMessage.getMessage().getId())) != null) && (dbMessages.size() > 0))
/* 1017:     */             {
/* 1018:1121 */               Message message = (Message)dbMessages.get(0);
/* 1019:1122 */               messages.add(message);
/* 1020:     */             }
/* 1021:     */           }
/* 1022:1125 */           region.setMessages(messages);
/* 1023:1127 */           if (ETPush.getLogLevel() <= 3) {
/* 1024:1128 */             Log.d("etpushsdk@RegionMonitor", "Will Monitor Region: " + region.getId() + ", " + region.getLatitude() + ", " + region.getLongitude() + ", " + region.getRadius());
/* 1025:     */           }
/* 1026:1130 */           this.geofencesToStartMonitoring.add(region.toGeofence());
/* 1027:     */         }
/* 1028:     */         List<Message> messages;
/* 1029:1133 */         if (regionsToStopMonitoring.size() > 0)
/* 1030:     */         {
/* 1031:1134 */           ETLocationManager.this.locationClient.removeGeofences(regionsToStopMonitoring, this);
/* 1032:     */         }
/* 1033:1136 */         else if (this.geofencesToStartMonitoring.size() > 0)
/* 1034:     */         {
/* 1035:1137 */           ETLocationManager.this.locationClient.addGeofences(this.geofencesToStartMonitoring, getGeofencePendingIntent(), this);
/* 1036:     */         }
/* 1037:     */         else
/* 1038:     */         {
/* 1039:1140 */           if (ETPush.getLogLevel() <= 3) {
/* 1040:1141 */             Log.d("etpushsdk@RegionMonitor", "geofence data hasn't changed, disconnecting.");
/* 1041:     */           }
/* 1042:1143 */           ETLocationManager.this.locationClient.disconnect();
/* 1043:     */         }
/* 1044:     */       }
/* 1045:     */       catch (SQLException e)
/* 1046:     */       {
/* 1047:1147 */         if (ETPush.getLogLevel() <= 6) {
/* 1048:1148 */           Log.e("etpushsdk@RegionMonitor", e.getMessage(), e);
/* 1049:     */         }
/* 1050:     */         try
/* 1051:     */         {
/* 1052:1151 */           ETLocationManager.this.locationClient.disconnect();
/* 1053:     */         }
/* 1054:     */         catch (Throwable localThrowable) {}
/* 1055:     */       }
/* 1056:     */       finally
/* 1057:     */       {
/* 1058:     */         Dao<RegionMessage, Integer> regionMessageDao;
/* 1059:1158 */         (regionMessageDao = new Handler(ETLocationManager.this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 1060:     */         {
/* 1061:     */           public void run()
/* 1062:     */           {
/* 1063:1161 */             if ((helper != null) && (helper.isOpen())) {
/* 1064:1162 */               helper.close();
/* 1065:     */             }
/* 1066:     */           }
/* 1067:1162 */         }, 10000L);
/* 1068:     */       }
/* 1069:     */     }
/* 1070:     */     
/* 1071:     */     public void onRemoveGeofencesByPendingIntentResult(int statusCode, PendingIntent pendingIntent)
/* 1072:     */     {
/* 1073:     */       ;
/* 1074:1170 */       if (ETPush.getLogLevel() <= 3) {
/* 1075:1171 */         Log.d("etpushsdk@RegionMonitor", "onRemoveGeofencesByPendingIntentResult() status: " + getStatusString(statusCode));
/* 1076:     */       }
/* 1077:1173 */       this.removeAllGeofences = Boolean.FALSE;
/* 1078:     */       
/* 1079:1175 */       helper = ETSqliteOpenHelper.getHelper(ETLocationManager.this.applicationContext);
/* 1080:     */       try
/* 1081:     */       {
/* 1082:1178 */         (updateBuilder = helper.getRegionDao().updateBuilder()).where().eq("location_type", Integer.valueOf(1));
/* 1083:1179 */         updateBuilder.updateColumnValue("active", Boolean.FALSE);
/* 1084:1180 */         updateBuilder.update();
/* 1085:     */       }
/* 1086:     */       catch (SQLException e)
/* 1087:     */       {
/* 1088:     */         UpdateBuilder<com.exacttarget.etpushsdk.data.Region, String> updateBuilder;
/* 1089:1183 */         if (ETPush.getLogLevel() <= 6) {
/* 1090:1184 */           Log.e("etpushsdk@RegionMonitor", e.getMessage(), e);
/* 1091:     */         }
/* 1092:     */       }
/* 1093:     */       finally
/* 1094:     */       {
/* 1095:     */         Handler localHandler;
/* 1096:1189 */         (localHandler = new Handler(ETLocationManager.this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 1097:     */         {
/* 1098:     */           public void run()
/* 1099:     */           {
/* 1100:1192 */             if ((helper != null) && (helper.isOpen())) {
/* 1101:1193 */               helper.close();
/* 1102:     */             }
/* 1103:     */           }
/* 1104:1193 */         }, 10000L);
/* 1105:     */       }
/* 1106:1199 */       ETLocationManager.this.locationClient.disconnect();
/* 1107:     */     }
/* 1108:     */     
/* 1109:     */     public void onRemoveGeofencesByRequestIdsResult(int statusCode, String[] geofenceRequestIds)
/* 1110:     */     {
/* 1111:     */       ;
/* 1112:1203 */       if (ETPush.getLogLevel() <= 3) {
/* 1113:1204 */         Log.d("etpushsdk@RegionMonitor", "onRemoveGeofencesByRequestIdsResult status: " + getStatusString(statusCode));
/* 1114:     */       }
/* 1115:1206 */       for (String geofenceRequestId : geofenceRequestIds) {
/* 1116:1207 */         if (ETPush.getLogLevel() <= 3) {
/* 1117:1208 */           Log.d("etpushsdk@RegionMonitor", "Unmonitor Region: " + geofenceRequestId);
/* 1118:     */         }
/* 1119:     */       }
/* 1120:1212 */       if (this.geofencesToStartMonitoring.size() > 0)
/* 1121:     */       {
/* 1122:1213 */         ETLocationManager.this.locationClient.addGeofences(this.geofencesToStartMonitoring, getGeofencePendingIntent(), this);return;
/* 1123:     */       }
/* 1124:1216 */       if (ETPush.getLogLevel() <= 3) {
/* 1125:1217 */         Log.d("etpushsdk@RegionMonitor", "no new geofences to monitor, disconnecting.");
/* 1126:     */       }
/* 1127:1219 */       ETLocationManager.this.locationClient.disconnect();
/* 1128:     */     }
/* 1129:     */     
/* 1130:     */     public void onAddGeofencesResult(int statusCode, String[] geofenceRequestIds)
/* 1131:     */     {
/* 1132:     */       ;
/* 1133:1224 */       if (ETPush.getLogLevel() <= 3) {
/* 1134:1225 */         Log.d("etpushsdk@RegionMonitor", "onAddGeofencesResult() status: " + getStatusString(statusCode));
/* 1135:     */       }
/* 1136:1228 */       if (statusCode != 0)
/* 1137:     */       {
/* 1138:1230 */         helper = ETSqliteOpenHelper.getHelper(ETLocationManager.this.applicationContext);
/* 1139:     */         try
/* 1140:     */         {
/* 1141:1232 */           if (ETPush.getLogLevel() <= 3) {
/* 1142:1233 */             Log.d("etpushsdk@RegionMonitor", "ERROR: Unable to monitor geofences, set them to inactive in db.");
/* 1143:     */           }
/* 1144:1236 */           (updateBuilder = helper.getRegionDao().updateBuilder()).where().eq("location_type", Integer.valueOf(1));
/* 1145:1237 */           updateBuilder.updateColumnValue("active", Boolean.FALSE);
/* 1146:1238 */           updateBuilder.update();
/* 1147:     */         }
/* 1148:     */         catch (SQLException e)
/* 1149:     */         {
/* 1150:     */           UpdateBuilder<com.exacttarget.etpushsdk.data.Region, String> updateBuilder;
/* 1151:1241 */           if (ETPush.getLogLevel() <= 6) {
/* 1152:1242 */             Log.e("etpushsdk@RegionMonitor", e.getMessage(), e);
/* 1153:     */           }
/* 1154:     */         }
/* 1155:     */         finally
/* 1156:     */         {
/* 1157:     */           Handler localHandler;
/* 1158:1247 */           (localHandler = new Handler(ETLocationManager.this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 1159:     */           {
/* 1160:     */             public void run()
/* 1161:     */             {
/* 1162:1250 */               if ((helper != null) && (helper.isOpen())) {
/* 1163:1251 */                 helper.close();
/* 1164:     */               }
/* 1165:     */             }
/* 1166:1251 */           }, 10000L);
/* 1167:     */         }
/* 1168:     */       }
/* 1169:1256 */       else if (ETPush.getLogLevel() <= 3)
/* 1170:     */       {
/* 1171:1259 */         for (String requestId : e) {
/* 1172:1260 */           Log.d("etpushsdk@RegionMonitor", "Monitor Region: " + requestId);
/* 1173:     */         }
/* 1174:     */       }
/* 1175:1265 */       ETLocationManager.this.locationClient.disconnect();
/* 1176:     */     }
/* 1177:     */     
/* 1178:     */     public void onConnectionFailed(ConnectionResult connectionResult)
/* 1179:     */     {
/* 1180:1269 */       if (ETPush.getLogLevel() <= 3) {
/* 1181:1270 */         Log.d("etpushsdk@RegionMonitor", "onConnectionFailed()");
/* 1182:     */       }
/* 1183:1272 */       if (ETPush.getLogLevel() <= 6) {
/* 1184:1273 */         Log.e("etpushsdk@RegionMonitor", "PlayServices connection failed: " + connectionResult.getErrorCode());
/* 1185:     */       }
/* 1186:     */     }
/* 1187:     */     
/* 1188:     */     public void onConnected(Bundle bundle)
/* 1189:     */     {
/* 1190:1278 */       if (ETPush.getLogLevel() <= 3) {
/* 1191:1279 */         Log.d("etpushsdk@RegionMonitor", "onConnected()");
/* 1192:     */       }
/* 1193:1282 */       if (this.removeAllGeofences.booleanValue())
/* 1194:     */       {
/* 1195:1283 */         ETLocationManager.this.locationClient.removeGeofences(getGeofencePendingIntent(), this);return;
/* 1196:     */       }
/* 1197:1286 */       updateGeofencesFromDatabase();
/* 1198:     */     }
/* 1199:     */     
/* 1200:     */     public void onDisconnected()
/* 1201:     */     {
/* 1202:1291 */       if (ETPush.getLogLevel() <= 3) {
/* 1203:1292 */         Log.d("etpushsdk@RegionMonitor", "onDisconnected()");
/* 1204:     */       }
/* 1205:     */     }
/* 1206:     */     
/* 1207:     */     private String getStatusString(int statusCode)
/* 1208:     */     {
/* 1209:1297 */       String statusString = null;
/* 1210:1298 */       switch (statusCode)
/* 1211:     */       {
/* 1212:     */       case 0: 
/* 1213:1300 */         statusString = "SUCCESS";
/* 1214:1301 */         break;
/* 1215:     */       case 1: 
/* 1216:1303 */         statusString = "ERROR";
/* 1217:1304 */         if (ETPush.getLogLevel() <= 6) {
/* 1218:1305 */           Log.e("etpushsdk@RegionMonitor", "LocationStatusCodes.ERROR");
/* 1219:     */         }
/* 1220:     */         break;
/* 1221:     */       case 1000: 
/* 1222:1309 */         statusString = "GEOFENCE_NOT_AVAILABLE";
/* 1223:1310 */         if (ETPush.getLogLevel() <= 5) {
/* 1224:1311 */           Log.w("etpushsdk@RegionMonitor", "LocationStatusCodes.GEOFENCE_NOT_AVAILABLE");
/* 1225:     */         }
/* 1226:     */         break;
/* 1227:     */       case 1001: 
/* 1228:1315 */         statusString = "GEOFENCE_TOO_MANY_GEOFENCES";
/* 1229:1316 */         if (ETPush.getLogLevel() <= 5) {
/* 1230:1317 */           Log.w("etpushsdk@RegionMonitor", "LocationStatusCodes.GEOFENCE_TOO_MANY_GEOFENCES");
/* 1231:     */         }
/* 1232:     */         break;
/* 1233:     */       case 1002: 
/* 1234:1321 */         statusString = "GEOFENCE_TOO_MANY_PENDING_INTENTS";
/* 1235:1322 */         if (ETPush.getLogLevel() <= 5) {
/* 1236:1323 */           Log.w("etpushsdk@RegionMonitor", "LocationStatusCodes.GEOFENCE_TOO_MANY_PENDING_INTENTS");
/* 1237:     */         }
/* 1238:     */         break;
/* 1239:     */       }
/* 1240:1327 */       return statusString;
/* 1241:     */     }
/* 1242:     */   }
/* 1243:     */   
/* 1244:     */   private class IBeaconMonitor
/* 1245:     */     implements IBeaconConsumer, MonitorNotifier, RangeNotifier
/* 1246:     */   {
/* 1247:     */     private static final String TAG = "etpushsdk@IBeaconMonitor";
/* 1248:     */     
/* 1249:     */     private IBeaconMonitor() {}
/* 1250:     */     
/* 1251:     */     public void onIBeaconServiceConnect()
/* 1252:     */     {
/* 1253:1337 */       Log.d("etpushsdk@IBeaconMonitor", "onIBeaconServiceConnect");
/* 1254:1338 */       ETLocationManager.this.iBeaconManager.setMonitorNotifier(this);
/* 1255:1339 */       ETLocationManager.this.iBeaconManager.setRangeNotifier(this);
/* 1256:1341 */       if ((ETLocationManager.this.isWatchingProximity()) && (ETLocationManager.this.getProximityInvalidated())) {
/* 1257:1343 */         ETLocationManager.this.startWatchingProximity();
/* 1258:     */       }
/* 1259:     */       BackgroundEvent bge;
/* 1260:1347 */       if (((bge = (BackgroundEvent)EventBus.getDefault().getStickyEvent(BackgroundEvent.class)) == null) || (bge.isInBackground())) {
/* 1261:1348 */         ETLocationManager.this.iBeaconManager.setBackgroundMode(ETLocationManager.this.iBeaconMonitor, true);
/* 1262:     */       }
/* 1263:     */     }
/* 1264:     */     
/* 1265:     */     public boolean bindService(Intent intent, ServiceConnection connection, int mode)
/* 1266:     */     {
/* 1267:1355 */       Log.d("etpushsdk@IBeaconMonitor", "bindService");
/* 1268:1356 */       return getApplicationContext().bindService(intent, connection, mode);
/* 1269:     */     }
/* 1270:     */     
/* 1271:     */     public Context getApplicationContext()
/* 1272:     */     {
/* 1273:1361 */       return ETLocationManager.this.applicationContext;
/* 1274:     */     }
/* 1275:     */     
/* 1276:     */     public void unbindService(ServiceConnection serviceConnection)
/* 1277:     */     {
/* 1278:1366 */       Log.d("etpushsdk@IBeaconMonitor", "unbindService");
/* 1279:1367 */       getApplicationContext().unbindService(serviceConnection);
/* 1280:     */     }
/* 1281:     */     
/* 1282:     */     public void didDetermineStateForRegion(int state, com.radiusnetworks.ibeacon.Region region)
/* 1283:     */     {
/* 1284:1373 */       if (ETPush.getLogLevel() <= 3) {
/* 1285:1374 */         Log.d("etpushsdk@IBeaconMonitor", "BeaconState - {state: " + state + ", id: '" + region.getUniqueId() + "', uuid: '" + region.getProximityUuid() + "', major: " + region.getMajor() + ", minor: " + region.getMinor() + "}");
/* 1286:     */       }
/* 1287:     */     }
/* 1288:     */     
/* 1289:     */     public void didEnterRegion(com.radiusnetworks.ibeacon.Region region)
/* 1290:     */     {
/* 1291:1380 */       if (ETPush.getLogLevel() <= 3) {
/* 1292:1381 */         Log.d("etpushsdk@IBeaconMonitor", "didEnterRegion - {id: '" + region.getUniqueId() + "', uuid: '" + region.getProximityUuid() + "', major: " + region.getMajor() + ", minor: " + region.getMinor() + "}");
/* 1293:     */       }
/* 1294:1383 */       final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(ETLocationManager.this.applicationContext);
/* 1295:     */       try
/* 1296:     */       {
/* 1297:     */         Dao<com.exacttarget.etpushsdk.data.Region, String> regionDao;
/* 1298:1387 */         (dbRegion = (com.exacttarget.etpushsdk.data.Region)(regionDao = helper.getRegionDao()).queryForId(region.getUniqueId())).setEntryCount(Integer.valueOf(dbRegion.getEntryCount().intValue() + 1));
/* 1299:1388 */         dbRegion.setHasEntered(Boolean.TRUE);
/* 1300:1389 */         regionDao.update(dbRegion);
/* 1301:1390 */         if (ETPush.getLogLevel() <= 3) {
/* 1302:1391 */           Log.d("etpushsdk@IBeaconMonitor", "Beacon: " + dbRegion.getId() + ", EntryCount: " + dbRegion.getEntryCount());
/* 1303:     */         }
/* 1304:1394 */         if (Config.isAnalyticsActive()) {
/* 1305:1395 */           ETAnalytics.engine().startTimeInRegionLog(dbRegion.getId(), true);
/* 1306:     */         }
/* 1307:1398 */         EventBus.getDefault().post(new BeaconRegionEnterEvent(dbRegion));
/* 1308:     */       }
/* 1309:     */       catch (SQLException e)
/* 1310:     */       {
/* 1311:1401 */         if (ETPush.getLogLevel() <= 6) {
/* 1312:1402 */           Log.e("etpushsdk@IBeaconMonitor", e.getMessage(), e);
/* 1313:     */         }
/* 1314:     */       }
/* 1315:     */       catch (ETException e)
/* 1316:     */       {
/* 1317:1406 */         if (ETPush.getLogLevel() <= 6) {
/* 1318:1407 */           Log.e("etpushsdk@IBeaconMonitor", e.getMessage(), e);
/* 1319:     */         }
/* 1320:     */       }
/* 1321:     */       finally
/* 1322:     */       {
/* 1323:1412 */         (e = new Handler(ETLocationManager.this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 1324:     */         {
/* 1325:     */           public void run()
/* 1326:     */           {
/* 1327:1415 */             if ((helper != null) && (helper.isOpen())) {
/* 1328:1416 */               helper.close();
/* 1329:     */             }
/* 1330:     */           }
/* 1331:1416 */         }, 10000L);
/* 1332:     */       }
/* 1333:     */     }
/* 1334:     */     
/* 1335:     */     public void didExitRegion(com.radiusnetworks.ibeacon.Region region)
/* 1336:     */     {
/* 1337:1425 */       if (ETPush.getLogLevel() <= 3) {
/* 1338:1426 */         Log.d("etpushsdk@IBeaconMonitor", "didExitRegion - {id: '" + region.getUniqueId() + "', uuid: '" + region.getProximityUuid() + "', major: " + region.getMajor() + ", minor: " + region.getMinor() + "}");
/* 1339:     */       }
/* 1340:1428 */       final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(ETLocationManager.this.applicationContext);
/* 1341:     */       try
/* 1342:     */       {
/* 1343:1430 */         Dao<com.exacttarget.etpushsdk.data.Region, String> regionDao = helper.getRegionDao();
/* 1344:1431 */         Dao<RegionMessage, Integer> regionMessageDao = helper.getRegionMessageDao();
/* 1345:1432 */         Dao<Message, String> messageDao = helper.getMessageDao();
/* 1346:     */         
/* 1347:     */ 
/* 1348:1435 */         (
/* 1349:1436 */           dbRegion = (com.exacttarget.etpushsdk.data.Region)regionDao.queryForId(region.getUniqueId())).setExitCount(Integer.valueOf(dbRegion.getExitCount().intValue() + 1));
/* 1350:1437 */         dbRegion.setHasEntered(Boolean.FALSE);
/* 1351:1438 */         regionDao.update(dbRegion);
/* 1352:1439 */         if (ETPush.getLogLevel() <= 3) {
/* 1353:1440 */           Log.d("etpushsdk@IBeaconMonitor", "Beacon: " + dbRegion.getId() + ", ExitCount: " + dbRegion.getEntryCount());
/* 1354:     */         }
/* 1355:1445 */         for (RegionMessage regionMessage : regionDao = regionMessageDao.queryForEq("region_id", dbRegion.getId()))
/* 1356:     */         {
/* 1357:     */           Message message;
/* 1358:1447 */           (message = (Message)messageDao.queryForId(regionMessage.getMessage().getId())).setHasEntered(Boolean.FALSE);
/* 1359:1448 */           message.setEntryTime(Long.valueOf(0L));
/* 1360:1449 */           if ((message.getEphemeralMessage().booleanValue()) && (message.getNotifyId() != null))
/* 1361:     */           {
/* 1362:     */             NotificationManager localNotificationManager;
/* 1363:1451 */             (localNotificationManager = (NotificationManager)ETLocationManager.this.applicationContext.getSystemService("notification")).cancel(message.getNotifyId().intValue());
/* 1364:1452 */             message.setNotifyId(null);
/* 1365:     */           }
/* 1366:1454 */           messageDao.update(message);
/* 1367:     */         }
/* 1368:1457 */         if (Config.isAnalyticsActive()) {
/* 1369:1458 */           ETAnalytics.engine().stopTimeInRegionLog(dbRegion.getId());
/* 1370:     */         }
/* 1371:1461 */         EventBus.getDefault().post(new BeaconRegionExitEvent(dbRegion));
/* 1372:     */       }
/* 1373:     */       catch (SQLException e)
/* 1374:     */       {
/* 1375:1464 */         if (ETPush.getLogLevel() <= 6) {
/* 1376:1465 */           Log.e("etpushsdk@IBeaconMonitor", e.getMessage(), e);
/* 1377:     */         }
/* 1378:     */       }
/* 1379:     */       catch (ETException e)
/* 1380:     */       {
/* 1381:1469 */         if (ETPush.getLogLevel() <= 6) {
/* 1382:1470 */           Log.e("etpushsdk@IBeaconMonitor", e.getMessage(), e);
/* 1383:     */         }
/* 1384:     */       }
/* 1385:     */       finally
/* 1386:     */       {
/* 1387:1475 */         (e = new Handler(ETLocationManager.this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 1388:     */         {
/* 1389:     */           public void run()
/* 1390:     */           {
/* 1391:1478 */             if ((helper != null) && (helper.isOpen())) {
/* 1392:1479 */               helper.close();
/* 1393:     */             }
/* 1394:     */           }
/* 1395:1479 */         }, 10000L);
/* 1396:     */       }
/* 1397:     */     }
/* 1398:     */     
/* 1399:     */     public void didRangeBeaconsInRegion(Collection<IBeacon> beacons, com.radiusnetworks.ibeacon.Region region)
/* 1400:     */     {
/* 1401:     */       ;
/* 1402:1491 */       final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(ETLocationManager.this.applicationContext);
/* 1403:     */       try
/* 1404:     */       {
/* 1405:1493 */         Dao<com.exacttarget.etpushsdk.data.Region, String> regionDao = helper.getRegionDao();
/* 1406:1494 */         for (IBeacon beacon : beacons)
/* 1407:     */         {
/* 1408:1495 */           if (ETPush.getLogLevel() <= 3) {
/* 1409:1496 */             Log.d("etpushsdk@IBeaconMonitor", "didRangeBeaconsInRegion - {proximity: " + beacon.getProximity() + ", id: '" + region.getUniqueId() + "', uuid: '" + region.getProximityUuid() + "', major: " + region.getMajor() + ", minor: " + region.getMinor() + "}");
/* 1410:     */           }
/* 1411:     */           com.exacttarget.etpushsdk.data.Region dbRegion;
/* 1412:1500 */           if ((dbRegion = (com.exacttarget.etpushsdk.data.Region)regionDao.queryForId(region.getUniqueId())) != null) {
/* 1413:1501 */             if (dbRegion.getHasEntered().booleanValue())
/* 1414:     */             {
/* 1415:1502 */               ETPush.pushManager().showFenceOrProximityMessage(region.getUniqueId(), -1, beacon.getProximity());
/* 1416:1503 */               EventBus.getDefault().post(new BeaconRegionRangeEvent(dbRegion, beacon.getProximity(), beacon.getRssi(), beacon.getAccuracy()));
/* 1417:     */             }
/* 1418:1506 */             else if (ETPush.getLogLevel() <= 3)
/* 1419:     */             {
/* 1420:1507 */               Log.d("etpushsdk@IBeaconMonitor", "Ranged region " + dbRegion.getId() + " but monitoring hasn't yet entered");
/* 1421:     */             }
/* 1422:     */           }
/* 1423:     */         }
/* 1424:     */       }
/* 1425:     */       catch (ETException e)
/* 1426:     */       {
/* 1427:1515 */         if (ETPush.getLogLevel() <= 6) {
/* 1428:1516 */           Log.e("etpushsdk@IBeaconMonitor", e.getMessage(), e);
/* 1429:     */         }
/* 1430:     */       }
/* 1431:     */       catch (SQLException e)
/* 1432:     */       {
/* 1433:1520 */         if (ETPush.getLogLevel() <= 6) {
/* 1434:1521 */           Log.e("etpushsdk@IBeaconMonitor", e.getMessage(), e);
/* 1435:     */         }
/* 1436:     */       }
/* 1437:     */       finally
/* 1438:     */       {
/* 1439:1526 */         (region = new Handler(ETLocationManager.this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 1440:     */         {
/* 1441:     */           public void run()
/* 1442:     */           {
/* 1443:1529 */             if ((helper != null) && (helper.isOpen())) {
/* 1444:1530 */               helper.close();
/* 1445:     */             }
/* 1446:     */           }
/* 1447:1530 */         }, 10000L);
/* 1448:     */       }
/* 1449:     */     }
/* 1450:     */   }
/* 1451:     */ }



/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar

 * Qualified Name:     com.exacttarget.etpushsdk.ETLocationManager

 * JD-Core Version:    0.7.0.1

 */