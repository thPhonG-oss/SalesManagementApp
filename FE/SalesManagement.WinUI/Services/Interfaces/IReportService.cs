using SalesManagement.WinUI.Models;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<ReportRevenueYear>> GetRevenueByYearAsync(
            DateTime startDate,
            DateTime endDate);

        Task<List<ReportRevenueMonth>> GetRevenueByMonthAsync(
            DateTime startDate,
            DateTime endDate);

        Task<List<ReportRevenueWeekly>> GetRevenueByWeeklyAsync(
            DateTime startDate,
            DateTime endDate);

        Task<List<ReportRevenueDaily>> GetRevenueByDailyAsync(
            DateTime startDate,
            DateTime endDate);

        Task<ReportRevenueCurrentYear?> GetRevenueCurrentYearAsync();

        Task<ReportRevenueCurrentYear?> GetRevenueCurrentMonthAsync();

        // Product report
        Task<List<ReportProductYear>> GetProductReportByYearAsync(
            DateTime startDate,
            DateTime endDate);

        Task<List<ReportProductWeekly>> GetProductReportByWeeklyAsync(
            DateTime startDate,
            DateTime endDate);

        Task<List<ReportProductSales>> GetProductSalesAsync(
            DateTime startDate,
            DateTime endDate);

        Task<List<ReportProductMonth>> GetProductReportByMonthAsync(
            DateTime startDate,
            DateTime endDate);

        Task<List<ReportProductDaily>> GetProductReportByDailyAsync(
            DateTime startDate,
            DateTime endDate);

    }
}
