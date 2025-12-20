using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Services;

using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;

namespace SalesManagement.WinUI.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }


    public MainPage()
    {
     
        ViewModel = App.Services.GetService<MainViewModel>()!;


        this.InitializeComponent();

       
        var navService = App.Services.GetService<INavigationService>() as NavigationService;
        navService?.SetFrame(ContentFrame);
        navService?.SetNavView(NavView);

        
    }

    private void NavView_Loaded(object sender, RoutedEventArgs e)
    {

        if (this.XamlRoot != null)
        {
            var loadingService = App.Services.GetService<ILoadingService>();
            loadingService?.SetXamlRoot(this.XamlRoot);
        }


        if (ViewModel.SelectedMenuItem != null && ViewModel.SelectedMenuItem.PageType != null)
        {
            ContentFrame.Navigate(ViewModel.SelectedMenuItem.PageType);
            NavView.Header = ViewModel.SelectedMenuItem.Title;
        }
    }


}