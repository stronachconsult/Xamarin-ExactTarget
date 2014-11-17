/*  1:   */ package com.exacttarget.etpushsdk;
/*  2:   */ 
/*  3:   */ import android.app.Activity;
/*  4:   */ import android.content.Intent;
/*  5:   */ import android.os.Bundle;
/*  6:   */ import android.webkit.WebSettings;
/*  7:   */ import android.webkit.WebView;
/*  8:   */ import android.webkit.WebViewClient;
/*  9:   */ import android.widget.LinearLayout;
/* 10:   */ import android.widget.LinearLayout.LayoutParams;
/* 11:   */ 
/* 12:   */ public class ETLandingPagePresenter
/* 13:   */   extends Activity
/* 14:   */ {
/* 15:   */   public void onCreate(Bundle savedInstanceState)
/* 16:   */   {
/* 17:46 */     super.onCreate(savedInstanceState);
/* 18:47 */     setTitle("Loading...");
/* 19:   */     LinearLayout ll;
/* 20:50 */     (ll = new LinearLayout(this)).setOrientation(1);
/* 21:51 */     ll.setLayoutParams(new LinearLayout.LayoutParams(-1, -1));
/* 22:52 */     ll.setGravity(17);
/* 23:   */     String website;
/* 24:   */     String website;
/* 25:56 */     if (getIntent().getExtras().containsKey("_x"))
/* 26:   */     {
/* 27:57 */       website = getIntent().getExtras().getString("_x");
/* 28:   */     }
/* 29:   */     else
/* 30:   */     {
/* 31:   */       String website;
/* 32:59 */       if (getIntent().getExtras().containsKey("_od"))
/* 33:   */       {
/* 34:60 */         website = getIntent().getExtras().getString("_od");
/* 35:   */       }
/* 36:   */       else
/* 37:   */       {
/* 38:63 */         website = null;
/* 39:64 */         setTitle("No website URL found in payload.");
/* 40:   */       }
/* 41:   */     }
/* 42:67 */     if (website != null)
/* 43:   */     {
/* 44:   */       WebView webView;
/* 45:69 */       (webView = new WebView(this)).loadUrl(website);
/* 46:70 */       webView.getSettings().setJavaScriptEnabled(true);
/* 47:71 */       ll.addView(webView);
/* 48:   */       
/* 49:73 */       webView.setWebViewClient(new WebViewClient()
/* 50:   */       {
/* 51:   */         public void onPageFinished(WebView view, String url)
/* 52:   */         {
/* 53:77 */           ETLandingPagePresenter.this.setTitle(view.getTitle());
/* 54:   */         }
/* 55:   */       });
/* 56:   */     }
/* 57:82 */     setContentView(ll);
/* 58:   */   }
/* 59:   */ }


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.ETLandingPagePresenter
 * JD-Core Version:    0.7.0.1
 */