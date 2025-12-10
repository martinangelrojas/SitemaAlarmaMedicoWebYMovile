using SistemaAlarmaMovil.Models;

namespace SistemaAlarmaMovil.Repositories.Interfaces
{
    public interface IPacienteRepository
    {
        Task<List<Paciente>> GetAllAsync();
        Task<Paciente> GetByIdAsync(int id);
        Task<int> SaveAsync(Paciente paciente);
        Task<int> DeleteAsync(Paciente paciente);
    }
}