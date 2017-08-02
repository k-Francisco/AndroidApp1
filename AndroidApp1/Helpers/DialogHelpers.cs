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
using AndroidApp1.Models.SavedChanges;

namespace AndroidApp1.Helpers
{
    public class DialogHelpers
    {

        public Android.Support.V7.App.AlertDialog ProjectOptionsDialog(MainActivity main, PsCore core, int position) {

            View view = LayoutInflater.From(main).Inflate(Resource.Layout.empty_listview, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();

            ListView lvOptions = view.FindViewById<ListView>(Resource.Id.lvTimesheetSettings);
            List<string> items = new List<string> { "Check Out Project", "Check In Project", "Publish Project", "Edit Project", "Delete Project", "Close" };
            var adapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, items);
            lvOptions.Adapter = adapter;
            lvOptions.ItemClick += (sender, e) =>
            {
                switch (e.Position)
                {
                    case 0:
                        main.projectChecks(1, position);
                        builder.Dismiss();
                        break;
                    case 1:
                        main.projectChecks(2, position);
                        builder.Dismiss();
                        break;
                    case 2:
                        main.projectChecks(3, position);
                        builder.Dismiss();
                        break;
                    case 3:
                        EditProjectDialog(main).Show();
                        builder.Dismiss();
                        break;
                    case 4:
                        DeleteProjectDialog(main).Show();
                        builder.Dismiss();
                        break;
                    case 5:
                        builder.Dismiss();
                        break;

                }
            };



            builder.SetTitle("Options");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);

            return builder;
        }

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


        public Android.Support.V7.App.AlertDialog EditProjectDialog(MainActivity main) {

            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();
            builder.SetTitle("Edit Project");
            builder.SetCanceledOnTouchOutside(false);

            builder.SetButton(-1, "Close", delegate { builder.Dismiss(); });
            return builder;
        }


        public Android.Support.V7.App.AlertDialog DeleteProjectDialog(MainActivity main) {

            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();
            builder.SetMessage("Are you sure you want to delete this project?");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetButton(-2, "DELETE", delegate { main.ModifyProject(1, ""); });
            builder.SetButton(-1, "CANCEL", delegate { builder.Dismiss(); });

            return builder;
        }


        public Android.Support.V7.App.AlertDialog AddTaskDialog(DetailsActivity details) {

            View view = LayoutInflater.From(details).Inflate(Resource.Layout.add_task_layout, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(details).Create();
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
                DatePickerDialog datePicker = new DatePickerDialog(details, (sender, e) => { taskStartDate.Text = e.Date.ToShortDateString() + " " + e.Date.ToShortTimeString(); }, today.Year, today.Month - 1, today.Day);
                datePicker.DatePicker.MinDate = today.Millisecond;
                datePicker.Show();
            };

            taskFinishDate.Click += delegate
            {
                DateTime today = DateTime.Today;
                DatePickerDialog datePicker = new DatePickerDialog(details, (sender, e) => { taskFinishDate.Text = e.Date.ToShortDateString() + " " + e.Date.ToShortTimeString(); }, today.Year, today.Month - 1, today.Day);
                datePicker.DatePicker.MinDate = today.Millisecond;
                datePicker.Show();
            };

            Spinner taskScheduling = view.FindViewById<Spinner>(Resource.Id.spnrTaskScheduling);
            Spinner projectNames = view.FindViewById<Spinner>(Resource.Id.spnrAddTask);
            projectNames.Visibility = ViewStates.Gone;
            //List<string> projectNameList = new List<string> { };
            //for (int i = 0; i < projects.D.Results.Count; i++) {
            //    projectNameList.Add(projects.D.Results[i].Name);
            //}
            //var spinnerAdapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, projectNameList);
            //projectNames.Adapter = spinnerAdapter;

            builder.SetButton(-1, "CANCEL", delegate { builder.Dismiss(); });
            builder.SetButton(-2, "ADD", delegate {

                Toast.MakeText(details, "Adding the task ...", ToastLength.Short).Show();

                //string projectId = "";
                //string temp = projectNames.SelectedItem.ToString();
                //for (int i = 0; i < projects.D.Results.Count; i++)
                //{
                //    if (temp == projects.D.Results[i].Name) {
                //        projectId = projects.D.Results[i].Id;
                //        break;
                //    }
                //}

                bool scheduling;
                if (taskScheduling.SelectedItem.Equals("Manual"))
                    scheduling = true;
                else
                    scheduling = false;

                var body = "{'parameters':{'Id':'" + Guid.NewGuid() + "', 'Name':'" + taskName.Text + "', 'Notes':'" + taskNotes.Text + "', 'Start':'" + taskStartDate.Text + "', 'Finish':'" + taskFinishDate.Text + "', 'IsManual':'" + scheduling + "' } }";
                ThreadPool.QueueUserWorkItem(async state =>
                {
                    bool checkoutSuccess = await details.core.CheckOut("", details.id);
                    if (checkoutSuccess) {
                        bool success = await details.core.AddTask(body, details.id);
                        if (success)
                        {
                            bool publishSuccess = await details.core.Publish("", details.id);
                            if (publishSuccess)
                            {
                                details.RunOnUiThread(() => { Toast.MakeText(details, "Successfully added the task", ToastLength.Short).Show(); });
                            }
                        }
                        else {
                            details.RunOnUiThread(() => { Toast.MakeText(details, "There was a problem adding the task", ToastLength.Short).Show(); });
                        }
                    }
                    else
                        details.RunOnUiThread(() => { Toast.MakeText(details, "There was a problem adding the task", ToastLength.Short).Show(); });

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

        public Android.Support.V7.App.AlertDialog OpensSettingsDialog(PsCore core, MainActivity main, string id, TimesheetFragment frag) {

            View view = LayoutInflater.From(main).Inflate(Resource.Layout.empty_listview, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();

            ListView lvOptions = view.FindViewById<ListView>(Resource.Id.lvTimesheetSettings);
            List<string> items = new List<string> { "Submit Timesheet", "Recall Timesheet", "Save Timesheet" };
            var adapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, items);
            lvOptions.Adapter = adapter;
            lvOptions.ItemClick += async (sender, e) =>
            {
                switch (e.Position)
                {
                    case 0:
                        SubmitTimesheet(core, main, id, frag).Show();
                        builder.Dismiss();
                        break;
                    case 1:
                        Toast.MakeText(main, "Recalling timesheet...!", ToastLength.Short).Show();
                        bool success = await core.RecallTimesheet("", id);
                        if (success)
                            Toast.MakeText(main, "Timesheet recalled!", ToastLength.Short).Show();
                        else
                            Toast.MakeText(main, "There was an error recalling the timesheet", ToastLength.Short).Show();

                        builder.Dismiss();
                        break;
                    case 2:
                        builder.Dismiss();
                        await frag.SaveTimesheetAsync();
                        break;
                }
            };



            builder.SetTitle("Options");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);
            builder.SetButton(-1, "CLOSE", delegate { builder.Dismiss(); });

            return builder;
        }

        public Android.Support.V7.App.AlertDialog SubmitTimesheet(PsCore core, MainActivity main, string id, TimesheetFragment frag) {

            View view = LayoutInflater.From(main).Inflate(Resource.Layout.timesheet_submit_comment_dialog, null);
            EditText comment = view.FindViewById<EditText>(Resource.Id.etTimesheetComment);

            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();
            builder.SetTitle("Comments:");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);
            builder.SetButton(-2, "SUBMIT", async delegate {
                Toast.MakeText(main, "Submitting timesheet...", ToastLength.Short).Show();
                bool success = await core.SubmitTimesheet("", id, comment.Text);
                if (success)
                {
                    Toast.MakeText(main, "Timesheet Submitted!", ToastLength.Short).Show();
                    builder.Dismiss();
                }
                else {
                    Toast.MakeText(main, "There was an error submitting the timesheet", ToastLength.Short).Show();
                    builder.Dismiss();
                    OpensSettingsDialog(core, main, id, frag).Show();
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

            Spinner exAssignments = view.FindViewById<Spinner>(Resource.Id.spnrExistingAssignments);
            List<string> projects = new List<string> { };
            ThreadPool.QueueUserWorkItem(async state =>
            {

                if (main.getProjectList().D.Results.Count != 0)
                {
                    foreach (var item in main.getProjectList().D.Results)
                    {
                        var thereIs = await core.GetProjectResourcesFiltered(item.ProjectId, main.getUserName());
                        if (thereIs.Length > 20) {
                            projects.Add(item.ProjectName);
                        }
                        if (item.ProjectOwnerName.Equals(main.getUserName()) && !projects.Contains(item.ProjectName))
                            projects.Add(item.ProjectName);
                    }
                }
                projects.Add("Personal Task");
                main.RunOnUiThread(() => {
                    var spinnerAdapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, projects);
                    exAssignments.Adapter = spinnerAdapter;
                });
            });

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

                if (exAssignments.SelectedItem.ToString().Equals("Personal Task"))
                {

                }
                else {
                    foreach (var item in main.getProjectList().D.Results)
                    {
                        if (item.ProjectName.Equals(exAssignments.SelectedItem.ToString())) {
                            inputs.Add("'ProjectId':'" + item.ProjectId + "'");
                        }
                    }
                }

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

            wrapperTaskName.Visibility = ViewStates.Gone;

            EditText taskname = view.FindViewById<EditText>(Resource.Id.etTimesheetLineTaskName);
            EditText comment = view.FindViewById<EditText>(Resource.Id.etTimesheetLineComment);

            taskname.Visibility = ViewStates.Gone;

            Spinner exAssignments = view.FindViewById<Spinner>(Resource.Id.spnrExistingAssignments);
            exAssignments.Visibility = ViewStates.Gone;
            TextView label = view.FindViewById<TextView>(Resource.Id.tvSpinnerLabel);
            label.Visibility = ViewStates.Gone;

            builder.SetButton(-1, "CLOSE", delegate { builder.Dismiss(); });
            builder.SetButton(-2, "Update", async delegate
            {

                if (taskname.Text != "") {
                    string body = "{ \"__metadata\":{ \"type\":\"PS.TimeSheetLine\"}, 'TaskName':'" + taskname.Text + "', 'Comment':'" + comment.Text + "'}";

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

        public Android.Support.V7.App.AlertDialog ShowTimesheetWorkDialog(MainActivity main, PsCore core, string periodId, string lineId, string taskName, string data, List<DateTime> days, int currentDayPosition, TimesheetFragment frag, string formDigest)
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
                        intent.PutExtra("taskName", taskName);
                        intent.PutExtra("periodId", periodId);
                        intent.PutExtra("lineId", lineId);
                        intent.PutExtra("lineWork", data);
                        intent.PutExtra("days", JsonConvert.SerializeObject(days));
                        intent.PutExtra("identifier", true);
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
                            main.RunOnUiThread(() => { Toast.MakeText(main, "Deleting line ...", ToastLength.Short).Show(); builder.Dismiss(); });
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

        public Android.Support.V7.App.AlertDialog ShowSavedTimesheetWork(MainActivity main, List<DateTime> days, TimesheetWork.RootObject work, string taskName, string periodId, string lineId)
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
                        intent.PutExtra("taskName",taskName);
                        intent.PutExtra("periodId", periodId);
                        intent.PutExtra("lineId", lineId);
                        intent.PutExtra("lineWork", JsonConvert.SerializeObject(work));
                        intent.PutExtra("days", JsonConvert.SerializeObject(days));
                        intent.PutExtra("identifier", false);
                        main.StartActivity(intent);
                        builder.Dismiss();
                        break;
                    case 1:
                        //EditTimesheetLine(core, main, periodId, lineId).Show();
                        break;
                    case 2:

                        //ThreadPool.QueueUserWorkItem(async state =>
                        //{
                        //    main.RunOnUiThread(() => { Toast.MakeText(main, "Deleting line ...", ToastLength.Short).Show(); builder.Dismiss(); });
                        //    bool success = await core.DeleteTimesheetLine("", periodId, lineId);
                        //    if (success)
                        //    {
                        //        main.RunOnUiThread(() => {
                        //            Toast.MakeText(main, "Succesfully deleted line!", ToastLength.Short).Show();
                        //            frag.fillTimesheetLines(currentDayPosition);
                        //        });
                        //    }
                        //    else
                        //    {
                        //        main.RunOnUiThread(() => {
                        //            Toast.MakeText(main, "There was a problem deleting the line", ToastLength.Short).Show();
                        //        });
                        //    }
                        //});

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

        public Android.Support.V7.App.AlertDialog AddEnterpriseResource(MainActivity main)
        {
            View view = LayoutInflater.From(main).Inflate(Resource.Layout.add_enterprise_resource_layout, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();

            EditText rName = view.FindViewById<EditText>(Resource.Id.etAddEnterpriseResource);
            Spinner rType = view.FindViewById<Spinner>(Resource.Id.spnrAddEnterpriseResource);

            builder.SetButton(-2, "ADD", async delegate
            {
                if (rName.Text != "")
                {
                    Toast.MakeText(main, "Adding resource ...", ToastLength.Short).Show();
                    string body = "{'parameters':{'Id':'" + Guid.NewGuid() + "', 'Name':'" + rName.Text + "', 'ResourceType':'" + (rType.SelectedItemPosition + 1).ToString() + "'}}";
                    bool isSuccess = await main.core.AddEnterpriseResource(body);
                    if (isSuccess)
                    {
                        Toast.MakeText(main, "Successfully added resource", ToastLength.Short).Show();
                        main.enterpriseResources = null;
                        main.checkDataAsync(4);
                    }
                    else
                        Toast.MakeText(main, "There was a problem adding the resource", ToastLength.Short).Show();
                }
            });
            builder.SetButton(-1, "CLOSE", delegate { builder.Dismiss(); });
            builder.SetTitle("Add Enterprise Resource");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);

            return builder;
        }

        public Android.Support.V7.App.AlertDialog EditEnterpriseResource(MainActivity main, int position) {

            View view = LayoutInflater.From(main).Inflate(Resource.Layout.edit_enterprise_resource, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();

            EditText rName = view.FindViewById<EditText>(Resource.Id.etEditEnterpriseResourceName);
            EditText rEmail = view.FindViewById<EditText>(Resource.Id.etEditEnterpriseResourceEmail);
            EditText rManager = view.FindViewById<EditText>(Resource.Id.etEditEnterpriseResourceManager);

            Spinner rType = view.FindViewById<Spinner>(Resource.Id.spnrEditEnterpriseResourceType);
            Spinner rGeneric = view.FindViewById<Spinner>(Resource.Id.spnrEditEnterpriseResourceIsGeneric);
            Spinner rActive = view.FindViewById<Spinner>(Resource.Id.spnrEditEnterpriseResourceIsActive);

            StringBuilder body = new StringBuilder();
            List<string> temp = new List<string> { };
            builder.SetButton(-2, "Update", delegate {
                body.Append("{ '__metadata': {'type': 'PS.EnterpriseResource' }, ");

                if (rName.Text != "")
                    temp.Add("'Name':'" + rName.Text + "'");

                if (rEmail.Text != "")
                    temp.Add("'Email':'" + rEmail.Text + "'");

                temp.Add("'ResourceType':'" + (rType.SelectedItemPosition + 1).ToString() + "'");
                temp.Add("'IsActive':'" + rActive.SelectedItem + "'");
                temp.Add("'IsGeneric':'" + rGeneric.SelectedItem + "'");

                for (int i = 0; i < temp.Count; i++)
                {
                    if (i != temp.Count - 1)
                    {
                        body.Append(temp[i] + ",");
                    }
                    else
                    {
                        body.Append(temp[i] + "}");
                    }
                }
                main.ModifyEnterpriseResources(1, position, body.ToString());

            });
            builder.SetButton(-1, "CLOSE", delegate { builder.Dismiss(); });
            builder.SetTitle("Edit Enterprise Resource");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);
            return builder;

        }

        public Android.Support.V7.App.AlertDialog EnterpriseResourceOptions(MainActivity main, int position) {

            View view = LayoutInflater.From(main).Inflate(Resource.Layout.empty_listview, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();

            ListView lvOptions = view.FindViewById<ListView>(Resource.Id.lvTimesheetSettings);
            List<string> items = new List<string> { "Edit Resource", "Delete Resource", "Close" };
            var adapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, items);
            lvOptions.Adapter = adapter;

            lvOptions.ItemClick += (sender, e) => {

                switch (e.Position) {

                    case 0: EditEnterpriseResource(main, position).Show();
                        builder.Dismiss();
                        break;
                    case 1: main.ModifyEnterpriseResources(2, position, "");
                        builder.Dismiss();
                        break;
                    case 2: builder.Dismiss();
                        break;

                }
            };

            builder.SetTitle("Options");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);

            return builder;
        }

        public Android.Support.V7.App.AlertDialog AddResourceToProject(DetailsActivity details) {

            View view = LayoutInflater.From(details).Inflate(Resource.Layout.empty_recycleview, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(details).Create();

            RecyclerView mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(view.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            Resourcez mResources = new Resourcez();
            var projectResources = JsonConvert.DeserializeObject<ProjectResources.RootObject>(details.projectResources);
            foreach (var item in projectResources.D.Results)
            {
                for (int i = 0; i < details.mEnterprise.D.Results.Count; i++) {
                    if (details.mEnterprise.D.Results[i].Name.Equals(item.Name)) {
                        details.mEnterprise.D.Results.RemoveAt(i);
                    }
                }
            }
            foreach (var item in details.mEnterprise.D.Results) {
                mResources.addResources(item.Name);
            }
            DialogProjectResourceAdapter mProjectResourceAdapter = new DialogProjectResourceAdapter(mResources);
            mRecyclerView.SetAdapter(mProjectResourceAdapter);

            builder.SetButton(-2, "Add", delegate {
                ThreadPool.QueueUserWorkItem(async state =>
               {
                   details.RunOnUiThread(() =>
                   {
                       Toast.MakeText(details, "Adding the resource...", ToastLength.Short).Show();
                   });
                   bool checkOutSuccess = await details.core.CheckOut("", details.id);
                   if (checkOutSuccess)
                   {
                       for (int i = 0; i < mResources.temp1.Count; i++)
                       {
                           for (int j = 0; j < details.mEnterprise.D.Results.Count; j++)
                           {
                               if (mResources.temp1[i].Equals(details.mEnterprise.D.Results[j].Name))
                               {

                                   bool isSuccess = await details.core.AddProjectResource("", details.id, details.mEnterprise.D.Results[j].Id);
                                   if (isSuccess)
                                   {
                                       details.RunOnUiThread(() =>
                                       {
                                           Toast.MakeText(details, "Successfully added " + details.mEnterprise.D.Results[j].Name, ToastLength.Short).Show();
                                       });
                                   }
                                   else
                                       details.RunOnUiThread(() =>
                                       {
                                           Toast.MakeText(details, "There was a problem adding " + details.mEnterprise.D.Results[j].Name, ToastLength.Short).Show();
                                       });
                                   break;
                               }
                           }
                       }
                       bool publishSuccess = await details.core.Publish("", details.id);
                       if (publishSuccess)
                       {
                           //details.RunOnUiThread(() =>
                           //{
                           //    Toast.MakeText(details, "Successfully added the resource", ToastLength.Short).Show();
                           //});
                           Thread.Sleep(3000);
                       }
                       else
                       {
                           //details.RunOnUiThread(() =>
                           //{
                           //    Toast.MakeText(details, "There was a problem publishing the project", ToastLength.Short).Show();
                           //});
                       }
                   }
                   else {
                       details.RunOnUiThread(() => {
                           Toast.MakeText(details, "There was a problem checking out the project", ToastLength.Short).Show();
                       });
                   }



                   builder.Dismiss();
               });

            });
            builder.SetButton(-1, "Close", delegate { builder.Dismiss(); });
            builder.SetTitle("Add Resources");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);
            return builder;
        }

        public Android.Support.V7.App.AlertDialog OpenSavedTimesheetSettings(MainActivity main, int position) {
            View view = LayoutInflater.From(main).Inflate(Resource.Layout.empty_listview, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(main).Create();

            ListView lvOptions = view.FindViewById<ListView>(Resource.Id.lvTimesheetSettings);
            List<string> items = new List<string> { "Delete saved timesheet", "Push changes" };
            var adapter = new ArrayAdapter(main, AndroidApp1.Resource.Layout.select_dialog_item_material, items);
            lvOptions.Adapter = adapter;
            lvOptions.ItemClick += async (sender, e) =>
            {
                switch (e.Position)
                {
                    case 0:
                        await main.service.DeleteSavedTimesheet(main, position);
                        builder.Dismiss();
                        break;
                    case 1:
                        
                        int batchCount = 0;
                        if (!main.prefs.GetString("tobepushed", "").Equals("")) {
                            Toast.MakeText(main, "Pushing changes...", ToastLength.Short).Show();
                            var list = JsonConvert.DeserializeObject<List<SavedChangesModel>>(main.prefs.GetString("tobepushed", ""));
                            foreach (var item in list) {
                                var body = "{'parameters':{'ActualWork':'" + item.actualHours + "', 'PlannedWork':'" + item.plannedHours + "', 'Start':'" + item.startDate + "', 'NonBillableOvertimeWork':'0h', 'NonBillableWork':'0h', 'OvertimeWork':'0h'}}";
                                
                                bool success = await main.core.AddTimesheetLineWork(body, item.periodId, item.lineId);
                                if (success)
                                    batchCount++;

                            }
                            if (batchCount == list.Count)
                            {
                                Toast.MakeText(main, "Successfully pushed the changes!", ToastLength.Short).Show();
                                main.prefs.Edit().Remove("tobepushed").Apply();
                            }
                            else {
                                Toast.MakeText(main, "There were some changes that were not pushed", ToastLength.Short).Show();
                                main.prefs.Edit().Remove("tobepushed").Apply();
                            }
                        }
                        else
                            Toast.MakeText(main, "You have no saved changes", ToastLength.Short).Show();
                        
                        break;

                }
            };

            builder.SetTitle("Options");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);
            builder.SetButton(-1, "CLOSE", delegate { builder.Dismiss(); });

            return builder;
        }

        public Android.Support.V7.App.AlertDialog DeleteProjectResource(DetailsActivity details)
        {

            View view = LayoutInflater.From(details).Inflate(Resource.Layout.empty_recycleview, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(details).Create();

            RecyclerView mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(view.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            Resourcez mResources = new Resourcez();
            var projectResources = JsonConvert.DeserializeObject<ProjectResources.RootObject>(details.projectResources);
            foreach (var item in projectResources.D.Results)
            {
                mResources.addResources(item.Name);
            }

            DialogProjectResourceAdapter mProjectResourceAdapter = new DialogProjectResourceAdapter(mResources);
            mRecyclerView.SetAdapter(mProjectResourceAdapter);

            builder.SetButton(-2, "Remove", delegate {
                ThreadPool.QueueUserWorkItem(async state =>
                {
                    details.RunOnUiThread(() =>
                    {
                        Toast.MakeText(details, "Removing the resource...", ToastLength.Short).Show();
                    });
                    bool checkOutSuccess = await details.core.CheckOut("", details.id);
                    if (checkOutSuccess)
                    {
                        for (int i = 0; i < mResources.temp1.Count; i++)
                        {
                            for (int j = 0; j < details.mEnterprise.D.Results.Count; j++)
                            {
                                if (mResources.temp1[i].Equals(details.mEnterprise.D.Results[j].Name))
                                {

                                    bool isSuccess = await details.core.DeleteProjectResource("", details.id, details.mEnterprise.D.Results[j].Id);
                                    if (isSuccess)
                                    {
                                        details.RunOnUiThread(() =>
                                        {
                                            Toast.MakeText(details, "Successfully removed " + details.mEnterprise.D.Results[j].Name, ToastLength.Short).Show();
                                        });
                                    }
                                    else
                                        details.RunOnUiThread(() =>
                                        {
                                            Toast.MakeText(details, "There was a problem removing " + details.mEnterprise.D.Results[j].Name, ToastLength.Short).Show();
                                        });
                                    break;
                                }
                            }
                        }
                        bool publishSuccess = await details.core.Publish("", details.id);
                        if (publishSuccess)
                        {
                            //details.RunOnUiThread(() =>
                            //{
                            //    Toast.MakeText(details, "Successfully removed the resource", ToastLength.Short).Show();
                            //});
                            Thread.Sleep(3000);
                        }
                        else
                        {
                            //details.RunOnUiThread(() =>
                            //{
                            //    Toast.MakeText(details, "There was a problem publishing the project", ToastLength.Short).Show();
                            //});
                        }
                    }
                    else
                    {
                        details.RunOnUiThread(() =>
                        {
                            Toast.MakeText(details, "There was a problem checking out the project", ToastLength.Short).Show();
                        });
                    }



                    builder.Dismiss();
                });

            });
            builder.SetButton(-1, "Close", delegate { builder.Dismiss(); });
            builder.SetTitle("Remove Resources");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);
            return builder;
        }

        public Android.Support.V7.App.AlertDialog AddTaskAssignments(DetailsActivity details) {

            View view = LayoutInflater.From(details).Inflate(Resource.Layout.two_spinner_layout, null);
            Android.Support.V7.App.AlertDialog builder = new Android.Support.V7.App.AlertDialog.Builder(details).Create();

            Spinner resources = view.FindViewById<Spinner>(Resource.Id.spnrResources);
            Spinner tasks = view.FindViewById<Spinner>(Resource.Id.spnrTasks);
            EditText notes = view.FindViewById<EditText>(Resource.Id.etNotes);

            List<string> resourcesNameList = new List<string> { };
            var temp1 = JsonConvert.DeserializeObject<ProjectResources.RootObject>(details.projectResources);
            foreach (var item in temp1.D.Results) {
                resourcesNameList.Add(item.Name);
            }
            var resourcesAdapter = new ArrayAdapter(details, AndroidApp1.Resource.Layout.select_dialog_item_material, resourcesNameList);
            resources.Adapter = resourcesAdapter;

            List<string> tasksNameList = new List<string> { };
            var temp2 = JsonConvert.DeserializeObject<Taskmodel.RootObject>(details.projectTasksJson);
            foreach (var item in temp2.D.Results)
            {
                tasksNameList.Add(item.Name);
            }
            var tasksAdapter = new ArrayAdapter(details, AndroidApp1.Resource.Layout.select_dialog_item_material, tasksNameList);
            tasks.Adapter = tasksAdapter;


            builder.SetButton(-2, "Add", delegate {
                Toast.MakeText(details, "Adding assignment...", ToastLength.Short).Show();
                string resourceId = "", taskId = "";

                foreach (var item in details.mEnterprise.D.Results) {
                    if (item.Name.Equals(resources.SelectedItem.ToString())) {
                        resourceId = item.Id;
                        break;
                    }
                }

                foreach (var item in temp2.D.Results) {
                    if (item.Name.Equals(tasks.SelectedItem.ToString())) {
                        taskId = item.Id;
                        break;
                    }
                }

                var body = "{'parameters':{'Notes':'"+notes.Text+"','ResourceId':'"+resourceId+"', 'TaskId':'"+taskId+"' } }";

                ThreadPool.QueueUserWorkItem(async state => {
                    bool checkoutSuccess = await details.core.CheckOut("", details.id);
                    if (checkoutSuccess) {
                        bool success = await details.core.AddAssignmentOnTask(body, details.id);
                        if (success)
                        {
                            bool publishSuccess = await details.core.Publish("", details.id);
                            if (publishSuccess)
                            {
                                details.RunOnUiThread(() => { Toast.MakeText(details, "Successfully added the assignment", ToastLength.Short).Show(); });
                            }
                            else
                                details.RunOnUiThread(() => { Toast.MakeText(details, "There was a problem adding the assignment", ToastLength.Short).Show(); });
                        }
                        else {
                            details.RunOnUiThread(() => { Toast.MakeText(details, "There was a problem adding the assignment", ToastLength.Short).Show(); });
                        }
                    }
                    else {
                        details.RunOnUiThread(() => { Toast.MakeText(details, "There was a problem adding the assignment", ToastLength.Short).Show(); });
                    }
                });

            });
            builder.SetButton(-1, "Close", delegate { builder.Dismiss(); });
            builder.SetTitle("Add task assignments");
            builder.SetCanceledOnTouchOutside(false);
            builder.SetView(view);
            return builder;

        }

    }
}