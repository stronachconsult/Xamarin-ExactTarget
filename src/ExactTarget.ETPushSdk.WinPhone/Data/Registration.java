/*   1:    */ package com.exacttarget.etpushsdk.data;
/*   2:    */ 
/*   3:    */ import android.content.Context;
/*   4:    */ import android.content.SharedPreferences;
/*   5:    */ import android.content.SharedPreferences.Editor;
/*   6:    */ import android.content.pm.ApplicationInfo;
/*   7:    */ import android.content.pm.PackageInfo;
/*   8:    */ import android.content.pm.PackageManager;
/*   9:    */ import android.content.pm.PackageManager.NameNotFoundException;
/*  10:    */ import android.os.Build;
/*  11:    */ import android.os.Build.VERSION;
/*  12:    */ import android.util.Log;
/*  13:    */ import com.exacttarget.etpushsdk.Config;
/*  14:    */ import com.exacttarget.etpushsdk.ETPush;
/*  15:    */ import com.exacttarget.etpushsdk.util.ETAmazonDeviceMessagingUtil;
/*  16:    */ import com.fasterxml.jackson.annotation.JsonIgnore;
/*  17:    */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*  18:    */ import com.fasterxml.jackson.annotation.JsonInclude;
/*  19:    */ import com.fasterxml.jackson.annotation.JsonInclude.Include;
/*  20:    */ import com.fasterxml.jackson.annotation.JsonProperty;
/*  21:    */ import com.j256.ormlite.field.DataType;
/*  22:    */ import com.j256.ormlite.field.DatabaseField;
/*  23:    */ import com.j256.ormlite.table.DatabaseTable;
/*  24:    */ import java.io.Serializable;
/*  25:    */ import java.util.ArrayList;
/*  26:    */ import java.util.Date;
/*  27:    */ import java.util.HashSet;
/*  28:    */ import java.util.TimeZone;
/*  29:    */ 
/*  30:    */ @DatabaseTable(tableName="registration")
/*  31:    */ @JsonIgnoreProperties(ignoreUnknown=false)
/*  32:    */ @JsonInclude(JsonInclude.Include.NON_NULL)
/*  33:    */ public class Registration
/*  34:    */   extends DeviceData
/*  35:    */   implements Serializable
/*  36:    */ {
/*  37:    */   private static final String TAG = "etpushsdk@Registration";
/*  38:    */   public static final String COLUMN_ID = "id";
/*  39:    */   public static final String COLUMN_LAST_SENT = "last_sent";
/*  40:    */   public static final String COLUMN_HWID = "hwid";
/*  41:    */   public static final String COLUMN_GCM_SENDER_ID = "gcm_sender_id";
/*  42:    */   private static final long serialVersionUID = 1L;
/*  43:    */   @DatabaseField(generatedId=true)
/*  44:    */   @JsonIgnore
/*  45:    */   private Integer id;
/*  46:    */   @DatabaseField(columnName="platform")
/*  47:    */   @JsonProperty("platform")
/*  48:    */   private String platform;
/*  49:    */   @DatabaseField(columnName="device_id")
/*  50:    */   @JsonProperty("deviceID")
/*  51:    */   private String deviceId;
/*  52:    */   @DatabaseField(columnName="device_token")
/*  53:    */   @JsonProperty("device_Token")
/*  54:    */   private String deviceToken;
/*  55:    */   @DatabaseField(columnName="subscriber_key")
/*  56:    */   @JsonProperty("subscriberKey")
/*  57:    */   private String subscriberKey;
/*  58:    */   @DatabaseField(columnName="et_app_id")
/*  59:    */   @JsonProperty("etAppId")
/*  60:    */   private String etAppId;
/*  61:    */   @DatabaseField(columnName="email")
/*  62:    */   @JsonProperty("email")
/*  63:    */   private String email;
/*  64:    */   @DatabaseField(columnName="badge")
/*  65:    */   @JsonProperty("badge")
/*  66:    */   private Integer badge;
/*  67:    */   @DatabaseField(columnName="timezone")
/*  68:    */   @JsonProperty("timeZone")
/*  69:    */   private Integer timeZone;
/*  70:    */   @DatabaseField(columnName="dst")
/*  71:    */   @JsonProperty("dST")
/*  72:    */   private Boolean dst;
/*  73:    */   @DatabaseField(columnName="tags", dataType=DataType.SERIALIZABLE)
/*  74:    */   @JsonProperty("tags")
/*  75:    */   private HashSet<String> tags;
/*  76:    */   @DatabaseField(columnName="attributes", dataType=DataType.SERIALIZABLE)
/*  77:    */   @JsonProperty("attributes")
/*  78:    */   private ArrayList<Attribute> attributes;
/*  79:    */   @DatabaseField(columnName="platform_version")
/*  80:    */   @JsonProperty("platform_Version")
/*  81:    */   private String platformVersion;
/*  82:    */   @DatabaseField(columnName="push_enabled")
/*  83:    */   @JsonProperty("push_Enabled")
/*  84:    */   private Boolean pushEnabled;
/*  85:    */   @DatabaseField(columnName="location_enabled")
/*  86:    */   @JsonProperty("location_Enabled")
/*  87:    */   private Boolean locationEnabled;
/*  88:    */   @DatabaseField(columnName="last_sent")
/*  89:    */   @JsonIgnore
/*  90:113 */   private Long lastSent = Long.valueOf(0L);
/*  91:    */   @DatabaseField(columnName="hwid")
/*  92:    */   @JsonProperty("hwid")
/*  93:    */   private String hwid;
/*  94:    */   @DatabaseField(columnName="gcm_sender_id")
/*  95:    */   @JsonProperty("gcmSenderId")
/*  96:    */   private String gcmSenderId;
/*  97:    */   @JsonIgnore
/*  98:    */   private static final String ET_REGISTRATION_CACHE = "et_registration_cache";
/*  99:    */   @JsonIgnore
/* 100:    */   private static final String ET_TAGS_CACHE = "et_tags_cache";
/* 101:    */   @JsonIgnore
/* 102:    */   private static final String ET_ATTRIBUTES_CACHE = "et_attributes_cache";
/* 103:    */   @JsonIgnore
/* 104:    */   private static final String ET_SUBSCRIBER_CACHE = "et_subscriber_cache";
/* 105:    */   @JsonIgnore
/* 106:    */   private static final String ET_SEPARATOR = "^|^";
/* 107:    */   @JsonIgnore
/* 108:    */   private static final String ET_SPLITTER = "\\^\\|\\^";
/* 109:    */   @DatabaseField(persisted=false)
/* 110:    */   @JsonIgnore
/* 111:143 */   private SharedPreferences prefs = null;
/* 112:    */   @JsonIgnore
/* 113:147 */   private Context applicationContext = null;
/* 114:    */   
/* 115:    */   public Registration() {}
/* 116:    */   
/* 117:    */   public Registration(Context context)
/* 118:    */   {
/* 119:156 */     this.applicationContext = context;
/* 120:157 */     this.prefs = context.getSharedPreferences("ETPush", 0);
/* 121:158 */     this.deviceId = uniqueDeviceIdentifier(context);
/* 122:159 */     this.timeZone = Integer.valueOf(TimeZone.getDefault().getRawOffset() / 1000);
/* 123:160 */     this.dst = Boolean.valueOf(TimeZone.getDefault().inDaylightTime(new Date()));
/* 124:    */     
/* 125:162 */     deserializeTags();
/* 126:163 */     if (ETAmazonDeviceMessagingUtil.isAmazonDevice())
/* 127:    */     {
/* 128:164 */       this.platform = "Amazon";
/* 129:165 */       this.tags.add("Amazon");
/* 130:    */     }
/* 131:    */     else
/* 132:    */     {
/* 133:168 */       this.platform = "Android";
/* 134:169 */       this.tags.add("Android");
/* 135:    */     }
/* 136:172 */     this.platformVersion = Build.VERSION.RELEASE;
/* 137:174 */     if (ETPush.getLogLevel() <= 3) {
/* 138:175 */       Log.d("etpushsdk@Registration", "Platform: \"" + this.platform + "\", platformVersion: \"" + this.platformVersion + "\"");
/* 139:    */     }
/* 140:178 */     this.hwid = (Build.MANUFACTURER + ' ' + Build.MODEL);
/* 141:179 */     if (ETPush.getLogLevel() <= 3) {
/* 142:180 */       Log.i("etpushsdk@Registration", "hwid: " + this.hwid);
/* 143:    */     }
/* 144:    */     try
/* 145:    */     {
/* 146:187 */       if (((context = (context = context.getPackageManager().getPackageInfo(context.getPackageName(), 0)).applicationInfo.flags) & 0x2) != 0) {
/* 147:188 */         this.tags.add("Debug");
/* 148:    */       }
/* 149:    */     }
/* 150:    */     catch (PackageManager.NameNotFoundException e)
/* 151:    */     {
/* 152:192 */       if (ETPush.getLogLevel() <= 5) {
/* 153:193 */         Log.w("etpushsdk@Registration", e.getMessage());
/* 154:    */       }
/* 155:    */     }
/* 156:198 */     this.tags.add("ALL");
/* 157:    */     
/* 158:200 */     serializeTags();
/* 159:    */     
/* 160:202 */     deserializeAttributes();
/* 161:203 */     addAttribute(new Attribute("_ETSDKVersion", "3.3.0"));
/* 162:    */     
/* 163:205 */     this.subscriberKey = ((String)Config.getETSharedPref(this.applicationContext, this.applicationContext.getSharedPreferences("et_registration_cache", 0), "et_subscriber_cache", null));
/* 164:    */   }
/* 165:    */   
/* 166:    */   public void addAttribute(Attribute attribute)
/* 167:    */   {
/* 168:209 */     if (this.attributes.contains(attribute)) {
/* 169:210 */       this.attributes.remove(attribute);
/* 170:    */     }
/* 171:212 */     this.attributes.add(attribute);
/* 172:213 */     serializeAttributes();
/* 173:    */   }
/* 174:    */   
/* 175:    */   public void removeAttribute(Attribute attribute)
/* 176:    */   {
/* 177:217 */     if (this.attributes.contains(attribute)) {
/* 178:218 */       this.attributes.remove(attribute);
/* 179:    */     }
/* 180:220 */     serializeAttributes();
/* 181:    */   }
/* 182:    */   
/* 183:    */   private void serializeAttributes()
/* 184:    */   {
/* 185:224 */     if (this.prefs != null)
/* 186:    */     {
/* 187:225 */       StringBuilder out = new StringBuilder();
/* 188:226 */       for (Attribute attribute : this.attributes)
/* 189:    */       {
/* 190:227 */         out.append(attribute.getKey());
/* 191:228 */         out.append("^|^");
/* 192:229 */         out.append(attribute.getValue());
/* 193:230 */         out.append("^|^");
/* 194:    */       }
/* 195:232 */       out.setLength(out.length() - 3);
/* 196:233 */       this.prefs.edit().putString("et_attributes_cache", out.toString()).commit();
/* 197:    */     }
/* 198:    */   }
/* 199:    */   
/* 200:    */   private void deserializeAttributes()
/* 201:    */   {
/* 202:238 */     if (this.prefs != null)
/* 203:    */     {
/* 204:239 */       this.attributes = new ArrayList();
/* 205:    */       String cacheString;
/* 206:241 */       if ((cacheString = (String)Config.getETSharedPref(this.applicationContext, this.applicationContext.getSharedPreferences("et_registration_cache", 0), "et_attributes_cache", null)) == null) {
/* 207:242 */         return;
/* 208:    */       }
/* 209:245 */       String[] tokens = cacheString.split("\\^\\|\\^");
/* 210:246 */       for (int i = 0; i < tokens.length; i += 2)
/* 211:    */       {
/* 212:247 */         while ((tokens[i] == null) || (tokens[i].isEmpty())) {
/* 213:248 */           i++;
/* 214:    */         }
/* 215:250 */         this.attributes.add(new Attribute(tokens[i], tokens[(i + 1)]));
/* 216:    */       }
/* 217:    */     }
/* 218:    */   }
/* 219:    */   
/* 220:    */   public void addTag(String tag)
/* 221:    */   {
/* 222:256 */     this.tags.add(tag);
/* 223:257 */     serializeTags();
/* 224:    */   }
/* 225:    */   
/* 226:    */   public void removeTag(String tag)
/* 227:    */   {
/* 228:261 */     this.tags.remove(tag);
/* 229:262 */     serializeTags();
/* 230:    */   }
/* 231:    */   
/* 232:    */   private void serializeTags()
/* 233:    */   {
/* 234:266 */     if (this.prefs != null)
/* 235:    */     {
/* 236:267 */       StringBuilder out = new StringBuilder();
/* 237:268 */       for (String tag : this.tags)
/* 238:    */       {
/* 239:269 */         out.append(tag);
/* 240:270 */         out.append("^|^");
/* 241:    */       }
/* 242:272 */       out.setLength(out.length() - 3);
/* 243:273 */       this.prefs.edit().putString("et_tags_cache", out.toString()).commit();
/* 244:    */     }
/* 245:    */   }
/* 246:    */   
/* 247:    */   private void deserializeTags()
/* 248:    */   {
/* 249:278 */     if (this.prefs != null)
/* 250:    */     {
/* 251:279 */       this.tags = new HashSet();
/* 252:    */       String cacheString;
/* 253:281 */       if ((cacheString = (String)Config.getETSharedPref(this.applicationContext, this.applicationContext.getSharedPreferences("et_registration_cache", 0), "et_tags_cache", null)) == null) {
/* 254:282 */         return;
/* 255:    */       }
/* 256:285 */       String[] tokens = cacheString.split("\\^\\|\\^");
/* 257:286 */       for (int i = 0; i < tokens.length; i++) {
/* 258:287 */         if ((tokens[i] != null) && (!tokens[i].isEmpty())) {
/* 259:288 */           this.tags.add(tokens[i]);
/* 260:    */         }
/* 261:    */       }
/* 262:    */     }
/* 263:    */   }
/* 264:    */   
/* 265:    */   public Integer getId()
/* 266:    */   {
/* 267:295 */     return this.id;
/* 268:    */   }
/* 269:    */   
/* 270:    */   public void setId(Integer id)
/* 271:    */   {
/* 272:299 */     this.id = id;
/* 273:    */   }
/* 274:    */   
/* 275:    */   public String getPlatform()
/* 276:    */   {
/* 277:303 */     return this.platform;
/* 278:    */   }
/* 279:    */   
/* 280:    */   public void setPlatform(String platform)
/* 281:    */   {
/* 282:307 */     this.platform = platform;
/* 283:    */   }
/* 284:    */   
/* 285:    */   public String getDeviceId()
/* 286:    */   {
/* 287:311 */     return this.deviceId;
/* 288:    */   }
/* 289:    */   
/* 290:    */   public void setDeviceId(String deviceId)
/* 291:    */   {
/* 292:315 */     this.deviceId = deviceId;
/* 293:    */   }
/* 294:    */   
/* 295:    */   public String getEtAppId()
/* 296:    */   {
/* 297:319 */     return this.etAppId;
/* 298:    */   }
/* 299:    */   
/* 300:    */   public void setEtAppId(String etAppId)
/* 301:    */   {
/* 302:323 */     this.etAppId = etAppId;
/* 303:    */   }
/* 304:    */   
/* 305:    */   public Integer getBadge()
/* 306:    */   {
/* 307:327 */     return this.badge;
/* 308:    */   }
/* 309:    */   
/* 310:    */   public void setBadge(Integer badge)
/* 311:    */   {
/* 312:331 */     this.badge = badge;
/* 313:    */   }
/* 314:    */   
/* 315:    */   public String getEmail()
/* 316:    */   {
/* 317:335 */     return this.email;
/* 318:    */   }
/* 319:    */   
/* 320:    */   public void setEmail(String email)
/* 321:    */   {
/* 322:339 */     this.email = email;
/* 323:    */   }
/* 324:    */   
/* 325:    */   public Integer getTimeZone()
/* 326:    */   {
/* 327:343 */     return this.timeZone;
/* 328:    */   }
/* 329:    */   
/* 330:    */   public void setTimeZone(Integer timeZone)
/* 331:    */   {
/* 332:347 */     this.timeZone = timeZone;
/* 333:    */   }
/* 334:    */   
/* 335:    */   public Boolean getDst()
/* 336:    */   {
/* 337:351 */     return this.dst;
/* 338:    */   }
/* 339:    */   
/* 340:    */   public void setDst(Boolean dst)
/* 341:    */   {
/* 342:355 */     this.dst = dst;
/* 343:    */   }
/* 344:    */   
/* 345:    */   public HashSet<String> getTags()
/* 346:    */   {
/* 347:359 */     return this.tags;
/* 348:    */   }
/* 349:    */   
/* 350:    */   public void setTags(HashSet<String> tags)
/* 351:    */   {
/* 352:363 */     this.tags = tags;
/* 353:    */   }
/* 354:    */   
/* 355:    */   public ArrayList<Attribute> getAttributes()
/* 356:    */   {
/* 357:367 */     return this.attributes;
/* 358:    */   }
/* 359:    */   
/* 360:    */   public void setAttributes(ArrayList<Attribute> attributes)
/* 361:    */   {
/* 362:371 */     this.attributes = attributes;
/* 363:    */   }
/* 364:    */   
/* 365:    */   public String getPlatformVersion()
/* 366:    */   {
/* 367:375 */     return this.platformVersion;
/* 368:    */   }
/* 369:    */   
/* 370:    */   public void setPlatformVersion(String platformVersion)
/* 371:    */   {
/* 372:379 */     this.platformVersion = platformVersion;
/* 373:    */   }
/* 374:    */   
/* 375:    */   public String getDeviceToken()
/* 376:    */   {
/* 377:383 */     if (this.deviceToken != null) {
/* 378:383 */       return this.deviceToken;
/* 379:    */     }
/* 380:383 */     return "";
/* 381:    */   }
/* 382:    */   
/* 383:    */   public void setDeviceToken(String deviceToken)
/* 384:    */   {
/* 385:387 */     this.deviceToken = deviceToken;
/* 386:    */   }
/* 387:    */   
/* 388:    */   public String getSubscriberKey()
/* 389:    */   {
/* 390:391 */     if ((this.subscriberKey != null) && (!this.subscriberKey.isEmpty())) {
/* 391:391 */       return this.subscriberKey;
/* 392:    */     }
/* 393:391 */     return this.deviceId;
/* 394:    */   }
/* 395:    */   
/* 396:    */   public void setSubscriberKey(String subscriberKey)
/* 397:    */   {
/* 398:395 */     this.subscriberKey = subscriberKey;
/* 399:396 */     if (this.prefs != null) {
/* 400:397 */       this.prefs.edit().putString("et_subscriber_cache", this.subscriberKey).commit();
/* 401:    */     }
/* 402:    */   }
/* 403:    */   
/* 404:    */   public Boolean getPushEnabled()
/* 405:    */   {
/* 406:402 */     return this.pushEnabled;
/* 407:    */   }
/* 408:    */   
/* 409:    */   public void setPushEnabled(Boolean pushEnabled)
/* 410:    */   {
/* 411:406 */     this.pushEnabled = pushEnabled;
/* 412:    */   }
/* 413:    */   
/* 414:    */   public Boolean getLocationEnabled()
/* 415:    */   {
/* 416:410 */     return this.locationEnabled;
/* 417:    */   }
/* 418:    */   
/* 419:    */   public void setLocationEnabled(Boolean locationEnabled)
/* 420:    */   {
/* 421:414 */     this.locationEnabled = locationEnabled;
/* 422:    */   }
/* 423:    */   
/* 424:    */   public Long getLastSent()
/* 425:    */   {
/* 426:418 */     return this.lastSent;
/* 427:    */   }
/* 428:    */   
/* 429:    */   public void setLastSent(Long lastSent)
/* 430:    */   {
/* 431:422 */     this.lastSent = lastSent;
/* 432:    */   }
/* 433:    */   
/* 434:    */   public String getHwid()
/* 435:    */   {
/* 436:426 */     return this.hwid;
/* 437:    */   }
/* 438:    */   
/* 439:    */   public void setHwid(String hwid)
/* 440:    */   {
/* 441:430 */     this.hwid = hwid;
/* 442:    */   }
/* 443:    */   
/* 444:    */   public String getGcmSenderId()
/* 445:    */   {
/* 446:434 */     return this.gcmSenderId;
/* 447:    */   }
/* 448:    */   
/* 449:    */   public void setGcmSenderId(String gcmSenderId)
/* 450:    */   {
/* 451:438 */     this.gcmSenderId = gcmSenderId;
/* 452:    */   }
/* 453:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.Registration
 * JD-Core Version:    0.7.0.1
 */