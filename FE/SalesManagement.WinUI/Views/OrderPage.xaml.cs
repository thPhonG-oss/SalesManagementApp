using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.ViewModels;





namespace SalesManagement.WinUI.Views
{
    public sealed partial class OrderPage : Page
    {
        
        public OrderViewModel ViewModel { get; }

        private bool _isDialogOpen = false;

        public OrderPage()
        {
            ViewModel = ViewModel = App.Services.GetService<OrderViewModel>()!;
            this.InitializeComponent();


           


            ViewModel.OpenDetailRequested += OnOpenDetailRequested;

           
            this.DataContext = ViewModel;
        }

       
        private async void OnOpenDetailRequested(OrderItemViewModel orderVM)
        {
            
            if (_isDialogOpen) return;

            
            _isDialogOpen = true;

            try
            {
                var dialog = new OrderDetailDialog(orderVM);

                if (this.Content != null && this.Content.XamlRoot != null)
                {
                    dialog.XamlRoot = this.Content.XamlRoot;
                }
                else
                {
                    dialog.XamlRoot = this.XamlRoot;
                }

                
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Lỗi Dialog: {ex.Message}");
            }
            finally
            {
                
                _isDialogOpen = false;
            }
        }
    }
}