using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SalesManagement.WinUI.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        private readonly IAuthService _authService;

        public UserService(
            IHttpClientFactory httpClientFactory,
            IAuthService authService)
        {
            _client = httpClientFactory.CreateClient("API");
            _authService = authService;
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request)
        {
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _client.PostAsJsonAsync(
                "/api/v1/users/change-password",
                request);

            Debug.WriteLine($"[CHANGE PASSWORD] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[CHANGE PASSWORD ERROR] {error}");
                throw new Exception(error);
            }
        }
    }
}
