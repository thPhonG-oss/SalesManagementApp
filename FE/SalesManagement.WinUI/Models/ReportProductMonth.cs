namespace SalesManagement.WinUI.Models
{
    public class ReportProductMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }
}
