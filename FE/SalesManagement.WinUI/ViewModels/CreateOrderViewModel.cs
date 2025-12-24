using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.ViewModels
{

    public partial class CreateOrderDetailViewModel : ObservableObject
    {
        public List<Product> AvailableProducts { get; }

        public CreateOrderDetailViewModel(List<Product> products)
        {
            AvailableProducts = products;
        }

        [ObservableProperty]
        private Product? _selectedProduct;

        [ObservableProperty]
        private int _quantity = 1;

        [ObservableProperty]
        private decimal _price = 0;

        // --- QUAN TRỌNG: PriceDouble và QuantityDouble PHẢI NẰM Ở ĐÂY ---
        // Vì đây là thuộc tính của TỪNG DÒNG
        public double PriceDouble
        {
            get => (double)Price;
            set
            {
                var decimalValue = (decimal)value;
                if (Price != decimalValue) Price = decimalValue;
            }
        }

        public double QuantityDouble
        {
            get => (double)Quantity;
            set
            {
                if (Quantity != (int)value) Quantity = (int)value;
            }
        }
        // ---------------------------------------------------------------

        // Logic cập nhật khi chọn sản phẩm
        partial void OnSelectedProductChanged(Product? value)
        {
            if (value != null)
            {
                Price = value.Price;
                OnPropertyChanged(nameof(PriceDouble)); // Cập nhật UI
            }
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(TotalDisplay));
        }

        partial void OnQuantityChanged(int value)
        {
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(TotalDisplay));
            OnPropertyChanged(nameof(QuantityDouble));
        }

        partial void OnPriceChanged(decimal value)
        {
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(TotalDisplay));
            OnPropertyChanged(nameof(PriceDouble));
        }

        public decimal Total => Quantity * Price;
        public string TotalDisplay => Total.ToString("N0");
        public string PriceDisplay => Price.ToString("N0");
    } // <--- KẾT THÚC CLASS CON TẠI ĐÂY


    public partial class CreateOrderViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        private List<Product> _allProducts = new();

        // Danh sách các dòng (Items chứa các object của class con ở trên)
        [ObservableProperty]
        private ObservableCollection<CreateOrderDetailViewModel> _items = new();

        [ObservableProperty]
        private string _totalAmountDisplay = "0 đ";

        public CreateOrderViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            LoadProducts();
        }

        // --- LƯU Ý: Ở CLASS NÀY KHÔNG ĐƯỢC CÓ PriceDouble ---
        // Nếu bạn viết public double PriceDouble ở đây là SAI.

        private async void LoadProducts()
        {
            _allProducts = await _orderService.GetProductsAsync();
            AddRow();
        }

        [RelayCommand]
        private void AddRow()
        {
            var newItem = new CreateOrderDetailViewModel(_allProducts);
            newItem.PropertyChanged += OnItemPropertyChanged;
            Items.Add(newItem);
            CalculateTotal();
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

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CreateOrderDetailViewModel.Total))
            {
                CalculateTotal();
            }
        }

        private void CalculateTotal()
        {
            decimal total = Items.Sum(x => x.Total);
            TotalAmountDisplay = total.ToString("N0") + " đ";
        }

        public async Task<bool> CreateOrderAsync()
        {
            var validItems = Items.Where(x => x.SelectedProduct != null && x.Quantity > 0).ToList();
            if (!validItems.Any()) return false;

            var newOrder = new Order
            {
                OrderId = "NEW" + DateTime.Now.Ticks.ToString()[^4..],
                Date = DateTime.Now,
                Status = "Mới tạo",
                ItemsCount = validItems.Count,
                Amount = validItems.Sum(x => x.Total)
            };

            var details = validItems.Select(x => new OrderDetail
            {
                ProductName = x.SelectedProduct!.ProductName,
                Quantity = x.Quantity,
                Price = x.Price
            }).ToList();

            return await _orderService.CreateOrderAsync(newOrder, details);
        }
    } // <--- KẾT THÚC CLASS CHA
}