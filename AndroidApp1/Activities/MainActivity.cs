using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;

using AndroidApp1.Fragments;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using System;
using Android.Content;
using Android.Util;
using System.Net.Http;
using System.Net;
using Android.Preferences;
using Newtonsoft.Json;
using PScore;
using System.Threading.Tasks;
using AndroidApp1.Helpers;
using System.Threading;

namespace AndroidApp1.Activities
{
    [Activity(Label = "Home")]
    public class MainActivity : BaseActivity
    {
        //fragments
        private static ProjectFragment projectFragment = new ProjectFragment();
        private static LoaderFragment loader = new LoaderFragment();
        private static DialogHelpers helpDialog = new DialogHelpers();

        //data
        private ProjectModel.RootObject projects;

        //core
        private PsCore core;

        //constants
        private const int PROJECT_DATA = 1;



        FloatingActionButton fab;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        ISharedPreferences prefs;
        

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.main;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            init(savedInstanceState); 
            
        }

        private async void init(Bundle savedInstanceState)
        {
            drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //Set hamburger items menu
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            //setup navigation view
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetCheckedItem(Resource.Id.nav_home_1);
            TextView userName = navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.tvName);
            TextView userEmail = navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.tvEmail);

            //handle navigation
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_home_1:
                        ListItemClicked(0);
                        break;
                    case Resource.Id.nav_home_2:
                        ListItemClicked(1);
                        break;
                    case Resource.Id.nav_home_3:
                        ListItemClicked(2);
                        break;
                    case Resource.Id.nav_home_4:
                        ListItemClicked(3);
                        break;
                    case Resource.Id.nav_home_5:
                        ListItemClicked(4);
                        break;
                    case Resource.Id.nav_home_6:
                        ListItemClicked(5);
                        break;
                }
                drawerLayout.CloseDrawers();
            };
            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                ListItemClicked(0);
            }

            
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += delegate {
                View view = LayoutInflater.Inflate(Resource.Layout.add_project_dialog, null);
                Android.Support.V7.App.AlertDialog addProjectDialog = helpDialog.AddProjectDialog(this, view);
                addProjectDialog.Show();
            };

            //setting the cookies
            var rtFa = Intent.GetStringExtra("rtFa");
            var FedAuth = Intent.GetStringExtra("FedAuth");
            core = new PsCore(rtFa, FedAuth);

            //setting formDigest
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            if (prefs.GetString("formDigest", null) == null)
            {
                core.setClient();
                string context = await core.GetFormDigest("");
                var data = JsonConvert.DeserializeObject<AndroidApp1.FormDigestModel.RootObject>(context);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString("formDigest", data.D.GetContextWebInformation.FormDigestValue);
                editor.Apply();
                core.setClient2(data.D.GetContextWebInformation.FormDigestValue, 0);
            }
            else {
                Log.Info("kfsama", prefs.GetString("formDigest",null));
                Log.Info("kfsama", rtFa);
                Log.Info("kfsama", FedAuth);
                core.setClient();
                core.setClient2(prefs.GetString("formDigest", null), 0);
            }

            ThreadPool.QueueUserWorkItem(async state =>
            {
                var data = await core.GetCurrentUser();
                var currentUser = JsonConvert.DeserializeObject<CurrentUser.RootObject>(data);
                RunOnUiThread(() => {
                    userName.Text = currentUser.D.Title;
                    userEmail.Text = currentUser.D.Email;
                });
            });

            checkDataAsync(PROJECT_DATA);
            switchFragment(loader);

        }

        int oldPosition = -1;
        private void ListItemClicked(int position)
        {
            //this way we don't load twice, but you might want to modify this a bit.
            if (position == oldPosition)
                return;

            oldPosition = position;

            
            switch (position)
            {
                case 0:
                    switchFragment(projectFragment);
                    SupportActionBar.Title = "Projects";
                    //checkData(PROJECT_DATA);
                    break;
                case 1:
                    switchFragment(loader);
                    SupportActionBar.Title = "Approvals";
                    break;
                case 2:
                    SupportActionBar.Title = "My Tasks";
                    break;
                case 3:
                    SupportActionBar.Title = "Timesheets";
                    break;
                case 4:
                    SupportActionBar.Title = "Settings";
                    break;
                case 5:
                    prefs.Edit().Clear().Apply();
                    Splashscreen splash = new Splashscreen();
                    Intent intent = new Intent(this, typeof(Splashscreen));
                    StartActivity(intent);
                    this.Finish();
                    break;
            }
        }

        //UI stuff
        Android.Support.V4.App.Fragment oldFragment = projectFragment;
        private void switchFragment(Android.Support.V4.App.Fragment fragment) {

            if (oldFragment == fragment)
                return;

            oldFragment = fragment;

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        public void seeDetails() {
            Intent intent = new Intent(this, typeof(DetailsActivity));
            StartActivity(intent);
        }

        //UI stuff end

        //data stuff

        public PsCore getCore() {
            return core;
        }

        public string getFormDigest() {
            return prefs.GetString("formDigest",null);
        }

        public void checkDataAsync(int whatData)
        {

            switch (whatData)
            {
                case 1:

                    if (projects == null)
                        fillDataAsync(whatData);
                    else
                        switchFragment(projectFragment);

                    break;
            }
        }

        public async void fillDataAsync(int whatData)
        {
            //1 for projects
            if (whatData == 1)
            {
                var data = await core.GetProjects();
                ThreadPool.QueueUserWorkItem(state =>
                {
                    projects = JsonConvert.DeserializeObject<ProjectModel.RootObject>(data);
                    RunOnUiThread(() => switchFragment(projectFragment));
                });
            }

        }

        

        public ProjectModel.RootObject getProjectList() {
            return projects;
        }
        //data stuff end


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}

