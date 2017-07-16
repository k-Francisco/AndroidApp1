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
using Android.Support.V7.Widget;
using Android.Graphics;
using AndroidApp1.Activities;
using AndroidApp1.Helpers;

namespace AndroidApp1.Adapters
{
    public class ProjectModel {

        public string mProjectName { get; set; }
        public string mProjectPercentComplete { get; set; }
        public string mProjectWork { get; set; }
        public string mProjectDuration { get; set; }
        public string mProjectOwner { get; set; }
        public bool isCheckedOut { get; set; }

    }

    public class Projects {

        List<ProjectModel> projectList = new List<ProjectModel> { };

        public Projects() { }

        public void addProjects(string projectName, string percentComplete, string work, string duration, bool isCheckedout, string projectOwner) {
            projectList.Add(new ProjectModel() { mProjectName = projectName, mProjectPercentComplete = percentComplete, mProjectWork = work, mProjectDuration = duration, isCheckedOut = isCheckedout, mProjectOwner = projectOwner});
        }

        public int numHome {
            get { return projectList.Count; }
        }

        public ProjectModel this[int i] {
            get { return projectList[i]; }
        }
    }

    public class ProjectViewHolder : RecyclerView.ViewHolder {

        public TextView projectName { get; set; }
        public TextView percentComplete { get; set; }
        public TextView work { get; set; }
        public TextView duration { get; set; }
        public View status { get; set; }
        public RelativeLayout rLayout { get; set; }
        public TextView projectOwner { get; set; }


        public ProjectViewHolder(View itemView, Action<int> listener) : base(itemView) {

            projectName = itemView.FindViewById<TextView>(Resource.Id.tvProjectName);
            percentComplete = itemView.FindViewById<TextView>(Resource.Id.tvProjectPercentComplete);
            work = itemView.FindViewById<TextView>(Resource.Id.tvProjectWork);
            duration = itemView.FindViewById<TextView>(Resource.Id.tvProjectDuration);
            status = itemView.FindViewById<View>(Resource.Id.vCheckedOutStatus);
            rLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.rlProjectView);
            projectOwner = itemView.FindViewById<TextView>(Resource.Id.tvProjectOwner);
  
        }

    }

    public class ProjectAdapter : RecyclerView.Adapter {

        public EventHandler<int> itemClick;
        public Projects mProjects;
        public MainActivity main;

        public ProjectAdapter(Projects project, MainActivity main) {
            this.main = main;
            mProjects = project;
        }

        public override int ItemCount {
            get { return mProjects.numHome; }
        }

        //public override int GetItemViewType(int position)
        //{
        //    return base.GetItemViewType(position);
        //}

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ProjectViewHolder vh = holder as ProjectViewHolder;
            vh.projectName.Text = mProjects[position].mProjectName;
            vh.percentComplete.Text = mProjects[position].mProjectPercentComplete;
            vh.work.Text = mProjects[position].mProjectWork;
            vh.duration.Text = mProjects[position].mProjectDuration;
            vh.projectOwner.Text = mProjects[position].mProjectOwner;
            if (mProjects[position].isCheckedOut == false)
                vh.status.SetBackgroundColor(Color.ParseColor("#30752F"));
            else
                vh.status.SetBackgroundColor(Color.DarkRed);

            vh.rLayout.Click += delegate { main.seeDetails(1, position); };
            vh.rLayout.LongClick += delegate { main.helpDialog.ProjectOptionsDialog(main, main.getCore(), position).Show(); };
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType){

            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.project_card, parent, false);
            ProjectViewHolder vh = new ProjectViewHolder(itemView, Onclick);
            return vh;
        }

        private void Onclick(int obj)
        {
            if (itemClick != null)
            {
                itemClick(this, obj);
            }
        }
    }

}