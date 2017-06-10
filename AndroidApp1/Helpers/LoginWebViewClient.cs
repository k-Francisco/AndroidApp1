using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using AndroidApp1.Activities;
using Android.Graphics;
using Android.Preferences;

namespace AndroidApp1.Helpers
{
    class LoginWebViewClient : WebViewClient
    {

        private Splashscreen splash;
        private string rtFa = "", FedAuth = "";
        bool isFed = false, isRtFa = false;

        public LoginWebViewClient(Splashscreen splash) {
            this.splash = splash;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            return base.ShouldOverrideUrlLoading(view, url);
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            CookieManager cookieManager = CookieManager.Instance;
            string generateToken = cookieManager.GetCookie("https://sharepointevo.sharepoint.com/SitePages/home.aspx?AjaxDelta=1");

            String[] token = generateToken.Split(new char[] { ';' });

            for (int i = 0; i < token.Length; i++) {
                if (token[i].Contains("rtFa")) {
                    rtFa = token[i].Replace("rtFa=","");
                    isRtFa = true;
                }
                if (token[i].Contains("FedAuth")) {
                    FedAuth = token[i].Replace("FedAuth=","");
                    isFed = true;
                }

                if (isFed && isRtFa) {
                    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(splash);
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString("rtFa", rtFa);
                    editor.PutString("FedAuth", FedAuth);
                    editor.Apply();
                    splash.checkCredentials();

                }

            }
        }

    }
}