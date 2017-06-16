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
using AndroidApp1.Activities;
using Android.Support.Design.Widget;
using PScore;
using Android.Util;

namespace AndroidApp1.Helpers
{
    class DialogHelpers
    {
        Android.Support.V7.App.AlertDialog builder;

        public Android.Support.V7.App.AlertDialog AddProjectDialog(MainActivity main, View view)
        {

            PsCore core = main.getCore();
            builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();
            builder.SetView(view);
            builder.SetTitle("Add Project");

            TextInputLayout projectNameWrapper = view.FindViewById<TextInputLayout>(Resource.Id.projectNameWrapper);
            TextInputLayout projectDescWrapper = view.FindViewById<TextInputLayout>(Resource.Id.projectDescWrapper);
            EditText projectName = view.FindViewById<EditText>(Resource.Id.etProjectName);
            EditText projectDesc = view.FindViewById<EditText>(Resource.Id.etProjectDescription);

            projectNameWrapper.Hint = "Project Name";
            projectDescWrapper.Hint = "Project Description";

            Spinner spnrEPT = view.FindViewById<Spinner>(Resource.Id.spnrAddProject);

            builder.SetCanceledOnTouchOutside(false);
            builder.SetButton(-1, "SUBMIT",
                (submit, e) =>
                {
                    Toast.MakeText(main, "adding shits", ToastLength.Short);
                    //string ept;
                    //if (spnrEPT.SelectedItem.Equals("Enterprise Project"))
                    //    ept = "09fa52b4-059b-4527-926e-99f9be96437a";
                    //else
                    //    ept = "f4066fec-bd67-4db9-8e6f-9cb3d3b297a6";
                    //String body = "{'parameters': {'Name': '"+projectName.Text+"', 'Description': '"+projectDesc.Text+"', 'EnterpriseProjectTypeId': '"+ept+"'} }";
                    //bool success = await core.AddProjects(body);
                    //if (success == true)
                    //    Log.Info("kfsama", "project added");
                    ////
                    //else
                    //    Log.Info("kfsama","error adding project");
                    //    //Toast.MakeText(main, "There was an error adding the project", ToastLength.Short);
                });
            builder.SetButton(-2, "CANCEL", delegate { builder.Dismiss();});

            return builder;
        }

    }
}