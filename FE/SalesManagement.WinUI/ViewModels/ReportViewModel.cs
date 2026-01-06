using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalesManagement.WinUI.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private readonly IReportService _reportService;

        private DateTimeOffset _startDate;
        private DateTimeOffset _endDate;
        private bool _isLoading;

        public ReportViewModel(IReportService reportService)
        {
            _reportService = reportService;

            // Mặc định: đầu năm hiện tại đến ngày hiện tại
            var today = DateTime.Today;
            _startDate = new DateTimeOffset(new DateTime(today.Year, 1, 1));
            _endDate = new DateTimeOffset(today);
        }

        public DateTimeOffset StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTimeOffset EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        // Revenue Collections
        public ObservableCollection<ReportRevenueDaily> RevenueDaily { get; } = new();
        public ObservableCollection<ReportRevenueWeekly> RevenueWeekly { get; } = new();
        public ObservableCollection<ReportRevenueMonth> RevenueMonth { get; } = new();
        public ObservableCollection<ReportRevenueYear> RevenueYear { get; } = new();

        // Product Collections
        public ObservableCollection<ReportProductDaily> ProductDaily { get; } = new();
        public ObservableCollection<ReportProductWeekly> ProductWeekly { get; } = new();
        public ObservableCollection<ReportProductMonth> ProductMonth { get; } = new();
        public ObservableCollection<ReportProductYear> ProductYear { get; } = new();
        public ObservableCollection<ReportProductSales> ProductSales { get; } = new();

        // CHUẨN BỊ DATA PIE
        public ObservableCollection<PieItem> RevenueDayPie { get; } = new();
        public ObservableCollection<PieItem> RevenueMonthPie { get; } = new();
        public ObservableCollection<PieItem> RevenueYearPie { get; } = new();


        // Current Period Data
        public ReportRevenueCurrentYear? CurrentYearData { get; private set; }
        public ReportRevenueCurrentYear? CurrentMonthData { get; private set; }

        public async Task LoadReportAsync()
        {
            IsLoading = true;

            try
            {
                ClearAllCollections();

                var start = StartDate.DateTime;
                var end = EndDate.DateTime;

                // Load tất cả dữ liệu song song
                var tasks = new List<Task>
                {
                    LoadRevenueDailyAsync(start, end),
                    LoadRevenueWeeklyAsync(start, end),
                    LoadRevenueMonthAsync(start, end),
                    LoadRevenueYearAsync(start, end),
                    LoadProductDailyAsync(start, end),
                    LoadProductWeeklyAsync(start, end),
                    LoadProductMonthAsync(start, end),
                    LoadProductYearAsync(start, end),
                    LoadProductSalesAsync(start, end),
                    LoadCurrentYearDataAsync(),
                    LoadCurrentMonthDataAsync()
                };

                await Task.WhenAll(tasks);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ClearAllCollections()
        {
            RevenueDaily.Clear();
            RevenueWeekly.Clear();
            RevenueMonth.Clear();
            RevenueYear.Clear();
            ProductDaily.Clear();
            ProductWeekly.Clear();
            ProductMonth.Clear();
            ProductYear.Clear();
            ProductSales.Clear();
        }

        private async Task LoadRevenueDailyAsync(DateTime start, DateTime end)
        {
            var data = await _reportService.GetRevenueByDailyAsync(start, end);

            foreach (var item in data)
                RevenueDaily.Add(item);

            // ===== PIE: TOP 7 DAYS =====
            RevenueDayPie.Clear();

            foreach (var d in data
                .OrderByDescending(x => x.Revenue)
                .Take(7))
            {
                RevenueDayPie.Add(new PieItem
                {
                    Label = d.Date.ToString("dd/MM"),
                    Value = (double)d.Revenue
                });
            }
        }


        private async Task LoadRevenueWeeklyAsync(DateTime start, DateTime end)
        {
            var data = await _reportService.GetRevenueByWeeklyAsync(start, end);
            foreach (var item in data)
                RevenueWeekly.Add(item);
        }

        private async Task LoadRevenueMonthAsync(DateTime start, DateTime end)
        {
            var data = await _reportService.GetRevenueByMonthAsync(start, end);

            foreach (var item in data)
                RevenueMonth.Add(item);

            RevenueMonthPie.Clear();

            foreach (var m in data)
            {
                RevenueMonthPie.Add(new PieItem
                {
                    Label = $"{m.MonthName}/{m.Year}",
                    Value = (double)m.Revenue
                });
            }
        }


        private async Task LoadRevenueYearAsync(DateTime start, DateTime end)
        {
            var data = await _reportService.GetRevenueByYearAsync(start, end);

            foreach (var item in data)
                RevenueYear.Add(item);

            RevenueYearPie.Clear();

            foreach (var y in data)
            {
                RevenueYearPie.Add(new PieItem
                {
                    Label = y.Year.ToString(),
                    Value = (double)y.Revenue
                });
            }
        }


        private async Task LoadProductDailyAsync(DateTime start, DateTime end)
        {
            var data = await _reportService.GetProductReportByDailyAsync(start, end);
            foreach (var item in data)
                ProductDaily.Add(item);
        }

        private async Task LoadProductWeeklyAsync(DateTime start, DateTime end)
        {
            var data = await _reportService.GetProductReportByWeeklyAsync(start, end);
            foreach (var item in data)
                ProductWeekly.Add(item);
        }

        private async Task LoadProductMonthAsync(DateTime start, DateTime end)
        {
            var data = await _reportService.GetProductReportByMonthAsync(start, end);
            foreach (var item in data)
                ProductMonth.Add(item);
        }

        private async Task LoadProductYearAsync(DateTime start, DateTime end)
        {
            var data = await _reportService.GetProductReportByYearAsync(start, end);
            foreach (var item in data)
                ProductYear.Add(item);
        }

        private async Task LoadProductSalesAsync(DateTime start, DateTime end)
        {
            var data = await _reportService.GetProductSalesAsync(start, end);
            foreach (var item in data)
                ProductSales.Add(item);
        }

        private async Task LoadCurrentYearDataAsync()
        {
            CurrentYearData = await _reportService.GetRevenueCurrentYearAsync();
            OnPropertyChanged(nameof(CurrentYearData));
        }

        private async Task LoadCurrentMonthDataAsync()
        {
            CurrentMonthData = await _reportService.GetRevenueCurrentMonthAsync();
            OnPropertyChanged(nameof(CurrentMonthData));
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}