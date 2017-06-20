using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using AndroidApp1.Activities;
using AndroidApp1.Adapters;
using Android.Util;
using System.Threading;

namespace AndroidApp1.Fragments
{
    public class ProjectFragment : Fragment
    {

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        Projects mProjects;
        ProjectAdapter mProjectAdapter;
        ProjectModel.RootObject mProjectList;
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
            if (mProjectList != null)
                for (int i = 0; i < mProjectList.D.Results.Count; i++)
                {

                    mProjects.addProjects(
                        mProjectList.D.Results[i].Name,
                        mProjectList.D.Results[i].PercentComplete.ToString(),
                        "0h",
                        "1d",
                        mProjectList.D.Results[i].IsCheckedOut
                        );
                }

            mProjectAdapter = new ProjectAdapter(mProjects, main);
            mProjectAdapter.itemClick += Adapter_ItemClick;
                mRecyclerView.SetAdapter(mProjectAdapter);

            return rootView;
        }

        private void Adapter_ItemClick(object sender, int e)
        {
            Log.Info("Kfsama", "item clicked at position " + e);
        }
    }
}