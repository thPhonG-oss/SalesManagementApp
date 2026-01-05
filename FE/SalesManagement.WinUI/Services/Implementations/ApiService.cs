using SalesManagement.WinUI.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IAuthService _authService;

        public ApiService(IHttpClientFactory httpClientFactory, IAuthService authService)
        {
            _httpClientFactory = httpClientFactory;
            _authService = authService;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                // Attach token BEFORE making the request
                var token = _authService.GetAccessToken();
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ WARNING: No token available for request to {endpoint}");
                }

                System.Diagnostics.Debug.WriteLine($"📡 GET Request: {endpoint}");
                System.Diagnostics.Debug.WriteLine($"🔐 Token Present: {!string.IsNullOrEmpty(token)}");

                var response = await client.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ API Error ({response.StatusCode}): {errorContent}");
                    throw new HttpRequestException(
                        $"API returned {response.StatusCode}: {errorContent}");
                }

                var json = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"✅ Response received, deserializing...");

                return JsonSerializer.Deserialize<T>(json, _jsonOptions)!;
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"🔴 HTTP Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"🔴 General Error: {ex.GetType().Name} - {ex.Message}");
                throw;
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                var token = _authService.GetAccessToken();
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                var jsonContent = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(jsonContent,
                    System.Text.Encoding.UTF8, "application/json");

                System.Diagnostics.Debug.WriteLine($"📡 POST Request: {endpoint}");
                var response = await client.PostAsync(endpoint, content);

                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"🔴 POST Error: {ex.Message}");
                throw;
            }
        }
    }
}