using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SalesManagement.WinUI.ViewModels
{
    public partial class CategoryManagementViewModel : ObservableObject
    {
        private readonly ICategoryService _categoryService;

        [ObservableProperty]
        private ObservableCollection<Category> categories = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private Category? selectedCategory;


        public CategoryManagementViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Load categories khi trang được mở
        public async Task LoadCategoriesAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                Debug.WriteLine("[CategoryManagementViewModel] Loading categories...");

                var result = await _categoryService.GetAllAsync();

                Categories.Clear();
                int i = 0;
                foreach (var category in result)
                {
                    Categories.Add(category);
                    Debug.WriteLine($"[CategoryManagementViewModel] Loaded Category: {Categories[i]}");
                    i++;
                }

                Debug.WriteLine($"[CategoryManagementViewModel] Loaded {Categories.Count} categories");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Không thể tải danh mục: {ex.Message}";
                Debug.WriteLine($"[CategoryManagementViewModel] LoadCategoriesAsync Error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadCategoriesAsync();
        }

        [RelayCommand]
        private void AddCategory()
        {
            // TODO: Mở dialog thêm category
            Debug.WriteLine("[CategoryManagementViewModel] Add category clicked");
        }

        [RelayCommand]
        private void EditCategory(Category? category)
        {
            if (category == null) return;

            // TODO: Mở dialog sửa category
            Debug.WriteLine($"[CategoryManagementViewModel] Edit category: {category.CategoryName}");
        }

        [RelayCommand]
        private async Task DeleteCategoryAsync(Category? category)
        {
            if (category == null) return;

            // TODO: Implement delete logic
            Debug.WriteLine($"[CategoryManagementViewModel] Delete category: {category.CategoryName}");
        }

        // 🚀 Thêm mới category qua API
        public async Task<bool> AddCategoryAsync(CreateCategoryRequest request)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                Debug.WriteLine($"[CategoryManagementViewModel] Creating Category: {request.CategoryName}");

                var result = await _categoryService.CreateAsync(request);

                if (result)
                {
                    Debug.WriteLine("[CategoryManagementViewModel] Create Success → Reload list");
                    await LoadCategoriesAsync();
                    return true;
                }
                else
                {
                    Debug.WriteLine("[CategoryManagementViewModel] Create Failed");
                    ErrorMessage = "Không thể thêm danh mục.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi thêm danh mục: {ex.Message}";
                Debug.WriteLine($"[CategoryManagementViewModel] AddCategoryAsync Error: {ex.Message}");
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryRequest request)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var result = await _categoryService.UpdateAsync(id, request);

                if (result)
                {
                    Debug.WriteLine("[CategoryManagementViewModel] Update Success → Reload list");
                    await LoadCategoriesAsync();
                    return true;
                }
                else
                {
                    ErrorMessage = "Không thể cập nhật danh mục.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi cập nhật: {ex.Message}";
                Debug.WriteLine($"[CategoryManagementViewModel] UpdateCategoryAsync Error: {ex.Message}");
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

    }
}