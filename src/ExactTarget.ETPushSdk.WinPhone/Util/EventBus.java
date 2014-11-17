/*   1:    */ package com.exacttarget.etpushsdk.util;
/*   2:    */ 
/*   3:    */ import android.util.Log;
/*   4:    */ import com.exacttarget.etpushsdk.ETPush;
/*   5:    */ import java.lang.reflect.Method;
/*   6:    */ import java.util.ArrayList;
/*   7:    */ import java.util.HashMap;
/*   8:    */ import java.util.Iterator;
/*   9:    */ import java.util.List;
/*  10:    */ import java.util.Map;
/*  11:    */ 
/*  12:    */ public class EventBus
/*  13:    */ {
/*  14:    */   private static final String TAG = "etpushsdk@EventBus";
/*  15: 61 */   private static final EventBus eventBus = new EventBus();
/*  16: 63 */   private List<Object> listenerRegistry = new ArrayList();
/*  17: 64 */   private Map<String, List<ListenerMethod>> registry = new HashMap();
/*  18: 65 */   private Map<String, Object> stickyEvents = new HashMap();
/*  19:    */   
/*  20:    */   public static EventBus getDefault()
/*  21:    */   {
/*  22: 76 */     return eventBus;
/*  23:    */   }
/*  24:    */   
/*  25:    */   public void register(Object listener)
/*  26:    */   {
/*  27: 84 */     synchronized ("etpushsdk@EventBus")
/*  28:    */     {
/*  29: 85 */       if (!this.listenerRegistry.contains(listener))
/*  30:    */       {
/*  31: 86 */         this.listenerRegistry.add(listener);
/*  32:    */         Object localObject1;
/*  33:    */         Method[] arr$;
/*  34: 89 */         int len$ = (arr$ = localObject1 = (localObject1 = listener.getClass()).getMethods()).length;
/*  35: 89 */         for (int i$ = 0; i$ < len$; i$++)
/*  36:    */         {
/*  37:    */           Method method;
/*  38: 90 */           if ((method = arr$[i$]).getName().startsWith("onEvent"))
/*  39:    */           {
/*  40:    */             Object localObject2;
/*  41: 93 */             String eventType = (localObject2 = (localObject2 = method.getParameterTypes())[0]).getName();
/*  42: 94 */             if (!this.registry.containsKey(eventType)) {
/*  43: 95 */               this.registry.put(eventType, new ArrayList());
/*  44:    */             }
/*  45:    */             List localList;
/*  46: 98 */             (localList = (List)this.registry.get(eventType)).add(new ListenerMethod(listener, method, null));
/*  47:100 */             if (this.stickyEvents.containsKey(eventType))
/*  48:    */             {
/*  49:101 */               Object stickyEvent = this.stickyEvents.get(eventType);
/*  50:    */               try
/*  51:    */               {
/*  52:103 */                 if (ETPush.getLogLevel() <= 3) {
/*  53:104 */                   Log.d("etpushsdk@EventBus", "Calling: " + listener.getClass().getName() + " - " + method.getName());
/*  54:    */                 }
/*  55:106 */                 method.invoke(listener, new Object[] { stickyEvent });
/*  56:    */               }
/*  57:    */               catch (Throwable e)
/*  58:    */               {
/*  59:109 */                 if (ETPush.getLogLevel() <= 6) {
/*  60:110 */                   Log.e("etpushsdk@EventBus", e.getMessage(), e);
/*  61:    */                 }
/*  62:    */               }
/*  63:    */             }
/*  64:    */           }
/*  65:    */         }
/*  66:    */       }
/*  67:117 */       return;
/*  68:    */     }
/*  69:    */   }
/*  70:    */   
/*  71:    */   public void unregister(Object listener)
/*  72:    */   {
/*  73:125 */     synchronized ("etpushsdk@EventBus")
/*  74:    */     {
/*  75:126 */       if (this.listenerRegistry.contains(listener))
/*  76:    */       {
/*  77:127 */         this.listenerRegistry.remove(listener);
/*  78:    */         Object localObject1;
/*  79:    */         Method[] arr$;
/*  80:130 */         int len$ = (arr$ = localObject1 = (localObject1 = listener.getClass()).getMethods()).length;
/*  81:130 */         for (int i$ = 0; i$ < len$; i$++)
/*  82:    */         {
/*  83:    */           Method method;
/*  84:131 */           if ((method = arr$[i$]).getName().startsWith("onEvent"))
/*  85:    */           {
/*  86:134 */             String eventType = (method = (method = method.getParameterTypes())[0]).getName();
/*  87:135 */             if (this.registry.containsKey(eventType))
/*  88:    */             {
/*  89:136 */               List<ListenerMethod> listeners = (List)this.registry.get(eventType);
/*  90:137 */               List<ListenerMethod> toRemove = new ArrayList();
/*  91:138 */               for (Iterator i$ = listeners.iterator(); i$.hasNext();)
/*  92:    */               {
/*  93:    */                 ListenerMethod listenerMethod;
/*  94:139 */                 if ((listenerMethod = (ListenerMethod)i$.next()).listener.equals(listener)) {
/*  95:140 */                   toRemove.add(listenerMethod);
/*  96:    */                 }
/*  97:    */               }
/*  98:143 */               listeners.removeAll(toRemove);
/*  99:    */             }
/* 100:    */           }
/* 101:    */         }
/* 102:    */       }
/* 103:148 */       return;
/* 104:    */     }
/* 105:    */   }
/* 106:    */   
/* 107:    */   public void post(Object event)
/* 108:    */   {
/* 109:156 */     synchronized ("etpushsdk@EventBus")
/* 110:    */     {
/* 111:    */       List<ListenerMethod> listeners;
/* 112:158 */       if ((listeners = (List)this.registry.get(event.getClass().getName())) != null) {
/* 113:159 */         for (ListenerMethod listener : listeners) {
/* 114:    */           try
/* 115:    */           {
/* 116:161 */             if (ETPush.getLogLevel() <= 3) {
/* 117:162 */               Log.d("etpushsdk@EventBus", "Calling: " + listener.listener.getClass().getName() + " - " + listener.method.getName());
/* 118:    */             }
/* 119:164 */             listener.method.invoke(listener.listener, new Object[] { event });
/* 120:    */           }
/* 121:    */           catch (Throwable e)
/* 122:    */           {
/* 123:167 */             if (ETPush.getLogLevel() <= 6) {
/* 124:168 */               Log.e("etpushsdk@EventBus", e.getMessage(), e);
/* 125:    */             }
/* 126:    */           }
/* 127:    */         }
/* 128:    */       }
/* 129:173 */       return;
/* 130:    */     }
/* 131:    */   }
/* 132:    */   
/* 133:    */   public void postSticky(Object event)
/* 134:    */   {
/* 135:182 */     post(event);
/* 136:    */     
/* 137:    */ 
/* 138:185 */     this.stickyEvents.put(event.getClass().getName(), event);
/* 139:    */   }
/* 140:    */   
/* 141:    */   private class ListenerMethod
/* 142:    */   {
/* 143:    */     public Object listener;
/* 144:    */     public Method method;
/* 145:    */     
/* 146:    */     private ListenerMethod(Object listener, Method method)
/* 147:    */     {
/* 148:192 */       this.listener = listener;
/* 149:193 */       this.method = method;
/* 150:    */     }
/* 151:    */   }
/* 152:    */   
/* 153:    */   public Object getStickyEvent(Class<?> clazz)
/* 154:    */   {
/* 155:203 */     return this.stickyEvents.get(clazz.getName());
/* 156:    */   }
/* 157:    */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.util.EventBus
 * JD-Core Version:    0.7.0.1
 */