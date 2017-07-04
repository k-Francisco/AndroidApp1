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
using PScore;
using System.Threading;
using Newtonsoft.Json;
using AndroidApp1.Helpers;

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
        DialogHelpers dialogs = new DialogHelpers();
        PsCore core;
        int currentDayPosition = 0;
        TimesheetLines.RootObject temp;
        List<string> periodTemp = new List<string> { };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.empty_recycleview, container, false);
            
            main = (Activity as MainActivity);
            core = main.getCore();
            periodList = main.getTimesheetPeriods();

            TimeSpan span;
            for (int i = 0; i < periodList.D.Results.Count; i++) {
                periodTemp.Add(periodList.D.Results[i].Name + ": " + periodList.D.Results[i].Start.ToShortDateString() + " - " + periodList.D.Results[i].End.ToShortDateString());
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

        public void OpenSettings(int position) {
            dialogs.OpensSettingsDialog(core, main, periodList.D.Results[position].Id).Show();
        }

        public void LongClick(int position) {
            Toast.MakeText(main, "Long clicked at position " + position, ToastLength.Short).Show();
        }

        public void ShowAddLineDialog() {
            dialogs.AddTimesheetLine(core, main, periodList.D.Results[currentDayPosition].Id);
        }

        //public void fillPeriodDays(Spinner periodDays, int position)
        //{
        //    days.Clear();
        //    TimeSpan span = periodList.D.Results[position].End.Subtract(periodList.D.Results[position].Start);

        //    for (int i = 0; i <= span.Days; i++) {
        //        days.Add(periodList.D.Results[position].Start.AddDays(i).ToShortDateString());
        //    }

        //    var daysAdapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, days);
        //    periodDays.Adapter = daysAdapter;
        //}

        public void fillTimesheetLines(int position) {

            if (temp != null)
                temp = null;

            if (mPeriodz.numHome > 1)
            {
                mPeriodz.removeItems(mPeriozAdapter);
                mPeriodz.addPeriod(periodTemp, position);
                currentDayPosition = position;
            }

            ThreadPool.QueueUserWorkItem(async state =>
            {
                try {

                    bool success = await core.CreateTimesheet("", periodList.D.Results[position].Id);
                    if (success) {
                        string data = await core.GetTimesheetLines(periodList.D.Results[position].Id);
                        temp = JsonConvert.DeserializeObject<TimesheetLines.RootObject>(data);

                        for (int i = 0; i < temp.D.Results.Count; i++)
                        {
                            mPeriodz.addPeriod(temp.D.Results[i].ProjectName,
                                temp.D.Results[i].TaskName,
                                temp.D.Results[i].Comment,
                                temp.D.Results[i].ValidationType.ToString(),
                                temp.D.Results[i].Status.ToString(),
                                temp.D.Results[i].TotalWork);
                        }
                        main.RunOnUiThread(() => { mPeriozAdapter.NotifyItemRangeInserted(mPeriodz.numHome - 1, temp.D.Results.Count-1); });
                    }
                }
                catch (Exception e) {
                    Log.Info("kfsama", e.Message);
                }
            });
            
        } 
    }
}