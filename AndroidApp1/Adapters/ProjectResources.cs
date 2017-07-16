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

namespace AndroidApp1.Adapters
{
    public class ProjectResourcesModel{

        public string resourceName { get; set; }
    }

    public class Resourcez {

        List<ProjectResourcesModel> resourcesList = new List<ProjectResourcesModel> { };
        public List<string> temp1 = new List<string> { };

        public Resourcez() { }

        public void addResources(string name) {
            resourcesList.Add(new ProjectResourcesModel() { resourceName = name });
        }

        public bool search(string name) {
            bool thereIs = false;

            for (int i = 0; i < temp1.Count; i++) {
                if (temp1[i].Equals(name))
                {
                    temp1.RemoveAt(i);
                    thereIs = true;
                }
            }

            if (thereIs == false) {
                temp1.Add(name);
            }

            return thereIs;
        }

        public int numHome
        {
            get { return resourcesList.Count; }
        }

        public ProjectResourcesModel this[int i]
        {
            get { return resourcesList[i]; }
        }

    }


    public class ProjectResourcesViewHolder : RecyclerView.ViewHolder {

        public TextView name { get; set; }
        public LinearLayout mLayout { get; set; }

        public ProjectResourcesViewHolder(View itemView, Action<int> listener) : base(itemView) {
            mLayout = itemView.FindViewById<LinearLayout>(Resource.Id.llProjectResource);
            name = itemView.FindViewById<TextView>(Resource.Id.tvProjectResourcesName);
        }
    }

    public class DialogProjectResourceAdapter : RecyclerView.Adapter {

        public EventHandler<int> itemClick;
        public Resourcez mResources;

        public DialogProjectResourceAdapter(Resourcez resources)
        {
            this.mResources = resources;
        }

        public override int ItemCount => mResources.numHome;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ProjectResourcesViewHolder vh = holder as ProjectResourcesViewHolder;
            vh.name.Text = mResources[position].resourceName;
            vh.mLayout.Click += delegate {
                if (mResources.search(mResources[position].resourceName) == false)
                    vh.mLayout.SetBackgroundColor(Color.ParseColor("#27ae60"));
                else
                    vh.mLayout.SetBackgroundColor(Color.ParseColor("#ffffff"));
            };
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.project_team_layout, parent, false);
            ProjectResourcesViewHolder vh = new ProjectResourcesViewHolder(itemView, OnClick);
            return vh;
        }

        private void OnClick(int obj)
        {
            if (itemClick != null)
            {
                itemClick(this, obj);
            }
        }

    }

    public class ProjectResourceAdapter : RecyclerView.Adapter {

        public EventHandler<int> itemClick;
        public Resourcez mResources;

        public ProjectResourceAdapter(Resourcez resources) {
            this.mResources = resources;
        }

        public override int ItemCount => mResources.numHome;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ProjectResourcesViewHolder vh = holder as ProjectResourcesViewHolder;
            vh.name.Text = mResources[position].resourceName;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.project_team_layout, parent, false);
            ProjectResourcesViewHolder vh = new ProjectResourcesViewHolder(itemView, OnClick);
            return vh;
        }

        private void OnClick(int obj)
        {
            if (itemClick != null)
            {
                itemClick(this, obj);
            }
        }
    }


}