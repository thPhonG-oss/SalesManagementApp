using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SalesManagement.WinUI.Models;
using System.Diagnostics;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class DetailItemPage : Page
    {
        public Product Product { get; private set; }

        public DetailItemPage()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Product product)
            {
                Product = product;
                Debug.WriteLine("Danh mục " + Product.Category.CategoryName);
                DataContext = Product;
            }
        }


        private void Back_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void Delete_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void Update_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
