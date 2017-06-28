using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using AndroidApp1.Activities;
using AndroidApp1.Adapters;
using Android.Util;
using System.Threading;
using Android.Support.V4.Widget;
using System.Text;

namespace AndroidApp1.Fragments
{
    public class ProjectFragment : Fragment
    {

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        Projects mProjects;
        ProjectAdapter mProjectAdapter;
        ProjectModel.RootObject mProjectList;
        ProjectData.RootObject mProjectServer;
        MainActivity main;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
            // Create your fragment here
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.empty_recycleview, null);
            main = (Activity as MainActivity);

            mRecyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(rootView.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);


            mProjects = new Projects();
            mProjectList = main.getProjectList();
            mProjectServer = main.getProjectServerList();
            if (mProjectList != null)
                for (int i = 0; i < mProjectList.D.Results.Count; i++)
                {
                    StringBuilder work = new StringBuilder();
                    work.Append(mProjectList.D.Results[i].ProjectWork.TrimEnd(new char[] { '0' , '.'}));
                    if (work.ToString().Equals(""))
                        work.Append("0");

                    StringBuilder temp = new StringBuilder();
                    temp.Append(mProjectList.D.Results[i].ProjectDuration.TrimEnd(new char[] { '0', '.' }));
                    if (temp.ToString().Equals("")) {
                        temp.Append("0");
                    }
                    int duration = Convert.ToInt32(temp.ToString())/8;
                    for (int j =0; j < mProjectServer.D.Results.Count; j++) {
                        if (mProjectServer.D.Results[j].Name.Equals(mProjectList.D.Results[i].ProjectName)) {
                            mProjects.addProjects(
                            mProjectList.D.Results[i].ProjectName,
                            mProjectList.D.Results[i].ProjectPercentCompleted.ToString(),
                            work.ToString() + "h",
                            duration.ToString() + "d",
                            mProjectServer.D.Results[j].IsCheckedOut);
                        }
                    }
                    
                }

            mProjectAdapter = new ProjectAdapter(mProjects, main);
            mProjectAdapter.itemClick += Adapter_ItemClick;
            mRecyclerView.SetAdapter(mProjectAdapter);
            return rootView;
        }

        private void Adapter_ItemClick(object sender, int e)
        {
            Log.Info("kfsama", "item clicked at position " + e);
        }
    }
}