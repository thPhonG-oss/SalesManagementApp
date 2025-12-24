using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Text;

namespace SalesManagement.WinUI.Models
{
    public class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void NotifyDiscountUI()
        {
            OnPropertyChanged(nameof(DiscountVisibility));
            OnPropertyChanged(nameof(PriceDecorations));
            OnPropertyChanged(nameof(PriceColor));
            OnPropertyChanged(nameof(SpecialPriceText));
            OnPropertyChanged(nameof(DiscountText));
        }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public int PublicationYear { get; set; }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PriceText));
            }
        }

        private decimal? _specialPrice;
        public decimal? SpecialPrice
        {
            get => _specialPrice;
            set
            {
                _specialPrice = value;
                OnPropertyChanged();
                NotifyDiscountUI();
            }
        }

        public int StockQuantity { get; set; }
        public int MinStockQuantity { get; set; }
        public int SoldQuantity { get; set; }
        public bool IsActive { get; set; }

        private decimal _discountPercentage;
        public decimal DiscountPercentage
        {
            get => _discountPercentage;
            set
            {
                _discountPercentage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DiscountText));
            }
        }

        private bool _isDiscounted;
        public bool IsDiscounted
        {
            get => _isDiscounted;
            set
            {
                _isDiscounted = value;
                OnPropertyChanged();
                NotifyDiscountUI();
            }
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Category Category { get; set; } = new();
        public List<ProductImage> Images { get; set; } = new();

        // ================= UI HELPER =================

        public string ImageUrl { get; set; } =
            "https://res.cloudinary.com/dznocieoi/image/upload/v1766487761/istockphoto-1396814518-612x612_upvria.jpg";

        public string StockText => $"{StockQuantity}";

        public string PriceText => Price.ToString("N0") + " đ";

        public string SpecialPriceText =>
            IsDiscounted && SpecialPrice.HasValue
                ? SpecialPrice.Value.ToString("N0") + " đ"
                : "";

        public Visibility DiscountVisibility =>
            IsDiscounted ? Visibility.Visible : Visibility.Collapsed;

        public TextDecorations PriceDecorations =>
            IsDiscounted ? TextDecorations.Strikethrough : TextDecorations.None;

        public Brush PriceColor =>
            IsDiscounted
                ? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 150, 150, 150))
                : new SolidColorBrush(Colors.Black);

        public double PriceValue
        {
            get => (double)Price;
            set
            {
                Price = (decimal)value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged();
            }
        }


        public string DiscountText => "-" + DiscountPercentage.ToString("0") + "%";
    }
}
