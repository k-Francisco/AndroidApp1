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
using Android.Support.V7.Widget;
using AndroidApp1.Adapters;

namespace AndroidApp1.Fragments
{
    public class TimesheetFragment : Fragment
    {
        TimesheetPeriod.RootObject periodList;
        RecyclerView mRecyclerView;
        TimesheetPeriodz mPeriodz;
        TimesheetPeriodAdapter mPeriozAdapter;
        RecyclerView.LayoutManager mLayoutManager;
        MainActivity main;
        List<string> days = new List<string> { };

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.empty_recycleview, container, false);

            main = (Activity as MainActivity);
            periodList = main.getTimesheetPeriods();

            TimeSpan span;
            List<string> periodTemp = new List<string> { };
            int currentDayPosition = 0;
            for (int i = 0; i < periodList.D.Results.Count; i++) {
                periodTemp.Add(periodList.D.Results[i].Name + ": " +periodList.D.Results[i].Start.ToShortDateString() + " - " + periodList.D.Results[i].End.ToShortDateString());
                span = periodList.D.Results[i].End.Subtract(periodList.D.Results[i].Start);
                for (int j = 0; j <= span.Days; j++) {
                    if (periodList.D.Results[i].Start.Date.AddDays(j).Equals(DateTime.Now.Date)) {
                        currentDayPosition = i;
                        break;
                    }
                }     
            }

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(view.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mPeriodz = new TimesheetPeriodz();
            mPeriodz.addPeriod(periodTemp, currentDayPosition);

            mPeriozAdapter = new TimesheetPeriodAdapter(mPeriodz, main, this);
            mPeriozAdapter.itemClick += Adapter_ItemClick;
            mRecyclerView.SetAdapter(mPeriozAdapter);
            return view;

        }

        private void Adapter_ItemClick(object sender, int e)
        {
            Log.Info("kfsama", "item clicked at position " + e);
        }

        public void fillPeriodDays(Spinner periodDays, int position)
        {
            days.Clear();
            TimeSpan span = periodList.D.Results[position].End.Subtract(periodList.D.Results[position].Start);

            for (int i = 0; i <= span.Days; i++) {
                days.Add(periodList.D.Results[position].Start.AddDays(i).ToShortDateString());
            }

            var daysAdapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, days);
            periodDays.Adapter = daysAdapter;
        }
    }
}