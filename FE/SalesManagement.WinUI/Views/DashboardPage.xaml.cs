using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Services;
using SalesManagement.WinUI.Services.Implementations;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using SalesManagement.WinUI.Views;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class DashboardPage : Page
    {
        private readonly DashboardViewModel _viewModel;

        public DashboardPage()
        {
            this.InitializeComponent();

            // Use App's DI container instead of manual instantiation
            _viewModel = App.Services.GetService<DashboardViewModel>()!;
            this.DataContext = _viewModel;

            // Hook Loaded event to ensure page is ready before loading data
            this.Loaded += DashboardPage_Loaded;
        }

        private async void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            // First ensure we have authentication
            var authService = App.Services.GetService<IAuthService>()!;

            if (!authService.IsAuthenticated())
            {
                System.Diagnostics.Debug.WriteLine("? User not authenticated, redirecting to login...");
                var navigationService = App.Services.GetService<INavigationService>()!;
                navigationService.NavigateTo(typeof(LoginPage));
                return;
            }

            System.Diagnostics.Debug.WriteLine("? User authenticated, loading dashboard...");

            // Now load the dashboard data
            await _viewModel.LoadDashboardData();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.RefreshDashboard();
        }
        
        public void ButtonEvent()
        {           
            // To do
        }
    }
}