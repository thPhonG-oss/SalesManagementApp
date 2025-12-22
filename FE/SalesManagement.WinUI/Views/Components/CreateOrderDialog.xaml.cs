using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SalesManagement.WinUI.Views.Components
{
    public sealed partial class CreateOrderDialog : ContentDialog
    {
        public CreateOrderViewModel ViewModel { get; }

        public CreateOrderDialog()
        {
            this.InitializeComponent();
            // L?y Service th? công ho?c truy?n vào
            var service = App.Services.GetService<IOrderService>();
            ViewModel = new CreateOrderViewModel(service);
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            args.Cancel = true;

            try
            {
                bool success = await ViewModel.CreateOrderAsync();
                if (success)
                {
                    args.Cancel = false; // ?óng dialog thành công
                }
                else
                {
                    // Có th? hi?n InfoBar l?i ? ?ây n?u mu?n
                }
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
