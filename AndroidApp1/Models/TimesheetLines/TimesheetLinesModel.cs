﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace TimesheetLines
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
    public class Result
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("Assignment")]
        public Assignment Assignment { get; set; }
        [JsonProperty("TimeSheet")]
        public TimeSheet TimeSheet { get; set; }
        [JsonProperty("Work")]
        public Work Work { get; set; }
        [JsonProperty("Comment")]
        public string Comment { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("LineClass")]
        public int LineClass { get; set; }
        [JsonProperty("ProjectName")]
        public string ProjectName { get; set; }
        [JsonProperty("Status")]
        public int Status { get; set; }
        [JsonProperty("TaskName")]
        public string TaskName { get; set; }
        [JsonProperty("TotalWork")]
        public string TotalWork { get; set; }
        [JsonProperty("TotalWorkMilliseconds")]
        public int TotalWorkMilliseconds { get; set; }
        [JsonProperty("TotalWorkTimeSpan")]
        public string TotalWorkTimeSpan { get; set; }
        [JsonProperty("ValidationType")]
        public int ValidationType { get; set; }
    }
    public class Metadata
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class Assignment
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class TimeSheet
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Work
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
}