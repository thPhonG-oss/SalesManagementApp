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

        private void SetAuthHeader()
        {
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Category>> GetAllAsync()
        {
            try
            {
                SetAuthHeader();

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Category>>>("/api/categories");

                if (response == null || !response.Success)
                {
                    Debug.WriteLine("[CategoryService] GetAllAsync: Response failed or null");
                    return new List<Category>();
                }

                return response.Data ?? new List<Category>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CategoryService] GetAllAsync Error: {ex.Message}");
                return new List<Category>();
            }
        }

        public async Task<bool> CreateAsync(CreateCategoryRequest request)
        {
            try
            {
                SetAuthHeader();

                Debug.WriteLine($"[CategoryService] CreateAsync - Name: {request.CategoryName}, Desc: {request.Description}, Active: {request.IsActive}");

                var response = await _httpClient.PostAsJsonAsync("/api/categories", request);
                Debug.WriteLine("aaaaa" + response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("[CategoryService] CreateAsync: Success");
                    return true;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[CategoryService] CreateAsync Failed: {response.StatusCode} - {content}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CategoryService] CreateAsync Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            try
            {
                SetAuthHeader();

                Debug.WriteLine($"[CategoryService] UpdateAsync - ID: {id}, Name: {request.CategoryName}, Desc: {request.Description}, Active: {request.IsActive}");

                var response = await _httpClient.PutAsJsonAsync($"/api/categories/{id}", request);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("[CategoryService] UpdateAsync: Success");
                    return true;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[CategoryService] UpdateAsync Failed: {response.StatusCode} - {content}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CategoryService] UpdateAsync Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.DeleteAsync($"/api/categories/{id}");
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("[CategoryService] DeleteAsync: Success");
                    return true;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[CategoryService] DeleteAsync Failed: {response.StatusCode} - {content}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CategoryService] DeleteAsync Error: {ex.Message}");
                return false;
            }
        }
    }
}