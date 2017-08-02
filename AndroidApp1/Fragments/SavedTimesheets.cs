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
using Newtonsoft.Json;

namespace AndroidApp1.Fragments
{
    public class SavedTimesheets : Fragment
    {
        RecyclerView mRecyclerView;
        TimesheetPeriodz mPeriodz;
        SavedTimesheetPeriodAdapter mPeriozAdapter;
        RecyclerView.LayoutManager mLayoutManager;
        MainActivity main;
        List<string> periodTemp = new List<string> { };
        List<DateTime> days = new List<DateTime> { };
        TimesheetLines.RootObject temp;
        List<TimesheetPeriod.Result> wtfList = new List<TimesheetPeriod.Result> { };
        List<List<string>> wtfList2 = new List<List<string>> { };
        int oldPosition;
        int currentDayPosition = 0;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.empty_recycleview, container, false);
            main = Activity as MainActivity;

            wtfList.Clear();
            wtfList2.Clear();
            periodTemp.Clear();
            oldPosition = -1;
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(view.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mPeriodz = new TimesheetPeriodz();
            foreach(var item in main.offline){
                wtfList.Add(JsonConvert.DeserializeObject<TimesheetPeriod.Result>(item.period));
                wtfList2.Add(JsonConvert.DeserializeObject < List<string>>(item.offlineTimesheetWork));
            }
            foreach (var item in wtfList) {
                periodTemp.Add(item.Name + ": " + item.Start.ToShortDateString() + " - " + item.End.ToShortDateString());
            }
            
            mPeriodz.addPeriod(periodTemp, currentDayPosition);
            mPeriozAdapter = new SavedTimesheetPeriodAdapter(mPeriodz, main, this);
            mPeriozAdapter.itemClick += Adapter_ItemClick;
            mRecyclerView.SetAdapter(mPeriozAdapter);

            return view;
        }

        private void Adapter_ItemClick(object sender, int e)
        {
        }

        public void persist(int position)
        {
            if (position == oldPosition)
                return;

            oldPosition = position;

            fillPeriodDays(position);
            fillTimesheetLines(position);
        }
        
        public void ShowWTF(int position)
        {
            List<string> anotherList = wtfList2.ElementAt(currentDayPosition);
            main.helpDialog.ShowSavedTimesheetWork(main, days, JsonConvert.DeserializeObject<TimesheetWork.RootObject>(anotherList.ElementAt(position-1)), temp.D.Results[position-1].TaskName, wtfList[currentDayPosition].Id, temp.D.Results[position-1].Id).Show();
            
        }

        public void OpenSettings(int position) {
            main.helpDialog.OpenSavedTimesheetSettings(main, position).Show();
        }

        private void fillTimesheetLines(int position)
        {
            if (temp != null)
                temp = null;

            if (mPeriodz.numHome > 1)
            {
                mPeriodz.removeItems(mPeriozAdapter);
                mPeriodz.addPeriod(periodTemp, position);
                currentDayPosition = position;
            }

            temp = JsonConvert.DeserializeObject<TimesheetLines.RootObject>(main.offline.ElementAt(currentDayPosition).offlineTimesheetLines);
            for (int i = 0; i < temp.D.Results.Count; i++)
            {
                mPeriodz.addPeriod(temp.D.Results[i].ProjectName,
                    temp.D.Results[i].TaskName,
                    temp.D.Results[i].Comment,
                    temp.D.Results[i].ValidationType.ToString(),
                    temp.D.Results[i].Status.ToString(),
                    temp.D.Results[i].TotalWork);
            }
            mPeriozAdapter.NotifyItemRangeInserted(mPeriodz.numHome - 1, temp.D.Results.Count - 1);

        }

        private void fillPeriodDays(int position)
        {
            days.Clear();
            TimeSpan span = wtfList.ElementAt(position).End.Subtract(wtfList.ElementAt(position).Start);

            for (int i = 0; i <= span.Days; i++)
            {
                days.Add(wtfList.ElementAt(position).Start.AddDays(i));
            }
        }
    }
}