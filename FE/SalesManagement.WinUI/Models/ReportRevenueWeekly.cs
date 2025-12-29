namespace SalesManagement.WinUI.Models
{
    public class ReportRevenueWeekly
    {
        public int Year { get; set; }
        public int WeekNumber { get; set; }

        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }

        public decimal Revenue { get; set; }
    }
}
