using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.ViewModels;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class PromotionPage : Page
    {
        public PromotionViewModel ViewModel { get; }

        public PromotionPage(PromotionViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += PromotionsPage_Loaded;
        }

        private async void PromotionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadPromotionsAsync();
        }
    }
}
