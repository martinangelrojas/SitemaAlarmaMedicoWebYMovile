using SistemaAlarmaMovil.Domain;
using SistemaAlarmaMovil.Helpers.Interfaces;
using SistemaAlarmaMovil.Repositories.Interfaces;
using SistemaAlarmaMovil.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SistemaAlarmaMovil.ViewModels
{
    public class GestionOrdenMedicaViewModel : BaseViewModel
    {
        private OrdenMedicaDto _orden;
        public OrdenMedicaDto Orden
        {
            get => _orden;
            set
            {
                if (SetProperty(ref _orden, value))
                {
                    LineasOrdenMedica.Clear();
                    if (value?.LineaOrdenMedica != null)
                    {
                        foreach (var linea in value.LineaOrdenMedica)
                            LineasOrdenMedica.Add(linea);
                    }
                }
            }
        }

        public ObservableCollection<LineaOrdenMedicaDto> LineasOrdenMedica { get; set; } = new();

        private readonly IOrdenMedicaServiceWeb _ordenMedicaServiceWeb;
        private readonly INotificacionesHelper _notificacionesHelper;
        private readonly ILineaOrdenMedicaRepository _lineaOrdenMedicaRepository;

        public ICommand EmpezarTratamientoCommand { get; }

        public GestionOrdenMedicaViewModel(
            IOrdenMedicaServiceWeb ordenMedicaServiceWeb, 
            INotificacionesHelper notificacionesHelper, 
            ILineaOrdenMedicaRepository lineaOrdenMedicaRepository,
            DatabaseService databaseService)
        {
            _ordenMedicaServiceWeb = ordenMedicaServiceWeb;
            _notificacionesHelper = notificacionesHelper;
            _lineaOrdenMedicaRepository = lineaOrdenMedicaRepository;

            EmpezarTratamientoCommand = new Command<LineaOrdenMedicaDto>(async (linea) => await EmpezarTratamiento(linea));
        }

        public async Task InicializarAsync(int ordenId)
        {
            await CargarOrden(ordenId);
        }

        private async Task CargarOrden(int id)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                
                Orden = await _ordenMedicaServiceWeb.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", 
                    $"No se pudo cargar la orden médica: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task EmpezarTratamiento(LineaOrdenMedicaDto lineaOrdenMedica)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await _ordenMedicaServiceWeb.EmpezarTratamiento(lineaOrdenMedica.LineaOrdenMedicaId.Value);
                await _notificacionesHelper.ProgramarNotificacionesLineaOrdenMedica(lineaOrdenMedica);
                
                if (Orden?.OrdenMedicaId.HasValue == true)
                {
                    LineasOrdenMedica.Clear();
                    var ordenActualizada = await _ordenMedicaServiceWeb.ObtenerPorId(Orden.OrdenMedicaId.Value);
                    Orden = ordenActualizada;
                    OnPropertyChanged(nameof(LineasOrdenMedica));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo iniciar el tratamiento", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
