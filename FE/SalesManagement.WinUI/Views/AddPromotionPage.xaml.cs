using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using System.Diagnostics;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class AddPromotionPage : Page
    {
        public AddPromotionViewModel ViewModel { get; }

        public AddPromotionPage()
        {
            this.InitializeComponent();

            var promotionService = App.Services.GetService(typeof(IPromotionService)) as IPromotionService;
            ViewModel = new AddPromotionViewModel(promotionService!);

            DataContext = ViewModel;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var success = await ViewModel.SaveAsync();
            Debug.WriteLine($"[AddPromotionPage] Save_Click: success = {success}");
            if (success)
            {
                Frame.GoBack();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
