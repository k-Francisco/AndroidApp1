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
using System.Threading;
using Android.Util;

namespace AndroidApp1.Helpers
{
    class LoginWebViewClient : WebViewClient
    {

        private Splashscreen splash;
        private string rtFa = "", FedAuth = "";
        private bool isFed = false, isRtFa = false, isRedirected = false;

        public LoginWebViewClient(Splashscreen splash) {
            this.splash = splash;
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            if (isRedirected == true) {
                CookieManager cookieManager = CookieManager.Instance;
                cookieManager.SetAcceptCookie(true);
                string generateToken = cookieManager.GetCookie("https://sharepointevo.sharepoint.com/SitePages/home.aspx?AjaxDelta=1");
                String[] token = generateToken.Split(new char[] { ';' });

                    for (int i = 0; i < token.Length; i++)
                    {
                        if (token[i].Contains("rtFa"))
                        {
                            rtFa = token[i].Replace("rtFa=", "");
                            isRtFa = true;
                        }
                        if (token[i].Contains("FedAuth"))
                        {
                            FedAuth = token[i].Replace("FedAuth=", "");
                            isFed = true;
                        }

                        if (isFed && isRtFa)
                        {
                            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(splash);
                            ISharedPreferencesEditor editor = prefs.Edit();
                            editor.PutString("rtFa", rtFa);
                            editor.PutString("FedAuth", FedAuth);
                            editor.PutStringSet("SavedWork", new List<string> { });
                            editor.PutStringSet("SavedLine", new List<string> { });
                            editor.PutStringSet("SavedPeriod", new List<string> { });
                            editor.Apply();
                            splash.checkCredentials();
                        isRedirected = false;
                        }

                    }
            }
        }

        public override void OnPageFinished(WebView view, string url)
        {
            isRedirected = true;
        }

    }
}