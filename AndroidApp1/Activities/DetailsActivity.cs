using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using AndroidApp1.Helpers;
using Android.Util;
using System.Threading;
using Newtonsoft.Json;
using AndroidApp1.Fragments;
using PScore;
using System.Threading.Tasks;
using Android.Widget;

namespace AndroidApp1.Activities
{
    [Activity(Label = "DetailsActivity")]
    public class DetailsActivity : AppCompatActivity
    {   
        public string projectTitle { get; set; }
        public string projectDataJson { get; set; }
        public string projectServerJson { get; set; }
        public string projectResources { get; set; }
        public string projectTasksJson { get; set; }
        DialogHelpers dialogs { get; set; }
        public EnterpriseResources.RootObject mEnterprise { get; set; }
        public string id { get; set; }
        bool canClick = false;
        
        public PsCore core { get; set; }
        ProjectDetailsFragment frag = new ProjectDetailsFragment();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.full_details);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            dialogs = new DialogHelpers();

            id = Intent.GetStringExtra("Id");
            projectServerJson = Intent.GetStringExtra("projectServer");
            projectDataJson = Intent.GetStringExtra("projectData");
            projectTitle = Intent.GetStringExtra("title");
            core = JsonConvert.DeserializeObject<PsCore>(Intent.GetStringExtra("core"));
            core.setClient();
            core.setClient2(Intent.GetStringExtra("formDigest"));
            
            SupportActionBar.Title = projectTitle;

             GetTeamAsync();
             GetProjectTasks();
            //retrieving data
            SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_frame, new LoaderFragment())
                        .Commit();

        }

        private async Task GetTeamAsync()
        {
            //ThreadPool.QueueUserWorkItem(async state =>
            //{
            //    projectResources = await core.GetProjectResources(Intent.GetStringExtra("url"));
            //    var data = await core.GetEnterpriseResources();
            //    mEnterprise = JsonConvert.DeserializeObject<EnterpriseResources.RootObject>(data);
            //    RunOnUiThread(()=> {
            //        SupportFragmentManager.BeginTransaction()
            //            .Replace(Resource.Id.content_frame, frag)
            //            .Commit();
            //    });
            //});

            projectResources = await core.GetProjectResources(Intent.GetStringExtra("url"));
            var data = await core.GetEnterpriseResources();
            mEnterprise = JsonConvert.DeserializeObject<EnterpriseResources.RootObject>(data);

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, frag)
                .Commit();
            canClick = true;
        }

        private async Task GetProjectTasks() {

            projectTasksJson = await core.GetTasks(id);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId) {

                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                case Resource.Id.mnAdd:
                    if(canClick == true)
                        dialogs.AddResourceToProject(this).Show();
                    return true;

                case Resource.Id.mnRemove:
                    if (canClick == true)
                        dialogs.DeleteProjectResource(this).Show();
                    return true;

                case Resource.Id.mnCheckin:
                    if(canClick == true)
                    ForceCheckInProject();
                    return true;

                case Resource.Id.mnAddTask:
                    if(canClick == true)
                        dialogs.AddTaskDialog(this).Show();
                    return true;

                case Resource.Id.mnAssignTask:
                    if(canClick == true)
                        dialogs.AddTaskAssignments(this).Show();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private async Task ForceCheckInProject()
        {
            Toast.MakeText(this, "Checking in...",ToastLength.Short).Show();
            bool isSuccess = await core.CheckIn("", id);
            if(isSuccess)
                Toast.MakeText(this, "Successfully checked in", ToastLength.Short).Show();
            else
                Toast.MakeText(this, "There was a problem checking in the project", ToastLength.Short).Show();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            if(Intent.GetBooleanExtra("ShowOptions", false) == true)
                MenuInflater.Inflate(Resource.Menu.details_menu, menu);
            return true;
        }

    }
}