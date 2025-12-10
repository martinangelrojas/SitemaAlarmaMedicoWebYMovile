using SistemaAlarmaMovil.Domain;

namespace SistemaAlarmaMovil.Helpers.Interfaces
{
    public interface INotificacionesHelper
    {
        Task ProgramarNotificacionesLineaOrdenMedica(LineaOrdenMedicaDto lineaOrdenMedica);
        Task EliminarNotificacionesOrdenMedica(int ordenMedicaId);
        Task ProgramarNotificacionPrueba(int segundos);
    }
}
