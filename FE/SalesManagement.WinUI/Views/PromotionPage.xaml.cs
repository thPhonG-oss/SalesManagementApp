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
    }
}
