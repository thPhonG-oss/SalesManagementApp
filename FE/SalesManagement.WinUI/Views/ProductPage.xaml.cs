using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SalesManagement.WinUI.Views
{
    public sealed partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            DataContext = App.Services.GetService<ProductViewModel>();
        }
    }
}