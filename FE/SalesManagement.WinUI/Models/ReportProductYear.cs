namespace SalesManagement.WinUI.Models
{
    public class ReportProductYear
    {
        public int Year { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int QuantitySold { get; set; }

        public decimal Revenue { get; set; }
    }
}
