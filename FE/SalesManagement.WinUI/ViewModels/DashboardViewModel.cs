using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IAuthService _authService;
        private readonly ILoadingService _loadingService;

        private DashboardResponse _dashboardData;
        private bool _isLoading;
        private string _errorMessage;

        public DashboardResponse DashboardData
        {
            get => _dashboardData;
            set => SetProperty(ref _dashboardData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public DashboardViewModel(
            IApiService apiService,
            IAuthService authService,
            ILoadingService loadingService = null)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loadingService = loadingService;
            DashboardData = new DashboardResponse();
        }

        public async Task LoadDashboardData()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                System.Diagnostics.Debug.WriteLine("=== Loading Dashboard Data ===");

                // Check token first
                var token = _authService.GetAccessToken();
                if (string.IsNullOrEmpty(token))
                {
                    ErrorMessage = "Authentication required. Please log in again.";
                    System.Diagnostics.Debug.WriteLine($"❌ {ErrorMessage}");
                    DashboardData = new DashboardResponse();
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"✅ Token found, length: {token.Length}");

                // Make API request
                var response = await _apiService.GetAsync<DashboardResponse>("api/v1/admin/dashboard");

                if (response != null)
                {
                    System.Diagnostics.Debug.WriteLine("✅ Dashboard data received successfully");
                    System.Diagnostics.Debug.WriteLine($"📊 Total Products: {response.TotalProducts}");
                    System.Diagnostics.Debug.WriteLine($"📦 Today Orders: {response.TodayOrderCount}");
                    System.Diagnostics.Debug.WriteLine($"💰 Today Revenue: {response.TodayRevenue:C}");
                    System.Diagnostics.Debug.WriteLine($"⚠️ Low Stock: {response.LowStockProducts?.Count ?? 0}");
                    System.Diagnostics.Debug.WriteLine($"🔥 Top Selling: {response.TopSellingProducts?.Count ?? 0}");
                    System.Diagnostics.Debug.WriteLine($"📋 Recent Orders: {response.RecentOrders?.Count ?? 0}");

                    DashboardData = response;
                }
                else
                {
                    ErrorMessage = "Failed to load dashboard data: empty response";
                    System.Diagnostics.Debug.WriteLine($"⚠️ {ErrorMessage}");
                    DashboardData = new DashboardResponse();
                }
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = $"Network error: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"🔴 HTTP Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"📍 Stack: {ex.StackTrace}");
                DashboardData = new DashboardResponse();
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = $"Request error: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"🔴 Argument Error: {ex.Message}");
                DashboardData = new DashboardResponse();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Unexpected error: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"🔴 Unexpected Error: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"📍 Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"📍 Stack: {ex.StackTrace}");
                DashboardData = new DashboardResponse();
            }
            finally
            {
                IsLoading = false;
                System.Diagnostics.Debug.WriteLine("=== Loading Complete ===\n");
            }
        }

        public async Task RefreshDashboard()
        {
            await LoadDashboardData();
        }
    }
}