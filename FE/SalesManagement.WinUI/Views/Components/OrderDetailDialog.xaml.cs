using Microsoft.UI.Xaml.Controls; // Quan tr?ng: ContentDialog n?m ? ?ây
using SalesManagement.WinUI.ViewModels;

namespace SalesManagement.WinUI.Views
{
   
    public sealed partial class OrderDetailDialog : ContentDialog
    {
        public OrderItemViewModel OrderVM { get; }

        public OrderDetailDialog(OrderItemViewModel orderVM)
        {
            this.InitializeComponent();
            OrderVM = orderVM;
        }
    }
}