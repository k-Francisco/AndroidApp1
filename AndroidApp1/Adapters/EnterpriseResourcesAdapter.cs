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
using Android.Graphics;
using AndroidApp1.Activities;

namespace AndroidApp1.Adapters
{
    public class ERModel
    {
        public string mName { get; set; }
        public string mEmail { get; set; }
        public bool isActive { get; set; }
    }

    public class EnterpriseR {

        List<ERModel> enterpriseRList = new List<ERModel> { };

        public EnterpriseR() { }

        public void addItems(string name, string email, bool active) {
            enterpriseRList.Add(new ERModel() { mName = name, mEmail = email, isActive = active});
        }

        public int numHome
        {
            get { return enterpriseRList.Count; }
        }

        public ERModel this[int i]
        {
            get { return enterpriseRList[i]; }
        }
    }

    public class EnterpriseResourceViewHolder : RecyclerView.ViewHolder {

        public TextView mName { get; set; }
        public TextView mEmail { get; set; }
        public View status { get; set; }
        public LinearLayout mLayout { get; set; }

        public EnterpriseResourceViewHolder(View itemView, Action<int> listener) : base(itemView) {
            mName = itemView.FindViewById<TextView>(Resource.Id.tvEnterpriseResourceName);
            mEmail = itemView.FindViewById<TextView>(Resource.Id.tvEnterpriseResourceEmail);
            status = itemView.FindViewById<View>(Resource.Id.vEnterpriseResourceStatus);
            mLayout = itemView.FindViewById<LinearLayout>(Resource.Id.llEnterpriseResources);
        }
    }

    public class EnterpriseResourceAdapter : RecyclerView.Adapter {

        public EventHandler<int> itemClick;
        public EnterpriseR mEnterprise;
        public MainActivity main;

        public EnterpriseResourceAdapter(EnterpriseR enterprise, MainActivity main) {
            this.mEnterprise = enterprise;
            this.main = main;
        }

        public override int ItemCount => mEnterprise.numHome;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            EnterpriseResourceViewHolder vh = holder as EnterpriseResourceViewHolder;
            vh.mName.Text = mEnterprise[position].mName;
            vh.mEmail.Text = mEnterprise[position].mEmail;
            vh.mLayout.LongClick += delegate { main.helpDialog.EnterpriseResourceOptions(main, position).Show(); };
            if (mEnterprise[position].isActive == false)
                vh.status.SetBackgroundColor(Color.ParseColor("#c0392b"));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.enterprise_resources_layout, parent, false);
            EnterpriseResourceViewHolder vh = new EnterpriseResourceViewHolder(view, ItemClick);
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