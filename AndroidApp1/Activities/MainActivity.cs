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
using Android.Runtime;
using System.Collections.Generic;
using AndroidApp1.Models.OfflineTimesheet;

namespace AndroidApp1.Activities
{
    [Activity(Label = "Projects")]
    public class MainActivity : BaseActivity
    {
        //fragments
        private static ProjectFragment projectFragment = new ProjectFragment();
        private static LoaderFragment loader = new LoaderFragment();
        private static TasksFragment taskFragment = new TasksFragment();
        private static TimesheetFragment timesheetFragment = new TimesheetFragment();
        private static SavedTimesheets savedTimesheets = new SavedTimesheets();
        private static EnterpriseResourcesFragment enterpriseResourceFragment = new EnterpriseResourcesFragment();
        private static SavedTimesheets savedTimesheetFragment = new SavedTimesheets();
        public DialogHelpers helpDialog { get; set; }


        //data
        private ProjectModel.RootObject projects;
        private ProjectData.RootObject pServer;
        private List<Taskmodel.RootObject> tasks;
        private List<string> projectsWithTasks = new List<string> { };
        private TimesheetPeriod.RootObject timesheetPeriods;
        public EnterpriseResources.RootObject enterpriseResources { get; set; }
        public List<string> tasksWithResource { get; set; }

        //core
        public PsCore core { get; set; }

        //constants
        private const int PROJECT_DATA = 1;
        private const int TASKS_DATA = 2;
        private const int TIMESHEET_DATA = 3;
        private const int RESOURCES_DATA = 4;
        private const int OFFLINE_DATA = 5;

        //json strings
        string projectJson;
        List<string> taskJson = new List<string> { };

        //ints
        private int tbModified = 0;
        private int refreshIdentifier = 1;
        private int fabfunctionidentifier = 1;

        //booleans
        bool willSwitch = false;
        bool online;
        Boolean hasAccess;

        //strings
        public string mText { get; set; }
        

        //cancellation token
        CancellationTokenSource cts = new CancellationTokenSource();

        //azure mobile app service
        public AzureDataServices service { get; set; }
        public IEnumerable<OfflineTimesheetModel> offline { get; set; }


        FloatingActionButton fab;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        SwipeRefreshLayout refresh;
        public ISharedPreferences prefs { get; set; }
        TextView userName;



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
            mText = "The device is offline.Please connect to the internet and restart the app";
            initOfflineSupportAsync();
            init(savedInstanceState);
           
        }

        private async void initOfflineSupportAsync()
        {
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            service = new AzureDataServices();
            await service.Initialize();
            await service.SyncData(online);
            
            offline = await service.pullData(online, userName.Text);
        }

        private async void init(Bundle savedInstanceState)
        {
            drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //Set hamburger items menu
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            //setup navigation view
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetCheckedItem(Resource.Id.nav_home_1);
            userName = navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.tvName);
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
            //if (savedInstanceState == null)
            //{
            //    ListItemClicked(0);
            //}

            
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.SetVisibility(ViewStates.Gone);
            fab.Click += delegate {
                fabFunctions();
            };

            refresh = FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresh.SetColorScheme(Resource.Color.refresher_1, Resource.Color.refresher_2);
            refresh.Refresh += delegate {
                refreshData(refreshIdentifier);
            };

            //setting the cookies
            var rtFa = Intent.GetStringExtra("rtFa");
            var FedAuth = Intent.GetStringExtra("FedAuth");
            online = Intent.GetBooleanExtra("connection", false);
            core = new PsCore(rtFa, FedAuth);
            helpDialog = new DialogHelpers();

            //setting formDigest
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            core.setClient();
            if (online)
            {
                string context = await core.GetFormDigest("");
                ThreadPool.QueueUserWorkItem(state =>
                {
                    try
                    {
                        var data = JsonConvert.DeserializeObject<AndroidApp1.FormDigestModel.RootObject>(context);
                        core.setClient2(data.D.GetContextWebInformation.FormDigestValue);
                        core.FormDigest = data.D.GetContextWebInformation.FormDigestValue;
                    }
                    catch (Exception e)
                    {
                        RunOnUiThread(() =>
                        {
                            mText = "You have no acces on this site";
                            switchFragment(new OfflineFragment());
                            willSwitch = true;
                        });
                    }

                });

                ThreadPool.QueueUserWorkItem(async state =>
                {
                    var data2 = await core.GetCurrentUser();
                    var currentUser = JsonConvert.DeserializeObject<CurrentUser.RootObject>(data2);
                    RunOnUiThread(async () =>
                   {
                       userName.Text = currentUser.D.Title;
                       userEmail.Text = currentUser.D.Email;
                       if (prefs.GetString("userName", "").Equals("") && prefs.GetString("email", "").Equals("")) {
                           ISharedPreferencesEditor editor = prefs.Edit();
                           editor.PutString("userName", currentUser.D.Title);
                           editor.PutString("email", currentUser.D.Email);
                           editor.Apply();
                           //prefs.Edit().PutString("userName", currentUser.D.Title).Apply();
                           //prefs.Edit().PutString("email", currentUser.D.Email).Apply();
                       }
                       hasAccess = await checkUserAccess(currentUser.D.Title);
                       if (hasAccess)
                       {
                           
                           checkDataAsync(PROJECT_DATA);
                       }
                       else
                       {
                           mText = "You have no acces on this site";
                           switchFragment(new OfflineFragment());
                           willSwitch = true;
                       }

                   });
                });
            }
            else {
                if (!prefs.GetString("userName", "").Equals(null) && !prefs.GetString("email", "").Equals(null))
                {
                    userName.Text = prefs.GetString("userName", "");
                    userEmail.Text = prefs.GetString("email", "");
                }
                switchFragment(new OfflineFragment());
                willSwitch = true;
            }
            
            tasksWithResource = new List<string> { };
            //checkDataAsync(PROJECT_DATA);

        }

        private async Task<bool> checkUserAccess(string user)
        {
            bool hasAccess = false;
            try {
                var result = await core.GetEnterpriseResourcesUnfiltered();
                var data = JsonConvert.DeserializeObject<EnterpriseResources.RootObject>(result);

                foreach (var item in data.D.Results)
                {
                    if (item.Name.Equals(user))
                        hasAccess = true;
                }
            }
            catch (Exception e) {
                Log.Info("kfsama", e.Message);
            }
            

            return hasAccess;
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
                    if (refresh.Refreshing)
                        refresh.Refreshing = false;

                    if (!online)
                        mText = "The device is offline.Please connect to the internet and restart the app";
                    else
                        mText = "You have no acces on this site";

                    refreshIdentifier = PROJECT_DATA;
                    fabfunctionidentifier = PROJECT_DATA;
                    checkDataAsync(PROJECT_DATA);
                    SupportActionBar.Title = "Projects";
                    break;
                case 1:
                    if (refresh.Refreshing)
                        refresh.Refreshing = false;

                    if (!online)
                        mText = "The device is offline.Please connect to the internet and restart the app";
                    else
                        mText = "You have no acces on this site";

                    SupportActionBar.Title = "Resources";
                    fabfunctionidentifier = RESOURCES_DATA;
                    refreshIdentifier = RESOURCES_DATA;
                    checkDataAsync(RESOURCES_DATA);
                   
                    break;
                case 2:
                    if (refresh.Refreshing)
                        refresh.Refreshing = false;

                    if (!online)
                        mText = "The device is offline.Please connect to the internet and restart the app";
                    else
                        mText = "You have no acces on this site";

                    SupportActionBar.Title = "My Tasks";
                    fabfunctionidentifier = TASKS_DATA;
                    refreshIdentifier = TASKS_DATA;
                    checkDataAsync(TASKS_DATA);
                    
                    break;
                case 3:
                    if (refresh.Refreshing)
                        refresh.Refreshing = false;

                    if (!online)
                        mText = "The device is offline.Please connect to the internet and restart the app";
                    else
                        mText = "You have no acces on this site";

                    SupportActionBar.Title = "Timesheets";
                    refreshIdentifier = TIMESHEET_DATA;
                    fabfunctionidentifier = TIMESHEET_DATA;
                    checkDataAsync(TIMESHEET_DATA);
                    
                    break;
                case 4:
                    int count = 0;
                    if (refresh.Refreshing)
                        refresh.Refreshing = false;

                    SupportActionBar.Title = "Saved Timsheets";
                    refreshIdentifier = OFFLINE_DATA;
                    fabfunctionidentifier = OFFLINE_DATA;

                    if (offline != null)
                    {
                        foreach (var item in offline)
                        {
                            count++;
                        }
                        if (count != 0) {
                            fab.SetVisibility(ViewStates.Gone);
                            switchFragment(savedTimesheetFragment);
                        }
                        else
                        {
                            mText = "You don't have any timesheet saved";
                            switchFragment(new OfflineFragment());
                            fab.SetVisibility(ViewStates.Gone);
                        }
                    }
                    else
                    {
                        mText = "You don't have any timesheet saved";
                        switchFragment(new OfflineFragment());
                        fab.SetVisibility(ViewStates.Gone);
                    }
                    
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
        Android.Support.V4.App.Fragment oldFragment;
        private void switchFragment(Android.Support.V4.App.Fragment fragment) {

            if (oldFragment == fragment)
                return;

            oldFragment = fragment;
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        private void clearFragment() {
            if (oldFragment != null) {
                SupportFragmentManager.BeginTransaction()
                .Remove(oldFragment)
                .Commit();
            }
            
        }

        private void fabFunctions() {
            View view = null;
            Android.Support.V7.App.AlertDialog dialog = null;
            switch (fabfunctionidentifier) {

                case 1:
                    if (view != null)
                        view = null;

                    if (dialog != null)
                        dialog = null;

                    view = LayoutInflater.Inflate(Resource.Layout.add_project_dialog, null);
                    dialog = helpDialog.AddProjectDialog(this, core, view);
                    dialog.Show();
                    break;

                case 2:
                    if (view != null)
                        view = null;

                    if (dialog != null)
                        dialog = null;

                    view = LayoutInflater.Inflate(Resource.Layout.add_task_layout, null);
                    //dialog = helpDialog.AddTaskDialog(this, core,view, pServer);
                    //dialog.Show();
                    break;

                case 3:
                    timesheetFragment.ShowAddLineDialog();
                    break;
                case 4:
                    helpDialog.AddEnterpriseResource(this).Show();
                    break;
                case 5:

                    break;
            }

        }

        public void seeDetails(int identifier, int position) {
            tbModified = position;
            Intent intent = null;
            switch (identifier) {
                //projects
                case 1:
                    if (intent != null)
                        intent = null;

                    intent = new Intent(this, typeof(DetailsActivity));
                    intent.PutExtra("projectData", JsonConvert.SerializeObject(projects));
                    intent.PutExtra("projectServer", JsonConvert.SerializeObject(pServer));
                    intent.PutExtra("title", pServer.D.Results[position].Name);
                    intent.PutExtra("core", JsonConvert.SerializeObject(core));
                    intent.PutExtra("formDigest", core.FormDigest);
                    intent.PutExtra("url", pServer.D.Results[position].ProjectResources.Deferred.Uri);
                    intent.PutExtra("Id", pServer.D.Results[position].Id);
                    if(projects.D.Results[position].ProjectOwnerName.Equals(userName.Text))
                        intent.PutExtra("ShowOptions", true);
                    else
                        intent.PutExtra("ShowOptions", false);
                    //Log.Info("kfsama", pServer.D.Results[position].ProjectResources.Deferred.Uri);
                    //Log.Info("kfsama", pServer.D.Results[position].Name);
                    //Log.Info("kfsama", pServer.D.Results[position].Id);
                    StartActivity(intent);
                    break;
                //tasks
                case 2:
                    break;
            }
        }

        //UI stuff end

        //data stuff

        public void checkDataAsync(int whatData)
        {
            if (hasAccess)
            {

                if (online)
                {
                    switch (whatData)
                    {
                        case 1:
                            fab.SetVisibility(ViewStates.Gone);
                            if (pServer == null && projects == null)
                                fillDataAsync(whatData);
                            else
                                switchFragment(projectFragment);

                            break;

                        case 2:
                            fab.SetVisibility(ViewStates.Gone);
                            if (tasks == null)
                                fillDataAsync(whatData);
                            else
                                switchFragment(taskFragment);
                            break;

                        case 3:
                            fab.SetVisibility(ViewStates.Visible);
                            if (timesheetPeriods == null)
                                fillDataAsync(whatData);
                            else
                                switchFragment(timesheetFragment);
                            break;

                        case 4:
                            fab.SetVisibility(ViewStates.Gone);
                            if (enterpriseResources == null)
                                fillDataAsync(whatData);
                            else
                                switchFragment(enterpriseResourceFragment);
                            break;
                    }
                }
                else
                {
                    willSwitch = true;
                    switchFragment(new OfflineFragment());
                    fab.SetVisibility(ViewStates.Gone);
                }

            }
            else {
                mText = "You have no acces on this site";
                switchFragment(new OfflineFragment());
                refresh.Refreshing = false;
                //willSwitch = true;
            }
            
        }

        public void fillDataAsync(int whatData)
        {

            clearFragment();
            oldFragment = null;
            willSwitch = false;
            refresh.Refreshing = true;
            //1 for projects
            //2 for tasks
            //3 for timesheet
            //4 for enterprise resources
            switch (whatData) {

                case 1:
                        

                    ThreadPool.QueueUserWorkItem(async state =>
                        {
                            projectJson = await core.GetProjects();
                            var temp = await core.GetProjectServer();
                            projects = JsonConvert.DeserializeObject<ProjectModel.RootObject>(projectJson);
                            pServer = JsonConvert.DeserializeObject<ProjectData.RootObject>(temp);
                            if (projects.D.Results.Count == 0 && pServer.D.Results.Count == 0) {
                                projects = null;
                                pServer = null;
                                mText = "There are no projects in this site";
                                RunOnUiThread(()=> { switchFragment(new OfflineFragment()); refresh.Refreshing = false; willSwitch = true; });
                            }
                            else
                                RunOnUiThread(() => { switchFragment(projectFragment); refresh.Refreshing = false; willSwitch = true; });
                            //CancellationToken tok = (CancellationToken)state;
                            //if (tok.IsCancellationRequested)
                            //{
                            //    projects = null;
                            //    pServer = null;
                            //    return;
                            //}
                        });
                    break;
                case 2:
                
                    tasks = new List<Taskmodel.RootObject> { };
                    tasksWithResource.Clear();
                    if (pServer == null && projects == null)
                    {
                        tasks = null;
                        mText = "There are no tasks assigned to you";
                        RunOnUiThread(() => { switchFragment(new OfflineFragment()); refresh.Refreshing = false; willSwitch = true; });
                    }
                    else {
                        ThreadPool.QueueUserWorkItem(async state =>
                        {

                            for (int i = 0; i < pServer.D.Results.Count; i++)
                            {
                                string data = await core.GetTasks(pServer.D.Results[i].Id);
                                string data2 = await core.GetResourceAssignment(pServer.D.Results[i].Id, userName.Text);
                                if (data.Length > 20)
                                {
                                    taskJson.Add(data);
                                    projectsWithTasks.Add(pServer.D.Results[i].Name);
                                }
                                if (data2.Length > 20) {
                                    var insides = JsonConvert.DeserializeObject<ProjectResourceAssignments.RootObject>(data2);
                                    foreach (var item in insides.D.Results) {
                                        if (item.ResourceName.Equals(userName.Text))
                                            tasksWithResource.Add(item.TaskName);
                                    }
                                }
                            }

                            for (int i = 0; i < taskJson.Count; i++)
                            {
                                tasks.Add(JsonConvert.DeserializeObject<Taskmodel.RootObject>(taskJson[i]));
                            }

                            if (tasks.Count > 0 && tasksWithResource.Count > 0)
                            {
                                RunOnUiThread(() => { switchFragment(taskFragment); refresh.Refreshing = false; willSwitch = true; });
                            }
                            else {
                                tasks = null;
                                mText = "There are no tasks assigned to you";
                                RunOnUiThread(() => { switchFragment(new OfflineFragment()); refresh.Refreshing = false; willSwitch = true; });
                            }
                        }, cts.Token);
                    }
                    break;

                case 3:
                
                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        var data = await core.GetTimesheetPeriods();
                        timesheetPeriods = JsonConvert.DeserializeObject<TimesheetPeriod.RootObject>(data);
                        RunOnUiThread(() => {
                            switchFragment(timesheetFragment);
                            refresh.Refreshing = false;
                            willSwitch = true;
                        });
                    }, cts.Token);
                    break;

                case 4:
                  
                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        var data = await core.GetEnterpriseResources();
                        enterpriseResources = JsonConvert.DeserializeObject<EnterpriseResources.RootObject>(data);
                        RunOnUiThread(()=> {
                            switchFragment(enterpriseResourceFragment);
                            refresh.Refreshing = false;
                            willSwitch = true;
                        });
                    });

                    break;
            }

        }

        public void projectChecks(int checkIdentifier ,int position) {
            tbModified = position;
            string body = "";
            switch (checkIdentifier) {
                //checkout
                case 1:
                    Toast.MakeText(this, "Checking out Project", ToastLength.Short).Show();
                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        bool success = await core.CheckOut(body, projects.D.Results[tbModified].ProjectId);
                        if (success) {
                            RunOnUiThread(()=> { Toast.MakeText(this, "Successfully checked out!", ToastLength.Short).Show(); refreshData(PROJECT_DATA); }); 
                        }
                        else
                            RunOnUiThread(() => { Toast.MakeText(this, "There was an error checking out the project", ToastLength.Short).Show(); });

                    });
                    break;
                //check in
                case 2:
                    Toast.MakeText(this, "Checking in Project", ToastLength.Short).Show();
                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        bool success = await core.CheckIn(body, projects.D.Results[tbModified].ProjectId);
                        if (success)
                        {
                            RunOnUiThread(() => { Toast.MakeText(this, "Successfully checked in!", ToastLength.Short).Show(); refreshData(PROJECT_DATA); });
                        }
                        else
                            RunOnUiThread(() => { Toast.MakeText(this, "There was an error checking in the project", ToastLength.Short).Show(); });

                    });
                    break;
                //Publish
                case 3:
                    Toast.MakeText(this, "Publishing Project", ToastLength.Short).Show();
                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        bool success = await core.Publish(body, projects.D.Results[tbModified].ProjectId);
                        if (success)
                        {
                            RunOnUiThread(() => { Toast.MakeText(this, "Successfully published!", ToastLength.Short).Show(); refreshData(PROJECT_DATA); });
                        }
                        else
                            RunOnUiThread(() => { Toast.MakeText(this, "There was an error publishing the project", ToastLength.Short).Show(); });

                    });

                    break;
            }
            
        }

        public async void ModifyProject(int identifier, string body)
        {
            switch (identifier)
            {
                //1 = delete project
                //2 = edit project
                case 1:
                    Toast.MakeText(this, "Deleting Project", ToastLength.Short);
                    bool isSuccess = await core.DeleteProject(body, projects.D.Results[tbModified].ProjectId);
                    if (isSuccess)
                    {
                        Toast.MakeText(this, "Project Successfully Deleted!", ToastLength.Short).Show();
                        refreshData(refreshIdentifier);
                    }
                    else
                        Toast.MakeText(this, "There was an error deleting the project", ToastLength.Short).Show();
                    break;

                case 2:
                    break;
            }
        }

        public void ModifyTask(int identifier, string projectName, string taskName, string body) {
            //1 = delete task
            //2 = edit task
            switch (identifier) {

                case 1:
                    Toast.MakeText(this, "Deleting task...", ToastLength.Short).Show();
                    string projectId = null;
                    string taskId = null;
                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        for (int i = 0; i < projects.D.Results.Count; i++)
                        {
                            if (projects.D.Results[i].ProjectName.Equals(projectName))
                            {
                                projectId = projects.D.Results[i].ProjectId;
                                break;
                            }
                        }

                        for (int i = 0; i < tasks.Count; i++)
                        {
                            for (int j = 0; j < tasks[i].D.Results.Count; j++)
                            {
                                if (tasks[i].D.Results[j].Name.Equals(taskName))
                                {
                                    taskId = tasks[i].D.Results[j].Id;
                                    break;
                                }
                            }
                        }
                        if (taskId != null && projectId != null) {
                            bool success = await core.DeleteTask(body, projectId, taskId);
                            if (success) 
                                RunOnUiThread(()=> { Toast.MakeText(this, "Successfully deleted the task! Publish the project to see the changes", ToastLength.Short).Show(); });
                            else
                                RunOnUiThread(() => { Toast.MakeText(this, "There was an error deleting the task", ToastLength.Short).Show(); });
                        }
                    });
                    break;
                case 2:
                    Toast.MakeText(this, "Updating task...", ToastLength.Short).Show();

                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        core.AddHeaders(2);
                        bool success = await core.UpdateTask(body, projectName, taskName);
                        if (success)
                            RunOnUiThread(() => { Toast.MakeText(this, "Successfully Updated! Please publish the changes", ToastLength.Short).Show(); core.AddHeaders(1); });
                        else
                            RunOnUiThread(()=> { Toast.MakeText(this, "There was an error updating the task", ToastLength.Short).Show(); });
                    });

                    break;
            }

        }

        public async void ModifyEnterpriseResources(int identifier, int position, string body)
        {

            switch (identifier)
            {
                case 1:
                    Toast.MakeText(this, "Updating Enterprise Resource...", ToastLength.Short).Show();
                    core.AddHeaders(2);
                    bool isSuccess1 = await core.UpdateEnterpriseResource(body, enterpriseResources.D.Results[position].Id);
                    if (isSuccess1)
                    {
                        Toast.MakeText(this, "Successfully updated!", ToastLength.Short).Show();
                        enterpriseResources = null;
                        checkDataAsync(RESOURCES_DATA);
                    }
                    else
                        Toast.MakeText(this, "There was a problem updating the resource", ToastLength.Short).Show();

                    core.AddHeaders(1);
                    break;
                case 2:
                    Toast.MakeText(this, "Deleting Enterprise Resource...", ToastLength.Short).Show();
                    bool isSuccess = await core.DeleteEnterpriseResource(body, enterpriseResources.D.Results[position].Id);
                    if (isSuccess) {
                        Toast.MakeText(this, "Successfully deleted!", ToastLength.Short).Show();
                        enterpriseResources = null;
                        checkDataAsync(RESOURCES_DATA);
                    }
                    else
                        Toast.MakeText(this, "There was a problem deleting the resource", ToastLength.Short).Show();
                    break;
            }
        }

        public async void refreshData(int whatData) {
            switch (whatData) {
                case 1:
                    projects = null;
                    projectJson = null;
                    pServer = null;
                    checkDataAsync(PROJECT_DATA);
                    break;
                case 2:
                    tasks = null;
                    projectsWithTasks.Clear();
                    taskJson.Clear();
                    checkDataAsync(TASKS_DATA);
                    break;
                case 3:
                    timesheetPeriods = null;
                    checkDataAsync(TIMESHEET_DATA);
                    break;
                case 4:
                    enterpriseResources = null;
                    checkDataAsync(RESOURCES_DATA);
                    break;
                case 5:
                    clearFragment();
                    oldFragment = null;
                    willSwitch = false;
                    refresh.Refreshing = true;

                    offline = null;
                    offline = await service.pullData(true, userName.Text);

                    int count = 0;
                    foreach (var item in offline)
                    {
                        count++;
                    }
                    if (count != 0)
                        switchFragment(savedTimesheetFragment);
                    else
                    {
                        mText = "You don't have any timesheet saved";
                        switchFragment(new OfflineFragment());
                        fab.SetVisibility(ViewStates.Gone);
                    }
                    refresh.Refreshing = false;
                    willSwitch = true;
                    break;
            }
            
        }

        public PsCore getCore() {
            return core;
        }

        public ProjectModel.RootObject getProjectList() {
            return projects;
        }

        public List<Taskmodel.RootObject> getTaskList() {
            return tasks;
        }

        public List<string> getProjectNamesWithTasks() {
            return projectsWithTasks;
        }

        public TimesheetPeriod.RootObject getTimesheetPeriods() {
            return timesheetPeriods;
        }

        public ProjectData.RootObject getProjectServerList() {
            return pServer;
        }

        public string getUserName() {
            return userName.Text;
        }
        //data stuff end


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    if(willSwitch == true)
                        drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}

