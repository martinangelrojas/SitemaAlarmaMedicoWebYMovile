using SistemaAlarmaMovil.Common;
using SistemaAlarmaMovil.Domain;

namespace SistemaAlarmaMovil.Services
{
    public interface IOrdenMedicaServiceWeb
    {
        Task<OrdenMedicaDto> ObtenerPorId(int id);
        Task<List<OrdenMedicaDto>> ObtenerTodos();
        Task<ServiceResponse> TomarOrdenMedica(int ordenMedicaId);
        Task<List<OrdenMedicaDto>> ObtenerPorDni(int dni);

        Task<ServiceResponse> EmpezarTratamiento(int lineaOrdenMedicaId);
    }
} 