using SQLite;

namespace SistemaAlarmaMovil.Models
{
    public class Paciente
    {
        [PrimaryKey, AutoIncrement]
        public int PacienteId { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public int Documento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public bool ValidadoConServidor { get; set; }
        public int? PacienteIdSistemaWeb { get; set; }

        [Ignore]
        public string NombreCompleto => $"{Apellido}, {Nombre}";
    }
} 