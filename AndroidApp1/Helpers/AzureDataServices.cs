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
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using AndroidApp1.Models.OfflineTimesheet;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Android.Util;
using System.Threading;

namespace AndroidApp1.Helpers
{
    public class AzureDataServices
    {
        public MobileServiceClient MobileService { get; set; }
        IMobileServiceSyncTable<OfflineTimesheetModel> offlineTimesheet;
        MobileServiceSQLiteStore storez;

        public async Task Initialize()
        {
            if (MobileService?.SyncContext?.IsInitialized ?? false)
                return;

            MobileService = new MobileServiceClient("http://projectservermobile.azurewebsites.net");

            const string path = "offlinetimesheetstore.db";

            storez = new MobileServiceSQLiteStore(path);
            storez.DefineTable<OfflineTimesheetModel>();
            await MobileService.SyncContext.InitializeAsync(storez);

            offlineTimesheet = MobileService.GetSyncTable<OfflineTimesheetModel>();
        }

        public async Task<IEnumerable<OfflineTimesheetModel>> pullData(bool online)
        {
            await Initialize();
            await SyncData(online);

            return await offlineTimesheet.OrderBy(c => c.Id).ToEnumerableAsync();
        }

        public async Task SyncData(bool online)
        {
            try {
                if (!online)
                    return;

                await MobileService.SyncContext.PushAsync();
                await offlineTimesheet.PullAsync("allOfflineTimesheet", offlineTimesheet.CreateQuery());
            }
            catch (Exception e) {
                Log.Info("kfsama", "no internet");
            }
            
        }

        public async Task UpdateData(string id,string period, string periodId, string timesheetLines, string timesheetWork)
        {

            var offline = new OfflineTimesheetModel { period = period, offlineTimesheetLines = timesheetLines, offlineTimesheetWork = timesheetWork };
            await offlineTimesheet.UpdateAsync(offline);
            await SyncData(true);
        }

        public async Task AddData(string name,string period, string timesheetLines, string timesheetWork)
        {

            var offline = new OfflineTimesheetModel { owner = name,period = period, offlineTimesheetLines = timesheetLines, offlineTimesheetWork = timesheetWork };
            await offlineTimesheet.InsertAsync(offline);
            await SyncData(true);
        }

        public async Task PurgeData() {
            await offlineTimesheet.PurgeAsync("allOfflineTimesheet", null, true, CancellationToken.None);
        }

        public void ClearDatabase() {
            MobileService.Dispose();
        }

    }
}