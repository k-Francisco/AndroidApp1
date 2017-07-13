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
using Android.Util;

namespace AndroidApp1.Adapters
{
    public interface SupperClass {

        int GetListItemType();
    }

    public class TimesheetPeriodModel : SupperClass
    {
        public List<string> periodTemp { get; set; }
        public int currentDayPosition { get; set; }

        public int GetListItemType()
        {
            return 1;
        }
    }

    public class TimesheetLineModel : SupperClass
    {
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string Comment { get; set; }
        public string BillingCategory { get; set; }
        public string ProcessStatus { get; set; }
        public string TotalWork { get; set; }

        public int GetListItemType()
        {
            return 2;
        }
    }

    public class TimesheetPeriodz {

        List<SupperClass> periodList = new List<SupperClass> { };
        bool once = false;
        public TimesheetPeriodz(){}

        public void addPeriod(List<string> period, int current) {
            periodList.Add(new TimesheetPeriodModel() { periodTemp = period, currentDayPosition = current });
        }

        public void addPeriod(string projectName, string taskName, string comment, string billing, string status, string totalWork) {
            periodList.Add(new TimesheetLineModel() { ProjectName = projectName, TaskName = taskName, Comment = comment, BillingCategory = billing, ProcessStatus = status, TotalWork = totalWork});
        }

        public void removeItems(TimesheetPeriodAdapter adapter) {
            int items = numHome;
            periodList.Clear();
            adapter.NotifyItemRangeRemoved(1, items - 1);
        }

        public bool getBool() {
            return once;
        }

        public void changeBool() {
            once = true;
        }

        public int numHome {
            get { return periodList.Count; }
        }

        public SupperClass this[int i]
        {
            get { return periodList[i]; }
        }

    }

    public class TimesheetperiodViewHolder : RecyclerView.ViewHolder
    {
        public Spinner mPeriod { get; set; }
        public Button mSettings { get; set; }

        public TimesheetperiodViewHolder(View view, Action<int> listener) : base(view)
        {
            mPeriod = view.FindViewById<Spinner>(Resource.Id.spnrTimesheetPeriod);
            mSettings = view.FindViewById<Button>(Resource.Id.ivTimesheetPeriodSettings);
            //view.LongClick += (sender, e) => { };
            //  mPeriodDay = view.FindViewById<Spinner>(Resource.Id.spnrTimesheetPeriodDay);
            
        }
    }

    public class TimesheetLineViewHolder : RecyclerView.ViewHolder {

        public TextView mProjectName { get; set; }
        public TextView mTaskName { get; set; }
        public TextView mComment { get; set; }
        public TextView mBilling { get; set; }
        public TextView mStatus { get; set; }
        public TextView mTotalWork { get; set; }

        public TimesheetLineViewHolder(View view, Action<int> listener) : base(view) {

            mProjectName = view.FindViewById<TextView>(Resource.Id.tvTimesheetProjectName);
            mTaskName = view.FindViewById<TextView>(Resource.Id.tvTimesheetTaskName);
            mComment = view.FindViewById<TextView>(Resource.Id.tvTimesheetComment);
            mBilling = view.FindViewById<TextView>(Resource.Id.tvTimesheetBillingCategory);
            mStatus = view.FindViewById<TextView>(Resource.Id.tvTimesheetProcessStatus);
            mTotalWork = view.FindViewById<TextView>(Resource.Id.tvTimesheetTotalWork);

        }

    }

    public class TimesheetPeriodAdapter : RecyclerView.Adapter {

        public EventHandler<int> itemClick;
        public TimesheetPeriodz mTimesheetPeriod;
        public MainActivity main;
        public TimesheetFragment frag;
        //List<string> temp = new List<string> { };

        public TimesheetPeriodAdapter(TimesheetPeriodz period, MainActivity main, TimesheetFragment frag) {

            this.mTimesheetPeriod = period;
            this.main = main;
            this.frag = frag;
        }

        public override int ItemCount {
            get { return mTimesheetPeriod.numHome; }
        }

        public override int GetItemViewType(int position)
        {
            return mTimesheetPeriod[position].GetListItemType();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            switch (mTimesheetPeriod[position].GetListItemType()) {

                case 1:
                    TimesheetperiodViewHolder vh1 = holder as TimesheetperiodViewHolder;
                    var periodAdapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, (mTimesheetPeriod[position] as TimesheetPeriodModel).periodTemp);
                    vh1.mPeriod.Adapter = periodAdapter;
                    vh1.mPeriod.SetSelection((mTimesheetPeriod[position] as TimesheetPeriodModel).currentDayPosition);
                    vh1.mPeriod.ItemSelected += (sender, e) =>
                    {
                        frag.persist(e.Position);
                    };
                    if (mTimesheetPeriod.getBool() == false) {
                        vh1.mSettings.Click += delegate { frag.OpenSettings((mTimesheetPeriod[position] as TimesheetPeriodModel).currentDayPosition); };
                        mTimesheetPeriod.changeBool();
                    }
                    
                    break;
                case 2:
                    TimesheetLineViewHolder vh2 = holder as TimesheetLineViewHolder;
                    vh2.mProjectName.Text = (mTimesheetPeriod[position] as TimesheetLineModel).ProjectName;
                    vh2.mTaskName.Text = (mTimesheetPeriod[position] as TimesheetLineModel).TaskName;
                    vh2.mComment.Text = (mTimesheetPeriod[position] as TimesheetLineModel).Comment;
                    vh2.mBilling.Text = (mTimesheetPeriod[position] as TimesheetLineModel).BillingCategory;
                    vh2.mStatus.Text = (mTimesheetPeriod[position] as TimesheetLineModel).ProcessStatus;
                    vh2.mTotalWork.Text = (mTimesheetPeriod[position] as TimesheetLineModel).TotalWork;
                    vh2.ItemView.LongClick += (sender, e) => { frag.LongClick(position); };

                    break;
            }
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = null;
            switch (viewType) {

                case 1:
                    view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.timesheet_fragment, parent, false);
                    return new TimesheetperiodViewHolder(view, ItemClick);
                case 2:
                    view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.timesheet_line_card, parent, false);
                    return new TimesheetLineViewHolder(view, ItemClick);

                default: return null;
            }
            
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