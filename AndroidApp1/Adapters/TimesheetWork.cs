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
using AndroidApp1.Activities;
using AndroidApp1.Fragments;

namespace AndroidApp1.Adapters
{
    public class TimesheetWorkModel{
        public DateTime startDate { get; set; }
        public string actualHours { get; set; }
        public string plannedHours { get; set; }
    }

    public class TimesheetWorkz {
        List<TimesheetWorkModel> workList = new List<TimesheetWorkModel> { };

        public TimesheetWorkz() { }

        public void addWork(DateTime start, string actual, string planned) {
            workList.Add(new TimesheetWorkModel() { startDate = start, actualHours = actual, plannedHours = planned});
        }

        public void updateWork(DateTime start, string actual, string planned) {
            var worker = workList.Where(w => w.startDate == start).FirstOrDefault();
            if (worker != null) {
                worker.startDate = start;
                worker.actualHours = actual;
                worker.plannedHours = planned;
            }
        }

        public List<TimesheetWorkModel> getList() { return workList; }

        public int numHome {
            get { return workList.Count; }
        }

        public TimesheetWorkModel this[int i] {
            get { return workList[i]; }
        }
    }

    public class TimesheetWorkViewHolder : RecyclerView.ViewHolder {

        public TextView start { get; set; }
        public EditText actual { get; set; }
        public EditText planned { get; set; }

        public TimesheetWorkViewHolder(View view, Action<int>listener):base(view)
        {
            start = view.FindViewById<TextView>(Resource.Id.tvTimesheetLineDay);
            actual = view.FindViewById<EditText>(Resource.Id.etTimesheetLineActual);
            planned = view.FindViewById<EditText>(Resource.Id.etTimesheetLinePlanned);
           
        }
    }

    public class TimesheetWorkAdapter : RecyclerView.Adapter {

        public EventHandler<int> itemClick;
        public TimesheetWorkz mTimesheetWork;

        public TimesheetWorkAdapter(TimesheetWorkz work) {
            this.mTimesheetWork = work;
        }

        public override int ItemCount {
            get { return mTimesheetWork.numHome; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TimesheetWorkViewHolder vh = holder as TimesheetWorkViewHolder;
            vh.start.Text = mTimesheetWork[position].startDate.ToLongDateString();
            vh.actual.Hint = mTimesheetWork[position].actualHours;
            vh.planned.Hint = mTimesheetWork[position].plannedHours;
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.timesheet_line_hours, parent, false);
            TimesheetWorkViewHolder vh = new TimesheetWorkViewHolder(view, ItemClick);
            return vh;
        }

        private void ItemClick(int obj)
        {
            if (itemClick != null)
            {
                itemClick(this, obj);
            }
        }
    }
}