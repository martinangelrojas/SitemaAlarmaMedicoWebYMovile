using SistemaAlarmaMovil.Domain;
using SistemaAlarmaMovil.Helpers.Interfaces;
using SistemaAlarmaMovil.Repositories.Interfaces;
using SistemaAlarmaMovil.Services;
using SistemaAlarmaMovil.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SistemaAlarmaMovil.ViewModels
{
    public class OrdenesMedicasViewModel : BaseViewModel
    {
        public ObservableCollection<OrdenMedicaDto> Ordenes { get; } = new();
        private readonly IOrdenMedicaServiceWeb _ordenMedicaServiceWeb;
        private readonly ISessionHelper _sessionHelper;
        private readonly ILineaOrdenMedicaRepository _lineaOrdenMedicaRepository;
        public DatabaseService _dataBase = new DatabaseService();
        public ICommand RecibirOrdenCommand { get; }

        public OrdenesMedicasViewModel(IOrdenMedicaServiceWeb ordenMedicaServiceWeb, ISessionHelper sessionHelper, INotificacionesHelper notificacionesHelper, ILineaOrdenMedicaRepository lineaOrdenMedicaRepository)
        {
            _sessionHelper = sessionHelper;
            _ordenMedicaServiceWeb = ordenMedicaServiceWeb;
            _lineaOrdenMedicaRepository = lineaOrdenMedicaRepository;

            RecibirOrdenCommand = new Command<OrdenMedicaDto>(async (orden) => await RecibirOrden(orden));
            Task.Run(CargarOrdenes);
        }

        private async Task RecibirOrden(OrdenMedicaDto orden)
        {
            try
            {
                var ordenMedicaApi = await _ordenMedicaServiceWeb.ObtenerPorId(orden.OrdenMedicaId.Value);

                await _ordenMedicaServiceWeb.TomarOrdenMedica(orden.OrdenMedicaId.Value);
                
                var page = Application.Current.Handler.MauiContext.Services.GetService<GestionOrdenMedicaPage>();
                page.SetOrdenId(orden.OrdenMedicaId.Value);
                await Application.Current.MainPage.Navigation.PushAsync(page);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo recibir la orden médica. Error: {ex.Message}", "OK");
            }
        }

        private async Task CargarOrdenes()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var paciente = await _sessionHelper.ObtenerSessionUsuarioYRegistroBD();
                var ordenes = await _ordenMedicaServiceWeb.ObtenerPorDni(paciente.Documento);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Ordenes.Clear();
                    foreach (var orden in ordenes)
                    {
                        Ordenes.Add(orden);
                    }
                });
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudieron cargar las órdenes médicas", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
