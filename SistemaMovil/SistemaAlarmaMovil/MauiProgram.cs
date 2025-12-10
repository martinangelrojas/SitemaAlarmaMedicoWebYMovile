using Microsoft.Extensions.Logging;
using SistemaAlarmaMovil.Common;
using SistemaAlarmaMovil.GoogleServices;
using SistemaAlarmaMovil.Services;
using SistemaAlarmaMovil.ViewModels;
using SistemaAlarmaMovil.Views;
using SistemaAlarmaMovil.Repositories;
using SQLite;
using SistemaAlarmaMovil.Helpers.Interfaces;
using SistemaAlarmaMovil.Helpers;
using SistemaAlarmaMovil.Services.Interfaces;
using SistemaAlarmaMovil.Repositories.Interfaces;

namespace SistemaAlarmaMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<HttpClient>((serviceProvider) =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                var client = new HttpClient(handler)
                {
                    BaseAddress = new Uri("http://10.0.2.2:7131/api/"),
                    Timeout = TimeSpan.FromSeconds(30)
                };

                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                return client;
            });
            builder.Services.AddSingleton<INotificacionesHelper, NotificacionesHelper>();
            builder.Services.AddSingleton<ISessionHelper, SessionHelper>();

            builder.Services.AddSingleton<HttpClientService>();
            builder.Services.AddSingleton<IGoogleAuthService, GoogleAuthService>();

            builder.Services.AddSingleton<IOrdenMedicaServiceWeb, OrdenMedicaServiceWeb>();
            builder.Services.AddSingleton<IPacienteServiceWeb, PacienteServiceWeb>();

            builder.Services.AddTransient<OrdenesMedicasViewModel>();
            builder.Services.AddTransient<ActualizarPacienteViewModel>();
            builder.Services.AddTransient<GestionOrdenMedicaViewModel>();

            builder.Services.AddTransient<OrdenesMedicasPage>();
            builder.Services.AddTransient<ActualizarPacientePage>();
            builder.Services.AddTransient<GestionOrdenMedicaPage>();

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<MainPage>();

            // Registrar servicios de base de datos
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<SQLiteAsyncConnection>(sp => 
                sp.GetRequiredService<DatabaseService>().GetConnection());

            builder.Services.AddSingleton<IPacienteRepository, PacienteRepository>();
            builder.Services.AddSingleton<ILineaOrdenMedicaRepository, LineaOrdenMedicaRepository>();

            builder.Services.AddSingleton<IToastHelper, ToastHelper>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
