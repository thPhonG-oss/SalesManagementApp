namespace SalesManagement.WinUI.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }

        public int PublicationYear { get; set; }

        public decimal Price { get; set; }
        public decimal? SpecialPrice { get; set; }

        public int StockQuantity { get; set; }
        public int MinStockQuantity { get; set; }
        public int SoldQuantity { get; set; }

        public bool IsActive { get; set; }

        public decimal DiscountPercentage { get; set; }
        public bool IsDiscounted { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Category Category { get; set; } = new();

        public List<ProductImage> Images { get; set; } = new();

        // 👉 tiện cho XAML
        public string ImageUrl =>
            Images != null && Images.Count > 0
                ? Images[0].ImageUrl
                : "https://bookbuy.vn/kcfinder/upload/files/thao-tung-tam-ly-nhan-dien-thuc-tinh-va-chua-lanh-nhung-ton-thuong-tiem-an.jpeg";
    }
}
