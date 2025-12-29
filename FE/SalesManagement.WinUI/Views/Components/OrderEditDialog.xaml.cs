using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Services.Interfaces; // Đảm bảo import Service
using SalesManagement.WinUI.ViewModels;
using System;

// Namespace thay đổi thêm .Components
namespace SalesManagement.WinUI.Views.Components
{
    public sealed partial class OrderEditDialog : ContentDialog
    {
        public OrderItemViewModel ViewModel { get; }

        public double TempAmount { get; set; }
        public string TempStatus { get; set; }
        public DateTimeOffset TempDate { get; set; }
        public OrderEditDialog(OrderItemViewModel viewModel)
        {
            this.InitializeComponent();
            ViewModel = viewModel;
            TempAmount = viewModel.AmountDouble;
            TempStatus = viewModel.Status;
            TempDate = viewModel.DateOffset;
        }

        // Xử lý khi bấm nút "Lưu thay đổi"
        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            args.Cancel = true; // Giữ dialog mở để xử lý
            ErrorBar.IsOpen = false;

            try
            {
                // GỌI HÀM SAVE CỦA VIEWMODEL (Không gọi Service trực tiếp)
                // Truyền các giá trị tạm vào để VM cập nhật
                bool success = await ViewModel.SaveAsync(TempAmount, TempStatus, TempDate);

                if (success)
                {
                    args.Cancel = false; // Đóng dialog
                }
                else
                {
                    ErrorBar.Message = "Không thể cập nhật. Vui lòng thử lại.";
                    ErrorBar.IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                ErrorBar.Message = ex.Message;
                ErrorBar.IsOpen = true;
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}