namespace SalesManagement.WinUI.Models
{
    public class ReportProductDaily
    {
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
    }
}
