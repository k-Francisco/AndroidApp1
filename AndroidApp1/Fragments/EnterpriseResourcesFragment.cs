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
    public class EnterpriseResourcesFragment : Fragment
    {

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        EnterpriseR mEnterprise;
        EnterpriseResourceAdapter adapter;
        MainActivity main;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
             View view =  inflater.Inflate(Resource.Layout.empty_recycleview, container, false);
            main = Activity as MainActivity;

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(view.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mEnterprise = new EnterpriseR();
            foreach (var item in main.enterpriseResources.D.Results)
            {
                mEnterprise.addItems(item.Name, item.Email, item.IsActive);
            }

            adapter = new EnterpriseResourceAdapter(mEnterprise, main);
            mRecyclerView.SetAdapter(adapter);
            return view;
           
        }
    }
}