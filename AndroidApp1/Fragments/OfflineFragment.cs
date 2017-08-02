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

namespace AndroidApp1.Fragments
{
    public class OfflineFragment : Fragment
    {

        TextView mText;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.offline_layout, container, false);

            mText = view.FindViewById<TextView>(Resource.Id.textView1);
            if ((Activity as MainActivity) != null)
                mText.Text = (Activity as MainActivity).mText;
            else
                mText.Text = "The device is offline.Please connect to the internet and restart the app";
            return view;
        }
    }
}