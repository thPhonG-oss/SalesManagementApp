using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.Views.Components;
using System.Collections.ObjectModel;
using System.Threading;

namespace SalesManagement.WinUI.ViewModels
{
    public partial class CustomerViewModel : ObservableObject
    {
        private readonly ICustomerService _customerService;
        private List<Customer> _allCustomers = new(); // Cache toàn bộ dữ liệu
        private CancellationTokenSource? _searchCts;

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
                // Nếu có search text, thực hiện fuzzy search
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    await PerformFuzzySearchAsync();
                }
                else
                {
                    // Load dữ liệu bình thường từ API
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
            // Hủy search trước đó nếu đang chạy
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();

            try
            {
                // Debounce 300ms
                await Task.Delay(300, _searchCts.Token);

                Page = 1;
                await LoadDataAsync();
            }
            catch (TaskCanceledException)
            {
                // Search bị hủy, không làm gì
            }
        }

        private async Task PerformFuzzySearchAsync()
        {
            try
            {
                // Load toàn bộ dữ liệu nếu chưa có (hoặc có thể cache)
                if (_allCustomers.Count == 0)
                {
                    // Lấy tất cả customers (có thể cần điều chỉnh tùy API)
                    var allData = await _customerService.GetCustomersAsync(1, 10000, string.Empty);
                    if (allData?.Customers != null)
                    {
                        _allCustomers = allData.Customers.ToList();
                    }
                }

                // Thực hiện fuzzy search
                var searchTerm = SearchText.Trim().ToLower();
                var filteredCustomers = _allCustomers
                    .Select(c => new
                    {
                        Customer = c,
                        Score = CalculateFuzzyScore(c, searchTerm)
                    })
                    .Where(x => x.Score > 0)
                    .OrderByDescending(x => x.Score)
                    .Select(x => x.Customer)
                    .ToList();

                // Tính pagination
                TotalPages = (int)Math.Ceiling((double)filteredCustomers.Count / PageSize);
                if (TotalPages == 0) TotalPages = 1;

                // Lấy dữ liệu cho trang hiện tại
                var pagedCustomers = filteredCustomers
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                // Cập nhật UI
                Customers.Clear();
                foreach (var customer in pagedCustomers)
                {
                    Customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Search error: {ex.Message}");
            }
        }

        private int CalculateFuzzyScore(Customer customer, string searchTerm)
        {
            int score = 0;
            var fullName = customer.FullName?.ToLower() ?? string.Empty;

            // Exact match (điểm cao nhất)
            if (fullName.Contains(searchTerm)) score += 100;

            // Fuzzy match - kiểm tra từng ký tự của search term có trong chuỗi không
            score += CalculateLevenshteinScore(fullName, searchTerm) * 10;

            // Bonus cho match ở đầu chuỗi
            if (fullName.StartsWith(searchTerm)) score += 50;

            // Bonus cho match từng từ
            var searchWords = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var nameWords = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var searchWord in searchWords)
            {
                foreach (var nameWord in nameWords)
                {
                    if (nameWord.Contains(searchWord)) score += 20;
                    if (nameWord.StartsWith(searchWord)) score += 10;
                }
            }

            return score;
        }

        private int CalculateLevenshteinScore(string source, string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
                return 0;

            int distance = LevenshteinDistance(source, target);
            int maxLength = Math.Max(source.Length, target.Length);

            // Chuyển distance thành score (càng gần càng cao điểm)
            return Math.Max(0, maxLength - distance);
        }

        private int LevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
                return string.IsNullOrEmpty(target) ? 0 : target.Length;

            if (string.IsNullOrEmpty(target))
                return source.Length;

            int[,] distance = new int[source.Length + 1, target.Length + 1];

            for (int i = 0; i <= source.Length; i++)
                distance[i, 0] = i;

            for (int j = 0; j <= target.Length; j++)
                distance[0, j] = j;

            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(
                        Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost);
                }
            }

            return distance[source.Length, target.Length];
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
            var newCustomer = new Customer();
            var dialog = new CustomerDialog(newCustomer, isEdit: false);
            dialog.XamlRoot = App.MainWindow.Content.XamlRoot;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var success = await _customerService.CreateCustomerAsync(newCustomer);
                if (success)
                {
                    _allCustomers.Clear(); // Clear cache để reload
                    Page = 1;
                    await LoadDataAsync();
                }
            }
        }

        [RelayCommand]
        private async Task EditCustomerAsync(Customer customer)
        {
            if (customer == null) return;

            var cloneCustomer = new Customer
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address
            };

            var dialog = new CustomerDialog(cloneCustomer, isEdit: true);
            dialog.XamlRoot = App.MainWindow.Content.XamlRoot;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var success = await _customerService.UpdateCustomerAsync(cloneCustomer.CustomerId, cloneCustomer);
                if (success)
                {
                    _allCustomers.Clear(); // Clear cache để reload
                    await LoadDataAsync();
                }
            }
        }

        [RelayCommand]
        private async Task DeleteCustomerAsync(Customer customer)
        {
            if (customer == null) return;

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
                var success = await _customerService.DeleteCustomerAsync(customer.CustomerId);
                if (success)
                {
                    _allCustomers.Clear(); // Clear cache để reload
                    await LoadDataAsync();
                }
            }
        }

        // Trigger search khi text thay đổi (real-time search với debounce)
        partial void OnSearchTextChanged(string value)
        {
            _ = SearchAsync();
        }
    }
}