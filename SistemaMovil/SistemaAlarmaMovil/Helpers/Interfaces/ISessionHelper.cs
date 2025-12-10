using SistemaAlarmaMovil.GoogleServices;
using SistemaAlarmaMovil.Models;

namespace SistemaAlarmaMovil.Helpers.Interfaces
{
    public interface ISessionHelper
    {
        Task<Paciente> ObtenerSessionUsuarioYRegistroBD();
        Task<GoogleUserDTO> ObtenerSessionUsuario();
        public Task LogoutAsync();
    }
}
