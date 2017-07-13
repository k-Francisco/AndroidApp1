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
using AndroidApp1.Fragments;
using Android.Support.V7.Widget;
using AndroidApp1.Adapters;

namespace AndroidApp1.Helpers
{
    class DialogHelpers
    {

        public Android.Support.V7.App.AlertDialog AddProjectDialog(MainActivity main, PsCore core, View view)
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
                            main.RunOnUiThread(() => { Toast.MakeText(main, "There was an error adding the project", ToastLength.Short).Show(); });
                    });


                }

                else
                {
                    projectName.Background.SetColorFilter(Color.Red, PorterDuff.Mode.SrcIn);
                    projectName.RequestFocus();
                    return;
                }

            });
            builder.SetButton(-1, "CANCEL", delegate { builder.Dismiss(); });

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


        public Android.Support.V7.App.AlertDialog AddTaskDialog(MainActivity main, PsCore core, View view, ProjectData.RootObject projects) {

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

            Spinner taskScheduling = view.FindViewById<Spinner>(Resource.Id.spnrTaskScheduling);
            Spinner projectNames = view.FindViewById<Spinner>(Resource.Id.spnrAddTask);
            List<string> projectNameList = new List<string> { };
            for (int i = 0; i < projects.D.Results.Count; i++) {
                projectNameList.Add(projects.D.Results[i].Name);
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
                    if (temp == projects.D.Results[i].Name) {
                        projectId = projects.D.Results[i].Id;
                        break;
                    }
                }

                bool scheduling;
                if (taskScheduling.SelectedItem.Equals("Manual"))
                    scheduling = true;
                else
                    scheduling = false;

                var body = "{'parameters':{'Id':'" + Guid.NewGuid() + "', 'Name':'" + taskName.Text + "', 'Notes':'" + taskNotes.Text + "', 'Start':'" + taskStartDate.Text + "', 'Finish':'" + taskFinishDate.Text + "', 'IsManual':'" + scheduling + "' } }";
                ThreadPool.QueueUserWorkItem(async state =>
                {

                    bool success = await core.AddTask(body, projectId);
                    if (success)
                        main.RunOnUiThread(() => { Toast.MakeText(main, "Successfully added the task! Publish the project to see the changes", ToastLength.Short).Show(); });
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
            ProjectData.RootObject projects = main.getProjectServerList();

            for (int i = 0; i < projects.D.Results.Count; i++) {
                if (projects.D.Results[i].Name.Equals(projectName)) {
                    projectid = projects.D.Results[i].Id;
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
                    inputs.Add("'Notes':'" + taskNotes.Text + "'");

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

                if (inputs.Count != 0)
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

        public Android.Support.V7.App.AlertDialog OpensSettingsDialog(PsCore core, MainActivity main, string id) {

            View view = LayoutInflater.From(main).Inflate(Resource.Layout.empty_listview, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();

            ListView lvOptions = view.FindViewById<ListView>(Resource.Id.lvTimesheetSettings);
            List<string> items = new List<string> { "Timesheet Details","Submit Timesheet", "Recall Timesheet", "Save Timesheet" };
            var adapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, items);
            lvOptions.Adapter = adapter;
            lvOptions.ItemClick += async (sender, e) =>
            {
                switch (e.Position)
                {
                    case 0:
                        break;
                    case 1:
                        SubmitTimesheet(core, main, id).Show();
                        builder.Dismiss();
                        break;
                    case 2:
                        Toast.MakeText(main, "Recalling timesheet...!", ToastLength.Short).Show();
                        bool success = await core.RecallTimesheet("", id);
                        if (success) 
                            Toast.MakeText(main, "Timesheet recalled!", ToastLength.Short).Show();
                        else
                            Toast.MakeText(main, "There was an error recalling the timesheet", ToastLength.Short).Show();

                        builder.Dismiss();
                        break;
                    case 3:
                        //bool exists = false;
                        //var response = await core.GetCustomLists();
                        //var data = JsonConvert.DeserializeObject<Custom_Lists.RootObject>(response);
                        //for (int i = 0; i < data.D.Results.Count; i++)
                        //{
                        //    if (data.D.Results[i].Id == id)
                        //    {
                        //        exists = true;
                        //        break;
                        //    }
                        //}

                        //    if (exists == false)
                        //    {
                        //        string body = "{'__metadata':{'type':'SP.List'}, 'AllowContentTypes': true, 'BaseTemplate':100, 'ContentTypesEnabled':true, 'Description':'My Description', 'Title':'"+username.Trim()+"_SavedTimesheets'}";

                        //        bool isSuccess = await core.CreateCustomList(body);
                        //        if (isSuccess)
                        //            RunOnUiThread(()=> { Toast.MakeText(this, "LIST SUCCESSFULLY CREATED", ToastLength.Short).Show(); });
                        //        else
                        //            RunOnUiThread(() => { Toast.MakeText(this, "LIST CREATION ERROR", ToastLength.Short).Show(); });

                        //    }
                        //    else
                        //    {
                        //        //update list for user and in the shared prefs;
                        //        //update the trigger for syncing saved timesheets
                        //    }

                        break;
                }
            };


            
            builder.SetTitle("Options");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);
            builder.SetButton(-1, "CLOSE", delegate { builder.Dismiss(); });

            return builder;
        }

        public Android.Support.V7.App.AlertDialog SubmitTimesheet(PsCore core, MainActivity main, string id) {

            View view = LayoutInflater.From(main).Inflate(Resource.Layout.timesheet_submit_comment_dialog, null);
            EditText comment = view.FindViewById<EditText>(Resource.Id.etTimesheetComment);

            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();
            builder.SetTitle("Comments:");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);
            builder.SetButton(-2, "SUBMIT", async delegate {
                Toast.MakeText(main, "Submitting timesheet...", ToastLength.Short).Show();
                bool success = await core.SubmitTimesheet("",id, comment.Text);
                if (success)
                {
                    Toast.MakeText(main, "Timesheet Submitted!", ToastLength.Short).Show();
                    builder.Dismiss();
                }
                else {
                    Toast.MakeText(main, "There was an error submitting the timesheet", ToastLength.Short).Show();
                    builder.Dismiss();
                    OpensSettingsDialog(core, main, id).Show();
                }
                    
                
            });
            builder.SetButton(-1, "CLOSE", delegate { builder.Dismiss(); });
            

            return builder;
        }

        public Android.Support.V7.App.AlertDialog AddTimesheetLine(PsCore core, MainActivity main, string id, int position, TimesheetFragment frag) {

            View view = LayoutInflater.From(main).Inflate(Resource.Layout.add_timesheet_line, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();
            builder.SetTitle("Add Line");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);

            TextInputLayout wrapperTaskName = view.FindViewById<TextInputLayout>(Resource.Id.timesheetLineWrapperTaskName);
            TextInputLayout wrapperComments = view.FindViewById<TextInputLayout>(Resource.Id.timesheetLineCommentWrapper);
            wrapperTaskName.Hint = "Task Name";
            wrapperComments.Hint = "Comments";

            EditText taskname = view.FindViewById<EditText>(Resource.Id.etTimesheetLineTaskName);
            EditText comment = view.FindViewById<EditText>(Resource.Id.etTimesheetLineComment);

            StringBuilder body = new StringBuilder();
            string intro = "{ 'parameters':{";
            List<string> inputs = new List<string> { };

            builder.SetButton(-1, "CLOSE", delegate { builder.Dismiss(); });
            builder.SetButton(-2, "ADD", async delegate
            {

                if (taskname.Text != "")
                    inputs.Add("'TaskName':'" + taskname.Text + "'");

                if (comment.Text != "")
                    inputs.Add("'Comment':'" + comment.Text + "'");

                if (inputs.Count >= 1)
                    body.Append(intro);

                for (int i = 0; i < inputs.Count; i++)
                {
                    if (i != inputs.Count - 1)
                        body.Append(inputs[i] + ",");
                    else
                        body.Append(inputs[i] + "} }");
                }

                if (body.ToString() != "")
                {
                    Toast.MakeText(main, "Adding line...", ToastLength.Short).Show();
                    bool success = await core.AddTimesheetLine(body.ToString(), id);
                    if (success)
                    {
                        Toast.MakeText(main, "Line successfully added!", ToastLength.Short).Show();
                        frag.fillTimesheetLines(position);
                    }
                    else
                        Toast.MakeText(main, "There was an error adding the line", ToastLength.Short).Show();
                }

            });

            return builder;
        }

        public Android.Support.V7.App.AlertDialog EditTimesheetLine(PsCore core, MainActivity main, string periodId, string lineId)
        {

            View view = LayoutInflater.From(main).Inflate(Resource.Layout.add_timesheet_line, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();
            builder.SetTitle("Update Line");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);

            TextInputLayout wrapperTaskName = view.FindViewById<TextInputLayout>(Resource.Id.timesheetLineWrapperTaskName);
            TextInputLayout wrapperComments = view.FindViewById<TextInputLayout>(Resource.Id.timesheetLineCommentWrapper);
            wrapperTaskName.Hint = "Task Name";
            wrapperComments.Hint = "Comments";

            EditText taskname = view.FindViewById<EditText>(Resource.Id.etTimesheetLineTaskName);
            EditText comment = view.FindViewById<EditText>(Resource.Id.etTimesheetLineComment);


            builder.SetButton(-1, "CLOSE", delegate { builder.Dismiss(); });
            builder.SetButton(-2, "Update", async delegate
            {

                if (taskname.Text != "") {
                    string body = "{ \"__metadata\":{ \"type\":\"PS.TimeSheetLine\"}, 'TaskName':'"+taskname.Text+"', 'Comment':'"+ comment.Text+"'}";

                    Toast.MakeText(main, "Updating line...", ToastLength.Short).Show();
                    core.AddHeaders(2);
                    bool success = await core.UpdateTimesheetLine(body, periodId, lineId);
                    if (success)
                    {
                        core.AddHeaders(1);
                        Toast.MakeText(main, "Line successfully Updated!", ToastLength.Short).Show();
                    }
                    else
                        Toast.MakeText(main, "There was an error updating the line", ToastLength.Short).Show();
                }

            });

            return builder;
        }

        public Android.Support.V7.App.AlertDialog ShowTimesheetWorkDialog(MainActivity main, PsCore core, string periodId, string lineId, string data, List<DateTime> days, int currentDayPosition, TimesheetFragment frag, string formDigest)
        {
            View view = LayoutInflater.From(main).Inflate(Resource.Layout.empty_listview, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();

            ListView lvOptions = view.FindViewById<ListView>(Resource.Id.lvTimesheetSettings);
            List<string> items = new List<string> { "View Line Details", "Edit Line", "Delete Line", "Close" };
            var adapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, items);
            lvOptions.Adapter = adapter;
            lvOptions.ItemClick += (sender, e) =>
            {
                switch (e.Position)
                {
                    case 0:
                        Intent intent = new Intent(main, typeof(TimesheetActivity));
                        intent.PutExtra("periodId", JsonConvert.SerializeObject(periodId));
                        intent.PutExtra("lineId", JsonConvert.SerializeObject(lineId));
                        intent.PutExtra("lineWork", data);
                        intent.PutExtra("days", JsonConvert.SerializeObject(days));
                        intent.PutExtra("rtFa", core.getRtFa());
                        intent.PutExtra("FedAuth", core.GetFedAuth());
                        intent.PutExtra("FormDigest", formDigest);
                        main.StartActivity(intent);
                        builder.Dismiss();
                        break;
                    case 1:
                        EditTimesheetLine(core, main, periodId, lineId).Show();
                        break;
                    case 2:

                        ThreadPool.QueueUserWorkItem(async state =>
                        {
                            main.RunOnUiThread(()=> { Toast.MakeText(main, "Deleting line ...", ToastLength.Short).Show(); builder.Dismiss(); });
                            bool success = await core.DeleteTimesheetLine("", periodId, lineId);
                            if (success)
                            {
                                main.RunOnUiThread(() => {
                                    Toast.MakeText(main, "Succesfully deleted line!", ToastLength.Short).Show();
                                    frag.fillTimesheetLines(currentDayPosition);
                                });
                            }
                            else
                            {
                                main.RunOnUiThread(() => {
                                    Toast.MakeText(main, "There was a problem deleting the line", ToastLength.Short).Show();
                                });
                            }
                        });

                        break;
                    case 3:
                        builder.Dismiss();
                        break;
                }
            };



            builder.SetTitle("Options");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);

            return builder;
        }

    }
}