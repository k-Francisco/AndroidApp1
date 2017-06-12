using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using AndroidApp1.Activities;

namespace AndroidApp1.Fragments
{
    public class LoadingFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.loading_layout, null);
            return rootView;
        }
    }
}