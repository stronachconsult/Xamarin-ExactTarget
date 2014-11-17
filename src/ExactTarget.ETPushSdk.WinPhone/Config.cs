using android.content.Context;
using android.content.SharedPreferences;
using android.content.SharedPreferences.Editor;
using android.content.pm.ApplicationInfo;
using android.content.pm.PackageManager;
using android.content.pm.PackageManager.NameNotFoundException;
using android.os.Bundle;
using android.util.Log;

namespace ExactTarget.ETPushSdk
{
    public class Config
    {
        private static const string TAG = "etpushSDK@Config";
        public static const string ET_KEY_RUN_ONCE = "et_key_run_once";
        public static const string ET_KEY_FOLLOW_LOCATION_CHANGES = "et_key_follow_location_changes";
        public static const string ET_SHARED_PREFS = "ETPush";
        public static const int ACTIVE_MAX_DISTANCE = 100;
        public static const long ACTIVE_MAX_TIME = 900000L;
        public static const int PASSIVE_MAX_DISTANCE = 0;
        public static const long PASSIVE_MAX_TIME = 300000L;
        public static const string ACTIVE_LOCATION_UPDATE_PROVIDER_DISABLED = "com.exacttarget.etpushsdk.active_location_update_provider_disabled";
        private static const string ET_BASE_URL = "https://consumer.exacttargetapis.com";
        protected static const string ET_REGISTRATION_URL = "https://consumer.exacttargetapis.com/device/v1/registration?access_token={access_token}";
        protected static const string ET_ANALYTICS_URL = "https://consumer.exacttargetapis.com/device/v1/event/analytic?access_token={access_token}";
        protected static const string ET_LOCATION_UPDATE_URL = "https://consumer.exacttargetapis.com/device/v1/location/{et_app_id}?access_token={access_token}";
        protected static const string ET_GEOFENCE_URL = "https://consumer.exacttargetapis.com/device/v1/location/{et_app_id}/fence/?latitude={latitude}&longitude={longitude}&deviceid={device_id}&access_token={access_token}";
        protected static const string ET_CLOUDPAGE_URL = "https://consumer.exacttargetapis.com/device/v1/{et_app_id}/message/?deviceid={device_id}&messagetype={messagetype}&contenttype={contenttype}&access_token={access_token}";
        protected static const string ET_PROXIMITY_URL = "https://consumer.exacttargetapis.com/device/v1/location/{et_app_id}/proximity/?latitude={latitude}&longitude={longitude}&deviceid={device_id}&access_token={access_token}";
        private static string etAppId;
        private static string accessToken;
        private static bool analyticsActive = false;
        private static bool locationManagerActive = false;
        private static bool cloudPagesActive = false;
        private static bool? readOnly = null;

        public static String getEtAppId()
        {
            return etAppId;
        }

        protected static void setEtAppId(String etAppId)
        {
            etAppId = etAppId;
        }

        protected static String getAccessToken()
        {
            return accessToken;
        }

        public static void setAccessToken(String accessToken)
        {
            accessToken = accessToken;
        }

        protected static boolean isAnalyticsActive()
        {
            return analyticsActive;
        }

        protected static void setAnalyticsActive(boolean analyticsActive)
        {
            analyticsActive = analyticsActive;
        }

        public static boolean isLocationManagerActive()
        {
            return locationManagerActive;
        }

        protected static void setLocationManagerActive(boolean locationManagerActive)
        {
            locationManagerActive = locationManagerActive;
        }

        protected static boolean isCloudPagesActive()
        {
            return cloudPagesActive;
        }

        protected static void setCloudPagesActive(boolean cloudPagesActive)
        {
            cloudPagesActive = cloudPagesActive;
        }

        public static Object getETSharedPref(Context context, SharedPreferences oldPrefs, String key, Object defaultValue)
  {
    ;
    ;
    ;
    Object returnValue;
    Object returnValue;
    if ((prefs = context.getSharedPreferences("ETPush", 0)).contains(key))
    {
      if ((defaultValue instanceof Boolean))
      {
        prefs = Boolean.valueOf(prefs.getBoolean(key, ((Boolean)defaultValue).booleanValue()));
      }
      else if ((defaultValue instanceof Float))
      {
        returnValue = Float.valueOf(returnValue.getFloat(key, ((Float)defaultValue).floatValue()));
      }
      else
      {
        Object returnValue;
        if ((defaultValue instanceof Integer))
        {
          returnValue = Integer.valueOf(returnValue.getInt(key, ((Integer)defaultValue).intValue()));
        }
        else
        {
          Object returnValue;
          if ((defaultValue instanceof Long)) {
            returnValue = Long.valueOf(returnValue.getLong(key, ((Long)defaultValue).longValue()));
          } else {
            returnValue = returnValue.getString(key, (String)defaultValue);
          }
        }
      }
    }
    else if (oldPrefs.contains(key))
    {
      Object returnValue;
      Object returnValue;
      if ((defaultValue instanceof Boolean))
      {
        returnValue = Boolean.valueOf(oldPrefs.getBoolean(key, ((Boolean)defaultValue).booleanValue()));
      }
      else
      {
        Object returnValue;
        if ((defaultValue instanceof Float))
        {
          returnValue = Float.valueOf(oldPrefs.getFloat(key, ((Float)defaultValue).floatValue()));
        }
        else
        {
          Object returnValue;
          if ((defaultValue instanceof Integer))
          {
            returnValue = Integer.valueOf(oldPrefs.getInt(key, ((Integer)defaultValue).intValue()));
          }
          else
          {
            Object returnValue;
            if ((defaultValue instanceof Long)) {
              returnValue = Long.valueOf(oldPrefs.getLong(key, ((Long)defaultValue).longValue()));
            } else {
              returnValue = oldPrefs.getString(key, (String)defaultValue);
            }
          }
        }
      }
      oldPrefs.edit().remove(key).apply();
    }
    else
    {
      returnValue = defaultValue;
    }
    return returnValue;
  }

        protected static boolean isReadOnly(Context context)
        {
            if (readOnly == null)
            {
                try
                {
                    readOnly = Boolean.valueOf((context = (context = context.getPackageManager().getApplicationInfo(context.getPackageName(), 128)).metaData).getBoolean("etpush_readonly", false));
                }
                catch (PackageManager.NameNotFoundException e)
                {
                    if (ETPush.getLogLevel() <= 6)
                    {
                        Log.e("etpushSDK@Config", e.getMessage(), e);
                    }
                    return false;
                }
            }
            return readOnly.booleanValue();
        }
    }
}