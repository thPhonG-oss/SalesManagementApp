using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using SalesManagement.WinUI.Views.Components;
using System;
using System.Threading.Tasks;




namespace SalesManagement.WinUI.Views
{
    public sealed partial class OrderPage : Page
    {

        public OrderViewModel ViewModel { get; }

        public OrderPage()
        {
            ViewModel = App.Services.GetService<OrderViewModel>()!;
            this.InitializeComponent();

            var dialogService = App.Services.GetService<IDialogService>();

            this.Loaded += (s, e) =>
            {
                if (this.XamlRoot != null)
                {
                    dialogService?.SetXamlRoot(this.XamlRoot);
                }
            };

            // Quan trọng: Chỉ để DataContext nếu dùng Binding {Binding}
            // Nếu dùng {x:Bind} thì ViewModel property ở trên là đủ.
            this.DataContext = ViewModel;
        }



        // Event handlers cho các button Edit/Delete
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("EditButton_Click được gọi");
            
            // Ngăn chặn SelectionChanged
            OrderGrid.SelectionChanged -= OrderGrid_SelectionChanged;
            
            var button = sender as Button;
            if (button?.Tag is OrderItemViewModel item)
            {
                await ViewModel.EditOrderCommand.ExecuteAsync(item);
            }
            
            // Bật lại sau khi xong
            OrderGrid.SelectionChanged += OrderGrid_SelectionChanged;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DeleteButton_Click được gọi");
            
            // Ngăn chặn SelectionChanged
            OrderGrid.SelectionChanged -= OrderGrid_SelectionChanged;
            
            var button = sender as Button;
            if (button?.Tag is OrderItemViewModel item)
            {
                await ViewModel.DeleteOrderCommand.ExecuteAsync(item);
            }
            
            // Bật lại sau khi xong
            OrderGrid.SelectionChanged += OrderGrid_SelectionChanged;
        }

        private void OrderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Chỉ xử lý khi có item được chọn
            if (OrderGrid.SelectedItem is OrderItemViewModel selectedItem)
            {
                System.Diagnostics.Debug.WriteLine($"OrderGrid_SelectionChanged: {selectedItem.OrderId}");
                ViewModel.SelectedOrder = selectedItem;
            }
        }

        
    }
}