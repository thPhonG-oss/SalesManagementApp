using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SalesManagement.WinUI.Services.Interfaces;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IStorageService _storageService;

    public LoginViewModel(IAuthService authService, IStorageService storageService)
    {
        _authService = authService;
        _storageService = storageService;

        // Get app version
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        AppVersion = version != null
            ? $"Version {version.Major}.{version.Minor}.{version.Build}"
            : "Version 1.0.0";
    }

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _rememberMe = false;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    [ObservableProperty]
    private bool _hasError = false;

    // Event for navigation
    public event EventHandler<bool>? LoginCompleted;

    [RelayCommand]
    private async Task LoginAsync()
    {
        // Clear previous errors
        ErrorMessage = string.Empty;
        HasError = false;

        // Validate input
        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorMessage = "Please enter your username";
            HasError = true;
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter your password";
            HasError = true;
            return;
        }

        IsLoading = true;

        try
        {
            var (success, errorMessage, user) = await _authService.LoginAsync(Username, Password);

            if (success && user != null)
            {
                // Save credentials if remember me is checked
                if (RememberMe)
                {
                    await _storageService.SaveCredentialsAsync(Username, Password, RememberMe);
                }
                else
                {
                    await _storageService.ClearCredentialsAsync();
                }

                // Raise login completed event
                LoginCompleted?.Invoke(this, true);
            }
            else
            {
                ErrorMessage = errorMessage;
                HasError = true;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            HasError = true;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void NavigateToConfig()
    {
        // TODO: Navigate to config screen
    }

    public async Task TryAutoLoginAsync()
    {
        try
        {
            var credentials = await _storageService.GetStoredCredentialsAsync();

            if (credentials != null && credentials.RememberMe)
            {
                Username = credentials.Username;
                Password = credentials.EncryptedPassword;
                RememberMe = true;

                // Validate stored token
                var isValid = await _authService.ValidateTokenAsync(credentials.EncryptedPassword);

                if (isValid)
                {
                    // Auto login
                    await LoginAsync();
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Auto login failed: {ex.Message}");
        }
    }

    partial void OnUsernameChanged(string value)
    {
        // Clear error when user starts typing
        if (HasError)
        {
            ErrorMessage = string.Empty;
            HasError = false;
        }
    }

    partial void OnPasswordChanged(string value)
    {
        // Clear error when user starts typing
        if (HasError)
        {
            ErrorMessage = string.Empty;
            HasError = false;
        }
    }
}