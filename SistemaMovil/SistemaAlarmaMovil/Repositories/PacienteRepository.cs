using SistemaAlarmaMovil.Models;
using SistemaAlarmaMovil.Repositories.Interfaces;
using SQLite;

namespace SistemaAlarmaMovil.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private SQLiteAsyncConnection _database;

        public PacienteRepository(SQLiteAsyncConnection database)
        {
            _database = database;
            _database.CreateTableAsync<Paciente>().Wait();
        }

        public async Task<List<Paciente>> GetAllAsync()
        {
            return await _database.Table<Paciente>().ToListAsync();
        }

        public async Task<Paciente> GetByIdAsync(int id)
        {
            return await _database.Table<Paciente>()
                .Where(p => p.PacienteId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveAsync(Paciente paciente)
        {
            if (paciente.PacienteId != 0)
            {
                return await _database.UpdateAsync(paciente);
            }
            else
            {
                return await _database.InsertAsync(paciente);
            }
        }

        public async Task<int> DeleteAsync(Paciente paciente)
        {
            return await _database.DeleteAsync(paciente);
        }
    }
}
