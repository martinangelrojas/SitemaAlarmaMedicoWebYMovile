using Microsoft.Maui.Controls;
using SistemaAlarmaMovil.Helpers;

namespace SistemaAlarmaMovil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await RequestPermissions();
        }

        private async Task RequestPermissions()
        {
            try
            {
                // Solicitar permisos de notificaciones
                await PermissionHelper.RequestNotificationPermissions();

                // Solicitar optimización de batería
                await PermissionHelper.RequestBatteryOptimization();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al solicitar permisos: {ex.Message}");
            }
        }
    }
}
