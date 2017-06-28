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
using Newtonsoft.Json;

namespace AndroidApp1.Helpers
{
    class DialogHelpers
    {

        public Android.Support.V7.App.AlertDialog AddProjectDialog(MainActivity main,PsCore core,View view)
        {
            Android.Support.V7.App.AlertDialog builder;

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
            builder.SetButton(-2, "ADD", delegate
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
                            main.RunOnUiThread(() => { Toast.MakeText(main, "Project added successfully", ToastLength.Short).Show(); main.refreshData(1); });
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


        public Android.Support.V7.App.AlertDialog EditProjectDialog(DetailsActivity details, View view, string projectJson) {

            string body = "";

            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(details).Create();
            builder.SetTitle("Edit Project");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);

            ThreadPool.QueueUserWorkItem(state => {
                var data = JsonConvert.DeserializeObject<ProjectModel.RootObject>(projectJson);

            });

            return builder;
        }


        public Android.Support.V7.App.AlertDialog DeleteProjectDialog(DetailsActivity details, string title) {

            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(details).Create();
            builder.SetTitle(title);
            builder.SetMessage("Are you sure you want to delete this project?");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetButton(-2, "DELETE", delegate { details.deleteProject(); });
            builder.SetButton(-1, "CANCEL", delegate { builder.Dismiss(); });

            return builder;
        }


        public Android.Support.V7.App.AlertDialog AddTaskDialog(MainActivity main,PsCore core,View view, ProjectModel.RootObject projects) {

            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();
            builder.SetView(view);
            builder.SetTitle("Add Task");
            builder.Window.SetSoftInputMode(SoftInput.AdjustResize);

            TextInputLayout taskNameWrapper = view.FindViewById<TextInputLayout>(Resource.Id.taskNameWrapper);
            TextInputLayout taskNotesWrapper = view.FindViewById<TextInputLayout>(Resource.Id.taskNotesWrapper);
            TextInputLayout taskStartDateWrapper = view.FindViewById<TextInputLayout>(Resource.Id.taskStartDateWrapper);
            TextInputLayout taskFinishDateWrapper = view.FindViewById<TextInputLayout>(Resource.Id.taskFinishDateWrapper);
            taskNameWrapper.Hint = "Task Name";
            taskNotesWrapper.Hint = "Task Notes";
            taskStartDateWrapper.Hint = "Start Date";
            taskFinishDateWrapper.Hint = "Finish Date";

            EditText taskName = view.FindViewById<EditText>(Resource.Id.etTaskName);
            EditText taskNotes = view.FindViewById<EditText>(Resource.Id.etTaskNotes);
            EditText taskStartDate = view.FindViewById<EditText>(Resource.Id.etTaskStartDate);
            EditText taskFinishDate = view.FindViewById<EditText>(Resource.Id.etTaskFinishDate);

            taskStartDate.Click += delegate {
                DateTime today = DateTime.Today;
                DatePickerDialog datePicker = new DatePickerDialog(main, (sender, e) => { taskStartDate.Text = e.Date.ToShortDateString() + " " +  e.Date.ToShortTimeString(); }, today.Year, today.Month - 1, today.Day);
                datePicker.DatePicker.MinDate = today.Millisecond;
                datePicker.Show();
            };

            taskFinishDate.Click += delegate
            {
                DateTime today = DateTime.Today;
                DatePickerDialog datePicker = new DatePickerDialog(main, (sender, e) => { taskFinishDate.Text = e.Date.ToShortDateString() + " " + e.Date.ToShortTimeString(); }, today.Year, today.Month - 1, today.Day);
                datePicker.DatePicker.MinDate = today.Millisecond;
                datePicker.Show();
            };

            Spinner taskScheduling = view.FindViewById<Spinner>(Resource.Id.spnrTaskScheduling);
            Spinner projectNames = view.FindViewById<Spinner>(Resource.Id.spnrAddTask);
            List<string> projectNameList = new List<string> { };
            for (int i = 0; i < projects.D.Results.Count; i++) {
                projectNameList.Add(projects.D.Results[i].ProjectName);
            }
            var spinnerAdapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, projectNameList);
            projectNames.Adapter = spinnerAdapter;

            builder.SetButton(-1, "CANCEL", delegate { builder.Dismiss(); });
            builder.SetButton(-2, "ADD", delegate {

                Toast.MakeText(main, "Adding the task ...", ToastLength.Short).Show();

                string projectId = "";
                string temp = projectNames.SelectedItem.ToString();
                for (int i = 0; i < projects.D.Results.Count; i++)
                {
                    if (temp == projects.D.Results[i].ProjectName) {
                        projectId = projects.D.Results[i].ProjectId;
                        break;
                    }
                }

                bool scheduling;
                if (taskScheduling.SelectedItem.Equals("Manual"))
                    scheduling = true;
                else
                    scheduling = false;

                var body = "{'parameters':{'Id':'" + Guid.NewGuid() + "', 'Name':'"+taskName.Text+"', 'Notes':'"+taskNotes.Text+"', 'Start':'" + taskStartDate.Text + "', 'Finish':'" + taskFinishDate.Text + "', 'IsManual':'"+scheduling+"' } }";
                ThreadPool.QueueUserWorkItem(async state =>
                {

                    bool success = await core.AddTask(body, projectId);
                    if (success)
                        main.RunOnUiThread(()=> { Toast.MakeText(main, "Successfully added the task! Publish the project to see the changes", ToastLength.Short).Show(); });
                    else
                        main.RunOnUiThread(() => { Toast.MakeText(main, "There was a problem adding the task", ToastLength.Short).Show(); });
                });

            });
            return builder;
        }


        public Android.Support.V7.App.AlertDialog DeleteTaskDialog(MainActivity main, string projectName, string taskName)
        {

            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();
            builder.SetTitle(taskName);
            builder.SetMessage("Are you sure you want to delete this task?");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetButton(-2, "DELETE", delegate { main.ModifyTask(1, projectName, taskName, ""); });
            builder.SetButton(-1, "CANCEL", delegate { builder.Dismiss(); });

            return builder;
        }


        public Android.Support.V7.App.AlertDialog EditTaskDialog(MainActivity main, string projectName, string taskName) {

            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();
            View view = LayoutInflater.From(main).Inflate(Resource.Layout.edit_task_dialog, null);
            builder.SetView(view);
            builder.SetTitle("Edit Task");
            builder.Window.SetSoftInputMode(SoftInput.AdjustResize);

            EditText taskNameEdit = view.FindViewById<EditText>(Resource.Id.etTaskNameEdit);
            EditText taskNotes = view.FindViewById<EditText>(Resource.Id.etTaskNotesEdit);
            EditText taskStartDate = view.FindViewById<EditText>(Resource.Id.etTaskStartDateEdit);
            EditText taskFinishDate = view.FindViewById<EditText>(Resource.Id.etTaskFinishDateEdit);
            EditText taskWork = view.FindViewById<EditText>(Resource.Id.etTaskWorkEdit);
            EditText taskPercentComplete = view.FindViewById<EditText>(Resource.Id.etTaskPercentCompleteEdit);

            taskStartDate.Click += delegate {
                DateTime today = DateTime.Today;
                DatePickerDialog datePicker = new DatePickerDialog(main, (sender, e) => { taskStartDate.Text = e.Date.ToShortDateString() + " " + e.Date.ToShortTimeString(); }, today.Year, today.Month - 1, today.Day);
                datePicker.DatePicker.MinDate = today.Millisecond;
                datePicker.Show();
            };

            taskFinishDate.Click += delegate
            {
                DateTime today = DateTime.Today;
                DatePickerDialog datePicker = new DatePickerDialog(main, (sender, e) => { taskFinishDate.Text = e.Date.ToShortDateString() + " " + e.Date.ToShortTimeString(); }, today.Year, today.Month - 1, today.Day);
                datePicker.DatePicker.MinDate = today.Millisecond;
                datePicker.Show();
            };

            Spinner taskScheduling = view.FindViewById<Spinner>(Resource.Id.spnrTaskSchedulingEdit);

            string scheduling = null;
            bool isManual = false;
            string projectid = null;
            string taskid = null;
            List<Taskmodel.RootObject> taskList = main.getTaskList();
            ProjectModel.RootObject projects = main.getProjectList();

            for (int i = 0; i < projects.D.Results.Count; i++) {
                if (projects.D.Results[i].ProjectName.Equals(projectName)) {
                    projectid = projects.D.Results[i].ProjectId;
                    break;
                }
            }

            for (int i = 0; i < taskList.Count; i++) {
                for (int j = 0; j < taskList[i].D.Results.Count; j++) {
                    if (taskList[i].D.Results[j].Name.Equals(taskName)) {
                        taskid = taskList[i].D.Results[j].Id;
                        /////////////////////////////
                        taskNameEdit.Hint = taskList[i].D.Results[j].Name;
                        taskNotes.Hint = taskList[i].D.Results[j].Notes.ToString();
                        taskStartDate.Hint = taskList[i].D.Results[j].Start.ToString();
                        taskFinishDate.Hint = taskList[i].D.Results[j].Finish.ToString();
                        taskWork.Hint = taskList[i].D.Results[j].Work;
                        taskPercentComplete.Hint = taskList[i].D.Results[j].PercentComplete.ToString();

                        if (taskList[i].D.Results[j].IsManual)
                        {
                            taskScheduling.SetSelection(0);
                            scheduling = "Manual";
                            isManual = true;
                        }

                        else
                        {
                            taskScheduling.SetSelection(1);
                            scheduling = "Automatic";
                            isManual = false;
                        }
                            
                        /////////////////////////////
                        break;
                    }
                }
            }

            StringBuilder body = new StringBuilder();
            string intro = "{ \"__metadata\":{ \"type\":\"PS.DraftTask\"},";
            List<string> inputs = new List<string> { };

            builder.SetButton(-1, "CANCEL", delegate { builder.Dismiss(); });
            builder.SetButton(-2, "EDIT", delegate {

                if (taskNameEdit.Text != "" && taskNameEdit.Text != taskNameEdit.Hint)
                    inputs.Add("'Name':'" + taskNameEdit.Text + "'");

                if (taskNotes.Text != "" && taskNotes.Text != taskNotes.Hint)
                    inputs.Add("'Notes':'"+taskNotes.Text+"'");

                if (taskStartDate.Text != "" && taskStartDate.Text != taskStartDate.Hint)
                    inputs.Add("'Start':'" + taskStartDate.Text + "'");

                if (taskFinishDate.Text != "" && taskFinishDate.Text != taskFinishDate.Hint)
                    inputs.Add("'Finish':'" + taskFinishDate.Text + "'");

                if (taskPercentComplete.Text != "" && taskPercentComplete.Text != taskPercentComplete.Hint)
                    inputs.Add("'PercentComplete':'" + taskPercentComplete.Text + "'");

                if (taskWork.Text != "" && taskWork.Text != taskWork.Hint)
                    inputs.Add("'Work':'" + taskWork.Text + "h'");

                if (!taskScheduling.SelectedItem.Equals(scheduling))
                    inputs.Add("'IsManual':'" + !isManual + "'");

                if(inputs.Count != 0)
                    body.Append(intro);

                for (int i = 0; i < inputs.Count; i++) {
                    if (i != inputs.Count - 1)
                    {
                        body.Append(inputs[i] + ",");
                    }
                    else {
                        body.Append(inputs[i] + "}");
                    }
                }
                main.ModifyTask(2, projectid, taskid, body.ToString());
                    
            });

            return builder;
        }

    }
}