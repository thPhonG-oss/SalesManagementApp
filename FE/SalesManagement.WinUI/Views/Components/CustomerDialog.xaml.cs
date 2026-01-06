using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Models;

namespace SalesManagement.WinUI.Views.Components
{
    public sealed partial class CustomerDialog : ContentDialog
    {
        public Customer Customer { get; set; }

        public CustomerDialog(Customer customer, bool isEdit)
        {
            this.InitializeComponent();
            Customer = customer;
            Title = isEdit ? $"C?p nh?t khách hàng #{customer.CustomerId}" : "Thêm m?i khách hàng";
        }
    }
}