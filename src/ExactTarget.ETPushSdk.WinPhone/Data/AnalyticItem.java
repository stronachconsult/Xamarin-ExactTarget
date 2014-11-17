/*   1:    */ package com.exacttarget.etpushsdk.data;
/*   2:    */ 
/*   3:    */ import android.content.Context;
/*   4:    */ import com.exacttarget.etpushsdk.Config;
/*   5:    */ import com.exacttarget.etpushsdk.util.JsonType;
/*   6:    */ import com.fasterxml.jackson.annotation.JsonFormat;
/*   7:    */ import com.fasterxml.jackson.annotation.JsonFormat.Shape;
/*   8:    */ import com.fasterxml.jackson.annotation.JsonIgnore;
/*   9:    */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*  10:    */ import com.fasterxml.jackson.annotation.JsonInclude;
/*  11:    */ import com.fasterxml.jackson.annotation.JsonInclude.Include;
/*  12:    */ import com.fasterxml.jackson.annotation.JsonProperty;
/*  13:    */ import com.j256.ormlite.field.DataType;
/*  14:    */ import com.j256.ormlite.field.DatabaseField;
/*  15:    */ import com.j256.ormlite.table.DatabaseTable;
/*  16:    */ import java.util.ArrayList;
/*  17:    */ import java.util.Date;
/*  18:    */ import java.util.List;
/*  19:    */ 
/*  20:    */ @DatabaseTable(tableName="analytic_item")
/*  21:    */ @JsonIgnoreProperties(ignoreUnknown=false)
/*  22:    */ @JsonInclude(JsonInclude.Include.NON_NULL)
/*  23:    */ public class AnalyticItem
/*  24:    */   extends DeviceData
/*  25:    */ {
/*  26:    */   public static final int ANALYTIC_TYPE_OPEN = 2;
/*  27:    */   public static final int ANALYTIC_TYPE_DISPLAY = 3;
/*  28:    */   public static final int ANALYTIC_TYPE_TIMEINAPP = 4;
/*  29:    */   public static final int ANALYTIC_TYPE_FENCE_ENTRY = 6;
/*  30:    */   public static final int ANALYTIC_TYPE_FENCE_EXIT = 7;
/*  31:    */   public static final int ANALYTIC_TYPE_RECEIVED = 10;
/*  32:    */   public static final int ANALYTIC_TYPE_TIME_IN_LOCATION = 11;
/*  33:    */   public static final int ANALYTIC_TYPE_BEACON_IN_RANGE = 12;
/*  34:    */   public static final int ANALYTIC_TYPE_TIME_WITH_BEACON_IN_RANGE = 13;
/*  35:    */   public static final String COLUMN_ET_APP_ID = "et_app_id";
/*  36:    */   public static final String COLUMN_DEVICE_ID = "device_id";
/*  37:    */   public static final String COLUMN_EVENT_DATE = "event_date";
/*  38:    */   public static final String COLUMN_ANALYTIC_TYPES = "analytic_types";
/*  39:    */   public static final String COLUMN_OBJECT_IDS = "object_ids";
/*  40:    */   public static final String COLUMN_VALUE = "value";
/*  41:    */   public static final String COLUMN_LAST_SENT = "last_sent";
/*  42:    */   public static final String COLUMN_READY_TO_SEND = "ready_to_send";
/*  43:    */   @DatabaseField(generatedId=true)
/*  44:    */   @JsonIgnore
/*  45:    */   private Integer id;
/*  46:    */   @DatabaseField(columnName="et_app_id")
/*  47:    */   @JsonProperty(required=true)
/*  48:    */   private String etAppId;
/*  49:    */   @DatabaseField(columnName="device_id")
/*  50:    */   @JsonProperty(required=true)
/*  51:    */   private String deviceId;
/*  52:    */   @DatabaseField(columnName="event_date", dataType=DataType.DATE_STRING, format="yyyy-MM-dd'T'HH:mm:ss.SSS'Z'")
/*  53:    */   @JsonProperty(required=true)
/*  54:    */   @JsonFormat(shape=JsonFormat.Shape.STRING, pattern="yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", locale="ENGLISH", timezone="UTC")
/*  55:    */   private Date eventDate;
/*  56:    */   @DatabaseField(columnName="analytic_types", persisterClass=JsonType.class)
/*  57:    */   @JsonProperty(required=true)
/*  58: 62 */   private List<Integer> analyticTypes = new ArrayList();
/*  59:    */   @DatabaseField(columnName="object_ids", persisterClass=JsonType.class)
/*  60:    */   @JsonProperty(required=true)
/*  61: 66 */   private List<String> objectIds = new ArrayList();
/*  62:    */   @DatabaseField(columnName="value")
/*  63:    */   @JsonProperty
/*  64:    */   private Integer value;
/*  65:    */   @DatabaseField(columnName="last_sent")
/*  66:    */   @JsonIgnore
/*  67: 74 */   private Long lastSent = Long.valueOf(0L);
/*  68:    */   @DatabaseField(columnName="ready_to_send")
/*  69:    */   @JsonIgnore
/*  70: 78 */   private Boolean readyToSend = Boolean.FALSE;
/*  71:    */   
/*  72:    */   public AnalyticItem()
/*  73:    */   {
/*  74: 84 */     this.etAppId = Config.getEtAppId();
/*  75:    */   }
/*  76:    */   
/*  77:    */   public AnalyticItem(Context context)
/*  78:    */   {
/*  79: 89 */     this.deviceId = uniqueDeviceIdentifier(context);
/*  80: 90 */     this.etAppId = Config.getEtAppId();
/*  81:    */   }
/*  82:    */   
/*  83:    */   public Integer getId()
/*  84:    */   {
/*  85: 94 */     return this.id;
/*  86:    */   }
/*  87:    */   
/*  88:    */   public void setId(Integer id)
/*  89:    */   {
/*  90: 98 */     this.id = id;
/*  91:    */   }
/*  92:    */   
/*  93:    */   public String getEtAppId()
/*  94:    */   {
/*  95:102 */     return this.etAppId;
/*  96:    */   }
/*  97:    */   
/*  98:    */   public void setEtAppId(String etAppId)
/*  99:    */   {
/* 100:106 */     this.etAppId = etAppId;
/* 101:    */   }
/* 102:    */   
/* 103:    */   public String getDeviceId()
/* 104:    */   {
/* 105:110 */     return this.deviceId;
/* 106:    */   }
/* 107:    */   
/* 108:    */   public void setDeviceId(String deviceId)
/* 109:    */   {
/* 110:114 */     this.deviceId = deviceId;
/* 111:    */   }
/* 112:    */   
/* 113:    */   public Date getEventDate()
/* 114:    */   {
/* 115:118 */     return this.eventDate;
/* 116:    */   }
/* 117:    */   
/* 118:    */   public void setEventDate(Date eventDate)
/* 119:    */   {
/* 120:122 */     this.eventDate = eventDate;
/* 121:    */   }
/* 122:    */   
/* 123:    */   public List<Integer> getAnalyticTypes()
/* 124:    */   {
/* 125:126 */     return this.analyticTypes;
/* 126:    */   }
/* 127:    */   
/* 128:    */   public void setAnalyticTypes(List<Integer> analyticTypes)
/* 129:    */   {
/* 130:130 */     this.analyticTypes = analyticTypes;
/* 131:    */   }
/* 132:    */   
/* 133:    */   public List<String> getObjectIds()
/* 134:    */   {
/* 135:134 */     return this.objectIds;
/* 136:    */   }
/* 137:    */   
/* 138:    */   public void setObjectIds(List<String> objectIds)
/* 139:    */   {
/* 140:138 */     this.objectIds = objectIds;
/* 141:    */   }
/* 142:    */   
/* 143:    */   public Integer getValue()
/* 144:    */   {
/* 145:142 */     return this.value;
/* 146:    */   }
/* 147:    */   
/* 148:    */   public void setValue(Integer value)
/* 149:    */   {
/* 150:146 */     this.value = value;
/* 151:    */   }
/* 152:    */   
/* 153:    */   public Long getLastSent()
/* 154:    */   {
/* 155:150 */     return this.lastSent;
/* 156:    */   }
/* 157:    */   
/* 158:    */   public void setLastSent(Long lastSent)
/* 159:    */   {
/* 160:154 */     this.lastSent = lastSent;
/* 161:    */   }
/* 162:    */   
/* 163:    */   public Boolean getReadyToSend()
/* 164:    */   {
/* 165:158 */     return this.readyToSend;
/* 166:    */   }
/* 167:    */   
/* 168:    */   public void setReadyToSend(Boolean readyToSend)
/* 169:    */   {
/* 170:162 */     this.readyToSend = readyToSend;
/* 171:    */   }
/* 172:    */   
/* 173:    */   public void addAnalyticType(int analyticType)
/* 174:    */   {
/* 175:166 */     this.analyticTypes.add(Integer.valueOf(analyticType));
/* 176:    */   }
/* 177:    */   
/* 178:    */   public void addObjectId(String objectId)
/* 179:    */   {
/* 180:170 */     this.objectIds.add(objectId);
/* 181:    */   }
/* 182:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.AnalyticItem
 * JD-Core Version:    0.7.0.1
 */