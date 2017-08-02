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
using AndroidApp1.Activities;
using System.Threading;
using Newtonsoft.Json;
using Android.Support.V7.Widget;
using AndroidApp1.Adapters;

namespace AndroidApp1.Fragments
{
    public class ProjectDetailsFragment : Fragment
    {

        TextView projecType, projectDesc, projectStats, projectPercent, projectWork, projectDuration, projectStart, projectEnd, projectOwner, projectLPD;
        DetailsActivity details;
        string projectData, projectServer, projectTitle;
        RecyclerView mRecyclerView, mRecyclerView2;
        RecyclerView.LayoutManager mLayoutManager, mLayoutManager2;
        Resourcez mResources, mTasks;
        ProjectResourceAdapter mProjectResourceAdapter, mProjectTaskAdapter;
        ProjectResources.RootObject mProjectResources;
        AndroidApp1.Taskmodel.RootObject mProjectTasks;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            View view = inflater.Inflate(Resource.Layout.project_details_layout, container, false);
            details = Activity as DetailsActivity;
            projectData = details.projectDataJson;
            projectServer = details.projectServerJson;
            projectTitle = details.projectTitle;

            projecType = view.FindViewById<TextView>(Resource.Id.tvProjectDetailsType);
            projectDesc = view.FindViewById<TextView>(Resource.Id.tvProjectDetailsDescription);
            projectStats = view.FindViewById<TextView>(Resource.Id.tvProjectDetailsStatus);
            projectPercent = view.FindViewById<TextView>(Resource.Id.tvProjectDetailsPercentComplete);
            projectWork = view.FindViewById<TextView>(Resource.Id.tvProjectDetailsWork);
            projectDuration = view.FindViewById<TextView>(Resource.Id.tvProjectDetailsDuration);
            projectStart = view.FindViewById<TextView>(Resource.Id.tvProjectDetailsStartDate);
            projectEnd = view.FindViewById<TextView>(Resource.Id.tvProjectDetailsEndDate);
            projectOwner = view.FindViewById<TextView>(Resource.Id.tvProjectDetailsOwner);
            projectLPD = view.FindViewById<TextView>(Resource.Id.tvProjectDetailsLPD);
            
            ThreadPool.QueueUserWorkItem(state => {
                var data1 = JsonConvert.DeserializeObject<ProjectData.RootObject>(projectServer);
                var data2 = JsonConvert.DeserializeObject<ProjectModel.RootObject>(projectData);

                var item1 = data1.D.Results.Where(p => p.Name == projectTitle).FirstOrDefault();
                if (item1 != null) {
                    details.RunOnUiThread(() => {
                        projectDesc.Text = item1.Description;
                        if (item1.IsCheckedOut == true)
                            projectStats.Text = "Checked-out";
                        else
                            projectStats.Text = "Checked-in";

                        projectPercent.Text = item1.PercentComplete.ToString() + "%";
                        projectStart.Text = item1.StartDate.ToLongDateString();
                        projectEnd.Text = item1.FinishDate.ToLongDateString();
                        projectLPD.Text = item1.LastPublishedDate.ToLongDateString();

                    });
                }

                var item2 = data2.D.Results.Where(p => p.ProjectName.Equals(projectTitle)).FirstOrDefault();
                if (item2 != null) {

                    details.RunOnUiThread(()=> {
                        projecType.Text = item2.EnterpriseProjectTypeName;
                        projectOwner.Text = item2.ProjectOwnerName;

                        StringBuilder work = new StringBuilder();
                        work.Append(item2.ProjectWork.TrimEnd(new char[] { '0', '.' }));
                        if (work.ToString().Equals(""))
                            work.Append("0");

                        StringBuilder temp = new StringBuilder();
                        temp.Append(item2.ProjectDuration.TrimEnd(new char[] { '0', '.' }));
                        if (temp.ToString().Equals(""))
                        {
                            temp.Append("0");
                        }
                        int duration = Convert.ToInt32(temp.ToString()) / 8;

                        projectWork.Text = work.ToString() + "h";
                        projectDuration.Text = duration.ToString() + "d";
                    });
                }

            });
            
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rvProjectDetailTeam);
            mLayoutManager = new LinearLayoutManager(view.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mResources = new Resourcez();
            mProjectResources = JsonConvert.DeserializeObject<ProjectResources.RootObject>(details.projectResources);

            if (mProjectResources.D.Results.Count == 0)
                mResources.addResources("No resource available");
            else {
                foreach (var item in mProjectResources.D.Results)
                {
                    mResources.addResources(item.Name);
                }
            }
            
            mProjectResourceAdapter = new ProjectResourceAdapter(mResources);
            mRecyclerView.SetAdapter(mProjectResourceAdapter);

            mRecyclerView2 = view.FindViewById<RecyclerView>(Resource.Id.rvProjectDetailTasks);
            mLayoutManager2 = new LinearLayoutManager(view.Context);
            mRecyclerView2.SetLayoutManager(mLayoutManager2);
            mTasks = new Resourcez();
            if (details.projectTasksJson != null) {
                mProjectTasks = JsonConvert.DeserializeObject<AndroidApp1.Taskmodel.RootObject>(details.projectTasksJson);
                if (mProjectTasks.D.Results.Count == 0)
                    mTasks.addResources("No Tasks in this project");
                else
                {
                    foreach (var item in mProjectTasks.D.Results)
                    {
                        mTasks.addResources(item.Name);
                    }
                }
            }
            else
                mTasks.addResources("Unable to get the project's tasks");

            mProjectTaskAdapter = new ProjectResourceAdapter(mTasks);
            mRecyclerView2.SetAdapter(mProjectTaskAdapter);
            return view;
           
        }

        
    }
}