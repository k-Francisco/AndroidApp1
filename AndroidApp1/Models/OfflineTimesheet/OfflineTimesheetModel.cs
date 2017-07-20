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
using Newtonsoft.Json;

namespace AndroidApp1.Models.OfflineTimesheet
{
    public class OfflineTimesheetModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        public string owner { get; set; }

        public string period { get; set; }

        public string offlineTimesheetLines { get; set; }

        public string offlineTimesheetWork { get; set; }
    }
}