using SistemaAlarmaMovil.Helpers.Interfaces;
using SistemaAlarmaMovil.ViewModels;
using SistemaAlarmaMovil.Views;

namespace SistemaAlarmaMovil
{
    public partial class MainPage : ContentPage
    {
        private readonly ISessionHelper _sessionHelper;

        public MainPage(ISessionHelper sessionHelper)
        {
            InitializeComponent();
            _sessionHelper = sessionHelper;
            CargarDatosDeUsuario();
        }

        private async void CargarDatosDeUsuario()
        {
            var paciente = await _sessionHelper.ObtenerSessionUsuarioYRegistroBD();

            if (paciente != null)
            {
                btnOrdenesMedicas.IsEnabled = true;
            }
        }

        private async void OnActualizarPacienteClicked(object sender, EventArgs e)
        {
            var mainPage = Application.Current.Handler.MauiContext.Services.GetService<ActualizarPacientePage>();
            await Navigation.PushAsync(mainPage);
        }

        private async void OnOrdenesMedicasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrdenesMedicasPage(
                Handler.MauiContext.Services.GetService<OrdenesMedicasViewModel>()));
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await _sessionHelper?.LogoutAsync();
            
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}
