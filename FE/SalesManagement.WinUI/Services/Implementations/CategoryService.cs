using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SalesManagement.WinUI.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public CategoryService(
            IHttpClientFactory httpClientFactory,
            IAuthService authService)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _authService = authService;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            try
            {
                var token = _authService.GetAccessToken();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                var response =
                    await _httpClient.GetFromJsonAsync<
                        ApiResponse<List<Category>>
                    >("/api/categories");

                //System.Diagnostics.Debug.WriteLine(response);


                if (response == null || !response.Success)
                    return new List<Category>();

                return response.Data ?? new List<Category>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CategoryService] Error: {ex.Message}");
                return new List<Category>();
            }
        }
    }

}
