using SistemaAlarmaMovil.Models;

namespace SistemaAlarmaMovil.Repositories.Interfaces
{
    public interface ILineaOrdenMedicaRepository
    {
        Task<List<LineaOrdenMedica>> GetAllAsync();
        Task<LineaOrdenMedica> GetByIdAsync(int id);
        Task<int> SaveAsync(LineaOrdenMedica lineaOrdenMedica);
    }
}
