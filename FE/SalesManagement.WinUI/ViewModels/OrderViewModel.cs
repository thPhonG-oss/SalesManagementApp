using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;

namespace SalesManagement.WinUI.ViewModels
{
    // 1. VM con cho từng dòng sản phẩm trong Dialog
    public class DetailItemViewModel
    {
        private readonly OrderDetail _model;

        public DetailItemViewModel(OrderDetail model)
        {
            _model = model;
        }

        public string ProductName => _model.ProductName;
        public int Quantity => _model.Quantity;
        public string PriceDisplay => _model.Price.ToString("N0");
        public string TotalDisplay => (_model.Quantity * _model.Price).ToString("N0");
    }

    // 2. VM cho một Đơn hàng (Dùng chung cho List và Dialog)
    public partial class OrderItemViewModel : ObservableObject
    {
        public Order Model { get; }

        // --- SỬA LỖI TẠI ĐÂY ---
        // Chỉ khai báo duy nhất 1 lần. Khởi tạo rỗng để chờ API trả về.
        public ObservableCollection<DetailItemViewModel> OrderItems { get; } = new();

        public OrderItemViewModel(Order model)
        {
            Model = model;
            // Lưu ý: Không nạp OrderItems ở đây nữa vì API danh sách chưa có thông tin chi tiết
        }

        // Hàm này sẽ được gọi từ Main ViewModel sau khi API 2 chạy xong
        public void LoadDetails(List<OrderDetail> details)
        {
            OrderItems.Clear();
            if (details != null)
            {
                foreach (var item in details)
                {
                    OrderItems.Add(new DetailItemViewModel(item));
                }
            }
        }

        // Các thuộc tính hiển thị (Binding ra View)
        public string OrderId => Model.OrderId;
        public string DateDisplay => Model.Date.ToString("dd/MM/yyyy");
        public string ItemsCountDisplay => $"{Model.ItemsCount} mặt hàng"; // Lấy từ số tổng hợp
        public string AmountDisplay => Model.Amount.ToString("N0");       // Lấy từ số tổng hợp
        public string Status => Model.Status;
        public string DialogTitle => $"Chi tiết đơn hàng {Model.OrderId}";

        // Logic màu sắc
        public Brush StatusColor
        {
            get
            {
                if (Status == "Đã thanh toán") return new SolidColorBrush(Color.FromArgb(40, 34, 197, 94));
                if (Status == "Đã hủy") return new SolidColorBrush(Color.FromArgb(40, 239, 68, 68));
                return new SolidColorBrush(Color.FromArgb(40, 234, 179, 8));
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

    // 3. Main ViewModel (OrderViewModel) - Giữ nguyên logic chính, chỉ cần cập nhật cách tính Stats
    public partial class OrderViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        private List<OrderItemViewModel> _allOrders = new();
        private const int _pageSize = 7;

        public event Action<OrderItemViewModel>? OpenDetailRequested;

        [ObservableProperty] private ObservableCollection<OrderItemViewModel> _displayOrders = new();
        [ObservableProperty] private int _currentPage = 1;
        [ObservableProperty] private string _paginationStatusText = string.Empty;
        [ObservableProperty] private bool _canGoNext;
        [ObservableProperty] private bool _canGoPrevious;
        [ObservableProperty] private OrderItemViewModel? _selectedOrder;
        [ObservableProperty] private bool _isLoading;

        // Stats
        [ObservableProperty] private int _totalOrdersCount;
        [ObservableProperty] private string _totalRevenueDisplay = "0 đ";
        [ObservableProperty] private int _pendingOrdersCount;

        public OrderViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            LoadDataAsync();
        }

        async partial void OnSelectedOrderChanged(OrderItemViewModel? value)
        {
            if (value != null)
            {
                try
                {
                    // 1. (Optional) Hiển thị trạng thái Loading nhỏ nếu cần
                    IsLoading = true;

                    // 2. GỌI API 2: Lấy chi tiết đơn hàng theo ID
                    var details = await _orderService.GetOrderDetailsAsync(value.OrderId);

                    // 3. Đổ dữ liệu vào ViewModel của item đó
                    value.LoadDetails(details);

                    // 4. Bắn sự kiện mở Dialog sau khi đã có dữ liệu
                    OpenDetailRequested?.Invoke(value);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi tải chi tiết: {ex.Message}");
                }
                finally
                {
                    IsLoading = false;
                    // Reset selected item để có thể chọn lại
                    DispatcherQueue.GetForCurrentThread().TryEnqueue(() => { SelectedOrder = null; });
                }
            }
        }

        private async void LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                var data = await _orderService.GetOrdersAsync();
                _allOrders = data.Select(x => new OrderItemViewModel(x)).ToList();
                CalculateStats();
                UpdatePagination();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CalculateStats()
        {
            TotalOrdersCount = _allOrders.Count;
            // Tính tổng tiền dựa trên Model mới
            decimal totalRevenue = _allOrders.Sum(x => x.Model.Amount);
            TotalRevenueDisplay = totalRevenue.ToString("N0") + " đ";
            PendingOrdersCount = _allOrders.Count(x => x.Status == "Mới tạo" || x.Status == "Đang xử lý");
        }

        // ... Các hàm Pagination (NextPage, PreviousPage, UpdatePagination) giữ nguyên như cũ ...
        private void UpdatePagination()
        {
            int totalPages = (int)Math.Ceiling((double)_allOrders.Count / _pageSize);
            if (CurrentPage < 1) CurrentPage = 1;
            if (CurrentPage > totalPages && totalPages > 0) CurrentPage = totalPages;

            var pageData = _allOrders.Skip((CurrentPage - 1) * _pageSize).Take(_pageSize).ToList();
            DisplayOrders.Clear();
            foreach (var item in pageData) DisplayOrders.Add(item);

            int startCount = _allOrders.Count == 0 ? 0 : (CurrentPage - 1) * _pageSize + 1;
            int endCount = Math.Min(CurrentPage * _pageSize, _allOrders.Count);
            PaginationStatusText = $"Hiển thị {startCount} - {endCount} trong {_allOrders.Count} đơn hàng";
            CanGoPrevious = CurrentPage > 1;
            CanGoNext = CurrentPage < totalPages;
        }

        [RelayCommand]
        private void NextPage() { if (CanGoNext) { CurrentPage++; UpdatePagination(); } }

        [RelayCommand]
        private void PreviousPage() { if (CanGoPrevious) { CurrentPage--; UpdatePagination(); } }
    }
}