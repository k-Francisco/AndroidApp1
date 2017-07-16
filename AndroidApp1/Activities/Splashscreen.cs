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
using Android.Support.V7.App;
using Android.Content.PM;
using AndroidApp1.Fragments;
using Android.Preferences;
using System.Threading;
using Android.Net;
using Java.Net;
using Android.Util;

namespace AndroidApp1.Activities
{
    [Activity(Label = "Project Server Mobile", 
        MainLauncher = true, 
        LaunchMode = LaunchMode.SingleTop, 
        Icon = "@drawable/Icon")]
    public class Splashscreen : AppCompatActivity
    {
        bool online;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_splash);

            this.Window.AddFlags(WindowManagerFlags.Fullscreen);

            

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.fragmentContainer, new PslogoFragment())
                .Commit();

            ThreadPool.QueueUserWorkItem(delegate (object state)
            {
                online = isOnline();
                Thread.Sleep(2000);
                checkCredentials();
            }, null);
        }

        private bool isOnline()
        {
            ConnectivityManager cm = (ConnectivityManager)GetSystemService(Context.ConnectivityService);
            NetworkInfo netInfo = cm.ActiveNetworkInfo;

            if (netInfo != null && netInfo.IsConnected) {
                try {
                    URL url = new URL("http://www.google.com");
                    HttpURLConnection urlc = (HttpURLConnection)url.OpenConnection();
                    urlc.ConnectTimeout = 3000;
                    urlc.Connect();
                    if (urlc.ResponseCode == HttpStatus.Ok) {
                        return true;
                    }
                }
                catch (Exception e) {
                    Log.Info("kfsama", e.Message);
                }
            }
            return false;
        }

        public void checkCredentials() {

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            if (prefs.GetString("rtFa", null) != null && prefs.GetString("FedAuth", null) != null)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("rtFa", prefs.GetString("rtFa", null));
                intent.PutExtra("FedAuth", prefs.GetString("FedAuth", null));
                intent.PutExtra("connection", online);
                StartActivity(intent);
                this.Finish();

            }
            else
            {
                if (online)
                {
                    SupportFragmentManager.BeginTransaction()
                    .Replace(Resource.Id.fragmentContainer, new Login())
                    .Commit();
                }
                else {
                    SupportFragmentManager.BeginTransaction()
                    .Replace(Resource.Id.fragmentContainer, new OfflineFragment())
                    .Commit();
                }
                
            }

        }
    }
}