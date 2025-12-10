namespace SistemaAlarmaMovil.Domain
{
    public class PacienteDto
    {
        public int? PacienteId { get; set; }
        public string? Apellido { get; set; }
        public string? Nombre { get; set; }
        public int? Documento { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string NombreCompleto => $"{Apellido}, {Nombre}";
    }
}
