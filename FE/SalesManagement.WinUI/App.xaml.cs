using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Services;
using SalesManagement.WinUI.Services.Implementations;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using SalesManagement.WinUI.Views;
using System.IO;
using System.Net.Http;

namespace SalesManagement.WinUI;

public partial class App : Application
{
    private static Window? s_mainWindow;
    private static IServiceProvider? s_serviceProvider;

    public static IServiceProvider Services => s_serviceProvider!;
    public static Window MainWindow => s_mainWindow!;

    public App()
    {
        InitializeComponent();
        s_serviceProvider = ConfigureServices();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        services.AddSingleton<IConfiguration>(configuration);

        // HttpClient with cookie support
        services.AddHttpClient("API", (serviceProvider, client) =>
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var baseUrl = config["ApiSettings:BaseUrl"] ?? "http://localhost:8081";

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        })



        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new System.Net.CookieContainer()
        });

        // ================= SERVICES =================
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IStorageService, StorageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ILoadingService, LoadingService>();
        services.AddSingleton<IOrderService, OrderService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IReportService, ReportService>();
        services.AddTransient<IPromotionService, PromotionService>();
        services.AddSingleton<ICategoryService, CategoryService>();
        services.AddSingleton<ICustomerService, CustomerService>();
        services.AddTransient<CustomerViewModel>();

        services.AddSingleton<IAppSettingsService, AppSettingsService>();
        services.AddSingleton<IUserService, UserService>();

        services.AddTransient<IApiService>(sp =>
    new ApiService(
        sp.GetRequiredService<IHttpClientFactory>(),
        sp.GetRequiredService<IAuthService>()
    )
);

        // ⭐ CATEGORY
        services.AddSingleton<ICategoryService, CategoryService>();


        // ================= VIEWMODELS =================
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<OrderViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddTransient<PromotionViewModel>();
        services.AddTransient<LoginViewModel>();
        // ⭐ REPORT


        services.AddTransient<ReportViewModel>();
        services.AddTransient<CategoryManagementViewModel>();




        services.AddTransient<ReportViewModel>();
        services.AddSingleton<CreateOrderViewModel>();

        services.AddTransient<ProductViewModel>();
        // ⭐ PRODUCT
        services.AddSingleton<IProductService, ProductService>();
        services.AddTransient<ProductViewModel>();



        // ⭐ THÊM MỚI - Settings
        services.AddTransient<SettingsViewModel>();

        // ================= VIEWS =================
        services.AddTransient<LoginPage>();
        services.AddTransient<MainPage>();
        services.AddTransient<DashboardPage>();
        services.AddTransient<CustomerPage>();


        // ⭐ PRODUCT PAGE
        services.AddTransient<ProductPage>();

        services.AddTransient<AddProductPage>();
        services.AddTransient<CategoryManagementPage>();

        services.AddTransient<AddProductViewModel>();


        // ⭐ THÊM DÒNG NÀY
        services.AddTransient<SettingsPage>();

        services.AddTransient<PromotionPage>();
        return services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        s_mainWindow = new MainWindow();

        var rootFrame = new Frame();
        s_mainWindow.Content = rootFrame;

        s_mainWindow.Activate();

        rootFrame.Loaded += (_, __) =>
        {
            try
            {
                var navService = Services.GetRequiredService<INavigationService>();
                navService.SetFrame(rootFrame);
                navService.NavigateTo(typeof(LoginPage));
            }
            catch (Exception ex)
            {
                File.WriteAllText("nav_error.log", ex.ToString());
                Environment.FailFast(ex.ToString());
            }
        };
    }


}

// Extension method to get services easily
public static class ServiceProviderExtensions
{
    public static T? GetService<T>(this IServiceProvider provider) where T : class
    {
        return provider.GetService(typeof(T)) as T;
    }
}