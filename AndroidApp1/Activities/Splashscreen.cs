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

namespace AndroidApp1.Activities
{
    [Activity(Label = "Project Server Mobile", 
        MainLauncher = true, 
        LaunchMode = LaunchMode.SingleTop, 
        Icon = "@drawable/Icon")]
    public class Splashscreen : AppCompatActivity
    {
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
                Thread.Sleep(2000);
                checkCredentials();
            }, null);
        }

        public void checkCredentials() {

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            if (prefs.GetString("rtFa", null) != null && prefs.GetString("FedAuth", null) != null)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("rtFa", prefs.GetString("rtFa", null));
                intent.PutExtra("FedAuth", prefs.GetString("FedAuth", null));
                StartActivity(intent);
                this.Finish();

            }
            else
            {
                SupportFragmentManager.BeginTransaction()
                    .Replace(Resource.Id.fragmentContainer, new Login())
                    .Commit();
            }

        }
    }
}