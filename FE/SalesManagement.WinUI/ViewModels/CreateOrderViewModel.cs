using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.ViewModels
{

    public partial class CreateOrderDetailViewModel : ObservableObject
    {
        public List<Product> AvailableProducts { get; }

        [ObservableProperty]
        private ObservableCollection<Product> _filteredProducts;

        public CreateOrderDetailViewModel(List<Product> products)
        {
            AvailableProducts = products ?? new List<Product>();
            _filteredProducts = new ObservableCollection<Product>(AvailableProducts);
            Debug.WriteLine($"CreateOrderDetailViewModel initialized with {AvailableProducts.Count} products");
        }

        [ObservableProperty]
        private Product? _selectedProduct;

        [ObservableProperty]
        private int _quantity = 1;

        // Giá sau khi đã trừ chiết khấu sản phẩm (nếu có)
        [ObservableProperty]
        private decimal _price = 0;

        [ObservableProperty]
        private decimal _originalPrice = 0;


        public double QuantityDouble
        {
            get => (double)Quantity;
            set { if (Quantity != (int)value) Quantity = (int)value; }
        }

        public void OnSearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            try
            {
                Debug.WriteLine($"OnSearchTextChanged called. Reason: {args.Reason}");
                
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    var query = sender.Text.ToLower().Trim();
                    Debug.WriteLine($"Search query: '{query}'");

                    if (string.IsNullOrEmpty(query))
                    {
                        FilteredProducts = new ObservableCollection<Product>(AvailableProducts);
                        Debug.WriteLine($"Reset to all {FilteredProducts.Count} products");
                    }
                    else
                    {
                        var matches = AvailableProducts.Where(p =>
                            p.ProductName.ToLower().Contains(query) ||
                            p.Price.ToString().Contains(query)
                        ).ToList();

                        FilteredProducts = new ObservableCollection<Product>(matches);
                        Debug.WriteLine($"Found {FilteredProducts.Count} matching products");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnSearchTextChanged: {ex.Message}");
            }
        }

        public void OnGotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("OnGotFocus called");
                
                if (sender is AutoSuggestBox box)
                {
                    if (FilteredProducts == null || !FilteredProducts.Any())
                    {
                        FilteredProducts = new ObservableCollection<Product>(AvailableProducts);
                        Debug.WriteLine($"Reset FilteredProducts to {FilteredProducts.Count} products");
                    }

                    box.IsSuggestionListOpen = true;
                    Debug.WriteLine("Suggestion list opened");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnGotFocus: {ex.Message}");
            }
        }
        partial void OnSelectedProductChanged(Product? value)
        {
            if (value != null)
            {
                OriginalPrice = value.Price;

                // LOGIC: Tính giá Special Price
                // Công thức: Price * (1 - discountPercentage)
                if (value.IsDiscounted && value.DiscountPercentage > 0)
                {
                    // Giả sử DiscountPercentage trong Product model lưu là 10 (tức 10%)
                    // Nếu lưu 0.1 thì bỏ chia 100
                    decimal factor = 1 - (value.DiscountPercentage / 100m);
                    Price = value.Price * factor;
                }
                else
                {
                    Price = value.Price;
                }
            }
            else
            {
                Price = 0;
                OriginalPrice = 0;
            }
            UpdateTotals();
        }

        partial void OnQuantityChanged(int value) => UpdateTotals();
        partial void OnPriceChanged(decimal value) => UpdateTotals();

        public void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            try
            {
                Debug.WriteLine("OnSuggestionChosen called");
                
                if (args.SelectedItem is Product product)
                {
                    SelectedProduct = product;
                    sender.Text = product.ProductName;
                    Debug.WriteLine($"Selected product: {product.ProductName}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnSuggestionChosen: {ex.Message}");
            }
        }
        private void UpdateTotals()
        {
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(TotalDisplay));
            OnPropertyChanged(nameof(PriceDisplay));
        }

        public decimal Total => Quantity * Price;
        public string TotalDisplay => Total.ToString("N0");
        public string PriceDisplay => Price.ToString("N0");
    } 


    public partial class CreateOrderViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IPromotionService _promotionService;

        private List<Product> _allProducts = new();
        private List<Promotion> _allPromotions = new();

        [ObservableProperty]
        private ObservableCollection<CreateOrderDetailViewModel> _items = new();

        // --- Promotion ---
        [ObservableProperty]
        private List<Promotion> _promotions = new();

        [ObservableProperty]
        private Promotion? _selectedPromotion;

        // --- Totals ---
        [ObservableProperty] private decimal _subTotalAmount;
        [ObservableProperty] private decimal _discountAmount;
        [ObservableProperty] private decimal _totalAmount;

        [ObservableProperty]
        private string _shipAddress = string.Empty;

        [ObservableProperty]
        private string _note = string.Empty;

        [ObservableProperty]
        private string _customerEmail = string.Empty;

        public string SubTotalDisplay => SubTotalAmount.ToString("N0") + " đ";
        public string DiscountAmountDisplay => "-" + DiscountAmount.ToString("N0") + " đ";
        public string TotalAmountDisplay => TotalAmount.ToString("N0") + " đ";

        public CreateOrderViewModel(
            IOrderService orderService,
            IProductService productService,
            IPromotionService promotionService)
        {
            _orderService = orderService;
            _productService = productService;
            _promotionService = promotionService;
            LoadData();
        }

        private async void LoadData()
        {
            
            var productRes = await _productService.GetProductsAsync(1, 1000);
            if (productRes?.Products != null)
            {
                _allProducts = productRes.Products;
                AddRow();
            }
            var promoData = await _promotionService.GetActivePromotionsAsync();
            _allPromotions = promoData ?? new List<Promotion>();

        }

        public void ResetData()
        {
            // 1. Xóa danh sách sản phẩm
            Items.Clear();
            AddRow(); // Thêm lại 1 dòng trống mặc định

            // 2. Xóa thông tin nhập liệu
            CustomerEmail = string.Empty;
            ShipAddress = string.Empty;
            Note = string.Empty;

            // 3. Reset Voucher & Tiền
            SelectedPromotion = null;
            SubTotalAmount = 0;
            DiscountAmount = 0;
            TotalAmount = 0;
        }

        [RelayCommand]
        private void AddRow()
        {
            var newItem = new CreateOrderDetailViewModel(_allProducts);
            newItem.PropertyChanged += OnItemPropertyChanged;
            Items.Add(newItem);
        }

        [RelayCommand]
        private void RemoveRow(CreateOrderDetailViewModel item)
        {
            if (Items.Contains(item))
            {
                item.PropertyChanged -= OnItemPropertyChanged;
                Items.Remove(item);
                CalculateTotal();
            }
        }

        public async void OpenPromotionList()
        {
            // 1. Lấy danh sách gốc (đã load ở LoadData)
            if (_allPromotions == null || !_allPromotions.Any())
            {
                var data = await _promotionService.GetActivePromotionsAsync();
                _allPromotions = data ?? new List<Promotion>();
            }
            foreach (var item in _allPromotions)
            {
                Debug.WriteLine($"[Valid Promotion] {item.PromotionId} - MinOrder: {item.MinOrderAmount}, SubTotal: {SubTotalAmount}");
            }

            // 2. Lọc danh sách: CHỈ lấy cái nào thỏa mãn SubTotal hiện tại
            // SubTotalAmount lúc chưa chọn sp sẽ là 0 -> Voucher 200k sẽ bị loại ngay ở đây
            var validPromotions = _allPromotions.Where(p =>
            {
                // Giữ lại Voucher rỗng (nếu bạn có thiết kế voucher ID=0 là "Không chọn gì")
                if (p.PromotionId == 0) return true;

                // Logic lọc cứng: Tiền hiện tại phải >= Điều kiện voucher
                return p.MinOrderAmount <= SubTotalAmount;
            }).ToList();

            foreach ( var item in validPromotions)
                            {
                Debug.WriteLine($"[Valid Promotion] {item.PromotionId} - MinOrder: {item.MinOrderAmount}, SubTotal: {SubTotalAmount}");
            }

            // 3. Cập nhật UI
            Promotions = validPromotions;

            // 4. --- FIX QUAN TRỌNG TẠI ĐÂY ---
            // Kiểm tra xem voucher ĐANG chọn có còn nằm trong danh sách mới lọc không?
            if (SelectedPromotion != null)
            {
                var stillValid = Promotions.Any(p => p.PromotionId == SelectedPromotion.PromotionId);
                if (!stillValid)
                {
                    // Nếu voucher đang chọn không còn hợp lệ (ví dụ: đang chọn voucher 200k mà tổng tiền tụt xuống 0)
                    // -> Hủy chọn ngay lập tức
                    SelectedPromotion = null;
                }
            }

            // XÓA BỎ DÒNG: SelectedPromotion = Promotions.FirstOrDefault(); 
            // Lý do: Đừng bao giờ tự ý chọn giúp khách, trừ khi bạn có một Voucher mặc định tên là "Không sử dụng" (ID=0)
        }

        partial void OnSelectedPromotionChanged(Promotion? value)
        {
            OnPropertyChanged(nameof(PromotionInfoVisibility)); 
            CalculateTotal();                                   
        }

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CreateOrderDetailViewModel.Total)) CalculateTotal();
        }

        private void CalculateTotal()
        {
            // 1. Tính tổng tạm tính mới
            SubTotalAmount = Items.Sum(x => x.Total);

            // --- LOGIC MỚI: Tự động hủy Voucher nếu không còn đủ điều kiện ---
            if(SelectedPromotion != null)
    {
                // Kiểm tra ngay lập tức: Tiền có đủ không?
                if (SubTotalAmount < SelectedPromotion.MinOrderAmount)
                {
                    // Nếu không đủ (ví dụ SubTotal = 0) -> ĐÁ VĂNG voucher ra ngay
                    SelectedPromotion = null;

                    // Debug để bạn kiểm chứng
                    System.Diagnostics.Debug.WriteLine("Đã gỡ bỏ Voucher vì không đủ điều kiện (SubTotal < MinOrder)");

                    // Return để hàm chạy lại từ đầu (do SelectedPromotion thay đổi kích hoạt lại hàm này)
                    return;
                }
            }
            // ----------------------------------------------------------------

            // 2. Tính toán Discount (Khi code chạy đến đây nghĩa là Voucher đã hợp lệ hoặc Null)
            decimal discount = 0;

            if (SelectedPromotion != null && SelectedPromotion.PromotionId != 0)
            {
                // Vì đã check ở trên rồi nên ở đây chắc chắn SubTotalAmount >= MinOrderAmount
                if (SelectedPromotion.DiscountType == "PERCENTAGE")
                {
                    discount = SubTotalAmount * (decimal)SelectedPromotion.DiscountPercentage;

                    if (SelectedPromotion.MaxDiscountValue > 0 && discount > SelectedPromotion.MaxDiscountValue)
                    {
                        discount = SelectedPromotion.MaxDiscountValue;
                    }
                }
                else
                {
                    discount = SelectedPromotion.DiscountValue;
                }
            }

            // Safety check: Không giảm quá số tiền tạm tính
            if (discount > SubTotalAmount) discount = SubTotalAmount;

            DiscountAmount = discount;
            TotalAmount = SubTotalAmount - DiscountAmount;

            // Cập nhật UI
            OnPropertyChanged(nameof(SubTotalDisplay));
            OnPropertyChanged(nameof(DiscountAmountDisplay));
            OnPropertyChanged(nameof(TotalAmountDisplay));
        }

        public Visibility PromotionInfoVisibility =>
        SelectedPromotion != null ? Visibility.Visible : Visibility.Collapsed;
        public async Task<bool> CreateOrderAsync()
        {
            // 1. Validate Email (Cơ bản)
            if (string.IsNullOrWhiteSpace(CustomerEmail))
            {
                // Có thể thêm thông báo lỗi ở đây nếu cần
                System.Diagnostics.Debug.WriteLine("Email không được để trống");
                return false;
            }

            var validItems = Items.Where(x => x.SelectedProduct != null && x.Quantity > 0).ToList();
            if (!validItems.Any()) return false;

            // 2. Tạo Request theo mẫu mới
            var request = new CreateOrderRequest
            {
                Email = CustomerEmail, // ✅ Gán Email từ UI

                PromotionId = (SelectedPromotion == null || SelectedPromotion.PromotionId == 0) ? null : SelectedPromotion.PromotionId,
              
                Notes = Note,
                ShippingAddress = string.IsNullOrWhiteSpace(ShipAddress) ? "Tại cửa hàng" : ShipAddress,
                PaymentMethod = "CASH_ON_DELIVERY",

                OrderItems = validItems.Select(x => new CreateOrderItemRequest
                {
                    ProductId = x.SelectedProduct!.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.Price,
                    Discount = 0, // Hoặc tính logic discount nếu có
                    TotalPrice = x.Total
                }).ToList()
            };

            Debug.WriteLine($"[CreateOrder] Request: {System.Text.Json.JsonSerializer.Serialize(request)}");
            return await _orderService.CreateOrderAsync(request);
        }



    } // <--- KẾT THÚC CLASS CHA
}