using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SalesManagement.WinUI.Services;
using SalesManagement.WinUI.Services.Implementations;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using SalesManagement.WinUI.Views;
using System;
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
            var baseUrl = config["ApiSettings:BaseUrl"] ?? "http://localhost:8080";

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new System.Net.CookieContainer()
        });

        // Services
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IStorageService, StorageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ILoadingService, LoadingService>();
        services.AddSingleton<IOrderService, MockOrderService>();
        services.AddSingleton<IDialogService, DialogService>();

        // ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<OrderViewModel>();

        // Views
        services.AddTransient<LoginPage>();
        services.AddTransient<MainPage>();

        return services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        s_mainWindow = new MainWindow();

        // Create root frame
        var rootFrame = new Microsoft.UI.Xaml.Controls.Frame();
        s_mainWindow.Content = rootFrame;

        // Navigate to login page
        rootFrame.Navigate(typeof(MainPage));

        s_mainWindow.Activate();
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