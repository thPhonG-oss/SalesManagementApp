using SalesManagement.WinUI.Models;

namespace SalesManagement.WinUI.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Authenticates user with username and password
    /// </summary>
    Task<(bool Success, string ErrorMessage, UserResponse? User)> LoginAsync(string username, string password);

    /// <summary>
    /// Validates if a token is still valid
    /// </summary>
    Task<bool> ValidateTokenAsync(string token);

    /// <summary>
    /// Refreshes the access token using refresh token
    /// </summary>
    Task<(bool Success, UserResponse? User)> RefreshTokenAsync();

    /// <summary>
    /// Logs out the current user
    /// </summary>
    Task<bool> LogoutAsync();

    /// <summary>
    /// Gets the current user information
    /// </summary>
    UserResponse? GetCurrentUser();

    /// <summary>
    /// Gets the current access token
    /// </summary>
    string? GetAccessToken();

    /// <summary>
    /// Checks if user is authenticated
    /// </summary>
    bool IsAuthenticated();
}