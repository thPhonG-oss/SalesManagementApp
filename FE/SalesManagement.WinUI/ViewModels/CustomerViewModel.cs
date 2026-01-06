using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.Views.Components; // Để dùng Dialog nếu cần
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace SalesManagement.WinUI.ViewModels
{
    public partial class CustomerViewModel : ObservableObject
    {
        private readonly ICustomerService _customerService;

        [ObservableProperty] private ObservableCollection<Customer> _customers = new();
        [ObservableProperty] private Customer? _selectedCustomer;
        [ObservableProperty] private string _searchText = string.Empty;
        [ObservableProperty] private bool _isLoading;

        // Pagination
        [ObservableProperty] private int _page = 1;
        [ObservableProperty] private int _pageSize = 20;
        [ObservableProperty] private int _totalPages = 1;

        public CustomerViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            LoadDataCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            if (IsLoading) return;
            IsLoading = true;

            try
            {
                var data = await _customerService.GetCustomersAsync(Page, PageSize, SearchText);
                Customers.Clear();

                if (data != null && data.Customers != null)
                {
                    foreach (var item in data.Customers)
                    {
                        Customers.Add(item);
                    }
                    TotalPages = data.TotalPages > 0 ? data.TotalPages : 1;
                }
            }
            finally
            {
                IsLoading = false;
                NextPageCommand.NotifyCanExecuteChanged();
                PrevPageCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            Page = 1;
            await LoadDataAsync();
        }

        [RelayCommand(CanExecute = nameof(CanGoNext))]
        private async Task NextPageAsync()
        {
            if (Page < TotalPages)
            {
                Page++;
                await LoadDataAsync();
            }
        }

        [RelayCommand(CanExecute = nameof(CanGoPrev))]
        private async Task PrevPageAsync()
        {
            if (Page > 1)
            {
                Page--;
                await LoadDataAsync();
            }
        }

        private bool CanGoNext() => Page < TotalPages;
        private bool CanGoPrev() => Page > 1;

        [RelayCommand]
        private async Task AddCustomerAsync()
        {
            // Tạo object mới
            var newCustomer = new Customer();

            // Mở Dialog
            var dialog = new SalesManagement.WinUI.Views.Components.CustomerDialog(newCustomer, isEdit: false);

            // Bắt buộc gán XamlRoot cho WinUI 3
            dialog.XamlRoot = App.MainWindow.Content.XamlRoot;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // Gọi Service tạo mới
                var success = await _customerService.CreateCustomerAsync(newCustomer);
                if (success)
                {
                    // Tải lại dữ liệu trang 1
                    Page = 1;
                    await LoadDataAsync();
                }
            }
        }

        [RelayCommand]
        private async Task EditCustomerAsync(Customer customer)
        {
            if (customer == null) return;

            // QUAN TRỌNG: Clone (Tạo bản sao) object để sửa. 
            // Nếu không clone, khi gõ phím trên Dialog, dữ liệu ở bảng bên dưới cũng đổi theo ngay lập tức (Binding).
            var cloneCustomer = new Customer
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address
            };

            var dialog = new SalesManagement.WinUI.Views.Components.CustomerDialog(cloneCustomer, isEdit: true);
            dialog.XamlRoot = App.MainWindow.Content.XamlRoot;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // Gọi Service cập nhật theo ID
                var success = await _customerService.UpdateCustomerAsync(cloneCustomer.CustomerId, cloneCustomer);
                if (success)
                {
                    await LoadDataAsync(); // Tải lại dữ liệu để thấy thay đổi
                }
            }
        }

        [RelayCommand]
        private async Task DeleteCustomerAsync(Customer customer)
        {
            if (customer == null) return;

            // Hiện hộp thoại xác nhận
            var confirmDialog = new ContentDialog
            {
                Title = "Xác nhận xóa",
                Content = $"Bạn có chắc chắn muốn xóa khách hàng: {customer.FullName}?",
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = App.MainWindow.Content.XamlRoot
            };

            var result = await confirmDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // Gọi Service xóa
                var success = await _customerService.DeleteCustomerAsync(customer.CustomerId);
                if (success)
                {
                    await LoadDataAsync();
                }
            }
        }

        // Trigger search khi text thay đổi
        partial void OnSearchTextChanged(string value)
        {
            // Có thể debounce nếu cần, ở đây gọi search luôn hoặc đợi user enter
            // Nếu muốn search real-time:
            // _ = SearchAsync();
        }
    }
}