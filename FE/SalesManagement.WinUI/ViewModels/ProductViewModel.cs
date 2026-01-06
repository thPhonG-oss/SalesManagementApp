using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage.Pickers;

namespace SalesManagement.WinUI.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly INavigationService _navigationService;
        private readonly IStorageService _storageService;

        // ===== CATEGORY =====
        public ObservableCollection<Category> Categories { get; } = new();

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    Page = 1;
                    ApplyFilterAndPaging();
                }
            }
        }

        // ===== PRICE FILTER =====
        private string? _selectedPriceFilter = "0-999999999";
        public string? SelectedPriceFilter
        {
            get => _selectedPriceFilter;
            set
            {
                if (SetProperty(ref _selectedPriceFilter, value))
                {
                    Page = 1;
                    ApplyFilterAndPaging();
                }
            }
        }

        // ===== PRODUCT =====
        private readonly ObservableCollection<Product> _allProducts = new();
        public ObservableCollection<Product> Products { get; } = new();

        // ===== SEARCH =====
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    Page = 1;
                    ApplyFilterAndPaging();
                }
            }
        }

        // ===== PAGINATION =====
        private int _page = 1;
        public int Page
        {
            get => _page;
            set
            {
                if (SetProperty(ref _page, value))
                {
                    ApplyFilterAndPaging();
                    UpdatePagingCommandState();
                }
            }
        }

        private int _totalPages = 1;
        public int TotalPages
        {
            get => _totalPages;
            set
            {
                if (SetProperty(ref _totalPages, value))
                {
                    UpdatePagingCommandState();
                }
            }
        }

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => SetProperty(ref _pageSize, value);
        }

        // ===== COMMAND =====
        public RelayCommand NextPageCommand { get; }
        public RelayCommand PrevPageCommand { get; }
        public RelayCommand OpenAddProductCommand { get; }
        public IAsyncRelayCommand OpenCategoryDialogCommand { get; }
        public IAsyncRelayCommand ImportProductsCommand { get; } // ⭐ NEW

        // ===== CTOR =====
        public ProductViewModel(
            ICategoryService categoryService,
            IProductService productService,
            INavigationService navigationService,
            IStorageService storageService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _navigationService = navigationService;
            _storageService = storageService;

            PrevPageCommand = new RelayCommand(
                () => Page--,
                () => Page > 1);

            NextPageCommand = new RelayCommand(
                () => Page++,
                () => Page < TotalPages);

            OpenAddProductCommand = new RelayCommand(OpenAddProduct);
            OpenCategoryDialogCommand = new AsyncRelayCommand(OpenCategoryDialogAsync);
            ImportProductsCommand = new AsyncRelayCommand(ImportProductsAsync); // ⭐ NEW

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var settings = await _storageService.GetAppSettingsAsync();
            PageSize = settings.ItemsPerPage;

            await LoadCategoriesAsync();
            await LoadProductsAsync();
        }

        private void OpenAddProduct()
        {
            _navigationService.NavigateTo(typeof(Views.AddProductPage));
        }

        private async Task LoadCategoriesAsync()
        {
            var data = await _categoryService.GetAllAsync();
            if (data == null) return;

            Categories.Clear();

            Categories.Add(new Category
            {
                CategoryId = -1,
                CategoryName = "Tất cả"
            });

            foreach (var item in data)
                Categories.Add(item);

            SelectedCategory = Categories.First();
        }

        private async Task LoadProductsAsync()
        {
            var result = await _productService.GetProductsAsync();
            if (result == null) return;

            _allProducts.Clear();

            foreach (var product in result.Products)
            {
                if (product.IsActive)
                {
                    _allProducts.Add(product);
                    Debug.WriteLine("------ " + product.SpecialPriceText);
                }
            }

            Page = 1;
            ApplyFilterAndPaging();
        }

        private void ApplyFilterAndPaging()
        {
            IEnumerable<Product> query = _allProducts;

            if (SelectedCategory != null && SelectedCategory.CategoryId != -1)
                query = query.Where(p => p.Category?.CategoryId == SelectedCategory.CategoryId);

            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(p =>
                    p.ProductName != null &&
                    p.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(SelectedPriceFilter))
            {
                var parts = SelectedPriceFilter.Split('-');
                if (parts.Length == 2 &&
                    decimal.TryParse(parts[0], out var minP) &&
                    decimal.TryParse(parts[1], out var maxP))
                {
                    query = query.Where(p => p.Price >= minP && p.Price <= maxP);
                }
            }

            var count = query.Count();
            TotalPages = Math.Max(1, (int)Math.Ceiling(count / (double)PageSize));

            if (Page > TotalPages)
                Page = 1;

            Products.Clear();
            foreach (var item in query
                .Skip((Page - 1) * PageSize)
                .Take(PageSize))
                Products.Add(item);
        }

        private void UpdatePagingCommandState()
        {
            PrevPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
        }

        private async Task OpenCategoryDialogAsync()
        {
            try
            {
                var dialog = new ContentDialog
                {
                    Title = "📂 Quản lý danh mục",
                    PrimaryButtonText = "Đóng",
                    IsPrimaryButtonEnabled = true,
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = App.MainWindow.Content.XamlRoot,
                    Content = new Views.CategoryManagementPage()
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Không thể mở trang quản lý danh mục: {ex.Message}",
                    CloseButtonText = "Đóng",
                    XamlRoot = App.MainWindow.Content.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }

        // ⭐ NEW - Import Products from Excel
        private async Task ImportProductsAsync()
        {
            try
            {
                var picker = new FileOpenPicker();

                // Khởi tạo Window handle cho WinUI 3
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                picker.FileTypeFilter.Add(".xlsx");
                picker.FileTypeFilter.Add(".xls");
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

                var file = await picker.PickSingleFileAsync();
                if (file == null) return;

                // Show loading dialog
                var loadingDialog = new ContentDialog
                {
                    Title = "Đang xử lý...",
                    Content = "Đang import sản phẩm từ file Excel, vui lòng đợi...",
                    XamlRoot = App.MainWindow.Content.XamlRoot
                };

                _ = loadingDialog.ShowAsync();

                var success = await _productService.ImportProductsFromExcelAsync(file);

                loadingDialog.Hide();

                if (success)
                {
                    var successDialog = new ContentDialog
                    {
                        Title = "Thành công",
                        Content = "Import sản phẩm thành công!",
                        CloseButtonText = "Đóng",
                        XamlRoot = App.MainWindow.Content.XamlRoot
                    };
                    await successDialog.ShowAsync();

                    // Reload products
                    await LoadProductsAsync();
                }
                else
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Import sản phẩm thất bại. Vui lòng kiểm tra lại file Excel.",
                        CloseButtonText = "Đóng",
                        XamlRoot = App.MainWindow.Content.XamlRoot
                    };
                    await errorDialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Có lỗi xảy ra: {ex.Message}",
                    CloseButtonText = "Đóng",
                    XamlRoot = App.MainWindow.Content.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }
    }
}