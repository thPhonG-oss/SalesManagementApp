using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.ObjectModel;

namespace SalesManagement.WinUI.ViewModels
{
    public class ProductViewModel : BaseViewModel

    {
        private readonly ICategoryService _categoryService;

        public ObservableCollection<Category> Categories { get; }
            = new ObservableCollection<Category>();

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public ProductViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            var data = await _categoryService.GetAllAsync();


            System.Diagnostics.Debug.WriteLine($"[VM] Categories = {data.Count}");

            Categories.Clear();
            foreach (var item in data)
            {
                Categories.Add(item);
            }
        }
    }
}
