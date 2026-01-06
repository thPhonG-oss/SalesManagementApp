using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.Views;
using System.Collections.ObjectModel;

namespace SalesManagement.WinUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;
    private readonly ILoadingService _loadingService; 

    // Danh sách menu
    public ObservableCollection<AppMenuItem> MainMenuItems { get; } = new();
    public ObservableCollection<AppMenuItem> FooterMenuItems { get; } = new();

    // Item đang được chọn
    [ObservableProperty]
    private AppMenuItem? _selectedMenuItem;

    public MainViewModel(IAuthService authService,
                         INavigationService navigationService,
                         ILoadingService loadingService)
    {
        _authService = authService;
        _navigationService = navigationService;
        _loadingService = loadingService;

        LoadMenu();
    }

    private void LoadMenu()
    {
        MainMenuItems.Add(new AppMenuItem
        {
            Title = "Tổng quan",
            Icon = Symbol.Home,
            PageType = typeof(DashboardPage),
            ShowHeader = true
        });

        MainMenuItems.Add(new AppMenuItem
        {
            Title = "Sản phẩm",
            Icon = Symbol.Tag, 
            PageType = typeof(ProductPage),
            ShowHeader = false
        });

        MainMenuItems.Add(new AppMenuItem
        {
            Title = "Đơn hàng",
            Icon = Symbol.List, 
            PageType = typeof(OrderPage), 
            ShowHeader = false
        });

        MainMenuItems.Add(new AppMenuItem
        {
            Title = "Báo cáo",
            Icon = Symbol.FourBars, 
            PageType = typeof(ReportPage),
            ShowHeader = false
        });

        MainMenuItems.Add(new AppMenuItem
        {
            Title = "Khách hàng",   
            Icon = Symbol.People,
            PageType = typeof(CustomerPage),
            ShowHeader = false
        });

        MainMenuItems.Add(new AppMenuItem
        {
            Title = "Cài đặt",
            Icon = Symbol.Setting,
            PageType = typeof(SettingsPage),
            ShowHeader = false
        });




        FooterMenuItems.Add(new AppMenuItem { Title = "Đăng xuất", Icon = Symbol.Contact, Tag = "Logout" });

        // Chọn trang đầu tiên
        SelectedMenuItem = MainMenuItems.FirstOrDefault();
    }

    // Command xử lý khi menu thay đổi
    // Hàm này sẽ được gọi khi View thay đổi SelectedItem
    partial void OnSelectedMenuItemChanged(AppMenuItem? value)
    {
        if (value == null) return;

        if (value.Tag == "Logout")
        {
            LogoutCommand.Execute(null);
        }
        else if (value.PageType != null)
        {
            _navigationService.NavigateTo(value.PageType);
            _navigationService.SetHeader(value.Title ?? string.Empty);

            string? headerText = value.ShowHeader ? value.Title : null;

            _navigationService.SetHeader(headerText);
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        // Gọi Service hiển thị Dialog 
        await _loadingService.ShowAsync("Đang đăng xuất...");
        try
        {
            await _authService.LogoutAsync();
            // Điều hướng về Login 
            if (App.MainWindow.Content is Frame rootFrame)
            {
                rootFrame.Navigate(typeof(LoginPage));
                rootFrame.BackStack.Clear();
            }
        }
        finally
        {
            _loadingService.Hide();
        }
    }
}