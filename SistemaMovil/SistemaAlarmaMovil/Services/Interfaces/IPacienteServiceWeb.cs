using SistemaAlarmaMovil.Common;
using SistemaAlarmaMovil.Domain;

namespace SistemaAlarmaMovil.Services.Interfaces
{
    public interface IPacienteServiceWeb
    {
        Task<ServiceResponse> AgregarOModificar(PacienteDto paciente);
    }
}