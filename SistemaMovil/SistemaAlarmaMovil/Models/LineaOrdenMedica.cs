using SQLite;

namespace SistemaAlarmaMovil.Models
{
    public class LineaOrdenMedica
    {
        [PrimaryKey, AutoIncrement]
        public int LineaId { get; set; }
        public int LineaOrdenMedicaId { get; set; }
        public bool LineaOrdenEmpezada { get; set; }
    }
}
