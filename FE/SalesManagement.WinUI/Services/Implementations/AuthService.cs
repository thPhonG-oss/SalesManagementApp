using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;

namespace SalesManagement.WinUI.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private UserResponse? _currentUser;
    private string? _accessToken;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<(bool Success, string ErrorMessage, UserResponse? User)> LoginAsync(string username, string password)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("API");

            var loginRequest = new LoginRequest
            {
                Username = username,
                Password = password
            };

            var response = await client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserResponse>();

                if (user != null)
                {
                    _currentUser = user;
                    _accessToken = user.AccessToken;

                    return (true, string.Empty, user);
                }

                return (false, "Invalid response from server", null);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return (false, "Invalid username or password", null);
            }
            else
            {
                return (false, $"Login failed: {response.StatusCode}", null);
            }
        }
        catch (HttpRequestException ex)
        {
            return (false, $"Network error: {ex.Message}", null);
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred: {ex.Message}", null);
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("API");

            var request = new { token };
            var response = await client.PostAsJsonAsync("/api/v1/auth/authenticate", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<bool>();
                return result;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<(bool Success, UserResponse? User)> RefreshTokenAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("API");

            var response = await client.PostAsync("/api/v1/auth/refresh", null);

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserResponse>();

                if (user != null)
                {
                    _currentUser = user;
                    _accessToken = user.AccessToken;
                    return (true, user);
                }
            }

            return (false, null);
        }
        catch
        {
            return (false, null);
        }
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("API");

            if (!string.IsNullOrEmpty(_accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            var response = await client.PostAsync("/api/v1/auth/logout", null);
            


            _currentUser = null;
            _accessToken = null;
            
            return response.IsSuccessStatusCode;
        }
        catch
        {
            _currentUser = null;
            _accessToken = null;
            return false;
        }
    }

    public UserResponse? GetCurrentUser()
    {
        return _currentUser;
    }

    public string? GetAccessToken()
    {
        return _accessToken;
    }

    public bool IsAuthenticated()
    {
        return _currentUser != null && !string.IsNullOrEmpty(_accessToken);
    }
}