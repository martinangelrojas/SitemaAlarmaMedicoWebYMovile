using SistemaAlarmaMovil.Models;
using SistemaAlarmaMovil.Repositories.Interfaces;
using SQLite;

namespace SistemaAlarmaMovil.Repositories
{
    public class LineaOrdenMedicaRepository : ILineaOrdenMedicaRepository
    {
        private SQLiteAsyncConnection _database;

        public LineaOrdenMedicaRepository(SQLiteAsyncConnection database)
        {
            _database = database;
            _database.CreateTableAsync<LineaOrdenMedica>().Wait();
        }

        public async Task<List<LineaOrdenMedica>> GetAllAsync()
        {
            return await _database.Table<LineaOrdenMedica>().ToListAsync();
        }

        public async Task<LineaOrdenMedica> GetByIdAsync(int id)
        {
            var result = await _database.Table<LineaOrdenMedica>()
                .FirstOrDefaultAsync(x => x.LineaOrdenMedicaId == id);
                
            return result;
        }

        public async Task<int> SaveAsync(LineaOrdenMedica lineaOrdenMedica)
        {
            if (lineaOrdenMedica.LineaId != 0)
            {
                return await _database.UpdateAsync(lineaOrdenMedica);
            }
            else
            {
                return await _database.InsertAsync(lineaOrdenMedica);
            }
        }
    }
}
