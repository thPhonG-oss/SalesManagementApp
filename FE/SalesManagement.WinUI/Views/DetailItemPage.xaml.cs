using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Storage.Pickers;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using System.Diagnostics;
using Windows.Storage;


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

        private async void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 🔹 Tạo FileOpenPicker tương thích WinUI 3
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
                var picker = new FileOpenPicker(windowId: Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd))
                {
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                    ViewMode = PickerViewMode.Thumbnail
                };

                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                // ⚠️ WinAppSDK 1.5+: trả về PickFileResult, không phải StorageFile
                var pickResult = await picker.PickSingleFileAsync();
                if (pickResult == null)
                    return;

                // ✅ Lấy StorageFile từ PickFileResult
                var file = await StorageFile.GetFileFromPathAsync(pickResult.Path);

                // ✅ Hiển thị ảnh xem trước
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(stream);

                    if (FindName("ProductImage") is Image imageControl)
                    {
                        imageControl.Source = bitmap;
                    }
                }

                // ✅ Gọi API upload multipart/form-data
                var productService = App.Services.GetService<IProductService>();
                if (productService != null && Product != null)
                {
                    bool success = await productService.UploadImageAsync(Product.ProductId, file);

                    var dialog = new ContentDialog
                    {
                        Title = success ? "Thành công" : "Lỗi",
                        Content = success
                            ? "Ảnh sản phẩm đã được cập nhật."
                            : "Không thể upload ảnh.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ChangeImage_Click error: {ex.Message}");
            }
        }



    }
}
