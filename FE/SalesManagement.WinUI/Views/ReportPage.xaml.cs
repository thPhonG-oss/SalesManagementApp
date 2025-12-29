using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.ViewModels;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class ReportPage : Page
    {
        public ReportViewModel ViewModel { get; }

        public ReportPage()
        {
            InitializeComponent();
            ViewModel = App.Services.GetRequiredService<ReportViewModel>();
            DataContext = ViewModel;
            Loaded += ReportPage_Loaded;
        }

        private async void ReportPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Tự động load khi vào trang với ngày mặc định (năm hiện tại)
            await ViewModel.LoadReportAsync();
        }

        private async void OnLoadClick(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadReportAsync();
        }
    }
}