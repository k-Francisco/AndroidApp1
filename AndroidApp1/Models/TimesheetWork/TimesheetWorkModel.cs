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

namespace TimesheetWork
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
        [JsonProperty("ActualWork")]
        public string ActualWork { get; set; }
        [JsonProperty("ActualWorkMilliseconds")]
        public int ActualWorkMilliseconds { get; set; }
        [JsonProperty("ActualWorkTimeSpan")]
        public string ActualWorkTimeSpan { get; set; }
        [JsonProperty("Comment")]
        public object Comment { get; set; }
        [JsonProperty("End")]
        public DateTime End { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("NonBillableOvertimeWork")]
        public string NonBillableOvertimeWork { get; set; }
        [JsonProperty("NonBillableOvertimeWorkMilliseconds")]
        public int NonBillableOvertimeWorkMilliseconds { get; set; }
        [JsonProperty("NonBillableOvertimeWorkTimeSpan")]
        public string NonBillableOvertimeWorkTimeSpan { get; set; }
        [JsonProperty("NonBillableWork")]
        public string NonBillableWork { get; set; }
        [JsonProperty("NonBillableWorkMilliseconds")]
        public int NonBillableWorkMilliseconds { get; set; }
        [JsonProperty("NonBillableWorkTimeSpan")]
        public string NonBillableWorkTimeSpan { get; set; }
        [JsonProperty("OvertimeWork")]
        public string OvertimeWork { get; set; }
        [JsonProperty("OvertimeWorkMilliseconds")]
        public int OvertimeWorkMilliseconds { get; set; }
        [JsonProperty("OvertimeWorkTimeSpan")]
        public string OvertimeWorkTimeSpan { get; set; }
        [JsonProperty("PlannedWork")]
        public string PlannedWork { get; set; }
        [JsonProperty("PlannedWorkMilliseconds")]
        public int PlannedWorkMilliseconds { get; set; }
        [JsonProperty("PlannedWorkTimeSpan")]
        public string PlannedWorkTimeSpan { get; set; }
        [JsonProperty("Start")]
        public DateTime Start { get; set; }
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
}