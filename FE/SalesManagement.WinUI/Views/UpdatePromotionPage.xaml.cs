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
        public PromotionResponse Promotion { get; private set; }

        public UpdatePromotionPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is PromotionResponse promotion)
            {
                Promotion = promotion;

                // 🔥 SET DataContext để Binding hoạt động
                DataContext = Promotion;

                DiscountValueBox.Value = (double)promotion.DiscountValue;
                StartDatePicker.Date = new DateTimeOffset(promotion.StartDate);
                EndDatePicker.Date = new DateTimeOffset(promotion.EndDate);

            }
            else
            {
                Debug.WriteLine("❌ Navigation parameter KHÔNG phải PromotionResponse");
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Promotion == null) return;

            var promotionService =
                App.Services.GetService<IPromotionService>();

            var request = new UpdatePromotionRequest
            {
                PromotionName = Promotion.PromotionName,
                Description = Promotion.Description,
                DiscountType = Promotion.DiscountType,
                DiscountValue = DiscountValueBox.Value,
                MinOrderValue = (double)Promotion.MinOrderAmount,
                MaxDiscountValue = (double)Promotion.MaxDiscountValue,
                UsageLimit = Promotion.UsageLimit,
                Active = Promotion.IsActive
            };


            Debug.WriteLine("==== " + request.DiscountValue);
            var success = await promotionService.UpdatePromotionAsync(
                Promotion.PromotionId,
                request
            );

            if (success)
            {
                Debug.WriteLine("✅ Cập nhật promotion thành công");
                if (Frame.CanGoBack)
                    Frame.GoBack();
            }
            else
            {
                Debug.WriteLine("❌ Cập nhật promotion thất bại");
            }
        }



        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

    }
}
