// SalesManagement.WinUI/Services/DialogService.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using SalesManagement.WinUI.Views;
using SalesManagement.WinUI.Views.Components; // Nơi chứa OrderEditDialog
using System;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services
{
    public class DialogService : IDialogService
    {
        private XamlRoot? _xamlRoot;

        public void SetXamlRoot(XamlRoot xamlRoot)
        {
            _xamlRoot = xamlRoot;
        }

        public async Task<bool> ShowConfirmationAsync(string title, string message, string primaryText, string closeText)
        {
            if (_xamlRoot == null) return false;

            var dialog = new ContentDialog
            {
                XamlRoot = _xamlRoot,
                Title = title,
                Content = message,
                PrimaryButtonText = primaryText,
                CloseButtonText = closeText,
                DefaultButton = ContentDialogButton.Close,
                // Dùng Resource an toàn hơn
                PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style
            };

            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        public async Task<bool> ShowEditOrderDialogAsync(OrderItemViewModel orderVm)
        {
            if (_xamlRoot == null) return false;

            // Tạo Dialog từ Component đã có
            var dialog = new OrderEditDialog(orderVm);
            dialog.XamlRoot = _xamlRoot;

            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        public async Task ShowOrderDetailDialogAsync(OrderItemViewModel orderVm)
        {
            if (_xamlRoot == null) return;

            // Khởi tạo Dialog chi tiết (Code cũ của bạn chuyển vào đây)
            var dialog = new OrderDetailDialog(orderVm);
            dialog.XamlRoot = _xamlRoot;

            // Chỉ cần Show, không cần trả về kết quả vì chỉ là xem chi tiết
            await dialog.ShowAsync();
        }

        public async Task<bool> ShowCreateOrderDialogAsync()
        {
            if (_xamlRoot == null) return false;

            var dialog = new CreateOrderDialog();
            dialog.XamlRoot = _xamlRoot;

            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }
    }
}