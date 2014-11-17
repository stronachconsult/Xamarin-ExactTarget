/*   1:    */ package com.exacttarget.etpushsdk.data;
/*   2:    */ 
/*   3:    */ import com.fasterxml.jackson.annotation.JsonFormat;
/*   4:    */ import com.fasterxml.jackson.annotation.JsonFormat.Shape;
/*   5:    */ import com.fasterxml.jackson.annotation.JsonIgnore;
/*   6:    */ import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
/*   7:    */ import com.fasterxml.jackson.annotation.JsonInclude;
/*   8:    */ import com.fasterxml.jackson.annotation.JsonInclude.Include;
/*   9:    */ import com.fasterxml.jackson.annotation.JsonProperty;
/*  10:    */ import com.fasterxml.jackson.annotation.JsonRawValue;
/*  11:    */ import com.fasterxml.jackson.databind.JsonNode;
/*  12:    */ import com.j256.ormlite.field.DataType;
/*  13:    */ import com.j256.ormlite.field.DatabaseField;
/*  14:    */ import com.j256.ormlite.table.DatabaseTable;
/*  15:    */ import java.io.Serializable;
/*  16:    */ import java.util.ArrayList;
/*  17:    */ import java.util.Date;
/*  18:    */ 
/*  19:    */ @DatabaseTable(tableName="messages")
/*  20:    */ @JsonIgnoreProperties(ignoreUnknown=true)
/*  21:    */ @JsonInclude(JsonInclude.Include.NON_NULL)
/*  22:    */ public class Message
/*  23:    */   implements Serializable
/*  24:    */ {
/*  25:    */   private static final long serialVersionUID = 1L;
/*  26:    */   public static final String COLUMN_ID = "id";
/*  27:    */   public static final String COLUMN_ALERT = "alert";
/*  28:    */   public static final String COLUMN_SOUND = "sound";
/*  29:    */   public static final String COLUMN_BADGE = "badge";
/*  30:    */   public static final String COLUMN_OPEN_DIRECT = "open_direct";
/*  31:    */   public static final String COLUMN_START_DATE = "start_date";
/*  32:    */   public static final String COLUMN_END_DATE = "end_date";
/*  33:    */   public static final String COLUMN_MESSAGE_TYPE = "message_type";
/*  34:    */   public static final String COLUMN_CONTENT_TYPE = "content_type";
/*  35:    */   public static final String COLUMN_PAGE_ID = "page_id";
/*  36:    */   public static final String COLUMN_URL = "url";
/*  37:    */   public static final String COLUMN_SUBJECT = "subject";
/*  38:    */   public static final String COLUMN_SITE_ID = "site_id";
/*  39:    */   public static final String COLUMN_READ = "read";
/*  40:    */   public static final String COLUMN_CUSTOM = "custom";
/*  41:    */   public static final String COLUMN_KEYS = "keys";
/*  42:    */   public static final String COLUMN_PERIOD_SHOW_COUNT = "period_show_count";
/*  43:    */   public static final String COLUMN_SHOW_COUNT = "show_count";
/*  44:    */   public static final String COLUMN_LAST_SHOWN_DATE = "last_shown_date";
/*  45:    */   public static final String COLUMN_NEXT_ALLOWED_SHOW = "next_allowed_show";
/*  46:    */   public static final String COLUMN_MESSAGE_LIMIT = "message_limit";
/*  47:    */   public static final String COLUMN_ROLLING_PERIOD = "rolling_period";
/*  48:    */   public static final String COLUMN_PERIOD_TYPE = "period_type";
/*  49:    */   public static final String COLUMN_NUMBER_OF_PERIODS = "number_of_periods";
/*  50:    */   public static final String COLUMN_MESSAGES_PER_PERIOD = "messages_per_period";
/*  51:    */   public static final String COLUMN_MESSAGE_DELETED = "message_deleted";
/*  52:    */   public static final String COLUMN_MIN_TRIPPED = "min_tripped";
/*  53:    */   public static final String COLUMN_PROXIMITY = "proximity";
/*  54:    */   public static final String COLUMN_EPHEMERAL_MESSAGE = "ephemeral_message";
/*  55:    */   public static final String COLUMN_HAS_ENTERED = "has_entered";
/*  56:    */   public static final String COLUMN_NOTIFY_ID = "notify_id";
/*  57:    */   public static final String COLUMN_LOITER_SECONDS = "loiter_seconds";
/*  58:    */   public static final String COLUMN_ENTRY_TIME = "entry_time";
/*  59: 74 */   public static final Integer MESSAGE_TYPE_UNKNOWN = Integer.valueOf(0);
/*  60: 75 */   public static final Integer MESSAGE_TYPE_BASIC = Integer.valueOf(1);
/*  61: 76 */   public static final Integer MESSAGE_TYPE_ENHANCED = Integer.valueOf(2);
/*  62: 77 */   public static final Integer MESSAGE_TYPE_FENCE_ENTRY = Integer.valueOf(3);
/*  63: 78 */   public static final Integer MESSAGE_TYPE_FENCE_EXIT = Integer.valueOf(4);
/*  64: 79 */   public static final Integer MESSAGE_TYPE_PROXIMITY_ENTRY = Integer.valueOf(5);
/*  65: 81 */   public static final Integer CONTENT_TYPE_NONE = Integer.valueOf(0);
/*  66: 82 */   public static final Integer CONTENT_TYPE_ALERT = Integer.valueOf(1);
/*  67: 83 */   public static final Integer CONTENT_TYPE_PAGE = Integer.valueOf(2);
/*  68:    */   public static final int PERIOD_TYPE_UNIT_NONE = 0;
/*  69:    */   public static final int PERIOD_TYPE_UNIT_YEAR = 1;
/*  70:    */   public static final int PERIOD_TYPE_UNIT_MONTH = 2;
/*  71:    */   public static final int PERIOD_TYPE_UNIT_WEEK = 3;
/*  72:    */   public static final int PERIOD_TYPE_UNIT_DAY = 4;
/*  73:    */   public static final int PERIOD_TYPE_UNIT_HOUR = 5;
/*  74:    */   public static final long UNIT_HOUR = 3600000L;
/*  75:    */   public static final long UNIT_DAY = 86400000L;
/*  76:    */   public static final long UNIT_WEEK = 604800000L;
/*  77:    */   public static final long UNIT_MONTH = 2592000000L;
/*  78:    */   public static final long UNIT_YEAR = 31536000000L;
/*  79:    */   public static final int PROXIMITY_UNKNOWN = 0;
/*  80:    */   public static final int PROXIMITY_IMMEDIATE = 1;
/*  81:    */   public static final int PROXIMITY_NEAR = 2;
/*  82:    */   public static final int PROXIMITY_FAR = 3;
/*  83:    */   @DatabaseField(id=true, columnName="id")
/*  84:    */   @JsonProperty("id")
/*  85:104 */   private String id = null;
/*  86:    */   @DatabaseField(columnName="alert")
/*  87:    */   @JsonProperty("alert")
/*  88:108 */   private String alert = null;
/*  89:    */   @DatabaseField(columnName="sound")
/*  90:    */   @JsonProperty("sound")
/*  91:112 */   private String sound = null;
/*  92:    */   @DatabaseField(columnName="badge")
/*  93:    */   @JsonProperty("badge")
/*  94:116 */   private String badge = null;
/*  95:    */   @DatabaseField(columnName="open_direct")
/*  96:    */   @JsonProperty("openDirect")
/*  97:120 */   private String openDirect = null;
/*  98:    */   @DatabaseField(columnName="start_date", dataType=DataType.DATE_STRING, format="yyyy-MM-dd'T'HH:mm:ss.SSS'Z'")
/*  99:    */   @JsonProperty("startDateUtc")
/* 100:    */   @JsonFormat(shape=JsonFormat.Shape.STRING, pattern="yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", locale="ENGLISH", timezone="UTC")
/* 101:124 */   private Date startDate = new Date(System.currentTimeMillis() - 1000L);
/* 102:    */   @DatabaseField(columnName="end_date", dataType=DataType.DATE_STRING, format="yyyy-MM-dd'T'HH:mm:ss.SSS'Z'")
/* 103:    */   @JsonProperty("endDateUtc")
/* 104:    */   @JsonFormat(shape=JsonFormat.Shape.STRING, pattern="yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", locale="ENGLISH", timezone="UTC")
/* 105:129 */   private Date endDate = null;
/* 106:    */   @DatabaseField(columnName="message_type")
/* 107:    */   @JsonProperty("messageType")
/* 108:134 */   private Integer messageType = null;
/* 109:    */   @DatabaseField(columnName="content_type")
/* 110:    */   @JsonProperty("contentType")
/* 111:138 */   private Integer contentType = null;
/* 112:    */   @DatabaseField(columnName="messages_per_period")
/* 113:    */   @JsonProperty("messagesPerPeriod")
/* 114:142 */   private Integer messagesPerPeriod = Integer.valueOf(-1);
/* 115:    */   @DatabaseField(columnName="number_of_periods")
/* 116:    */   @JsonProperty("numberOfPeriods")
/* 117:146 */   private Integer numberOfPeriods = Integer.valueOf(-1);
/* 118:    */   @DatabaseField(columnName="period_type")
/* 119:    */   @JsonProperty("periodType")
/* 120:150 */   private Integer periodType = Integer.valueOf(0);
/* 121:    */   @DatabaseField(columnName="rolling_period")
/* 122:    */   @JsonProperty("isRollingPeriod")
/* 123:154 */   private Boolean isRollingPeriod = Boolean.TRUE;
/* 124:    */   @DatabaseField(columnName="message_limit")
/* 125:    */   @JsonProperty("messageLimit")
/* 126:158 */   private Integer messageLimit = Integer.valueOf(-1);
/* 127:    */   @DatabaseField(columnName="next_allowed_show", dataType=DataType.DATE_STRING, format="yyyy-MM-dd'T'HH:mm:ss.SSS'Z'")
/* 128:    */   @JsonIgnore
/* 129:162 */   private Date nextAllowedShow = new Date(System.currentTimeMillis() - 1000L);
/* 130:    */   @DatabaseField(columnName="last_shown_date", dataType=DataType.DATE_STRING, format="yyyy-MM-dd'T'HH:mm:ss.SSS'Z'")
/* 131:    */   @JsonIgnore
/* 132:166 */   private Date lastShownDate = null;
/* 133:    */   @DatabaseField(columnName="show_count")
/* 134:    */   @JsonIgnore
/* 135:170 */   private Integer showCount = Integer.valueOf(0);
/* 136:    */   @DatabaseField(columnName="period_show_count")
/* 137:    */   @JsonIgnore
/* 138:174 */   private Integer periodShowCount = Integer.valueOf(0);
/* 139:    */   @DatabaseField(columnName="min_tripped")
/* 140:    */   @JsonProperty("minTripped")
/* 141:178 */   private Integer minTripped = Integer.valueOf(0);
/* 142:    */   @DatabaseField(columnName="keys", dataType=DataType.SERIALIZABLE)
/* 143:    */   @JsonProperty("keys")
/* 144:182 */   private ArrayList<Attribute> keys = null;
/* 145:    */   @DatabaseField(columnName="custom", dataType=DataType.STRING)
/* 146:    */   @JsonIgnore
/* 147:    */   private String custom;
/* 148:    */   @DatabaseField(persisted=false)
/* 149:    */   @JsonRawValue
/* 150:    */   @JsonProperty("custom")
/* 151:190 */   private JsonNode customObj = null;
/* 152:    */   @DatabaseField(columnName="read")
/* 153:195 */   private Boolean read = Boolean.FALSE;
/* 154:    */   @DatabaseField(columnName="site_id")
/* 155:    */   @JsonProperty("siteId")
/* 156:    */   private String siteId;
/* 157:    */   @DatabaseField(columnName="subject")
/* 158:    */   @JsonProperty("subject")
/* 159:    */   private String subject;
/* 160:    */   @DatabaseField(columnName="url")
/* 161:    */   @JsonProperty("url")
/* 162:    */   private String url;
/* 163:    */   @DatabaseField(columnName="page_id")
/* 164:    */   @JsonProperty("pageId")
/* 165:    */   private String pageId;
/* 166:    */   @DatabaseField(columnName="message_deleted")
/* 167:    */   @JsonIgnore
/* 168:214 */   private Boolean messageDeleted = Boolean.FALSE;
/* 169:    */   @DatabaseField(columnName="proximity")
/* 170:    */   @JsonProperty("proximity")
/* 171:218 */   private int proximity = 2;
/* 172:    */   @DatabaseField(columnName="ephemeral_message")
/* 173:    */   @JsonProperty("ephemeralMessage")
/* 174:222 */   private Boolean ephemeralMessage = Boolean.FALSE;
/* 175:    */   @DatabaseField(columnName="has_entered")
/* 176:    */   @JsonIgnore
/* 177:226 */   private Boolean hasEntered = Boolean.FALSE;
/* 178:    */   @DatabaseField(columnName="notify_id")
/* 179:    */   @JsonIgnore
/* 180:    */   private Integer notifyId;
/* 181:    */   @DatabaseField(columnName="loiter_seconds")
/* 182:    */   @JsonProperty("loiterSeconds")
/* 183:234 */   private Integer loiterSeconds = Integer.valueOf(0);
/* 184:    */   @DatabaseField(columnName="entry_time")
/* 185:    */   @JsonIgnore
/* 186:238 */   private Long entryTime = Long.valueOf(0L);
/* 187:    */   
/* 188:    */   public String getId()
/* 189:    */   {
/* 190:243 */     return this.id;
/* 191:    */   }
/* 192:    */   
/* 193:    */   public void setId(String id)
/* 194:    */   {
/* 195:247 */     this.id = id;
/* 196:    */   }
/* 197:    */   
/* 198:    */   public String getAlert()
/* 199:    */   {
/* 200:251 */     return this.alert;
/* 201:    */   }
/* 202:    */   
/* 203:    */   public void setAlert(String alert)
/* 204:    */   {
/* 205:255 */     this.alert = alert;
/* 206:    */   }
/* 207:    */   
/* 208:    */   public String getSound()
/* 209:    */   {
/* 210:259 */     return this.sound;
/* 211:    */   }
/* 212:    */   
/* 213:    */   public void setSound(String sound)
/* 214:    */   {
/* 215:263 */     this.sound = sound;
/* 216:    */   }
/* 217:    */   
/* 218:    */   public String getBadge()
/* 219:    */   {
/* 220:267 */     return this.badge;
/* 221:    */   }
/* 222:    */   
/* 223:    */   public void setBadge(String badge)
/* 224:    */   {
/* 225:271 */     this.badge = badge;
/* 226:    */   }
/* 227:    */   
/* 228:    */   public String getOpenDirect()
/* 229:    */   {
/* 230:275 */     return this.openDirect;
/* 231:    */   }
/* 232:    */   
/* 233:    */   public void setOpenDirect(String openDirect)
/* 234:    */   {
/* 235:279 */     this.openDirect = openDirect;
/* 236:    */   }
/* 237:    */   
/* 238:    */   public Date getStartDate()
/* 239:    */   {
/* 240:283 */     return this.startDate;
/* 241:    */   }
/* 242:    */   
/* 243:    */   public void setStartDate(Date startDate)
/* 244:    */   {
/* 245:287 */     this.startDate = startDate;
/* 246:    */   }
/* 247:    */   
/* 248:    */   public Date getEndDate()
/* 249:    */   {
/* 250:291 */     return this.endDate;
/* 251:    */   }
/* 252:    */   
/* 253:    */   public void setEndDate(Date endDate)
/* 254:    */   {
/* 255:295 */     this.endDate = endDate;
/* 256:    */   }
/* 257:    */   
/* 258:    */   public Integer getMessageType()
/* 259:    */   {
/* 260:299 */     return this.messageType;
/* 261:    */   }
/* 262:    */   
/* 263:    */   public void setMessageType(Integer messageType)
/* 264:    */   {
/* 265:303 */     this.messageType = messageType;
/* 266:    */   }
/* 267:    */   
/* 268:    */   public Integer getMessagesPerPeriod()
/* 269:    */   {
/* 270:307 */     return this.messagesPerPeriod;
/* 271:    */   }
/* 272:    */   
/* 273:    */   public void setMessagesPerPeriod(Integer messagesPerPeriod)
/* 274:    */   {
/* 275:311 */     this.messagesPerPeriod = messagesPerPeriod;
/* 276:    */   }
/* 277:    */   
/* 278:    */   public Integer getNumberOfPeriods()
/* 279:    */   {
/* 280:315 */     return this.numberOfPeriods;
/* 281:    */   }
/* 282:    */   
/* 283:    */   public void setNumberOfPeriods(Integer numberOfPeriods)
/* 284:    */   {
/* 285:319 */     this.numberOfPeriods = numberOfPeriods;
/* 286:    */   }
/* 287:    */   
/* 288:    */   public Integer getPeriodType()
/* 289:    */   {
/* 290:323 */     return this.periodType;
/* 291:    */   }
/* 292:    */   
/* 293:    */   public void setPeriodType(Integer periodType)
/* 294:    */   {
/* 295:327 */     this.periodType = periodType;
/* 296:    */   }
/* 297:    */   
/* 298:    */   public Boolean getIsRollingPeriod()
/* 299:    */   {
/* 300:331 */     return this.isRollingPeriod;
/* 301:    */   }
/* 302:    */   
/* 303:    */   public void setIsRollingPeriod(Boolean isRollingPeriod)
/* 304:    */   {
/* 305:335 */     this.isRollingPeriod = isRollingPeriod;
/* 306:    */   }
/* 307:    */   
/* 308:    */   public Integer getMessageLimit()
/* 309:    */   {
/* 310:339 */     return this.messageLimit;
/* 311:    */   }
/* 312:    */   
/* 313:    */   public void setMessageLimit(Integer messageLimit)
/* 314:    */   {
/* 315:343 */     this.messageLimit = messageLimit;
/* 316:    */   }
/* 317:    */   
/* 318:    */   public ArrayList<Attribute> getKeys()
/* 319:    */   {
/* 320:347 */     return this.keys;
/* 321:    */   }
/* 322:    */   
/* 323:    */   public void setKeys(ArrayList<Attribute> keys)
/* 324:    */   {
/* 325:351 */     this.keys = keys;
/* 326:    */   }
/* 327:    */   
/* 328:    */   public Date getNextAllowedShow()
/* 329:    */   {
/* 330:355 */     return this.nextAllowedShow;
/* 331:    */   }
/* 332:    */   
/* 333:    */   public void setNextAllowedShow(Date nextAllowedShow)
/* 334:    */   {
/* 335:359 */     this.nextAllowedShow = nextAllowedShow;
/* 336:    */   }
/* 337:    */   
/* 338:    */   public Date getLastShownDate()
/* 339:    */   {
/* 340:363 */     return this.lastShownDate;
/* 341:    */   }
/* 342:    */   
/* 343:    */   public void setLastShownDate(Date lastShownDate)
/* 344:    */   {
/* 345:367 */     this.lastShownDate = lastShownDate;
/* 346:    */   }
/* 347:    */   
/* 348:    */   public Integer getShowCount()
/* 349:    */   {
/* 350:371 */     return this.showCount;
/* 351:    */   }
/* 352:    */   
/* 353:    */   public void setShowCount(Integer showCount)
/* 354:    */   {
/* 355:375 */     this.showCount = showCount;
/* 356:    */   }
/* 357:    */   
/* 358:    */   public Integer getPeriodShowCount()
/* 359:    */   {
/* 360:379 */     return this.periodShowCount;
/* 361:    */   }
/* 362:    */   
/* 363:    */   public void setPeriodShowCount(Integer periodShowCount)
/* 364:    */   {
/* 365:383 */     this.periodShowCount = periodShowCount;
/* 366:    */   }
/* 367:    */   
/* 368:    */   public String getCustom()
/* 369:    */   {
/* 370:387 */     if (this.customObj == null) {
/* 371:387 */       return this.custom;
/* 372:    */     }
/* 373:387 */     return this.customObj.toString();
/* 374:    */   }
/* 375:    */   
/* 376:    */   public void setCustom(String custom)
/* 377:    */   {
/* 378:391 */     this.custom = custom;
/* 379:    */   }
/* 380:    */   
/* 381:    */   public JsonNode getCustomObj()
/* 382:    */   {
/* 383:396 */     return this.customObj;
/* 384:    */   }
/* 385:    */   
/* 386:    */   public void setCustomObj(JsonNode customObj)
/* 387:    */   {
/* 388:400 */     this.customObj = customObj;
/* 389:    */   }
/* 390:    */   
/* 391:    */   public Integer getContentType()
/* 392:    */   {
/* 393:404 */     return this.contentType;
/* 394:    */   }
/* 395:    */   
/* 396:    */   public void setContentType(Integer contentType)
/* 397:    */   {
/* 398:408 */     this.contentType = contentType;
/* 399:    */   }
/* 400:    */   
/* 401:    */   public Boolean getRead()
/* 402:    */   {
/* 403:412 */     return this.read;
/* 404:    */   }
/* 405:    */   
/* 406:    */   public void setRead(Boolean read)
/* 407:    */   {
/* 408:416 */     this.read = read;
/* 409:    */   }
/* 410:    */   
/* 411:    */   public String getSiteId()
/* 412:    */   {
/* 413:420 */     return this.siteId;
/* 414:    */   }
/* 415:    */   
/* 416:    */   public void setSiteId(String siteId)
/* 417:    */   {
/* 418:424 */     this.siteId = siteId;
/* 419:    */   }
/* 420:    */   
/* 421:    */   public String getSubject()
/* 422:    */   {
/* 423:428 */     if ((this.subject == null) && (this.alert == null)) {
/* 424:429 */       return "A new message";
/* 425:    */     }
/* 426:431 */     if (this.subject == null) {
/* 427:432 */       return this.alert;
/* 428:    */     }
/* 429:434 */     return this.subject;
/* 430:    */   }
/* 431:    */   
/* 432:    */   public void setSubject(String subject)
/* 433:    */   {
/* 434:438 */     this.subject = subject;
/* 435:    */   }
/* 436:    */   
/* 437:    */   public String getUrl()
/* 438:    */   {
/* 439:442 */     return this.url;
/* 440:    */   }
/* 441:    */   
/* 442:    */   public void setUrl(String url)
/* 443:    */   {
/* 444:446 */     this.url = url;
/* 445:    */   }
/* 446:    */   
/* 447:    */   public String getPageId()
/* 448:    */   {
/* 449:450 */     return this.pageId;
/* 450:    */   }
/* 451:    */   
/* 452:    */   public void setPageId(String pageId)
/* 453:    */   {
/* 454:454 */     this.pageId = pageId;
/* 455:    */   }
/* 456:    */   
/* 457:    */   public Boolean getMessageDeleted()
/* 458:    */   {
/* 459:458 */     return this.messageDeleted;
/* 460:    */   }
/* 461:    */   
/* 462:    */   public void setMessageDeleted(Boolean messageDeleted)
/* 463:    */   {
/* 464:462 */     this.messageDeleted = messageDeleted;
/* 465:    */   }
/* 466:    */   
/* 467:    */   public Integer getMinTripped()
/* 468:    */   {
/* 469:466 */     return this.minTripped;
/* 470:    */   }
/* 471:    */   
/* 472:    */   public void setMinTripped(Integer minTripped)
/* 473:    */   {
/* 474:470 */     this.minTripped = minTripped;
/* 475:    */   }
/* 476:    */   
/* 477:    */   public int getProximity()
/* 478:    */   {
/* 479:474 */     return this.proximity;
/* 480:    */   }
/* 481:    */   
/* 482:    */   public void setProximity(int proximity)
/* 483:    */   {
/* 484:478 */     this.proximity = proximity;
/* 485:    */   }
/* 486:    */   
/* 487:    */   public Boolean getEphemeralMessage()
/* 488:    */   {
/* 489:482 */     return this.ephemeralMessage;
/* 490:    */   }
/* 491:    */   
/* 492:    */   public void setEphemeralMessage(Boolean ephemeralMessage)
/* 493:    */   {
/* 494:486 */     this.ephemeralMessage = ephemeralMessage;
/* 495:    */   }
/* 496:    */   
/* 497:    */   public Boolean getHasEntered()
/* 498:    */   {
/* 499:490 */     return this.hasEntered;
/* 500:    */   }
/* 501:    */   
/* 502:    */   public void setHasEntered(Boolean hasEntered)
/* 503:    */   {
/* 504:494 */     this.hasEntered = hasEntered;
/* 505:    */   }
/* 506:    */   
/* 507:    */   public Integer getNotifyId()
/* 508:    */   {
/* 509:498 */     return this.notifyId;
/* 510:    */   }
/* 511:    */   
/* 512:    */   public void setNotifyId(Integer notifyId)
/* 513:    */   {
/* 514:502 */     this.notifyId = notifyId;
/* 515:    */   }
/* 516:    */   
/* 517:    */   public Integer getLoiterSeconds()
/* 518:    */   {
/* 519:506 */     return this.loiterSeconds;
/* 520:    */   }
/* 521:    */   
/* 522:    */   public void setLoiterSeconds(Integer loiterSeconds)
/* 523:    */   {
/* 524:510 */     this.loiterSeconds = loiterSeconds;
/* 525:    */   }
/* 526:    */   
/* 527:    */   public Long getEntryTime()
/* 528:    */   {
/* 529:514 */     return this.entryTime;
/* 530:    */   }
/* 531:    */   
/* 532:    */   public void setEntryTime(Long entryTime)
/* 533:    */   {
/* 534:518 */     this.entryTime = entryTime;
/* 535:    */   }
/* 536:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.data.Message
 * JD-Core Version:    0.7.0.1
 */