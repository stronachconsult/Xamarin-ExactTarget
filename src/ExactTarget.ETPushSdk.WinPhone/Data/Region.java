/*   1:    */ package com.exacttarget.etpushsdk.data;
/*   2:    */ 
/*   3:    */ import com.fasterxml.jackson.annotation.JsonIgnore;
/*   4:    */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*   5:    */ import com.fasterxml.jackson.annotation.JsonInclude;
/*   6:    */ import com.fasterxml.jackson.annotation.JsonInclude.Include;
/*   7:    */ import com.fasterxml.jackson.annotation.JsonProperty;
/*   8:    */ import com.google.android.gms.location.Geofence;
/*   9:    */ import com.google.android.gms.location.Geofence.Builder;
/*  10:    */ import com.j256.ormlite.field.DatabaseField;
/*  11:    */ import com.j256.ormlite.table.DatabaseTable;
/*  12:    */ import java.util.Date;
/*  13:    */ import java.util.List;
/*  14:    */ 
/*  15:    */ @DatabaseTable(tableName="regions")
/*  16:    */ @JsonIgnoreProperties(ignoreUnknown=true)
/*  17:    */ @JsonInclude(JsonInclude.Include.NON_NULL)
/*  18:    */ public class Region
/*  19:    */ {
/*  20:    */   public static final String TAG = "etpushsdk@Region";
/*  21:    */   public static final String COLUMN_ID = "id";
/*  22:    */   public static final String COLUMN_LATITUDE = "latitude";
/*  23:    */   public static final String COLUMN_LONGITUDE = "longitude";
/*  24:    */   public static final String COLUMN_RADIUS = "radius";
/*  25:    */   public static final String COLUMN_ACTIVE = "active";
/*  26:    */   public static final String COLUMN_BEACON_GUID = "beacon_guid";
/*  27:    */   public static final String COLUMN_BEACON_MAJOR = "beacon_major";
/*  28:    */   public static final String COLUMN_BEACON_MINOR = "beacon_minor";
/*  29:    */   public static final String COLUMN_ENTRY_COUNT = "entry_count";
/*  30:    */   public static final String COLUMN_EXIT_COUNT = "exit_count";
/*  31:    */   public static final String COLUMN_DESCRIPTION = "description";
/*  32:    */   public static final String COLUMN_NAME = "name";
/*  33:    */   public static final String COLUMN_LOCATION_TYPE = "location_type";
/*  34:    */   public static final String COLUMN_HAS_ENTERED = "has_entered";
/*  35:    */   public static final int LOCATION_TYPE_FENCE = 1;
/*  36:    */   public static final int LOCATION_TYPE_BEACON = 3;
/*  37:    */   public static final String MAGIC_FENCE_ID = "~~m@g1c_f3nc3~~";
/*  38:    */   @DatabaseField(id=true, columnName="id")
/*  39:    */   @JsonProperty("id")
/*  40:    */   private String id;
/*  41:    */   @DatabaseField(columnName="name")
/*  42:    */   @JsonProperty("name")
/*  43:    */   private String name;
/*  44:    */   @DatabaseField(persisted=false)
/*  45:    */   @JsonProperty("center")
/*  46:    */   private LatLon center;
/*  47:    */   @DatabaseField(columnName="location_type")
/*  48:    */   @JsonProperty("locationType")
/*  49: 74 */   private Integer locationType = Integer.valueOf(1);
/*  50:    */   @DatabaseField(columnName="latitude")
/*  51:    */   @JsonIgnore
/*  52:    */   private Double latitude;
/*  53:    */   @DatabaseField(columnName="longitude")
/*  54:    */   @JsonIgnore
/*  55:    */   private Double longitude;
/*  56:    */   @DatabaseField(columnName="radius")
/*  57:    */   @JsonProperty("radius")
/*  58:    */   private Integer radius;
/*  59:    */   @DatabaseField(persisted=false)
/*  60:    */   @JsonProperty("messages")
/*  61:    */   private List<Message> messages;
/*  62:    */   @DatabaseField(columnName="active")
/*  63:    */   @JsonIgnore
/*  64:    */   private Boolean active;
/*  65:    */   @DatabaseField(columnName="beacon_guid")
/*  66:    */   @JsonProperty("proximityUuid")
/*  67:    */   private String guid;
/*  68:    */   @DatabaseField(columnName="beacon_major")
/*  69:    */   @JsonProperty("major")
/*  70:    */   private Integer major;
/*  71:    */   @DatabaseField(columnName="beacon_minor")
/*  72:    */   @JsonProperty("minor")
/*  73:    */   private Integer minor;
/*  74:    */   @DatabaseField(columnName="entry_count")
/*  75:    */   @JsonIgnore
/*  76:110 */   private Integer entryCount = Integer.valueOf(0);
/*  77:    */   @DatabaseField(columnName="exit_count")
/*  78:    */   @JsonIgnore
/*  79:114 */   private Integer exitCount = Integer.valueOf(0);
/*  80:    */   @DatabaseField(columnName="description")
/*  81:    */   @JsonProperty("description")
/*  82:    */   private String description;
/*  83:    */   @DatabaseField(columnName="has_entered")
/*  84:    */   @JsonIgnore
/*  85:122 */   private Boolean hasEntered = Boolean.FALSE;
/*  86:    */   
/*  87:    */   public String getId()
/*  88:    */   {
/*  89:127 */     return this.id;
/*  90:    */   }
/*  91:    */   
/*  92:    */   public void setId(String id)
/*  93:    */   {
/*  94:131 */     this.id = id;
/*  95:    */   }
/*  96:    */   
/*  97:    */   public String getName()
/*  98:    */   {
/*  99:135 */     return this.name;
/* 100:    */   }
/* 101:    */   
/* 102:    */   public void setName(String name)
/* 103:    */   {
/* 104:139 */     this.name = name;
/* 105:    */   }
/* 106:    */   
/* 107:    */   public Integer getLocationType()
/* 108:    */   {
/* 109:143 */     if (this.locationType == null) {
/* 110:144 */       this.locationType = Integer.valueOf(1);
/* 111:    */     }
/* 112:146 */     return this.locationType;
/* 113:    */   }
/* 114:    */   
/* 115:    */   public void setLocationType(Integer locationType)
/* 116:    */   {
/* 117:150 */     this.locationType = locationType;
/* 118:    */   }
/* 119:    */   
/* 120:    */   public LatLon getCenter()
/* 121:    */   {
/* 122:154 */     return this.center;
/* 123:    */   }
/* 124:    */   
/* 125:    */   public void setCenter(LatLon center)
/* 126:    */   {
/* 127:158 */     this.center = center;
/* 128:159 */     setLatitude(center.getLatitude());
/* 129:160 */     setLongitude(center.getLongitude());
/* 130:    */   }
/* 131:    */   
/* 132:    */   public Double getLatitude()
/* 133:    */   {
/* 134:164 */     return this.latitude;
/* 135:    */   }
/* 136:    */   
/* 137:    */   public void setLatitude(Double latitude)
/* 138:    */   {
/* 139:168 */     this.latitude = latitude;
/* 140:    */   }
/* 141:    */   
/* 142:    */   public Double getLongitude()
/* 143:    */   {
/* 144:172 */     return this.longitude;
/* 145:    */   }
/* 146:    */   
/* 147:    */   public void setLongitude(Double longitude)
/* 148:    */   {
/* 149:176 */     this.longitude = longitude;
/* 150:    */   }
/* 151:    */   
/* 152:    */   public Integer getRadius()
/* 153:    */   {
/* 154:180 */     return this.radius;
/* 155:    */   }
/* 156:    */   
/* 157:    */   public void setRadius(Integer radius)
/* 158:    */   {
/* 159:184 */     this.radius = radius;
/* 160:    */   }
/* 161:    */   
/* 162:    */   public List<Message> getMessages()
/* 163:    */   {
/* 164:188 */     return this.messages;
/* 165:    */   }
/* 166:    */   
/* 167:    */   public void setMessages(List<Message> messages)
/* 168:    */   {
/* 169:192 */     this.messages = messages;
/* 170:    */   }
/* 171:    */   
/* 172:    */   public Boolean getActive()
/* 173:    */   {
/* 174:196 */     return this.active;
/* 175:    */   }
/* 176:    */   
/* 177:    */   public void setActive(Boolean active)
/* 178:    */   {
/* 179:200 */     this.active = active;
/* 180:    */   }
/* 181:    */   
/* 182:    */   public String getGuid()
/* 183:    */   {
/* 184:204 */     return this.guid;
/* 185:    */   }
/* 186:    */   
/* 187:    */   public void setGuid(String guid)
/* 188:    */   {
/* 189:208 */     this.guid = guid;
/* 190:    */   }
/* 191:    */   
/* 192:    */   public Integer getMajor()
/* 193:    */   {
/* 194:212 */     return this.major;
/* 195:    */   }
/* 196:    */   
/* 197:    */   public void setMajor(Integer major)
/* 198:    */   {
/* 199:216 */     this.major = major;
/* 200:    */   }
/* 201:    */   
/* 202:    */   public Integer getMinor()
/* 203:    */   {
/* 204:220 */     return this.minor;
/* 205:    */   }
/* 206:    */   
/* 207:    */   public void setMinor(Integer minor)
/* 208:    */   {
/* 209:224 */     this.minor = minor;
/* 210:    */   }
/* 211:    */   
/* 212:    */   public Integer getEntryCount()
/* 213:    */   {
/* 214:228 */     return this.entryCount;
/* 215:    */   }
/* 216:    */   
/* 217:    */   public void setEntryCount(Integer entryCount)
/* 218:    */   {
/* 219:232 */     this.entryCount = entryCount;
/* 220:    */   }
/* 221:    */   
/* 222:    */   public Integer getExitCount()
/* 223:    */   {
/* 224:236 */     return this.exitCount;
/* 225:    */   }
/* 226:    */   
/* 227:    */   public void setExitCount(Integer exitCount)
/* 228:    */   {
/* 229:240 */     this.exitCount = exitCount;
/* 230:    */   }
/* 231:    */   
/* 232:    */   public String getDescription()
/* 233:    */   {
/* 234:244 */     return this.description;
/* 235:    */   }
/* 236:    */   
/* 237:    */   public void setDescription(String description)
/* 238:    */   {
/* 239:248 */     this.description = description;
/* 240:    */   }
/* 241:    */   
/* 242:    */   public Boolean getHasEntered()
/* 243:    */   {
/* 244:252 */     return this.hasEntered;
/* 245:    */   }
/* 246:    */   
/* 247:    */   public void setHasEntered(Boolean hasEntered)
/* 248:    */   {
/* 249:256 */     this.hasEntered = hasEntered;
/* 250:    */   }
/* 251:    */   
/* 252:    */   public Geofence toGeofence()
/* 253:    */   {
/* 254:266 */     long lastEnding = 0L;
/* 255:268 */     for (Message message : this.messages) {
/* 256:269 */       if (lastEnding != -1L) {
/* 257:270 */         if (message.getEndDate() != null)
/* 258:    */         {
/* 259:271 */           if (lastEnding < message.getEndDate().getTime()) {
/* 260:272 */             lastEnding = message.getEndDate().getTime();
/* 261:    */           }
/* 262:    */         }
/* 263:    */         else {
/* 264:276 */           lastEnding = -1L;
/* 265:    */         }
/* 266:    */       }
/* 267:    */     }
/* 268:    */     int transitionTypes;
/* 269:281 */     if ("~~m@g1c_f3nc3~~".equals(getId())) {
/* 270:283 */       transitionTypes = 2;
/* 271:    */     } else {
/* 272:287 */       transitionTypes = 3;
/* 273:    */     }
/* 274:289 */     return new Geofence.Builder().setRequestId(this.id).setCircularRegion(this.latitude.doubleValue(), this.longitude.doubleValue(), this.radius.intValue()).setTransitionTypes(transitionTypes).setExpirationDuration(lastEnding == -1L ? -1L : lastEnding - System.currentTimeMillis()).build();
/* 275:    */   }
/* 276:    */   
/* 277:    */   public boolean equals(Object o)
/* 278:    */   {
/* 279:299 */     if ((o instanceof Region))
/* 280:    */     {
/* 281:300 */       if ((this.id == null) && (((Region)o).id == null)) {
/* 282:301 */         return true;
/* 283:    */       }
/* 284:303 */       if (this.id != null) {
/* 285:304 */         return this.id.equals(((Region)o).id);
/* 286:    */       }
/* 287:    */     }
/* 288:307 */     return false;
/* 289:    */   }
/* 290:    */   
/* 291:    */   public int hashCode()
/* 292:    */   {
/* 293:312 */     return this.id.hashCode();
/* 294:    */   }
/* 295:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.Region
 * JD-Core Version:    0.7.0.1
 */