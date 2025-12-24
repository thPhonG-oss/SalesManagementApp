using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SalesManagement.WinUI.ViewModels
{
    public partial class AddProductViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<Category> Categories { get; } = new();

        [ObservableProperty] private string productName = string.Empty;
        [ObservableProperty] private string description = string.Empty;
        [ObservableProperty] private double price;

        [ObservableProperty] private Category? selectedCategory;
        [ObservableProperty]
        private int selectedCategoryId = -1;

        [ObservableProperty] private string message = string.Empty;
        [ObservableProperty] private string messageTrue = string.Empty;
        // THÊM MỚI ĐỂ KHỚP CreateProductRequest
        [ObservableProperty] private string author = string.Empty;
        [ObservableProperty] private string publisher = string.Empty;
        [ObservableProperty] private int publicationYear;
        [ObservableProperty] private int stockQuantity;
        [ObservableProperty] private int minStockQuantity;

        public IRelayCommand CreateProductCommand { get; }

        public AddProductViewModel(
            IProductService productService,
            ICategoryService categoryService,
            INavigationService navigationService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _navigationService = navigationService;

            CreateProductCommand = new AsyncRelayCommand(CreateProductAsync);

            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            var data = await _categoryService.GetAllAsync();
            Debug.WriteLine($"Loaded {data} categories.");

            Categories.Clear();

            // Mục mặc định
            Categories.Add(new Category
            {
                CategoryId = -1,
                CategoryName = "— Chọn danh mục —"
            });

            if (data != null)
            {
                foreach (var item in data)
                    Categories.Add(item);
            }

            SelectedCategory = Categories.First();
        }

        private async Task CreateProductAsync()
        {

            if (string.IsNullOrWhiteSpace(ProductName))
            {
                Message = "Vui lòng nhập tên sản phẩm!";
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                Message = "Vui lòng nhập mô tả sản phẩm!";
                return;
            }

            if (Price == null || Price <= 0)
            {
                Message = "Giá sản phẩm phải lớn hơn 0!";
                return;
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                Message = "Vui lòng nhập tên tác giả!";
                return;
            }

            if (string.IsNullOrWhiteSpace(publisher))
            {
                Message = "Vui lòng nhập nhà xuất bản!";
                return;
            }

            if (publicationYear == null || publicationYear <= 0)
            {
                Message = "Vui lòng nhập năm xuất bản hợp lệ!";
                return;
            }

            if (stockQuantity == null || stockQuantity < 0)
            {
                Message = "Số lượng tồn kho không hợp lệ!";
                return;
            }

            if (minStockQuantity == null || minStockQuantity < 0)
            {
                Message = "Số lượng tối thiểu không hợp lệ!";
                return;
            }

            if (SelectedCategoryId == -1)
            {
                Message = "Vui lòng chọn danh mục!";
                return;
            }


            var request = new CreateProductRequest
            {
                ProductName = ProductName,
                Description = Description,
                Price = (decimal)Price,
                CategoryId = SelectedCategoryId,
                Author = author,
                Publisher = publisher,
                PublicationYear = publicationYear,
                StockQuantity = stockQuantity,
                MinStockQuantity = minStockQuantity
            };


            bool ok = await _productService.CreateProductAsync(request);

            if (ok)
            {
                MessageTrue = "Thêm sản phẩm thành công!";
                await Task.Delay(800);
                _navigationService.GoBack();
            }
            else
            {
                Debug.WriteLine(ok);
                Message = "Tên sách bị trùng!";
            }
        }
    }
}
