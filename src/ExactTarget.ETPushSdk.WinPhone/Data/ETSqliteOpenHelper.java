/*   1:    */ package com.exacttarget.etpushsdk.data;
/*   2:    */ 
/*   3:    */ import android.content.Context;
/*   4:    */ import android.database.sqlite.SQLiteDatabase;
/*   5:    */ import android.util.Log;
/*   6:    */ import com.exacttarget.etpushsdk.ETPush;
/*   7:    */ import com.j256.ormlite.android.apptools.OrmLiteSqliteOpenHelper;
/*   8:    */ import com.j256.ormlite.dao.Dao;
/*   9:    */ import com.j256.ormlite.support.ConnectionSource;
/*  10:    */ import com.j256.ormlite.table.TableUtils;
/*  11:    */ import java.sql.SQLException;
/*  12:    */ import java.util.concurrent.atomic.AtomicInteger;
/*  13:    */ 
/*  14:    */ public class ETSqliteOpenHelper
/*  15:    */   extends OrmLiteSqliteOpenHelper
/*  16:    */ {
/*  17:    */   private static final String TAG = "etpushsdk@ETSqliteOpenHelper";
/*  18:    */   private static final String DATABASE_NAME = "etdb.db";
/*  19:    */   private static final int DATABASE_VERSION = 3;
/*  20: 33 */   private static ETSqliteOpenHelper helper = null;
/*  21: 34 */   private static final AtomicInteger usageCounter = new AtomicInteger(0);
/*  22:    */   private static Dao<BeaconRequest, Integer> beaconRequestDao;
/*  23:    */   private static Dao<GeofenceRequest, Integer> geofenceRequestDao;
/*  24:    */   private static Dao<LocationUpdate, Integer> locationUpdateDao;
/*  25:    */   private static Dao<Message, String> messageDao;
/*  26:    */   private static Dao<RegionMessage, Integer> regionMessageDao;
/*  27:    */   private static Dao<Region, String> regionDao;
/*  28:    */   private static Dao<Registration, Integer> registrationDao;
/*  29:    */   private static Dao<AnalyticItem, Integer> analyticItemDao;
/*  30:    */   
/*  31:    */   private ETSqliteOpenHelper(Context context)
/*  32:    */   {
/*  33: 49 */     super(context, "etdb.db", null, 3, context.getApplicationContext().getClassLoader().getResourceAsStream("com/exacttarget/etpushsdk/data/ormlite_config.txt"));
/*  34:    */   }
/*  35:    */   
/*  36:    */   public void onCreate(SQLiteDatabase database, ConnectionSource connectionSource)
/*  37:    */   {
/*  38:    */     try
/*  39:    */     {
/*  40:    */       ;
/*  41: 55 */       if (ETPush.getLogLevel() <= 3) {
/*  42: 56 */         Log.d("etpushsdk@ETSqliteOpenHelper", "onCreate");
/*  43:    */       }
/*  44: 58 */       TableUtils.createTable(connectionSource, BeaconRequest.class);
/*  45: 59 */       TableUtils.createTable(connectionSource, GeofenceRequest.class);
/*  46: 60 */       TableUtils.createTable(connectionSource, LocationUpdate.class);
/*  47: 61 */       TableUtils.createTable(connectionSource, Message.class);
/*  48: 62 */       TableUtils.createTable(connectionSource, Region.class);
/*  49: 63 */       TableUtils.createTable(connectionSource, RegionMessage.class);
/*  50: 64 */       TableUtils.createTable(connectionSource, Registration.class);
/*  51: 65 */       TableUtils.createTable(connectionSource, AnalyticItem.class);
/*  52:    */       
/*  53:    */ 
/*  54:    */ 
/*  55:    */ 
/*  56:    */ 
/*  57:    */ 
/*  58: 72 */       return;
/*  59:    */     }
/*  60:    */     catch (SQLException e)
/*  61:    */     {
/*  62: 68 */       if (ETPush.getLogLevel() <= 6) {
/*  63: 69 */         Log.e("etpushsdk@ETSqliteOpenHelper", "Can't create database", e);
/*  64:    */       }
/*  65: 71 */       throw new RuntimeException(e);
/*  66:    */     }
/*  67:    */   }
/*  68:    */   
/*  69:    */   public static ETSqliteOpenHelper getHelper(Context context)
/*  70:    */   {
/*  71: 80 */     synchronized ("etpushsdk@ETSqliteOpenHelper")
/*  72:    */     {
/*  73: 81 */       if (helper == null) {
/*  74: 82 */         helper = new ETSqliteOpenHelper(context);
/*  75:    */       }
/*  76: 84 */       usageCounter.incrementAndGet();
/*  77:    */     }
/*  78: 86 */     return helper;
/*  79:    */   }
/*  80:    */   
/*  81:    */   public void close()
/*  82:    */   {
/*  83: 96 */     synchronized ("etpushsdk@ETSqliteOpenHelper")
/*  84:    */     {
/*  85: 97 */       if (usageCounter.decrementAndGet() == 0)
/*  86:    */       {
/*  87: 98 */         super.close();
/*  88: 99 */         beaconRequestDao = null;
/*  89:100 */         geofenceRequestDao = null;
/*  90:101 */         locationUpdateDao = null;
/*  91:102 */         messageDao = null;
/*  92:103 */         regionMessageDao = null;
/*  93:104 */         regionDao = null;
/*  94:105 */         registrationDao = null;
/*  95:106 */         analyticItemDao = null;
/*  96:107 */         helper = null;
/*  97:    */       }
/*  98:109 */       return;
/*  99:    */     }
/* 100:    */   }
/* 101:    */   
/* 102:    */   public void reset(Context applicationContext)
/* 103:    */   {
/* 104:116 */     synchronized ("etpushsdk@ETSqliteOpenHelper")
/* 105:    */     {
/* 106:117 */       close();
/* 107:    */       
/* 108:    */ 
/* 109:    */ 
/* 110:    */ 
/* 111:    */ 
/* 112:    */ 
/* 113:    */ 
/* 114:    */ 
/* 115:    */ 
/* 116:    */ 
/* 117:    */ 
/* 118:    */ 
/* 119:130 */       applicationContext.deleteDatabase("etdb.db");
/* 120:131 */       return;
/* 121:    */     }
/* 122:    */   }
/* 123:    */   
/* 124:    */   public void onUpgrade(SQLiteDatabase database, ConnectionSource connectionSource, int oldVersion, int newVersion)
/* 125:    */   {
/* 126:    */     try
/* 127:    */     {
/* 128:    */       ;
/* 129:137 */       if (oldVersion < 2)
/* 130:    */       {
/* 131:138 */         Log.d("etpushsdk@ETSqliteOpenHelper", "Updating DB from " + oldVersion + " to 2");
/* 132:139 */         getRegistrationDao().executeRaw("ALTER TABLE `registration` ADD COLUMN last_sent INTEGER;", new String[0]);
/* 133:140 */         getRegistrationDao().executeRaw("ALTER TABLE `registration` ADD COLUMN hwid TEXT;", new String[0]);
/* 134:141 */         getRegistrationDao().executeRaw("ALTER TABLE `registration` ADD COLUMN gcm_sender_id TEXT;", new String[0]);
/* 135:142 */         getLocationUpdateDao().executeRaw("ALTER TABLE `location_update` ADD COLUMN last_sent INTEGER;", new String[0]);
/* 136:143 */         getMessageDao().executeRaw("ALTER TABLE `messages` ADD COLUMN message_deleted BOOLEAN;", new String[0]);
/* 137:    */       }
/* 138:146 */       if (oldVersion < 3)
/* 139:    */       {
/* 140:147 */         getRegionDao().executeRaw("ALTER TABLE `regions` ADD COLUMN beacon_guid TEXT;", new String[0]);
/* 141:148 */         getRegionDao().executeRaw("ALTER TABLE `regions` ADD COLUMN beacon_major INTEGER;", new String[0]);
/* 142:149 */         getRegionDao().executeRaw("ALTER TABLE `regions` ADD COLUMN beacon_minor INTEGER;", new String[0]);
/* 143:150 */         getRegionDao().executeRaw("ALTER TABLE `regions` ADD COLUMN entry_count INTEGER;", new String[0]);
/* 144:151 */         getRegionDao().executeRaw("ALTER TABLE `regions` ADD COLUMN exit_count INTEGER;", new String[0]);
/* 145:152 */         getRegionDao().executeRaw("ALTER TABLE `regions` ADD COLUMN description TEXT;", new String[0]);
/* 146:153 */         getRegionDao().executeRaw("ALTER TABLE `regions` ADD COLUMN location_type INTEGER;", new String[0]);
/* 147:154 */         getRegionDao().executeRaw("ALTER TABLE `regions` ADD COLUMN name TEXT;", new String[0]);
/* 148:155 */         getRegionDao().executeRaw("ALTER TABLE `regions` ADD COLUMN has_entered BOOLEAN;", new String[0]);
/* 149:156 */         getMessageDao().executeRaw("ALTER TABLE `messages` ADD COLUMN min_tripped INTEGER;", new String[0]);
/* 150:157 */         getMessageDao().executeRaw("ALTER TABLE `messages` ADD COLUMN proximity INTEGER;", new String[0]);
/* 151:158 */         getMessageDao().executeRaw("ALTER TABLE `messages` ADD COLUMN ephemeral_message BOOLEAN;", new String[0]);
/* 152:159 */         getMessageDao().executeRaw("ALTER TABLE `messages` ADD COLUMN has_entered BOOLEAN;", new String[0]);
/* 153:160 */         getMessageDao().executeRaw("ALTER TABLE `messages` ADD COLUMN notify_id INTEGER;", new String[0]);
/* 154:161 */         getMessageDao().executeRaw("ALTER TABLE `messages` ADD COLUMN loiter_seconds INTEGER;", new String[0]);
/* 155:162 */         getMessageDao().executeRaw("ALTER TABLE `messages` ADD COLUMN entry_time INTEGER;", new String[0]);
/* 156:163 */         TableUtils.createTable(connectionSource, AnalyticItem.class);
/* 157:164 */         TableUtils.createTable(connectionSource, BeaconRequest.class);
/* 158:    */       }
/* 159:170 */       return;
/* 160:    */     }
/* 161:    */     catch (SQLException e)
/* 162:    */     {
/* 163:168 */       Log.e("etpushsdk@ETSqliteOpenHelper", "Can't create database", e);
/* 164:169 */       throw new RuntimeException(e);
/* 165:    */     }
/* 166:    */   }
/* 167:    */   
/* 168:    */   public Dao<BeaconRequest, Integer> getBeaconRequestDao()
/* 169:    */     throws SQLException
/* 170:    */   {
/* 171:178 */     if (beaconRequestDao == null) {
/* 172:179 */       beaconRequestDao = getDao(BeaconRequest.class);
/* 173:    */     }
/* 174:181 */     return beaconRequestDao;
/* 175:    */   }
/* 176:    */   
/* 177:    */   public Dao<GeofenceRequest, Integer> getGeofenceRequestDao()
/* 178:    */     throws SQLException
/* 179:    */   {
/* 180:189 */     if (geofenceRequestDao == null) {
/* 181:190 */       geofenceRequestDao = getDao(GeofenceRequest.class);
/* 182:    */     }
/* 183:192 */     return geofenceRequestDao;
/* 184:    */   }
/* 185:    */   
/* 186:    */   public Dao<LocationUpdate, Integer> getLocationUpdateDao()
/* 187:    */     throws SQLException
/* 188:    */   {
/* 189:200 */     if (locationUpdateDao == null) {
/* 190:201 */       locationUpdateDao = getDao(LocationUpdate.class);
/* 191:    */     }
/* 192:203 */     return locationUpdateDao;
/* 193:    */   }
/* 194:    */   
/* 195:    */   public Dao<Message, String> getMessageDao()
/* 196:    */     throws SQLException
/* 197:    */   {
/* 198:211 */     if (messageDao == null) {
/* 199:212 */       messageDao = getDao(Message.class);
/* 200:    */     }
/* 201:214 */     return messageDao;
/* 202:    */   }
/* 203:    */   
/* 204:    */   public Dao<Region, String> getRegionDao()
/* 205:    */     throws SQLException
/* 206:    */   {
/* 207:222 */     if (regionDao == null) {
/* 208:223 */       regionDao = getDao(Region.class);
/* 209:    */     }
/* 210:225 */     return regionDao;
/* 211:    */   }
/* 212:    */   
/* 213:    */   public Dao<RegionMessage, Integer> getRegionMessageDao()
/* 214:    */     throws SQLException
/* 215:    */   {
/* 216:233 */     if (regionMessageDao == null) {
/* 217:234 */       regionMessageDao = getDao(RegionMessage.class);
/* 218:    */     }
/* 219:236 */     return regionMessageDao;
/* 220:    */   }
/* 221:    */   
/* 222:    */   public Dao<Registration, Integer> getRegistrationDao()
/* 223:    */     throws SQLException
/* 224:    */   {
/* 225:244 */     if (registrationDao == null) {
/* 226:245 */       registrationDao = getDao(Registration.class);
/* 227:    */     }
/* 228:247 */     return registrationDao;
/* 229:    */   }
/* 230:    */   
/* 231:    */   public Dao<AnalyticItem, Integer> getAnalyticItemDao()
/* 232:    */     throws SQLException
/* 233:    */   {
/* 234:255 */     if (analyticItemDao == null) {
/* 235:256 */       analyticItemDao = getDao(AnalyticItem.class);
/* 236:    */     }
/* 237:258 */     return analyticItemDao;
/* 238:    */   }
/* 239:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.ETSqliteOpenHelper
 * JD-Core Version:    0.7.0.1
 */