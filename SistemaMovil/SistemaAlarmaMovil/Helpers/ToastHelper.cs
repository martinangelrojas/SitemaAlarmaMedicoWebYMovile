using Android.Widget;
using SistemaAlarmaMovil.Helpers.Interfaces;

namespace SistemaAlarmaMovil.Helpers
{
    public class ToastHelper : IToastHelper
    {
        public void ShowToast(string message, int duration = 500)
        {
            if (Platform.CurrentActivity != null)
            {
                Platform.CurrentActivity.RunOnUiThread(() =>
                {
                    Toast.MakeText(Platform.CurrentActivity, message, ToastLength.Long).Show();
                });
            }
        }
    }
} 