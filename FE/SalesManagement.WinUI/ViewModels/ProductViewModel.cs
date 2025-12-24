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
        private readonly INavigationService _navigationService; // ⭐ THÊM

        // ================= CATEGORY =================
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

        // ================= PRODUCT =================
        private readonly ObservableCollection<Product> _allProducts = new();
        public ObservableCollection<Product> Products { get; } = new();

        // ================= SEARCH =================
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

        // ================= PAGINATION =================
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

        private const int _pageSize = 20;

        // ================= COMMAND =================
        public RelayCommand NextPageCommand { get; }
        public RelayCommand PrevPageCommand { get; }

        public RelayCommand OpenAddProductCommand { get; }   // ⭐ THÊM

        // ================= CTOR =================
        public ProductViewModel(
            ICategoryService categoryService,
            IProductService productService,
            INavigationService navigationService)   // ⭐ THÊM
        {
            _categoryService = categoryService;
            _productService = productService;
            _navigationService = navigationService; // ⭐ THÊM

            PrevPageCommand = new RelayCommand(
                () => Page--,
                () => Page > 1);

            NextPageCommand = new RelayCommand(
                () => Page++,
                () => Page < TotalPages);

            OpenAddProductCommand = new RelayCommand(OpenAddProduct); // ⭐ THÊM

            _ = LoadCategoriesAsync();
            _ = LoadProductsAsync();
        }

        // ================= MỞ TRANG THÊM SẢN PHẨM =================
        private void OpenAddProduct()
        {
            _navigationService.SetHeader("Thêm sản phẩm");
            //_navigationService.NavigateTo(typeof(Views.AddProductPage));
        }

        // ================= LOAD CATEGORY =================
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

        // ================= LOAD PRODUCT (1 LẦN) =================
        private async Task LoadProductsAsync()
        {
            var result = await _productService.GetProductsAsync();
            if (result == null) return;

            _allProducts.Clear();

            foreach (var product in result.Products)
                _allProducts.Add(product);

            Page = 1;
            ApplyFilterAndPaging();
        }

        // ================= FILTER + PAGING =================
        private void ApplyFilterAndPaging()
        {
            IEnumerable<Product> query = _allProducts;

            if (SelectedCategory != null && SelectedCategory.CategoryId != -1)
                query = query.Where(p => p.Category?.CategoryId == SelectedCategory.CategoryId);

            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(p =>
                    p.ProductName != null &&
                    p.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            var count = query.Count();
            TotalPages = Math.Max(1, (int)Math.Ceiling(count / (double)_pageSize));

            if (Page > TotalPages)
                Page = 1;

            Products.Clear();
            foreach (var item in query
                .Skip((Page - 1) * _pageSize)
                .Take(_pageSize))
                Products.Add(item);
        }

        private void UpdatePagingCommandState()
        {
            PrevPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
        }
    }
}
