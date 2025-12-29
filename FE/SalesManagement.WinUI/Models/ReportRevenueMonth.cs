namespace SalesManagement.WinUI.Models
{
    public class ReportRevenueMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }
}
