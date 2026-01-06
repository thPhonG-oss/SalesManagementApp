using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Diagnostics;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class UpdatePromotionPage : Page
    {
        public UpdatePromotionViewModel ViewModel { get; private set; }

        public UpdatePromotionPage()
        {
            this.InitializeComponent();
            ViewModel = new UpdatePromotionViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is PromotionResponse promotion)
            {
                // Load dữ liệu vào ViewModel
                ViewModel.LoadFrom(promotion);

                // Set DataContext
                DataContext = ViewModel;

                // Set giá trị cho các control không thể bind trực tiếp
                DiscountValueBox.Value = (double)promotion.DiscountValue;
                StartDatePicker.Date = new DateTimeOffset(promotion.StartDate);
                EndDatePicker.Date = new DateTimeOffset(promotion.EndDate);

                Debug.WriteLine($"✅ Loaded promotion: {promotion.PromotionName}");
                Debug.WriteLine($"   StartDate: {promotion.StartDate}");
                Debug.WriteLine($"   EndDate: {promotion.EndDate}");
            }
            else
            {
                Debug.WriteLine("❌ Navigation parameter KHÔNG phải PromotionResponse");
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null || string.IsNullOrEmpty(ViewModel.PromotionCode))
            {
                Debug.WriteLine("❌ ViewModel chưa được khởi tạo");
                return;
            }

            // Lấy giá trị từ DatePicker
            var startDate = StartDatePicker.Date.DateTime;
            var endDate = EndDatePicker.Date.DateTime;

            // Validate
            if (endDate < startDate)
            {
                var dialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Ngày kết thúc phải sau ngày bắt đầu",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
                return;
            }

            var promotionService = App.Services.GetService<IPromotionService>();

            var request = new UpdatePromotionRequest
            {
                PromotionName = ViewModel.PromotionName,
                Description = ViewModel.Description,
                DiscountType = ViewModel.DiscountType,
                DiscountValue = DiscountValueBox.Value,
                MinOrderValue = ViewModel.MinOrderValue,
                MaxDiscountValue = ViewModel.MaxDiscountValue,
                UsageLimit = (int)ViewModel.UsageLimit,
                Active = ViewModel.IsActive,
                StartDate = startDate,
                EndDate = endDate
            };

            Debug.WriteLine($"==== Updating Promotion ====");
            Debug.WriteLine($"ID: {ViewModel.PromotionId}");
            Debug.WriteLine($"Name: {request.PromotionName}");
            Debug.WriteLine($"StartDate: {request.StartDate}");
            Debug.WriteLine($"EndDate: {request.EndDate}");

            var success = await promotionService.UpdatePromotionAsync(
                ViewModel.PromotionId,
                request
            );

            if (success)
            {
                Debug.WriteLine("✅ Cập nhật promotion thành công");

                var successDialog = new ContentDialog
                {
                    Title = "Thành công",
                    Content = "Cập nhật khuyến mãi thành công!",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await successDialog.ShowAsync();

                if (Frame.CanGoBack)
                    Frame.GoBack();
            }
            else
            {
                Debug.WriteLine("❌ Cập nhật promotion thất bại");

                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Không thể cập nhật khuyến mãi. Vui lòng thử lại.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}