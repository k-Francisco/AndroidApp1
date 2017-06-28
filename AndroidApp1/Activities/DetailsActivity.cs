﻿using System;
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

namespace AndroidApp1.Activities
{
    [Activity(Label = "DetailsActivity")]
    public class DetailsActivity : AppCompatActivity
    {
        DialogHelpers dialogs = new DialogHelpers();
        private ProjectModel.RootObject details;
        string projectTitle, json;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.full_details);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            string position = Intent.GetStringExtra("position");
            json = Intent.GetStringExtra("json");
            projectTitle = Intent.GetStringExtra("title");

            SupportActionBar.Title = projectTitle;
            //retrieving data
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, new LoadingFragment())
                .Commit();

            ThreadPool.QueueUserWorkItem(state => {
                details = JsonConvert.DeserializeObject<ProjectModel.RootObject>(json);
            });

        }

        public void deleteProject() {
            Intent intent = new Intent();
            intent.PutExtra("identifier", 1);
            intent.PutExtra("body", "");
            SetResult(Result.Ok, intent);
            Finish();
        }

        public void EditProject(string body) {
            Intent intent = new Intent();
            intent.PutExtra("identifier",2);
            intent.PutExtra("body", body);
            SetResult(Result.Ok, intent);
            Finish();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId) {

                case Android.Resource.Id.Home:
                    SetResult(Result.Canceled);
                    Finish();
                    return true;

                case Resource.Id.mnEdit:
                    View view = LayoutInflater.Inflate(Resource.Layout.edit_project_dialog, null);
                    dialogs.EditProjectDialog(this, view, json);
                    return true;

                case Resource.Id.mnDelete:
                    dialogs.DeleteProjectDialog(this, projectTitle).Show();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.details_menu, menu);
            return true;
        }

        
    }
}