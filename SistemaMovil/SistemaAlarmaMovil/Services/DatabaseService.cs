using SQLite;
using SistemaAlarmaMovil.Models;

namespace SistemaAlarmaMovil.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "SistemaAlarmas.db");
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return _database;
        }

        public async Task InitializeDatabaseAsync()
        {
            await _database.CreateTableAsync<Paciente>();
            await _database.CreateTableAsync<LineaOrdenMedica>();
        }

        public async Task VerificarTablaLineaOrdenMedica()
        {
            try
            {
                // Verificar si la tabla existe
                var tableInfo = await _database.GetTableInfoAsync("LineaOrdenMedica");
                string mensaje = $"Tabla LineaOrdenMedica existe: {tableInfo.Any()}\n";
                
                if (!tableInfo.Any())
                {
                    // Si la tabla no existe, la creamos
                    await _database.CreateTableAsync<LineaOrdenMedica>();
                    mensaje += "La tabla ha sido creada.\n";
                    
                    // Obtenemos la información de la tabla recién creada
                    tableInfo = await _database.GetTableInfoAsync("LineaOrdenMedica");
                }
                
                if (tableInfo.Any())
                {
                    mensaje += "Columnas en la tabla:\n";
                    foreach (var columna in tableInfo)
                    {
                        mensaje += $"- {columna.Name} ({columna.GetType})\n";
                    }
                }

                // Contar registros
                var count = await _database.Table<LineaOrdenMedica>().CountAsync();
                mensaje += $"\nNúmero de registros: {count}";

                await Application.Current.MainPage.DisplayAlert("Información de la Base de Datos", mensaje, "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error al verificar tabla", 
                    $"Error: {ex.Message}\nStackTrace: {ex.StackTrace}", "OK");
            }
        }

        public async Task VerificarEstadoBaseDeDatos()
        {
            try
            {
                var tableInfo = await _database.GetTableInfoAsync("LineaOrdenMedica");
                await Application.Current.MainPage.DisplayAlert("Info DB", 
                    $"Tabla LineaOrdenMedica existe: {tableInfo.Any()}\n" +
                    $"Número de columnas: {tableInfo.Count}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error DB", 
                    $"Error al verificar base de datos: {ex.Message}", "OK");
            }
        }

        public async Task BorrarTablasAsync()
        {
            await _database.DropTableAsync<Paciente>();
            await _database.DropTableAsync<LineaOrdenMedica>();
        }

        public async Task EliminarBaseDeDatos()
        {
            try
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "SistemaAlarmas.db");

                await _database.CloseAsync();
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Base de datos eliminada correctamente", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Info", "La base de datos no existe", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", 
                    $"Error al eliminar la base de datos: {ex.Message}", "OK");
            }
        }
    }
} 