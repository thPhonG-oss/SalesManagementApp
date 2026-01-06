using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.ViewModels;

namespace SalesManagement.WinUI.Views
{
    public sealed partial class CustomerPage : Page
    {
        public CustomerViewModel ViewModel { get; }

        public CustomerPage()
        {
            this.InitializeComponent();
            this.Name = "PageRoot";

            ViewModel = App.Services.GetService<CustomerViewModel>();
            this.DataContext = ViewModel;
        }

        // ? THÊM HÀM NÀY Ð? G?I SEARCH
        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (ViewModel.SearchCommand.CanExecute(null))
            {
                ViewModel.SearchCommand.Execute(null);
            }
        }
    }
}