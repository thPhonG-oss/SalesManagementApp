using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.ViewModels;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class CategoryManagementPage : Page
    {
        public CategoryManagementViewModel ViewModel { get; }

        public CategoryManagementPage()
        {
            this.InitializeComponent();

            // Inject ViewModel từ DI Container
            ViewModel = App.Services.GetRequiredService<CategoryManagementViewModel>();
            DataContext = ViewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (ViewModel != null)
                await ViewModel.LoadCategoriesAsync();
        }

        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            await AddCategoryDialog.ShowAsync();
        }

        private async void AddCategoryDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string name = CategoryNameTextBox.Text.Trim();
            string desc = DescriptionTextBox.Text.Trim();
            bool isActive = IsActiveToggle.IsOn;

            var request = new CreateCategoryRequest
            {
                CategoryName = name,
                Description = desc,
                IsActive = isActive
            };

            bool success = await ViewModel.AddCategoryAsync(request);

            if (success)
            {
                await new ContentDialog
                {
                    Title = "Thành công",
                    Content = "Đã thêm danh mục mới.",
                    CloseButtonText = "Đóng",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();

                await ViewModel.LoadCategoriesAsync();
            }
            else
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = ViewModel.ErrorMessage ?? "Không thể thêm danh mục.",
                    CloseButtonText = "Đóng",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }

            // Sau khi thêm xong, refresh danh sách:
            //await ViewModel.LoadCategoriesAsync();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private Category? _editingCategory;

        private async void EditCategoryDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (_editingCategory == null) return;

            var request = new UpdateCategoryRequest
            {
                CategoryName = EditCategoryNameTextBox.Text.Trim(),
                Description = EditDescriptionTextBox.Text.Trim(),
                //IsActive = EditIsActiveToggle.IsOn
                IsActive = true
            };

            bool success = await ViewModel.UpdateCategoryAsync(_editingCategory.CategoryId, request);

            if (success)
            {
                await new ContentDialog
                {
                    Title = "Thành công",
                    Content = "Đã cập nhật danh mục.",
                    CloseButtonText = "Đóng",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
            else
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = ViewModel.ErrorMessage ?? "Không thể cập nhật danh mục.",
                    CloseButtonText = "Đóng",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }

        private async void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Category category)
            {
                _editingCategory = category;

                EditCategoryNameTextBox.Text = category.CategoryName;
                EditDescriptionTextBox.Text = category.Description;
                //EditIsActiveToggle.IsOn = category.IsActive;

                await EditCategoryDialog.ShowAsync();
            }
        }

        private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Category category)
            {
                var confirmDialog = new ContentDialog
                {
                    Title = "Xác nhận xóa",
                    Content = $"Bạn có chắc chắn muốn xóa danh mục '{category.CategoryName}' không?",
                    PrimaryButtonText = "Xóa",
                    CloseButtonText = "Hủy",
                    XamlRoot = this.XamlRoot
                };
                var result = await confirmDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    bool success = await ViewModel.DeleteCategoryAsync(category.CategoryId);
                    if (success)
                    {
                        await new ContentDialog
                        {
                            Title = "Thành công",
                            Content = "Đã xóa danh mục.",
                            CloseButtonText = "Đóng",
                            XamlRoot = this.XamlRoot
                        }.ShowAsync();
                    }
                    else
                    {
                        await new ContentDialog
                        {
                            Title = "Lỗi",
                            Content = ViewModel.ErrorMessage ?? "Không thể xóa danh mục.",
                            CloseButtonText = "Đóng",
                            XamlRoot = this.XamlRoot
                        }.ShowAsync();
                    }
                }
            }
        }
    }
}
