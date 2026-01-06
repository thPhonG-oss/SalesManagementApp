using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.ViewModels;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class PromotionPage : Page
    {
        private readonly INavigationService _navigationService;
        public PromotionViewModel ViewModel { get; }

        public PromotionPage()
        {
            InitializeComponent();

            ViewModel = App.Services.GetRequiredService<PromotionViewModel>();
            DataContext = ViewModel;

            _navigationService = App.Services.GetRequiredService<INavigationService>();

            Loaded += PromotionsPage_Loaded;
        }

        private async void PromotionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadPromotionsAsync();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddPromotionPage));
        }

        private void EditPromotion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
                button.DataContext is PromotionResponse promotion)
            {
                Frame.Navigate(typeof(UpdatePromotionPage), promotion);
            }
        }

        private async Task ShowSuccessDialog()
        {
            var dialog = new ContentDialog
            {
                Title = "Thành công",
                Content = "Khuyến mãi đã được hủy thành công.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private async Task ShowErrorDialog()
        {
            var dialog = new ContentDialog
            {
                Title = "Lỗi",
                Content = "Không thể hủy khuyến mãi. Vui lòng thử lại.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private async void CancelPromotion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
                button.DataContext is not PromotionResponse promotion)
                return;

            var dialog = new ContentDialog
            {
                Title = "Xác nhận hủy khuyến mãi",
                Content = $"Bạn có chắc chắn muốn hủy khuyến mãi \"{promotion.PromotionName}\"?",
                PrimaryButtonText = "Hủy khuyến mãi",
                CloseButtonText = "Không",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result != ContentDialogResult.Primary)
                return;

            // 🔥 Gọi API
            var success = await ViewModel.DeactivatePromotionAsync(promotion.PromotionId);

            if (success)
            {
                await ShowSuccessDialog();
                await ViewModel.LoadPromotionsAsync(); // reload list
            }
            else
            {
                await ShowErrorDialog();
            }
        }

    }
}
