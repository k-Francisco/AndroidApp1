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
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Newtonsoft.Json;
using AndroidApp1.Adapters;
using PScore;
using System.Threading;
using Android.Util;
using System.Threading.Tasks;

namespace AndroidApp1.Activities
{
    [Activity(Label = "TimesheetActivity")]
    public class TimesheetActivity : AppCompatActivity
    {
        string periodId;
        string lineId;
        string rtFa = "";
        string FedAuth = "";
        List<DateTime> days;
        TimesheetWork.RootObject work;
        PsCore core;
        string username;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.timesheet_line_dialog);

            periodId = Intent.GetStringExtra("periodId");
            lineId = Intent.GetStringExtra("lineId");
            days = JsonConvert.DeserializeObject<List<DateTime>>(Intent.GetStringExtra("days"));
            work = JsonConvert.DeserializeObject<TimesheetWork.RootObject>(Intent.GetStringExtra("lineWork"));
            rtFa = Intent.GetStringExtra("rtFa");
            FedAuth = Intent.GetStringExtra("FedAuth");
            string formDigest = Intent.GetStringExtra("FormDigest");
            core = new PsCore(rtFa, FedAuth);
            core.setClient();
            core.setClient2(formDigest);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            RecyclerView recyclerView = FindViewById<RecyclerView>(Resource.Id.rvTimesheetLineHours);
            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);

            TimesheetWorkz workz = new TimesheetWorkz();
            for (int i = 0; i < days.Count; i++) {
                workz.addWork(days[i], "0h", "0h");
            }

            for (int i = 0; i < work.D.Results.Count; i++) {
                workz.updateWork(work.D.Results[i].Start, work.D.Results[i].ActualWork, work.D.Results[i].PlannedWork);
            }

            TimesheetWorkAdapter adapter = new TimesheetWorkAdapter(workz);
            recyclerView.SetAdapter(adapter);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId) {

                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.mnSave:
                   
                    return true;

                default: return base.OnOptionsItemSelected(item);
            }
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.timesheet_menu, menu);
            return true;
        }
    }
}