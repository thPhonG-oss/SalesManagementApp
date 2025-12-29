// FE/SalesManagement.WinUI/ViewModels/ProductViewModel.cs
// Cập nhật để sử dụng Settings

using CommunityToolkit.Mvvm.Input;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.ObjectModel;

namespace SalesManagement.WinUI.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly INavigationService _navigationService;
        private readonly IStorageService _storageService; // ⭐ THÊM MỚI

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

        // ⭐ THÊM MỚI - Dynamic Page Size
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

        // ===== CTOR =====
        public ProductViewModel(
            ICategoryService categoryService,
            IProductService productService,
            INavigationService navigationService,
            IStorageService storageService) // ⭐ INJECT MỚI
        {
            _categoryService = categoryService;
            _productService = productService;
            _navigationService = navigationService;
            _storageService = storageService; // ⭐ LƯU

            PrevPageCommand = new RelayCommand(
                () => Page--,
                () => Page > 1);

            NextPageCommand = new RelayCommand(
                () => Page++,
                () => Page < TotalPages);

            OpenAddProductCommand = new RelayCommand(OpenAddProduct);

            _ = InitializeAsync(); // ⭐ THAY THẾ
        }

        // ⭐ THÊM MỚI - Initialize với Settings
        private async Task InitializeAsync()
        {
            // Load settings trước
            var settings = await _storageService.GetAppSettingsAsync();
            PageSize = settings.ItemsPerPage;

            // Sau đó load data
            await LoadCategoriesAsync();
            await LoadProductsAsync();
        }

        // ===== OPEN ADD PRODUCT PAGE =====
        private void OpenAddProduct()
        {
            _navigationService.NavigateTo(typeof(Views.AddProductPage));
        }

        // ===== LOAD CATEGORY =====
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

        // ===== LOAD PRODUCT =====
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
                }
            }

            Page = 1;
            ApplyFilterAndPaging();
        }

        // ===== FILTER + PAGING =====
        private void ApplyFilterAndPaging()
        {
            IEnumerable<Product> query = _allProducts;

            // Filter by category
            if (SelectedCategory != null && SelectedCategory.CategoryId != -1)
                query = query.Where(p => p.Category?.CategoryId == SelectedCategory.CategoryId);

            // Filter by search
            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(p =>
                    p.ProductName != null &&
                    p.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            // Filter by price
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

            // ⭐ SỬ DỤNG PageSize ĐỘNG
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
    }
}