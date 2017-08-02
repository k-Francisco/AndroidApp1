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

namespace AndroidApp1.Models.SavedChanges
{
    public class SavedChangesModel
    {
        public DateTime startDate { get; set; }
        public string actualHours { get; set; }
        public string plannedHours { get; set; }
        public string lineId { get; set; }
        public string periodId { get; set; }
    }
}