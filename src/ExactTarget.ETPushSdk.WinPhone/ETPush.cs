using android.app.Activity;
using android.app.IntentService;
using android.content.BroadcastReceiver;
using android.content.Context;
using android.content.Intent;
using android.content.SharedPreferences;
using android.content.SharedPreferences.Editor;
using android.content.pm.PackageInfo;
using android.content.pm.PackageManager;
using android.content.pm.PackageManager.NameNotFoundException;
using android.content.pm.ResolveInfo;
using android.database.sqlite.SQLiteDatabase;
using android.net.Uri;
using android.os.AsyncTask;
using android.os.Handler;
using android.os.PowerManager;
using android.os.PowerManager.WakeLock;
using android.util.Log;
using com.exacttarget.etpushsdk.data.Attribute;
using com.exacttarget.etpushsdk.data.ETSqliteOpenHelper;
using com.exacttarget.etpushsdk.data.Message;
using com.exacttarget.etpushsdk.data.Region;
using com.exacttarget.etpushsdk.data.RegionMessage;
using com.exacttarget.etpushsdk.data.Registration;
using com.exacttarget.etpushsdk.event.BackgroundEvent;
using com.exacttarget.etpushsdk.event.RegistrationEvent;
using com.exacttarget.etpushsdk.event.RegistrationEventListener;
using com.exacttarget.etpushsdk.location.receiver.BootReceiver;
using com.exacttarget.etpushsdk.location.receiver.LocationChangedReceiver;
using com.exacttarget.etpushsdk.location.receiver.PassiveLocationChangedReceiver;
using com.exacttarget.etpushsdk.location.receiver.PowerStateChangedReceiver;
using com.exacttarget.etpushsdk.util.ETAmazonDeviceMessagingUtil;
using com.exacttarget.etpushsdk.util.ETGooglePlayServicesUtil;
using com.exacttarget.etpushsdk.util.EventBus;
using com.google.android.gms.gcm.GoogleCloudMessaging;
using com.j256.ormlite.dao.Dao;
using com.j256.ormlite.stmt.DeleteBuilder;
using com.j256.ormlite.stmt.Where;
using java.io.IOException;
using java.lang.reflect.Constructor;
using java.lang.reflect.Method;
using java.util.ArrayList;
using java.util.Calendar;
using java.util.Date;
using java.util.HashSet;
using java.util.List;
using java.util.Set;

namespace ExactTarget.ETPushSdk
{

public class ETPush : RegistrationEventListener
{
  private static final String TAG = "etpushSDK@ETPush";
  private static final String PUSH_ENABLED_KEY = "et_push_enabled";
  private static final String GCM_REG_ID_KEY = "gcm_reg_id_key";
  private static final String GCM_APP_VERSION_KEY = "gcm_app_version_key";
  private static final String GCM_SENDER_ID_KEY = "gcm_sender_id_key";
  public static final String ETPushSDKVersionString = "3.3.0";
  public static final int ETPushSDKVersionNumber = 3300;
  private Class<?> recipentClass;
  private Class<?> openDirectRecipient;
  private Class<?> cloudPageRecipient;
  private String notificationAction = null;
  private Uri notificationActionUri = null;
  private static String gcmSenderID;
  private static Context applicationContext;
  private static SharedPreferences prefs;
  private static ETPush pushManager;
  private static Set<Integer> activityHashSet = new HashSet();
  private static PauseWaitTask pauseWaitTask = null;
  private static PowerManager.WakeLock wakeLock;
  private static Registration registration;
  private static Integer logLevel = Integer.valueOf(5);
  private Object adm = null;
  
  private ETPush(Context applicationContext)
  {
    applicationContext = applicationContext;
    prefs = applicationContext.getSharedPreferences("ETPush", 0);
    
    (
      ETPush.registration = new Registration(applicationContext)).setDeviceToken(getRegistrationId());
    

    wakeLock = (applicationContext = (PowerManager)applicationContext.getSystemService("power")).newWakeLock(1, "etpushSDK@ETPush");
    
    EventBus.getDefault().register(this);
  }
  
  public static ETPush pushManager()
    throws ETException
  {
    if (pushManager == null) {
      throw new ETException("No ETPush instance available for use. Did you forget to call readyAimFire() first?");
    }
    return pushManager;
  }
  
  public static void resetPush(Context applicationContext, String etAppId, String accessToken)
    throws ETException
  {
    if (getLogLevel() <= 3) {
      Log.d("etpushSDK@ETPush", "Reset ETPush");
    }
    if (Config.isLocationManagerActive())
    {
      if (ETLocationManager.locationManager().isWatchingLocation()) {
        ETLocationManager.locationManager().stopWatchingLocation();
      }
      if (ETLocationManager.locationManager().isWatchingProximity()) {
        ETLocationManager.locationManager().stopWatchingProximity();
      }
      ETLocationManager.resetLocationManager(applicationContext);
    }
    if (Config.isAnalyticsActive()) {
      ETAnalytics.resetAnalytics(applicationContext);
    }
    if (Config.isCloudPagesActive()) {
      ETCloudPageManager.resetCloudPages(applicationContext);
    }
    pushManager = null;
    registration = null;
    gcmSenderID = null;
    

    prefs.edit().clear().apply();
    prefs = applicationContext.getSharedPreferences("ETPush", 0);
    ETSqliteOpenHelper localETSqliteOpenHelper;
    (localETSqliteOpenHelper = ETSqliteOpenHelper.getHelper(applicationContext)).reset(applicationContext);
    
    readyAimFire(applicationContext, etAppId, accessToken, Config.isAnalyticsActive(), Config.isLocationManagerActive(), Config.isCloudPagesActive());
  }
  
  public static void readyAimFire(Context applicationContext, String etAppId, String accessToken)
    throws ETException
  {
    readyAimFire(applicationContext, etAppId, accessToken, true, true, true);
  }
  
  public static void readyAimFire(Context applicationContext, String etAppId, String accessToken, boolean enableAnalytics, boolean enableLocationManager, boolean enableCloudPages)
    throws ETException
  {
    if (pushManager == null)
    {
      if (getLogLevel() <= 3) {
        Log.d("etpushSDK@ETPush", "readyAimFire()");
      }
      pushManager = new ETPush(applicationContext);
      if (!etAppId.toLowerCase().matches("[0-9a-f]{8}-[a-f0-9]{4}-4[a-f0-9]{3}-[89aAbB][a-f0-9]{3}-[a-f0-9]{12}"))
      {
        if (getLogLevel() <= 6) {
          Log.e("etpushSDK@ETPush:readyAimFire", "ERROR: The etAppId is not a valid UUID. Did you copy/paste incorrectly?");
        }
        throw new ETException("The etAppId is not a valid UUID. Did you copy/paste incorrectly?");
      }
      if (accessToken.length() != 24)
      {
        if (getLogLevel() <= 6) {
          Log.e("etpushSDK@ETPush:readyAimFire", "ERROR: The accessToken is not 24 characters. Did you copy/paste incorrectly?");
        }
        throw new ETException("The accessToken is not 24 characters. Did you copy/paste incorrectly?");
      }
      Config.setEtAppId(etAppId);
      Config.setAccessToken(accessToken);
      
      Config.setAnalyticsActive(enableAnalytics);
      Config.setLocationManagerActive(enableLocationManager);
      Config.setCloudPagesActive(enableCloudPages);
      if (!Config.isReadOnly(applicationContext)) {
        checkManifestSetup(applicationContext);
      }
      if (enableAnalytics) {
        ETAnalytics.readyAimFire(applicationContext);
      }
      if (enableLocationManager) {
        ETLocationManager.readyAimFire(applicationContext);
      }
      if (enableCloudPages) {
        ETCloudPageManager.readyAimFire(applicationContext);
      }
    }
    else
    {
      throw new ETException("You must have called readyAimFire more than once. It should only be called from your Application's Application#onCreate() method.");
    }
  }
  
  private static void checkManifestSetup(Context applicationContext)
    throws ETException
  {
    PackageManager packageManager = applicationContext.getPackageManager();
    String packageName = applicationContext.getPackageName();
    List<String> permissions;
    (permissions = new ArrayList()).add(packageName + ".permission.C2D_MESSAGE");
    if (ETGooglePlayServicesUtil.isAvailable(applicationContext, false)) {
      permissions.add("com.google.android.c2dm.permission.RECEIVE");
    }
    if ((ETAmazonDeviceMessagingUtil.isAmazonDevice()) && 
      (!ETAmazonDeviceMessagingUtil.isAvailable(applicationContext, false))) {
      throw new ETException("ApplicationManifest.xml missing required manifest entries for Amazon Device Messaging.");
    }
    permissions.add("android.permission.INTERNET");
    permissions.add("android.permission.GET_ACCOUNTS");
    permissions.add("android.permission.WAKE_LOCK");
    permissions.add("android.permission.ACCESS_NETWORK_STATE");
    permissions.add("android.permission.WRITE_EXTERNAL_STORAGE");
    permissions.add("android.permission.ACCESS_WIFI_STATE");
    if (Config.isLocationManagerActive())
    {
      permissions.add("android.permission.ACCESS_COARSE_LOCATION");
      permissions.add("android.permission.ACCESS_FINE_LOCATION");
      permissions.add("android.permission.RECEIVE_BOOT_COMPLETED");
    }
    for (String permission : permissions) {
      if (0 != packageManager.checkPermission(permission, packageName)) {
        throw new ETException("ApplicationManifest.xml missing required permission: " + permission);
      }
    }
    Intent regIntent;
    (regIntent = new Intent("com.google.android.c2dm.intent.REGISTRATION")).addCategory(packageName);
    List receiversInfo;
    if (((receiversInfo = packageManager.queryBroadcastReceivers(regIntent, 0)) == null) || (receiversInfo.size() <= 0)) {
      throw new ETException("No BroadcastReceiver defined in ApplicationManifest.xml to handle GCM registration. Did you forget to add ET_GenericReceiver to your manifest or mis-configure it?");
    }
    Intent pushIntent;
    (pushIntent = new Intent("com.google.android.c2dm.intent.RECEIVE")).addCategory(packageName);
    if (((receiversInfo = packageManager.queryBroadcastReceivers(pushIntent, 0)) == null) || (receiversInfo.size() <= 0)) {
      throw new ETException("No BroadcastReceiver defined in ApplicationManifest.xml to handle GCM messages. Did you forget to add ET_GenericReceiver to your manifest or mis-configure it?");
    }
    List<Class<? extends BroadcastReceiver>> receiverClasses;
    (receiverClasses = new ArrayList()).add(ETSendDataReceiver.class);
    if (Config.isLocationManagerActive())
    {
      receiverClasses.add(LocationChangedReceiver.class);
      receiverClasses.add(PassiveLocationChangedReceiver.class);
      receiverClasses.add(PowerStateChangedReceiver.class);
      receiverClasses.add(ETLocationTimeoutReceiver.class);
      receiverClasses.add(ETLocationWakeupReceiver.class);
      receiverClasses.add(ETLocationProviderChangeReceiver.class);
      receiverClasses.add(ETGeofenceReceiver.class);
      receiverClasses.add(BootReceiver.class);
    }
    for (Class<? extends BroadcastReceiver> receiverClass : receiverClasses)
    {
      Intent locIntent = new Intent(applicationContext, receiverClass);
      if (((locIntent = packageManager.queryBroadcastReceivers(locIntent, 0)) == null) || (locIntent.size() <= 0)) {
        throw new ETException("ApplicationManifest.xml missing BroadcastReceiver: " + receiverClass.getName());
      }
    }
    List<Class<? extends IntentService>> serviceClasses;
    (serviceClasses = new ArrayList()).add(ETSendDataIntentService.class);
    if (Config.isLocationManagerActive())
    {
      serviceClasses.add(ETLocationTimeoutService.class);
      serviceClasses.add(ETLocationWakeupService.class);
      serviceClasses.add(ETLocationProviderChangeService.class);
      serviceClasses.add(ETGeofenceIntentService.class);
    }
    for (Class<? extends IntentService> serviceClass : serviceClasses)
    {
      Intent serviceIntent = new Intent(applicationContext, serviceClass);
      List<ResolveInfo> servicesInfo;
      if (((servicesInfo = packageManager.queryIntentServices(serviceIntent, 0)) == null) || (servicesInfo.size() <= 0)) {
        throw new ETException("ApplicationManifest.xml missing Service: " + serviceClass.getName());
      }
    }
    if ((!checkActivityExistsInManifest(applicationContext, ETLandingPagePresenter.class)) && 
      (getLogLevel() <= 5)) {
      Log.w("etpushSDK@ETPush", ETLandingPagePresenter.class.getName() + " is not found in AndroidManifest.  This will impact the ability to display CloudPages and OpenDirect URLs.");
    }
  }
  
  protected static boolean checkActivityExistsInManifest(Context applicationContext, Class<?> cls)
  {
    Intent recipientIntent;
    (recipientIntent = new Intent()).setClass(applicationContext, cls);
    if ((applicationContext = applicationContext.getPackageManager().queryIntentActivities(recipientIntent, 65536)).size() <= 0) {
      return false;
    }
    return true;
  }
  
  private void registerForRemoteNotifications()
    throws ETException
  {
    if (ETAmazonDeviceMessagingUtil.isAmazonDevice())
    {
      if (ETAmazonDeviceMessagingUtil.isAvailable(applicationContext, true))
      {
        String regid;
        if ((regid = ADM_getRegistrationId()) == null) {
          ADM_startRegister();
        } else {
          registerDeviceToken(regid);
        }
      }
    }
    else if (ETGooglePlayServicesUtil.isAvailable(applicationContext, true))
    {
      String regid;
      if ((regid = getRegistrationId()).isEmpty())
      {
        registerGCM_inBackground();return;
      }
      registerDeviceToken(regid);
    }
  }
  
  private void registerGCM_inBackground()
  {
    new AsyncTask()
    {
      protected String doInBackground(Void... params)
      {
        try
        {
          String regid = (params = GoogleCloudMessaging.getInstance(ETPush.applicationContext)).register(new String[] { ETPush.gcmSenderID });
          msg = "Device registered, registration ID=" + regid;
        }
        catch (IOException ex)
        {
          String msg;
          if (ETPush.getLogLevel() <= 6) {
            Log.e("etpushSDK@ETPush", ex.getMessage(), ex);
          }
          ex = "Error :" + ex.getMessage();
        }
        return ex;
      }
      
      protected void onPostExecute(String msg)
      {
        if (ETPush.getLogLevel() <= 3) {
          Log.d("etpushSDK@ETPush", msg);
        }
      }
    }.execute(new Void[] { null, null, null });
  }
  
  private void storeRegistrationId(String regId)
  {
    int appVersion = getAppVersion();
    if (getLogLevel() <= 3) {
      Log.d("etpushSDK@ETPush", "Saving regId and app version " + appVersion);
    }
    SharedPreferences.Editor editor;
    (editor = prefs.edit()).putString("gcm_reg_id_key", regId);
    editor.putInt("gcm_app_version_key", appVersion);
    editor.putString("gcm_sender_id_key", gcmSenderID);
    editor.commit();
  }
  
  private String getRegistrationId()
  {
    String registrationId;
    if ((registrationId = (String)Config.getETSharedPref(applicationContext, applicationContext.getSharedPreferences("etpushSDK@ETPush", 0), "gcm_reg_id_key", "")).isEmpty())
    {
      if (getLogLevel() <= 4) {
        Log.i("etpushSDK@ETPush", "Registration not found.");
      }
      return "";
    }
    int registeredVersion = ((Integer)Config.getETSharedPref(applicationContext, applicationContext.getSharedPreferences("etpushSDK@ETPush", 0), "gcm_app_version_key", Integer.valueOf(-2147483648))).intValue();
    int currentVersion = getAppVersion();
    if (registeredVersion != currentVersion)
    {
      if (getLogLevel() <= 4) {
        Log.i("etpushSDK@ETPush", "App version changed.");
      }
      return "";
    }
    if (!(registeredVersion = (String)Config.getETSharedPref(applicationContext, applicationContext.getSharedPreferences("etpushSDK@ETPush", 0), "gcm_sender_id_key", "")).equals(gcmSenderID))
    {
      if (getLogLevel() <= 4) {
        Log.i("etpushSDK@ETPush", "GCM Sender Id changed.");
      }
      return "";
    }
    return registrationId;
  }
  
  private int getAppVersion()
  {
    try
    {
      PackageInfo localPackageInfo;
      return (localPackageInfo = applicationContext.getPackageManager().getPackageInfo(applicationContext.getPackageName(), 0)).versionCode;
    }
    catch (PackageManager.NameNotFoundException e)
    {
      throw new RuntimeException("Could not get package name: " + e);
    }
  }
  
  private void setPushStateInPreferences(boolean state)
  {
    prefs.edit().putBoolean("et_push_enabled", state).apply();
  }
  
  public boolean isPushEnabled()
  {
    return ((Boolean)Config.getETSharedPref(applicationContext, applicationContext.getSharedPreferences("etpushSDK@ETPush", 0), "et_push_enabled", Boolean.valueOf(false))).booleanValue();
  }
  
  public void enablePush()
    throws ETException
  {
    if ((ETGooglePlayServicesUtil.isAvailable(applicationContext, true)) || (ETAmazonDeviceMessagingUtil.isAvailable(applicationContext, true)))
    {
      setPushStateInPreferences(true);
      if (!Config.isReadOnly(applicationContext)) {
        registerForRemoteNotifications();
      }
    }
    else if (isPushEnabled())
    {
      if (getLogLevel() <= 5) {
        Log.w("etpushSDK@ETPush", "Push was enabled, but now disabled since Google Play or Amazon Messaging not available");
      }
      disablePush();
    }
  }
  
  public void disablePush()
    throws ETException
  {
    setPushStateInPreferences(false);
    if (!Config.isReadOnly(applicationContext)) {
      registerDeviceToken("");
    }
  }
  
  public void activityPaused(Activity activity)
  {
    if (!Config.isAnalyticsActive())
    {
      if (getLogLevel() <= 5) {
        Log.w("etpushSDK@ETPush", "--WARNING-- activityPaused() called, but analytics is disabled.");
      }
      return;
    }
    Integer hashCode = Integer.valueOf(activity.hashCode());
    if (activityHashSet.contains(hashCode))
    {
      if (getLogLevel() <= 3) {
        Log.d("etpushSDK@ETPush", "paused: " + hashCode);
      }
      activityHashSet.remove(hashCode);
    }
    else if (getLogLevel() <= 5)
    {
      Log.w("etpushSDK@ETPush", "unrecognized activity: " + hashCode);
    }
    if (activityHashSet.isEmpty()) {
      if (pauseWaitTask == null)
      {
        if (getLogLevel() <= 3) {
          Log.d("etpushSDK@ETPush", "start pauseWaitTask");
        }
        wakeLock.acquire();
        (
          ETPush.pauseWaitTask = new PauseWaitTask(null)).execute(new Void[0]);
      }
    }
  }
  
  public void activityResumed(Activity activity)
  {
    if (!Config.isAnalyticsActive())
    {
      if (getLogLevel() <= 5) {
        Log.w("etpushSDK@ETPush", "--WARNING-- activityResumed() called, but analytics is disabled.");
      }
      return;
    }
    Integer hashCode = Integer.valueOf(activity.hashCode());
    if ((activityHashSet.isEmpty()) && (pauseWaitTask == null)) {
      EventBus.getDefault().postSticky(new BackgroundEvent(false));
    }
    if (!activityHashSet.contains(hashCode))
    {
      if (getLogLevel() <= 3) {
        Log.d("etpushSDK@ETPush", "resumed: " + hashCode);
      }
      activityHashSet.add(hashCode);
      if ((pauseWaitTask != null) && (!pauseWaitTask.isCancelled())) {
        pauseWaitTask.cancel(true);
      }
      pauseWaitTask = null;return;
    }
    if (getLogLevel() <= 5) {
      Log.w("etpushSDK@ETPush", "activityResumed() already called for this activity.");
    }
  }
  
  public void setNotificationRecipientClass(Class<?> cls)
    throws ETException
  {
    if (!checkActivityExistsInManifest(applicationContext, cls)) {
      throw new ETException(cls.getName() + " is not found in AndroidManifest.");
    }
    this.recipentClass = cls;
  }
  
  public Class<?> getNotificationRecipientClass()
  {
    return this.recipentClass;
  }
  
  public void addAttribute(String attribute, String value)
    throws ETException
  {
    if (registration == null) {
      (ETPush.registration = new Registration(applicationContext)).setDeviceToken(getRegistrationId());
    }
    registration.addAttribute(new Attribute(attribute, value));
  }
  
  public void removeAttribute(String attribute)
    throws ETException
  {
    if (registration == null) {
      (ETPush.registration = new Registration(applicationContext)).setDeviceToken(getRegistrationId());
    }
    registration.removeAttribute(new Attribute(attribute, ""));
  }
  
  public ArrayList<Attribute> getAttributes()
    throws ETException
  {
    if (registration == null) {
      (ETPush.registration = new Registration(applicationContext)).setDeviceToken(getRegistrationId());
    }
    return registration.getAttributes();
  }
  
  public void addTag(String tag)
    throws ETException
  {
    if (registration == null) {
      (ETPush.registration = new Registration(applicationContext)).setDeviceToken(getRegistrationId());
    }
    registration.addTag(tag);
  }
  
  public void removeTag(String tag)
    throws ETException
  {
    if (registration == null) {
      (ETPush.registration = new Registration(applicationContext)).setDeviceToken(getRegistrationId());
    }
    registration.removeTag(tag);
  }
  
  public HashSet<String> getTags()
    throws ETException
  {
    if (registration == null) {
      (ETPush.registration = new Registration(applicationContext)).setDeviceToken(getRegistrationId());
    }
    return registration.getTags();
  }
  
  public void setSubscriberKey(String subKey)
    throws ETException
  {
    if (registration == null) {
      (ETPush.registration = new Registration(applicationContext)).setDeviceToken(getRegistrationId());
    }
    registration.setSubscriberKey(subKey);
  }
  
  public static void setLogLevel(int logLevel)
    throws ETException
  {
    if ((logLevel >= 2) && (logLevel <= 7)) {
      logLevel = Integer.valueOf(logLevel);
    } else {
      throw new ETException("logLevel must be between Log.VERBOSE and Log.ASSERT");
    }
    if (logLevel.intValue() <= 3) {
      Log.d("etpushSDK@ETPush", "Logging set to DEBUG.");
    }
  }
  
  public static int getLogLevel()
  {
    return logLevel.intValue();
  }
  
  protected void registerDeviceToken(String deviceToken)
    throws ETException
  {
    if (registration == null) {
      registration = new Registration(applicationContext);
    }
    registration.setDeviceToken(deviceToken);
    if (!deviceToken.isEmpty()) {
      storeRegistrationId(deviceToken);
    }
    sendRegistration();
  }
  
  public void onEvent(RegistrationEvent event)
  {
    if (getLogLevel() <= 3) {
      Log.d("etpushSDK@ETPush", "onEventRegistrationEvent() id=" + event.getId());
    }
    if ((event.getId() != null) && (event.getId().intValue() > 0))
    {
      final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(applicationContext);
      try
      {
        DeleteBuilder<Registration, Integer> deleteBuilder;
        (deleteBuilder = helper.getRegistrationDao().deleteBuilder()).where().le("id", event.getId());
        int rowsUpdated;
        if ((rowsUpdated = deleteBuilder.delete()) > 0)
        {
          if (getLogLevel() <= 3) {
            Log.d("etpushSDK@ETPush", "success, removed sent registration id from db: " + event.getId());
          }
        }
        else if (getLogLevel() <= 6) {
          Log.e("etpushSDK@ETPush", "Error: rowsUpdated = " + rowsUpdated);
        }
      }
      catch (java.sql.SQLException e)
      {
        if (getLogLevel() <= 6) {
          Log.e("etpushSDK@ETPush", e.getMessage(), e);
        }
      }
      finally
      {
        (e = new Handler(applicationContext.getMainLooper())).postDelayed(new Runnable()
        {
          public void run()
          {
            if ((helper != null) && (helper.isOpen())) {
              helper.close();
            }
          }
        }, 10000L);
      }
    }
  }
  
  private void sendRegistration()
  {
    final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(applicationContext);
    try
    {
      registration.setId(null);
      registration.setEtAppId(Config.getEtAppId());
      registration.setPushEnabled(Boolean.valueOf(isPushEnabled()));
      helper.getRegistrationDao().create(registration);
      Intent sendDataIntent;
      (sendDataIntent = new Intent(applicationContext, ETSendDataReceiver.class)).putExtra("et_send_type_extra", "et_send_type_registration");
      applicationContext.sendBroadcast(sendDataIntent);
    }
    catch (java.sql.SQLException e)
    {
      if (getLogLevel() <= 6) {
        Log.e("etpushSDK@ETPush", e.getMessage(), e);
      }
    }
    finally
    {
      Handler localHandler;
      (localHandler = new Handler(applicationContext.getMainLooper())).postDelayed(new Runnable()
      {
        public void run()
        {
          if ((helper != null) && (helper.isOpen())) {
            helper.close();
          }
        }
      }, 10000L);
    }
  }
  
  private Object ADM_get()
    throws ETException
  {
    if (this.adm == null) {
      try
      {
        Class localClass;
        Object ctor = (localClass = Class.forName("com.amazon.device.messaging.ADM")).getConstructor(new Class[] { Context.class });
        this.adm = ((Constructor)ctor).newInstance(new Object[] { applicationContext });
      }
      catch (ClassNotFoundException localClassNotFoundException)
      {
        throw new ETException("unable to find com.amazon.device.messaging.ADM");
      }
      catch (Exception e)
      {
        throw new ETException(e.getCause().getMessage());
      }
    }
    return this.adm;
  }
  
  private String ADM_getRegistrationId()
    throws ETException
  {
    try
    {
      Class localClass;
      registrationId = (String)(localClass = Class.forName("com.amazon.device.messaging.ADM")).getMethod("getRegistrationId", new Class[0]).invoke(ADM_get(), new Object[0]);
    }
    catch (ClassNotFoundException localClassNotFoundException)
    {
      String registrationId;
      throw new ETException("unable to find com.amazon.device.messaging.ADM");
    }
    catch (Exception e)
    {
      throw new ETException(e.getCause().getMessage());
    }
    return e;
  }
  
  private void ADM_startRegister()
    throws ETException
  {
    try
    {
      Class localClass;
      (localClass = Class.forName("com.amazon.device.messaging.ADM")).getMethod("startRegister", new Class[0]).invoke(ADM_get(), new Object[0]);
      





      return;
    }
    catch (ClassNotFoundException localClassNotFoundException)
    {
      throw new ETException("unable to find com.amazon.device.messaging.ADM");
    }
    catch (Exception e)
    {
      throw new ETException(e.getCause().getMessage());
    }
  }
  
  public String getDeviceToken()
    throws ETException
  {
    if (registration == null) {
      (ETPush.registration = new Registration(applicationContext)).setDeviceToken(getRegistrationId());
    }
    return registration.getDeviceToken();
  }
  
  protected void unregisterDeviceToken()
    throws ETException
  {
    registerDeviceToken("");
  }
  
  public void setGcmSenderID(String gcmSenderID)
  {
    if (registration == null) {
      (ETPush.registration = new Registration(applicationContext)).setDeviceToken(getRegistrationId());
    }
    registration.setGcmSenderId(gcmSenderID);
    gcmSenderID = gcmSenderID;
  }
  
  public Class<?> getOpenDirectRecipient()
  {
    return this.openDirectRecipient;
  }
  
  public void setOpenDirectRecipient(Class<?> openDirectRecipient)
    throws ETException
  {
    if (!checkActivityExistsInManifest(applicationContext, openDirectRecipient)) {
      throw new ETException(openDirectRecipient.getName() + " is not found in AndroidManifest.");
    }
    this.openDirectRecipient = openDirectRecipient;
  }
  
  /**
   * @deprecated
   */
  protected Class<?> getCloudPageRecipient()
  {
    return this.cloudPageRecipient;
  }
  
  /**
   * @deprecated
   */
  protected void setCloudPageRecipient(Class<?> cloudPageRecipient)
  {
    this.cloudPageRecipient = cloudPageRecipient;
  }
  
  protected String getNotificationAction()
  {
    return this.notificationAction;
  }
  
  public void setNotificationAction(String notificationAction)
  {
    this.notificationAction = notificationAction;
  }
  
  protected Uri getNotificationActionUri()
  {
    return this.notificationActionUri;
  }
  
  public void setNotificationActionUri(Uri notificationActionUri)
  {
    this.notificationActionUri = notificationActionUri;
  }
  
  protected void showFenceOrProximityMessage(String regionId, int transitionType, int proximity)
  {
    if (getLogLevel() <= 3) {
      Log.d("etpushSDK@ETPush", "showFenceOrProximityMessage()");
    }
    try
    {
      if (!isPushEnabled())
      {
        if (getLogLevel() <= 3) {
          Log.d("etpushSDK@ETPush", "Push is disabled, no fence or proximity messages will show. Thanks for playing.");
        }
        return;
      }
      if ((!ETLocationManager.locationManager().isWatchingLocation()) && ((transitionType == 1) || (transitionType == 2)))
      {
        if (getLogLevel() <= 3) {
          Log.d("etpushSDK@ETPush", "Location is disabled, no fence messages will show. Thanks for playing.");
        }
        return;
      }
      if ((!ETLocationManager.locationManager().isWatchingProximity()) && (transitionType != 1) && (transitionType != 2))
      {
        if (getLogLevel() <= 3) {
          Log.d("etpushSDK@ETPush", "Proximity is disabled, no beacon messages will show. Thanks for playing.");
        }
        return;
      }
    }
    catch (ETException e)
    {
      if (getLogLevel() <= 6) {
        Log.e("etpushSDK@ETPush", e.getMessage(), e);
      }
    }
    Date now = new Date();
    final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(applicationContext);
    try
    {
      Region region = (Region)helper.getRegionDao().queryForId(regionId);
      switch (transitionType)
      {
      case 2: 
        region.setExitCount(Integer.valueOf(region.getExitCount().intValue() + 1));
        helper.getRegionDao().update(region);
        if (getLogLevel() <= 3) {
          Log.d("etpushSDK@ETPush", "GeofenceExitCount: " + region.getId() + ": " + region.getExitCount());
        }
        if (Config.isAnalyticsActive()) {
          ETAnalytics.engine().stopTimeInRegionLog(regionId);
        }
        break;
      case 1: 
        region.setEntryCount(Integer.valueOf(region.getEntryCount().intValue() + 1));
        helper.getRegionDao().update(region);
        if (getLogLevel() <= 3) {
          Log.d("etpushSDK@ETPush", "GeofenceEntryCount: " + region.getId() + ": " + region.getEntryCount());
        }
        if (Config.isAnalyticsActive()) {
          ETAnalytics.engine().startTimeInRegionLog(regionId, false);
        }
        break;
      }
      List<String> displayedMessageIds = new ArrayList();
      List localList;
      for (RegionMessage regionMessage : localList = helper.getRegionMessageDao().queryForEq("region_id", regionId))
      {
        Message message;
        if ((message = (Message)helper.getMessageDao().queryForId(regionMessage.getMessage().getId())) != null) {
          if ((Message.MESSAGE_TYPE_FENCE_ENTRY.equals(message.getMessageType())) && (2 == transitionType))
          {
            if (getLogLevel() <= 3) {
              Log.d("etpushSDK@ETPush", "ignoring message " + message.getId() + " because it's an entry and we were triggered by an exit");
            }
          }
          else if ((Message.MESSAGE_TYPE_FENCE_EXIT.equals(message.getMessageType())) && (1 == transitionType))
          {
            if (getLogLevel() <= 3) {
              Log.d("etpushSDK@ETPush", "ignoring message " + message.getId() + " because it's an exit and we were triggered by an entry");
            }
          }
          else if ((message.getEndDate() != null) && (message.getEndDate().before(now)))
          {
            if (getLogLevel() <= 3) {
              Log.d("etpushSDK@ETPush", "fence or proximity message " + message.getId() + " has expired, deleting...");
            }
            helper.getMessageDao().delete(message);
            helper.getRegionMessageDao().delete(regionMessage);
          }
          else if ((message.getStartDate() != null) && (message.getStartDate().after(now)))
          {
            if (getLogLevel() <= 3) {
              Log.d("etpushSDK@ETPush", "fence or proximity message " + message.getId() + " hasn't started yet: " + message.getStartDate());
            }
          }
          else if ((message.getMessageLimit().intValue() >= 0) && (message.getShowCount().intValue() >= message.getMessageLimit().intValue()))
          {
            if (getLogLevel() <= 3) {
              Log.d("etpushSDK@ETPush", "fence or proximity message " + message.getId() + " hit its messageLimit, not showing.");
            }
          }
          else if ((message.getMessagesPerPeriod().intValue() >= 0) && 
            (message.getPeriodShowCount().intValue() >= message.getMessagesPerPeriod().intValue()) && (message.getNextAllowedShow().after(now)))
          {
            if (getLogLevel() <= 3) {
              Log.d("etpushSDK@ETPush", "fence or proximity message " + message.getId() + " hit its messagesPerPeriod Limit, not showing.");
            }
          }
          else if (message.getNextAllowedShow().after(now))
          {
            if (getLogLevel() <= 3) {
              Log.d("etpushSDK@ETPush", "fence or proximity message " + message.getId() + " hit before nextAllowedShow, not showing.");
            }
          }
          else
          {
            if (2 == transitionType)
            {
              if (message.getMinTripped().intValue() > region.getExitCount().intValue())
              {
                if (getLogLevel() > 3) {
                  continue;
                }
                Log.d("etpushSDK@ETPush", "fence message " + message.getId() + " hit before minTripped reached, not showing.");
              }
            }
            else if (message.getMinTripped().intValue() > region.getEntryCount().intValue())
            {
              if (getLogLevel() > 3) {
                continue;
              }
              Log.d("etpushSDK@ETPush", "fence or proximity message " + message.getId() + " hit before minTripped reached, not showing."); continue;
            }
            try
            {
              if ((ETLocationManager.locationManager().isWatchingProximity()) && (transitionType != 1) && (transitionType != 2))
              {
                if ((proximity == 0) || (proximity > message.getProximity()))
                {
                  if (getLogLevel() <= 3) {
                    Log.d("etpushSDK@ETPush", "Proximity was " + proximity + ", but message.proximity was " + message.getProximity() + ", not showing.");
                  }
                  continue;
                }
                if (message.getHasEntered().booleanValue())
                {
                  if (getLogLevel() <= 3) {
                    Log.d("etpushSDK@ETPush", "We're still inside the region and have never left, not showing.");
                  }
                  continue;
                }
              }
            }
            catch (ETException e)
            {
              if (getLogLevel() > 6) {}
            }
            continue;
            


            message.setLastShownDate(now);
            message.setShowCount(Integer.valueOf(message.getShowCount().intValue() + 1));
            if ((message.getMessagesPerPeriod().intValue() >= 0) && (message.getNumberOfPeriods().intValue() >= 0) && (!message.getPeriodType().equals(Integer.valueOf(0))))
            {
              message.setPeriodShowCount(Integer.valueOf(message.getPeriodShowCount().intValue() + 1));
              if (message.getPeriodShowCount().intValue() >= message.getMessagesPerPeriod().intValue())
              {
                long timeIntervalToAdd = 0L;
                switch (message.getPeriodType().intValue())
                {
                case 5: 
                  timeIntervalToAdd = message.getNumberOfPeriods().intValue() * 3600000L;
                  break;
                case 4: 
                  timeIntervalToAdd = message.getNumberOfPeriods().intValue() * 86400000L;
                  break;
                case 3: 
                  timeIntervalToAdd = message.getNumberOfPeriods().intValue() * 604800000L;
                  break;
                case 2: 
                  timeIntervalToAdd = message.getNumberOfPeriods().intValue() * 2592000000L;
                  break;
                case 1: 
                  timeIntervalToAdd = message.getNumberOfPeriods().intValue() * 31536000000L;
                }
                message.setNextAllowedShow(new Date(now.getTime() + timeIntervalToAdd));
                if (!message.getIsRollingPeriod().booleanValue())
                {
                  Calendar brokenDate;
                  (brokenDate = Calendar.getInstance()).setTimeInMillis(message.getNextAllowedShow().getTime());
                  brokenDate.set(14, 0);
                  brokenDate.set(13, 0);
                  switch (message.getPeriodType().intValue())
                  {
                  case 5: 
                    brokenDate.set(12, 0);
                    break;
                  case 4: 
                    brokenDate.set(10, 0);
                    brokenDate.set(12, 0);
                    break;
                  case 3: 
                    brokenDate.set(7, 1);
                    brokenDate.set(10, 0);
                    brokenDate.set(12, 0);
                    break;
                  case 2: 
                    brokenDate.set(5, 1);
                    brokenDate.set(10, 0);
                    brokenDate.set(12, 0);
                    break;
                  case 1: 
                    brokenDate.set(2, 0);
                    brokenDate.set(5, 1);
                    brokenDate.set(10, 0);
                    brokenDate.set(12, 0);
                  }
                  message.setNextAllowedShow(brokenDate.getTime());
                }
              }
            }
            if ((message.getPeriodShowCount().intValue() >= 0) && (message.getMessagesPerPeriod().intValue() >= 0) && (message.getPeriodShowCount().intValue() > message.getMessagesPerPeriod().intValue())) {
              message.setPeriodShowCount(Integer.valueOf(0));
            }
            if ((transitionType != 1) && (transitionType != 2))
            {
              long timestamp = System.currentTimeMillis();
              if (message.getLoiterSeconds().intValue() > 0)
              {
                if (message.getEntryTime().longValue() == 0L)
                {
                  message.setEntryTime(Long.valueOf(timestamp));
                  if (getLogLevel() <= 3) {
                    Log.d("etpushSDK@ETPush", "Entered, but loiteringTime has not yet triggered.");
                  }
                  helper.getMessageDao().update(message);
                  continue;
                }
                if (timestamp <= message.getEntryTime().longValue() + message.getLoiterSeconds().intValue() * 1000L)
                {
                  if (getLogLevel() > 3) {
                    continue;
                  }
                  Log.d("etpushSDK@ETPush", "Entered, but loiteringTime has not yet triggered."); continue;
                }
              }
              message.setHasEntered(Boolean.TRUE);
            }
            else
            {
              helper.getMessageDao().update(message);
              Intent pushIntent;
              (pushIntent = new Intent("com.google.android.c2dm.intent.RECEIVE")).addCategory(applicationContext.getPackageName());
              if ((message.getOpenDirect() != null) && (message.getOpenDirect().length() > 0)) {
                pushIntent.putExtra("_od", message.getOpenDirect());
              }
              if ((message.getContentType() != null) && (Message.CONTENT_TYPE_PAGE.equals(message.getContentType())) && (message.getUrl() != null) && (message.getUrl().length() > 0)) {
                pushIntent.putExtra("_x", message.getUrl());
              }
              pushIntent.putExtra("_m", message.getId());
              if ((message.getSubject() != null) && (message.getSubject().length() >= 0)) {
                pushIntent.putExtra("alert", message.getSubject());
              }
              if ((message.getSound() != null) && (message.getSound().length() > 0)) {
                pushIntent.putExtra("sound", message.getSound());
              }
              if ((message.getKeys() != null) && (message.getKeys().size() > 0)) {
                for (Attribute attribute : message.getKeys()) {
                  pushIntent.putExtra(attribute.getKey(), attribute.getValue());
                }
              }
              if ((message.getCustom() != null) && (message.getCustom().length() > 0)) {
                pushIntent.putExtra("custom", message.getCustom());
              }
              pushIntent.putExtra("transitionType", transitionType);
              pushIntent.putExtra("regionId", regionId);
              
              displayedMessageIds.add(message.getId());
              if (getLogLevel() <= 3) {
                Log.d("etpushSDK@ETPush", "Sending broadcast Intent to display fence/proximity message: " + message.getId());
              }
              applicationContext.sendBroadcast(pushIntent);
            }
          }
        }
      }
      if ((Config.isAnalyticsActive()) && (displayedMessageIds.size() > 0)) {
        ETAnalytics.engine().logFenceOrProximityMessageDisplayed(regionId, transitionType, proximity, displayedMessageIds);
      }
    }
    catch (java.sql.SQLException e)
    {
      if (getLogLevel() <= 6) {
        Log.e("etpushSDK@ETPush", e.getMessage(), e);
      }
    }
    catch (Throwable e)
    {
      if (getLogLevel() <= 6) {
        Log.e("etpushSDK@ETPush", e.getMessage(), e);
      }
    }
    finally
    {
      (transitionType = new Handler(applicationContext.getMainLooper())).postDelayed(new Runnable()
      {
        public void run()
        {
          if ((helper != null) && (helper.isOpen())) {
            helper.close();
          }
        }
      }, 10000L);
    }
  }
  
  private class PauseWaitTask
    extends AsyncTask<Void, Void, Integer>
  {
    private PauseWaitTask() {}
    
    protected Integer doInBackground(Void... params)
    {
      try
      {
        if (ETPush.getLogLevel() <= 3) {
          Log.d("etpushSDK@ETPush", "started pauseWaitTask");
        }
        Thread.sleep(2000L);
        
        ETPush.access$302(null);
        
        helper = ETSqliteOpenHelper.getHelper(ETPush.applicationContext);
        try
        {
          helper.getWritableDatabase().execSQL("VACUUM");
          if (ETPush.getLogLevel() <= 3) {
            Log.d("etpushSDK@ETPush", "SQLite VACUUM complete");
          }
        }
        catch (android.database.SQLException sqlException)
        {
          Handler localHandler1;
          if (ETPush.getLogLevel() <= 6) {
            Log.e("etpushSDK@ETPush", ((android.database.SQLException)sqlException).getMessage(), (Throwable)sqlException);
          }
        }
        finally
        {
          Handler localHandler2;
          (localHandler2 = new Handler(ETPush.applicationContext.getMainLooper())).postDelayed(new Runnable()
          {
            public void run()
            {
              if ((helper != null) && (helper.isOpen())) {
                helper.close();
              }
            }
          }, 10000L);
        }
        EventBus.getDefault().postSticky(new BackgroundEvent(true));
      }
      catch (InterruptedException localInterruptedException)
      {
        if (ETPush.getLogLevel() <= 3) {
          Log.d("etpushSDK@ETPush", "pauseWaitTask interrupted");
        }
      }
      finally
      {
        if (ETPush.wakeLock.isHeld()) {
          ETPush.wakeLock.release();
        }
      }
      if (ETPush.getLogLevel() <= 3) {
        Log.d("etpushSDK@ETPush", "ended pauseWaitTask");
      }
      return Integer.valueOf(0);
    }
  }
}
}