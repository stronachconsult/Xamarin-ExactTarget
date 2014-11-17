using android.content.Context;
using android.content.Intent;
using android.content.SharedPreferences;
using android.os.Handler;
using android.support.v4.content.WakefulBroadcastReceiver;
using android.util.Log;
using com.exacttarget.etpushsdk.data.AnalyticItem;
using com.exacttarget.etpushsdk.data.BeaconRequest;
using com.exacttarget.etpushsdk.data.CloudPagesResponse;
using com.exacttarget.etpushsdk.data.DeviceData;
using com.exacttarget.etpushsdk.data.ETSqliteOpenHelper;
using com.exacttarget.etpushsdk.data.GeofenceRequest;
using com.exacttarget.etpushsdk.data.LocationUpdate;
using com.exacttarget.etpushsdk.data.Message;
using com.exacttarget.etpushsdk.data.Registration;
using com.exacttarget.etpushsdk.event.AnalyticItemEvent;
using com.exacttarget.etpushsdk.event.BeaconResponseEvent;
using com.exacttarget.etpushsdk.event.GeofenceResponseEvent;
using com.exacttarget.etpushsdk.event.LocationUpdateEvent;
using com.exacttarget.etpushsdk.event.RegistrationEvent;
using com.exacttarget.etpushsdk.util.JSONUtil;
using com.j256.ormlite.dao.Dao;
using com.j256.ormlite.stmt.QueryBuilder;
using com.j256.ormlite.stmt.Where;
using java.io.Serializable;
using java.sql.SQLException;
using java.text.DecimalFormat;
using java.util.ArrayList;
using java.util.Iterator;
using java.util.List;

namespace ExactTarget.ETPushSdk
{

public class ETSendDataReceiver : WakefulBroadcastReceiver
{
  private static final String TAG = "etpushsdk@ETSendDataReceiver";
  public static final String SEND_TYPE_EXTRA = "et_send_type_extra";
  protected static final String SEND_TYPE_ANALYTIC_EVENTS = "et_send_type_analytic_events";
  protected static final String SEND_TYPE_REGISTRATION = "et_send_type_registration";
  protected static final String SEND_TYPE_LOCATION = "et_send_type_location";
  protected static final String SEND_TYPE_GEOFENCE_REQUEST = "et_send_type_geofence";
  protected static final String SEND_TYPE_PROXIMITY_REQUEST = "et_send_type_proximity";
  protected static final String SEND_TYPE_CLOUDPAGE_REQUEST = "et_send_type_cloudpage";
  public static final String SEND_TYPE_CUSTOM_APP_REQUEST = "et_send_type_custom_app_request";
  private static final Long FIVE_MINUTES_IN_MILLIS = Long.valueOf(300000L);
  private final DecimalFormat latLngFormat = new DecimalFormat("#.######");
  private static SharedPreferences sp = null;
  
  public void onReceive(Context context, Intent intent)
  {
    String sendType = intent.getStringExtra("et_send_type_extra");
    if (ETPush.getLogLevel() <= 3) {
      Log.d("etpushsdk@ETSendDataReceiver", "onReceive()");
    }
    Long now = Long.valueOf(System.currentTimeMillis());
    
    final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(context.getApplicationContext());
    try
    {
      if (ETPush.getLogLevel() <= 3) {
        Log.d("etpushsdk@ETSendDataReceiver", "Request: " + sendType);
      }
      Dao<AnalyticItem, Integer> analyticItemDao;
      Intent intentService;
      if (("et_send_type_analytic_events".equals(sendType)) && (!Config.isReadOnly(context)))
      {
        intent = (analyticItemDao = helper.getAnalyticItemDao()).queryBuilder().orderBy("id", true).where().eq("ready_to_send", Boolean.TRUE).and().lt("last_sent", Long.valueOf(now.longValue() - FIVE_MINUTES_IN_MILLIS.longValue())).query();
        List<AnalyticItem> analyticEventList;
        List<Integer> analyticEventIds = new ArrayList();
        for (Iterator i$ = analyticEventList.iterator(); i$.hasNext();)
        {
          AnalyticItem analyticEvent;
          (analyticEvent = (AnalyticItem)i$.next()).setLastSent(now);
          analyticItemDao.update(analyticEvent);
          analyticEventIds.add(analyticEvent.getId());
        }
        Intent intentService;
        (intentService = new Intent(context, ETSendDataIntentService.class)).putExtra("param_database_ids", (Serializable)analyticEventIds);
        intentService.putExtra("param_http_method", "POST");
        intentService.putExtra("param_http_url", "https://consumer.exacttargetapis.com/device/v1/event/analytic?access_token={access_token}");
        intentService.putExtra("param_http_response_type", AnalyticItemEvent.class.getName());
        intentService.putExtra("param_data_json", JSONUtil.objectToJson(analyticEventList));
        startWakefulService(context, intentService);
      }
      else
      {
        Dao<Registration, Integer> registrationDao;
        Registration registration;
        if (("et_send_type_registration".equals(analyticItemDao)) && (!Config.isReadOnly(context)))
        {
          if (((registration = (Registration)(registrationDao = helper.getRegistrationDao()).queryBuilder().orderBy("id", false).queryForFirst()) != null) && (registration.getLastSent().longValue() < now.longValue() - FIVE_MINUTES_IN_MILLIS.longValue()))
          {
            registration.setLastSent(now);
            registrationDao.update(registration);
            Intent intentService;
            (intentService = new Intent(context, ETSendDataIntentService.class)).putExtra("param_database_id", registration.getId());
            if (ETPush.getLogLevel() <= 3) {
              Log.d("etpushsdk@ETSendDataReceiver", "REGISTRATION ID: " + registration.getId());
            }
            intentService.putExtra("param_http_method", "POST");
            intentService.putExtra("param_http_url", "https://consumer.exacttargetapis.com/device/v1/registration?access_token={access_token}");
            intentService.putExtra("param_http_response_type", RegistrationEvent.class.getName());
            intentService.putExtra("param_data_json", JSONUtil.objectToJson(registration));
            startWakefulService(context, intentService);
          }
          else if (ETPush.getLogLevel() <= 3)
          {
            Log.d("etpushsdk@ETSendDataReceiver", "SKIP registration send.");
          }
        }
        else
        {
          Dao<LocationUpdate, Integer> locationDao;
          Iterator i$;
          if (("et_send_type_location".equals(registrationDao)) && (!Config.isReadOnly(context)))
          {
            for (i$ = (registration = (locationDao = helper.getLocationUpdateDao()).queryBuilder().orderBy("id", true).where().lt("last_sent", Long.valueOf(now.longValue() - FIVE_MINUTES_IN_MILLIS.longValue())).query()).iterator(); i$.hasNext();)
            {
              LocationUpdate locationUpdate;
              (locationUpdate = (LocationUpdate)i$.next()).setLastSent(now);
              locationDao.update(locationUpdate);
              if (ETPush.getLogLevel() <= 3) {
                Log.d("etpushsdk@ETSendDataReceiver", "Send Location Update for: " + locationUpdate.getId());
              }
              Intent intentService;
              (intentService = new Intent(context, ETSendDataIntentService.class)).putExtra("param_database_id", locationUpdate.getId());
              intentService.putExtra("param_http_method", "POST");
              intentService.putExtra("param_http_url", "https://consumer.exacttargetapis.com/device/v1/location/{et_app_id}?access_token={access_token}");
              intentService.putExtra("param_http_response_type", LocationUpdateEvent.class.getName());
              intentService.putExtra("param_data_json", JSONUtil.objectToJson(locationUpdate));
              startWakefulService(context, intentService);
            }
          }
          else
          {
            Dao<GeofenceRequest, Integer> geofenceDao;
            if ("et_send_type_geofence".equals(locationDao))
            {
              List<GeofenceRequest> geofenceRequestList;
              if (((geofenceRequestList = (geofenceDao = helper.getGeofenceRequestDao()).queryForAll()) != null) && (geofenceRequestList.size() > 0))
              {
                GeofenceRequest lastRequest = (GeofenceRequest)geofenceRequestList.get(geofenceRequestList.size() - 1);
                geofenceDao.delete(geofenceRequestList);
                
                String url = "https://consumer.exacttargetapis.com/device/v1/location/{et_app_id}/fence/?latitude={latitude}&longitude={longitude}&deviceid={device_id}&access_token={access_token}".replaceAll("\\{device_id\\}", lastRequest.getDeviceId()).replaceAll("\\{latitude\\}", this.latLngFormat.format(lastRequest.getLatitude())).replaceAll("\\{longitude\\}", this.latLngFormat.format(lastRequest.getLongitude()));
                if (Config.isReadOnly(context.getApplicationContext())) {
                  url = url + "&all=true";
                }
                Intent intentService;
                (intentService = new Intent(context, ETSendDataIntentService.class)).putExtra("param_http_method", "GET");
                intentService.putExtra("param_http_url", url);
                intentService.putExtra("param_http_response_type", GeofenceResponseEvent.class.getName());
                startWakefulService(context, intentService);
              }
            }
            else
            {
              Dao<BeaconRequest, Integer> beaconDao;
              if ("et_send_type_proximity".equals(geofenceDao))
              {
                List<BeaconRequest> beaconRequestList;
                if (((beaconRequestList = (beaconDao = helper.getBeaconRequestDao()).queryForAll()) != null) && (beaconRequestList.size() > 0))
                {
                  BeaconRequest lastRequest = (BeaconRequest)beaconRequestList.get(beaconRequestList.size() - 1);
                  beaconDao.delete(beaconRequestList);
                  
                  String url = "https://consumer.exacttargetapis.com/device/v1/location/{et_app_id}/proximity/?latitude={latitude}&longitude={longitude}&deviceid={device_id}&access_token={access_token}".replaceAll("\\{device_id\\}", lastRequest.getDeviceId()).replaceAll("\\{latitude\\}", this.latLngFormat.format(lastRequest.getLatitude())).replaceAll("\\{longitude\\}", this.latLngFormat.format(lastRequest.getLongitude()));
                  if (Config.isReadOnly(context.getApplicationContext())) {
                    url = url + "&all=true";
                  }
                  Intent intentService;
                  (intentService = new Intent(context, ETSendDataIntentService.class)).putExtra("param_http_method", "GET");
                  intentService.putExtra("param_http_url", url);
                  intentService.putExtra("param_http_response_type", BeaconResponseEvent.class.getName());
                  startWakefulService(context, intentService);
                }
              }
              else
              {
                String url;
                Intent intentService;
                if ("et_send_type_cloudpage".equals(beaconDao))
                {
                  if (sp == null) {
                    sp = context.getSharedPreferences("ETPush", 0);
                  }
                  url = "https://consumer.exacttargetapis.com/device/v1/{et_app_id}/message/?deviceid={device_id}&messagetype={messagetype}&contenttype={contenttype}&access_token={access_token}".replaceAll("\\{device_id\\}", new DeviceData().uniqueDeviceIdentifier(context)).replaceAll("\\{messagetype\\}", Message.MESSAGE_TYPE_BASIC.toString()).replaceAll("\\{contenttype\\}", Message.CONTENT_TYPE_PAGE.toString());
                  



                  (
                    intentService = new Intent(context, ETSendDataIntentService.class)).putExtra("param_http_method", "GET");
                  intentService.putExtra("param_http_url", url);
                  intentService.putExtra("param_http_response_type", CloudPagesResponse.class.getName());
                  startWakefulService(context, intentService);
                }
                else if ("et_send_type_custom_app_request".equals(url))
                {
                  (intentService = new Intent(context, ETSendDataIntentService.class)).putExtra("param_http_method", intentService.getStringExtra("param_http_method"));
                  intentService.putExtra("param_http_url", intentService.getStringExtra("param_http_url"));
                  intentService.putExtra("param_http_response_type", intentService.getStringExtra("param_http_response_type"));
                  if (((json = intentService.getStringExtra("param_data_json")) != null) && (json.length() > 0)) {
                    intentService.putExtra("param_data_json", json);
                  }
                  startWakefulService(context, intentService);
                }
                else if ((!Config.isReadOnly(context)) && 
                  (ETPush.getLogLevel() <= 6))
                {
                  Log.e("etpushsdk@ETSendDataReceiver", "Unknown SEND_TYPE for ETSendDataReceiver: " + intentService);
                }
              }
            }
          }
        }
      }
    }
    catch (SQLException e)
    {
      if (ETPush.getLogLevel() <= 6) {
        Log.e("etpushsdk@ETSendDataReceiver", e.getMessage(), e);
      }
    }
    finally
    {
      (context = new Handler(context.getMainLooper())).postDelayed(new Runnable()
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
}