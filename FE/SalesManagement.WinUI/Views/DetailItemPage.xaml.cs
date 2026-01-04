using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;


namespace SalesManagement.WinUI.Views
{
    public sealed partial class DetailItemPage : Page
    {
        public Product Product { get; private set; }
        public DetailItemViewModel ViewModel { get; private set; }
        private readonly INavigationService _navigationService;
        public DetailItemPage()
        {
            this.InitializeComponent();
            _navigationService = App.Services.GetRequiredService<INavigationService>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Product product)
            {
                Product = product;
                Debug.WriteLine("Danh mục: " + Product.Category?.CategoryName);
                DataContext = Product;
                Debug.WriteLine("Đã vào DetailItemPage " + Product.DisplayImages.First());
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Product == null)
                return;

            var dialog = new ContentDialog
            {
                Title = "Xác nhận xóa",
                Content = $"Bạn có chắc muốn xóa sản phẩm \"{Product.ProductName}\" (ID: {Product.ProductId}) không?",
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // Lấy service từ App.Services
                var productService = App.Services.GetService<IProductService>();
                if (productService == null)
                    return;

                Debug.WriteLine("ID cần xóa" + Product.ProductId);

                var success = await productService.DeleteProductAsync(Product.ProductId);

                if (success)
                {
                    var successDialog = new ContentDialog
                    {
                        Title = "Thành công",
                        Content = $"Đã xóa sản phẩm ID {Product.ProductId}.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await successDialog.ShowAsync();

                    if (Frame.CanGoBack)
                        Frame.GoBack();
                }
                else
                {
                    var failDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Không thể xóa sản phẩm.",
                        CloseButtonText = "Đóng",
                        XamlRoot = this.XamlRoot
                    };
                    await failDialog.ShowAsync();
                }
            }
        }


        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (Product != null)
            {
                _navigationService.NavigateTo(typeof(UpdateProductPage), Product);

            }
        }

        private async void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not Product product)
                return;

            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile file = await picker.PickSingleFileAsync();
            if (file == null) return;

            var productService = App.Services.GetService<IProductService>();
            bool success = await productService.UploadImageAsync(product.ProductId, file);

            await new ContentDialog
            {
                Title = success ? "Thành công" : "Lỗi",
                Content = success ? "Upload ảnh thành công" : "Upload ảnh thất bại",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            }.ShowAsync();
        }


    }
}
