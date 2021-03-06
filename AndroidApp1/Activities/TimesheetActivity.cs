﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Newtonsoft.Json;
using AndroidApp1.Adapters;
using PScore;
using System.Threading;
using Android.Util;
using System.Threading.Tasks;
using Android.Preferences;
using AndroidApp1.Models.SavedChanges;

namespace AndroidApp1.Activities
{
    [Activity(Label = "TimesheetActivity")]
    public class TimesheetActivity : AppCompatActivity
    {
        string periodId = "";
        string lineId = "";
        string rtFa = "";
        string FedAuth = "";
        List<DateTime> days;
        TimesheetWork.RootObject work;
        PsCore core;
        StringBuilder body = new StringBuilder();
        TimesheetWorkz workz;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.timesheet_line_dialog);


            if (Intent.GetBooleanExtra("identifier", false)) {
                rtFa = Intent.GetStringExtra("rtFa");
                FedAuth = Intent.GetStringExtra("FedAuth");
                string formDigest = Intent.GetStringExtra("FormDigest");
                core = new PsCore(rtFa, FedAuth);
                core.setClient();
                core.setClient2(formDigest);
            }
            periodId = Intent.GetStringExtra("periodId");
            lineId = Intent.GetStringExtra("lineId");
            days = JsonConvert.DeserializeObject<List<DateTime>>(Intent.GetStringExtra("days"));
            work = JsonConvert.DeserializeObject<TimesheetWork.RootObject>(Intent.GetStringExtra("lineWork"));

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = Intent.GetStringExtra("taskName");

            RecyclerView recyclerView = FindViewById<RecyclerView>(Resource.Id.rvTimesheetLineHours);
            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);

            workz = new TimesheetWorkz();
            for (int i = 0; i < days.Count; i++) {
                workz.addWork(days[i], "0h", "0h");
            }

            for (int i = 0; i < work.D.Results.Count; i++) {
                workz.updateWork(work.D.Results[i].Start, work.D.Results[i].ActualWork, work.D.Results[i].PlannedWork);
            }

            TimesheetWorkAdapter adapter = new TimesheetWorkAdapter(workz, this);
            recyclerView.SetAdapter(adapter);

        }
        List<TimesheetWorkModel> temp = new List<TimesheetWorkModel> { };
        List<SavedChangesModel> savedTemp = new List<SavedChangesModel> { };
        public void setActualHours(string actualHours, DateTime day) {
            if (Intent.GetBooleanExtra("identifier", false))
            {
                if (actualHours != "")
                {
                    var temp2 = temp.Where(t => t.startDate == day).FirstOrDefault();
                    if (temp2 != null)
                    {
                        temp2.actualHours = actualHours + "h";
                    }
                    else
                    {
                        temp.Add(new TimesheetWorkModel() { actualHours = actualHours + "h", startDate = day, plannedHours = "0h" });
                    }
                }
                else
                {
                    var temp2 = temp.Where(t => t.startDate == day).FirstOrDefault();
                    if (temp2 != null)
                    {
                        temp2.actualHours = "0h";
                    }
                }
            }
            else {
                if (actualHours != "")
                {
                    var temp2 = savedTemp.Where(t => t.startDate == day).FirstOrDefault();
                    if (temp2 != null)
                    {
                        temp2.actualHours = actualHours + "h";
                    }
                    else
                    {
                        savedTemp.Add(new SavedChangesModel() { actualHours = actualHours + "h", startDate = day, plannedHours = "0h", lineId = lineId, periodId = periodId });
                    }
                }
                else
                {
                    var temp2 = savedTemp.Where(t => t.startDate == day).FirstOrDefault();
                    if (temp2 != null)
                    {
                        temp2.actualHours = "0h";
                    }
                }
            }

        }

        public void setPlannedHours(string plannedHours, DateTime day) {
            if (Intent.GetBooleanExtra("identifier", false))
            {
                if (plannedHours != "")
                {
                    var temp2 = temp.Where(t => t.startDate == day).FirstOrDefault();
                    if (temp2 != null)
                    {
                        temp2.plannedHours = plannedHours + "h";

                    }
                    else
                    {
                        temp.Add(new TimesheetWorkModel() { plannedHours = plannedHours + "h", startDate = day, actualHours = "0h" });

                    }
                }
                else
                {
                    var temp2 = temp.Where(t => t.startDate == day).FirstOrDefault();
                    if (temp2 != null)
                    {
                        temp2.plannedHours = "0h";
                    }
                }
            }
            else {
                if (plannedHours != "")
                {
                    var temp2 = savedTemp.Where(t => t.startDate == day).FirstOrDefault();
                    if (temp2 != null)
                    {
                        temp2.plannedHours = plannedHours + "h";
                    }
                    else
                    {
                        savedTemp.Add(new SavedChangesModel() { plannedHours = plannedHours + "h", startDate = day, actualHours = "0h", lineId = lineId, periodId = periodId });
                    }
                }
                else
                {
                    var temp2 = savedTemp.Where(t => t.startDate == day).FirstOrDefault();
                    if (temp2 != null)
                    {
                        temp2.plannedHours = "0h";
                    }
                }
            }

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId) {

                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.mnSave:
                    if (Intent.GetBooleanExtra("identifier", false))
                        saveChangesAsync();
                    else {
                        saveLocalChanges();
                    }
                    return true;

                default: return base.OnOptionsItemSelected(item);
            }
        }

        private async void saveChangesAsync()
        {
            Toast.MakeText(this, "Saving...", ToastLength.Short).Show();
            int batchCount = 0;
            body.Clear();
            for (int i = 0; i < temp.Count; i++)
            {
                body.Append("{'parameters':{'ActualWork':'" + temp[i].actualHours + "', 'PlannedWork':'" + temp[i].plannedHours + "', 'Start':'" + temp[i].startDate + "', 'NonBillableOvertimeWork':'0h', 'NonBillableWork':'0h', 'OvertimeWork':'0h'}}");
                bool isSuccess = await core.AddTimesheetLineWork(body.ToString(), periodId, lineId);
                if (isSuccess)
                    batchCount++;

                body.Clear();
            }

            if (batchCount == temp.Count)
            {
                Toast.MakeText(this, "Successfully updated the changes", ToastLength.Short).Show();
                Finish();
            }
            else {
                Toast.MakeText(this, "There was an error saving the changes", ToastLength.Short).Show();
            }
        }

        public void saveLocalChanges() {

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            if (!prefs.GetString("tobepushed", "").Equals(""))
            {
                var list = JsonConvert.DeserializeObject<List<TimesheetWorkModel>>(prefs.GetString("tobepushed", ""));
                foreach (var item in savedTemp)
                {
                    var listTemp = list.Where(t => t.startDate == item.startDate).FirstOrDefault();
                    if (listTemp != null)
                    {
                        listTemp.actualHours = item.actualHours;
                        listTemp.plannedHours = item.plannedHours;
                    }
                    else
                    {
                        list.Add(new TimesheetWorkModel() { plannedHours = item.plannedHours + "h", startDate = item.startDate, actualHours = item.actualHours });
                    }
                }
                prefs.Edit().Remove("tobepushed").Apply();
                prefs.Edit().PutString("tobepushed", JsonConvert.SerializeObject(list)).Apply();
                Toast.MakeText(this, "Saved!", ToastLength.Short).Show();
            }
            else {
                prefs.Edit().PutString("tobepushed", JsonConvert.SerializeObject(savedTemp)).Apply();
                Toast.MakeText(this, "Saved!", ToastLength.Short).Show();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.timesheet_menu, menu);
            return true;
        }

        
    }
}