using System.ComponentModel;

namespace SistemaAlarmaMovil.Domain
{
    public class OrdenMedicaDto
    {
        public int? OrdenMedicaId { get; set; }
        public int? PacienteId { get; set; }
        public PacienteDto? Paciente { get; set; }
        public int? MedicoId { get; set; }
        public MedicoDto? Medico { get; set; }
        public DateTime? Fecha { get; set; }
        public ObraSocialDto? ObraSocial { get; set; }
        public bool? EntregadaAlPaciente { get; set; }
        public List<LineaOrdenMedicaDto>? LineaOrdenMedica { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
