using System.Text;
using System.Text.Json;

namespace SistemaAlarmaMovil.Common
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;
        //private readonly string _baseUrl = "https://10.0.2.2:7131";
        //private readonly string _baseUrl = "http://192.168.0.6:5000";
        private readonly string _baseUrl = "https://presentacionapi-app-202511081941.mangoriver-de8fe4e4.brazilsouth.azurecontainerapps.io";
        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                return JsonSerializer.Deserialize<T>(content, options);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public async Task<bool> PostAsync<T>(string endpoint, T data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> PutAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.PutAsync($"{_baseUrl}/{endpoint}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
} 