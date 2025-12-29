namespace SalesManagement.WinUI.Models
{
    public class ReportRevenueCurrentYear
    {
        public List<ReportRevenueDaily> DailyRevenue { get; set; } = new();
        public List<ReportRevenueWeekly> WeeklyRevenue { get; set; } = new();
        public List<ReportRevenueMonth> MonthlyRevenue { get; set; } = new();
        public List<ReportRevenueYear> YearlyRevenue { get; set; } = new();
        public List<ReportTopProduct> TopProducts { get; set; } = new();
        public List<ReportDailyProductSale> DailyProductSales { get; set; } = new();
    }
}
