using SistemaAlarmaMovil.Common;
using SistemaAlarmaMovil.Domain;
using SistemaAlarmaMovil.Services.Interfaces;

namespace SistemaAlarmaMovil.Services
{
    public class PacienteServiceWeb : IPacienteServiceWeb
    {
        private readonly HttpClientService _httpClientService;

        public PacienteServiceWeb(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<ServiceResponse> AgregarOModificar(PacienteDto paciente)
        {
            var response = new ServiceResponse();
            try
            {
                var success = await _httpClientService.PostAsync("Paciente/agregarOModificar", paciente);
                if (!success)
                {
                    response.AddError("No se pudo guardar o actualizar el paciente");
                }
            }
            catch (Exception ex)
            {
                response.AddError(ex.Message);
            }
            return response;
        }
    }
}
