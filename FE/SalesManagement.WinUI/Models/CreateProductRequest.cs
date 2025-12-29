namespace SalesManagement.WinUI.Models
{
    public class CreateProductRequest
    {
        public int CategoryId { get; set; }
        public string ProductName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Author { get; set; } = "";
        public string Publisher { get; set; } = "";
        public int PublicationYear { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int MinStockQuantity { get; set; }
    }
}
