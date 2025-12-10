using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Microsoft.Maui.ApplicationModel;

namespace SistemaAlarmaMovil.Helpers
{
    public class PermissionHelper
    {
        public static async Task<bool> RequestNotificationPermissions()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.PostNotifications>();
                    if (status != PermissionStatus.Granted)
                        return false;
                }
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                var alarmManager = Platform.CurrentActivity.GetSystemService(Context.AlarmService) as AlarmManager;
                if (alarmManager != null && !alarmManager.CanScheduleExactAlarms())
                {
                    var intent = new Intent(Android.Provider.Settings.ActionRequestScheduleExactAlarm);
                    Platform.CurrentActivity.StartActivity(intent);
                    return false;
                }
            }

            return true;
        }

        public static async Task<bool> RequestBatteryOptimization()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var powerManager = Platform.CurrentActivity.GetSystemService(Context.PowerService) as PowerManager;
                if (powerManager != null && !powerManager.IsIgnoringBatteryOptimizations(Platform.CurrentActivity.PackageName))
                {
                    var intent = new Intent(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
                    intent.SetData(Android.Net.Uri.Parse("package:" + Platform.CurrentActivity.PackageName));
                    Platform.CurrentActivity.StartActivity(intent);
                    return false;
                }
            }
            return true;
        }

        public static bool CheckNotificationPermissions()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                var status = Permissions.CheckStatusAsync<Permissions.PostNotifications>().Result;
                if (status != PermissionStatus.Granted)
                    return false;
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                var alarmManager = Platform.CurrentActivity.GetSystemService(Context.AlarmService) as AlarmManager;
                if (alarmManager != null && !alarmManager.CanScheduleExactAlarms())
                    return false;
            }

            return true;
        }
    }
} 