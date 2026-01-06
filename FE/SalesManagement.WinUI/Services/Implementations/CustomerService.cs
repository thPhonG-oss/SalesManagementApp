using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _client;
        private readonly IAuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions;

        public CustomerService(IHttpClientFactory httpClientFactory, IAuthService authService)
        {
            _client = httpClientFactory.CreateClient("API");
            _authService = authService;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        private void AttachToken()
        {
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<CustomerListData?> GetCustomersAsync(int page = 1, int size = 20, string search = "")
        {
            AttachToken();

            // API Spring Boot đếm trang từ 0, UI đếm từ 1 -> Cần trừ 1
            int apiPage = page > 0 ? page - 1 : 0;

            var url = $"/api/v1/customers?page={apiPage}&size={size}";
            if (!string.IsNullOrEmpty(search))
            {
                url += $"&keyword={search}"; // Kiểm tra lại backend dùng "search" hay "keyword"
            }

            try
            {
                var response = await _client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return null;

                // QUAN TRỌNG: Deserialize trực tiếp vào CustomerListData
                // KHÔNG DÙNG ApiResponse<> vì API trả về json gốc
                var data = JsonSerializer.Deserialize<CustomerListData>(content, _jsonOptions);

                // Chỉnh lại số trang hiển thị cho khớp UI (0 -> 1)
                if (data != null) data.Page++;

                return data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetCustomers] Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            AttachToken();
            var response = await _client.PostAsJsonAsync("/api/v1/customers", customer);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCustomerAsync(int id, Customer customer)
        {
            AttachToken();
            var response = await _client.PutAsJsonAsync($"/api/v1/customers/{id}", customer);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            AttachToken();
            var response = await _client.DeleteAsync($"/api/v1/customers/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}