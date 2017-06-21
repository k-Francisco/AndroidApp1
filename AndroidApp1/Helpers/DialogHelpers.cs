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
using Android.Graphics;
using System.Threading;

namespace AndroidApp1.Helpers
{
    class DialogHelpers
    {
        

        public Android.Support.V7.App.AlertDialog AddProjectDialog(MainActivity main, View view)
        {
            Android.Support.V7.App.AlertDialog builder;

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

            projectName.Click += (sender, e) => {
                if (projectName.IsFocused)
                {
                    projectName.Background.ClearColorFilter();
                }
            };
            

            Spinner spnrEPT = view.FindViewById<Spinner>(Resource.Id.spnrAddProject);
            builder.SetCanceledOnTouchOutside(false);
            builder.SetButton(-2, "SUBMIT", delegate
            {
                Toast.MakeText(main, "Adding Project ...", ToastLength.Short).Show();
                
                if (projectName.Text != "")
                {
                    
                    string ept;
                    if (spnrEPT.SelectedItem.Equals("Enterprise Project"))
                        ept = "09fa52b4-059b-4527-926e-99f9be96437a";
                    else
                        ept = "f4066fec-bd67-4db9-8e6f-9cb3d3b297a6";

                    String body = "{'parameters': {'Name': '" + projectName.Text + "', 'Description': '" + projectDesc.Text + "', 'EnterpriseProjectTypeId': '" + ept + "'} }";

                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        bool success = await core.AddProjects(body);
                        if (success == true) {
                            main.RunOnUiThread(() => { Toast.MakeText(main, "Project added successfully", ToastLength.Short).Show(); });
                            main.refreshData();
                        }
                           
                        else
                            main.RunOnUiThread(()=>{ Toast.MakeText(main, "There was an error adding the project", ToastLength.Short).Show(); });
                    });
                    

                }

                else
                {
                    projectName.Background.SetColorFilter(Color.Red, PorterDuff.Mode.SrcIn);
                    projectName.RequestFocus();
                    return;
                }

            });
            builder.SetButton(-1, "CANCEL", delegate { builder.Dismiss();});

            return builder;
        }

        public Android.Support.V7.App.AlertDialog DeleteProjectDialog(DetailsActivity details) {

            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(details).Create();
            builder.SetTitle("Project Title");
            builder.SetMessage("Are you sure you want to delete this project?");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetButton(-2, "DELETE", delegate { details.deleteProject(); });
            builder.SetButton(-1, "CANCEL", delegate { builder.Dismiss(); });

            return builder;
        }

    }
}