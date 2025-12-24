using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Diagnostics;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class UpdateProductPage : Page
    {
        public Product Product { get; private set; }
        private readonly INavigationService _navigationService;
        public UpdateProductPage()
        {
            this.InitializeComponent();
            _navigationService = App.Services.GetRequiredService<INavigationService>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Product product)
            {
                // Gán dữ liệu để binding 2 chiều
                Product = product;
                var priceType = Product.PriceValue.GetType();

                Debug.WriteLine(Product.StockQuantity);
                Debug.WriteLine("Kiểu dữ liệu: " + priceType);

                DataContext = Product;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Product == null)
                return;

            var productService = App.Services.GetService<IProductService>();
            if (productService == null)
                return;

            var dialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                PrimaryButtonText = "OK"
            };

            try
            {
                var success = await productService.UpdateProductAsync(Product.ProductId, Product);

                if (success)
                {
                    dialog.Title = "Thành công";
                    dialog.Content = "Sản phẩm đã được cập nhật thành công.";

                    // Sai khi bấm oke dialog xong thì trở về trang danh sách
                    dialog.PrimaryButtonClick += (s, args) =>
                    {
                        _navigationService.NavigateTo(typeof(ProductPage));
                    };
                }
                else
                {
                    dialog.Title = "Thất bại";
                    dialog.Content = "Không thể cập nhật sản phẩm. Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Update error: {ex.Message}");
                dialog.Title = "Lỗi";
                dialog.Content = "Đã xảy ra lỗi khi cập nhật sản phẩm.";
            }

            await dialog.ShowAsync();
        }

    }
}
