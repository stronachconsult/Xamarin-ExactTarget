/*   1:    */ package com.exacttarget.etpushsdk;
/*   2:    */ 
/*   3:    */ import android.content.Context;
/*   4:    */ import android.content.Intent;
/*   5:    */ import android.os.Handler;
/*   6:    */ import android.util.Log;
/*   7:    */ import com.exacttarget.etpushsdk.data.CloudPagesResponse;
/*   8:    */ import com.exacttarget.etpushsdk.data.ETSqliteOpenHelper;
/*   9:    */ import com.exacttarget.etpushsdk.data.Message;
/*  10:    */ import com.exacttarget.etpushsdk.event.BackgroundEvent;
/*  11:    */ import com.exacttarget.etpushsdk.event.CloudPagesChangedEvent;
/*  12:    */ import com.exacttarget.etpushsdk.util.EventBus;
/*  13:    */ import com.j256.ormlite.dao.Dao;
/*  14:    */ import java.sql.SQLException;
/*  15:    */ import java.util.ArrayList;
/*  16:    */ 
/*  17:    */ public class ETCloudPageManager
/*  18:    */ {
/*  19: 34 */   private static final String TAG = ETCloudPageManager.class.getSimpleName();
/*  20:    */   private static ETCloudPageManager cloudPageManager;
/*  21:    */   private Context applicationContext;
/*  22:    */   
/*  23:    */   private ETCloudPageManager(Context applicationContext)
/*  24:    */   {
/*  25: 41 */     this.applicationContext = applicationContext;
/*  26:    */     
/*  27: 43 */     EventBus.getDefault().register(this);
/*  28:    */   }
/*  29:    */   
/*  30:    */   public static ETCloudPageManager cloudPageManager()
/*  31:    */     throws ETException
/*  32:    */   {
/*  33: 51 */     if (Config.isCloudPagesActive())
/*  34:    */     {
/*  35: 52 */       if (cloudPageManager == null) {
/*  36: 53 */         throw new ETException("You forgot to call readyAimFire first.");
/*  37:    */       }
/*  38: 56 */       return cloudPageManager;
/*  39:    */     }
/*  40: 59 */     throw new ETException("ETCloudPageManager disabled.");
/*  41:    */   }
/*  42:    */   
/*  43:    */   protected static void resetCloudPages(Context applicationContext)
/*  44:    */   {
/*  45: 68 */     cloudPageManager = null;
/*  46:    */   }
/*  47:    */   
/*  48:    */   protected static void readyAimFire(Context applicationContext)
/*  49:    */     throws ETException
/*  50:    */   {
/*  51: 76 */     if (cloudPageManager == null)
/*  52:    */     {
/*  53: 77 */       if (ETPush.getLogLevel() <= 3) {
/*  54: 78 */         Log.d(TAG, "readyAimFire()");
/*  55:    */       }
/*  56: 80 */       cloudPageManager = new ETCloudPageManager(applicationContext);return;
/*  57:    */     }
/*  58: 83 */     throw new ETException("You must have called readyAimFire more than once.");
/*  59:    */   }
/*  60:    */   
/*  61:    */   public void onEventBackgroundEvent(BackgroundEvent event)
/*  62:    */   {
/*  63: 94 */     if ((Config.isCloudPagesActive()) && 
/*  64: 95 */       (!event.isInBackground()))
/*  65:    */     {
/*  66: 97 */       if (ETPush.getLogLevel() <= 3) {
/*  67: 98 */         Log.d(TAG, "In FOREGROUND");
/*  68:    */       }
/*  69:100 */       refreshData();
/*  70:    */     }
/*  71:    */   }
/*  72:    */   
/*  73:    */   public void onEventCloudPagesResponse(CloudPagesResponse response)
/*  74:    */   {
/*  75:112 */     final ETSqliteOpenHelper helper = ETSqliteOpenHelper.getHelper(this.applicationContext);
/*  76:    */     try
/*  77:    */     {
/*  78:114 */       Dao<Message, String> messageDao = helper.getMessageDao();
/*  79:115 */       for (Message message : response.getMessages())
/*  80:    */       {
/*  81:    */         Message dbMessage;
/*  82:117 */         if ((dbMessage = (Message)messageDao.queryForId(message.getId())) != null)
/*  83:    */         {
/*  84:119 */           message.setRead(dbMessage.getRead());
/*  85:    */           
/*  86:121 */           message.setMessageDeleted(dbMessage.getMessageDeleted());
/*  87:    */           
/*  88:123 */           messageDao.update(message);
/*  89:    */         }
/*  90:    */         else
/*  91:    */         {
/*  92:126 */           messageDao.create(message);
/*  93:    */         }
/*  94:    */       }
/*  95:129 */       if ((response.getMessages() != null) && (!response.getMessages().isEmpty())) {
/*  96:131 */         EventBus.getDefault().post(new CloudPagesChangedEvent());
/*  97:    */       }
/*  98:    */     }
/*  99:    */     catch (SQLException e)
/* 100:    */     {
/* 101:135 */       if (ETPush.getLogLevel() <= 6) {
/* 102:136 */         Log.e(TAG, e.getMessage(), e);
/* 103:    */       }
/* 104:    */     }
/* 105:    */     finally
/* 106:    */     {
/* 107:141 */       (e = new Handler(this.applicationContext.getMainLooper())).postDelayed(new Runnable()
/* 108:    */       {
/* 109:    */         public void run()
/* 110:    */         {
/* 111:144 */           if ((helper != null) && (helper.isOpen())) {
/* 112:145 */             helper.close();
/* 113:    */           }
/* 114:    */         }
/* 115:145 */       }, 10000L);
/* 116:    */     }
/* 117:    */   }
/* 118:    */   
/* 119:    */   public void refreshData()
/* 120:    */   {
/* 121:    */     Intent sendDataIntent;
/* 122:157 */     (sendDataIntent = new Intent(this.applicationContext, ETSendDataReceiver.class)).putExtra("et_send_type_extra", "et_send_type_cloudpage");
/* 123:158 */     this.applicationContext.sendBroadcast(sendDataIntent);
/* 124:    */   }
/* 125:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.ETCloudPageManager
 * JD-Core Version:    0.7.0.1
 */