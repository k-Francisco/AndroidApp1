using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using AndroidApp1.Helpers;
using AndroidApp1.Activities;

namespace AndroidApp1.Fragments
{
    public class Login : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.login_screen, container, false);

            WebView webView = rootView.FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.DomStorageEnabled = true;
            webView.ClearCache(true);
            CookieManager.Instance.RemoveSessionCookie();
            webView.LoadUrl("https://sharepointevo.sharepoint.com");
            webView.SetWebViewClient(new LoginWebViewClient(Activity as Splashscreen));

            return rootView;

            
        }
    }
}