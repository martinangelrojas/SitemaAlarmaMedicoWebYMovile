using SistemaAlarmaMovil.Common;
using SistemaAlarmaMovil.Domain;

namespace SistemaAlarmaMovil.Services
{
    public class OrdenMedicaServiceWeb : IOrdenMedicaServiceWeb
    {
        private readonly HttpClientService _httpClientService;

        public OrdenMedicaServiceWeb(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<OrdenMedicaDto> ObtenerPorId(int id)
        {
            return await _httpClientService.GetAsync<OrdenMedicaDto>($"OrdenMedica/obtenerPorId/{id}");
        }

        public async Task<List<OrdenMedicaDto>> ObtenerTodos()
        {
            try
            {
                var result = await _httpClientService.GetAsync<List<OrdenMedicaDto>>("OrdenMedica/obtenerTodos");
                return result ?? new List<OrdenMedicaDto>();
            }
            catch (Exception)
            {
                return new List<OrdenMedicaDto>();
            }
        }

        public async Task<ServiceResponse> TomarOrdenMedica(int ordenMedicaId)
        {
            var response = new ServiceResponse();
            try
            {
                var success = await _httpClientService.PutAsync($"OrdenMedica/tomarOrdenMedica?ordenMedicaId={ordenMedicaId}");
                if (!success)
                {
                    response.AddError("No se pudo actualizar la orden médica");
                }
            }
            catch (Exception ex)
            {
                response.AddError(ex.Message);
            }
            return response;
        }

        public async Task<List<OrdenMedicaDto>> ObtenerPorDni(int dni)
        {
            try
            {
                var result = await _httpClientService.GetAsync<List<OrdenMedicaDto>>($"OrdenMedica/obtenerPorDni?dni={dni}");
                return result ?? new List<OrdenMedicaDto>();
            }
            catch (Exception)
            {
                return new List<OrdenMedicaDto>();
            }
        }

        public async Task<ServiceResponse> EmpezarTratamiento(int lineaOrdenMedicaId)
        {
            var response = new ServiceResponse();
            try
            {
                var success = await _httpClientService.PutAsync($"OrdenMedica/empezarTratamientoLineaOrden?lineaOrdenMedicaId={lineaOrdenMedicaId}");
                if (!success)
                {
                    response.AddError("No se pudo actualizar la línea orden médica");
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
