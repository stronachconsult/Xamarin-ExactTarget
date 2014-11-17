using android.app.IntentService;
using android.content.Context;
using android.content.Intent;
using android.net.ConnectivityManager;
using android.net.NetworkInfo;
using android.os.SystemClock;
using android.support.v4.content.WakefulBroadcastReceiver;
using android.util.Base64;
using android.util.Log;
using com.exacttarget.etpushsdk.event.GeofenceResponseEvent;
using com.exacttarget.etpushsdk.util.EventBus;
using com.exacttarget.etpushsdk.util.JSONUtil;
using java.lang.reflect.Method;
using java.util.ArrayList;
using java.util.List;
using org.apache.http.HttpEntity;
using org.apache.http.HttpResponse;
using org.apache.http.StatusLine;
using org.apache.http.client.HttpClient;
using org.apache.http.client.methods.HttpGet;
using org.apache.http.client.methods.HttpPost;
using org.apache.http.client.methods.HttpPut;
using org.apache.http.client.methods.HttpUriRequest;
using org.apache.http.entity.StringEntity;
using org.apache.http.impl.client.DefaultHttpClient;
using org.apache.http.util.EntityUtils;

 namespace ExactTarget.ETPushSdk {
 
 public class ETSendDataIntentService : IntentService
 {
   private static final String TAG = "etpushsdk@ETSendDataIntentService";
   public static final String PARAM_DATABASE_ID = "param_database_id";
   public static final String PARAM_DATABASE_IDS = "param_database_ids";
   public static final String PARAM_HTTP_METHOD = "param_http_method";
   public static final String PARAM_HTTP_URL = "param_http_url";
   public static final String PARAM_HTTP_RESPONSE_TYPE = "param_http_response_type";
   public static final String PARAM_DATA_JSON = "param_data_json";
   public static final String PARAM_BASIC_USER = "param_basic_user";
   public static final String PARAM_BASIC_PASS = "param_basic_pass";
   public static final String PARAM_AUTH_TOKEN = "param_auth_token";
   
   public ETSendDataIntentService()
   {
     super("ETSendDataIntentService");
   }
   
   protected void onHandleIntent(Intent intent)
   {
     if (ETPush.getLogLevel() <= 3) {
       Log.d("etpushsdk@ETSendDataIntentService", "onHandleIntent()");
     }
     Integer databaseId = Integer.valueOf(intent.getIntExtra("param_database_id", -1));
     
 
     List<Integer> databaseIds = (List)intent.getSerializableExtra("param_database_ids");
     
     String httpMethod = intent.getStringExtra("param_http_method");
     String httpUrl = intent.getStringExtra("param_http_url");
     String httpResponseType = intent.getStringExtra("param_http_response_type");
     String dataJson = intent.getStringExtra("param_data_json");
     String basicUser = intent.getStringExtra("param_basic_user");
     String basicPass = intent.getStringExtra("param_basic_pass");
     String authToken = intent.getStringExtra("param_auth_token");
     Context applicationContext = getApplicationContext();
     if (isOnline(applicationContext)) {
       sendUpdate(applicationContext, httpMethod, httpUrl, httpResponseType, basicUser, basicPass, authToken, databaseId, databaseIds, dataJson);
     } else if (ETPush.getLogLevel() <= 5) {
       Log.w("etpushsdk@ETSendDataIntentService", "SendUpdate: Network not available");
     }
     WakefulBroadcastReceiver.completeWakefulIntent(intent);
   }
   
   private boolean isOnline(Context c)
   {
     boolean isConnected = false;
     NetworkInfo netInfo;
     if (((netInfo = (c = (ConnectivityManager)c.getSystemService("connectivity")).getActiveNetworkInfo()) != null) && (netInfo.isConnected())) {
       isConnected = true;
     }
     return isConnected;
   }
   
   private void sendUpdate(Context applicationContext, String httpMethod, String in_httpUrl, String httpResponseType, String basicUser, String basicPass, String authToken, Integer databaseId, List<Integer> databaseIds, String httpData)
   {
     try
     {
       ;
       ;
       ;
       ;
       ;
       ;
       ;
       ;
       if (ETPush.getLogLevel() <= 3) {
         Log.d("etpushsdk@ETSendDataIntentService", "Sending data...");
       }
       httpUrl = in_httpUrl.replaceAll("\\{et_app_id\\}", Config.getEtAppId()).replaceAll("\\{access_token\\}", Config.getAccessToken());
       
 
 
       HttpClient httpClient = new DefaultHttpClient();
       if ("GET".equals(httpMethod))
       {
         httpUrl = new HttpGet(httpUrl);
       }
       else if ("POST".equals(httpMethod))
       {
         ((HttpPost)(etRequest = new HttpPost(etRequest))).setEntity(new StringEntity(httpData));
         etRequest.setHeader("Content-type", "application/json");
       }
       else if ("PUT".equals(httpMethod))
       {
         ((HttpPut)(etRequest = new HttpPut(etRequest))).setEntity(new StringEntity(httpData));
         etRequest.setHeader("Content-type", "application/json");
       }
       else
       {
         throw new ETException("Invalid Request Method: " + httpMethod + ", only GET, POST, PUT supported.");
       }
       if ((basicUser != null) && (basicPass != null) && (!basicUser.isEmpty()) && (!basicPass.isEmpty()))
       {
         String creds = "Basic " + Base64.encodeToString(new String(new StringBuilder().append(basicUser).append(':').append(basicPass).toString()).getBytes(), 2);
         
         etRequest.setHeader("Authorization", creds);
       }
       else if ((authToken != null) && (!authToken.isEmpty()))
       {
         String creds = "Token token=\"" + authToken + "\"";
         
         etRequest.setHeader("Authorization", creds);
       }
       etRequest.setHeader("Accept", "application/json");
       etRequest.setHeader("User-Agent", generateUserAgent());
       etRequest.setHeader("X-ET-TOKEN", Config.getAccessToken());
       if (ETPush.getLogLevel() <= 3)
       {
         Log.d("etpushsdk@ETSendDataIntentService", "Request Url: " + in_httpUrl);
         Log.d("etpushsdk@ETSendDataIntentService", "Request data: " + httpData);
       }
       long start = SystemClock.elapsedRealtime();
       etPostResponse = httpClient.execute(etRequest);
       if (ETPush.getLogLevel() <= 3) {
         Log.d("etpushsdk@ETSendDataIntentService", "Request took: " + (SystemClock.elapsedRealtime() - start) + "ms");
       }
       if (((sCode = etPostResponse.getStatusLine().getStatusCode()) >= 200) && (sCode <= 299))
       {
         if (ETPush.getLogLevel() <= 3) {
           Log.d("etpushsdk@ETSendDataIntentService", "Success with StatusCode: " + String.valueOf(sCode));
         }
       }
       else if ((sCode >= 400) && (sCode <= 499))
       {
         if (ETPush.getLogLevel() <= 5)
         {
           Log.w("etpushsdk@ETSendDataIntentService", "Warning with StatusCode: " + String.valueOf(sCode));
           if (sCode == 402) {
             Log.w("etpushsdk@ETSendDataIntentService", "You are attempting to use a feature that is not enabled in your account. If you believe this is incorrect, please contact Global Support.");
           } else {
             Log.w("etpushsdk@ETSendDataIntentService", "A client error occurred while communicating with ExactTarget. Please verify that you have everything configured correctly.");
           }
         }
       }
       else if (ETPush.getLogLevel() <= 6) {
         Log.e("etpushsdk@ETSendDataIntentService", "Error with StatusCode: " + String.valueOf(sCode));
       }
       etResponseEntity = etPostResponse.getEntity();
       String jsonResponse = null;
       if (etResponseEntity != null)
       {
         jsonResponse = EntityUtils.toString(etResponseEntity);
         if ((sCode >= 200) && (sCode <= 299))
         {
           if (ETPush.getLogLevel() <= 3) {
             Log.d("etpushsdk@ETSendDataIntentService", "Success Response: " + jsonResponse);
           }
         }
         else if ((sCode >= 400) && (sCode <= 499))
         {
           if (ETPush.getLogLevel() <= 5) {
             Log.w("etpushsdk@ETSendDataIntentService", "Warning Response: " + jsonResponse);
           }
         }
         else
         {
           if (ETPush.getLogLevel() <= 6) {
             Log.e("etpushsdk@ETSendDataIntentService", "Error Response: " + jsonResponse);
           }
           if (!httpResponseType.equals(GeofenceResponseEvent.class.getName())) {
             databaseId = Integer.valueOf(0);
           }
         }
       }
       if ((httpResponseType != null) && (!httpResponseType.isEmpty()))
       {
         if ((responseObject = JSONUtil.jsonToObject(jsonResponse, Class.forName(httpResponseType))) == null)
         {
           responseObject = Class.forName(httpResponseType).newInstance();
           if ((databaseIds != null) && (!databaseIds.isEmpty())) {
             if ((setDatabaseIds = responseObject.getClass().getMethod("setDatabaseIds", new Class[] { List.class })) != null) {
               setDatabaseIds.invoke(responseObject, new Object[] { databaseIds });
             }
           }
         }
         Method setId = null;
         if ((databaseIds != null) && (!databaseIds.isEmpty()))
         {
           if (!((ArrayList)responseObject).isEmpty()) {
             for (int i = 0; i < databaseIds.size(); i++)
             {
               Object responseObjectItem = ((ArrayList)responseObject).get(i);
               if (setId == null) {
                 setId = responseObjectItem.getClass().getMethod("setId", new Class[] { Integer.class });
               }
               setId.invoke(responseObjectItem, new Object[] { databaseIds.get(i) });
             }
           }
         }
         else if (databaseId.intValue() > 0) {
           (setId = responseObject.getClass().getMethod("setId", new Class[] { Integer.class })).invoke(responseObject, new Object[] { databaseId });
         }
         EventBus.getDefault().post(responseObject);
       }
       if (ETPush.getLogLevel() <= 3) {
         Log.d("etpushsdk@ETSendDataIntentService", "Sending data done.");
       }
       return;
     }
     catch (Throwable e)
     {
       if (ETPush.getLogLevel() <= 6) {
         Log.e("etpushsdk@ETSendDataIntentService", e.getMessage(), e);
       }
     }
   }
   
   private String generateUserAgent()
   {
     return "ETPushSDK/3.3.0 (Android)";
   }
 }

}