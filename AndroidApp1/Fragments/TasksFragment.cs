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
using Android.Support.V7.Widget;
using AndroidApp1.Adapters;
using AndroidApp1.Activities;

namespace AndroidApp1.Fragments
{
    public class TasksFragment : Fragment
    {

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        Tasks mTasks;
        TasksAdapter mTasksAdapter;
        List<Taskmodel.RootObject> mTaskList;
        List<string> mProjectNames = new List<string> { };
        MainActivity main;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View rootView = inflater.Inflate(Resource.Layout.empty_recycleview, container, false);
            main = (Activity as MainActivity);

            mRecyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(rootView.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mTasks = new Tasks();
            mProjectNames = main.getProjectNamesWithTasks();
            mTaskList = main.getTaskList();
            if (mTaskList != null)
            {
                for (int i = 0; i < mTaskList.Count; i++)
                {
                    for (int j = 0; j < mTaskList[i].D.Results.Count; j++)
                    {
                        mTasks.addTasks(mTaskList[i].D.Results[j].Name,
                                        mTaskList[i].D.Results[j].PercentComplete.ToString(),
                                        mTaskList[i].D.Results[j].Work,
                                        mTaskList[i].D.Results[j].Duration,
                                        mProjectNames[i]);
                    }
                }
            }
            mTasksAdapter = new TasksAdapter(mTasks, main);
            mTasksAdapter.itemClick += Adapter_ItemClick;
            mRecyclerView.SetAdapter(mTasksAdapter);

            return rootView;
        }

        private void Adapter_ItemClick(object sender, int e)
        {
            Log.Info("kfsama", "item clicked at position " + e);
        }
    }
}