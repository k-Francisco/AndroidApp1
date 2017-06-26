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

namespace AndroidApp1.Activities
{
    [Activity(Label = "Projects")]
    public class MainActivity : BaseActivity
    {
        //fragments
        private static ProjectFragment projectFragment = new ProjectFragment();
        private static LoaderFragment loader = new LoaderFragment();
        private static TasksFragment taskFragment = new TasksFragment();
        private static DialogHelpers helpDialog;

        //data
        private ProjectModel.RootObject projects;
        private List<Taskmodel.RootObject> tasks;
        private List<string> projectsWithTasks = new List<string> { };

        //core
        private PsCore core;

        //constants
        private const int PROJECT_DATA = 1;
        private const int TASKS_DATA = 2;
        private const int REQUEST_CODE = 1;
        
        //json strings
        string projectJson;
        List<string> taskJson = new List<string> { };

        //ints
        private int tbModified = 0;
        private int refreshIdentifier = 1;
        private int fabfunctionidentifier = 1;

        FloatingActionButton fab;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        SwipeRefreshLayout refresh;
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
            //if (savedInstanceState == null)
            //{
            //    ListItemClicked(0);
            //}

            
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += delegate {
                fabFunctions();
            };

            refresh = FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresh.SetColorScheme(Resource.Color.refresher_1, Resource.Color.refresher_2);
            refresh.Refresh += delegate {
                refreshData(refreshIdentifier);
                //refresh.Refreshing = false;
            };

            //setting the cookies
            var rtFa = Intent.GetStringExtra("rtFa");
            var FedAuth = Intent.GetStringExtra("FedAuth");
            core = new PsCore(rtFa, FedAuth);
            helpDialog = new DialogHelpers();

            //setting formDigest
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            core.setClient();
            string context = await core.GetFormDigest("");
            var data = JsonConvert.DeserializeObject<AndroidApp1.FormDigestModel.RootObject>(context);
            core.setClient2(data.D.GetContextWebInformation.FormDigestValue);

            ThreadPool.QueueUserWorkItem(async state =>
            {
                var data2 = await core.GetCurrentUser();
                var currentUser = JsonConvert.DeserializeObject<CurrentUser.RootObject>(data2);
                RunOnUiThread(() => {
                    userName.Text = currentUser.D.Title;
                    userEmail.Text = currentUser.D.Email;
                });
            });

            checkDataAsync(PROJECT_DATA);
            
            //switchFragment(loader);

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
                    refreshIdentifier = 1;
                    fabfunctionidentifier = 1;
                    switchFragment(projectFragment);
                    SupportActionBar.Title = "Projects";
                    break;
                case 1:
                    //switchFragment(loader);
                    refresh.Refreshing = true;
                    SupportActionBar.Title = "Resources";
                    break;
                case 2:
                    if (refresh.Refreshing)
                        refresh.Refreshing = false;

                    SupportActionBar.Title = "My Tasks";
                    fabfunctionidentifier = 2;
                    refreshIdentifier = 2;
                    checkDataAsync(TASKS_DATA);
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
                    dialog = helpDialog.AddProjectDialog(this, core,view);
                    dialog.Show();
                    break;

                case 2:
                    if (view != null)
                        view = null;

                    if (dialog != null)
                        dialog = null;

                    view = LayoutInflater.Inflate(Resource.Layout.add_task_layout, null);
                    dialog = helpDialog.AddTaskDialog(this, core,view, projects);
                    dialog.Show();
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
                    intent.PutExtra("json", projectJson);
                    intent.PutExtra("position", position.ToString());
                    intent.PutExtra("title", projects.D.Results[position].Name);
                    StartActivityForResult(intent, REQUEST_CODE);
                    break;
                //tasks
                case 2:
                    break;
            }
        }

        //UI stuff end

        //data stuff

        //public string getFormDigest() {
        //    return prefs.GetString("formDigest",null);
        //}

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

                case 2:
                    if (tasks == null)
                        fillDataAsync(whatData);
                    else
                        switchFragment(taskFragment);
                    break;
            }
        }

        public async void fillDataAsync(int whatData)
        {

            clearFragment();
            oldFragment = null;
            //1 for projects
            switch (whatData) {

                case 1:
                        refresh.Refreshing = true;
                        projectJson = await core.GetProjects();
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            projects = JsonConvert.DeserializeObject<ProjectModel.RootObject>(projectJson);
                            RunOnUiThread(() => { switchFragment(projectFragment); refresh.Refreshing = false; });
                        });
                    break;
                case 2:
                    refresh.Refreshing = true; 
                    tasks = new List<Taskmodel.RootObject> { };
                    for (int i = 0; i<projects.D.Results.Count; i++) {
                            string data = await core.GetTasks(projects.D.Results[i].Id);
                        if (data.Length > 20) {
                            taskJson.Add(data);
                            projectsWithTasks.Add(projects.D.Results[i].Name);
                        }
                            
                    }
                    
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        for (int i = 0; i < taskJson.Count; i++) {
                            tasks.Add(JsonConvert.DeserializeObject<Taskmodel.RootObject>(taskJson[i]));
                        }

                        if (tasks.Count > 0) {
                            RunOnUiThread(() =>{ switchFragment(taskFragment); refresh.Refreshing = false; });
                        }
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
                        bool success = await core.CheckOut(body, projects.D.Results[tbModified].Id);
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
                        bool success = await core.CheckIn(body, projects.D.Results[tbModified].Id);
                        if (success)
                        {
                            RunOnUiThread(() => { Toast.MakeText(this, "Successfully checked in!", ToastLength.Short).Show(); refreshData(PROJECT_DATA); });
                        }
                        else
                            RunOnUiThread(() => { Toast.MakeText(this, "There was an error checking out the project", ToastLength.Short).Show(); });

                    });
                    break;
                //Publish
                case 3:
                    Toast.MakeText(this, "Publishing Project", ToastLength.Short).Show();
                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        bool success = await core.Publish(body, projects.D.Results[tbModified].Id);
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

        private async void ModifyProject(int identifier)
        {
            switch (identifier)
            {
                //1 = delete project
                //2 = edit project
                case 1:
                    Toast.MakeText(this, "Deleting Project", ToastLength.Short);
                    string body = "";
                    bool isSuccess = await core.DeleteProject(body, projects.D.Results[tbModified].Id);
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
                            if (projects.D.Results[i].Name.Equals(projectName))
                            {
                                projectId = projects.D.Results[i].Id;
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
                            RunOnUiThread(() => { Toast.MakeText(this, "Successfully Updated!", ToastLength.Short).Show(); core.AddHeaders(1); });
                        else
                            RunOnUiThread(()=> { Toast.MakeText(this, "There was an error updating the task", ToastLength.Short).Show(); });
                    });

                    break;
            }

        }

        public void refreshData(int whatData) {
            switch (whatData) {
                case 1:
                    projects = null;
                    projectJson = null;
                    checkDataAsync(PROJECT_DATA);
                    break;
                case 2:
                    tasks = null;
                    projectsWithTasks.Clear();
                    taskJson.Clear();
                    checkDataAsync(TASKS_DATA);
                    break;
            }
            
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
        //data stuff end


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            
            if (resultCode == Result.Ok)
            {
                int identifier = data.GetIntExtra("identifier", 0);
                ModifyProject(identifier);
            }
        }


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

