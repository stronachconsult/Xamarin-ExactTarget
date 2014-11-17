/*   1:    */ package com.exacttarget.etpushsdk.adapter;
/*   2:    */ 
/*   3:    */ import android.content.Context;
/*   4:    */ import android.os.Handler;
/*   5:    */ import android.util.Log;
/*   6:    */ import android.widget.BaseAdapter;
/*   7:    */ import android.widget.ListAdapter;
/*   8:    */ import com.exacttarget.etpushsdk.ETPush;
/*   9:    */ import com.exacttarget.etpushsdk.data.ETSqliteOpenHelper;
/*  10:    */ import com.exacttarget.etpushsdk.data.Message;
/*  11:    */ import com.exacttarget.etpushsdk.event.CloudPagesChangedEvent;
/*  12:    */ import com.exacttarget.etpushsdk.util.EventBus;
/*  13:    */ import com.j256.ormlite.dao.Dao;
/*  14:    */ import com.j256.ormlite.stmt.QueryBuilder;
/*  15:    */ import com.j256.ormlite.stmt.Where;
/*  16:    */ import java.sql.SQLException;
/*  17:    */ import java.util.ArrayList;
/*  18:    */ import java.util.Date;
/*  19:    */ import java.util.List;
/*  20:    */ 
/*  21:    */ public abstract class CloudPageListAdapter
/*  22:    */   extends BaseAdapter
/*  23:    */   implements ListAdapter
/*  24:    */ {
/*  25:    */   private static final String TAG = "etpushsdk@CloudPageListAdapter";
/*  26:    */   private Context applicationContext;
/*  27:    */   public static final int DISPLAY_ALL = 0;
/*  28:    */   public static final int DISPLAY_UNREAD = 1;
/*  29:    */   public static final int DISPLAY_READ = 2;
/*  30:    */   private int display;
/*  31: 49 */   private List<Message> allMessages = new ArrayList();
/*  32: 50 */   private List<Message> unreadMessages = new ArrayList();
/*  33: 51 */   private List<Message> readMessages = new ArrayList();
/*  34:    */   private Handler uiHandler;
/*  35:    */   
/*  36:    */   public CloudPageListAdapter(Context appContext)
/*  37:    */   {
/*  38: 57 */     this.applicationContext = appContext;
/*  39: 58 */     this.uiHandler = new Handler();
/*  40: 59 */     EventBus.getDefault().register(this);
/*  41: 60 */     reloadData();
/*  42:    */   }
/*  43:    */   
/*  44:    */   protected void finalize()
/*  45:    */     throws Throwable
/*  46:    */   {
/*  47: 65 */     EventBus.getDefault().unregister(this);
/*  48: 66 */     super.finalize();
/*  49:    */   }
/*  50:    */   
/*  51:    */   public void onEvent(CloudPagesChangedEvent event)
/*  52:    */   {
/*  53: 75 */     this.uiHandler.post(new Runnable()
/*  54:    */     {
/*  55:    */       public void run()
/*  56:    */       {
/*  57: 77 */         CloudPageListAdapter.this.notifyDataSetChanged();
/*  58:    */       }
/*  59:    */     });
/*  60:    */   }
/*  61:    */   
/*  62:    */   public int getCount()
/*  63:    */   {
/*  64: 86 */     int count = 0;
/*  65: 88 */     switch (this.display)
/*  66:    */     {
/*  67:    */     case 0: 
/*  68: 90 */       count = this.allMessages.size();
/*  69: 91 */       break;
/*  70:    */     case 1: 
/*  71: 93 */       count = this.unreadMessages.size();
/*  72: 94 */       break;
/*  73:    */     case 2: 
/*  74: 96 */       count = this.readMessages.size();
/*  75:    */     }
/*  76:100 */     return count;
/*  77:    */   }
/*  78:    */   
/*  79:    */   public Object getItem(int position)
/*  80:    */   {
/*  81:107 */     Message message = null;
/*  82:108 */     switch (this.display)
/*  83:    */     {
/*  84:    */     case 0: 
/*  85:110 */       message = (Message)this.allMessages.get(position);
/*  86:111 */       break;
/*  87:    */     case 1: 
/*  88:113 */       message = (Message)this.unreadMessages.get(position);
/*  89:114 */       break;
/*  90:    */     case 2: 
/*  91:116 */       message = (Message)this.readMessages.get(position);
/*  92:    */     }
/*  93:120 */     return message;
/*  94:    */   }
/*  95:    */   
/*  96:    */   public long getItemId(int position)
/*  97:    */   {
/*  98:128 */     return position;
/*  99:    */   }
/* 100:    */   
/* 101:    */   public void notifyDataSetChanged()
/* 102:    */   {
/* 103:133 */     reloadData();
/* 104:    */     
/* 105:135 */     super.notifyDataSetChanged();
/* 106:    */   }
/* 107:    */   
/* 108:    */   private void reloadData()
/* 109:    */   {
/* 110:139 */     this.allMessages.clear();
/* 111:140 */     this.unreadMessages.clear();
/* 112:141 */     this.readMessages.clear();
/* 113:    */     
/* 114:143 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 115:    */     try
/* 116:    */     {
/* 117:    */       QueryBuilder<Message, String> queryBuilder;
/* 118:146 */       (queryBuilder = helper.getMessageDao().queryBuilder()).where().eq("message_type", Message.MESSAGE_TYPE_BASIC).and().eq("content_type", Message.CONTENT_TYPE_PAGE).and().eq("message_deleted", Boolean.FALSE);
/* 119:    */       
/* 120:    */ 
/* 121:    */ 
/* 122:    */ 
/* 123:    */ 
/* 124:152 */       queryBuilder.orderBy("start_date", false);
/* 125:    */       
/* 126:154 */       List<Message> messages = helper.getMessageDao().query(queryBuilder.prepare());
/* 127:155 */       this.allMessages.addAll(messages);
/* 128:156 */       now = new Date();
/* 129:157 */       for (Message message : messages) {
/* 130:158 */         if ((Message.MESSAGE_TYPE_BASIC.equals(message.getMessageType())) && (Message.CONTENT_TYPE_PAGE.equals(message.getContentType())) && ((message.getEndDate() == null) || (message.getEndDate().after(now)))) {
/* 131:159 */           if (message.getRead().booleanValue()) {
/* 132:160 */             this.readMessages.add(message);
/* 133:    */           } else {
/* 134:163 */             this.unreadMessages.add(message);
/* 135:    */           }
/* 136:    */         }
/* 137:    */       }
/* 138:    */     }
/* 139:    */     catch (SQLException e)
/* 140:    */     {
/* 141:169 */       if (ETPush.getLogLevel() <= 6) {
/* 142:170 */         Log.e("etpushsdk@CloudPageListAdapter", e.getMessage(), e);
/* 143:    */       }
/* 144:    */     }
/* 145:    */     finally
/* 146:    */     {
/* 147:    */       Date now;
/* 148:175 */       (now = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 149:    */       {
/* 150:    */         public void run()
/* 151:    */         {
/* 152:178 */           if ((helper != null) && (helper.isOpen())) {
/* 153:179 */             helper.close();
/* 154:    */           }
/* 155:    */         }
/* 156:179 */       }, 10000L);
/* 157:    */     }
/* 158:    */   }
/* 159:    */   
/* 160:    */   public int getDisplay()
/* 161:    */   {
/* 162:187 */     return this.display;
/* 163:    */   }
/* 164:    */   
/* 165:    */   public void setDisplay(int display)
/* 166:    */   {
/* 167:196 */     if (this.display != display)
/* 168:    */     {
/* 169:197 */       this.display = display;
/* 170:198 */       notifyDataSetChanged();
/* 171:    */     }
/* 172:    */   }
/* 173:    */   
/* 174:    */   public void setMessageRead(Message message)
/* 175:    */   {
/* 176:207 */     message.setRead(Boolean.TRUE);
/* 177:208 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 178:    */     try
/* 179:    */     {
/* 180:210 */       helper.getMessageDao().update(message);
/* 181:211 */       notifyDataSetChanged();
/* 182:    */     }
/* 183:    */     catch (SQLException e)
/* 184:    */     {
/* 185:214 */       if (ETPush.getLogLevel() <= 6) {
/* 186:215 */         Log.e("etpushsdk@CloudPageListAdapter", e.getMessage(), e);
/* 187:    */       }
/* 188:    */     }
/* 189:    */     finally
/* 190:    */     {
/* 191:    */       Handler localHandler;
/* 192:220 */       (localHandler = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 193:    */       {
/* 194:    */         public void run()
/* 195:    */         {
/* 196:223 */           if ((helper != null) && (helper.isOpen())) {
/* 197:224 */             helper.close();
/* 198:    */           }
/* 199:    */         }
/* 200:224 */       }, 10000L);
/* 201:    */     }
/* 202:    */   }
/* 203:    */   
/* 204:    */   public void setMessageUnread(Message message)
/* 205:    */   {
/* 206:236 */     message.setRead(Boolean.FALSE);
/* 207:237 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 208:    */     try
/* 209:    */     {
/* 210:239 */       helper.getMessageDao().update(message);
/* 211:240 */       notifyDataSetChanged();
/* 212:    */     }
/* 213:    */     catch (SQLException e)
/* 214:    */     {
/* 215:243 */       if (ETPush.getLogLevel() <= 6) {
/* 216:244 */         Log.e("etpushsdk@CloudPageListAdapter", e.getMessage(), e);
/* 217:    */       }
/* 218:    */     }
/* 219:    */     finally
/* 220:    */     {
/* 221:    */       Handler localHandler;
/* 222:249 */       (localHandler = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 223:    */       {
/* 224:    */         public void run()
/* 225:    */         {
/* 226:252 */           if ((helper != null) && (helper.isOpen())) {
/* 227:253 */             helper.close();
/* 228:    */           }
/* 229:    */         }
/* 230:253 */       }, 10000L);
/* 231:    */     }
/* 232:    */   }
/* 233:    */   
/* 234:    */   public void deleteMessage(Message message)
/* 235:    */   {
/* 236:265 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/* 237:    */     try
/* 238:    */     {
/* 239:267 */       message.setMessageDeleted(Boolean.TRUE);
/* 240:268 */       helper.getMessageDao().update(message);
/* 241:269 */       notifyDataSetChanged();
/* 242:    */     }
/* 243:    */     catch (SQLException e)
/* 244:    */     {
/* 245:272 */       if (ETPush.getLogLevel() <= 6) {
/* 246:273 */         Log.e("etpushsdk@CloudPageListAdapter", e.getMessage(), e);
/* 247:    */       }
/* 248:    */     }
/* 249:    */     finally
/* 250:    */     {
/* 251:    */       Handler localHandler;
/* 252:278 */       (localHandler = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 253:    */       {
/* 254:    */         public void run()
/* 255:    */         {
/* 256:281 */           if ((helper != null) && (helper.isOpen())) {
/* 257:282 */             helper.close();
/* 258:    */           }
/* 259:    */         }
/* 260:282 */       }, 10000L);
/* 261:    */     }
/* 262:    */   }
/* 263:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.adapter.CloudPageListAdapter
 * JD-Core Version:    0.7.0.1
 */