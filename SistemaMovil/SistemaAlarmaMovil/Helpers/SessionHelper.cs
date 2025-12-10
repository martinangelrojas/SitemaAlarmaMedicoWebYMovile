using SistemaAlarmaMovil.GoogleServices;
using SistemaAlarmaMovil.Helpers.Interfaces;
using SistemaAlarmaMovil.Models;
using SistemaAlarmaMovil.Repositories.Interfaces;

namespace SistemaAlarmaMovil.Helpers
{
    public class SessionHelper : ISessionHelper
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IPacienteRepository _pacienteRepository;

        public SessionHelper(IGoogleAuthService googleAuthService, IPacienteRepository pacienteRepository)
        {
            _googleAuthService = googleAuthService;
            _pacienteRepository = pacienteRepository;
        }
        
        public async Task<Paciente> ObtenerSessionUsuarioYRegistroBD()
        {
            var loggedUser = await ObtenerSessionUsuario();

            if (loggedUser != null)
            {
                var pacientes = await _pacienteRepository.GetAllAsync();
                return pacientes.FirstOrDefault(p => p.Email == loggedUser.Email);
            }

            return null;
        }

        public async Task<GoogleUserDTO> ObtenerSessionUsuario()
        {
            var loggedUser = await _googleAuthService.GetCurrentUserAsync();
            //var loggedUser = new GoogleUserDTO()
            //{
            //    Email = "juan.foxer@gmail.com",
            //    FullName = "Juan Arce",
            //    TokenId = "33764132",
            //    UserName = "juan.arce"
            //};
            return loggedUser;
        }

        public async Task LogoutAsync()
        {
            await _googleAuthService.LogoutAsync();
        }
    }
}
