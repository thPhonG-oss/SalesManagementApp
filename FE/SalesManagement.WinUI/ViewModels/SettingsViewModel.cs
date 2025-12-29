// FE/SalesManagement.WinUI/ViewModels/SettingsViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.ObjectModel;

namespace SalesManagement.WinUI.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly IStorageService _storageService;
        private AppSettings _originalSettings = new();

        // ===== DANH SÁCH LỰA CHỌN =====
        public ObservableCollection<int> PageSizeOptions { get; } = new()
        {
            5, 10, 15, 20
        };

        public ObservableCollection<string> ThemeOptions { get; } = new()
        {
            "Light", "Dark", "System"
        };

        // ===== PROPERTIES =====
        [ObservableProperty]
        private int _selectedPageSize = 20;

        [ObservableProperty]
        private bool _rememberLastScreen = true;

        [ObservableProperty]
        private string _selectedTheme = "Light";

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private bool _hasChanges = false;

        // ===== COMMANDS =====
        public IAsyncRelayCommand SaveCommand { get; }
        public IAsyncRelayCommand ResetCommand { get; }
        public IRelayCommand CancelCommand { get; }

        public SettingsViewModel(IStorageService storageService)
        {
            _storageService = storageService;

            SaveCommand = new AsyncRelayCommand(SaveSettingsAsync);
            ResetCommand = new AsyncRelayCommand(ResetSettingsAsync);
            CancelCommand = new RelayCommand(CancelChanges);

            _ = LoadSettingsAsync();
        }

        // ===== LOAD SETTINGS =====
        private async Task LoadSettingsAsync()
        {
            try
            {
                var settings = await _storageService.GetAppSettingsAsync();

                _originalSettings = new AppSettings
                {
                    ItemsPerPage = settings.ItemsPerPage,
                    RememberLastScreen = settings.RememberLastScreen,
                    Theme = settings.Theme
                };

                SelectedPageSize = settings.ItemsPerPage;
                RememberLastScreen = settings.RememberLastScreen;
                SelectedTheme = settings.Theme;

                HasChanges = false;
                StatusMessage = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi tải cấu hình: {ex.Message}";
            }
        }

        // ===== SAVE SETTINGS =====
        private async Task SaveSettingsAsync()
        {
            try
            {
                var settings = new AppSettings
                {
                    ItemsPerPage = SelectedPageSize,
                    RememberLastScreen = RememberLastScreen,
                    Theme = SelectedTheme
                };

                await _storageService.SaveAppSettingsAsync(settings);

                // ⭐ ÁP DỤNG THEME NGAY LẬP TỨC
                ApplyTheme(SelectedTheme);

                _originalSettings = settings;
                HasChanges = false;
                StatusMessage = "✅ Đã lưu cấu hình thành công!";

                await Task.Delay(2000);
                StatusMessage = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Lỗi lưu cấu hình: {ex.Message}";
            }
        }

        // ===== RESET SETTINGS =====
        private async Task ResetSettingsAsync()
        {
            try
            {
                await _storageService.ResetAppSettingsAsync();
                await LoadSettingsAsync();

                // ⭐ ÁP DỤNG THEME MẶC ĐỊNH
                ApplyTheme(SelectedTheme);

                StatusMessage = "✅ Đã khôi phục cấu hình mặc định!";
                await Task.Delay(2000);
                StatusMessage = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Lỗi reset cấu hình: {ex.Message}";
            }
        }

        // ===== CANCEL CHANGES =====
        private void CancelChanges()
        {
            SelectedPageSize = _originalSettings.ItemsPerPage;
            RememberLastScreen = _originalSettings.RememberLastScreen;
            SelectedTheme = _originalSettings.Theme;

            HasChanges = false;
            StatusMessage = "Đã hủy thay đổi";
        }

        // ===== PROPERTY CHANGED HANDLERS =====
        partial void OnSelectedPageSizeChanged(int value)
        {
            CheckForChanges();
        }

        partial void OnRememberLastScreenChanged(bool value)
        {
            CheckForChanges();
        }

        partial void OnSelectedThemeChanged(string value)
        {
            CheckForChanges();
        }

        private void CheckForChanges()
        {
            HasChanges = SelectedPageSize != _originalSettings.ItemsPerPage
                      || RememberLastScreen != _originalSettings.RememberLastScreen
                      || SelectedTheme != _originalSettings.Theme;
        }

        // ⭐⭐⭐ HÀM ÁP DỤNG THEME - QUAN TRỌNG ⭐⭐⭐
        private void ApplyTheme(string theme)
        {
            try
            {
                if (App.MainWindow?.Content is FrameworkElement rootElement)
                {
                    rootElement.RequestedTheme = theme switch
                    {
                        "Dark" => ElementTheme.Dark,
                        "Light" => ElementTheme.Light,
                        _ => ElementTheme.Default
                    };

                    System.Diagnostics.Debug.WriteLine($"✅ Theme applied: {theme}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error applying theme: {ex.Message}");
            }
        }
    }
}