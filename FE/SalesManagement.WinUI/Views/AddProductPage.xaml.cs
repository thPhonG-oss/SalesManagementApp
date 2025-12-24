using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SalesManagement.WinUI.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AddProductPage : Page
{
    private readonly INavigationService _navigationService;
    public AddProductPage()
    {
        this.InitializeComponent();
        DataContext = App.Services.GetService<AddProductViewModel>();
        _navigationService = App.Services.GetRequiredService<INavigationService>();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        //var nav = App.Services.GetService<INavigationService>();
        //nav?.GoBack();
        _navigationService.GoBack();
    }

}
