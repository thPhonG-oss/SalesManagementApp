using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services;
using SalesManagement.WinUI.Services.Implementations;
using SalesManagement.WinUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Microsoft.UI.Dispatching;

namespace SalesManagement.WinUI.ViewModels
{
    // Wrapper cho từng Item để xử lý hiển thị (Màu sắc, Format tiền tệ)
    public partial class OrderItemViewModel : ObservableObject
    {
        public Order Model { get; }

        public OrderItemViewModel(Order model)
        {
            Model = model;
        }

        // Các thuộc tính hiển thị
        public string OrderId => Model.OrderId;
        public string DateDisplay => Model.Date.ToString("dd/MM/yyyy");
        public string ItemsCountDisplay => $"{Model.ItemsCount} mặt hàng";
        public string AmountDisplay => Model.Amount.ToString("N0"); // Format số: 1.000.000
        public string Status => Model.Status;

        // Logic màu sắc chuyển từ Model sang ViewModel
        public Brush StatusColor
        {
            get
            {
                if (Status == "Đã thanh toán") return new SolidColorBrush(Color.FromArgb(40, 34, 197, 94));
                if (Status == "Đã hủy") return new SolidColorBrush(Color.FromArgb(40, 239, 68, 68));
                return new SolidColorBrush(Color.FromArgb(40, 234, 179, 8)); // Đang xử lý
            }
        }

        public Brush StatusBorder
        {
            get
            {
                if (Status == "Đã thanh toán") return new SolidColorBrush(Color.FromArgb(255, 21, 128, 61));
                if (Status == "Đã hủy") return new SolidColorBrush(Color.FromArgb(255, 185, 28, 28));
                return new SolidColorBrush(Color.FromArgb(255, 161, 98, 7));
            }
        }
    }

    // ViewModel chính cho Trang
    public partial class OrderViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        

        private List<OrderItemViewModel> _allOrders = new();
        private const int _pageSize = 7;

        public event Action<OrderItemViewModel>? OpenDetailRequested;

        [ObservableProperty]
        private ObservableCollection<OrderItemViewModel> _displayOrders = new();

        [ObservableProperty]
        private int _currentPage = 1;

        [ObservableProperty]
        private string _paginationStatusText = string.Empty;

        [ObservableProperty]
        private bool _canGoNext;

        [ObservableProperty]
        private bool _canGoPrevious;

        [ObservableProperty]
        private OrderItemViewModel? _selectedOrder;

        [ObservableProperty]
        private bool _isLoading;

        // Thống kê (Stats)
        [ObservableProperty] private int _totalOrdersCount;
        [ObservableProperty] private string _totalRevenueDisplay = "0 đ";
        [ObservableProperty] private int _pendingOrdersCount;

        public OrderViewModel(IOrderService orderService)
        {
            _orderService = orderService; 
            LoadDataAsync();
        }

        partial void OnSelectedOrderChanged(OrderItemViewModel? value)
        {
            if (value != null)
            {
                // 1. Bắn sự kiện mở Dialog
                OpenDetailRequested?.Invoke(value);

                // 2. SỬA LỖI LÚC ĐƯỢC LÚC KHÔNG:
                // Thay vì gán null ngay, ta đưa nó vào hàng đợi của UI Thread.
                // Nó sẽ chờ UI vẽ xong rồi mới reset về null.
                DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
                {
                    SelectedOrder = null;
                });
            }
        }

        private async void LoadDataAsync()
        {
            IsLoading = true;

            try
            {
                // Gọi Service (Giả lập delay hoặc gọi thật)
                var data = await _orderService.GetOrdersAsync();

                _allOrders = data.Select(x => new OrderItemViewModel(x)).ToList();
                CalculateStats();
                UpdatePagination();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần (ví dụ hiển thị thông báo lỗi)
                System.Diagnostics.Debug.WriteLine("Lỗi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                // 3. Kết thúc Loading (Dù thành công hay lỗi đều phải chạy dòng này)
                IsLoading = false;
            }
        }

        private void CalculateStats()
        {
            TotalOrdersCount = _allOrders.Count;
            TotalRevenueDisplay = _allOrders.Sum(x => x.Model.Amount).ToString("N0") + " đ";
            PendingOrdersCount = _allOrders.Count(x => x.Status == "Đang xử lý");
        }

        private void UpdatePagination()
        {
            int totalPages = (int)Math.Ceiling((double)_allOrders.Count / _pageSize);

            // Guard clause
            if (CurrentPage < 1) CurrentPage = 1;
            if (CurrentPage > totalPages && totalPages > 0) CurrentPage = totalPages;

            var pageData = _allOrders
                .Skip((CurrentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            DisplayOrders.Clear();
            foreach (var item in pageData) DisplayOrders.Add(item);

            // Update UI State
            int startCount = _allOrders.Count == 0 ? 0 : (CurrentPage - 1) * _pageSize + 1;
            int endCount = Math.Min(CurrentPage * _pageSize, _allOrders.Count);

            PaginationStatusText = $"Hiển thị {startCount} - {endCount} trong {_allOrders.Count} đơn hàng";

            CanGoPrevious = CurrentPage > 1;
            CanGoNext = CurrentPage < totalPages;
        }

        [RelayCommand]
        private void NextPage()
        {
            if (CanGoNext)
            {
                CurrentPage++;
                UpdatePagination();
            }
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (CanGoPrevious)
            {
                CurrentPage--;
                UpdatePagination();
            }
        }

        [RelayCommand]
        private async Task DeleteOrder(OrderItemViewModel item)
        {
            if (item == null) return;

            // Trong MVVM, Dialog service nên được tách riêng, nhưng để đơn giản ta dùng XamlRoot được truyền vào hoặc access từ View (cách đơn giản nhất ở đây là giả định logic xóa thành công)

            // Hiển thị Dialog (Lưu ý: Trong Pure MVVM chuẩn, ViewModel không nên reference UI. 
            // Tuy nhiên với WinUI 3 đơn giản, ta có thể dùng IDialogService. Ở đây mình giả lập xóa luôn).

            _allOrders.Remove(item);
            CalculateStats(); 
            UpdatePagination(); 
        }


    }
}