using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services;
using SalesManagement.WinUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;

namespace SalesManagement.WinUI.ViewModels
{
   
    
    public class DetailItemViewModel : ObservableObject
    {
        private readonly OrderDetail _model;

        public DetailItemViewModel(OrderDetail model)
        {
            _model = model;
        }

        public string ProductName => _model.ProductName;
        public int Quantity
        {
            get => _model.Quantity;
            set
            {
                if (_model.Quantity != value)
                {
                    // Cập nhật giá trị vào Model
                    _model.Quantity = value;

                    // Thông báo cho UI biết Quantity đã đổi
                    OnPropertyChanged(nameof(Quantity));

                    // 3. Quan trọng: Thông báo cho UI tính lại Thành Tiền ngay lập tức
                    OnPropertyChanged(nameof(TotalDisplay));
                }
            }
        }
        public string PriceDisplay => _model.Price.ToString("N0");
        public string TotalDisplay => (_model.Quantity * _model.Price).ToString("N0");
    }

    
    public partial class OrderItemViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        public Order Model { get; }


        private readonly Func<OrderItemViewModel, Task>? _editAction;
        private readonly Func<OrderItemViewModel, Task>? _deleteAction;

        
        public ObservableCollection<DetailItemViewModel> OrderItems { get; } = new();

        public OrderItemViewModel(Order model,
                                IOrderService orderService,
                                Func<OrderItemViewModel, Task>? editAction = null,
                                Func<OrderItemViewModel, Task>? deleteAction = null)
        {
            Model = model;
            _orderService = orderService;
            _editAction = editAction;
            _deleteAction = deleteAction;
          
        }

        
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

      
        public string OrderId => Model.Id.ToString();
        public string DateDisplay => Model.Date.ToString("dd/MM/yyyy");
        public string ItemsCountDisplay => $"{Model.ItemsCount} mặt hàng"; // Lấy từ số tổng hợp
        public string AmountDisplay => Model.Amount.ToString("N0");       // Lấy từ số tổng hợp
        public string Status
        {
            get => Model.Status;
            set
            {
                if (Model.Status != value)
                {
                    SetProperty(Model.Status, value, model => Model.Status = model, value);

                    Model.Status = value;
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(StatusColor)); 
                    OnPropertyChanged(nameof(StatusBorder)); 
                }
            }
        }
        public string DialogTitle => $"Chi tiết đơn hàng {Model.OrderCode}";

       
        public Brush StatusColor
        {
            get
            {
                if (Status == "Đã thanh toán") return new SolidColorBrush(Color.FromArgb(40, 34, 197, 94));
                if (Status == "Đã hủy") return new SolidColorBrush(Color.FromArgb(40, 239, 68, 68));
                return new SolidColorBrush(Color.FromArgb(40, 33, 150, 243));
            }
        }

        public Brush StatusBorder
        {
            get
            {
                if (Status == "Đã thanh toán") return new SolidColorBrush(Color.FromArgb(255, 21, 128, 61));
                if (Status == "Đã hủy") return new SolidColorBrush(Color.FromArgb(255, 185, 28, 28));
                return new SolidColorBrush(Color.FromArgb(255, 30, 136, 229));
            }
        }


        public DateTimeOffset DateOffset
        {
            get => new DateTimeOffset(Model.Date);
            set
            {
               
                if (Model.Date != value.DateTime)
                {
                    Model.Date = value.DateTime;

                    
                    OnPropertyChanged(nameof(DateOffset));
                    OnPropertyChanged(nameof(DateDisplay)); 
                }
            }
        }

        public double AmountDouble
        {
            get => (double)Model.Amount;
            set
            {
               
                var decimalValue = (decimal)value;

                if (Model.Amount != decimalValue)
                {
                    Model.Amount = decimalValue;

                    OnPropertyChanged(nameof(AmountDouble));
                    OnPropertyChanged(nameof(AmountDisplay)); 
                   
                }
            }
        }

        public async Task<bool> SaveAsync(double tempAmount, string tempStatus, DateTimeOffset tempDate)
        {
            try
            {
               
                this.AmountDouble = tempAmount;
                this.Status = tempStatus;
                this.DateOffset = tempDate;

               
                return await _orderService.UpdateOrderAsync(this.Model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        [RelayCommand]
        private async Task Edit()
        {
            if (_editAction != null) await _editAction(this);
        }

        [RelayCommand]
        private async Task Delete()
        {
            if (_deleteAction != null) await _deleteAction(this);
        }

        [RelayCommand]
        private async Task Print()
        {
            // Gọi API kích hoạt in
            bool success = await _orderService.PrintOrderAsync(OrderId);

            if (success)
            {
                // Tùy chọn: Hiện thông báo thành công (nếu App có InfoBar hoặc Toast)
                System.Diagnostics.Debug.WriteLine("Đã gửi lệnh in!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Gửi lệnh in thất bại.");
            }
        }

    }

    
    public partial class OrderViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        private readonly IDialogService _dialogService;

        private readonly CreateOrderViewModel _createOrderVM;


        private List<OrderItemViewModel> _allOrders = new();
        private const int _pageSize = 7;
        private bool _isProcessingAction = false; // Thêm flag để tránh xử lý SelectedOrder khi đang Edit/Delete


        // Data Sources
        [ObservableProperty] private ObservableCollection<OrderItemViewModel> _displayOrders = new();

        // Pagination Properties
        [ObservableProperty] private int _currentPage = 1;
        [ObservableProperty] private int _totalCount = 0; // Tổng số bản ghi server báo về
        [ObservableProperty] private string _paginationStatusText = string.Empty;
        [ObservableProperty] private bool _canGoNext;
        [ObservableProperty] private bool _canGoPrevious;

        // Stats Properties
        [ObservableProperty] private int _totalOrdersCount;
        [ObservableProperty] private string _totalRevenueDisplay = "0 đ";
        [ObservableProperty] private int _pendingOrdersCount;

        // Filter Properties
        public ObservableCollection<string> StatusFilters { get; } = new() { "Tất cả", "Mới tạo", "Đã thanh toán", "Đã hủy" };

        [ObservableProperty] private string _selectedStatusFilter = "Tất cả";
        [ObservableProperty] private DateTimeOffset? _filterFromDate;
        [ObservableProperty] private DateTimeOffset? _filterToDate;

        // Selection & Loading
        [ObservableProperty] private OrderItemViewModel? _selectedOrder;
        [ObservableProperty] private bool _isLoading;

        public OrderViewModel(IOrderService orderService, IDialogService dialogService, CreateOrderViewModel createOrderVM)
        {
            _orderService = orderService;
            _dialogService = dialogService;
            
            _createOrderVM = createOrderVM;
            LoadDataAsync(1);
        }

        partial void OnSelectedStatusFilterChanged(string value) => LoadDataAsync(1);

       
        partial void OnFilterFromDateChanged(DateTimeOffset? value) => LoadDataAsync(1);
        partial void OnFilterToDateChanged(DateTimeOffset? value) => LoadDataAsync(1);

        async partial void OnSelectedOrderChanged(OrderItemViewModel? value)
        {
          
            if (_isProcessingAction || value == null)
            {
                return;
            }

            try
            {
              
                IsLoading = true;

               
                var details = await _orderService.GetOrderDetailsAsync(value.OrderId);

               
                value.LoadDetails(details);

               
                await _dialogService.ShowOrderDetailDialogAsync(value);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi tải chi tiết: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
               
                DispatcherQueue.GetForCurrentThread().TryEnqueue(() => { SelectedOrder = null; });
            }
        }

        public async void LoadDataAsync(int pageNumber = 1)
        {
            if (IsLoading) return;
            IsLoading = true;

            try
            {
                CurrentPage = pageNumber;

                
                string status = SelectedStatusFilter;
                DateTime? from = FilterFromDate?.DateTime;
                DateTime? to = FilterToDate?.DateTime;

                
                var (items, count, revenue, pending) = await _orderService.GetOrdersAsync(CurrentPage, _pageSize, status, from, to);

               
                DisplayOrders.Clear();
                foreach (var item in items)
                {
                    DisplayOrders.Add(new OrderItemViewModel(item, _orderService, EditOrder, DeleteOrder));
                }

                
                TotalCount = count;
                TotalOrdersCount = count;
                TotalRevenueDisplay = revenue.ToString("N0") + " đ";
                PendingOrdersCount = pending;

                UpdatePaginationUI();
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

        private void UpdatePaginationUI()
        {
            int totalPages = (int)Math.Ceiling((double)TotalCount / _pageSize);

            // Tính toán text hiển thị (Ví dụ: 1 - 7 trong 25)
            int startCount = TotalCount == 0 ? 0 : (CurrentPage - 1) * _pageSize + 1;
            int endCount = Math.Min(CurrentPage * _pageSize, TotalCount);

            PaginationStatusText = $"Hiển thị {startCount} - {endCount} trong {TotalCount} đơn hàng";

            CanGoPrevious = CurrentPage > 1;
            CanGoNext = CurrentPage < totalPages;
        }

        private void CalculateStats()
        {
            TotalOrdersCount = _allOrders.Count;
            
            decimal totalRevenue = _allOrders.Sum(x => x.Model.Amount);
            TotalRevenueDisplay = totalRevenue.ToString("N0") + " đ";
            PendingOrdersCount = _allOrders.Count(x => x.Status == "Mới tạo" || x.Status == "Đang xử lý");
        }

        
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
        private void NextPage()
        {
            if (CanGoNext) LoadDataAsync(CurrentPage + 1);
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (CanGoPrevious) LoadDataAsync(CurrentPage - 1);
        }

        [RelayCommand]
        private void ClearFilter()
        {
            // Reset các biến Filter, các hàm OnChanged sẽ tự động gọi LoadDataAsync(1)
            _isProcessingAction = true; // Chặn reload liên tục nếu muốn, nhưng ở đây để đơn giản cứ để nó chạy
            SelectedStatusFilter = "Tất cả";
            FilterFromDate = null;
            FilterToDate = null;
            LoadDataAsync(1); // Gọi tường minh để chắc chắn
        }

        [RelayCommand]
        private async Task EditOrder(OrderItemViewModel item)
        {
            if (item == null) return;

            _isProcessingAction = true; // Cờ chặn logic SelectionChanged
            try
            {
               
                IsLoading = true; // Hiển thị vòng xoay loading

                
                var details = await _orderService.GetOrderDetailsAsync(item.OrderId);

                
                item.LoadDetails(details);

                IsLoading = false; 


               
                bool isSaved = await _dialogService.ShowEditOrderDialogAsync(item);

                if (isSaved)
                {
                    await RefreshAfterEdit();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi mở sửa đơn hàng: {ex.Message}");
                IsLoading = false;
               
            }
            finally
            {
                _isProcessingAction = false;
                
                DispatcherQueue.GetForCurrentThread().TryEnqueue(() => { SelectedOrder = null; });
            }
        }

        [RelayCommand]
        private async Task DeleteOrder(OrderItemViewModel item)
        {
            if (item == null) return;

            // 1. Hỏi xác nhận
            bool isConfirmed = await _dialogService.ShowConfirmationAsync(
                "Xác nhận xóa",
                $"Bạn có chắc chắn muốn xóa đơn hàng ?", // Dùng OrderCodeDisplay cho đẹp
                "Xóa",
                "Hủy");

            if (!isConfirmed) return;

            // 2. Thực hiện xóa
            // Lưu ý: Không bật IsLoading ở đây vội, để LoadDataAsync lo
            try
            {
                bool success = await _orderService.DeleteOrderAsync(item.OrderId);

                if (success)
                {
                    // --- LOGIC XỬ LÝ TRANG (QUAN TRỌNG) ---

                    // Nếu item bị xóa là item CUỐI CÙNG của trang hiện tại
                    // VÀ chúng ta đang ở trang > 1
                    // THÌ phải lùi về trang trước đó.
                    if (DisplayOrders.Count == 1 && CurrentPage > 1)
                    {
                        CurrentPage--;
                    }

                    // Gọi hàm LoadDataAsync để tải lại dữ liệu mới từ Server
                    // Hàm này sẽ tự lo việc hiển thị Loading và cập nhật danh sách
                    LoadDataAsync(CurrentPage);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Xóa thất bại order {item.OrderId}");
                    // Có thể hiện thông báo lỗi ở đây
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi xóa: {ex.Message}");
            }
            // Không cần finally { IsLoading = false } vì LoadDataAsync sẽ làm việc đó
        }

        [RelayCommand]
        private async Task CreateNewOrder()
        {
            // Mở dialog
            bool created = await _dialogService.ShowCreateOrderDialogAsync(_createOrderVM);

            if (created)
            {
                _createOrderVM.ResetData();
                LoadDataAsync();
            }
        }
        public async Task RefreshAfterEdit()
        {
            
            LoadDataAsync();
            
        }

    }
}