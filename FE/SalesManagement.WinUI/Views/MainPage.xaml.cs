using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Services.Interfaces;

namespace SalesManagement.WinUI.Views;

public sealed partial class MainPage : Page
{
    private readonly IAuthService _authService;
    private readonly IStorageService _storageService;

    public MainPage()
    {
        InitializeComponent();

        _authService = App.Services.GetService<IAuthService>()!;
        _storageService = App.Services.GetService<IStorageService>()!;
    }

    private async void OnLogoutClick(object sender, RoutedEventArgs e)
    {
        // Logout (this clears tokens on server and in memory)
        await _authService.LogoutAsync();

        // DON'T clear credentials if user chose Remember Me
        // await _storageService.ClearCredentialsAsync(); // REMOVED

        // Navigate back to login using root frame
        if (App.MainWindow.Content is Frame rootFrame)
        {
            rootFrame.Navigate(typeof(LoginPage));
        }
    }
}