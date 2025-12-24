using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SalesManagement.WinUI.Views
{
    public sealed partial class ProductPage : Page
    {
        private readonly INavigationService _navigationService;
        public ProductPage()
        {
            InitializeComponent();
            DataContext = App.Services.GetService<ProductViewModel>();

            // ⭐ DÒNG QUAN TRỌNG BỊ THIẾU
            _navigationService = App.Services.GetRequiredService<INavigationService>();
        }


        private void ProductList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Product product)
            {
                _navigationService.NavigateTo(typeof(DetailItemPage), product);
            }
        }

    }
}