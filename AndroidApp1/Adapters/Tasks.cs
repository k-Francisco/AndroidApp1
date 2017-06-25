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
using AndroidApp1.Helpers;

namespace AndroidApp1.Adapters
{
    public class TasksModel {
        public string mTaskProjectName { get; set; }
        public string mTasksPercentComplete { get; set; }
        public string mtasksWork { get; set; }
        public string mTasksDuration { get; set; }
        public string mTaskName { get; set; }
    }

    public class Tasks {

        List<TasksModel> taskList = new List<TasksModel> { };

        public Tasks() { }

        public void addTasks(string taskName, string taskPercentComplete, string taskWork, string taskDuration, string taskProjectName)
        {
            taskList.Add(new TasksModel() { mTaskName = taskName, mTasksPercentComplete = taskPercentComplete, mtasksWork = taskWork, mTasksDuration = taskDuration, mTaskProjectName = taskProjectName });
        }

        public int numHome
        {
            get { return taskList.Count; }
        }

        public TasksModel this[int i]
        {
            get { return taskList[i]; }
        }

    }

    public class TasksViewHolder : RecyclerView.ViewHolder
    {

        public TextView taskName { get; set; }
        public TextView taskPercentComplete { get; set; }
        public TextView taskWork { get; set; }
        public TextView taskDuration { get; set; }
        public TextView taskProjectName { get; set; }
        public Button editTask { get; set; }
        public Button deleteTask { get; set; }

        public TasksViewHolder(View itemView, Action<int> listener) : base(itemView)
        {

            taskName = itemView.FindViewById<TextView>(Resource.Id.tvTaskName);
            taskPercentComplete = itemView.FindViewById<TextView>(Resource.Id.tvTaskPercentComplete);
            taskWork = itemView.FindViewById<TextView>(Resource.Id.tvTaskWork);
            taskDuration = itemView.FindViewById<TextView>(Resource.Id.tvTaskDuration);
            taskProjectName = itemView.FindViewById<TextView>(Resource.Id.tvTaskProjectName);
            editTask = itemView.FindViewById<Button>(Resource.Id.btnTaskEdit);
            deleteTask = itemView.FindViewById<Button>(Resource.Id.btnTaskDelete);
        }

    }

    public class TasksAdapter : RecyclerView.Adapter {

        public EventHandler<int> itemClick;
        public Tasks mTasks;
        public MainActivity main;

        public TasksAdapter(Tasks tasks, MainActivity main)
        {
            this.main = main;
            mTasks = tasks;
        }

        public override int ItemCount
        {
            get { return mTasks.numHome; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DialogHelpers dialogs = new DialogHelpers();
            TasksViewHolder vh = holder as TasksViewHolder;
            vh.taskName.Text = mTasks[position].mTaskName;
            vh.taskProjectName.Text = mTasks[position].mTaskProjectName;
            vh.taskPercentComplete.Text = mTasks[position].mTasksPercentComplete;
            vh.taskWork.Text = mTasks[position].mtasksWork;
            vh.taskDuration.Text = mTasks[position].mTasksDuration;
            vh.deleteTask.Click += delegate { dialogs.DeleteTaskDialog(main, mTasks[position].mTaskProjectName, mTasks[position].mTaskName).Show(); };
            vh.editTask.Click += delegate { };
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.task_card, parent, false);
            TasksViewHolder vh = new TasksViewHolder(itemView, Onclick);
            return vh;
        }

        private void Onclick(int obj)
        {
            if (itemClick != null)
            {
                itemClick(this, obj);
            }
        }
    }


}