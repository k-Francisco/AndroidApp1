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
    public interface facewash {

        int GetListItemType();
    }

    public class TimesheetPeriodModel : facewash
    {
        public List<string> periodTemp { get; set; }
        public int currentDayPosition { get; set; }

        public int GetListItemType()
        {
            return 1;
        }
    }

    public class TimesheetPeriodz {

        List<TimesheetPeriodModel> periodList = new List<TimesheetPeriodModel> { };

        public TimesheetPeriodz(){}

        public void addPeriod(List<string> period, int current) {
            periodList.Add(new TimesheetPeriodModel() { periodTemp = period, currentDayPosition = current});
        }

        public int numHome {
            get { return periodList.Count; }
        }

        public TimesheetPeriodModel this[int i]
        {
            get { return periodList[i]; }
        }

    }

    public class TimesheetperiodViewHolder : RecyclerView.ViewHolder
    {
        public Spinner mPeriod { get; set; }
        public Spinner mPeriodDay { get; set; }

        public TimesheetperiodViewHolder(View view, Action<int> listener) : base(view)
        {
            mPeriod = view.FindViewById<Spinner>(Resource.Id.spnrTimesheetPeriod);
            mPeriodDay = view.FindViewById<Spinner>(Resource.Id.spnrTimesheetPeriodDay);
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

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            //temp = mTimesheetPeriod[position].periodTemp;
            TimesheetperiodViewHolder vh1 = holder as TimesheetperiodViewHolder;
            var periodAdapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, mTimesheetPeriod[position].periodTemp);
            vh1.mPeriod.Adapter = periodAdapter;
            vh1.mPeriod.SetSelection(mTimesheetPeriod[position].currentDayPosition);
            vh1.mPeriod.ItemSelected += (sender, e)=> { frag.fillPeriodDays(vh1.mPeriodDay, e.Position); };
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.timesheet_fragment, parent, false);
            TimesheetperiodViewHolder vh1 = new TimesheetperiodViewHolder(view, ItemClick);
            return vh1;
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