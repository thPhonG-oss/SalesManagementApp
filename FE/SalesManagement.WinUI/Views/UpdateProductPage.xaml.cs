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

            ContentDialog dialog;

            try
            {
                var success = await productService.UpdateProductAsync(Product.ProductId, Product);

                if (success)
                {
                    dialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Thành công",
                        Content = "Sản phẩm đã được cập nhật thành công.",
                        PrimaryButtonText = "OK"
                    };

                    var result = await dialog.ShowAsync();

                    // 🔥 CHỈ navigate sau khi user bấm OK
                    if (result == ContentDialogResult.Primary)
                    {
                        _navigationService.NavigateTo(typeof(ProductPage));
                    }
                }
                else
                {
                    dialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Thất bại",
                        Content = "Không thể cập nhật sản phẩm. Vui lòng thử lại.",
                        PrimaryButtonText = "OK"
                    };

                    await dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Update error: {ex.Message}");

                dialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Lỗi",
                    Content = "Đã xảy ra lỗi khi cập nhật sản phẩm.",
                    PrimaryButtonText = "OK"
                };

                await dialog.ShowAsync();
            }
        }


    }
}
