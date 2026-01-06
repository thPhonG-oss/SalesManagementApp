using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using SalesManagement.WinUI.Views.Dialogs;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SalesManagement.WinUI.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage()
        {
            this.InitializeComponent();
            try
            {
                ViewModel = App.Services.GetRequiredService<SettingsViewModel>();
                this.DataContext = ViewModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi khởi tạo SettingsViewModel: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        private async void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var userService = App.Services.GetRequiredService<IUserService>();

            var dialog = new ChangePasswordDialog
            {
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result != ContentDialogResult.Primary)
                return;

            // Validate
            if (string.IsNullOrWhiteSpace(dialog.OldPassword) ||
                string.IsNullOrWhiteSpace(dialog.NewPassword) ||
                string.IsNullOrWhiteSpace(dialog.ConfirmPassword))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng nhập đầy đủ thông tin",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (dialog.NewPassword != dialog.ConfirmPassword)
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Mật khẩu xác nhận không khớp",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            try
            {
                await userService.ChangePasswordAsync(new ChangePasswordRequest
                {
                    OldPassword = dialog.OldPassword,
                    NewPassword = dialog.NewPassword,
                    ConfirmNewPassword = dialog.ConfirmPassword
                });

                ViewModel.StatusMessage = "Đổi mật khẩu thành công";
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = ex.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }

    }
}
