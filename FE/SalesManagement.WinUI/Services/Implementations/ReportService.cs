using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SalesManagement.WinUI.Services
{
    public class ReportService : IReportService
    {
        private readonly HttpClient _client;
        private readonly IAuthService _authService;

        public ReportService(IHttpClientFactory httpClientFactory, IAuthService authService)
        {
            _client = httpClientFactory.CreateClient("API");
            _authService = authService;
        }

        private void AttachToken()
        {
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<ReportRevenueYear>> GetRevenueByYearAsync(
            DateTime startDate,
            DateTime endDate)
        {
            AttachToken();

            string url =
                $"/api/v1/reports/revenue/year" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}";

            var response = await _client.GetAsync(url);

            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT YEAR] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new List<ReportRevenueYear>();

            return await response.Content
                .ReadFromJsonAsync<List<ReportRevenueYear>>()
                ?? new List<ReportRevenueYear>();
        }

        public async Task<List<ReportRevenueMonth>> GetRevenueByMonthAsync(
            DateTime startDate,
            DateTime endDate)
        {
            AttachToken();

            string url =
                $"/api/v1/reports/revenue/month" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}";

            var response = await _client.GetAsync(url);
            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT MONTH] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new List<ReportRevenueMonth>();

            return await response.Content
                .ReadFromJsonAsync<List<ReportRevenueMonth>>()
                ?? new List<ReportRevenueMonth>();
        }

        public async Task<List<ReportRevenueWeekly>> GetRevenueByWeeklyAsync(
            DateTime startDate,
            DateTime endDate)
        {
            AttachToken();

            string url =
                $"/api/v1/reports/revenue/weekly" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}";

            var response = await _client.GetAsync(url);
            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT WEEKLY] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new List<ReportRevenueWeekly>();

            return await response.Content
                .ReadFromJsonAsync<List<ReportRevenueWeekly>>()
                ?? new List<ReportRevenueWeekly>();
        }

        public async Task<List<ReportRevenueDaily>> GetRevenueByDailyAsync(
            DateTime startDate,
            DateTime endDate)
        {
            AttachToken();

            string url =
                $"/api/v1/reports/revenue/daily" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}";

            var response = await _client.GetAsync(url);
            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT DAILY] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new List<ReportRevenueDaily>();

            return await response.Content
                .ReadFromJsonAsync<List<ReportRevenueDaily>>()
                ?? new List<ReportRevenueDaily>();
        }

        public async Task<ReportRevenueCurrentYear?> GetRevenueCurrentYearAsync()
        {
            AttachToken();

            var response = await _client.GetAsync(
                "/api/v1/reports/revenue/current-year");
            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT CURRENT YEAR] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<ReportRevenueCurrentYear>();
        }

        public async Task<ReportRevenueCurrentYear?> GetRevenueCurrentMonthAsync()
        {
            AttachToken();

            var response = await _client.GetAsync(
                "/api/v1/reports/revenue/current-month");
            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT CURRENT MONTH] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<ReportRevenueCurrentYear>();
        }

        public async Task<List<ReportProductYear>> GetProductReportByYearAsync(
            DateTime startDate,
            DateTime endDate)
        {
            AttachToken();

            string url =
                "/api/v1/reports/products/year" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}";

            var response = await _client.GetAsync(url);
            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT PRODUCT YEAR] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new List<ReportProductYear>();

            return await response.Content
                .ReadFromJsonAsync<List<ReportProductYear>>()
                ?? new List<ReportProductYear>();
        }

        public async Task<List<ReportProductWeekly>> GetProductReportByWeeklyAsync(
            DateTime startDate,
            DateTime endDate)
        {
            AttachToken();
            string url =
                "/api/v1/reports/products/weekly" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}";

            var response = await _client.GetAsync(url);
            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT PRODUCT WEEKLY] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new List<ReportProductWeekly>();

            return await response.Content
                .ReadFromJsonAsync<List<ReportProductWeekly>>()
                ?? new List<ReportProductWeekly>();
        }

        public async Task<List<ReportProductSales>> GetProductSalesAsync(
            DateTime startDate,
            DateTime endDate)
        {
            AttachToken();

            string url =
                "/api/v1/reports/products/sales" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}";

            var response = await _client.GetAsync(url);
            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT PRODUCT SALES] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new List<ReportProductSales>();

            return await response.Content
                .ReadFromJsonAsync<List<ReportProductSales>>()
                ?? new List<ReportProductSales>();
        }

        public async Task<List<ReportProductMonth>> GetProductReportByMonthAsync(
            DateTime startDate,
            DateTime endDate)
        {
            AttachToken();

            string url =
                "/api/v1/reports/products/month" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}";

            var response = await _client.GetAsync(url);
            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT PRODUCT MONTH] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new List<ReportProductMonth>();

            return await response.Content
                .ReadFromJsonAsync<List<ReportProductMonth>>()
                ?? new List<ReportProductMonth>();
        }

        public async Task<List<ReportProductDaily>> GetProductReportByDailyAsync(
            DateTime startDate,
            DateTime endDate)
        {
            AttachToken();

            string url =
                "/api/v1/reports/products/daily" +
                $"?startDate={startDate:yyyy-MM-dd}" +
                $"&endDate={endDate:yyyy-MM-dd}";

            var response = await _client.GetAsync(url);
            Debug.WriteLine("111111111111111");

            Debug.WriteLine($"[REPORT PRODUCT DAILY] {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new List<ReportProductDaily>();

            return await response.Content
                .ReadFromJsonAsync<List<ReportProductDaily>>()
                ?? new List<ReportProductDaily>();
        }

    }
}
