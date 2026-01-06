using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using SalesManagement.WinUI.Views.Dialogs;
using Windows.System;

namespace SalesManagement.WinUI.Views;

public sealed partial class LoginPage : Page
{
    public LoginViewModel ViewModel { get; }

    public LoginPage()
    {
        InitializeComponent();

        // Get ViewModel from DI
        ViewModel = App.Services.GetService<LoginViewModel>()!;

        // Subscribe to login completed event
        ViewModel.LoginCompleted += OnLoginCompleted;

        // Try auto login
        _ = TryAutoLoginAsync();
    }

    private async Task TryAutoLoginAsync()
    {
        await ViewModel.TryAutoLoginAsync();
    }

    private void OnLoginCompleted(object? sender, bool success)
    {
        if (success)
        {
            // Navigate to main page
            NavigateToMainPage();
        }
    }

    private void NavigateToMainPage()
    {
        // Get the current frame from the window
        if (App.MainWindow.Content is Frame rootFrame)
        {
            rootFrame.Navigate(typeof(MainPage));
        }
    }

    private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            // Move focus to password
            PasswordBox.Focus(FocusState.Programmatic);
        }
    }

    private async void OnPasswordBoxKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            // Execute login command
            if (ViewModel.LoginCommand.CanExecute(null))
            {
                await ViewModel.LoginCommand.ExecuteAsync(null);
            }
        }
    }

    private async void OnServerConfigClicked(object sender, RoutedEventArgs e)
    {
        var dialog = new ServerConfigDialog(
            App.Services.GetRequiredService<IAppSettingsService>())
        {
            XamlRoot = this.XamlRoot
        };

        await dialog.ShowAsync();
    }

}