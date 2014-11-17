/*  1:   */ package com.exacttarget.etpushsdk.util;
/*  2:   */ 
/*  3:   */ import com.j256.ormlite.field.FieldType;
/*  4:   */ import com.j256.ormlite.field.SqlType;
/*  5:   */ import com.j256.ormlite.field.types.StringType;
/*  6:   */ import java.lang.reflect.Field;
/*  7:   */ import java.sql.SQLException;
/*  8:   */ import java.util.Collection;
/*  9:   */ 
/* 10:   */ public class JsonType
/* 11:   */   extends StringType
/* 12:   */ {
/* 13:   */   private static JsonType singleton;
/* 14:   */   
/* 15:   */   public JsonType()
/* 16:   */   {
/* 17:16 */     super(SqlType.STRING, new Class[] { String.class });
/* 18:   */   }
/* 19:   */   
/* 20:   */   public static JsonType getSingleton()
/* 21:   */   {
/* 22:19 */     if (singleton == null) {
/* 23:20 */       singleton = new JsonType();
/* 24:   */     }
/* 25:22 */     return singleton;
/* 26:   */   }
/* 27:   */   
/* 28:   */   public boolean isValidForField(Field field)
/* 29:   */   {
/* 30:26 */     return Collection.class.isAssignableFrom(field.getType());
/* 31:   */   }
/* 32:   */   
/* 33:   */   public Object javaToSqlArg(FieldType fieldType, Object obj)
/* 34:   */     throws SQLException
/* 35:   */   {
/* 36:   */     ;
/* 37:31 */     if ("java.lang.String".equals(obj.getClass().getName()))
/* 38:   */     {
/* 39:33 */       fieldType = super.javaToSqlArg(fieldType, obj);
/* 40:   */     }
/* 41:   */     else
/* 42:   */     {
/* 43:36 */       String json = JSONUtil.objectToJson(obj);
/* 44:37 */       sqlArg = super.javaToSqlArg(sqlArg, json);
/* 45:   */     }
/* 46:40 */     return sqlArg;
/* 47:   */   }
/* 48:   */   
/* 49:   */   public Object sqlArgToJava(FieldType fieldType, Object sqlArg, int columnPos)
/* 50:   */     throws SQLException
/* 51:   */   {
/* 52:46 */     return JSONUtil.jsonToObject(fieldType = (String)super.sqlArgToJava(fieldType, sqlArg, columnPos), Object.class);
/* 53:   */   }
/* 54:   */   
/* 55:   */   public Object resultStringToJava(FieldType fieldType, String stringValue, int columnPos)
/* 56:   */     throws SQLException
/* 57:   */   {
/* 58:52 */     return JSONUtil.jsonToObject(fieldType = (String)super.resultStringToJava(fieldType, stringValue, columnPos), Object.class);
/* 59:   */   }
/* 60:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.util.JsonType
 * JD-Core Version:    0.7.0.1
 */