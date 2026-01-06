using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection; // Cần namespace này để dùng GetService<T>
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using System;

namespace SalesManagement.WinUI.Views.Components
{
    public sealed partial class CreateOrderDialog : ContentDialog
    {
        public CreateOrderViewModel ViewModel { get; set; } = default!;

        public CreateOrderDialog( )
        {
            this.InitializeComponent();

            // 1. Lấy các service từ DI Container (App.Services)
            var orderService = App.Services.GetRequiredService<IOrderService>();
            var productService = App.Services.GetRequiredService<IProductService>();
            var promotionService = App.Services.GetRequiredService<IPromotionService>();

           
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            args.Cancel = true; // Mặc định giữ dialog mở để đợi xử lý async

            try
            {
                bool success = await ViewModel.CreateOrderAsync();
                if (success)
                {
                    args.Cancel = false; // Cho phép đóng dialog nếu thành công
                }
                else
                {
                    // Xử lý khi lỗi (ví dụ hiện thông báo đỏ dưới footer)
                    // Hiện tại giữ dialog mở để user sửa lại
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                System.Diagnostics.Debug.WriteLine($"Lỗi tạo đơn: {ex.Message}");
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}